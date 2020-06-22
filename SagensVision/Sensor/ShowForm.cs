using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Threading;
using HalconDotNet;
using System.Text.RegularExpressions;
using ViewWindow.Model;
namespace SagensVision.Sensor
{
    public partial class ShowForm : DevExpress.XtraEditors.XtraForm
    {
        ShowWindow sw = new ShowWindow();
        ROIController roiController;
        SagensSdk.DataContext context = new SagensSdk.DataContext();
        public ShowForm()
        {
            InitializeComponent();
            splitContainerControl1.Panel2.Controls.Add(sw);
            sw.Dock = DockStyle.Fill;
           
            MyGlobal.GoSDK.MeasurementRecFinish += GoSDK_MeasurementRecFinish;
            textBox_Current.Text = "0";
            textBox_Total.Text = "0";

            roiController = sw.hwnd[1].viewWindow._roiController;
            roiController.NotifyRCObserver = new IconicDelegate(ROiMove);
            context = MyGlobal.GoSDK.context;
        }

        List<HObject> HeightImage = new List<HObject>();
        List<HObject> IntensityImage = new List<HObject>();
        int CurrentIndex = 0;
      
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
                        ShowSection();
                        break;
                    case HWndCtrl.ERR_READING_IMG:
                        MessageBox.Show("Problem occured while reading file! \n", "Profile ",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        static class ShowWitch
        {
            public static bool Online = false;
            public static bool ShowProfile = false;
            public static bool ShowHPicture = false;
            public static bool ShowIPicture = false;
            public static bool SaveCache = false;
        }
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sw.ResetWindow();
        }

        private void barToggleSwitchItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {          
            BarToggleSwitchItem barSwitch = (BarToggleSwitchItem)sender;
            switch (barSwitch.Name)
            {
                case "barToggleSwitchItem1":
                    ShowWitch.Online = barSwitch.Checked;
                    if (ShowWitch.Online)
                    {
                        MyGlobal.GoSDK.SurfaceZRecFinish += GoSDK_SurfaceZRecFinish;
                        MyGlobal.GoSDK.SurfaceIntensityRecFinish += GoSDK_SurfaceIntensityRecFinish;
                        MyGlobal.GoSDK.SigleProfileRecFinish += GoSDK_ProfileRecFinish;
                    }
                    else
                    {
                        MyGlobal.GoSDK.SurfaceZRecFinish -= GoSDK_SurfaceZRecFinish;
                        MyGlobal.GoSDK.SurfaceIntensityRecFinish -= GoSDK_SurfaceIntensityRecFinish;
                        MyGlobal.GoSDK.SigleProfileRecFinish -= GoSDK_ProfileRecFinish;
                    }         
                    break;
                case "barToggleSwitchItem2":
                    ShowWitch.ShowProfile = barSwitch.Checked;
                    if (ShowWitch.ShowProfile)
                    {
                        MyGlobal.GoSDK.EnableProfle = true;
                    }
                    else
                    {
                        MyGlobal.GoSDK.EnableProfle = false;
                    }
                    break;
                case "barToggleSwitchItem3":
                    ShowWitch.ShowHPicture = barSwitch.Checked;
                    break;
                case "barToggleSwitchItem4":
                    ShowWitch.ShowIPicture = barSwitch.Checked;
                    break;
            }
        }

