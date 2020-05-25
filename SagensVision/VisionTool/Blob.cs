using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using HalconDotNet;
using System.IO;
using System.Threading;
using ChoiceTech.Halcon.Control;
using DevExpress.XtraEditors.Repository;

namespace SagensVision.VisionTool
{
    public partial class Blob : DevExpress.XtraEditors.XtraUserControl
    {
        SettingParam setting = new SettingParam();
        HWindow_Final window = null;
        public Blob()
        {
            InitializeComponent();
            UpDownBase upDown = (UpDownBase)numericUpDown1;
            upDown.TextChanged += new EventHandler(Up_textChanged);
            UpDownBase upDown2 = (UpDownBase)numericUpDown2;
            UpDownBase upDown3 = (UpDownBase)numericUpDown3;
            UpDownBase upDown4 = (UpDownBase)numericUpDown4;
            UpDownBase upDown5 = (UpDownBase)numericUpDown5;
            upDown2.TextChanged += new EventHandler(Up_textChanged);
            upDown3.TextChanged += new EventHandler(Up_textChanged);
            upDown4.TextChanged += new EventHandler(Up_textChanged);
            upDown5.TextChanged += new EventHandler(Up_textChanged);

        }
        bool RunFlag = false;
        void   Up_textChanged(object sender,EventArgs e)
        {
            if (!image.IsInitialized() || Runflag2||RunFlag3)
            {
                return;
            }
            RunFlag = true;
            NumericUpDown numericUpDown = (NumericUpDown)sender;
            if (numericUpDown.Name == "numericUpDown1" || numericUpDown.Name == "numericUpDown2")
            {
                //全局阈值分割
                setting.LocalOrGlobal = false;
                int low = (int)numericUpDown1.Value;
                int high = (int)numericUpDown2.Value;
                if (low >= high)
                {
                    //MessageBox.Show("最小阈值设置不能大于最大阈值设置！");
                    return;
                }
                else
                {
                    
                    setting.minThreshold = low;
                    setting.maxThreshold = high;
                    TrackBarRange range = new TrackBarRange(0,255);
                    range.Minimum = low;range.Maximum = high;
                    rangeTrackBarControl1.Value = range;
                  
                    HObject Region = new HObject();
                    Region.Dispose();
                    string Error = RunThreshold(image, out Region);

                    if (window != null && Error == "OK")
                    {
                        HOperatorSet.SetColor(window.HWindowHalconID, "red");
                        HOperatorSet.DispObj(Region, window.HWindowHalconID);                      
                    }
                    else
                    {
                        MessageBox.Show(Error);
                    }
                   
                }
            }
            else //本地阈值
            {
                int width = (int)numericUpDown3.Value;
                int high = (int)numericUpDown4.Value;
                int offset = (int)numericUpDown5.Value;
                trackBarControl1.Value = width;
                trackBarControl2.Value = high;
                trackBarControl3.Value = offset;
                HObject Region = new HObject();
                Region.Dispose();
                setting.maskWith = width;setting.maskHeight = high;
                setting.LocalOrGlobal = true;
                setting.thresholdOffset = offset;
                string Error = RunThreshold(image, out Region);
                if (window != null && Error == "OK")
                {
                    HOperatorSet.SetColor(window.HWindowHalconID, "red");
                    HOperatorSet.DispObj(Region, window.HWindowHalconID);
                }
                else
                {
                    MessageBox.Show(Error);
                }
            }
            RunFlag = false;
        }
        public HWindow_Final WindowControl
        {
            set
            {
                window = value;
            }
        }

        public class SettingParam
        {
            /// <summary>
            /// 使用局部阈值（true）还是全局阈值（false)
            /// </summary>
            public bool LocalOrGlobal = true;
            /// <summary>
            /// 局部阈值分割宽
            /// </summary>
            public int maskWith = 20;
            /// <summary>
            /// 局部阈值分割高
            /// </summary>
            public int maskHeight = 20;
            /// <summary>
            /// 局部阈值偏移
            /// </summary>
            public int thresholdOffset = 1;
            /// <summary>
            /// 全局最小阈值
            /// </summary>
            public int minThreshold = 219;
            /// <summary>
            /// 全局最大阈值
            /// </summary>
            public int maxThreshold = 255;
            /// <summary>
            /// 提取区域前景 明or暗
            /// </summary>
            public string LightOrDark = "light";

        }

