using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using HalconDotNet;

namespace SagensVision.VisionTool
{
    public partial class CalibFormMain : DevExpress.XtraEditors.XtraForm
    {
        public CalibFormMain()
        {
            InitializeComponent();
        }
        VisionTool.FitLineSet fitlineSet = new FitLineSet("Calib_FindLine");
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            fitlineSet.ToolType = "Calib";
            fitlineSet.FindType = "FitLineSet";
            fitlineSet.ShowDialog();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            VisionTool.FitLineSet fix = new FitLineSet("Calib_Fix");
            fix.ToolType = "Calib";
            fix.FindType = "Fix";
            fix.ShowDialog();
        }
        VisionTool.IntersectForm intersect = new IntersectForm();
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            intersect.ShowDialog();
        }

        private void CalibFormMain_Load(object sender, EventArgs e)
        {
            intersect.GenDataSource += Intersect_GenDataSource;

        }

        private void Intersect_GenDataSource(string obj)
        {
            if (fitlineSet.H_Lines.Count == 0)
            {
                return;
            }
            string SideName = comboBox1.SelectedItem.ToString();
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            switch (obj)
            {
                case "Lines_H":
                    intersect.Lines_H.Clear();
                    for (int i = 0; i < fitlineSet.fpTool.fParam[Id].roiP.Count; i++)
                    {
                        if (fitlineSet.fpTool.fParam[Id].DicPointName[i].Contains("H"))
                        {
                            intersect.Lines_H.Add(fitlineSet.H_Lines[i]);
                        }
                    }

                    break;
                case "Lines_V":
                    intersect.Lines_V.Clear();
                    for (int i = 0; i < fitlineSet.fpTool.fParam[Id].roiP.Count; i++)
                    {
                        if (fitlineSet.fpTool.fParam[Id].DicPointName[i].Contains("V"))
                        {
                            intersect.Lines_V.Add(fitlineSet.H_Lines[i]);
                        }
                    }

                    break;
            }
        }
        public static event Action<int,HObject, HObject, HObject> ChangeSide;
        public static event Action<bool> ChangeStation;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SideName = comboBox1.SelectedItem.ToString();
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            HObject IntensityImage = null;
            HObject HeightImage = null;
            HObject OriginImage = null;
            if (MyGlobal.ImageMulti.Count >= Id + 1)
            {
                IntensityImage = MyGlobal.ImageMulti[Id][0];
                if (MyGlobal.globalConfig.enableAlign)
                {
                    if (MyGlobal.ImageMulti[Id].Length < 3)
                    {
                        MessageBox.Show("请重新加载数据");
                        return;
                    }
                    HeightImage = MyGlobal.ImageMulti[Id][2];
                    OriginImage = MyGlobal.ImageMulti[Id][1];
                }
                else
                {
                    HeightImage = MyGlobal.ImageMulti[Id][1];
                    OriginImage = null;
                }

            }
            ChangeSide?.Invoke(Id,IntensityImage, HeightImage, OriginImage);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            intersect.ShowDialog();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex ==0)
            {
                ChangeStation?.Invoke(true);//isRight
            }
            else
            {
                ChangeStation?.Invoke(false);

            }
        }
    }
}