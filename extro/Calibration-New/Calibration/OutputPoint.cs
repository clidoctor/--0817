using ChoiceTech.Halcon.Control;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Calibration
{
    public class OutputPoint
    {
        public CLGlobal Global = new CLGlobal();
        public  void GetLcPoint(HObject img, List<RoiParam> roiList,out List<HTuple> rowList, out List<HTuple> colList, out List<HTuple> Coord, HWindow_Final hWindow_Final1)
        {
               
            
            HTuple ImgWidth, ImgHeight;
            HOperatorSet.GetImageSize(img, out ImgWidth, out ImgHeight);
            HOperatorSet.CreateMetrologyModel(out Global.metrologyHandle);
            HOperatorSet.SetMetrologyModelImageSize(Global.metrologyHandle, ImgWidth, ImgHeight);
            //List<RoiParam> roiList = roiparamList;
            
            rowList = new List<HTuple>();colList = new List<HTuple>();Coord = new List<HTuple>();
            for (int i = 0; i < roiList.Count; i++)
            {
                HTuple row = new HTuple(), col = new HTuple(),coord = new HTuple();
                if (roiList[i].roi_name.Contains("直线"))
                {
                    GetMetrologyRect(img, roiList[i], out row, out col,out coord,  hWindow_Final1);
                }
                else if (roiList[i].roi_name.Contains("圆弧"))
                {
                    GetMetrologyCircle(img, roiList[i], out row, out col,out coord, hWindow_Final1);
                }
                else if (roiList[i].roi_name.Contains("定线"))
                {
                    GetMetrologyFixline(roiList[i], out row, out col, hWindow_Final1);
                }
                else if (roiList[i].roi_name.Contains("定弧"))
                {
                    GetMetrologyFixCircle(roiList[i], out row, out col, hWindow_Final1);
                }
                rowList.Add(row);
                colList.Add(col);
                Coord.Add(coord);
            }

            HOperatorSet.ClearMetrologyObject(Global.metrologyHandle, "all");
        }

        internal  void GetMetrologyRect(HObject img, RoiParam roi,out HTuple row,out HTuple col,out HTuple LineCoord, HWindow_Final hWindow_Final1)
        {

            HTuple ImgWidth, ImgHeight;
            HOperatorSet.GetImageSize(img, out ImgWidth, out ImgHeight);
            HOperatorSet.CreateMetrologyModel(out Global.metrologyHandle);

            row = new HTuple();
            col = new HTuple();
            LineCoord = new HTuple();
            if (roi == null || img == null)
            {
                return;
            }
            try
            {
                HTuple index;

                //限制
                HTuple distance1;
                HOperatorSet.DistancePp(roi.roi_row_start, roi.roi_col_start, roi.roi_row_end, roi.roi_col_end, out distance1);
             
                if (distance1< roi.meterbox_width* roi.meterbox_num)
                {
                    roi.meterbox_width = (int)( distance1.D / roi.meterbox_num);
                    if (roi.meterbox_width==0)
                    {
                        roi.meterbox_width = 1;
                    }
                }
                HOperatorSet.AddMetrologyObjectLineMeasure(Global.metrologyHandle, roi.roi_row_start,
                    roi.roi_col_start, roi.roi_row_end, roi.roi_col_end, roi.meterbox_height, roi.meterbox_width, 2, 10, new HTuple(),
                    new HTuple(), out index);

                

                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_transition", roi.edge_transition);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_select", roi.edge_select);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_sigma", roi.edge_sigma);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_threshold", roi.edge_threshold);
                //HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_length2", roi.meterbox_height);
                //HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_length1", roi.meterbox_width);

                HTuple distance;
                HOperatorSet.DistancePp(roi.roi_row_start, roi.roi_col_start, roi.roi_row_end, roi.roi_col_end, out distance);
                HTuple measure_distance = distance / ((roi.meterbox_num > 1 ? roi.meterbox_num : 2) - 1);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_distance", measure_distance);


                HOperatorSet.ApplyMetrologyModel(img, Global.metrologyHandle);
                HObject Contours;
                HTuple row1 = new HTuple(), col1 = new HTuple();
                HOperatorSet.GetMetrologyObjectMeasures(out Contours, Global.metrologyHandle, index, "all", out row1, out col1);

                //获取拟合后直线的点
                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all",
               "used_edges", "row", out row);
                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all",
                    "used_edges", "column", out col);
                HTuple row_begin, column_begin, row_end, column_end;
                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all", "result_type", "row_begin", out row_begin);
                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all", "result_type", "column_begin", out column_begin);
                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all", "result_type", "row_end", out row_end);
                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all", "result_type", "column_end", out column_end);
                LineCoord = LineCoord.TupleConcat(row_begin).TupleConcat(column_begin).TupleConcat(row_end).TupleConcat(column_end);
                if (row.TupleLength() > 0)
                {
                    row = row - roi.yOffset;
                    col = col - roi.xOffset;
                }

                if (hWindow_Final1 != null)
                {
                    HOperatorSet.SetColor(hWindow_Final1.hWindowControl.HalconWindow, "red");
                    HOperatorSet.DispObj(Contours, hWindow_Final1.hWindowControl.HalconWindow);

                }
                Contours.Dispose();
                if (LineCoord.TupleLength() > 1)
                {

                    HObject cross;
                    HOperatorSet.GenCrossContourXld(out cross, row, col, 30, 0);
                    //HOperatorSet.GenCircle(out cross, row, col, 10);
                    HObject contour, fitcontour;
                    //HOperatorSet.GenContourPolygonXld(out contour, row, col);
                    //HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
                    //HOperatorSet.FitLineContourXld(contour, "tukey", -1, 0, 5, 2,
                    //   out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
                    //HOperatorSet.GenRegionLine(out fitcontour, LineCoord[0], LineCoord[1], LineCoord[2], LineCoord[3]);
                    HOperatorSet.GenContourPolygonXld(out fitcontour, (new HTuple(LineCoord[0].D)).TupleConcat(LineCoord[2]), (new HTuple(LineCoord[1].D)).TupleConcat(LineCoord[3]));
                    if (hWindow_Final1 != null)
                    {
                        HOperatorSet.SetColor(hWindow_Final1.hWindowControl.HalconWindow, "red");
                        HOperatorSet.SetLineWidth(hWindow_Final1.hWindowControl.HalconWindow, 3);
                        HOperatorSet.DispObj(fitcontour, hWindow_Final1.hWindowControl.HalconWindow);
                        HOperatorSet.SetLineWidth(hWindow_Final1.hWindowControl.HalconWindow, 1);

                        HOperatorSet.SetDraw(hWindow_Final1.hWindowControl.HalconWindow, "fill");
                        HOperatorSet.DispObj(cross, hWindow_Final1.hWindowControl.HalconWindow);
                        HOperatorSet.SetDraw(hWindow_Final1.hWindowControl.HalconWindow, "margin");

                       
                    }

                    cross.Dispose();
                    fitcontour.Dispose();
                   
                }
                HOperatorSet.ClearMetrologyObject(Global.metrologyHandle, index);
            }
            catch (Exception ex)
            {
                CLGlobal.DispMsg(ex.Message);
            }
            
        }

        internal  void GetMetrologyCircle(HObject img, RoiParam roi, out HTuple row, out HTuple col,out HTuple CircleCoord, HWindow_Final hWindow_Final1)
        {

            HTuple ImgWidth, ImgHeight;
            HOperatorSet.GetImageSize(img, out ImgWidth, out ImgHeight);
            HOperatorSet.CreateMetrologyModel(out Global.metrologyHandle);

            row = new HTuple();
            col = new HTuple();
            CircleCoord = new HTuple();
            if (roi == null || img == null)
            {
                return;
            }
            try
            {
                HTuple circleRow = roi.roi_row_start;
                HTuple circleCol = roi.roi_col_start;
                HTuple circleRadius = (int)roi.roi_row_end;
                HTuple index;
                HTuple start_phi = new HTuple(roi.circle_startAng).TupleRad();
                HTuple end_phi = new HTuple(roi.circle_endAng).TupleRad();


                HOperatorSet.AddMetrologyObjectGeneric(Global.metrologyHandle, "circle", circleRow.TupleConcat(circleCol).TupleConcat(circleRadius),
                    100, 20, 1, 10, new HTuple("start_phi", "end_phi"), start_phi.TupleConcat(end_phi), out index);

                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_transition", roi.edge_transition);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_select", roi.edge_select);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_sigma", roi.edge_sigma);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_threshold", roi.edge_threshold);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_length1", roi.meterbox_height);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_length2", roi.meterbox_width);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "num_measures",
                roi.meterbox_num);
                HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "min_score", 0.7);

                HObject arc;
                HOperatorSet.GenCircleContourXld(out arc, roi.roi_row_start, roi.roi_col_start, roi.roi_row_end, new HTuple(roi.circle_startAng).TupleRad(), new HTuple(roi.circle_endAng).TupleRad(), "positive", 1);
                HTuple arcRow, arcCol;
                HOperatorSet.GetContourXld(arc, out arcRow, out arcCol);

                //HTuple measure_distance = arcRow.TupleLength() / ((roi.meterbox_num > 1 ? roi.meterbox_num : 2) - 1);
                //HOperatorSet.SetMetrologyObjectParam(Global.metrologyHandle, index, "measure_distance", measure_distance);


                HOperatorSet.ApplyMetrologyModel(img, Global.metrologyHandle);
                HObject Contours;
                HTuple row1 = new HTuple(), col1 = new HTuple();
                HOperatorSet.GetMetrologyObjectMeasures(out Contours, Global.metrologyHandle, index, "all", out row1, out col1);

                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all",
                "used_edges", "row", out row);
                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all",
                    "used_edges", "column", out col);

                HTuple hv_Row, hv_Column, hv_Radius;
                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all", "result_type", "row", out hv_Row);
                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all", "result_type", "column", out hv_Column);
                HOperatorSet.GetMetrologyObjectResult(Global.metrologyHandle, index, "all", "result_type", "radius", out hv_Radius);
                CircleCoord = CircleCoord.TupleConcat(hv_Row).TupleConcat(hv_Column).TupleConcat(hv_Radius);
                if (row.TupleLength() > 0)
                {
                    row = row - roi.yOffset;
                    col = col - roi.xOffset;
                }
                if (hWindow_Final1 != null)
                {
                    HOperatorSet.SetColor(hWindow_Final1.hWindowControl.HalconWindow, "red");
                    HOperatorSet.DispObj(Contours, hWindow_Final1.hWindowControl.HalconWindow);
                }
                Contours.Dispose();

                if (row.TupleLength() > 2 )
                {
                    HObject cirContour, circle;
                    HOperatorSet.GenContourPolygonXld(out cirContour, row, col);
                    HTuple Row, Column, Radius, StartPhi, EndPhi, PointOrder;
                    HOperatorSet.FitCircleContourXld(cirContour, "algebraic", -1, 0, 0, 3, 2,
                       out Row, out Column, out Radius, out StartPhi, out EndPhi, out PointOrder);
                    HOperatorSet.GenCircleContourXld(out circle, Row, Column, Radius, StartPhi, EndPhi, PointOrder, 1);
                    Global.CircleCenterX = row.TupleString(".6");
                    Global.CircleCenterY = Column.TupleString(".6");
                    Global.CircleRadius = Radius.TupleString(".6");
                    if (hWindow_Final1 != null )
                    {
                        HOperatorSet.DispObj(circle, hWindow_Final1.hWindowControl.HalconWindow);
                    }
                    cirContour.Dispose();
                    circle.Dispose();
                }
                if (row.TupleLength() != 0)
                {
                    if (hWindow_Final1 != null)
                    {
                        HObject cross;
                        HOperatorSet.GenCrossContourXld(out cross, row, col, 35, 0.8);
                        //HOperatorSet.GenCircle(out cross, row, col, 10);

                        HOperatorSet.SetColor(hWindow_Final1.hWindowControl.HalconWindow, "red");
                        HOperatorSet.SetLineWidth(hWindow_Final1.hWindowControl.HalconWindow, 3);

                        if (hWindow_Final1 != null)
                        {
                            HOperatorSet.DispObj(cross, hWindow_Final1.hWindowControl.HalconWindow);
                        }
                        //if (!Global.IsShowCircle)
                        //{
                        //    HOperatorSet.DispPolygon(hWindow_Final1.hWindowControl.HalconWindow, row, col);
                        //}
                        HOperatorSet.SetColor(hWindow_Final1.hWindowControl.HalconWindow, "red");
                        HOperatorSet.SetLineWidth(hWindow_Final1.hWindowControl.HalconWindow, 1);


                        cross.Dispose();
                    }
                }
                arc.Dispose();
                HOperatorSet.ClearMetrologyObject(Global.metrologyHandle, index);
                if (row.TupleLength() > 1 && roi.circle_oriente == "顺时针")
                {
                    HOperatorSet.TupleInverse(row, out row);
                    HOperatorSet.TupleInverse(col, out col);
                }
            }
            catch (Exception ex)
            {
                CLGlobal.DispMsg(ex.Message);
            }
            
        }

        internal  void GetMetrologyFixline(RoiParam roi,out HTuple SelectedRows,out HTuple SelectedCols, HWindow_Final hWindow_Final1)
        {
            SelectedRows = new HTuple();
            SelectedCols = new HTuple();
            if (roi == null)
            {
                return;
            }
            HTuple distance;
            HOperatorSet.DistancePp(roi.roi_row_start, roi.roi_col_start, roi.roi_row_end, roi.roi_col_end, out distance);
            HTuple measure_distance = distance / roi.meterbox_num;


            HObject contour, fitcontour;
            HOperatorSet.GenContourPolygonXld(out contour, new HTuple(roi.roi_row_start).TupleConcat(new HTuple(roi.roi_row_end)), new HTuple(roi.roi_col_start).TupleConcat(roi.roi_col_end));
            HTuple RowBegin, ColBegin, RowEnd, ColEnd, Nr, Nc, Dist;
            HOperatorSet.FitLineContourXld(contour, "tukey", -1, 0, 5, 2,
                out RowBegin, out ColBegin, out RowEnd, out ColEnd, out Nr, out Nc, out Dist);
            HOperatorSet.GenRegionLine(out fitcontour, RowBegin, ColBegin, RowEnd, ColEnd);
            HTuple Rows, Cols;
            HOperatorSet.GetRegionPoints(fitcontour, out Rows, out Cols);
            for (int i = 0; i < roi.meterbox_num; i++)
            {
                SelectedRows = SelectedRows.TupleConcat(Rows.TupleSelect((int)(measure_distance.TupleRound() * i)));
                SelectedCols = SelectedCols.TupleConcat(Cols.TupleSelect((int)(measure_distance.TupleRound() * i)));
            }
            if (SelectedRows.TupleLength() > 0)
            {
                SelectedRows = SelectedRows - roi.yOffset;
                SelectedCols = SelectedCols - roi.xOffset;
            }
            if (hWindow_Final1!=null)
            {
                HOperatorSet.SetColor(hWindow_Final1.hWindowControl.HalconWindow, "red");
                HOperatorSet.SetLineWidth(hWindow_Final1.hWindowControl.HalconWindow, 3);
                HOperatorSet.DispObj(fitcontour, hWindow_Final1.hWindowControl.HalconWindow);
                HOperatorSet.SetLineWidth(hWindow_Final1.hWindowControl.HalconWindow, 1);
                HObject cross;
                HOperatorSet.GenCrossContourXld(out cross, SelectedRows, SelectedCols, 35, 0.8);
                HOperatorSet.DispObj(cross, hWindow_Final1.hWindowControl.HalconWindow);
                HOperatorSet.SetDraw(hWindow_Final1.hWindowControl.HalconWindow, "margin");
                cross.Dispose();
            }
            contour.Dispose(); fitcontour.Dispose();

        }

        internal  void GetMetrologyFixCircle(RoiParam roi,out HTuple SelectedRows,out HTuple SelectedCols, HWindow_Final hWindow_Final1)
        {
            SelectedRows = new HTuple();
            SelectedCols = new HTuple();
            if (roi == null)
            {
                return;
            }
            HTuple circleRow = roi.roi_row_start;
            HTuple circleCol = roi.roi_col_start;
            HTuple circleRadius = (int)roi.roi_row_end;

            Global.CircleCenterX = circleRow.TupleString(".6");
            Global.CircleCenterY = circleCol.TupleString(".6");
            Global.CircleRadius = circleRadius.TupleString(".6");

            HTuple start_phi = new HTuple(roi.circle_startAng).TupleRad();
            HTuple end_phi = new HTuple(roi.circle_endAng).TupleRad();



            HObject circle;
            HOperatorSet.GenCircleContourXld(out circle, circleRow, circleCol, circleRadius, start_phi, end_phi, "positive", 1);
            HTuple Rows, Cols;
            HOperatorSet.GetContourXld(circle, out Rows, out Cols);
            HTuple distance = Rows.TupleLength();

            HTuple measure_distance = (distance / roi.meterbox_num).TupleRound();


            for (int i = 0; i < roi.meterbox_num; i++)
            {
                SelectedRows = SelectedRows.TupleConcat(Rows.TupleSelect((int)(measure_distance.TupleRound() * i)));
                SelectedCols = SelectedCols.TupleConcat(Cols.TupleSelect((int)(measure_distance.TupleRound() * i)));
            }
            if (SelectedRows.TupleLength() > 0)
            {
                SelectedRows = SelectedRows - roi.yOffset;
                SelectedCols = SelectedCols - roi.xOffset;
            }
            if (hWindow_Final1!=null)
            {
                HObject cross;
                HOperatorSet.GenCrossContourXld(out cross, SelectedRows, SelectedCols, 35, 0.8);
                HOperatorSet.SetColor(hWindow_Final1.hWindowControl.HalconWindow, "red");
                HOperatorSet.DispObj(cross, hWindow_Final1.hWindowControl.HalconWindow);
                HOperatorSet.DispObj(circle, hWindow_Final1.hWindowControl.HalconWindow);
                cross.Dispose();
            }
            circle.Dispose();
            if (SelectedRows.TupleLength() > 1 && roi.circle_oriente == "顺时针")
            {
                HOperatorSet.TupleInverse(SelectedRows, out SelectedRows);
                HOperatorSet.TupleInverse(SelectedCols, out SelectedCols);
            }
        }

        #region 做垂线
      
        // Local procedures 
        internal  void GetVerticaLine(HTuple hv_Row1, HTuple hv_Col1,
            HTuple hv_Row2, HTuple hv_Col2,out HTuple hv_RowStart, out HTuple hv_ColStart, out HTuple hv_RowEnd, out HTuple hv_ColEnd)
        {


            HTuple hv_Phi = null, hv_RowM = null, hv_ColM = null;
            HTuple hv_LineLength = null;
            // Initialize local and output iconic variables 
            HOperatorSet.LineOrientation(hv_Row1, hv_Col1, hv_Row2, hv_Col2, out hv_Phi);

            //计算该直线的中点
            hv_RowM = (hv_Row1 + hv_Row2) / 2;
            hv_ColM = (hv_Col1 + hv_Col2) / 2;

            //********************生成垂线*************************
            //垂线长度
            hv_LineLength = 180;
            //起点
            hv_RowStart = hv_RowM - ((hv_Phi.TupleCos()) * hv_LineLength);
            hv_ColStart = hv_ColM - ((hv_Phi.TupleSin()) * hv_LineLength);
            //终点
            hv_RowEnd = hv_RowM + ((hv_Phi.TupleCos()) * hv_LineLength);
            hv_ColEnd = hv_ColM + ((hv_Phi.TupleSin()) * hv_LineLength);
           
        }
        #endregion
    }
}
