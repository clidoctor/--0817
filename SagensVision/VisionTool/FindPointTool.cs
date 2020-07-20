using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SagensSdk;
using System.IO;
using ViewWindow.Model;
using ChoiceTech.Halcon.Control;
using System.Collections;
using System.Diagnostics;
using HalconDotNet;

namespace SagensVision.VisionTool
{

    public class FindPointTool
    {
        /// <summary>
        ///保存的数据
        /// </summary>
        public static double[,] RCAll;

        public static double[][] RArray;
        public static double[][] CArray;

        public static double[][] Row;
        public static double[][] Phi;
        public List<ROI>[] roiList = new List<ROI>[4];
        public List<ROI>[] roiList2 = new List<ROI>[4];
        public FitProfileParam[] fParam = new FitProfileParam[4];
        public IntersetionCoord[] intersectCoordList = new IntersetionCoord[4];
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="FindType">找线方式 "Fix" or "FitLineSet"</param>
        /// <param name="IsRight">"左工位(false) or 右工位(true)"</param>
        /// <param name="ToolType">"工具类型 Calib  or GlueGuide"</param>
        /// <returns></returns>
        public string Init(string FindType, bool IsRight, string ToolType)
        {
            try
            {
                ParamPath.ToolType = ToolType;
                HWindow_Final hwnd = new HWindow_Final();
                string[] SideName = { "Side1", "Side2", "Side3", "Side4" };
                string initError = "OK";
                for (int i = 0; i < 4; i++)
                {
                    fParam[i] = new FitProfileParam();
                    roiList[i] = new List<ROI>();
                    roiList2[i] = new List<ROI>();
                    fParam[i].DicPointName = new List<string>();
                    //hwindow_final2.viewWindow.notDisplayRoi();
                }
                for (int i = 0; i < 4; i++)
                {
                    if (true)
                    {
                        ParamPath.ParaName ="Side" + (i + 1).ToString();
                    }
                    //ParamPath.ParaName = FindType + "_Side" + (i + 1).ToString();
                    ParamPath.IsRight = IsRight;
                    if (!Directory.Exists(ParamPath.ParamDir))
                    {
                        Directory.CreateDirectory(ParamPath.ParamDir);
                    }
                }

                if (FindType == "Fix")
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ParamPath.ParaName = FindType + "_Side" + (i + 1).ToString();
                        ParamPath.IsRight = IsRight;
                        if (File.Exists(ParamPath.ParamDir + SideName[i] + ".xml"))
                        {
                            fParam[i] = (FitProfileParam)StaticOperate.ReadXML(ParamPath.ParamDir + SideName[i] + ".xml", typeof(FitProfileParam));
                        }
                        else
                        {
                            initError = "定位参数加载失败";
                            continue;
                        }
                        if (File.Exists(ParamPath.ParamDir + SideName[i] + "_Section.roi"))
                        {
                            hwnd.viewWindow.loadROI(ParamPath.ParamDir + SideName[i] + "_Section.roi", out roiList[i]);
                            hwnd.viewWindow.notDisplayRoi();
                        }
                        else
                        {
                            initError = "截面设置Roi加载失败";
                            continue;
                        }
                        if (File.Exists(ParamPath.ParamDir + SideName[i] + "_Region.roi"))
                        {
                            hwnd.viewWindow.loadROI(ParamPath.ParamDir + SideName[i] + "_Region.roi", out roiList2[i]);
                            hwnd.viewWindow.notDisplayRoi();
                        }
                        else
                        {
                            initError = "感兴趣区域加载失败";
                            continue;
                        }

                        if (File.Exists(ParamPath.Path_Param))
                        {
                            intersectCoordList[i] = (IntersetionCoord)StaticOperate.ReadXML(ParamPath.Path_Param, typeof(IntersetionCoord));
                        }
                        else
                        {
                            initError = "定位设置参数.xml加载失败";
                            continue;
                        }
                    }
                }
                else
                {
                    //string Path = "";

                    //Path = IsRight ? MyGlobal.ConfigPath_Right : MyGlobal.ConfigPath_Left;

                    for (int i = 0; i < 4; i++)
                    {
                        ParamPath.ParaName = FindType + "_Side" + (i + 1).ToString();
                        ParamPath.IsRight = IsRight;
                        if (File.Exists(ParamPath.ParamDir + SideName[i] + ".xml"))
                        {
                            fParam[i] = (FitProfileParam)StaticOperate.ReadXML(ParamPath.ParamDir + SideName[i] + ".xml", typeof(FitProfileParam));
                        }
                        else
                        {
                            initError = "抓边参数加载失败";
                            continue;
                        }
                        if (File.Exists(ParamPath.ParamDir + SideName[i] + "_Section.roi"))
                        {
                            hwnd.viewWindow.loadROI(ParamPath.ParamDir + SideName[i] + "_Section.roi", out roiList[i]);
                            hwnd.viewWindow.notDisplayRoi();
                        }
                        else
                        {
                            initError = "截面设置Roi加载失败";
                            continue;
                        }
                        if (File.Exists(ParamPath.ParamDir + SideName[i] + "_Region.roi"))
                        {
                            hwnd.viewWindow.loadROI(ParamPath.ParamDir + SideName[i] + "_Region.roi", out roiList2[i]);
                            hwnd.viewWindow.notDisplayRoi();
                        }
                        else
                        {
                            initError = "感兴趣区域加载失败";
                            continue;
                        }

                        ParamPath.ParaName = "Fix" + "_Side" + (i + 1).ToString();
                        ParamPath.IsRight = IsRight;
                        if (File.Exists(ParamPath.Path_Param))
                        {
                            intersectCoordList[i] = (IntersetionCoord)StaticOperate.ReadXML(ParamPath.Path_Param, typeof(IntersetionCoord));
                        }
                        else
                        {
                            initError = "定位设置参数.xml加载失败";
                            continue;
                        }
                    }
                }
                return initError;
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "参数设置加载失败:" + ex.Message + RowNum;
            }

        }

        public void ShowProfile(int ProfileId, out HTuple row, out HTuple col, HWindow_Final hwind = null)
        {
            row = new HTuple(); col = new HTuple();
            try
            {
                //显示截取的轮廓
                if (hwind != null)
                {
                    hwind.viewWindow.ClearWindow();
                    HObject image = new HObject();
                    HOperatorSet.GenImageConst(out image, "byte", 1000, 1000);
                    hwind.HobjectToHimage(image);
                }

                if (RArray == null || RArray.GetLength(0) == 0)
                {
                    return;
                }

                HTuple row1 = -new HTuple(RArray[ProfileId]);
                HTuple col1 = new HTuple(CArray[ProfileId]);
                if (row1.Length == 0)
                {

                    return;
                }

                //分辨率 x 0.007--0.01    y 0.035 -- 0.05

                double xResolution = MyGlobal.globalConfig.dataContext.xResolution;
                double yResolution = MyGlobal.globalConfig.dataContext.yResolution;
                HTuple SeqC = new HTuple();
                //double s1 = Math.Abs(Math.Cos(Phi[ProfileId][4]));
                //double s2 = Math.Abs(Math.Sin(Phi[ProfileId][4]));

                double s1 = Math.Abs(Math.Cos(Phi[ProfileId][4]));
                double s2 = Math.Abs(Math.Sin(Phi[ProfileId][4]));

                double scale = 0;

                scale = xResolution * s1 + yResolution * s2;

                //if (s1 >= s2)
                //{
                //    scale = xResolution / s1;
                //}
                //else
                //{
                //    scale = yResolution * s2;
                //}

                //double scale = xResolution * Math.Cos(Phi[CurrentIndex - 1][4]) + yResolution * Math.Sin(Phi[CurrentIndex - 1][4]);
                scale = Math.Abs(scale);
                HOperatorSet.TupleGenSequence(scale, (row1.Length + 1) * scale, scale, out col1);
                col1 = col1 * 200;
                int len1 = row1.Length;
                col1 = col1.TupleSelectRange(0, len1 - 1);

                HTuple rowmin = row1.TupleMin();
                row = row1 - rowmin + 150;                
                col = col1;
                if (hwind != null)
                {
                    HObject contour = new HObject();
                    HOperatorSet.GenRegionPoints(out contour, row, col);
                    hwind.viewWindow.displayHobject(contour, "red", true);
                }


            }
            catch (Exception)
            {

                throw;
            }

        }

        public void FindFirstAnchor(int SideId, out HTuple Row, out HTuple Col, int CurrentIndex)
        {
            Row = new HTuple(); Col = new HTuple();
            try
            {
                int Id = SideId - 1;
                if (RArray == null)
                {
                    return;
                }
                HTuple row = new HTuple(RArray[CurrentIndex - 1]);
                HTuple col = new HTuple(CArray[CurrentIndex - 1]);

                ShowProfile(CurrentIndex - 1, out row, out col);

                if (RArray[CurrentIndex - 1].Length == 0)
                {
                    return;
                }
                //HTuple row1 = -row;
                //HTuple col1 = col;
                //row = row1 - 0 + 150;
                //col = col1;

                //除去 -30 *200 的点
                HTuple eq30_1 = row.TupleEqualElem(-6000 - 0 + 150);
                HTuple eqId_1 = eq30_1.TupleFind(1);
                HTuple temp_1 = row.TupleRemove(eqId_1);
                HTuple temp_2 = col.TupleRemove(eqId_1);

                //取最左
                HTuple maxZCol = true ? temp_2.TupleMin() : temp_2.TupleMax();
                //HTuple maxZCol = fParam[Id].BeLeft ? temp_2.TupleMin() : temp_2.TupleMax();
                HTuple mZRowId = col.TupleFindFirst(maxZCol);
                HTuple mZRow = row[mZRowId];
                Row = mZRow;
                Col = maxZCol;
            }
            catch (Exception ex)
            {

                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";

            }
        }

        /// <summary>
        /// 获取极大值或极小值
        /// </summary>
        /// <param name="rows">输入行坐标</param>
        /// <param name="cols">输入列坐标</param>
        /// <param name="sigma">输入平滑系数</param>
        /// <param name="IsUpDown">以上下方向筛选还是左右方向筛选</param>
        /// <param name="IsMaxOrMin">选择极大值还是极小值</param>
        /// <param name="hv_rowout">输出极值行坐标</param>
        /// <param name="hv_colout">输出极值列坐标</param>
        /// <param name="UseLeft"> 是否选择最左侧点</param>
        /// <returns></returns>
        public string PeakTroughOfWave(HTuple rows, HTuple cols, HTuple sigma, bool IsUpDown, bool IsMaxOrMin, out HTuple hv_rowout, out HTuple hv_colout, bool UseLeft = false)
        {
            try
            {
                hv_rowout = new HTuple(); hv_colout = new HTuple();
                HTuple hv_Function = null, hv_SmoothedFunction = null, hv_Derivative = null, hv_ZeroCrossings = null, hv_Y = null, hv_Min = null, hv_Max = null;
                HTuple hv_Indices1 = null, hv_Indices2 = null;

                if (rows.Type == HTupleType.EMPTY || rows.Length < 5)
                {
                    hv_rowout = new HTuple();
                    hv_colout = new HTuple();
                    return "NG";
                }
                HOperatorSet.CreateFunct1dArray(rows, out hv_Function);
                if (sigma.D==0)
                {
                    sigma = 0.5;
                }
                if (sigma > ((rows.Length - 2) / 7.8))
                {
                    sigma = 0.5;
                }

                HOperatorSet.SmoothFunct1dGauss(hv_Function, sigma, out hv_SmoothedFunction);
                HOperatorSet.DerivateFunct1d(hv_SmoothedFunction, "first", out hv_Derivative);
                HOperatorSet.ZeroCrossingsFunct1d(hv_Derivative, out hv_ZeroCrossings);
                HTuple hv_indCol = ((hv_ZeroCrossings.TupleString(".0f"))).TupleNumber();

                if (hv_indCol.Length == 0)
                {
                    //if (UseLeft)
                    //{
                    //    hv_colout = cols.TupleMin();
                    //    HTuple minId = cols.TupleFind(hv_colout);
                    //    hv_rowout = rows[minId];
                    //}
                    //else
                    //{
                    hv_colout = new HTuple();
                    hv_rowout = new HTuple();
                    //}                   

                    return "OK";
                }
                HOperatorSet.GetYValueFunct1d(hv_SmoothedFunction, hv_ZeroCrossings, "constant",
                    out hv_Y);
                HTuple max = new HTuple(); HTuple min = new HTuple();
                HTuple hv_indColMax = new HTuple(); HTuple hv_indColMin = new HTuple();
                //轮廓最左点
                HTuple colLeft = cols.TupleMin();

                if (true)
                {
                    for (int i = 0; i < hv_Y.Length; i++)
                    {
                        if (hv_indCol[i].I + 3 + 5 >= hv_SmoothedFunction.Length)
                        {
                            break;
                        }
                        if (hv_indCol[i].I < 2)
                        {
                            continue;
                        }
                        if (hv_SmoothedFunction[hv_indCol[i].I + 3] < hv_SmoothedFunction[hv_indCol[i].I + 3 - 5] && hv_SmoothedFunction[hv_indCol[i].I + 3] < hv_SmoothedFunction[hv_indCol[i].I + 3 + 5])
                        {

                            max = max.TupleConcat(hv_Y[i]);
                            hv_indColMax = hv_indColMax.TupleConcat(hv_indCol[i]);

                        }
                        if (hv_SmoothedFunction[hv_indCol[i].I + 3] > hv_SmoothedFunction[hv_indCol[i].I + 3 - 5] && hv_SmoothedFunction[hv_indCol[i].I + 3] > hv_SmoothedFunction[hv_indCol[i].I + 3 + 5])
                        {
                            if (cols[hv_indCol[i].I] > colLeft) //不取最左侧点
                            {
                                min = min.TupleConcat(hv_Y[i]);
                                hv_indColMin = hv_indColMin.TupleConcat(hv_indCol[i]);
                            }
                            else
                            {
                                if (UseLeft)
                                {
                                    min = min.TupleConcat(hv_Y[i]);
                                    hv_indColMin = hv_indColMin.TupleConcat(hv_indCol[i]);
                                }
                            }
                        }
                    }

                    if (IsMaxOrMin)//极大值
                    {
                        if (max.Length == 0)
                        {

                            return "PeakTroughOfWave：" + "No MaxValue";
                        }
                        hv_Y = max;
                        hv_indCol = hv_indColMax;
                    }
                    else
                    {
                        if (min.Length == 0)
                        {
                            return "PeakTroughOfWave：" + "No MinValue";
                        }
                        hv_Y = min;
                        hv_indCol = hv_indColMin;
                    }
                }


                if (IsUpDown)//上下方向 Row 方向最大和最小值
                {
                    HOperatorSet.TupleMin(hv_Y, out hv_Min);
                    HOperatorSet.TupleMax(hv_Y, out hv_Max);
                    HOperatorSet.TupleFindFirst(hv_Y, hv_Min, out hv_Indices1);
                    HOperatorSet.TupleFindFirst(hv_Y, hv_Max, out hv_Indices2);

                    hv_rowout = new HTuple();
                    hv_rowout = hv_rowout.TupleConcat(hv_Y.TupleSelect(
                        hv_Indices1));
                    hv_rowout = hv_rowout.TupleConcat(hv_Y.TupleSelect(hv_Indices2));
                    hv_colout = new HTuple();
                    hv_colout = hv_colout.TupleConcat(cols.TupleSelect(
                        hv_indCol.TupleSelect(hv_Indices1)));
                    hv_colout = hv_colout.TupleConcat(cols.TupleSelect(
                        hv_indCol.TupleSelect(hv_Indices2)));

                }
                else//左右方向 Col 方向 最大和最小值
                {
                    HTuple Col = cols[hv_indCol];
                    HOperatorSet.TupleMin(Col, out hv_Min);//最左侧
                    HOperatorSet.TupleMax(Col, out hv_Max);//最右侧
                    HOperatorSet.TupleFindFirst(Col, hv_Min, out hv_Indices1);
                    HOperatorSet.TupleFindFirst(Col, hv_Max, out hv_Indices2);


                    hv_rowout = new HTuple();
                    hv_rowout = hv_rowout.TupleConcat(hv_Y.TupleSelect(
                        hv_Indices1));
                    hv_rowout = hv_rowout.TupleConcat(hv_Y.TupleSelect(hv_Indices2));
                    hv_colout = new HTuple();
                    hv_colout = hv_colout.TupleConcat(hv_Min);
                    hv_colout = hv_colout.TupleConcat(hv_Max);
                }


                return "OK";
            }
            catch (Exception ex)
            {
                hv_colout = new HTuple(); hv_rowout = new HTuple();
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "PeakTroughOfWave：" + ex.Message + RowNum;
            }
        }

        /// <summary>
        /// 获取拐点
        /// </summary>
        /// <param name="clipRow">轮廓行坐标</param>
        /// <param name="clipCol">轮廓列坐标</param>
        /// <param name="Deg">轮廓旋转角度</param>
        /// <param name="pRow">拐点行坐标</param>
        /// <param name="pCol">拐点列坐标</param>
        /// <param name="Hwindow">显示窗口</param>
        /// <param name="isUpDown">以上下方向筛选还是以左右方向筛选</param>
        /// <param name="isMax">是否启用极大值(true 极大值，false 极小值）</param>
        /// <returns></returns>
        private string GetInflection(HTuple clipRow, HTuple clipCol, HTuple Deg, out HTuple pRow, out HTuple pCol,out HTuple RotateRow,out HTuple RotateCol,out HTuple HomMat,out HTuple rotateMaxR,out HTuple rotateMaxC, HWindow_Final Hwindow = null, bool isUpDown = true, bool isMax = true, bool useLeft = true, bool ifShowFeatures = false,bool UseZzoom=false,double Zzoom =1,double sigma = 0.5)
        {
            pRow = new HTuple(); pCol = new HTuple();
            RotateRow = new HTuple();RotateCol = new HTuple();HomMat = new HTuple();
            rotateMaxR = new HTuple();rotateMaxC = new HTuple();
            try
            {
                if (clipRow.Length == 0)
                {
                    return "No Clip";
                }
                if (!ifShowFeatures)
                {
                    Hwindow = null;
                }
                HObject IntersectionO = new HObject(); HObject crossO = new HObject();
                HObject ContourO = new HObject();
                HTuple RowO, ColO, rowPeak, colPeak;
                HTuple area, row, col, homMat2D;
                HTuple Phi = Deg.TupleRad();

                HOperatorSet.GenRegionPoints(out IntersectionO, clipRow, clipCol);

                HOperatorSet.GenContourPolygonXld(out ContourO, clipRow, clipCol);


                HOperatorSet.AreaCenter(IntersectionO, out area, out row, out col);
                HOperatorSet.VectorAngleToRigid(row, col, 0, row, col, Phi, out homMat2D);
                HOperatorSet.AffineTransContourXld(ContourO, out ContourO, homMat2D);
                if (sigma<3)
                {
                    sigma = 3;
                }
                HOperatorSet.SmoothContoursXld(ContourO, out ContourO, sigma);            
                HOperatorSet.GetContourXld(ContourO, out RowO, out ColO);
                
                if (UseZzoom)
                {
                    if (Zzoom==0)
                    {
                        Zzoom = 1;
                    }
                    RowO = RowO * Zzoom;
                }
                RotateRow = RowO; RotateCol = ColO;

                if (Hwindow != null)
                {
                    HOperatorSet.SetDraw(Hwindow.HWindowHalconID, "margin");
                    HOperatorSet.GenContourPolygonXld(out ContourO, RowO, ColO);
                    Hwindow.viewWindow.displayHobject(ContourO, "white", true);
                    HOperatorSet.SetDraw(Hwindow.HWindowHalconID, "margin");
                }
      
                string ware = PeakTroughOfWave(RowO, ColO, sigma, isUpDown, isMax, out rowPeak, out colPeak, useLeft);
                string msg = "";
                //if (ware=="NG")
                //{
                //    msg = "轮廓点异常";
                //    return msg;
                //}
                if (rowPeak.Length == 0)
                {
                    pRow = new HTuple(); pCol = new HTuple();
                    return "OK";
                    msg = "OK";
                    if (true)
                    {
                        colPeak = ColO.TupleMin();
                        HTuple ind = ColO.TupleFindFirst(colPeak);
                        rowPeak = RowO[ind];
                        rowPeak = rowPeak.TupleConcat(rowPeak);
                        colPeak = colPeak.TupleConcat(colPeak);

                    }
                    //else
                    //{
                    //    colPeak = ColO.TupleMax();
                    //    HTuple ind = ColO.TupleFindFirst(colPeak);
                    //    rowPeak = RowO[ind];
                    //    rowPeak = rowPeak.TupleConcat(rowPeak);
                    //    colPeak = colPeak.TupleConcat(colPeak);
                    //}

                }
                else
                {
                    msg = "OK";
                }

                HTuple Max = rowPeak.TupleMin();
                HTuple maxId = rowPeak.TupleFindFirst(Max);

                colPeak = colPeak[maxId];
                rowPeak = Max;
                rotateMaxR = rowPeak; rotateMaxC = colPeak;
                if (UseZzoom)
                {
                    if (Zzoom == 0)
                    {
                        Zzoom = 1;
                    }
                    rowPeak = rowPeak / Zzoom;
                }
                
                HOperatorSet.VectorAngleToRigid(row, col, Phi, row, col, 0, out homMat2D);
                HOperatorSet.AffineTransPoint2d(homMat2D, rowPeak, colPeak, out pRow, out pCol);
                HomMat = homMat2D;
                if (Hwindow != null)
                {
                    HOperatorSet.GenCrossContourXld(out crossO, pRow, pCol, 12, 0);
                    HOperatorSet.SetDraw(Hwindow.HWindowHalconID, "margin");

                    Hwindow.viewWindow.displayHobject(crossO, "red", true);
                    Hwindow.viewWindow.displayHobject(IntersectionO, "yellow", true);
                    HOperatorSet.SetDraw(Hwindow.HWindowHalconID, "margin");
                }


                return msg;
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "GetInflection error " + ex.Message + RowNum;
            }
        }

        /// <summary>
        /// 由轮廓拟合点
        /// </summary>
        /// <param name="SideId">边序号 从1 开始</param>
        /// <param name="LastR">输出行坐标</param>
        /// <param name="LastC">输出列坐标</param>
        /// <param name="hwnd_profile">输入窗体（可选）</param>
        /// <returns></returns>
        public string FindMaxPtFallDown(int SideId, int ProfileId, out HTuple LastR, out HTuple LastC, out HTuple AnchorR, out HTuple AnchorC, HWindow_Final hwnd_profile = null, bool ShowFeatures = false, bool UseFit = false, HWindow_Final hwnd = null)
        {
            LastR = new HTuple(); LastC = new HTuple(); AnchorR = new HTuple(); AnchorC = new HTuple();
            try
            {
                int Id = SideId - 1;
                int roiID = -1;
                for (int i = 0; i < fParam[Id].roiP.Count; i++)
                {

                    for (int j = 0; j < fParam[Id].roiP[i].NumOfSection; j++)
                    {
                        roiID++;
                        if (roiID == ProfileId)
                        {
                            break;
                        }
                    }
                    if (roiID == ProfileId)
                    {
                        roiID = i;
                        break;
                    }
                }

                HTuple row, col;
                ShowProfile(ProfileId, out row, out col, hwnd_profile);

                HTuple RotateRow = new HTuple();HTuple RotateCol = new HTuple();HTuple HomMat = new HTuple();
                HTuple rotateMaxR = new HTuple();HTuple rotateMaxC = new HTuple();
                if (RArray == null || RArray[ProfileId].Length == 0)
                {
                    return "RArray is NULL";
                }

                //除去 -30 *200 的点
                HTuple eq30_1 = row.TupleEqualElem(-6000 - 0 + 150);
                HTuple eqId_1 = eq30_1.TupleFind(1);
                HTuple temp_1 = row.TupleRemove(eqId_1);
                HTuple temp_2 = col.TupleRemove(eqId_1);

                if (roiList[Id].Count == 0)
                {
                    return "roiList is Null";
                }

                //取最左 作为初步锚定点
                HTuple maxZCol = true ? temp_2.TupleMin() : temp_2.TupleMax();

                //HTuple maxZCol = fParam[Id].BeLeft ? temp_2.TupleMin() : temp_2.TupleMax();
                HTuple mZRowId = col.TupleFindFirst(maxZCol);
                HTuple mZRow = row[mZRowId];


                
                // 锚定 Roi
                HTuple RoiCoord = roiList[Id][roiID].getModelData();
                double R1 = RoiCoord[0] + mZRow.D - fParam[Id].roiP[roiID].AnchorRow;
                double C1 = RoiCoord[1] + maxZCol.D - fParam[Id].roiP[roiID].AnchorCol;
                double R2 = RoiCoord[2] + mZRow.D - fParam[Id].roiP[roiID].AnchorRow;
                double C2 = RoiCoord[3] + maxZCol.D - fParam[Id].roiP[roiID].AnchorCol;
                if (C1 < 0)
                {
                    C1 = 0;
                }

                //生成矩形框
                HObject left = new HObject();
                HObject right = new HObject();
                HOperatorSet.GenRegionLine(out left, 0, C1, 1000, C1);
                HOperatorSet.GenRegionLine(out right, 0, C2, 1000, C2);
                if (hwnd_profile != null && ShowFeatures)
                {
                    hwnd_profile.viewWindow.displayHobject(left, "green");
                    hwnd_profile.viewWindow.displayHobject(right, "green");

                }


                //求矩形区域内轮廓 极值
                HTuple ColLess = temp_2.TupleLessElem(C2);
                HTuple ColGreater = temp_2.TupleGreaterElem(C1);
                HTuple sub = ColLess.TupleSub(ColGreater);
                HTuple IntersetID = sub.TupleFind(0);
                //HTuple IntersetID = eq0.TupleFind(1);
                if (IntersetID == -1)
                {
                    return "区域内 无有效点";
                }
                HTuple RowNew = temp_1[IntersetID];
                HTuple ColNew = temp_2[IntersetID];
                double Zzoom = 0;
                HTuple maxZ = new HTuple(); HTuple mZCol = new HTuple();
                HObject Profile = new HObject();
                //最高点下降
                if (fParam[Id].roiP[roiID].SelectedType == 1)
                {
                    //取最上
                    maxZ = RowNew.TupleMin();
                    HTuple mZColId = RowNew.TupleFindFirst(maxZ);
                    mZCol = ColNew[mZColId];
                    RotateRow = row;
                    RotateCol = col;
                    HOperatorSet.GenContourPolygonXld(out Profile, RotateRow, RotateCol);
                    if (hwnd_profile != null && ShowFeatures)
                    {
                        hwnd_profile.viewWindow.displayHobject(Profile, "white");
                    }
                }
                else
                {
                    //取极值
                    int deg = fParam[Id].roiP[roiID].AngleOfProfile;

                    bool ShowFeatures1 = fParam[Id].roiP[roiID].useZzoom  ? false : ShowFeatures;                    
                    GetInflection(RowNew, ColNew, deg, out maxZ, out mZCol,out RotateRow,out RotateCol,out HomMat,out rotateMaxR,out rotateMaxC, hwnd_profile, true, true, true, ShowFeatures1
                        , fParam[Id].roiP[roiID].useZzoom, fParam[Id].roiP[roiID].ClippingPer, fParam[Id].roiP[roiID].Sigma);

                    
                    if (fParam[Id].roiP[roiID].useZzoom)
                    {
                       
                        if (fParam[Id].roiP[roiID].ClippingPer == 0)
                        {
                            Zzoom = 1;
                        }
                        else
                        {
                            Zzoom = fParam[Id].roiP[roiID].ClippingPer;
                        }
                       
                    }
                    else
                    {
                        RotateRow = row;
                        RotateCol = col;
                    }
                   
                    HOperatorSet.GenContourPolygonXld(out Profile, RotateRow, RotateCol);
                    if (hwnd_profile != null && ShowFeatures && fParam[Id].roiP[roiID].useZzoom)
                    {
                        hwnd_profile.viewWindow.displayHobject(Profile, "white");
                    }
                    if (maxZ.Length == 0)
                    {
                        return "Ignore";
                    }
                    HTuple Max1 = fParam[Id].roiP[roiID].useZzoom ? rotateMaxR : maxZ.TupleMax();
                    HTuple maxId1 = maxZ.TupleFindFirst(Max1);

                    mZCol = fParam[Id].roiP[roiID].useZzoom ? rotateMaxC.D : mZCol[maxId1].D;
                    maxZ = Max1;

                    if (hwnd_profile != null && ShowFeatures)
                    {
                        HObject cross2 = new HObject();
                        HOperatorSet.GenCrossContourXld(out cross2, maxZ, mZCol, 10, 0);
                        hwnd_profile.viewWindow.displayHobject(cross2, "blue");

                    }

                    if (maxZ.Length == 0)
                    {
                        return "Ignore";

                        //return "FindMaxPt fail";
                    }
                }





                double xresolution = MyGlobal.globalConfig.dataContext.xResolution;
                HTuple rowStart = maxZ.D + fParam[Id].roiP[roiID].TopDownDist * 200;
                HTuple rowEnd = rowStart;
                HTuple colStart = mZCol;
                double dist = fParam[Id].roiP[roiID].xDist == 0 ? 300 : fParam[Id].roiP[roiID].xDist * 200;
                HTuple colEnd = new HTuple();
                if (!fParam[Id].roiP[roiID].useCenter)
                {
                    colEnd = fParam[Id].roiP[roiID].useLeft ? mZCol - dist : mZCol + dist;
                }
                else
                {
                    //取轮廓中心
                    colStart = mZCol - dist;
                    colEnd = mZCol + dist;
                }


                //求交点
                HObject Line = new HObject();  HObject Intersect = new HObject();
                HOperatorSet.GenContourPolygonXld(out Line, rowStart.TupleConcat(rowEnd), colStart.TupleConcat(colEnd));

           
               
                HTuple IntersecR, intersecC, isOver;
                HOperatorSet.IntersectionContoursXld(Profile, Line, "mutual", out IntersecR, out intersecC, out isOver);
                if (hwnd_profile != null && ShowFeatures)
                {
                    //hwnd_profile.viewWindow.displayHobject(Profile, "white");
                    hwnd_profile.viewWindow.displayHobject(Line, "green");

                }
                if (IntersecR.Length == 0)
                {
                    return "Ignore";

                }
                if (IntersecR.Length > 1)
                {
                    //取最上
                    //取距离 极值或最高点 最近的点/最远点
                    HTuple distMin = new HTuple();

                    HTuple minC = new HTuple(); HTuple minCid = new HTuple();
                    if (!fParam[Id].roiP[roiID].useCenter)
                    {
                        for (int i = 0; i < intersecC.Length; i++)
                        {
                            double innerDis = Math.Abs(intersecC[i].D - mZCol.D);
                            distMin = distMin.TupleConcat(innerDis);
                        }
                        HTuple minDist = fParam[Id].roiP[roiID].useNear ? distMin.TupleMin() : distMin.TupleMax();
                        HTuple minId = distMin.TupleFindFirst(minDist);
                        minC = intersecC[minId];
                        minCid = intersecC.TupleFindFirst(minC);
                        LastR = LastR.TupleConcat(IntersecR[minCid]);
                        LastC = LastC.TupleConcat(minC);
                    }
                    else
                    {
                        ////取轮廓中心
                        //取离最大值或极值最近的两个点
                        HTuple distless = new HTuple(); HTuple distgreater = new HTuple();
                        for (int i = 0; i < intersecC.Length; i++)
                        {
                            double innerDis = intersecC[i].D - mZCol.D;
                            if (innerDis < 0)
                            {
                                distless = distless.TupleConcat(innerDis);
                            }
                            else
                            {
                                distgreater = distgreater.TupleConcat(innerDis);
                            }
                        }
                        HTuple min1 = distless.TupleMax();
                        HTuple min2 = distgreater.TupleMin();
                        HTuple minC1 = mZCol.D + min1; HTuple minC2 = mZCol.D + min2;
                        //取 区间 minCId1 -- minCId2
                        HTuple MinMaxCol = RotateCol.TupleGreaterEqualElem(minC1);
                        HTuple MinMaxColID = MinMaxCol.TupleFind(1);
                        HTuple tempcol = RotateCol[MinMaxColID];
                        HTuple temprow = RotateRow[MinMaxColID];
                        HTuple MinMaxCol2 = tempcol.TupleLessEqualElem(minC2);
                        HTuple MinMaxColID2 = MinMaxCol2.TupleFind(1);
                        HTuple SegCol = tempcol[MinMaxColID2];
                        HTuple SegRow = temprow[MinMaxColID2];

                        HTuple centerR = SegRow.TupleMean();
                        HTuple centerC = SegCol.TupleMean();
                        LastR = LastR.TupleConcat(centerR);
                        LastC = LastC.TupleConcat(centerC);
                    }


                }
                else
                {
                    //取最上
                    LastR = LastR.TupleConcat(IntersecR);
                    LastC = LastC.TupleConcat(intersecC);

                    //取最左

                }
                HObject cross = new HObject();
                HOperatorSet.GenCrossContourXld(out cross, IntersecR, intersecC, 6, 0);
                if (hwnd_profile != null && ProfileId == 1)
                {
                    hwnd_profile.viewWindow.displayHobject(cross, "white");
                }


                if (LastR.Length == 0)
                {
                    return "Ignore";
                }
                HTuple Max = LastR.TupleMax();
                HTuple maxId = LastR.TupleFindFirst(Max);

                LastC = LastC[maxId];
                LastR = Max;
                AnchorR = LastR;
                AnchorC = LastC;


                ////启用中间点
                //if (fParam[Id].roiP[roiID].useMidPt)
                //{
                //    HTuple EdgeCol = fParam[Id].roiP[roiID].useLeft ? temp_2.TupleMin() : temp_2.TupleMax();
                //    HTuple EdgColId = temp_2.TupleFindFirst(EdgeCol);
                //    HTuple EdgegRow = temp_1[EdgColId];
                //    //取中间点
                //    HTuple midR = (LastR + EdgegRow) / 2;
                //    HTuple midC = (LastC + EdgeCol) / 2;
                //    LastR = midR;
                //    LastC = midC;
                //}


                if (hwnd_profile != null && ShowFeatures)
                {
                    HObject cross1 = new HObject();
                    HOperatorSet.GenCrossContourXld(out cross1, LastR, LastC, 10, 0);
                    hwnd_profile.viewWindow.displayHobject(cross1, "green");

                }

                if (fParam[Id].roiP[roiID].SelectedType == 0 && fParam[Id].roiP[roiID].useZzoom)
                {
                    LastR = LastR / Zzoom;
                    HOperatorSet.AffineTransPoint2d(HomMat, LastR, LastC, out LastR, out LastC);
                }

                if (LastR.Length == 0)
                {
                    return "Ignore";

                    //return "FindMaxPt fail";
                }
                //HOperatorSet.TupleGenSequence(0, LastR.Length - 1, 1, out LastR);
                HTuple PtID = new HTuple();
                HOperatorSet.TupleGreaterEqualElem(col, LastC, out PtID);
                PtID = PtID.TupleFindFirst(1);
                LastR = Row[ProfileId][PtID];
                LastC = CArray[ProfileId][PtID];
                //LastR = ProfileId;
                //if (MyGlobal.globalConfig.dataContext.xResolution != 0)
                //{
                //    LastC = LastC / 200 / MyGlobal.globalConfig.dataContext.xResolution;
                //}
                //else
                //{
                //    LastC = LastC / 200 / 0.007;
                //}
               

                if (hwnd != null)
                {
                    HObject cross1 = new HObject();
                    HOperatorSet.GenCrossContourXld(out cross1, LastR, LastC, 10, 0);
                    HOperatorSet.SetColor(hwnd.HWindowHalconID, "red");
                    HOperatorSet.DispObj(cross1, hwnd.HWindowHalconID);

                }


                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "FindMaxPt error" + ex.Message + RowNum;
            }

        }

        /// <summary>
        /// 由轮廓拟合点
        /// </summary>
        /// <param name="SideId">边序号 从1 开始</param>
        /// <param name="LastR">输出行坐标</param>
        /// <param name="LastC">输出列坐标</param>
        /// <param name="hwnd">输入窗体（可选）</param>
        /// <returns></returns>
        public string FindMaxPt(int SideId, int ProfileId, out HTuple LastR, out HTuple LastC, out HTuple AnchorR, out HTuple AnchorC, HWindow_Final hwnd = null, bool ShowFeatures = false, bool UseFit = false, HWindow_Final hwnd_profile = null)
        {
            LastR = new HTuple(); LastC = new HTuple(); AnchorR = new HTuple(); AnchorC = new HTuple();
            try
            {
                int Id = SideId - 1;
                int roiID = -1;
                for (int i = 0; i < fParam[Id].roiP.Count; i++)
                {

                    for (int j = 0; j < fParam[Id].roiP[i].NumOfSection; j++)
                    {
                        roiID++;
                        if (roiID == ProfileId)
                        {
                            break;
                        }
                    }
                    if (roiID == ProfileId)
                    {
                        roiID = i;
                        break;
                    }
                }

                HTuple row, col;
                ShowProfile(ProfileId, out row, out col, hwnd_profile);

                HTuple RotateRow = new HTuple(); HTuple RotateCol = new HTuple(); HTuple HomMat = new HTuple();
                HTuple rotateMaxR = new HTuple();HTuple rotateMaxC = new HTuple();
                if (RArray == null || RArray[ProfileId].Length == 0)
                {
                    return "RArray is NULL";
                }
                //HTuple row1 = -row;
                //HTuple col1 = col;

                //row = row1 - 0 + 150;
                //col = col1;


                //除去 -30 *200 的点
                HTuple eq30_1 = row.TupleEqualElem(-6000 - 0 + 150);
                HTuple eqId_1 = eq30_1.TupleFind(1);
                HTuple temp_1 = row.TupleRemove(eqId_1);
                HTuple temp_2 = col.TupleRemove(eqId_1);

                if (roiList[Id].Count == 0)
                {
                    return "roiList is Null";
                }

                ////取最左 作为初步锚定点
                //HTuple maxZCol =  temp_2.TupleMin();

                //HTuple mZRowId = col.TupleFindFirst(maxZCol);
                //HTuple mZRow = row[mZRowId];
                HTuple mZRow = 0; HTuple maxZCol = 0;

                
                // 锚定 Roi
                HTuple RoiCoord = roiList[Id][roiID].getModelData();
                double R1 = RoiCoord[0] + mZRow.D - fParam[Id].roiP[roiID].AnchorRow;
                double C1 = RoiCoord[1] + maxZCol.D - fParam[Id].roiP[roiID].AnchorCol;
                double R2 = RoiCoord[2] + mZRow.D - fParam[Id].roiP[roiID].AnchorRow;
                double C2 = RoiCoord[3] + maxZCol.D - fParam[Id].roiP[roiID].AnchorCol;

                if (C1 < 0)
                {
                    C1 = 0;
                }

                //生成矩形框
                HObject left = new HObject();
                HObject right = new HObject();
                HOperatorSet.GenRegionLine(out left, 0, C1, 1000, C1);
                HOperatorSet.GenRegionLine(out right, 0, C2, 1000, C2);
                if (hwnd_profile != null && ShowFeatures)
                {
                    hwnd_profile.viewWindow.displayHobject(left, "green");
                    hwnd_profile.viewWindow.displayHobject(right, "green");

                }

                //求矩形区域内轮廓 极值
                HTuple ColLess = temp_2.TupleLessElem(C2);
                HTuple ColGreater = temp_2.TupleGreaterElem(C1);
                HTuple sub = ColLess.TupleSub(ColGreater);
                HTuple IntersetID = sub.TupleFind(0);
                //HTuple IntersetID = eq0.TupleFind(1);
                if (IntersetID == -1)
                {
                    return "区域内 无有效点";
                }
                HTuple RowNew = temp_1[IntersetID];
                HTuple ColNew = temp_2[IntersetID];

           
                int deg = fParam[Id].roiP[roiID].AngleOfProfile;
                GetInflection(RowNew, ColNew, deg, out LastR, out LastC,out RotateRow,out RotateCol,out HomMat,out rotateMaxR,out rotateMaxC, hwnd_profile, true, true, true, ShowFeatures
                    , fParam[Id].roiP[roiID].useZzoom, fParam[Id].roiP[roiID].ClippingPer, fParam[Id].roiP[roiID].Sigma);

                if (LastR.Length == 0)
                {
                    return "Ignore";
                }
                HTuple Max = LastR.TupleMax();
                HTuple maxId = LastR.TupleFindFirst(Max);

                LastC = LastC[maxId];
                LastR = Max;
                AnchorR = LastR;
                AnchorC = LastC;

                ////启用中间点
                //if (fParam[Id].roiP[roiID].useMidPt)
                //{
                //    HTuple EdgeCol = fParam[Id].roiP[roiID].useLeft ? temp_2.TupleMin() : temp_2.TupleMax();
                //    HTuple EdgColId = temp_2.TupleFindFirst(EdgeCol);
                //    HTuple EdgegRow = temp_1[EdgColId];
                //    //取中间点
                //    HTuple midR = (LastR + EdgegRow) / 2;
                //    HTuple midC = (LastC + EdgeCol) / 2;
                //    LastR = midR;
                //    LastC = midC;
                //}


                if (hwnd_profile != null && ShowFeatures)
                {
                    HObject cross = new HObject();
                    HOperatorSet.GenCrossContourXld(out cross, LastR, LastC, 10, 0);
                    hwnd_profile.viewWindow.displayHobject(cross, "green");

                }

                if (LastR.Length == 0)
                {
                    return "Ignore";
                }
                HTuple PtID = new HTuple();
                HOperatorSet.TupleGreaterEqualElem(col, LastC, out PtID);
                PtID = PtID.TupleFindFirst(1);
                LastR = Row[ProfileId][PtID];
                LastC = CArray[ProfileId][PtID];

                //LastR = ProfileId;
                //if (MyGlobal.globalConfig.dataContext.xResolution != 0)
                //{
                //    LastC = LastC / 200 / MyGlobal.globalConfig.dataContext.xResolution;
                //}
                //else
                //{
                //    LastC = LastC / 200 / 0.007;
                //}

                if (fParam[0].BeLeft)
                {
                    // 作为锚定点
                    //第一条
                    int sX1 = fParam[Id].roiP[roiID].StartOffSet1.X;
                    int sY1 = fParam[Id].roiP[roiID].StartOffSet1.Y;
                    int eX1 = fParam[Id].roiP[roiID].EndOffSet1.X;
                    int eY1 = fParam[Id].roiP[roiID].EndOffSet1.Y;
                    //第二条
                    int sX2 = fParam[Id].roiP[roiID].StartOffSet2.X;
                    int sY2 = fParam[Id].roiP[roiID].StartOffSet2.Y;
                    int eX2 = fParam[Id].roiP[roiID].EndOffSet2.X;
                    int eY2 = fParam[Id].roiP[roiID].EndOffSet2.Y;
                    //起点
                    HTuple gX1 = sX1 < eX1 ? col.TupleGreaterEqualElem(AnchorC.D + sX1) : col.TupleGreaterEqualElem(AnchorC.D + eX1);
                    HTuple gY1 = sY1 < eY1 ? row.TupleGreaterEqualElem(AnchorR.D + sY1) : row.TupleGreaterEqualElem(AnchorR.D + eY1);
                    HTuple eqx1 = gX1.TupleFind(1);
                    HTuple eqy1 = gY1.TupleFind(1);
                    HTuple SID1 = eqx1.TupleIntersection(eqy1);
                    //终点
                    gX1 = sX1 < eX1 ? col.TupleLessEqualElem(AnchorC.D + eX1) : col.TupleLessEqualElem(AnchorC.D + sX1);
                    gY1 = sY1 < eY1 ? row.TupleLessEqualElem(AnchorR.D + eY1) : row.TupleLessEqualElem(AnchorR.D + sY1);
                    eqx1 = gX1.TupleFind(1);
                    eqy1 = gY1.TupleFind(1);
                    HTuple EID1 = eqx1.TupleIntersection(eqy1);
                    HTuple FID1 = SID1.TupleIntersection(EID1);//第一条 选取点索引

                    if (FID1.Length == 0)
                    {

                        return "OK";

                    }
                    //起点
                    HTuple gX2 = sX2 < eX2 ? col.TupleGreaterEqualElem(AnchorC.D + sX2) : col.TupleGreaterEqualElem(AnchorC.D + eX2);
                    HTuple gY2 = sY2 < eY2 ? row.TupleGreaterEqualElem(AnchorR.D + sY2) : row.TupleGreaterEqualElem(AnchorR.D + eY2);
                    HTuple eqx2 = gX2.TupleFind(1);
                    HTuple eqy2 = gY2.TupleFind(1);
                    HTuple SID2 = eqx2.TupleIntersection(eqy2);
                    //终点
                    gX2 = sX2 < eX2 ? col.TupleLessEqualElem(AnchorC.D + eX2) : col.TupleLessEqualElem(AnchorC.D + sX2);
                    gY2 = sY2 < eY2 ? row.TupleLessEqualElem(AnchorR.D + eY2) : row.TupleLessEqualElem(AnchorR.D + sY2);
                    eqx2 = gX2.TupleFind(1);
                    eqy2 = gY2.TupleFind(1);
                    HTuple EID2 = eqx2.TupleIntersection(eqy2);
                    HTuple FID2 = SID2.TupleIntersection(EID2);//第二条 选取点索引
                    if (FID2.Length == 0)
                    {
                        return "OK";
                    }

                    HTuple intersectR, intersectC, isOverlapping;

                    if (true)
                    {
                        HTuple Linr1 = row[FID1];
                        HTuple Linc1 = col[FID1];
                        HTuple Linr2 = row[FID2];
                        HTuple Linc2 = col[FID2];

                        HObject line1 = new HObject();
                        HOperatorSet.GenContourPolygonXld(out line1, Linr1, Linc1);
                        HTuple Rowbg1, Colbg1, RowEd1, ColEd1, Nr1, Nc1, Dist1;
                        HOperatorSet.FitLineContourXld(line1, "tukey", -1, 0, 5, 2, out Rowbg1, out Colbg1, out RowEd1, out ColEd1, out Nr1, out Nc1, out Dist1);
                        HOperatorSet.GenRegionLine(out line1, Rowbg1, Colbg1, RowEd1, ColEd1);

                        HObject line2 = new HObject();
                        HOperatorSet.GenContourPolygonXld(out line2, Linr2, Linc2);
                        HTuple Rowbg2, Colbg2, RowEd2, ColEd2, Nr2, Nc2, Dist2;
                        HOperatorSet.FitLineContourXld(line2, "tukey", -1, 0, 5, 2, out Rowbg2, out Colbg2, out RowEd2, out ColEd2, out Nr2, out Nc2, out Dist2);
                        HOperatorSet.GenRegionLine(out line2, Rowbg2, Colbg2, RowEd2, ColEd2);


                        HOperatorSet.IntersectionLines(Rowbg1, Colbg1, RowEd1, ColEd1, Rowbg2, Colbg2, RowEd2, ColEd2, out intersectR, out intersectC, out isOverlapping);


                        HTuple PtID2 = new HTuple();
                        HOperatorSet.TupleGreaterEqualElem(col, intersectC, out PtID2);
                        PtID2 = PtID2.TupleFindFirst(1);
                        LastR = Row[ProfileId][PtID2];
                        LastC = CArray[ProfileId][PtID2];
                        if (hwnd_profile != null)
                        {
                            HObject cross2 = new HObject();
                            HOperatorSet.GenCrossContourXld(out cross2, intersectR, intersectC, 10, 45);
                            HOperatorSet.SetColor(hwnd_profile.HWindowHalconID, "blue");
                            HOperatorSet.DispObj(cross2, hwnd_profile.HWindowHalconID);
                            HOperatorSet.DispObj(line1, hwnd_profile.HWindowHalconID);
                            HOperatorSet.DispObj(line2, hwnd_profile.HWindowHalconID);
                        }
                    }
                }


                if (hwnd != null)
                {
                    HObject cross1 = new HObject();
                    HOperatorSet.GenCrossContourXld(out cross1, LastR, LastC, 10, 0);
                    HOperatorSet.SetColor(hwnd.HWindowHalconID, "red");
                    HOperatorSet.DispObj(cross1, hwnd.HWindowHalconID);

                }


                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "FindMaxPt error" + ex.Message + RowNum;
            }

        }

        public string FindEdge(int SideId, int ProfileId, out HTuple EdgeR, out HTuple EdgeC, HWindow_Final hwnd = null, bool ShowFeatures = false)
        {
            EdgeR = new HTuple(); EdgeC = new HTuple();
            try
            {
                int Id = SideId - 1;
                int roiID = -1;
                for (int i = 0; i < fParam[Id].roiP.Count; i++)
                {

                    for (int j = 0; j < fParam[Id].roiP[i].NumOfSection; j++)
                    {
                        roiID++;
                        if (roiID == ProfileId)
                        {
                            break;
                        }
                    }
                    if (roiID == ProfileId)
                    {
                        roiID = i;
                        break;
                    }
                }
                HTuple row, col;
                ShowProfile(ProfileId, out row, out col, hwnd);

                if (RArray == null || RArray[ProfileId].Length == 0)
                {
                    return "RArray is NULL";
                }

                if (roiList[Id].Count == 0)
                {
                    return "FindEdge error roiList is Null";
                }
                

                EdgeC = fParam[Id].roiP[roiID].useLeft ? col.TupleMin() : col.TupleMax();
                HTuple mZRowId = col.TupleFindFirst(EdgeC);
                EdgeR = row[mZRowId];

                if (hwnd != null && ShowFeatures)
                {
                    HObject cross = new HObject();
                    HOperatorSet.GenCrossContourXld(out cross, EdgeR, EdgeC, 10, 0);
                    hwnd.viewWindow.displayHobject(cross, "green");

                }

                HTuple PtID = new HTuple();
                HOperatorSet.TupleGreaterEqualElem(col, EdgeC, out PtID);
                PtID = PtID.TupleFindFirst(1);
                EdgeR = Row[ProfileId][PtID];
                EdgeC = CArray[ProfileId][PtID];

                if (hwnd != null)
                {
                    HObject cross1 = new HObject();
                    HOperatorSet.GenCrossContourXld(out cross1, EdgeR, EdgeC, 10, 0);
                    HOperatorSet.SetColor(hwnd.HWindowHalconID, "green");
                    HOperatorSet.DispObj(cross1, hwnd.HWindowHalconID);
                }

                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "FindEdge error" + ex.Message + RowNum;
            }

        }

        public string FindIntersectPoint(int Side, HObject HeightImage, out IntersetionCoord intersectCoord, HWindow_Final hwnd = null, bool debug = false,bool ShowFeatures = true)
        {
            List<HTuple> Hlines = new List<HTuple>(); intersectCoord = new IntersetionCoord();
            try
            {
                string ok = FindPoint(Side, HeightImage, out Hlines, hwnd, debug,ShowFeatures);
                if (ok != "OK")
                {
                    return ok;
                }
                if (Hlines.Count != 4)
                {
                    return "定位找线区域数量错误";
                }
                for (int i = 0; i < Hlines.Count; i++)
                {
                    if (Hlines[i].Length == 0)
                    {
                        return $"定位找线区域{i + 1}运行失败";
                    }
                }

                //1,4 取中线  2，3取拟合线 
                HTuple rb1 = (Hlines[0][0].D + Hlines[3][0].D) / 2;
                HTuple cb1 = (Hlines[0][1].D + Hlines[3][1].D) / 2;
                HTuple re1 = (Hlines[0][2].D + Hlines[3][2].D) / 2;
                HTuple ce1 = (Hlines[0][3].D + Hlines[3][3].D) / 2;

                //取1 4 中点
                HTuple midR = (rb1.D + re1.D) / 2;
                HTuple midC = (cb1.D + ce1.D) / 2;
                HObject crossMid = new HObject();
                HOperatorSet.GenCrossContourXld(out crossMid, midR, midC, 30, 0.5);
                rb1 = midR.D;
                re1 = midR.D;
                cb1 = 0;
                ce1 = 5000;

                HObject contHorizon = new HObject();
                HOperatorSet.GenContourPolygonXld(out contHorizon, rb1.TupleConcat(re1), cb1.TupleConcat(ce1));

                HTuple rb2 = (Hlines[1][0].D + Hlines[2][0].D) / 2;
                HTuple cb2 = (Hlines[1][1].D + Hlines[2][1].D) / 2;
                HTuple re2 = (Hlines[1][2].D + Hlines[2][2].D) / 2;
                HTuple ce2 = (Hlines[1][3].D + Hlines[2][3].D) / 2;
                HTuple rowVer = new HTuple(); HTuple colVer = new HTuple();
                rowVer = rowVer.TupleConcat(Hlines[1][0].D).TupleConcat(Hlines[1][2].D).TupleConcat(Hlines[2][0].D).TupleConcat(Hlines[2][2].D);
                colVer = colVer.TupleConcat(Hlines[1][1].D).TupleConcat(Hlines[1][3].D).TupleConcat(Hlines[2][1].D).TupleConcat(Hlines[2][3].D);

                HObject contVer = new HObject();
                HOperatorSet.GenContourPolygonXld(out contVer, rowVer, colVer);

                //拟合线
                //HObject line = new HObject();
                HTuple Nr, Nc, Dist;
                HOperatorSet.FitLineContourXld(contVer, "tukey", -1, 0, 5, 2, out rb2, out cb2, out re2, out ce2, out Nr, out Nc, out Dist);
                HOperatorSet.GenContourPolygonXld(out contVer, rb2.TupleConcat(re2), cb2.TupleConcat(ce2));

                //HOperatorSet.GenContourPolygonXld(out contVer, rb2.TupleConcat(re2), cb2.TupleConcat(ce2));
                HTuple Row, Col, Angle;
                HOperatorSet.AngleLx(rb2, cb2, re2, ce2, out Angle);
                HTuple isOver;
                //HOperatorSet.IntersectionContoursXld(contHorizon, contVer, "mutual", out Row, out Col, out isOver);
                HOperatorSet.IntersectionLines(rb1, cb1, re1, ce1, rb2, cb2, re2, ce2, out Row, out Col, out isOver);
                if (hwnd != null && ShowFeatures)
                {
                    //hwnd.viewWindow.displayHobject(contHorizon, "red");
                    hwnd.viewWindow.displayHobject(crossMid, "red");
                    HObject Cross = new HObject();
                    HOperatorSet.GenCrossContourXld(out Cross, Row, Col, 30, 0.5);
                    hwnd.viewWindow.displayHobject(contVer, "blue");
                    hwnd.viewWindow.displayHobject(Cross, "red");


                    double xResolution = MyGlobal.globalConfig.dataContext.xResolution;
                    double yResolution = MyGlobal.globalConfig.dataContext.yResolution;
                    HTuple row1 = Row.D * xResolution;
                    HTuple col1 = Col.D * yResolution;

                    string Rowstr = (Math.Round(row1.D, 3)).ToString();
                    string Colstr = (Math.Round(col1.D, 3)).ToString();
                    string Anglestr = (Math.Round(Angle.D, 3)).ToString();

                    hwnd.viewWindow.dispMessage(Rowstr, "red", Row, Col + 100);
                    hwnd.viewWindow.dispMessage(Colstr, "red", Row.D + 50, Col + 100);
                    hwnd.viewWindow.dispMessage(Anglestr, "red", Row.D + 100, Col + 100);

                }
                intersectCoord.Row = Row.D;
                intersectCoord.Col = Col.D;
                intersectCoord.Angle = Angle.D;
                if (ok != "OK")
                {
                    return ok;
                }


                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "FindIntersectPoint error" + ex.Message + RowNum;
            }
        }

        public void DispSection(ROIRectangle2 roi, int SideID, int RoiId, out HTuple[] LineCoord, HWindow_Final hwnd = null)
        {
            LineCoord = new HTuple[1];
            try
            {
                HTuple RoiCoord = roi.getModelData();

                double cosa = Math.Cos(RoiCoord[2].D);
                double sina = Math.Sin(RoiCoord[2].D);
                double r1 = RoiCoord[0].D + RoiCoord[4].D * cosa;
                double c1 = RoiCoord[1].D - RoiCoord[4].D * sina;
                double r2 = RoiCoord[0].D - RoiCoord[4].D * cosa;
                double c2 = RoiCoord[1].D + RoiCoord[4].D * sina;

                //

                int Num = fParam[SideID].roiP[RoiId].NumOfSection;
                LineCoord = new HTuple[Num];
                double Average = RoiCoord[4].D * 2 / (Num + 1);
                for (int i = 0; i < Num; i++)
                {
                    LineCoord[i] = new HTuple();
                    double AverageLen = Average * (i + 1);
                    double Row2 = r2 + AverageLen * cosa + RoiCoord[3].D * sina;
                    double Col2 = c2 - AverageLen * sina + RoiCoord[3].D * cosa;

                    double Row1 = r2 + AverageLen * cosa - RoiCoord[3].D * sina;
                    double Col1 = c2 - AverageLen * sina - RoiCoord[3].D * cosa;

                    LineCoord[i] = LineCoord[i].TupleConcat(Row1).TupleConcat(Col1).TupleConcat(Row2).TupleConcat(Col2).TupleConcat(RoiCoord[2].D);
                    if (hwnd != null)
                    {
                        HObject Line = new HObject();
                        HOperatorSet.GenRegionLine(out Line, LineCoord[i][0], LineCoord[i][1], LineCoord[i][2], LineCoord[i][3]);
                        HOperatorSet.SetColor(hwnd.HWindowHalconID, "red");
                        HOperatorSet.DispObj(Line, hwnd.HWindowHalconID);
                    }

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        string GenSection(HObject Image, HTuple lineCoord, out HTuple row, out HTuple col, out HTuple IgnorePt)
        {
            row = new HTuple(); col = new HTuple(); IgnorePt = 0;
            try
            {
                HTuple width, height, lessId1, lessId2;
                HOperatorSet.GetImageSize(Image, out width, out height);

                HObject Rline; HObject Rline1 = new HObject();
                HOperatorSet.GenRegionLine(out Rline, lineCoord[0], lineCoord[1], lineCoord[2], lineCoord[3]);
                HOperatorSet.GenContourRegionXld(Rline, out Rline1, "center");
                HOperatorSet.GetContourXld(Rline1, out row, out col);

                if (row.Length == 0)
                {
                    IgnorePt = 1;
                    return "GenSection截面在图像之外";
                }

                HTuple deg = -(new HTuple(lineCoord[4].D)).TupleDeg();
                double tan = Math.Abs(Math.Tan(-lineCoord[4].D));

                int len1 = Math.Abs((int)(lineCoord[1].D - lineCoord[3].D));
                int len2 = Math.Abs((int)(lineCoord[0].D - lineCoord[2].D));
                int len = len1 > len2 ? len1 : len2;
                HTuple x0 = lineCoord[1].D;
                HTuple y0 = lineCoord[0].D;
                HTuple newr = y0; HTuple newc = x0;
                for (int i = 1; i < len; i++)
                {
                    HTuple row1 = new HTuple(); HTuple col1 = new HTuple();
                    if (len1 > len2)
                    {
                        row1 = lineCoord[0].D > lineCoord[2].D ? y0 - i * tan : y0 + i * tan;
                        col1 = lineCoord[1].D > lineCoord[3].D ? x0 - i : x0 + i;
                    }
                    else
                    {
                        row1 = lineCoord[0].D < lineCoord[2].D ? y0 + i : y0 - i;
                        col1 = lineCoord[1].D < lineCoord[3].D ? x0 + i / tan : x0 - i / tan;
                    }

                    newr = newr.TupleConcat(row1);
                    newc = newc.TupleConcat(col1);
                }
                row = newr;
                col = newc;





                HOperatorSet.TupleLessElem(row, height, out lessId1);
                HOperatorSet.TupleFind(lessId1, 1, out lessId1);
                if (lessId1.D == -1)
                {
                    IgnorePt = 1;
                    row = 0;
                    col = 0;
                    return "GenSection截面在图像之外";
                }

                row = row[lessId1];
                col = col[lessId1];
                HOperatorSet.TupleLessElem(col, width, out lessId2);
                HOperatorSet.TupleFind(lessId2, 1, out lessId2);
                if (lessId2.D == -1)
                {
                    IgnorePt = 1;
                    row = 0;
                    col = 0;
                    return "GenSection截面在图像之外";
                }

                row = row[lessId2];
                col = col[lessId2];

                //且 行列大于零
                HTuple lessId3, lessId4;
                HOperatorSet.TupleGreaterElem(col, 0, out lessId3);
                HOperatorSet.TupleFind(lessId3, 1, out lessId3);
                row = row[lessId3];
                col = col[lessId3];

                HOperatorSet.TupleGreaterElem(row, 0, out lessId4);
                HOperatorSet.TupleFind(lessId4, 1, out lessId4);
                row = row[lessId4];
                col = col[lessId4];

                //HOperatorSet.GetGrayval(Image, row, col, out Zpoint);
                //HTuple EqId = new HTuple();
                //HOperatorSet.TupleNotEqualElem(Zpoint, -30, out EqId);
                //HOperatorSet.TupleFind(EqId, 1, out EqId);
                //if (EqId.D != -1)
                //{
                //    Zpoint = Zpoint[EqId];
                //    Cpoint = col[EqId];
                //    Rpoint = row[EqId];
                //    IgnorePt = 0;

                //}
                //else
                //{
                //    IgnorePt = 1;
                //}


                Rline.Dispose();
                Rline1.Dispose();
                //ConstImage.Dispose();

                //Contour.Dispose();

                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start!= -1 ?  "--" + a.Substring(start, a.Length - start):"";
                return "GenSection error" + ex.Message + RowNum;
            }
        }

        public string GenProfileCoord(int SideId, HObject HeightImage, out double[][] Rarray, out double[][] Row, out double[][] Carray, out double[][] Phi, out int ignorePt, HTuple Fix = null)
        {
            Rarray = null; Carray = null; Row = null; Phi = null; ignorePt = 0;
            try
            {
                int SId = SideId - 1;
                int k = 0;
                //
                for (int i = 0; i < roiList2[SId].Count; i++)
                {
                    for (int j = 0; j < fParam[SId].roiP[i].NumOfSection; j++)
                    {
                        k++;
                    }
                }
                int n = 0;
                Rarray = new double[k][]; Carray = new double[k][]; Row = new double[k][]; Phi = new double[k][];
                double[] Rarray1;
                double[] Carray1;

                HTuple HPhi = new HTuple();

                int[] profileNum = new int[k];
                for (int i = 0; i < roiList2[SId].Count; i++)
                {
                    //Debug.WriteLine(n);
                    HTuple orignal = roiList2[SId][i].getModelData();
                    ROIRectangle2 temp = new ROIRectangle2(orignal[0], orignal[1], orignal[2], orignal[3], orignal[4]);
                    HTuple[] lineCoord = new HTuple[1];
                    //将矩形进行定位
                    if (Fix != null)
                    {
                        HTuple recCoord = temp.getModelData();
                        HTuple CenterR = new HTuple(); HTuple CenterC = new HTuple();
                        List<ROI> temproi = new List<ROI>();
                        HTuple tempR = new HTuple(); HTuple tempC = new HTuple();
                        HOperatorSet.AffineTransPoint2d(Fix, recCoord[0], recCoord[1], out CenterR, out CenterC);
                        temp.Row = CenterR; temp.Column = CenterC;
                    }

                    DispSection(temp, SId, i, out lineCoord);
                    for (int j = 0; j < lineCoord.Length; j++)
                    {
                        /* HTuple Sigle = new HTuple();*/
                        HTuple col = new HTuple(); HTuple row = new HTuple(); HTuple ignore = new HTuple();
                        string ok = GenSection(HeightImage, lineCoord[j], out row, out col, out ignore);
                        ignorePt += ignore;
                        profileNum[n] = row.Length;
                        Rarray[n] = row;
                        Carray[n] = col;
                        //HRarray = HRarray.TupleConcat(row);
                        //HCarray = HCarray.TupleConcat(col);
                        //HRow = HRow.TupleConcat(row);

                        Phi[n] = lineCoord[j];
                        n++;
                        //if (n == 358)
                        //{
                        //    Debug.WriteLine("error");
                        //}
                    }
                }
                int total = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < profileNum[i]; j++)
                    {
                        //if (Rarray[i][j] != 0 && Carray[i][j] != 0)
                        //{
                        total++;
                        //}

                    }
                }
                Rarray1 = new double[total];
                Carray1 = new double[total];

                int tt = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < profileNum[i]; j++)
                    {
                        Rarray1[tt] = Rarray[i][j];
                        Carray1[tt] = Carray[i][j];
                        tt++;

                    }
                }

                HTuple Zpoint = new HTuple(); HTuple HRow = new HTuple(); HTuple HRarray = new HTuple(); HTuple HCarray = new HTuple();
                try
                {
                    HOperatorSet.GetGrayval(HeightImage, Rarray1, Carray1, out Zpoint);
                }
                catch (Exception ex)
                {
                    string a = ex.StackTrace;
                    int ind = a.IndexOf("行号");
                    int start = ind;
                    string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                    return "GenProfileCoord error: 区域位于图像之外" + RowNum;
                }

                HRarray = Rarray1; HCarray = Carray1;
                int m = 0;

                for (int i = 0; i < profileNum.Length; i++)
                {
                    if (profileNum[i] == 0)
                    {
                        continue;
                    }
                    HTuple z = Zpoint.TupleSelectRange(m, m + profileNum[i] - 1);
                    HTuple r = HRarray.TupleSelectRange(m, m + profileNum[i] - 1);
                    HTuple c = HCarray.TupleSelectRange(m, m + profileNum[i] - 1);


                    HTuple EqId = new HTuple();
                    HOperatorSet.TupleGreaterElem(z, -10, out EqId);
                    HOperatorSet.TupleFind(EqId, 1, out EqId);
                    if (EqId.D != -1)
                    {
                        z = z[EqId];
                        r = r[EqId];
                        c = c[EqId];
                    }
                    else
                    {
                        ignorePt += 1;

                    }

                    Rarray[i] = z * 200;
                    Carray[i] = c;
                    Row[i] = r;
                    //Phi[m] = HPhi[j];
                    m += profileNum[i];

                }

                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "GenProfileCoord error:" + ex.Message + RowNum;
            }
        }

        private int Ignore = 0;
        public string FindPoint(int SideId, bool IsRight, HObject IntesityImage, HObject HeightImage, out double[][] RowCoord, out double[][] ColCoord, out double[][] ZCoord, out string[][] StrLineOrCircle, out HTuple[] originalPoint, HTuple HomMat3D = null, HWindow_Final hwind = null, bool debug = false, HTuple homMatFix = null, HObject OriginImage = null,bool ShowFeatures = true)
        {
            //HObject RIntesity = new HObject(), RHeight = new HObject();
            StringBuilder Str = new StringBuilder();
            originalPoint = new HTuple[2];
            RowCoord = null; ColCoord = null; ZCoord = null; StrLineOrCircle = null;
            try
            {
                int Sid = SideId - 1;
                string ok1 = GenProfileCoord(Sid + 1, HeightImage, out RArray, out Row, out CArray, out Phi, out Ignore, homMatFix);
                if (ok1 != "OK")
                {
                    return ok1;
                }
                int Len = roiList2[Sid].Count;// 区域数量
                if (Len == 0)
                {
                    //RIntesity.Dispose();
                    //RHeight.Dispose();
                    return "参数设置错误";
                }
                RowCoord = new double[Len][]; ColCoord = new double[Len][]; ZCoord = new double[Len][]; StrLineOrCircle = new string[Len][];
                double[][] RowCoordt = new double[Len][]; double[][] ColCoordt = new double[Len][]; /*double[][] ZCoordt = new double[Len][];*/

                int Num = 0; int Add = 0; HTuple NewZ = new HTuple();
                HTuple origRow = new HTuple();
                HTuple origCol = new HTuple();
                for (int i = 0; i < Len; i++)
                {
                    if (i == 9)
                    {
                        Debug.WriteLine(i);
                    }
                    HTuple row = new HTuple(), col = new HTuple();
                    HTuple edgeRow = new HTuple(); HTuple edgeCol = new HTuple();//边缘点
                    for (int j = Num; j < Add + fParam[Sid].roiP[i].NumOfSection; j++)
                    {
                        if (Num == 99)
                        {
                            Debug.WriteLine(Num);
                        }
                        //Debug.WriteLine(Num);
                        HTuple row1, col1; HTuple anchor, anchorc; HTuple edgeR1, edgeC1;
                        if (fParam[Sid].roiP[i].SelectedType == 0)
                        {
                            if (fParam[Sid].roiP[i].TopDownDist != 0 && fParam[Sid].roiP[i].xDist != 0)
                            {
                                string ok = FindMaxPtFallDown(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
                            }
                            else
                            {
                                string ok = FindMaxPt(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
                            }

                        }
                        else
                        {
                            //取最高点下降
                            string ok = FindMaxPtFallDown(Sid + 1, j, out row1, out col1, out anchor, out anchorc);

                        }
                        if (fParam[Sid].roiP[i].useMidPt)
                        {
                            //取边缘点
                            string ok = FindEdge(Sid + 1, j, out edgeR1, out edgeC1);
                            edgeRow = edgeRow.TupleConcat(edgeR1);
                            edgeCol = edgeCol.TupleConcat(edgeC1);
                        }

                        row = row.TupleConcat(row1);
                        col = col.TupleConcat(col1);

                        Num++;
                    }

                    string msg = "";
                    if (hwind != null && ShowFeatures)
                    {
                        msg = fParam[Sid].DicPointName[i];
                    }

                    if (row.Length < 2)
                    {
                        return "区域" + msg + "拟合点数过少";
                    }


                    Add = Num;
                    if (row.Length == 0) //区域之外
                    {
                        continue;
                    }
                    HObject siglePart = new HObject();



                    int Clipping = 0;
                    int iNum = row.Length;
                    double clipping = (double)fParam[Sid].roiP[i].ClippingPer / 100;
                    Clipping = (int)(iNum * clipping);
                    if (Clipping == iNum / 2)
                    {
                        Clipping = iNum / 2 - 1;
                    }

                    HTuple lineAngle;
                    //取Roi角度
                    lineAngle = 1.571 - fParam[Sid].roiP[i].phi;
                    double angle1 = 1.571 + fParam[Sid].roiP[i].phi;
                    double Smoothcont = fParam[Sid].roiP[i].SmoothCont;
                    string IgnoreStr = IgnorPoint(row, col, angle1, Smoothcont, out row, out col);
                    if (IgnoreStr != "OK")
                    {
                        return "忽略点处理失败" + IgnoreStr;
                    }

                    if (hwind != null && debug)
                    {
                        HObject Cross = new HObject();
                        HOperatorSet.GenCrossContourXld(out Cross, row, col, 5, 0.5);

                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(Cross, "green", false);
                        };
                        hwind.Invoke(sw);

                        if (fParam[Sid].roiP[i].useMidPt)
                        {
                            HObject Cross1 = new HObject();
                            HOperatorSet.GenCrossContourXld(out Cross1, edgeRow, edgeCol, 5, 0.5);

                            Action sw2 = () =>
                            {
                                hwind.viewWindow.displayHobject(Cross1, "green", false);
                            };
                            hwind.Invoke(sw2);
                        }
                    }


                    //直线段拟合
                    //if (fParam[Sid].roiP[i].LineOrCircle != "圆弧段")
                    //{
                    HObject line = new HObject();
                    HOperatorSet.GenContourPolygonXld(out line, row, col);

                    HTuple Rowbg, Colbg, RowEd, ColEd, Nr, Nc, Dist;
                    HOperatorSet.FitLineContourXld(line, "tukey", -1, 0, 5, 2, out Rowbg, out Colbg, out RowEd, out ColEd, out Nr, out Nc, out Dist);
                    HOperatorSet.GenContourPolygonXld(out line, Rowbg.TupleConcat(RowEd), Colbg.TupleConcat(ColEd));

                    HObject lineEdge = new HObject();
                    if (fParam[Sid].roiP[i].useMidPt)
                    {
                        HOperatorSet.GenContourPolygonXld(out lineEdge, edgeRow, edgeCol);
                        HTuple Rowbg1, Colbg1, RowEd1, ColEd1, Nr1, Nc1, Dist1;
                        HOperatorSet.FitLineContourXld(lineEdge, "tukey", -1, 0, 5, 2, out Rowbg1, out Colbg1, out RowEd1, out ColEd1, out Nr1, out Nc1, out Dist1);
                        HOperatorSet.GenContourPolygonXld(out lineEdge, Rowbg1.TupleConcat(RowEd1), Colbg1.TupleConcat(ColEd1));

                        HTuple midRbg = (Rowbg + Rowbg1) / 2;
                        HTuple midRed = (RowEd + RowEd1) / 2;
                        HTuple midCbg = (Colbg + Colbg1) / 2;
                        HTuple midCed = (ColEd + ColEd1) / 2;
                        Rowbg = midRbg;
                        RowEd = midRed;
                        Colbg = midCbg;
                        ColEd = midCed;
                    }

                    //取拟合线与ROI中心交点
                    //HObject RoiCenter = new HObject();
                    HTuple recCoord = roiList2[Sid][i].getModelData();
                    HTuple CenterR = new HTuple(); HTuple CenterC = new HTuple();
                    if (homMatFix != null)
                    {
                        HOperatorSet.AffineTransPoint2d(homMatFix, recCoord[0], recCoord[1], out CenterR, out CenterC);
                    }
                    else
                    {
                        CenterR = recCoord[0];
                        CenterC = recCoord[1];
                    }

                    double EndR = CenterR + 100 * Math.Sin(fParam[Sid].roiP[i].phi);
                    double EndC = CenterC + 100 * Math.Cos(fParam[Sid].roiP[i].phi);
                    //HOperatorSet.GenRegionLine(out RoiCenter, fParam[Sid].roiP[i].CenterRow, fParam[Sid].roiP[i].CenterCol, EndR, EndC);
                    //HOperatorSet.GenRegionLine(out line, Rowbg, Colbg, RowEd, ColEd);
                    //if (hwind != null && debug)
                    //{
                    //    hwind.viewWindow.displayHobject(RoiCenter);
                    //    hwind.viewWindow.displayHobject(line);
                    //}
                    HTuple isOverlapping = new HTuple();
                    HOperatorSet.IntersectionLines(CenterR, CenterC, EndR, EndC, Rowbg, Colbg, RowEd, ColEd, out row, out col, out isOverlapping);

                    HTuple lineCoord = Rowbg.TupleConcat(Colbg).TupleConcat(RowEd).TupleConcat(ColEd);

                    double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
                    double Yresolution = MyGlobal.globalConfig.dataContext.yResolution;

                    if (Xresolution == 0)
                    {
                        return "XResolution=0";
                    }
                    double DisX = fParam[Sid].roiP[i].offset * Math.Sin(lineAngle.D) / Xresolution;
                    double DisY = fParam[Sid].roiP[i].offset * Math.Cos(lineAngle.D) / Yresolution;

                    double D = Math.Sqrt(DisX * DisX + DisY * DisY);
                    if (fParam[Sid].roiP[i].offset > 0)
                    {
                        D = -D;
                    }
                    double distR = D * Math.Cos(lineAngle.D);
                    double distC = D * Math.Sin(lineAngle.D);

                    //row = (Rowbg.D + RowEd.D) / 2 - distR;
                    //col = (Colbg.D + ColEd.D) / 2 - distC;
                    row = row.D - distR;
                    col = col.D - distC;

                    double xOffset = fParam[Sid].roiP[i].Xoffset / Xresolution;
                    double yOffset = fParam[Sid].roiP[i].Yoffset / Yresolution;
                    row = row - yOffset;
                    col = col - xOffset;

                    if (hwind != null && ShowFeatures)
                    {
                        HObject cross1 = new HObject();
                        HOperatorSet.GenCrossContourXld(out cross1, row, col, 30, 0.5);

                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(cross1, "cadet blue", false);
                        };
                        hwind.Invoke(sw);
                    }


                    //HTuple linephi = new HTuple();
                    //HOperatorSet.LineOrientation(Rowbg, Colbg, RowEd, ColEd, out linephi);
                    //HTuple deg = -(new HTuple(linephi.D)).TupleDeg();
                    //double tan = Math.Tan(-linephi.D);

                    //int len1 = Math.Abs((int)(Rowbg.D - RowEd.D));
                    //int len2 = Math.Abs((int)(Colbg.D - ColEd.D));
                    //int len = len1 > len2 ? len1 : len2;
                    //HTuple x0 = Colbg.D;
                    //HTuple y0 = Rowbg.D;
                    //double[] newr = new double[len]; double[] newc = new double[len];
                    //for (int m = 0; m < len; m++)
                    //{
                    //    HTuple row1 = new HTuple(); HTuple col1 = new HTuple();
                    //    if (len1 > len2)
                    //    {
                    //        row1 = Rowbg.D > RowEd.D ? y0 - m : y0 + m;
                    //        col1 = Rowbg.D > RowEd.D ? x0 - m / tan : x0 + m / tan;

                    //    }
                    //    else
                    //    {
                    //        row1 = Colbg.D > ColEd.D ? y0 - m * tan : y0 + m * tan;
                    //        col1 = Colbg.D > ColEd.D ? x0 - m : x0 + m;
                    //    }
                    //    newr[m] = row1;
                    //    newc[m] = col1;
                    //}
                    //row = newr;col = newc;

                    //HOperatorSet.GenContourPolygonXld(out line, row, col);


                    if (hwind != null && debug)
                    {
                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(line, "red");
                            hwind.viewWindow.displayHobject(lineEdge, "red");
                        };
                        hwind.Invoke(sw);


                    }
                    siglePart = line;
                    //}
                    //else
                    //{
                    //    HObject ArcObj = new HObject();
                    //    HOperatorSet.GenContourPolygonXld(out ArcObj, row, col);
                    //    HTuple Rb, Cb, Re, Ce, Nr1, Nc1, Ptorder;
                    //    HOperatorSet.FitLineContourXld(ArcObj, "tukey", -1, Clipping, 5, 2, out Rb, out Cb, out Re, out Ce, out Nr1, out Nc1, out Ptorder);
                    //    HOperatorSet.GenContourPolygonXld(out ArcObj, Rb.TupleConcat(Re), Cb.TupleConcat(Ce));

                    //    HTuple lineAngle;
                    //    HOperatorSet.AngleLx(Rb, Cb, Re, Ce, out lineAngle);
                    //    double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
                    //    double Yresolution = MyGlobal.globalConfig.dataContext.yResolution;

                    //    if (Xresolution == 0)
                    //    {
                    //        return "XResolution = 0";
                    //    }
                    //    double DisX = fParam[Sid].roiP[i].offset * Math.Sin(lineAngle.D) / Xresolution;
                    //    double DisY = fParam[Sid].roiP[i].offset * Math.Cos(lineAngle.D) / Yresolution;
                    //    double D = Math.Sqrt(DisX * DisX + DisY * DisY);
                    //    if (fParam[Sid].roiP[i].offset < 0)
                    //    {
                    //        D = -D;
                    //    }
                    //    double distR = D * Math.Cos(lineAngle.D);
                    //    double distC = D * Math.Sin(lineAngle.D);

                    //    row = (Rb.D + Re.D) / 2 - distR;
                    //    col = (Cb.D + Ce.D) / 2 - distC;

                    //    double xOffset = fParam[Sid].roiP[i].Xoffset / Xresolution;
                    //    double yOffset = fParam[Sid].roiP[i].Yoffset / Yresolution;
                    //    row = row - yOffset;
                    //    col = col - xOffset;

                    //    if (hwind != null)
                    //    {
                    //        HObject CrossArc = new HObject();
                    //        HOperatorSet.GenCrossContourXld(out CrossArc, row, col, 30, 0.5);
                    //        hwind.viewWindow.displayHobject(CrossArc, "blue");
                    //    }
                    //    if (hwind != null && debug)
                    //    {

                    //        hwind.viewWindow.displayHobject(ArcObj, "red");
                    //        hwind.viewWindow.dispMessage(msg, "blue", row.D, col.D);
                    //    }

                    //    siglePart = ArcObj;
                    //}



                    //加上 x y z 偏移
                    //row = row + fParam[Sid].roiP[i].Yoffset;
                    //col = col + fParam[Sid].roiP[i].Xoffset;


                    //旋转至原图
                    //HOperatorSet.AffineTransPoint2d(homMatRotateInvert, row, col, out row, out col);



                    HTuple AvraRow = new HTuple(), AvraCol = new HTuple(), AvraZ = new HTuple();
                    int PointCount = fParam[Sid].roiP[i].NumOfSection;

                    //取Z值 
                    HTuple zcoord = new HTuple();
                    try
                    {
                        if (MyGlobal.globalConfig.enableAlign)
                        {
                            HOperatorSet.GetGrayval(OriginImage, row, col, out zcoord);
                        }
                        else
                        {
                            HOperatorSet.GetGrayval(HeightImage, row, col, out zcoord);
                        }

                    }
                    catch (Exception ex)
                    {
                        string a = ex.StackTrace;
                        int ind = a.IndexOf("行号");
                        int start = ind;
                        string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                        return "偏移点" + msg + "设置在图像之外" + RowNum;
                    }

                    #region MyRegion
                    ////除去 -100 的点
                    //HTuple eq100 = new HTuple();
                    //HOperatorSet.TupleLessElem(zcoord, -5, out eq100);
                    //HTuple eq100Id = new HTuple();
                    //HOperatorSet.TupleFind(eq100, 1, out eq100Id);
                    //HTuple not5 = eq100.TupleFind(0);
                    //HTuple Total = zcoord[not5];
                    //HTuple Mean = Total.TupleMean();
                    //if (eq100Id != -1)
                    //{
                    //    for (int m = 0; m < eq100Id.Length; m++)
                    //    {
                    //        if (eq100Id[m] == 0)
                    //        {
                    //            HTuple meanZ = new HTuple();
                    //            meanZ = meanZ.TupleConcat(zcoord[eq100Id[m].D], zcoord[eq100Id[m].D + 1], zcoord[eq100Id[m].D + 2], zcoord[eq100Id[m].D + 3], zcoord[eq100Id[m].D + 4], zcoord[eq100Id[m].D + 5]);
                    //            HTuple eq30 = new HTuple();
                    //            HOperatorSet.TupleGreaterElem(meanZ, -5, out eq30);
                    //            HTuple eq30Id = new HTuple();
                    //            HOperatorSet.TupleFind(eq30, 1, out eq30Id);
                    //            if (eq30Id != -1)
                    //            {
                    //                meanZ = meanZ[eq30Id];
                    //                meanZ = meanZ.TupleMean();
                    //                zcoord[eq100Id[m].D] = meanZ;
                    //            }
                    //            else
                    //            {
                    //                meanZ = Mean;
                    //                zcoord[eq100Id[m].D] = meanZ;
                    //            }

                    //        }
                    //        if (eq100Id[m] - 1 >= 0)
                    //        {
                    //            if (zcoord[eq100Id[m].D - 1].D < -5)
                    //            {
                    //                zcoord[eq100Id[m].D] = Mean;
                    //            }
                    //            else
                    //            {
                    //                zcoord[eq100Id[m].D] = zcoord[eq100Id[m].D - 1];
                    //            }

                    //        }
                    //        else
                    //        {
                    //            zcoord[eq100Id[m].D] = Mean;
                    //        }

                    //    }
                    //}


                    ////将行列Z 均分
                    //double  Div = row.Length / (double) ((PointCount - 1));
                    //if (Div <1)
                    //{
                    //    Div = 1;
                    //}
                    //for (int k = 0; k < PointCount; k++)
                    //{
                    //    if (k == 0)
                    //    {
                    //        AvraRow = AvraRow.TupleConcat(row[0]);
                    //        AvraCol = AvraCol.TupleConcat(col[0]);
                    //        AvraZ = AvraZ.TupleConcat(zcoord[0]);
                    //        continue;
                    //    }

                    //    if (k == 529)
                    //    {
                    //        //Debug.WriteLine("k" + k);
                    //    }
                    //    //Debug.WriteLine("k" + k);


                    //    int id = (int)(Div * k);
                    //    if (id >= row.Length)
                    //    {
                    //        break;
                    //    }
                    //    AvraRow = AvraRow.TupleConcat(row[id]);
                    //    AvraCol = AvraCol.TupleConcat(col[id]);
                    //    AvraZ = AvraZ.TupleConcat(zcoord[id]);
                    //}
                    //row = AvraRow;
                    //col = AvraCol;
                    //zcoord = AvraZ;


                    //HTuple xc = new HTuple();
                    //HOperatorSet.TupleGenSequence(0, zcoord.Length - 1, 1, out xc);
                    //HObject Cont = new HObject();
                    //HOperatorSet.GenContourPolygonXld(out Cont, zcoord, xc);
                    //if (fParam[Sid].roiP[i].LineOrCircle == "直线段")
                    //{

                    //    HTuple Rowbg1, Colbg1, RowEd1, ColEd1, Nr1, Nc1, Dist1;
                    //    HOperatorSet.FitLineContourXld(Cont, "tukey", -1, 0, 5, 2, out Rowbg1, out Colbg1, out RowEd1, out ColEd1, out Nr1, out Nc1, out Dist1);

                    //    HTuple linephi1 = new HTuple();
                    //    HOperatorSet.LineOrientation(Rowbg1, Colbg1, RowEd1, ColEd1, out linephi1);
                    //    //HTuple deg = -(new HTuple(linephi1.D)).TupleDeg();
                    //    double tan1 = Math.Tan(-linephi1.D);

                    //    int len11 = Math.Abs((int)(Rowbg1.D - RowEd1.D));
                    //    int len21 = Math.Abs((int)(Colbg1.D - ColEd1.D));
                    //    int len0 = zcoord.Length;
                    //    HTuple x01 = Colbg1.D;
                    //    HTuple y01 = Rowbg1.D;
                    //    double[] newr1 = new double[len0]; double[] newc1 = new double[len0];
                    //    for (int m = 0; m < len0; m++)
                    //    {
                    //        HTuple row1 = new HTuple(); HTuple col1 = new HTuple();
                    //        if (len11 > len21)
                    //        {
                    //            row1 = Rowbg1.D > RowEd1.D ? y01 - m : y01 + m;
                    //            col1 = Rowbg1.D > RowEd1.D ? x01 - m / tan1 : x01 + m / tan1;

                    //        }
                    //        else
                    //        {
                    //            row1 = Colbg1.D > ColEd1.D ? y01 - m * tan1 : y01 + m * tan1;
                    //            col1 = Colbg1.D > ColEd1.D ? x01 - m : x01 + m;
                    //        }
                    //        newr1[m] = row1;
                    //        newc1[m] = col1;
                    //    }
                    //    zcoord = newr1; xc = newc1;

                    //}
                    //else
                    //{
                    //    HOperatorSet.GenContourPolygonXld(out Cont, xc, zcoord);
                    //    HOperatorSet.SmoothContoursXld(Cont, out Cont, 15);
                    //    HOperatorSet.GetContourXld(Cont, out xc, out zcoord);
                    //}


                    //HTuple origRow = row;
                    //HTuple origCol = col;

                    #region 单边去重
                    //if (i > 0)
                    //{
                    //        HObject regXld = new HObject();
                    //        HOperatorSet.GenRegionContourXld(siglePart, out regXld, "filled");
                    //        HTuple phi;
                    //        HOperatorSet.RegionFeatures(regXld, "phi", out phi);
                    //        phi = phi.TupleDeg();
                    //        phi = phi.TupleAbs();
                    //        //HTuple last1 = RowCoordt[i - 1][0];//x1
                    //        //HTuple lastc1 = ColCoordt[i - 1][0];//x1
                    //        //HTuple sub1 = Math.Abs(row[0].D - last1.D);
                    //        //HTuple sub2 = Math.Abs(col[0].D - lastc1.D);
                    //        //HTuple pt1 = sub1.D > sub2.D ? RowCoordt[i - 1][RowCoordt[i - 1].Length - 1] : ColCoordt[i - 1][ColCoordt[i - 1].Length - 1];
                    //        HTuple pt1 = phi.D > 75 ? RowCoordt[i - 1][RowCoordt[i - 1].Length - 1] : ColCoordt[i - 1][ColCoordt[i - 1].Length - 1];
                    //        HTuple colbase = Sid == 0 || Sid == 2 ? col.TupleLessEqualElem(pt1) : col.TupleGreaterEqualElem(pt1);
                    //        HTuple Grater1 = phi.D > 75 ? row.TupleGreaterEqualElem(pt1) : colbase;

                    //        switch (Sid)
                    //    {
                    //        case 0: //x2>x1

                    //            HTuple Grater1id = Grater1.TupleFind(1);
                    //            row = row.TupleRemove(Grater1id);
                    //            col = col.TupleRemove(Grater1id);
                    //            zcoord = zcoord.TupleRemove(Grater1id);

                    //            break;
                    //        case 1: //y2>y1

                    //            HTuple Grater2id = Grater1.TupleFind(1);
                    //            row = row.TupleRemove(Grater2id);
                    //            col = col.TupleRemove(Grater2id);
                    //            //origRow = origRow.TupleRemove(Grater2id);
                    //            //origCol = origCol.TupleRemove(Grater2id);
                    //            zcoord = zcoord.TupleRemove(Grater2id);
                    //            break;
                    //        case 2: //x2<x1

                    //            HTuple Grater3id = Grater1.TupleFind(1);
                    //            row = row.TupleRemove(Grater3id);
                    //                col = col.TupleRemove(Grater3id);
                    //                //origRow = origRow.TupleRemove(Grater3id);
                    //                //origCol = origCol.TupleRemove(Grater3id);
                    //                zcoord = zcoord.TupleRemove(Grater3id);
                    //                break;
                    //        case 3: //y2<y1

                    //            HTuple Grater4id = Grater1.TupleFind(1);
                    //            row = row.TupleRemove(Grater4id);
                    //            col = col.TupleRemove(Grater4id);
                    //            //origRow = origRow.TupleRemove(Grater4id);
                    //            //origCol = origCol.TupleRemove(Grater4id);
                    //            zcoord = zcoord.TupleRemove(Grater4id);
                    //            break;
                    //    }
                    //}
                    //if (row.Length == 0)
                    //{
                    //    return "单边设置 区域重复点数过大";
                    //}
                    #endregion 单边去重
                    #endregion
                    //

                    if (hwind != null && debug == false && ShowFeatures)
                    {
                        HObject NewSide = new HObject();
                        HOperatorSet.GenContourPolygonXld(out NewSide, row, col);

                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(NewSide, "red");
                        };
                        hwind.Invoke(sw);
                    }
                    origRow = origRow.TupleConcat(row);
                    origCol = origCol.TupleConcat(col);
                    RowCoordt[i] = row;
                    ColCoordt[i] = col;
                    //进行矩阵变换
                    if (HomMat3D != null)
                    {
                        //HOperatorSet.AffineTransPoint3d(HomMat3D, row, col,zcoord, out row, out col,out zcoord);
                        HOperatorSet.AffineTransPoint2d(HomMat3D, row, col, out row, out col);
                        if (IsRight)
                        {
                            zcoord = zcoord + fParam[Sid].roiP[i].Zoffset + fParam[Sid].SigleZoffset + MyGlobal.globalPointSet_Right.TotalZoffset;
                            row = row + MyGlobal.globalPointSet_Right.gbParam[Sid].Xoffset;
                            col = col + MyGlobal.globalPointSet_Right.gbParam[Sid].Yoffset;
                        }
                        else
                        {
                            zcoord = zcoord + fParam[Sid].roiP[i].Zoffset + fParam[Sid].SigleZoffset + MyGlobal.globalPointSet_Left.TotalZoffset;
                            row = row + MyGlobal.globalPointSet_Left.gbParam[Sid].Xoffset;
                            col = col + MyGlobal.globalPointSet_Left.gbParam[Sid].Yoffset;
                        }

                    }

                    for (int n = 0; n < row.Length; n++)
                    {
                        Str.Append(row[n].D.ToString() + "," + col[n].D.ToString() + "," + zcoord[n].D.ToString() + "\r\n");
                    }
                    RowCoord[i] = row;
                    ColCoord[i] = col;
                    ZCoord[i] = zcoord;
                    StrLineOrCircle[i] = new string[zcoord.Length];


                    //ColCoordt[i] = col;
                    //ZCoordt[i] = zcoord;


                    //StrLineOrCircle[i][0] = fParam[Sid].roiP[i].LineOrCircle == "直线段" ? "1;" : "2;";
                    switch (fParam[Sid].roiP[i].LineOrCircle)
                    {
                        case "连接段":
                            StrLineOrCircle[i][0] = "0;";
                            break;
                        case "直线段":
                            StrLineOrCircle[i][0] = "1;";
                            break;
                        case "圆弧段":
                            StrLineOrCircle[i][0] = "2;";
                            break;
                    }
                    //for (int n = 1; n < zcoord.Length; n++)
                    //{

                    //    if (n== zcoord.Length -1 &&  i == Len -1 && Sid !=3) //最后一段 第四边不给
                    //    {
                    //        StrLineOrCircle[i][n] = StrLineOrCircle[i][0];
                    //    }
                    //    else
                    //    {
                    //        StrLineOrCircle[i][n] = "0;";

                    //    }
                    //}

                    //NewZ = NewZ.TupleConcat(ZCoord[i][0]);
                }
                //HTuple zId =  NewZ.TupleGreaterElem(-10);
                //zId = zId.TupleFind(1);
                //NewZ = NewZ[zId];
                //for (int i = 0; i < ZCoord.GetLength(0); i++)
                //{
                //    if (ZCoord[i]== null)
                //    {
                //        return "Find Point 第" + (i + 1).ToString() + "点未取到";
                //    }
                //    if (ZCoord[i][0]==-30)
                //    {

                //        ZCoord[i][0] = NewZ.TupleMean();
                //    }
                //}
                originalPoint[0] = origRow;
                originalPoint[1] = origCol;

                for (int i = 0; i < ZCoord.GetLength(0); i++)
                {



                    string msg = fParam[Sid].DicPointName[i] + "(" + Math.Round(ZCoord[i][0], 3).ToString() + ")";
                    if (hwind != null && ShowFeatures)
                    {

                        Action sw = () =>
                        {
                            hwind.viewWindow.dispMessage(msg, "blue", origRow[i], origCol[i]);
                        };
                        hwind.Invoke(sw);
                    }

                    #region 判断高度范围
                    //int[] order = new int[] { 3, 2, 1, 0 };
                    //if (true) //改变基准值排序方向
                    //{

                    //}
                    //if (IsRight)
                    //{
                    //    ////判断 Z 值高度
                    //    if (MyGlobal.xyzBaseCoord_Right.ZCoord != null && MyGlobal.xyzBaseCoord_Right.ZCoord.Count != 0)
                    //    {

                    //        if (i >= MyGlobal.xyzBaseCoord_Right.ZCoord[order[Sid]].GetLength(0))
                    //        {
                    //            return "请重新设置基准值";
                    //        }
                    //        double sub = ZCoord[i][0] - MyGlobal.xyzBaseCoord_Right.ZCoord[order[Sid]][i][0];
                    //        if (sub > MyGlobal.globalPointSet_Right.HeightMax || sub < MyGlobal.globalPointSet_Right.HeightMin)
                    //        {
                    //            if (UseSelfOffset) //启用自动补偿
                    //            {
                    //                ZCoord[i][0] -= sub;
                    //            }
                    //            else
                    //            {
                    //                if (hwind != null && ShowFeatures)
                    //                {

                    //                    Action sw = () =>
                    //                    {
                    //                        hwind.viewWindow.dispMessage(msg + "-Height NG", "red", origRow[i], origCol[i]);
                    //                    };
                    //                    hwind.Invoke(sw);
                    //                }
                    //                return $"{msg}高度超出范围" + Math.Round(sub, 3);
                    //            }


                    //        }

                    //    }
                    //}
                    //else
                    //{
                    //    ////判断 Z 值高度
                    //    if (MyGlobal.xyzBaseCoord_Left.ZCoord != null && MyGlobal.xyzBaseCoord_Left.ZCoord.Count != 0)
                    //    {
                    //        if (i >= MyGlobal.xyzBaseCoord_Left.ZCoord[order[Sid]].GetLength(0))
                    //        {
                    //            return "请重新设置基准值";
                    //        }
                    //        double sub = ZCoord[i][0] - MyGlobal.xyzBaseCoord_Left.ZCoord[order[Sid]][i][0];
                    //        if (sub > MyGlobal.globalPointSet_Left.HeightMax || sub < MyGlobal.globalPointSet_Left.HeightMin)
                    //        {
                    //            if (UseSelfOffset) //启用自动补偿
                    //            {
                    //                ZCoord[i][0] += sub;
                    //            }
                    //            else
                    //            {
                    //                if (hwind != null && ShowFeatures)
                    //                {

                    //                    Action sw = () =>
                    //                    {
                    //                        hwind.viewWindow.dispMessage(msg + "-Height NG", "red", origRow[i], origCol[i]);
                    //                    };
                    //                    hwind.Invoke(sw);
                    //                }
                    //                return $"{msg}高度超出范围" + Math.Round(ZCoord[i][0], 3);
                    //            }

                    //        }

                    //    }
                    //}
                    #endregion

                    #region 高度滤波

                    //在当前点附近取圆
                    HObject Circle = new HObject();
                    double radius = fParam[Sid].roiP[i].ZftRad;
                    //if (ZCoord[i][0] < -10 && radius == 0)
                    //{
                    //    radius = 0.05;
                    //}
                    double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
                    radius = radius / Xresolution;
                    if (radius != 0)
                    {
                        HOperatorSet.GenCircle(out Circle, origRow[i], origCol[i], radius);
                        //if (debug && hwind !=null)
                        //{
                        //    hwind.viewWindow.displayHobject(Circle, "red");
                        //}

                        HTuple rows, cols; HTuple Zpt = new HTuple();
                        HOperatorSet.GetRegionPoints(Circle, out rows, out cols);
                        try
                        {
                            if (MyGlobal.globalConfig.enableAlign)
                            {
                                HOperatorSet.GetGrayval(OriginImage, rows, cols, out Zpt);
                            }
                            else
                            {
                                HOperatorSet.GetGrayval(HeightImage, rows, cols, out Zpt);
                            }
                            HTuple greater = new HTuple();
                            HOperatorSet.TupleGreaterElem(Zpt, -10, out greater);
                            HTuple greaterId = greater.TupleFind(1);
                            if (greaterId.D == -1)
                            {
                                return "高度滤波区域" + fParam[Sid].DicPointName[i] + "无有效z值";
                            }
                            HTuple Zgreater = Zpt[greaterId];
                            HTuple maxPer = fParam[Sid].roiP[i].ZftMax / 100;
                            HTuple minPer = fParam[Sid].roiP[i].ZftMin / 100;

                            int imax = (int)maxPer.D * Zgreater.Length;
                            int imin = (int)minPer.D * Zgreater.Length;
                            //if (imax != imin)
                            //{
                            HTuple Down = Zgreater.TupleSelectRange(imin, Zgreater.Length - imax - 1);
                            ZCoord[i][0] = Down.TupleMean();
                            //}
                            //else
                            //{
                            //    ZCoord[i][0] = Zgreater.TupleMean();
                            //}
                            #endregion
                             

                        }
                        catch (Exception ex)
                        {
                            string a = ex.StackTrace;
                            int ind = a.IndexOf("行号");
                            int start = ind;
                            string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                            return "偏移点" + fParam[Sid].DicPointName[i] + "设置在图像之外" + RowNum;
                        }
                    }
                    
                }

                if (HomMat3D == null)
                {
                    StaticOperate.writeTxt("D:\\Laser3DOrign.txt", Str.ToString());
                }

                //RIntesity.Dispose();
                //RHeight.Dispose();
                return "OK";
            }
            catch (Exception ex)
            {

                //RIntesity.Dispose();
                //RHeight.Dispose();
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "FindPoint error:" + ex.Message + RowNum;
            }
        }

        public string FindPoint(int SideId, HObject IntesityImage, HObject HeightImage, out List<HTuple> Lines, HWindow_Final hwind = null, bool debug = false, HTuple homMatFix = null, HObject OriginImage = null, bool ShowFeatures = true)
        {

            Lines = new List<HTuple>();
            try
            {
                int Sid = SideId - 1;
                string ok1 = GenProfileCoord(Sid + 1, HeightImage, out RArray, out Row, out CArray, out Phi, out Ignore, homMatFix);
                if (ok1 != "OK")
                {
                    return ok1;
                }
                int Len = roiList2[Sid].Count;// 区域数量
                if (Len == 0)
                {
                    //RIntesity.Dispose();
                    //RHeight.Dispose();
                    return "参数设置错误";
                }
                double[][] RowCoordt = new double[Len][]; double[][] ColCoordt = new double[Len][]; /*double[][] ZCoordt = new double[Len][];*/

                int Num = 0; int Add = 0; HTuple NewZ = new HTuple();
                HTuple origRow = new HTuple();
                HTuple origCol = new HTuple();
                for (int i = 0; i < Len; i++)
                {
                    if (i == 11)
                    {
                        Debug.WriteLine(i);
                    }
                    HTuple row = new HTuple(), col = new HTuple();
                    HTuple edgeRow = new HTuple(); HTuple edgeCol = new HTuple();//边缘点
                    for (int j = Num; j < Add + fParam[Sid].roiP[i].NumOfSection; j++)
                    {
                        if (Num == 99)
                        {
                            Debug.WriteLine(Num);
                        }
                        //Debug.WriteLine(Num);
                        HTuple row1, col1; HTuple anchor, anchorc; HTuple edgeR1, edgeC1;
                        if (fParam[Sid].roiP[i].SelectedType == 0)
                        {
                            if (fParam[Sid].roiP[i].TopDownDist != 0 && fParam[Sid].roiP[i].xDist != 0)
                            {
                                string ok = FindMaxPtFallDown(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
                            }
                            else
                            {
                                string ok = FindMaxPt(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
                            }

                        }
                        else
                        {
                            //取最高点下降
                            string ok = FindMaxPtFallDown(Sid + 1, j, out row1, out col1, out anchor, out anchorc);

                        }
                        if (fParam[Sid].roiP[i].useMidPt)
                        {
                            //取边缘点
                            string ok = FindEdge(Sid + 1, j, out edgeR1, out edgeC1);
                            edgeRow = edgeRow.TupleConcat(edgeR1);
                            edgeCol = edgeCol.TupleConcat(edgeC1);
                        }

                        row = row.TupleConcat(row1);
                        col = col.TupleConcat(col1);

                        Num++;
                    }

                    string msg = "";
                    if (hwind != null && ShowFeatures)
                    {
                        msg = fParam[Sid].DicPointName[i];
                    }

                    if (row.Length < 2)
                    {
                        return "区域" + msg + "拟合点数过少";
                    }


                    Add = Num;
                    if (row.Length == 0) //区域之外
                    {
                        continue;
                    }
                    HObject siglePart = new HObject();



                    int Clipping = 0;
                    int iNum = row.Length;
                    double clipping = (double)fParam[Sid].roiP[i].ClippingPer / 100;
                    Clipping = (int)(iNum * clipping);
                    if (Clipping == iNum / 2)
                    {
                        Clipping = iNum / 2 - 1;
                    }

                    HTuple lineAngle;
                    //取Roi角度
                    lineAngle = 1.571 - fParam[Sid].roiP[i].phi;
                    double angle1 = 1.571 + fParam[Sid].roiP[i].phi;
                    double Smoothcont = fParam[Sid].roiP[i].SmoothCont;
                    string IgnoreStr = IgnorPoint(row, col, angle1, Smoothcont, out row, out col);
                    if (IgnoreStr != "OK")
                    {
                        return "忽略点处理失败" + IgnoreStr;
                    }

                    if (hwind != null && debug)
                    {
                        HObject Cross = new HObject();
                        HOperatorSet.GenCrossContourXld(out Cross, row, col, 5, 0.5);

                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(Cross, "green", false);
                        };
                        hwind.Invoke(sw);

                        if (fParam[Sid].roiP[i].useMidPt)
                        {
                            HObject Cross1 = new HObject();
                            HOperatorSet.GenCrossContourXld(out Cross1, edgeRow, edgeCol, 5, 0.5);

                            Action sw2 = () =>
                            {
                                hwind.viewWindow.displayHobject(Cross1, "green", false);
                            };
                            hwind.Invoke(sw2);
                        }
                    }

                    //直线段拟合
                    HObject line = new HObject();
                    HOperatorSet.GenContourPolygonXld(out line, row, col);

                    HTuple Rowbg, Colbg, RowEd, ColEd, Nr, Nc, Dist;
                    HOperatorSet.FitLineContourXld(line, "tukey", -1, 0, 5, 2, out Rowbg, out Colbg, out RowEd, out ColEd, out Nr, out Nc, out Dist);
                    HOperatorSet.GenContourPolygonXld(out line, Rowbg.TupleConcat(RowEd), Colbg.TupleConcat(ColEd));

                    HObject lineEdge = new HObject();
                    if (fParam[Sid].roiP[i].useMidPt)
                    {
                        HOperatorSet.GenContourPolygonXld(out lineEdge, edgeRow, edgeCol);
                        HTuple Rowbg1, Colbg1, RowEd1, ColEd1, Nr1, Nc1, Dist1;
                        HOperatorSet.FitLineContourXld(lineEdge, "tukey", -1, 0, 5, 2, out Rowbg1, out Colbg1, out RowEd1, out ColEd1, out Nr1, out Nc1, out Dist1);
                        HOperatorSet.GenContourPolygonXld(out lineEdge, Rowbg1.TupleConcat(RowEd1), Colbg1.TupleConcat(ColEd1));

                        HTuple midRbg = (Rowbg + Rowbg1) / 2;
                        HTuple midRed = (RowEd + RowEd1) / 2;
                        HTuple midCbg = (Colbg + Colbg1) / 2;
                        HTuple midCed = (ColEd + ColEd1) / 2;
                        Rowbg = midRbg;
                        RowEd = midRed;
                        Colbg = midCbg;
                        ColEd = midCed;
                    }

                    //取拟合线与ROI中心交点
                    //HObject RoiCenter = new HObject();
                    HTuple recCoord = roiList2[Sid][i].getModelData();
                    HTuple CenterR = new HTuple(); HTuple CenterC = new HTuple();
                    if (homMatFix != null)
                    {
                        HOperatorSet.AffineTransPoint2d(homMatFix, recCoord[0], recCoord[1], out CenterR, out CenterC);
                    }
                    else
                    {
                        CenterR = recCoord[0];
                        CenterC = recCoord[1];
                    }

                    double EndR = CenterR + 100 * Math.Sin(fParam[Sid].roiP[i].phi);
                    double EndC = CenterC + 100 * Math.Cos(fParam[Sid].roiP[i].phi);

                    HTuple isOverlapping = new HTuple();
                    HOperatorSet.IntersectionLines(CenterR, CenterC, EndR, EndC, Rowbg, Colbg, RowEd, ColEd, out row, out col, out isOverlapping);

                    HTuple lineCoord = Rowbg.TupleConcat(Colbg).TupleConcat(RowEd).TupleConcat(ColEd);
                    Lines.Add(lineCoord);

                    if (hwind != null && debug)
                    {
                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(line, "red");
                            hwind.viewWindow.displayHobject(lineEdge, "red");
                        };
                        hwind.Invoke(sw);
                    }
                }

                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "FindPoint error:" + ex.Message + RowNum;
            }
        }


        public string FindPoint(int SideId, HObject HeightImage, out List<HTuple> HLines, HWindow_Final hwind = null, bool debug = false, bool ShowFeatures = true)
        {
            //HObject RIntesity = new HObject(), RHeight = new HObject();
            StringBuilder Str = new StringBuilder();
            HLines = new List<HTuple>();

            double[][] RowCoord = null; double[][] ColCoord = null; double[][] ZCoord = null; string[][] StrLineOrCircle = null;
            try
            {
                int Sid = SideId - 1;
                string ok1 = GenProfileCoord(Sid + 1, HeightImage, out RArray, out Row, out CArray, out Phi, out Ignore, null);
                if (ok1 != "OK")
                {
                    return ok1;
                }
                int Len = roiList2[Sid].Count;// 区域数量
                if (Len == 0)
                {

                    //RIntesity.Dispose();
                    //RHeight.Dispose();
                    return "参数设置错误";
                }
                RowCoord = new double[Len][]; ColCoord = new double[Len][]; ZCoord = new double[Len][]; StrLineOrCircle = new string[Len][];
                double[][] RowCoordt = new double[Len][]; double[][] ColCoordt = new double[Len][]; /*double[][] ZCoordt = new double[Len][];*/

                int Num = 0; int Add = 0; HTuple NewZ = new HTuple();
                for (int i = 0; i < Len; i++)
                {

                    HTuple row = new HTuple(), col = new HTuple();

                    for (int j = Num; j < Add + fParam[Sid].roiP[i].NumOfSection; j++)
                    {
                        if (Num == 99)
                        {
                            Debug.WriteLine(Num);
                        }
                        //Debug.WriteLine(Num);
                        HTuple row1, col1; HTuple anchor, anchorc;
                        if (fParam[Sid].roiP[i].SelectedType == 0)
                        {
                            if (fParam[Sid].roiP[i].TopDownDist != 0 && fParam[Sid].roiP[i].xDist != 0)
                            {
                                string ok = FindMaxPtFallDown(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
                            }
                            else
                            {
                                string ok = FindMaxPt(Sid + 1, j, out row1, out col1, out anchor, out anchorc);

                            }
                        }
                        else
                        {
                            //取最高点下降
                            string ok = FindMaxPtFallDown(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
                        }



                        row = row.TupleConcat(row1);
                        col = col.TupleConcat(col1);
                        Num++;
                    }
                    Add = Num;
                    if (row.Length < 2)
                    {
                        //return "区域" + (i + 1).ToString() + "拟合点数过少";
                        HLines.Add(new HTuple());
                        continue;
                    }



                    if (row.Length == 0) //区域之外
                    {
                        continue;
                    }
                    HObject siglePart = new HObject();

                    int Clipping = 0;
                    int iNum = row.Length;
                    double clipping = fParam[Sid].roiP[i].ClippingPer / 100;
                    Clipping = (int)(iNum * clipping);
                    if (Clipping == iNum / 2)
                    {
                        Clipping = iNum / 2 - 1;
                    }
                    ////直线段拟合
                    //if (fParam[Sid].roiP[i].LineOrCircle != "圆弧段")
                    //{
                    //除去偏离较大点
                    HTuple lineAngle;
                    lineAngle = 1.571 - fParam[Sid].roiP[i].phi;
                    double angle1 = 1.571 + fParam[Sid].roiP[i].phi;
                    double Smoothcont = fParam[Sid].roiP[i].SmoothCont;
                    string IgnoreStr = IgnorPoint(row, col, angle1, Smoothcont, out row, out col);
                    if (IgnoreStr != "OK")
                    {
                        return "忽略点处理失败" + IgnoreStr;
                    }
                    if (hwind != null && debug)
                    {
                        HObject Cross = new HObject();
                        HOperatorSet.GenCrossContourXld(out Cross, row, col, 5, 0.5);
                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(Cross, "green", false);
                        };
                        hwind.Invoke(sw);
                    }

                    HObject line = new HObject();
                    HOperatorSet.GenContourPolygonXld(out line, row, col);

                    HTuple Rowbg, Colbg, RowEd, ColEd, Nr, Nc, Dist;
                    HOperatorSet.FitLineContourXld(line, "tukey", -1, Clipping, 5, 2, out Rowbg, out Colbg, out RowEd, out ColEd, out Nr, out Nc, out Dist);
                    HOperatorSet.GenContourPolygonXld(out line, Rowbg.TupleConcat(RowEd), Colbg.TupleConcat(ColEd));

                    //取拟合线与ROI中心交点
                    //HObject RoiCenter = new HObject();
                    HTuple recCoord = roiList2[Sid][i].getModelData();
                    HTuple CenterR = new HTuple(); HTuple CenterC = new HTuple();

                    CenterR = recCoord[0];
                    CenterC = recCoord[1];

                    double EndR = CenterR + 100 * Math.Sin(fParam[Sid].roiP[i].phi);
                    double EndC = CenterC + 100 * Math.Cos(fParam[Sid].roiP[i].phi);
                    //HOperatorSet.GenRegionLine(out RoiCenter, fParam[Sid].roiP[i].CenterRow, fParam[Sid].roiP[i].CenterCol, EndR, EndC);
                    //HOperatorSet.GenRegionLine(out line, Rowbg, Colbg, RowEd, ColEd);
                    //if (hwind!=null && debug)
                    //{
                    //    hwind.viewWindow.displayHobject(RoiCenter);
                    //    hwind.viewWindow.displayHobject(line);
                    //}
                    HTuple isOverlapping = new HTuple();
                    HOperatorSet.IntersectionLines(CenterR, CenterC, EndR, EndC, Rowbg, Colbg, RowEd, ColEd, out row, out col, out isOverlapping);

                    double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
                    double Yresolution = MyGlobal.globalConfig.dataContext.yResolution;

                    if (Xresolution == 0)
                    {
                        return "XResolution=0";
                    }
                    double DisX = fParam[Sid].roiP[i].offset * Math.Sin(lineAngle.D) / Xresolution;
                    double DisY = fParam[Sid].roiP[i].offset * Math.Cos(lineAngle.D) / Yresolution;

                    double D = Math.Sqrt(DisX * DisX + DisY * DisY);
                    if (fParam[Sid].roiP[i].offset > 0)
                    {
                        D = -D;
                    }
                    double distR = D * Math.Cos(lineAngle.D);
                    double distC = D * Math.Sin(lineAngle.D);

                    double xOffset = fParam[Sid].roiP[i].Xoffset / Xresolution;
                    double yOffset = fParam[Sid].roiP[i].Yoffset / Yresolution;

                    Rowbg = Rowbg.D - distR + yOffset;
                    RowEd = RowEd.D - distR + yOffset;
                    Colbg = Colbg.D - distC + xOffset;
                    ColEd = ColEd.D - distC + xOffset;

                    HTuple lineCoord = Rowbg.TupleConcat(Colbg).TupleConcat(RowEd).TupleConcat(ColEd);
                    HLines.Add(lineCoord);

                    //row = (Rowbg.D + RowEd.D) / 2;
                    //col = (Colbg.D + ColEd.D) / 2;
                    row = row - distR + yOffset;
                    col = col - distC + xOffset;

                    if (hwind != null && ShowFeatures )
                    {
                        HObject cross1 = new HObject();
                        HOperatorSet.GenCrossContourXld(out cross1, row, col, 30, 0.5);

                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(cross1, "yellow");
                        };

                        hwind.Invoke(sw);
                    }

                    //HTuple linephi = new HTuple();
                    //HOperatorSet.LineOrientation(Rowbg, Colbg, RowEd, ColEd, out linephi);
                    //HTuple deg = -(new HTuple(linephi.D)).TupleDeg();
                    //double tan = Math.Tan(-linephi.D);

                    //int len1 = Math.Abs((int)(Rowbg.D - RowEd.D));
                    //int len2 = Math.Abs((int)(Colbg.D - ColEd.D));
                    //int len = len1 > len2 ? len1 : len2;
                    //HTuple x0 = Colbg.D;
                    //HTuple y0 = Rowbg.D;
                    //double[] newr = new double[len]; double[] newc = new double[len];
                    //for (int m = 0; m < len; m++)
                    //{
                    //    HTuple row1 = new HTuple(); HTuple col1 = new HTuple();
                    //    if (len1 > len2)
                    //    {
                    //        row1 = Rowbg.D > RowEd.D ? y0 - m : y0 + m;
                    //        col1 = Rowbg.D > RowEd.D ? x0 - m / tan : x0 + m / tan;

                    //    }
                    //    else
                    //    {
                    //        row1 = Colbg.D > ColEd.D ? y0 - m * tan : y0 + m * tan;
                    //        col1 = Colbg.D > ColEd.D ? x0 - m : x0 + m;
                    //    }
                    //    newr[m] = row1;
                    //    newc[m] = col1;
                    //}
                    //row = newr;col = newc;

                    //HOperatorSet.GenContourPolygonXld(out line, row, col);


                    if (hwind != null && debug)
                    {

                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(line, "red");
                        };
                        hwind.Invoke(sw);
                    }
                    siglePart = line;
                    //}
                    //else
                    //{
                    //    HObject ArcObj = new HObject();
                    //    HOperatorSet.GenContourPolygonXld(out ArcObj, row, col);
                    //    HTuple Rowbg, Colbg, RowEd, ColEd, Nr1, Nc1, Ptorder;
                    //    HOperatorSet.FitLineContourXld(ArcObj, "tukey", -1, Clipping, 5, 2, out Rowbg, out Colbg, out RowEd, out ColEd, out Nr1, out Nc1, out Ptorder);
                    //    HOperatorSet.GenContourPolygonXld(out ArcObj, Rowbg.TupleConcat(RowEd), Colbg.TupleConcat(ColEd));


                    //    HTuple lineAngle;
                    //    HOperatorSet.AngleLx(Rowbg, Colbg, RowEd, ColEd, out lineAngle);
                    //    double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
                    //    double Yresolution = MyGlobal.globalConfig.dataContext.yResolution;

                    //    if (Xresolution == 0)
                    //    {
                    //        return "XResolution=0";
                    //    }
                    //    double DisX = fParam[Sid].roiP[i].offset * Math.Sin(lineAngle.D) / Xresolution;
                    //    double DisY = fParam[Sid].roiP[i].offset * Math.Cos(lineAngle.D) / Yresolution;

                    //    double D = Math.Sqrt(DisX * DisX + DisY * DisY);
                    //    if (fParam[Sid].roiP[i].offset < 0)
                    //    {
                    //        D = -D;
                    //    }
                    //    double distR = D * Math.Cos(lineAngle.D);
                    //    double distC = D * Math.Sin(lineAngle.D);

                    //    double xOffset = fParam[Sid].roiP[i].Xoffset / Xresolution;
                    //    double yOffset = fParam[Sid].roiP[i].Yoffset / Yresolution;

                    //    Rowbg = Rowbg.D - distR + yOffset;
                    //    RowEd = RowEd.D - distR + yOffset;
                    //    Colbg = Colbg.D - distC + xOffset;
                    //    ColEd = ColEd.D - distC + xOffset;


                    //    row = (Rowbg.D + RowEd.D) / 2 ;
                    //    col = (Colbg.D + ColEd.D) / 2 ;
                    //    HTuple lineCoord = Rowbg.TupleConcat(Colbg).TupleConcat(RowEd).TupleConcat(ColEd);
                    //    HLines.Add(lineCoord);

                    //    if (hwind != null)
                    //    {
                    //        HObject CrossArc = new HObject();
                    //        HOperatorSet.GenCrossContourXld(out CrossArc, row, col, 30, 0.5);
                    //        hwind.viewWindow.displayHobject(CrossArc, "blue");
                    //    }
                    //    if (hwind != null && debug)
                    //    {
                    //        hwind.viewWindow.displayHobject(ArcObj, "red");

                    //    }

                    //    siglePart = ArcObj;
                    //}

                    if (hwind != null && ShowFeatures)
                    {
                        //string[] color = { "red", "blue", "green", "lime green", "black" };
                        if (row.Length != 0)
                        {
                            //Random rad = new Random();
                            //int radi = rad.Next(4);
                            string msg = "";
                            //foreach (var item in DicPointName[Sid])
                            //{
                            //    if (item.Value == i)
                            //    {
                            //        msg = item.Key;
                            //    }
                            //}
                            msg = fParam[Sid].DicPointName[i];

                            Action sw = () =>
                            {
                                hwind.viewWindow.dispMessage("Fix" + msg, "red", row.D, col.D);
                            };
                            hwind.Invoke(sw);

                        }
                    }
                    if (hwind != null && ShowFeatures)
                    {
                        HObject cross1 = new HObject();
                        HOperatorSet.GenCrossContourXld(out cross1, row, col, 30, 0.5);
                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(cross1, "red");
                        };
                        hwind.Invoke(sw);
                    }

                    //加上 x y z 偏移
                    //row = row + fParam[Sid].roiP[i].Yoffset;
                    //col = col + fParam[Sid].roiP[i].Xoffset;


                    //旋转至原图
                    //HOperatorSet.AffineTransPoint2d(homMatRotateInvert, row, col, out row, out col);



                    HTuple AvraRow = new HTuple(), AvraCol = new HTuple(), AvraZ = new HTuple();
                    int PointCount = fParam[Sid].roiP[i].NumOfSection;

                    ////取Z值 
                    //HTuple zcoord = new HTuple();
                    //try
                    //{
                    //    HOperatorSet.GetGrayval(HeightImage, row, col, out zcoord);
                    //}
                    //catch (Exception)
                    //{

                    //    return "偏移点设置在图像之外";
                    //}
                    //

                    if (hwind != null && debug == false)
                    {
                        HObject NewSide = new HObject();
                        HOperatorSet.GenContourPolygonXld(out NewSide, row, col);

                        Action sw = () =>
                        {
                            hwind.viewWindow.displayHobject(NewSide, "red");
                        };
                        hwind.Invoke(sw);
                    }

                    RowCoord[i] = row;
                    ColCoord[i] = col;
                    //ZCoord[i] = zcoord;
                    //StrLineOrCircle[i] = new string[zcoord.Length];


                    //ColCoordt[i] = col;
                    //ZCoordt[i] = zcoord;


                    //StrLineOrCircle[i][0] = fParam[Sid].roiP[i].LineOrCircle == "直线段" ? "1;" : "2;";
                    //for (int n = 1; n < zcoord.Length; n++)
                    //{

                    //    if (n == zcoord.Length - 1 && i == Len - 1 && Sid != 3) //最后一段 第四边不给
                    //    {
                    //        StrLineOrCircle[i][n] = StrLineOrCircle[i][0];
                    //    }
                    //    else
                    //    {
                    //        StrLineOrCircle[i][n] = "0;";

                    //    }
                    //}

                    //NewZ = NewZ.TupleConcat(ZCoord[i][0]);
                }
                //HTuple zId = NewZ.TupleGreaterElem(-10);
                //zId = zId.TupleFind(1);
                //NewZ = NewZ[zId];
                //for (int i = 0; i < ZCoord.GetLength(0); i++)
                //{
                //    if (ZCoord[i] == null)
                //    {
                //        return "Find Point 第" + (i + 1).ToString() + "点未取到";
                //    }
                //    if (ZCoord[i][0] == -30)
                //    {

                //        ZCoord[i][0] = NewZ.TupleMean();
                //    }
                //}
                //RIntesity.Dispose();
                //RHeight.Dispose();
                return "OK";
            }
            catch (Exception ex)
            {

                //RIntesity.Dispose();
                //RHeight.Dispose();
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "FindPoint error:" + ex.Message + RowNum;
            }
        }

        string IgnorPoint(HTuple row, HTuple col, double Phi, double SmoothCont, out HTuple Row, out HTuple Col)
        {
            Row = new HTuple(); Col = new HTuple();
            try
            {
                HTuple RowMean, ColMean, RowMedian, ColMedian;
                HOperatorSet.TupleMean(row, out RowMean);
                HOperatorSet.TupleMean(col, out ColMean);
                HOperatorSet.TupleMedian(row, out RowMedian);
                HOperatorSet.TupleMedian(col, out ColMedian);
                double SubR = Math.Abs(RowMean.D - RowMedian.D);
                double SubC = Math.Abs(ColMean.D - ColMedian.D);
                double Sin = Math.Sin(Phi);
                double Cos = Math.Cos(Phi);
                double Standard = SubR * Cos + SubC * Sin;

                int Num = (int)(row.Length * SmoothCont);
                if (SmoothCont == 0 || SmoothCont == 1)
                {
                    Row = row;
                    Col = col;
                    return "OK";
                }
                HTuple SUB = new HTuple();
                for (int i = 0; i < row.Length; i++)
                {
                    double rowsub = Math.Abs(row[i].D - RowMedian.D);
                    double colsub = Math.Abs(col[i].D - ColMedian.D);
                    //double sub1 = rowsub * Cos + colsub * Sin;
                    //SUB = SUB.TupleConcat(sub1);
                    double Add = rowsub * Cos * rowsub * Cos + colsub * Sin * colsub * Sin;
                    double Sqrt = Math.Sqrt(Add);
                    SUB = SUB.TupleConcat(Sqrt);
                }
                HTuple NewId = new HTuple();
                HTuple Less = new HTuple();
                HTuple Indices = new HTuple();
                HOperatorSet.TupleSortIndex(SUB, out Indices);
                HTuple selected = new HTuple();
                HOperatorSet.TupleSelectRange(Indices, row.Length - Num, row.Length - 1, out selected);
                HOperatorSet.TupleRemove(row, selected, out Row);
                HOperatorSet.TupleRemove(col, selected, out Col);
                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "IgnorPoint error" + ex.Message + RowNum;
            }
        }

    }
}
