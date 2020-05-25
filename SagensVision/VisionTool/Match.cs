using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Matching;

namespace SagensVision.VisionTool
{
    public partial class Match : DevExpress.XtraEditors.XtraForm
    {
        Matching.Form1 match = new Form1();
        public Match()
        {
            InitializeComponent();
            this.Controls.Add(match);
            match.Dock = DockStyle.Fill;
        }
    }
}