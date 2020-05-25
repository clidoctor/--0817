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
    public partial class BlobForm : DevExpress.XtraEditors.XtraForm
    {
        public BlobForm()
        {
            InitializeComponent();
            HWindow_Final control = new HWindow_Final();
            splitContainerControl1.Panel1.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            Blob blob = new Blob();
            splitContainerControl1.Panel2.Controls.Add(blob);
            blob.Dock = DockStyle.Fill;
            blob.WindowControl = control;
        }
    }
}