        string RunThreshold(HObject Image,out  HObject Region)
        {
            Region = new HObject();
            try
            {
                HObject GrayImage = new HObject();
                HTuple Width, Height;
                HOperatorSet.GetImageSize(Image, out Width, out Height);
                //HObject Region = new HObject();
                //HOperatorSet.GenRectangle1(out Region, 0, 0, Height, Width);
                //HTuple MinGray, MaxGray, RangeGray;
                //HOperatorSet.MinMaxGray(Region, Image, 0, out MinGray, out MaxGray, out RangeGray);
                HObject tempImage = new HObject();
                HOperatorSet.ScaleImageMax(Image, out tempImage);
                if (setting.LocalOrGlobal)
                {
                    HObject MeanImage = new HObject();
                    HOperatorSet.MeanImage(tempImage, out MeanImage, setting.maskWith, setting.maskHeight);

                    HOperatorSet.DynThreshold(tempImage, MeanImage, out Region, setting.thresholdOffset, setting.LightOrDark);
                    MeanImage.Dispose();
                }
                else //全局阈值
                {
                    HOperatorSet.Threshold(tempImage, out Region, setting.minThreshold, setting.maxThreshold);
                }

                tempImage.Dispose();

                return "OK";

            }
            catch (Exception ex)
            {

                return "RunThreshold Error:"+ex.Message;
            }
            
        }
        HObject image = new HObject();
        bool readImage = false;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (readImage)
                {
                    return;
                }
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "(*.tiff)|*.tiff|所有文件(*.*)|*.*||";
                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    if (String.IsNullOrEmpty(dialog.FileName))
                    {
                        return;
                    }
                    readImage = true;
                    ThreadPool.QueueUserWorkItem(delegate {

                        string path = dialog.FileName;
                        HOperatorSet.ReadImage(out image, path);
                        if (window!=null)
                        {
                            window.HobjectToHimage(image);
                        }

                    });
                    readImage = false;
                }
            }
            catch (Exception)
            {
                readImage = true;
                throw;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        bool Runflag2 = false;
        private void rangeTrackBarControl1_ValueChanged(object sender, EventArgs e)
        {
            if (!image.IsInitialized()|| RunFlag )
            {
                return;
            }
                Runflag2 = true;
                //全局阈值分割
                setting.LocalOrGlobal = false;
                int low = rangeTrackBarControl1.Value.Minimum;
                int high = rangeTrackBarControl1.Value.Maximum;
                numericUpDown1.Value = (decimal)low;
                numericUpDown2.Value = (decimal)high;
               
                    setting.minThreshold = low;
                    setting.maxThreshold = high;
                    HObject Region = new HObject();
                    Region.Dispose();
                    string Error = RunThreshold(image, out Region);

                    if (window != null && Error == "OK")
                    {
                        HOperatorSet.SetColor(window.HWindowHalconID, "red");
                        HOperatorSet.DispObj(Region, window.HWindowHalconID);

                    }
                    else
                    {
                        MessageBox.Show(Error);
                    }
                    Runflag2 = false;

        }
        bool RunFlag3 = false;
        private void trackBarControl1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!image.IsInitialized() || RunFlag)
                {
                    return;
                }
                RunFlag3 = true;
                TrackBarControl control = (TrackBarControl)sender;
                string name = control.Name;
                int Num = control.Value;
                switch (name)
                {
                    case "trackBarControl1":
                        setting.maskWith = Num;
                        numericUpDown3.Value = Num;
                        break;
                    case "trackBarControl2":
                        setting.maskHeight = Num;
                        numericUpDown4.Value = Num;
                        break;
                    case "trackBarControl3":
                        setting.thresholdOffset = Num;
                        numericUpDown5.Value = Num;
                        break;
                }
                HObject Region = new HObject();
                Region.Dispose();
                string Error = RunThreshold(image, out Region);
                if (window != null && Error == "OK")
                {
                    HOperatorSet.SetColor(window.HWindowHalconID, "red");
                    HOperatorSet.DispObj(Region, window.HWindowHalconID);
                }
                else
                {
                    MessageBox.Show(Error);
                }
                RunFlag3 = false;

            }
            catch (Exception)
            {

                RunFlag3 = false;
                throw;
            }
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!image.IsInitialized())
            {
                return;
            }
            if (comboBoxEdit1.SelectedItem.ToString() == "明")
            {
                setting.LightOrDark = "light";
            }
            else
            {
                setting.LightOrDark = "dark";

            }
            HObject Region = new HObject();
            Region.Dispose();
            string Error = RunThreshold(image, out Region);
            if (window != null && Error == "OK")
            {
                HOperatorSet.SetColor(window.HWindowHalconID, "red");
                HOperatorSet.DispObj(Region, window.HWindowHalconID);
            }
            else
            {
                MessageBox.Show(Error);
            }
        }
    }
}
