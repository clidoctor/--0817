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
        bool isRight;
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
                if (isRight)
                {
                    textBox_HeightMax.Text = MyGlobal.globalPointSet_Right.HeightMax.ToString();
                    textBox_HeightMin.Text = MyGlobal.globalPointSet_Right.HeightMin.ToString();
                    textBox_XYMax.Text = MyGlobal.globalPointSet_Right.XYMax.ToString();
                    textBox_XYMin.Text = MyGlobal.globalPointSet_Right.XYMin.ToString();

                    textBox_Start.Text = MyGlobal.globalPointSet_Right.Startpt.ToString();
                    textBox_totalZ.Text = MyGlobal.globalPointSet_Right.TotalZoffset.ToString();
                    textBox_xOffset.Text = MyGlobal.globalPointSet_Right.gbParam[0].Xoffset.ToString();
                    textBox_yOffset.Text = MyGlobal.globalPointSet_Right.gbParam[0].Yoffset.ToString();
                }
                else
                {
                    textBox_HeightMax.Text = MyGlobal.globalPointSet_Left.HeightMax.ToString();
                    textBox_HeightMin.Text = MyGlobal.globalPointSet_Left.HeightMin.ToString();
                    textBox_XYMax.Text = MyGlobal.globalPointSet_Left.XYMax.ToString();
                    textBox_XYMin.Text = MyGlobal.globalPointSet_Left.XYMin.ToString();

                    textBox_Start.Text = MyGlobal.globalPointSet_Left.Startpt.ToString();
                    textBox_totalZ.Text = MyGlobal.globalPointSet_Left.TotalZoffset.ToString();
                    textBox_xOffset.Text = MyGlobal.globalPointSet_Left.gbParam[0].Xoffset.ToString();
                    textBox_yOffset.Text = MyGlobal.globalPointSet_Left.gbParam[0].Yoffset.ToString();
                }

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
                    if (isRight)
                    {
                        MyGlobal.globalPointSet_Right.HeightMin = num;
                    }
                    else
                    {
                        MyGlobal.globalPointSet_Left.HeightMin = num;
                    }
                    break;
                case "textBox_HeightMax":
                   
                    if (isRight)
                    {
                        MyGlobal.globalPointSet_Right.HeightMax = num;
                    }
                    else
                    {
                        MyGlobal.globalPointSet_Left.HeightMax = num;
                    }
                    break;
                case "textBox_totalZ":
                    if (isRight)
                    {
                        MyGlobal.globalPointSet_Right.TotalZoffset = num;
                    }
                    else
                    {
                        MyGlobal.globalPointSet_Left.TotalZoffset = num;
                    }
                    break;
                case "textBox_Start":
                    if ((int)num <= 0)
                    {
                        num = 1;
                    }
                    if (isRight)
                    {
                        MyGlobal.globalPointSet_Right.Startpt = (int)num;
                    }
                    else
                    {
                        MyGlobal.globalPointSet_Left.Startpt = (int)num;
                    }
                    break;
                case "textBox_xOffset":
                    if (isRight)
                    {
                        MyGlobal.globalPointSet_Right.gbParam[SideId].Xoffset = num;
                    }
                    else
                    {
                        MyGlobal.globalPointSet_Left.gbParam[SideId].Xoffset = num;
                    }
                    break;
                case "textBox_yOffset":
                    if (isRight)
                    {
                        MyGlobal.globalPointSet_Right.gbParam[SideId].Yoffset = num;
                    }
                    else
                    {
                        MyGlobal.globalPointSet_Left.gbParam[SideId].Yoffset = num;
                    }
                    break;
                case "textBox_XYMax":
                   
                    if (isRight)
                    {
                        MyGlobal.globalPointSet_Right.XYMax = num;
                    }
                    else
                    {
                        MyGlobal.globalPointSet_Left.XYMax = num;
                    }
                    break;
                case "textBox_XYMin":
                   
                    if (isRight)
                    {
                        MyGlobal.globalPointSet_Right.XYMin = num;
                    }
                    else
                    {
                        MyGlobal.globalPointSet_Left.XYMin = num;
                    }
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
                if (isRight)
                {
                    if (MessageBox.Show("提示", "是否保存右工位参数", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show("提示", "是否保存左工位参数", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                

                MyGlobal.globalConfig.isSaveKdat = checkedListBox_save_data.GetItemChecked(0);
                MyGlobal.globalConfig.isSaveFileDat = checkedListBox_save_data.GetItemChecked(1);
                MyGlobal.globalConfig.isSaveImg = checkedListBox_save_data.GetItemChecked(2);
                StaticOperate.WriteXML(MyGlobal.globalConfig, MyGlobal.AllTypePath + "Global.xml");
                if (isRight)
                {
                    StaticOperate.WriteXML(MyGlobal.globalPointSet_Right, MyGlobal.AllTypePath + "GlobalPoint_Right.xml");
                }
                else
                {
                    StaticOperate.WriteXML(MyGlobal.globalPointSet_Left, MyGlobal.AllTypePath + "GlobalPoint_Left.xml");
                }
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
            isRight = MyGlobal.IsRight;
            LoadToUi();
            
            if (isRight)
            {
                comboBox2.SelectedIndex = 0;
                comboBox2.BackColor = Color.LimeGreen;
            }
            else
            {
                comboBox2.SelectedIndex = 1;
                comboBox2.BackColor = Color.Yellow;
            }
          
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Right")
            {
                isRight = true;
                comboBox2.BackColor = Color.LimeGreen;
            }
            else
            {
                isRight = false;
                comboBox2.BackColor = Color.Yellow;
            }
            LoadToUi();
            MessageBox.Show("切换成功!");
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {

            NewProduct nProduct = new NewProduct(isRight);
            nProduct.ShowDialog();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            ImgRotateFrm imgrotatefrm = new ImgRotateFrm(isRight);
            imgrotatefrm.Show();
        }
    }

    
}