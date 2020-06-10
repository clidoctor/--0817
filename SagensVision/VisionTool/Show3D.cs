using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ChoiceTech.Halcon.Control;

namespace SagensVision.VisionTool
{
    public partial class Show3D : DevExpress.XtraEditors.XtraForm
    {
        HWindow_Final Window = new HWindow_Final();
        public Show3D()
        {
            InitializeComponent();
           
            
        }

        private void chartControl1_Click(object sender, EventArgs e)
        {

        }
    }
}