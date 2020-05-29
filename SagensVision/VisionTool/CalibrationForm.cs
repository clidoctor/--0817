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
            calib.GetRobotIdxDelegate += OnGetIdxDelegate;
        }

        private void OnGetIdxDelegate(string idx)
        {
            byte[] buffer = new byte[128];
            MyGlobal.sktClient.Send(Encoding.UTF8.GetBytes("current_idx"));
            ThreadPool.QueueUserWorkItem(delegate
            {
                while (!MyGlobal.ReceiveMsg.Contains("point"))
                {
                    Thread.Sleep(100);
                }
                string[] msgArr = MyGlobal.ReceiveMsg.Split(',');

                calib.SetValue(idx, msgArr);
                MyGlobal.ReceiveMsg = "";
            });
        }

        private void CalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string[] SideName = { "Side1", "Side2", "Side3", "Side4" };
            for (int i = 0; i < 4; i++)
            {
                Calibration.ParamPath.ParaName = SideName[i];
                if (File.Exists(Calibration.ParamPath.Path_tup))
                {
                    HOperatorSet.ReadTuple(Calibration.ParamPath.Path_tup, out MyGlobal.HomMat3D[i]);
                }

            }
        }
    }
}