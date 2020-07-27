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
using System.IO;
using DevExpress.XtraEditors.Repository;

namespace SagensVision.VisionTool
{
    public partial class GlobalParam : DevExpress.XtraEditors.XtraForm
    {
        public delegate void RunOff();
        public RunOff Run;
        bool isRight;
        public static event Action<string> ChangeType;
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
                isLoading = true;
                comboBox1.SelectedIndex = 0;
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
                    cb_IsUp.Checked = MyGlobal.globalPointSet_Right.IsUp[0];
                    cb_Reverse.Checked = MyGlobal.globalPointSet_Right.IsReverse;
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
                    cb_IsUp.Checked = MyGlobal.globalPointSet_Left.IsUp[0];
                    cb_Reverse.Checked = MyGlobal.globalPointSet_Left.IsReverse;

                }

                checkedListBox_save_data.SetItemChecked(0, MyGlobal.globalConfig.isSaveKdat);
                checkedListBox_save_data.SetItemChecked(1, MyGlobal.globalConfig.isSaveFileDat);
                checkedListBox_save_data.SetItemChecked(2, MyGlobal.globalConfig.isSaveImg);
                checkBox1.Checked =  MyGlobal.globalConfig.enableAlign;
                cb_Features.Checked = MyGlobal.globalConfig.enableFeature;
                cb_UseFix.Checked = MyGlobal.globalConfig.isUseFix;
                cb_UseSelfOffset.Checked = MyGlobal.globalConfig.isUseSelfOffset;
                isLoading = false;
            }
            catch (Exception)
            {

                throw;
            }
        }
       
        private void ChangeAxisName(int StartPt)
        {
            try
            {
               
               
                FindPointTool fptool = new FindPointTool();               
                fptool = isRight ? MyGlobal.Right_findPointTool_Find:MyGlobal.Left_findPointTool_Find;
                int count = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < fptool.fParam[i].DicPointName.Count; j++)
                    {
                        count++;
                    }
                }
                string[] LastName = new string[count];
                for (int i = 0; i < count; i++)
                {
                    int start = StartPt;
                    if (StartPt - 1 + i >= count)
                    {
                        start = StartPt - 1 + i - count;
                    }
                    else
                    {
                        start = StartPt - 1 + i;
                    }
                    LastName[i] = "_" + (start + 1).ToString();
                }


                int n = 0;
                for (int i = 0; i < 4; i++)
                {
                    List<string> NewDicPointName = fptool.fParam[i].DicPointName;//C1(边）_1（点)_1(顺序）_
                    for (int j = 0; j < fptool.fParam[i].DicPointName.Count; j++)
                    {
                        string name = fptool.fParam[i].DicPointName[j];                       
                        string[] ArrayName = name.Split('_');
                        if (ArrayName.Length == 3)
                        {
                            NewDicPointName[j] += LastName[n];
                        }
                        if (ArrayName.Length < 3)
                        {
                            NewDicPointName[j] += LastName[n];
                            NewDicPointName[j] += LastName[n];
                        }
                        //if (ArrayName.Length == 4)
                        //{
                        //    //ArrayName[3] = LastName[n];
                        //    NewDicPointName[j] = NewDicPointName[j].Replace(ArrayName[3],LastName[n]);
                        //}
                        if (ArrayName.Length >= 4)
                        {
                            if (ArrayName[1]!="")
                            {
                                NewDicPointName[j] = ArrayName[0] + "_" + ArrayName[1] + LastName[n];
                            }
                            else
                            {
                                NewDicPointName[j] = ArrayName[0]  + LastName[n] + LastName[n];
                            }


                        }

                        n++;             
                    }
                    ParamPath.ToolType = MyGlobal.ToolType_GlueGuide;
                    ParamPath.ParaName = MyGlobal.FindPointType_FitLineSet + "_" + "Side"+(i+1).ToString();
                    ParamPath.IsRight = isRight;
                    StaticOperate.WriteXML(fptool.fParam[i], ParamPath.ParamDir + "Side" + (i + 1).ToString() + ".xml");
                }

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
            if (!ok || isLoading)
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
                    //ChangeAxisName(MyGlobal.globalPointSet_Right.Startpt);
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
            int SideId = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            if (isRight)
            {
                textBox_xOffset.Text = MyGlobal.globalPointSet_Right.gbParam[SideId].Xoffset.ToString();
                textBox_yOffset.Text = MyGlobal.globalPointSet_Right.gbParam[SideId].Yoffset.ToString();
                cb_IsUp.Checked = MyGlobal.globalPointSet_Right.IsUp[SideId];
            }
            else
            {
                textBox_xOffset.Text = MyGlobal.globalPointSet_Left.gbParam[SideId].Xoffset.ToString();
                textBox_yOffset.Text = MyGlobal.globalPointSet_Left.gbParam[SideId].Yoffset.ToString();
                cb_IsUp.Checked = MyGlobal.globalPointSet_Left.IsUp[SideId];
            }
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (isRight)
                {
                    if (MessageBox.Show("是否保存右工位参数", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    if (MessageBox.Show( "是否保存左工位参数", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
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

                //切换物料  Surface_Curvature
                try
                {
                        if (MyGlobal.globalConfig.enableAlign)
                        {
                           
                            if (MyGlobal.PathName.CurrentType.Contains("_SurfaceCurvature"))
                            {
                                return;
                            }
                            string Currenttype = MyGlobal.PathName.CurrentType + "_SurfaceCurvature";
                            if (!Directory.Exists(MyGlobal.AllTypePath + MyGlobal.PathName.CurrentType + "_SurfaceCurvature"))
                            {
                                string pathCurrent = isRight ? MyGlobal.ConfigPath_Right : MyGlobal.ConfigPath_Left;
                                MyGlobal.PathName.CurrentType = Currenttype;

                                CopyFiles(pathCurrent, MyGlobal.ConfigPath_Right);

                                CopyFiles(pathCurrent, MyGlobal.ConfigPath_Left);
                            }
                            MyGlobal.PathName.CurrentType = Currenttype;
                            ChangeType?.Invoke(Currenttype);
                        }
                        else
                        {
                            if (MyGlobal.PathName.CurrentType.Contains("_SurfaceCurvature"))
                            {
                                int ind = MyGlobal.PathName.CurrentType.IndexOf("_SurfaceCurvature");
                                string Currenttype = MyGlobal.PathName.CurrentType.Substring(0, ind);
                                MyGlobal.PathName.CurrentType = Currenttype;
                            }                                                     
                            ChangeType?.Invoke(MyGlobal.PathName.CurrentType);
                        }
                   
                }
                catch (Exception ex)
                {

                    MessageBox.Show("物料切换失败" + ex.Message);
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
        bool isLoading = false;
        private void GlobalParam_Load(object sender, EventArgs e)
        {
            isLoading = true;
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
            isLoading = false;


            //RepositoryItemComboBox w = (RepositoryItemComboBox)barEditItem2.Edit;
            //w.Items.Add("亮度图");
            //w.Items.Add("曲面图");
            //w.Items.Add("彩色图");
            comboBoxEdit1.EditValueChanged += ShowImgTypeChanged;
            comboBoxEdit1.EditValue = MyGlobal.globalConfig.ShowImgType;
        }
        private void ShowImgTypeChanged(object sender, EventArgs e)
        {
            if ("亮度图" == comboBoxEdit1.EditValue.ToString())
            {
                MyGlobal.isShowHeightImg = true;
                MyGlobal.isShowSurfaceImg = false;
            }
            else if ("曲面图" == comboBoxEdit1.EditValue.ToString())
            {
                MyGlobal.isShowHeightImg = true;
                MyGlobal.isShowSurfaceImg = true;
            }
            else
            {
                MyGlobal.isShowHeightImg = false;
                MyGlobal.isShowSurfaceImg = false;
            }
            MyGlobal.globalConfig.ShowImgType = comboBoxEdit1.EditValue.ToString();
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
        void CopyFiles(string FilePath, string NewPath)
        {
            DirectoryInfo dirinf = new DirectoryInfo(FilePath);
            FileSystemInfo[] fileinfo = dirinf.GetFileSystemInfos();
            for (int i = 0; i < fileinfo.Length; i++)
            {
                if (fileinfo[i] is FileInfo)
                {
                    File.Copy(fileinfo[i].FullName, NewPath + fileinfo[i].Name);
                }
                else
                {
                    string dir = "\\" + fileinfo[i].Name + "\\";
                    if (!Directory.Exists(NewPath + dir))
                    {
                        Directory.CreateDirectory(NewPath + dir);
                    }
                    CopyFiles(FilePath + dir, NewPath + dir);
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MyGlobal.globalConfig.enableAlign = checkBox1.Checked;
        }

        private void cb_Features_CheckedChanged(object sender, EventArgs e)
        {
            MyGlobal.globalConfig.enableFeature = cb_Features.Checked;
        }

        private void cb_IsUp_CheckedChanged(object sender, EventArgs e)
        {
            SideName = comboBox1.SelectedItem.ToString();
            int SideId = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            if (isRight)
            {
                MyGlobal.globalPointSet_Right.IsUp[SideId] = cb_IsUp.Checked;
            }
            else
            {
                MyGlobal.globalPointSet_Left.IsUp[SideId] = cb_IsUp.Checked;

            }
        }

        private void cb_UseFix_CheckedChanged(object sender, EventArgs e)
        {
            MyGlobal.globalConfig.isUseFix = cb_UseFix.Checked;
            
        }

        private void cb_UseSelfOffset_CheckedChanged(object sender, EventArgs e)
        {
            MyGlobal.globalConfig.isUseSelfOffset = cb_UseSelfOffset.Checked;

        }

        private void cb_Reverse_CheckedChanged(object sender, EventArgs e)
        {
            if (isRight)
            {
                MyGlobal.globalPointSet_Right.IsReverse = cb_Reverse.Checked;
            }
            else
            {
                MyGlobal.globalPointSet_Left.IsReverse = cb_Reverse.Checked;

            }
        }
    }

    
}