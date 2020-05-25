using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Docking;
using ChoiceTech.Halcon.Control;

namespace SagensVision.Sensor
{
    public partial class ShowWindow : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 1,轮廓窗口 2 高度图窗口 3 亮度图窗口
        /// </summary>
        public HWindow_Final[] hwnd = new HWindow_Final[3];           
        public ShowWindow()
        {
            InitializeComponent();
            DockPanel[] dp = { dockPanel1, dockPanel2, dockPanel3 };
            for (int i = 0; i < 3; i++)
            {               
                hwnd[i] = new HWindow_Final();
                dp[i].Controls.Add(hwnd[i]);
                hwnd[i].Dock = DockStyle.Fill;
            }
            
        }

        public void ResetWindow()
        {
            dockPanel1.Show();
            dockPanel2.Show();
            dockPanel3.Show();
            dockPanel1.DockedAsTabbedDocument = true;
            dockPanel2.DockedAsTabbedDocument = true;
            dockPanel3.DockedAsTabbedDocument = true;

        }
    }
}
