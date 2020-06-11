using DevExpress.XtraCharts;
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

        public void setValue()
        {
            chartControl1.Series[0].Points[0].Values = new double[] { MyGlobal.globalConfig.OkCnt };
            chartControl1.Series[0].Points[1].Values = new double[] { MyGlobal.globalConfig.AnchorErrorCnt };
            chartControl1.Series[0].Points[2].Values = new double[] { MyGlobal.globalConfig.FindEgdeErrorCnt };
            chartControl1.Series[0].Points[3].Values = new double[] { MyGlobal.globalConfig.ExploreHeightErrorCnt };
            Pie3DSeriesView pie3DSeriesView = (Pie3DSeriesView)chartControl1.Series[0].View;
            int totalCnt = MyGlobal.globalConfig.OkCnt + MyGlobal.globalConfig.AnchorErrorCnt +
                MyGlobal.globalConfig.FindEgdeErrorCnt + MyGlobal.globalConfig.ExploreHeightErrorCnt;
            pie3DSeriesView.Titles[0].Text = $"总产能：{totalCnt}";
        }

        private void btn_show_clear_data_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认清空生产数据?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MyGlobal.globalConfig.OkCnt = 0;
                MyGlobal.globalConfig.AnchorErrorCnt = 0;
                MyGlobal.globalConfig.FindEgdeErrorCnt = 0;
                MyGlobal.globalConfig.ExploreHeightErrorCnt = 0;
            }
            setValue();

            StaticOperate.WriteXML(MyGlobal.globalConfig, MyGlobal.ConfigPath + "Global.xml");
        }

    }
   
}
