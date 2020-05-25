using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using HalconDotNet;
using ChoiceTech.Halcon.Control;
using ViewWindow.Model;
using System.Collections;
using System.IO;

namespace SagensVision.VisionTool
{
    public partial class FindMax : DevExpress.XtraEditors.XtraForm
    {
        LookForCircleLine.mainUtl CircleLine;
        public List<ROI> roiList = new List<ROI>();
        ROIController roiController;
        List<int> roiInd = new List<int>();
        public InputParam inParam = new InputParam();
        string Path = AppDomain.CurrentDomain.BaseDirectory + "Setting";
        string settingPath = "";
        string roiPath = "";
        int station = 1;
        HObject OrigImage = new HObject();
        public FindMaxResult fResult = new FindMaxResult();
        public FindMax(int Station = 1)
        {
            InitializeComponent();
            station = Station;
            Path = AppDomain.CurrentDomain.BaseDirectory + "Setting" + Station.ToString() + "\\";
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            if (Station==1)
            {
                this.Text = "工位一感光片设置";
            }
            else
            {
                this.Text = "工位二感光片设置";
            }


            CircleLine = new LookForCircleLine.mainUtl(Path);
            panelControl1.Controls.Add(CircleLine);
            Size sz = CircleLine.Size;
            panelControl1.Size = sz;
            CircleLine.Dock = DockStyle.Fill;
            CircleLine.isFitWindow = false;

            int height = panelControl1.Size.Height;
            int width = (int)(height * (0.006 / 0.05) * 5);

            CircleLine.splitContainer1.SplitterDistance = width;
            //splitContainerControl1.SplitterPosition = sz.Width;
            roiController = CircleLine.hWindow_Final1.viewWindow._roiController;
            roiController.NotifyRCObserver = new IconicDelegate(ROiMove);
            UpDownBase upDown = (UpDownBase)numericUpDown1;
            upDown.TextChanged += new EventHandler(numericUpDown1_ValueChanged);                  
            UpDownBase upDown2 = (UpDownBase)numericUpDown2;
            upDown2.TextChanged += new EventHandler(numericUpDown2_ValueChanged);
            UpDownBase upDown3 = (UpDownBase)numericUpDown3;
            upDown3.TextChanged += new EventHandler(numericUpDown3_ValueChanged);
            UpDownBase upDown4 = (UpDownBase)numericUpDown4;
            upDown4.TextChanged += new EventHandler(numericUpDown4_ValueChanged);

            //settingPath = Path + "MaxSetting.xml";
            roiPath = Path + "Roi.roi";
            //if (File.Exists(settingPath))
            //{
            //    inParam = (InputParam)StaticOperate.ReadXML(settingPath, inParam.GetType());

            //}
            inParam = LoadXml(Station);
            ShowToUI();

            CircleLine.load = new LookForCircleLine.mainUtl.LoadImage(LoadImage);
        }

        public class FindMaxResult
        {
            public HTuple Max = new HTuple();
            public bool DetectOK = false;
        }
        public InputParam LoadXml(int Station)
        {
            InputParam inParam = new InputParam();
            string Path = AppDomain.CurrentDomain.BaseDirectory + "Setting" + Station.ToString() + "\\";
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            settingPath = Path + "MaxSetting.xml";
            if (File.Exists(settingPath))
            {     
                inParam = (InputParam)StaticOperate.ReadXML(settingPath, inParam.GetType());
            }
            return inParam;
        }

        public List<ROI> LoadROI(int Station)
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory + "Setting" + Station.ToString() + "\\";
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            roiPath = Path + "Roi.roi";
            if (File.Exists(roiPath))
            {
                CircleLine.hWindow_Final1.viewWindow.loadROI(roiPath, out roiList);

            }
            return roiList;
        }

        public class InputParam
        {
            public double BaseOffset = 0.05;
            public double PlateWidth = 0.1;
            public double UpLimits = 0.2;
            public double DownLimits = -0.2;
        }

