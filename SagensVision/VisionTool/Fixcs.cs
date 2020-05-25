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
    public partial class Fixcs : DevExpress.XtraEditors.XtraForm
    {
        //public  mainUtl FixLine = new mainUtl();
        public Fixcs()
        {
            InitializeComponent();
           
            //this.Controls.Add(FixLine);
            //FixLine.Dock = DockStyle.Fill;
        }

        private void Fixcs_FormClosing(object sender, FormClosingEventArgs e)
        {
            //    FixLine.DisposeImage();
            //    FixLine.LoadParam();
        }
    }
}