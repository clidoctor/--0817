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
    public partial class LineCircleForm : DevExpress.XtraEditors.XtraForm
    {
        public LineCircleForm()
        {
            InitializeComponent();
            LookForCircleLine.mainUtl CircleLine = new LookForCircleLine.mainUtl();
            this.Controls.Add(CircleLine);
            CircleLine.Dock = DockStyle.Fill;
        }
    }
}