        public void ROiMove(int value)
        {
            try
            {
                switch (value)
                {
                    //case ROIController.EVENT_CHANGED_ROI_SIGN:
                    //case ROIController.EVENT_DELETED_ACTROI:
                    //case ROIController.EVENT_DELETED_ALL_ROIS:
                    case ROIController.EVENT_UPDATE_ROI:

                        //MessageBox.Show("");
                        //ShowSection();
                        break;
                    //case HWndCtrl.ERR_READING_IMG:
                    //    MessageBox.Show("Problem occured while reading file! \n", "Profile ",
                    //        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void LoadImage()
        {
            if (File.Exists(MyGlobal.ModelPath + (station -1).ToString() + "Origin.tiff"))
            {
                HOperatorSet.ReadImage(out OrigImage, MyGlobal.ModelPath + (station - 1).ToString() + "Origin.tiff");
            }
            else
            {
                MessageBox.Show("高度图 Origin 加载失败");
                return;
            }
           
            rec2Count = 0;
            roiInd.Clear();
            roiList.Clear();
            if (File.Exists(roiPath))
            {
                CircleLine.hWindow_Final1.viewWindow.loadROI(roiPath, out roiList);
                roiInd.Clear();
               
                ArrayList tempRoi = roiController.ROIList;
                int count = tempRoi.Count;
                int sub = count - roiList.Count + 2;
                for (int i = sub; i < tempRoi.Count; i++)
                {
                    roiInd.Add(i);
                }
                if (roiList.Count > 2)
                {
                    rec2Count = 2;
                }
                else
                {
                    rec2Count = roiList.Count;
                }
            }

        }

        void ShowToUI()
        {
            numericUpDown1.Value = (decimal) inParam.PlateWidth;
            numericUpDown2.Value = (decimal)inParam.BaseOffset;
            numericUpDown3.Value = (decimal)inParam.UpLimits;
            numericUpDown4.Value = (decimal)inParam.DownLimits;

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (CircleLine.hWindow_Final1.Image==null )
                {
                  
                    return;
                }

                roiList.Clear();
                ArrayList tempRoi = roiController.ROIList;
                for (int i = 0; i < tempRoi.Count; i++)
                {
                    ROI roi = (ROI)tempRoi[i];
                    if (roi.Type == "ROIRectangle1")
                    {
                        roi.Msize = new Size(1, 1);
                        roiList.Add(roi);
                    }
                }
                if (roiList.Count==0)
                {
                    MessageBox.Show("请先添加凸台基准");
                    return;
                }
                HObject line=new HObject();
                HObject Image = CircleLine.hWindow_Final1.Image;
                //PreHandle(Image, out Image);
                UpdateRoi(Image);
                //List<ROI> temp = new List<ROI>();
                //List<ROI> temp2 = new List<ROI>();

                ////ArrayList tempRoi = roiController.ROIList;
                //if (roiList.Count > 0)
                //{
                //    for (int i = 0; i < 2; i++)
                //    {
                //        ROI roi = (ROI)tempRoi[i];
                //        temp.Add(roi);
                //    }
                //    for (int i = 2; i < tempRoi.Count; i++)
                //    {
                //        ROI roi = (ROI)tempRoi[i];
                //        temp2.Add(roi);
                //    }
                //}
                //CircleLine.hWindow_Final1.HobjectToHimage(Image);
                //roiList = temp2;
                //CircleLine.roiList = temp;
                //CircleLine.hWindow_Final1.viewWindow.displayROI(ref temp);
                //CircleLine.hWindow_Final1.viewWindow.displayROI(ref roiList);


                string ok = FitLine(CircleLine.hWindow_Final1.Image, CircleLine.hWindow_Final1,out line);
                if (ok!="OK")
                {
                    MessageBox.Show(ok);
                }
               
                //HOperatorSet.WriteImage(Image, "tiff", 0, MyGlobal.ModelPath + (station -1).ToString()+".tiff");
            }
            catch (Exception)
            {

                throw;
            }
        }

        void UpdateRoi(HObject Image)
        {
            List<ROI> temp = new List<ROI>();
            List<ROI> temp2 = new List<ROI>();

            ArrayList tempRoi = roiController.ROIList;
            if (roiList.Count > 0)
            {
                
                for (int i = 0; i < 2; i++)
                {
                    
                    ROI roi = (ROI)tempRoi[i];
                    roi.Msize = new Size(4, 4);
                    temp.Add(roi);
                }
                for (int i = 2; i < tempRoi.Count; i++)
                {
                    ROI roi = (ROI)tempRoi[i];
                    roi.Msize = new Size(2, 2);
                    temp2.Add(roi);
                }
            }

            CircleLine.hWindow_Final1.HobjectToHimage(Image);
            roiList = temp2;
            CircleLine.roiList = temp;
            CircleLine.hWindow_Final1.viewWindow.displayROI(ref temp);
            CircleLine.hWindow_Final1.viewWindow.displayROI(ref roiList);
        }

        public void scale_image_range(HObject ho_Image, out HObject ho_ImageScaled, HTuple hv_Min,
    HTuple hv_Max)
        {

            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_SelectedChannel = null, ho_LowerRegion = null;
            HObject ho_UpperRegion = null;

            // Local copy input parameter variables 
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = ho_Image.CopyObj(1, -1);


            HTuple hv_LowerLimit = new HTuple(), hv_UpperLimit = new HTuple();
            HTuple hv_Mult = null, hv_Add = null, hv_Channels = null;
            HTuple hv_Index = null, hv_MinGray = new HTuple(), hv_MaxGray = new HTuple();
            HTuple hv_Range = new HTuple();
            HTuple hv_Max_COPY_INP_TMP = hv_Max.Clone();
            HTuple hv_Min_COPY_INP_TMP = hv_Min.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_SelectedChannel);
            HOperatorSet.GenEmptyObj(out ho_LowerRegion);
            HOperatorSet.GenEmptyObj(out ho_UpperRegion);

            //
            if ((int)(new HTuple((new HTuple(hv_Min_COPY_INP_TMP.TupleLength())).TupleEqual(
                2))) != 0)
            {
                hv_LowerLimit = hv_Min_COPY_INP_TMP[1];
                hv_Min_COPY_INP_TMP = hv_Min_COPY_INP_TMP[0];
            }
            else
            {
                hv_LowerLimit = 0.0;
            }
            if ((int)(new HTuple((new HTuple(hv_Max_COPY_INP_TMP.TupleLength())).TupleEqual(
                2))) != 0)
            {
                hv_UpperLimit = hv_Max_COPY_INP_TMP[1];
                hv_Max_COPY_INP_TMP = hv_Max_COPY_INP_TMP[0];
            }
            else
            {
                hv_UpperLimit = 255.0;
            }
            //
            //Calculate scaling parameters
            hv_Mult = (((hv_UpperLimit - hv_LowerLimit)).TupleReal()) / (hv_Max_COPY_INP_TMP - hv_Min_COPY_INP_TMP);
            hv_Add = ((-hv_Mult) * hv_Min_COPY_INP_TMP) + hv_LowerLimit;
            //
            //Scale image
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ScaleImage(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0, hv_Mult, hv_Add);
                ho_Image_COPY_INP_TMP.Dispose();
                ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
            }
            //
            //Clip gray values if necessary
            //This must be done for each channel separately
            HOperatorSet.CountChannels(ho_Image_COPY_INP_TMP, out hv_Channels);
            HTuple end_val48 = hv_Channels;
            HTuple step_val48 = 1;
            for (hv_Index = 1; hv_Index.Continue(end_val48, step_val48); hv_Index = hv_Index.TupleAdd(step_val48))
            {
                ho_SelectedChannel.Dispose();
                HOperatorSet.AccessChannel(ho_Image_COPY_INP_TMP, out ho_SelectedChannel, hv_Index);
                HOperatorSet.MinMaxGray(ho_SelectedChannel, ho_SelectedChannel, 0, out hv_MinGray,
                    out hv_MaxGray, out hv_Range);
                ho_LowerRegion.Dispose();
                HOperatorSet.Threshold(ho_SelectedChannel, out ho_LowerRegion, ((hv_MinGray.TupleConcat(
                    hv_LowerLimit))).TupleMin(), hv_LowerLimit);
                ho_UpperRegion.Dispose();
                HOperatorSet.Threshold(ho_SelectedChannel, out ho_UpperRegion, hv_UpperLimit,
                    ((hv_UpperLimit.TupleConcat(hv_MaxGray))).TupleMax());
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.PaintRegion(ho_LowerRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                        hv_LowerLimit, "fill");
                    ho_SelectedChannel.Dispose();
                    ho_SelectedChannel = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.PaintRegion(ho_UpperRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                        hv_UpperLimit, "fill");
                    ho_SelectedChannel.Dispose();
                    ho_SelectedChannel = ExpTmpOutVar_0;
                }
                if ((int)(new HTuple(hv_Index.TupleEqual(1))) != 0)
                {
                    ho_ImageScaled.Dispose();
                    HOperatorSet.CopyObj(ho_SelectedChannel, out ho_ImageScaled, 1, 1);
                }
                else
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.AppendChannel(ho_ImageScaled, ho_SelectedChannel, out ExpTmpOutVar_0
                            );
                        ho_ImageScaled.Dispose();
                        ho_ImageScaled = ExpTmpOutVar_0;
                    }
                }
            }
            ho_Image_COPY_INP_TMP.Dispose();
            ho_SelectedChannel.Dispose();
            ho_LowerRegion.Dispose();
            ho_UpperRegion.Dispose();

            return;
        }

       public string PreHandle(HObject Image,out HObject scaleImage)
        {
            scaleImage = new HObject();
            try
            {
                if (roiList.Count > 0)
                {
                    HRegion region = roiList[0].getRegion();
                    HTuple grayValue, regionRow, regionCol, NeqId, Neq;
                    HOperatorSet.GetRegionPoints(region, out regionRow, out regionCol);
                    HOperatorSet.GetGrayval(Image, regionRow, regionCol, out grayValue);
                    HOperatorSet.TupleNotEqualElem(grayValue, -30, out Neq);
                    HOperatorSet.TupleFind(Neq, 1, out NeqId);
                    HTuple NewGray = grayValue[NeqId];
                    HTuple min, max;
                    HOperatorSet.TupleMin(NewGray, out min);
                    HOperatorSet.TupleMax(NewGray, out max);
                    scale_image_range(Image, out scaleImage, min, max);
                    HOperatorSet.ScaleImageMax(scaleImage, out scaleImage);
                }
                else
                {
                    scaleImage = Image;
                }
                return "OK";
            }
            catch (Exception ex)
            {

                return "PreHandle "+ ex.Message;
            }
            
        }

        string  FitLine(HObject Image,HWindow_Final hWindow_Final, out HObject line, HTuple Homat2D = null)
        {
            line = new HObject();
            List<HTuple> row = new List<HTuple>(); List<HTuple> col = new List<HTuple>();
            HTuple[] coord0 = new HTuple[2], coord1 = new HTuple[2], coord2 = new HTuple[2], coord3 = new HTuple[2];
            if (Homat2D!=null)
            {
                if (Homat2D.Length!=0)
                {                   
                    for (int i = 0; i < CircleLine.roiparamList.Count; i++)
                    {                                             
                             coord0[i] = CircleLine.roiparamList[i].roi_row_start;
                             coord2[i] = CircleLine.roiparamList[i].roi_row_end;
                             coord1[i] = CircleLine.roiparamList[i].roi_col_start;
                             coord3[i] = CircleLine.roiparamList[i].roi_col_end;

                        HTuple affinePx, affinePy, affinePx1, affinePy1;
                            HOperatorSet.AffineTransPoint2d(Homat2D, coord0[i], coord1[i], out affinePx, out affinePy);
                            HOperatorSet.AffineTransPoint2d(Homat2D, coord2[i], coord3[i], out affinePx1, out affinePy1);
                        CircleLine.roiparamList[i].roi_row_start = affinePx;
                        CircleLine.roiparamList[i].roi_row_end = affinePx1;
                        CircleLine.roiparamList[i].roi_col_start = affinePy;
                        CircleLine.roiparamList[i].roi_col_end = affinePy1;
                    }
                   
                }
            }
            LookForCircleLine.OutputPoint output = new LookForCircleLine.OutputPoint();
            output.GetLcPoint(Image,CircleLine.roiparamList, out row, out col, hWindow_Final);
            if (coord0[0]!=null)
            {
                for (int i = 0; i < CircleLine.roiparamList.Count; i++)
                {
                    CircleLine.roiparamList[i].roi_row_start = coord0[i];
                    CircleLine.roiparamList[i].roi_row_end = coord2[i];
                    CircleLine.roiparamList[i].roi_col_start = coord1[i];
                    CircleLine.roiparamList[i].roi_col_end = coord3[i];
                }
            }
            
           

            HTuple Row = new HTuple(); HTuple Col = new HTuple();
            if (row.Count == 2)
            {
                if (row[0].Length > 0 && row[1].Length > 0)
                {
                    Row = row[0].TupleConcat(row[1]);
                    Col = col[0].TupleConcat(col[1]);
                    HObject Contour = new HObject();
                    HOperatorSet.GenContourPolygonXld(out Contour, Row, Col);                  
                    HTuple rowBg, colBg, rowEd, colEd,Nr,Nc,Dist;
                    HOperatorSet.FitLineContourXld(Contour, "tukey", -1, 0, 5, 2, out rowBg, out colBg, out rowEd, out colEd, out Nr, out Nc, out Dist);
                    HOperatorSet.GenContourPolygonXld(out line, rowBg.TupleConcat(rowEd), colBg.TupleConcat(colEd));
                    if (hWindow_Final!=null)
                    {
                        HOperatorSet.DispObj(line, hWindow_Final.HWindowHalconID);
                    }
                    return "OK";
                }
                return "拟合线失败！";
            }
            else
            {
                return "拟合线失败！";
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                if (CircleLine.hWindow_Final1.Image != null && CircleLine.hWindow_Final1.Image.IsInitialized())
                {
                    //HObject image = CircleLine.hWindow_Final1.Image;
                    //HTuple width, height;
                    //HOperatorSet.GetImageSize(image, out width, out height); 
                    if (rec2Count!=2)
                    {
                        MessageBox.Show("请先添加凸台基准区域");
                        return;
                    }  
                    roiController.setROIShape(new ROIRectangle1());
                 
                    ArrayList tempRoi = roiController.ROIList;                   
                    int count = tempRoi.Count;
                    roiInd.Add(count);               
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (roiInd.Count>0)
                {
                    if (roiInd.Contains(roiController.activeROIidx))
                    {
                        roiInd.RemoveAt(roiInd.Count-1);
                        if (roiList.Count>0)
                        {
                            roiList.RemoveAt(roiInd.Count - 1 + 2);
                        }                       
                        roiController.removeActive();
                        
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (roiInd.Count > 0)
                {                    
                    foreach (var item in roiInd)
                    {
                        roiController.setActiveROIIdx(roiInd[0]);
                        if (roiList.Count>0)
                        {
                            roiList.RemoveAt(roiInd[0] - 2);
                        }                       
                        roiController.removeActive();
                    }
                    roiInd.Clear();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (CircleLine.hWindow_Final1.Image==null || !CircleLine.hWindow_Final1.Image.IsInitialized())
                {
                    return;
                }

                simpleButton3_Click(sender, e);

                roiList.Clear();
                ArrayList tempRoi = roiController.ROIList;
                for (int i = 0; i < tempRoi.Count; i++)
                {
                    ROI roi = (ROI)tempRoi[i];
                    if (roi.Type == "ROIRectangle1")
                    {                        
                        roi.Msize = new Size(1, 1);
                        roiList.Add(roi);
                    }
                }
                HTuple max = new HTuple();

                string ok = RunMaxPoint(CircleLine.hWindow_Final1.Image, OrigImage, CircleLine.hWindow_Final1,new HTuple(),out fResult);                
                MessageBox.Show(ok);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string RunMaxPoint(HObject Image,HObject orgionImage,HWindow_Final hWindow_Final,HTuple Homat,out FindMaxResult fResult)
        {
            fResult = new FindMaxResult();
            try
            {
               
                HObject Line = new HObject();
                //PreHandle(Image, out ScaleImage);
                FitLine(Image, null, out Line,Homat);
               
                //Image = hWindow_Final.Image;
                double xResolution = 0.006;
                if (MyGlobal.GoSDK.context.xResolution !=0)
                {
                    xResolution = MyGlobal.GoSDK.context.xResolution;
                }

                double MoveLeft = (inParam.PlateWidth )/ xResolution;
                double BaseOffset = inParam.BaseOffset/ xResolution;
                HTuple baseZ = new HTuple();
                List<ROI> _roiList = new List<ROI>();
                if (Homat.Length!=0)
                {                    
                    for (int i = 0; i < roiList.Count; i++)
                    {
                        _roiList.Add(new ROI());
                        HTuple coord = roiList[i].getModelData();
                        HTuple affinePx, affinePy, affinePx1, affinePy1;
                        HOperatorSet.AffineTransPoint2d(Homat, coord[0], coord[1], out affinePx, out affinePy);
                        HOperatorSet.AffineTransPoint2d(Homat, coord[2], coord[3], out affinePx1, out affinePy1);
                        ROI roi = new ROIRectangle1(affinePx, affinePy, affinePx1, affinePy1);
                        _roiList[i] = roi;
                    }
                }
                else
                {
                    _roiList = roiList;
                }
                if (_roiList.Count>2)
                {
                    HTuple grayValue = new HTuple();
                    for (int i = 0; i < 2; i++)
                    {
                        HTuple recR, recC;
                        HRegion temp = _roiList[i].getRegion();
                        HOperatorSet.GetRegionPoints(temp, out recR, out recC);
                        hWindow_Final.viewWindow.displayHobject(temp, "green", true);
                        HTuple GrayValue = new HTuple();

                        HTuple width, height;
                        HOperatorSet.GetImageSize(Image, out width, out height);
                        HTuple newReq = recR.TupleLessElem(height);
                        HTuple newCeq = recC.TupleLessElem(width);
                        HTuple Rid = newReq.TupleFind(1);
                        HTuple Cid = newCeq.TupleFind(1);
                        HTuple id = Rid.TupleIntersection(Cid);
                        if (id.Length != 0)
                        {
                            recR = recR[id];
                            recC = recC[id];

                            HOperatorSet.GetGrayval(orgionImage, recR, recC, out GrayValue);
                            HTuple newGray = GrayValue.TupleGreaterElem(-30);
                            HTuple NeqId = new HTuple();
                            HOperatorSet.TupleFind(newGray, 1, out NeqId);
                            GrayValue = GrayValue[NeqId];
                            HOperatorSet.TupleSort(GrayValue, out GrayValue);
                            int len = GrayValue.Length;
                            int len10 = (int)(len * 0.05);
                            GrayValue = GrayValue.TupleSelectRange(len10, len - 1);
                            GrayValue = GrayValue.TupleSelectRange(0, GrayValue.Length - len10);
                            HTuple mean = GrayValue.TupleMean();
                            grayValue = grayValue.TupleConcat(mean);
                        }
                        else
                        {
                            return "基准区域超出图像";
                        }
                    }
                    baseZ = grayValue.TupleMean();
                }

                HTuple MaxZ = new HTuple();
                for (int i = 2; i < _roiList.Count; i++)
                {
                    HRegion temp = _roiList[i].getRegion();
                    HObject regionContour = new HObject();
                    HOperatorSet.GenContourRegionXld(temp, out regionContour, "border");
                    HTuple intersectR, intersectC,isOverlapping;

                    //hWindow_Final.viewWindow.displayHobject(regionContour, "red", true);

                    HOperatorSet.IntersectionContoursXld(regionContour, Line, "mutual", out intersectR, out intersectC, out isOverlapping);
                    if (intersectR.Length>1)
                    {
                        HTuple Coord = _roiList[i].getModelData();
                        HTuple colLeft = intersectC[0] - MoveLeft;
                        HTuple rowLeft = Coord[0];
                        HTuple colRight = intersectC[1] + BaseOffset;
                        HTuple rowRight = Coord[2];
                        HObject rec = new HObject();
                        if (colRight.D<= colLeft.D)
                        {
                            colRight = colLeft.D + 1;

                        }
                        HOperatorSet.GenRectangle1(out rec, rowLeft, colLeft, rowRight, colRight);
                        //HOperatorSet.SetColor(hWindow_Final.HWindowHalconID, "red");
                        //HOperatorSet.SetDraw(hWindow_Final.HWindowHalconID, "fill");
                        //HOperatorSet.DispObj(rec, hWindow_Final.HWindowHalconID);
                        hWindow_Final.viewWindow.displayHobject(rec, "red", true);
                        HTuple recR, recC;
                        HOperatorSet.GetRegionPoints(rec, out recR, out recC);
                        HTuple GrayValue = new HTuple();
                        HTuple width, height;
                        HOperatorSet.GetImageSize(Image, out width, out height);
                        HTuple newReq = recR.TupleLessElem(height);
                        HTuple newCeq = recC.TupleLessElem(width);
                        HTuple Rid = newReq.TupleFind(1);
                        HTuple Cid = newCeq.TupleFind(1);
                        HTuple id = Rid.TupleIntersection(Cid);
                        if (id.Length != 0)
                        {
                            recR = recR[id];
                            recC = recC[id];
                            HOperatorSet.GetGrayval(orgionImage, recR, recC, out GrayValue);
                        }
                        else
                        {
                            return "感光片区域超出图像";
                        }
                        HTuple newGray = GrayValue.TupleFind(-30);
                        GrayValue = GrayValue.TupleRemove(newGray);
                        HOperatorSet.TupleSort(GrayValue, out GrayValue);
                        int len = GrayValue.Length;
                        int len10 = (int)(len * 0.05);
                        //GrayValue = GrayValue.TupleSelectRange(len10, len - 1);
                        GrayValue = GrayValue.TupleSelectRange(GrayValue.Length - len10*3 - 1, GrayValue.Length - len10);

                        HTuple max = GrayValue.TupleMean();
                        HTuple subMax = max.D - baseZ.D;
                        double maxSub = Math.Round(subMax.D, 3);
                        MaxZ = MaxZ.TupleConcat(maxSub);
                        HOperatorSet.SetColor(hWindow_Final.HWindowHalconID, "blue");
                        HOperatorSet.SetFont(hWindow_Final.HWindowHalconID, "-Arial - 20 -");
                        HOperatorSet.SetTposition(hWindow_Final.HWindowHalconID, i*10, 10);
                        HOperatorSet.WriteString(hWindow_Final.HWindowHalconID, maxSub.ToString());

                    }                   

                }
                if (MaxZ.Length==0)
                {
                    return "遮光片区域未找到";
                }
                fResult.Max = MaxZ;
                HTuple maxAll = MaxZ.TupleMax();
                if (maxAll.D > inParam.DownLimits && maxAll.D<inParam.UpLimits)
                {
                    fResult.DetectOK = true;
                    HOperatorSet.SetColor(hWindow_Final.HWindowHalconID, "green");
                    HOperatorSet.SetFont(hWindow_Final.HWindowHalconID, "-Arial - 50 -");
                    HOperatorSet.SetTposition(hWindow_Final.HWindowHalconID, 0, 10);
                    HOperatorSet.WriteString(hWindow_Final.HWindowHalconID, "OK");
                }
                else
                {
                    fResult.DetectOK = false;
                    HOperatorSet.SetColor(hWindow_Final.HWindowHalconID, "red");
                    HOperatorSet.SetFont(hWindow_Final.HWindowHalconID, "-Arial - 50 -");
                    HOperatorSet.SetTposition(hWindow_Final.HWindowHalconID, 0, 10);
                    HOperatorSet.WriteString(hWindow_Final.HWindowHalconID, "NG");
                }
                return "OK";

            }
            catch (Exception ex)
            {

                return "RunMaxPoint" + ex.Message;
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                roiList.Clear();
                ArrayList tempRoi = roiController.ROIList;
                for (int i = 0; i < tempRoi.Count; i++)
                {
                    ROI roi = (ROI)tempRoi[i];
                    if (roi.Type == "ROIRectangle1")
                    {
                        roi.Msize = new Size(4, 4);
                        roiList.Add(roi);
                    }
                }

                StaticOperate.WriteXML(inParam, settingPath);
                CircleLine.hWindow_Final1.viewWindow.saveROI(roiList, roiPath);
                inParam = LoadXml(station);
                MessageBox.Show("保存成功！");
            }
            catch (Exception)
            {

                throw;
            }
        }
        int rec2Count = 0;
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            try
            {
                if (CircleLine.hWindow_Final1.Image == null || !CircleLine.hWindow_Final1.Image.IsInitialized())
                {
                    return;
                }
               
                //ArrayList tempRoi = roiController.ROIList;
                //for (int i = 0; i < tempRoi.Count; i++)
                //{
                //    ROI roi = (ROI)tempRoi[i];
                //    if (roi.Type == "ROIRectangle2")
                //    {
                //        rec2Count++;
                //    }
                //}
                if ( rec2Count >=2)
                {
                    return;
                }
                roiController.setROIShape(new ROIRectangle1());
              
                rec2Count++;
            }
            catch (Exception)
            {

                throw;
            }
        }

    

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            inParam.BaseOffset = (double)numericUpDown2.Value;

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
            inParam.PlateWidth = (double)numericUpDown1.Value;

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            inParam.UpLimits = (double)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            inParam.DownLimits = (double)numericUpDown4.Value;

        }

        
        private void simpleButton8_Click(object sender, EventArgs e)
        {

        }
    }
}