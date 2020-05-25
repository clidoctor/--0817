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

namespace SagensVision.VisionTool
{
    public partial class CalibrationForm : DevExpress.XtraEditors.XtraForm
    {
        public CalibrationForm()
        {
            InitializeComponent();
            Calibration.mainUtl calib = new Calibration.mainUtl();
            this.Controls.Add(calib);
            calib.Dock = DockStyle.Fill;
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