        void GoSDK_SurfaceZRecFinish()
        {
            try
            {
                if (!ShowWitch.ShowHPicture)
                {
                    return;
                }
                float[] SurfacePointZ = MyGlobal.GoSDK.SurfaceDataZ;
                long SurfaceWidth, SurfaceHeight;
                SurfaceWidth = MyGlobal.GoSDK.SurfaceWidth;
                SurfaceHeight = MyGlobal.GoSDK.SurfaceHeight;
                if (SurfacePointZ!=null)
                {
                    HObject Height = new HObject();
                    MyGlobal.GoSDK.GenHalconImage(SurfacePointZ, SurfaceWidth, SurfaceHeight, out Height);
                    sw.hwnd[1].HobjectToHimage(Height);
                    if (ShowWitch.SaveCache)
                    {
                        HeightImage.Add(Height);
                        //Action Show = () => {
                            textBox_Total.Text = HeightImage.Count.ToString();
                            CurrentIndex = IntensityImage.Count;
                            trackBarControl1.Properties.Maximum = CurrentIndex;
                            trackBarControl1.Properties.Minimum = 1;
                            textBox_Current.Text = "1";
                        //};
                        //this.Invoke(Show);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void GoSDK_SurfaceIntensityRecFinish()
        {
            if (!ShowWitch.ShowIPicture)
            {
                return;
            }
            byte[] SurfacePointZ = MyGlobal.GoSDK.SurfaceDataIntensity;
            long SurfaceWidth, SurfaceHeight;
            SurfaceWidth = MyGlobal.GoSDK.SurfaceWidth;
            SurfaceHeight = MyGlobal.GoSDK.SurfaceHeight;
            if (SurfacePointZ != null)
            {
                HObject Intensity = new HObject();
                MyGlobal.GoSDK.GenHalconImage(SurfacePointZ, SurfaceWidth, SurfaceHeight, out Intensity);
                sw.hwnd[2].HobjectToHimage(Intensity);
                if (ShowWitch.SaveCache)
                {
                    IntensityImage.Add(Intensity);
                    Action Show = () =>
                    {
                        textBox_Total.Text = IntensityImage.Count.ToString();
                        CurrentIndex = IntensityImage.Count;
                        trackBarControl1.Properties.Maximum = CurrentIndex;
                        trackBarControl1.Properties.Minimum = 1;
                        textBox_Current.Text = "1";
                    };
                    this.Invoke(Show);
                }
            }
        }
        void GoSDK_MeasurementRecFinish()
        {

        }

        void GoSDK_ProfileRecFinish() 
        {
            if (!ShowWitch.ShowProfile)
            {
                return;
            }
            SagensSdk.Profile profile = MyGlobal.GoSDK.SigleProfile;
            if (profile.points != null)
            {
                ShowSigleProfile(profile);

                if (ShowWitch.SaveCache)
                {
                    int len = MyGlobal.GoSDK.ProfileList.Count;
                    Action Show = () =>
                    {
                        if (ShowWitch.ShowIPicture)
                        {
                            HObject image = new HObject();
                            if (MyGlobal.GoSDK.ProfileList.Count>0)
                            {
                                GenIntesityProfile(MyGlobal.GoSDK.ProfileList, out image);
                                sw.hwnd[2].HobjectToHimage(image);
                            }

                        }

                        textBox_Total.Text = len.ToString();
                        CurrentIndex = len;
                        trackBarControl1.Properties.Maximum = CurrentIndex;
                        trackBarControl1.Properties.Minimum = 1;
                        textBox_Current.Text = "1";
                    };
                    this.Invoke(Show);
                }
                else
                {
                    MyGlobal.GoSDK.ProfileList.Clear();
                }
              
            }

        }

        void ShowSigleProfile(SagensSdk.Profile profile)
        {
            SagensSdk.ProfilePoint[] point = profile.points;
            double[] row, col;
            GetRowCol(point, out row, out col);
            HTuple RowNew, ColNew;
            HObject Region, Contour, ConstImage;
            GenProfile(true, row, col, out RowNew, out ColNew, out Region, out Contour);
            HOperatorSet.GenImageConst(out ConstImage, "byte", 1000, 1000);
            sw.hwnd[0].HobjectToHimage(ConstImage);
            sw.hwnd[0].viewWindow.displayHobject(Region, "red");
        }

        void ShowSection()
        {
            ROI line = roiController.getActiveROI();
            HTuple lineCoord = line.getModelData();
            HObject Rline = new HObject();
            HOperatorSet.GenRegionLine(out Rline, lineCoord[0], lineCoord[1], lineCoord[2], lineCoord[3]);
            HTuple row, col;
            HObject Rline1 = new HObject();
            HOperatorSet.GenContourRegionXld(Rline, out Rline1, "border");
            HOperatorSet.GetContourXld(Rline1, out row, out col);
            HTuple Zpoint = new HTuple(),Xpoint = new HTuple();
            if (row.Length==0)
            {
                return;
            }
         
            HTuple width, height,lessId1, lessId2;
            HOperatorSet.GetImageSize(sw.hwnd[1].Image, out width, out height);
            HOperatorSet.TupleLessElem(row, height, out lessId1);
            HOperatorSet.TupleFind(lessId1, 1, out lessId1);
           
            row = row[lessId1];
            col = col[lessId1];
            HOperatorSet.TupleLessElem(col, width, out lessId2);
            HOperatorSet.TupleFind(lessId2, 1, out lessId2);
            row = row[lessId2];
            col = col[lessId2];
            HOperatorSet.GetGrayval(sw.hwnd[1].Image, row, col,out Zpoint);
            HTuple EqId = new HTuple();
            HOperatorSet.TupleNotEqualElem(Zpoint, -30, out EqId);
            HOperatorSet.TupleFind(EqId, 1, out EqId);
            Zpoint = Zpoint[EqId];
            HOperatorSet.TupleGenSequence(0, Zpoint.Length,context.xResolution, out Xpoint);
          
            Zpoint = Zpoint *50;
            Xpoint = Xpoint *50;
            HTuple RowNew, ColNew;
            HObject Region, Contour, ConstImage;
            GenProfile(true, Zpoint, Xpoint, out RowNew, out ColNew, out Region, out Contour);
            HOperatorSet.GenImageConst(out ConstImage, "byte", 1000, 1000);
            sw.hwnd[0].HobjectToHimage(ConstImage);
            sw.hwnd[0].viewWindow.displayHobject(Region, "red",true);

            Rline.Dispose();
            Rline1.Dispose();
            ConstImage.Dispose();
            Region.Dispose();
            Contour.Dispose();
        }

        void GenIntesityProfile(List<SagensSdk.Profile> profile,out HObject Image)
        {
            int len = profile.Count;
            int width = profile[0].points.Length;
            byte[] imageArray = new byte[width * len];
            int k = 0;
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    imageArray[k] = profile[i].points[j].Intensity;
                    k++;
                }
            }
            Image = new HObject();
            HOperatorSet.GenEmptyObj(out Image);
            MyGlobal.GoSDK.GenHalconImage(imageArray, width, len, out Image);

        }

        void GetRowCol(SagensSdk.ProfilePoint[] point,out double[] Row,out double[] Col)
        {
            long len = point.Length;
            Row = new double[len];Col = new double[len];
            for (int i = 0; i < len; i++)
            {
                Row[i] = point[i].z/0.006;
                Col[i] = point[i].x/0.006;
            }
        }

        ulong tempEncoder = 0;
        ulong count = 0;
        bool isRun = false;

        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {
            ShowWitch.SaveCache = checkButton1.Checked;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            HeightImage.Clear();
            IntensityImage.Clear();
            MyGlobal.GoSDK.ProfileList.Clear();
            roiController.resetROI();
            textBox_Current.Text = "0";
            textBox_Total.Text = "0";
            trackBarControl1.Properties.Maximum = 0;
            trackBarControl1.Properties.Minimum = 0;
            CurrentIndex = 0;
        }

        private void textBox_Current_TextChanged(object sender, EventArgs e)
        {
           
        }
        /// <summary>
        /// 显示对应帧数的图片或轮廓 从1 开始
        /// </summary>
        /// <param name="num"> 对应帧数 从1 开始</param>
        void ShowImage(int num)
        {
            try
            {
                if (HeightImage.Count > 0)
                {
                    sw.hwnd[1].HobjectToHimage(HeightImage[num - 1]);
                }
                if (IntensityImage.Count > 0)
                {
                    sw.hwnd[2].HobjectToHimage(IntensityImage[num - 1]);
                }
                if (MyGlobal.GoSDK.ProfileList.Count>0)
                {
                    SagensSdk.Profile profile = MyGlobal.GoSDK.ProfileList[num - 1];
                    ShowSigleProfile(profile);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (CurrentIndex==0)
            {
                return;
            }
            if (CurrentIndex==1)
            {
                CurrentIndex = 1;
                textBox_Current.Text = CurrentIndex.ToString();
                return;
            }
            if (CurrentIndex>1)
            {
                CurrentIndex--;
                trackBarControl1.Value = CurrentIndex;
                textBox_Current.Text = CurrentIndex.ToString();
                ShowImage(CurrentIndex);
            }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int Total = Convert.ToInt32(textBox_Total.Text.ToString());
            if (CurrentIndex>=Total)
            {
                CurrentIndex = Total;
                textBox_Current.Text = CurrentIndex.ToString();
                trackBarControl1.Value = CurrentIndex;
                return;
            }
            CurrentIndex++;
            trackBarControl1.Value = CurrentIndex;
            textBox_Current.Text = CurrentIndex.ToString();
            ShowImage(CurrentIndex);
           
        }

        private void trackBarControl1_ValueChanged(object sender, EventArgs e)
        {
           
            CurrentIndex = trackBarControl1.Value;
            textBox_Current.Text = CurrentIndex.ToString();
            ShowImage(CurrentIndex);
        }

        private void textBox_Current_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            string index = textBox_Current.Text.ToString();
            bool ok = Regex.IsMatch(index, @"(?i)^[0-9]+$");
            if (CurrentIndex == 0 || index == "0")
            {
                return;
            }
            if (ok)
            {
                int num = int.Parse(index);
                CurrentIndex = num;
                trackBarControl1.Value = CurrentIndex;
                ShowImage(num);

            }
        }


        /// <summary>
        /// 由点获取轮廓
        /// </summary>
        /// <param name="isLeft">是否在左边</param>
        /// <param name="IgnorePoints">起始结束忽略点数</param>
        /// <param name="Row">输入轮廓行坐标</param>
        /// <param name="Col">输入轮廓列坐标</param>
        /// <param name="RowNew">输出转换后轮廓列坐标</param>
        /// <param name="ColNew">输出转换后轮廓行坐标</param>
        /// <param name="Region">输处转换后轮廓Region</param>
        /// <returns></returns>
        public string GenProfile(bool isLeft, HTuple Row, HTuple Col, out HTuple RowNew, out HTuple ColNew, out HObject Region, out HObject Contour)
        {
            RowNew = new HTuple(); ColNew = new HTuple(); Region = new HObject();
            Contour = new HObject();
            try
            {

                HTuple Lenr = Row.Length;
                HTuple Lenc = Col.Length;
                HTuple min = Lenr.TupleMin2(Lenc);
                Row =- Row.TupleSelectRange(0, min.I - 1);
                Col = Col.TupleSelectRange(0, min.I - 1);
                HOperatorSet.GenContourPolygonXld(out Contour, Row, Col);
                //HOperatorSet.WriteObject(Contour, "1.hobj");
                HTuple hommat2DIdentity = new HTuple(); HTuple Hommat2DRotate = new HTuple();
                HTuple Hommat2DTranslate = new HTuple();
                HOperatorSet.HomMat2dIdentity(out hommat2DIdentity);
                //HOperatorSet.HomMat2dRotate(hommat2DIdentity, 1.57, 0, 0, out Hommat2DRotate);
                //HObject tempContour = new HObject();
                //HOperatorSet.AffineTransContourXld(Contour, out tempContour, Hommat2DRotate);
                //HOperatorSet.GetContourXld(tempContour, out Row, out Col);
                HTuple ColMin = Col.TupleMin();

                HOperatorSet.HomMat2dTranslate(hommat2DIdentity, 500, -ColMin + 500, out Hommat2DTranslate);
                HOperatorSet.AffineTransContourXld(Contour, out Contour, Hommat2DTranslate);
                //HOperatorSet.WriteObject(Contour, "2.hobj");
                HOperatorSet.GetContourXld(Contour, out RowNew, out ColNew);
                HOperatorSet.GenRegionPoints(out Region, RowNew, ColNew);

                if (isLeft == false)
                {
                    //找到最高点进行镜像变换
                    HTuple Rowmin = RowNew.TupleMin();
                    HTuple minInd = RowNew.TupleFindFirst(Rowmin);
                    HTuple HomMat2DId = new HTuple(); HTuple HomMat2dReflect = new HTuple();
                    HOperatorSet.HomMat2dIdentity(out HomMat2DId);
                    HOperatorSet.HomMat2dReflect(HomMat2DId, Rowmin, ColNew[minInd] + 50, Rowmin + 100, ColNew[minInd] + 50, out HomMat2dReflect);
                    HOperatorSet.AffineTransContourXld(Contour, out Contour, HomMat2dReflect);
                    HOperatorSet.AffineTransRegion(Region, out Region, HomMat2dReflect, "nearest_neighbor");
                    HOperatorSet.GetContourXld(Contour, out RowNew, out ColNew);
                    HOperatorSet.GenRegionPoints(out Region, RowNew, ColNew);
                    //HOperatorSet.WriteObject(Region, "4.hobj");
                }
                return "OK";

            }
            catch (Exception ex)
            {
                return "GenProfile error " + ex.Message;
            }

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (sw.hwnd[1].Image!=null && sw.hwnd[1].Image.IsInitialized())
                {
                    roiController.setROIShape(new ROILine());
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}