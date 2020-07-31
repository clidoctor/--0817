using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using HalconDotNet;
using System.Threading;

namespace SagensVision.VisionTool
{
    public partial class CalibrationForm : DevExpress.XtraEditors.XtraForm
    {
        Calibration.mainUtl calib;
        public CalibrationForm()
        {
            InitializeComponent();
            calib = new Calibration.mainUtl();
            this.Controls.Add(calib);
            calib.Dock = DockStyle.Fill;
            //calib.GetRobotIdxDelegate += OnGetIdxDelegate;
            if (MyGlobal.globalConfig.uiStyle == "2")
            {
                calib.GetRobotIdxDelegate += OnGetIdxByReadCsv;
            }
            else { calib.GetRobotIdxDelegate += OnGetIdxDelegate; }
            
            this.MaximizeBox = false;
        }

        private void OnGetIdxDelegate(string idx,string LorR)
        {
            byte[] buffer = new byte[128];
            try
            {
                MyGlobal.sktClient.Send(Encoding.UTF8.GetBytes(LorR + "POS"));
            }
            catch (Exception)
            {
                
            }
            
            ThreadPool.QueueUserWorkItem(delegate
            {
                while (!MyGlobal.ReceiveMsg.Contains(LorR + "POS"))
                {
                    Thread.Sleep(100);
                }
                try
                {
                    string[] msgArr = MyGlobal.ReceiveMsg.Split(',');

                    calib.SetValue(idx, msgArr);
                    MyGlobal.ReceiveMsg = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("接收数据异常：" + ex.Message);
                }
                
            });
        }

        private void OnGetIdxByReadCsv(string idx, string LorR)
        {
            try
            {
                using (StreamReader sr = new StreamReader($@"C:\\Twinwin\\data\\{LorR}\\Calibration.csv"))
                {
                    while (!sr.EndOfStream)
                    {
                        string a = sr.ReadLine();
                        string[] b = a.Split(',');
                        if (idx == b[0])
                        {
                            calib.SetValue(idx, b);
                            break;
                        }
                    }
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Read CSV:" + ex.Message);
            }
        }

        private void CalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string[] SideName = { "Side1", "Side2", "Side3", "Side4" };
            for (int i = 0; i < 4; i++)
            {
                Calibration.ParamPath.ParaName = SideName[i];
                Calibration.ParamPath.LeftOrRight = "Right";
                if (File.Exists(Calibration.ParamPath.Path_tup))
                {
                    HOperatorSet.ReadTuple(Calibration.ParamPath.Path_tup, out MyGlobal.HomMat3D_Right[i]);
                }
                Calibration.ParamPath.LeftOrRight = "Left";
                if (File.Exists(Calibration.ParamPath.Path_tup))
                {
                    HOperatorSet.ReadTuple(Calibration.ParamPath.Path_tup, out MyGlobal.HomMat3D_Left[i]);
                }
            }
        }
    }
}