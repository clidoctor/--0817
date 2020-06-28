using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SagensVision.VisionTool
{
    public partial class CalibFormMain : DevExpress.XtraEditors.XtraForm
    {
        public CalibFormMain()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            VisionTool.FitLineSet fix = new FitLineSet("Calib_Fix");
        }
    }
}