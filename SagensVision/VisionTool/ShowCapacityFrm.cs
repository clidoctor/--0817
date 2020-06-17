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

        public void setValue(bool IsRight)
        {
            if (IsRight)
            {
                chartControl1.Series[0].Points[0].Values = new double[] { MyGlobal.globalPointSet_Right.OkCnt };
                chartControl1.Series[0].Points[1].Values = new double[] { MyGlobal.globalPointSet_Right.AnchorErrorCnt };
                chartControl1.Series[0].Points[2].Values = new double[] { MyGlobal.globalPointSet_Right.FindEgdeErrorCnt };
                chartControl1.Series[0].Points[3].Values = new double[] { MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt };
                Pie3DSeriesView pie3DSeriesView = (Pie3DSeriesView)chartControl1.Series[0].View;
                int totalCnt = MyGlobal.globalPointSet_Right.OkCnt + MyGlobal.globalPointSet_Right.AnchorErrorCnt +
                    MyGlobal.globalPointSet_Right.FindEgdeErrorCnt + MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt;
                if (totalCnt == 0)
                {
                    chartControl1.Series[0].Points[0].Values = new double[] { 1 };
                }
                pie3DSeriesView.Titles[0].Text = $"总产能：{totalCnt}";
            }
            else
            {
                chartControl1.Series[0].Points[0].Values = new double[] { MyGlobal.globalPointSet_Left.OkCnt };
                chartControl1.Series[0].Points[1].Values = new double[] { MyGlobal.globalPointSet_Left.AnchorErrorCnt };
                chartControl1.Series[0].Points[2].Values = new double[] { MyGlobal.globalPointSet_Left.FindEgdeErrorCnt };
                chartControl1.Series[0].Points[3].Values = new double[] { MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt };
                Pie3DSeriesView pie3DSeriesView = (Pie3DSeriesView)chartControl1.Series[0].View;
                int totalCnt = MyGlobal.globalPointSet_Left.OkCnt + MyGlobal.globalPointSet_Left.AnchorErrorCnt +
                    MyGlobal.globalPointSet_Left.FindEgdeErrorCnt + MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt;
                if (totalCnt == 0)
                {
                    chartControl1.Series[0].Points[0].Values = new double[] { 1 };
                }
                pie3DSeriesView.Titles[0].Text = $"总产能：{totalCnt}";
            }

           
        }

        private void btn_show_clear_data_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认清空生产数据?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MyGlobal.globalPointSet_Right.OkCnt = 0;
                MyGlobal.globalPointSet_Right.AnchorErrorCnt = 0;
                MyGlobal.globalPointSet_Right.FindEgdeErrorCnt = 0;
                MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt = 0;
                MyGlobal.globalPointSet_Left.OkCnt = 0;
                MyGlobal.globalPointSet_Left.AnchorErrorCnt = 0;
                MyGlobal.globalPointSet_Left.FindEgdeErrorCnt = 0;
                MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt = 0;
            }
            setValue(true);
            setValue(false);

            StaticOperate.WriteXML(MyGlobal.globalPointSet_Right, MyGlobal.AllTypePath + "GlobalPoint_Right.xml");
            StaticOperate.WriteXML(MyGlobal.globalPointSet_Left, MyGlobal.AllTypePath + "GlobalPoint_Left.xml");

        }

    }
   
}
