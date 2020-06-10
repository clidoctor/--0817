using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SagensVision.VisionTool
{
    public partial class ShowCapacityFrm : Form
    {
        public ShowCapacityFrm()
        {
            InitializeComponent();
        }

        private void ShowCapacityFrm_Load(object sender, EventArgs e)
        {

        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            chartControl1.Series[0].Points[0].Values = new double[] { 10};

        }
    }
}
