using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;

namespace SagensVision.VisionTool
{
    public partial class GlobalParam : DevExpress.XtraEditors.XtraForm
    {
        public delegate void RunOff();
        public RunOff Run;
        public GlobalParam()
        {
            InitializeComponent();
            Run = new RunOff(dmmy);            
        }
        public void dmmy()
        {

        }
        void LoadToUi()
        {
            try
            {
                textBox_ColorMin.Text = MyGlobal.globalConfig.Color_min.ToString();
                textBox_ColorMax.Text = MyGlobal.globalConfig.Color_max.ToString();
                textBox_HeightMax.Text = MyGlobal.globalConfig.HeightMax.ToString();
                textBox_HeightMin.Text = MyGlobal.globalConfig.HeightMin.ToString();
                textBox_XYMax.Text = MyGlobal.globalConfig.XYMax.ToString();
                textBox_XYMin.Text = MyGlobal.globalConfig.XYMin.ToString();

                textBox_Start.Text = MyGlobal.globalConfig.Startpt.ToString();
                textBox_totalZ.Text = MyGlobal.globalConfig.TotalZoffset.ToString();                
                textBox_xOffset.Text = MyGlobal.globalConfig.gbParam[0].Xoffset.ToString();
                textBox_yOffset.Text = MyGlobal.globalConfig.gbParam[0].Yoffset.ToString();

                checkedListBox_save_data.SetItemChecked(0, MyGlobal.globalConfig.isSaveKdat);
                checkedListBox_save_data.SetItemChecked(1, MyGlobal.globalConfig.isSaveFileDat);
                checkedListBox_save_data.SetItemChecked(2, MyGlobal.globalConfig.isSaveImg);

                ///task

            }
            catch (Exception)
            {

                throw;
            }
        }
        string SideName = "Side1";
        private void textBox_ColorMin_TextChanged(object sender, EventArgs e)
        {
            SideName = comboBox1.SelectedItem.ToString();
            int SideId = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            TextBox tb = (TextBox)sender;
            string index = tb.Text.ToString();
            //bool ok = Regex.IsMatch(index, @"(?i)^(\-[0-9]{1,}[.][0-9]*)+$") || Regex.IsMatch(index, @"(?i)^(\-[0-9]{1,}[0-9]*)+$") || Regex.IsMatch(index, @"(?i)^([0-9]{1,}[0-9]*)+$") || Regex.IsMatch(index, @"(?i)^(\[0-9]{1,}[0-9]*)+$");
            bool ok = Regex.IsMatch(index, @"^[-]?\d+[.]?\d*$");//是否为数字
                                                                //bool ok = Regex.IsMatch(index, @"^([-]?)\d*$");//是否为整数
            if (!ok)
            {
                return;
            }
            double num = double.Parse(index);

            switch (tb.Name)
            {
                case "textBox_ColorMin":
                    MyGlobal.globalConfig.Color_min = num;
                    break;
                case "textBox_ColorMax":
                    MyGlobal.globalConfig.Color_max = num;
                    break;
                case "textBox_HeightMin":
                    MyGlobal.globalConfig.HeightMin = num;
                    break;
                case "textBox_HeightMax":
                    MyGlobal.globalConfig.HeightMax = num;
                    break;
                case "textBox_totalZ":
                    MyGlobal.globalConfig.TotalZoffset = num;
                    break;
                case "textBox_Start":
                    if ((int)num <= 0)
                    {
                        num = 1;
                    }
                    MyGlobal.globalConfig.Startpt = (int)num;
                    break;
                case "textBox_xOffset":
                    MyGlobal.globalConfig.gbParam[SideId].Xoffset = (int)num;
                    break;
                case "textBox_yOffset":
                    MyGlobal.globalConfig.gbParam[SideId].Yoffset = (int)num;
                    break;
                case "textBox_XYMax":
                    MyGlobal.globalConfig.XYMax = num;
                    break;
                case "textBox_XYMin":
                    MyGlobal.globalConfig.XYMin = num;
                    break;
            }

           
            }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SideName = comboBox1.SelectedItem.ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                MyGlobal.globalConfig.isSaveKdat = checkedListBox_save_data.GetItemChecked(0);
                MyGlobal.globalConfig.isSaveFileDat = checkedListBox_save_data.GetItemChecked(1);
                MyGlobal.globalConfig.isSaveImg = checkedListBox_save_data.GetItemChecked(2);
                StaticOperate.WriteXML(MyGlobal.globalConfig, MyGlobal.ConfigPath + "Global.xml");
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {

                MessageBox.Show("保存失败！" +ex.Message);
            }
           
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void GlobalParam_Load(object sender, EventArgs e)
        {
            LoadToUi();
        }

       
    }

    
}