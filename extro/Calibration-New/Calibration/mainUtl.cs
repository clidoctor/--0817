using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;
using ChoiceTech.Halcon.Control;

namespace Calibration
{


    /**
     * 调整好参数，调用OutputPoint.GetLcPoint()即可获取所有的边缘点
     * 
     * 
     */
    public partial class mainUtl : UserControl
    {
        public Action<string> GetRobotIdxDelegate;
        
        public void SetValue(string idx, string[] pt)
        {
            Action x = () =>
            {
                NumTempx[int.Parse(idx) - 1].Value = (decimal)double.Parse(pt[1]);
                NumTempy[int.Parse(idx) - 1].Value = (decimal)double.Parse(pt[2]);
            };
            this.Invoke(x);
        }

        /// <summary>
        /// 图像是否自动适应窗口
        /// </summary>
        public bool isFitWindow = true;
        private string settingPath = "setting.xml";
        private string roiPath = "RoiLineCircle.roi";
        public delegate void LoadImage();
        public LoadImage load;
        CLGlobal Global = new CLGlobal();
        public List<RoiParam> roiparamList = new List<RoiParam>();
        RoiParam calibParam = new RoiParam();
        HTuple HomMat2Dxy = new HTuple();
        HTuple HomMat2Dz = new HTuple();

        HObject Image = new HObject();
       
        public void dmmy()
        {

        }
        public mainUtl(string Path = "")
        {
            InitializeComponent();
            settingPath = Path + "setting.xml";
            roiPath = Path + "RoiLineCircle.roi";
            load = new LoadImage(dmmy);
            IniParam();
        }

        //public void Init(string Path = "")
        //{
        //    settingPath = Path + "setting.xml";
        //    roiPath = Path + "RoiLineCircle.roi";
        //    load = new LoadImage(dmmy);
        //    IniParam();
        //}
        bool IsLoading = false;
        private void MainUtl_Load(object sender, EventArgs e)
        {
            IsLoading = true;
            this.listbox_tools.SelectedIndexChanged += new System.EventHandler(this.listbox_tools_SelectedIndexChanged);
            //Global.ShowXval += OnShowXval;
            //Global.ShowYval += OnShowYval;
            CLGlobal.MsgBox = richTextBox1;

            hWindow_Final1.hWindowControl.MouseDown += hWindow_Final1_MouseMove;
            hWindow_Final1.hWindowControl.MouseClick += hWindow_Final1_MouseMove;
            hWindow_Final1.hWindowControl.MouseMove += hWindow_Final1_MouseMove;
            this.comboBox5.SelectedIndex = 0;
            for (int i = 0; i < 16; i++)
            {
                PointStr.Add("");
                calibParam.intersectCoord.Add(new IntersetionCoord());
            }
           
            LoadToUI();
           
          
            IsLoading = false;
            b1.Click += btn_get_idx_Click;
            b2.Click += btn_get_idx_Click;
            b3.Click += btn_get_idx_Click;
            b4.Click += btn_get_idx_Click;
            b5.Click += btn_get_idx_Click;
            b6.Click += btn_get_idx_Click;
            b7.Click += btn_get_idx_Click;
            b8.Click += btn_get_idx_Click;
            b9.Click += btn_get_idx_Click;
        }

        private void btn_get_idx_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            GetRobotIdxDelegate?.Invoke(btn.Name.Substring(1,1));
        }



        private void OnShowXval(string s)
        {
            lb_circle_center_x.Text = s;
        }
        private void OnShowYval(string s)
        {
            lb_circle_center_y.Text = s;
        }
        public void IniParam()
        {
            listbox_tools.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            if (File.Exists(settingPath))
            {
              roiparamList  = (List<RoiParam>)XmlSeriazlize.LoadFormXml(settingPath, typeof(List<RoiParam>));
                
                for (int i = 0; i <  roiparamList  .Count; i++)
                {
                    if ( roiparamList  [i].roi_name != null)
                    {
                        listbox_tools.Items.Add( roiparamList  [i].roi_name);
                        if (roiparamList[i].roi_name.Contains("直线"))
                        {
                            comboBox1.Items.Add(roiparamList[i].roi_name);
                            comboBox2.Items.Add(roiparamList[i].roi_name);
                        }
                        else
                        {
                            comboBox3.Items.Add(roiparamList[i].roi_name);
                        }
                    }
                    //if ( roiparamList  [i].roi_name.Contains("直线"))
                    //{
                    //    hWindow_Final1.viewWindow.genLine( roiparamList  [i].roi_row_start,  roiparamList  [i].roi_col_start,  roiparamList  [i].roi_row_end,  roiparamList  [i].roi_col_end, ref roiList);
                    //}
                    //else if ( roiparamList  [i].roi_name.Contains("圆弧"))
                    //{
                    //    hWindow_Final1.viewWindow.genCircle( roiparamList  [i].roi_row_start,  roiparamList  [i].roi_col_start,  roiparamList  [i].roi_row_end, ref roiList);
                    //}
                    //else if ( roiparamList  [i].roi_name.Contains("定线"))
                    //{
                    //    hWindow_Final1.viewWindow.genLine( roiparamList  [i].roi_row_start,  roiparamList  [i].roi_col_start,  roiparamList  [i].roi_row_end,  roiparamList  [i].roi_col_end, ref roiList);
                    //}
                    //else if ( roiparamList  [i].roi_name.Contains("定弧"))
                    //{
                    //    hWindow_Final1.viewWindow.genCircle( roiparamList  [i].roi_row_start,  roiparamList  [i].roi_col_start,  roiparamList  [i].roi_row_end, ref roiList);
                    //}
                    int toolIdx = Global.ToolIndexer.IndexOf(int.Parse( roiparamList  [i].roi_name.Substring(0, 1)));
                    if (toolIdx!=-1)
                    {
                        Global.ToolIndexer.RemoveAt(toolIdx);
                    }
             
                }
            }
            #region 参数修改事件
            if (listbox_tools.Items.Count != 0)
            {
                listbox_tools.SelectedIndex = 0;
                switch (roiparamList[listbox_tools.SelectedIndex].edge_select)
                {
                    case "所有":
                        cmb_select.SelectedItem = "all";
                        break;
                    case "第一点":
                        cmb_select.SelectedItem = "first";
                        break;
                    case "最后一点":
                        cmb_select.SelectedItem = "last";
                        break;
                }
                switch (roiparamList[listbox_tools.SelectedIndex].edge_transition)
                {
                    case "所有":
                        cmb_transition.SelectedItem = "all";
                        break;
                    case "由明到暗":
                        cmb_transition.SelectedItem = "negative";
                        break;
                    case "由暗到明":
                        cmb_transition.SelectedItem = "positive";
                        break;
                }             
                com_circle_oriente.SelectedItem =  roiparamList  [listbox_tools.SelectedIndex].circle_oriente;
                numx_meterbox_height.Value =  roiparamList  [listbox_tools.SelectedIndex].meterbox_height;
                numx_meterbox_width.Value =  roiparamList  [listbox_tools.SelectedIndex].meterbox_width;
                numx_meterbox_num.Value =  roiparamList  [listbox_tools.SelectedIndex].meterbox_num;
                numx_circle_startAng.Value =  roiparamList  [listbox_tools.SelectedIndex].circle_startAng;
                numx_circle_endAng.Value =  roiparamList  [listbox_tools.SelectedIndex].circle_endAng;
                numx_sigma.Value = (decimal) roiparamList  [listbox_tools.SelectedIndex].edge_sigma;
                numx_threshold.Value = (decimal) roiparamList  [listbox_tools.SelectedIndex].edge_threshold;
                numx_offset_x.Value = (decimal) roiparamList  [listbox_tools.SelectedIndex].xOffset;
                numx_offset_y.Value = (decimal) roiparamList  [listbox_tools.SelectedIndex].yOffset;
            }


            cmb_select.SelectedIndexChanged += cmb_param_update;
            cmb_transition.SelectedIndexChanged += cmb_param_update;
            com_circle_oriente.SelectedIndexChanged += cmb_param_update;

            numx_meterbox_height.ValueChanged += numx_param_update;
            numx_meterbox_width.ValueChanged += numx_param_update;
            numx_meterbox_num.ValueChanged += numx_param_update;
            numx_circle_endAng.ValueChanged += numx_param_update;
            numx_circle_startAng.ValueChanged += numx_param_update;
            numx_sigma.ValueChanged += numx_param_update;
            numx_threshold.ValueChanged += numx_param_update;
            numx_offset_x.ValueChanged += numx_param_update;
            numx_offset_y.ValueChanged += numx_param_update;
            #endregion
        }

        private void numx_param_update(object sender, EventArgs e)
        {
            if (listbox_tools.SelectedIndex < 0)
            {
                return;
            }
            NumericUpDown numx = sender as NumericUpDown;
            switch (numx.Name)
            {
                case "numx_meterbox_width":
                     roiparamList  [listbox_tools.SelectedIndex].meterbox_width = (int)numx_meterbox_width.Value;
                    break;
                case "numx_meterbox_height":
                     roiparamList  [listbox_tools.SelectedIndex].meterbox_height = (int)numx_meterbox_height.Value;
                    break;
                case "numx_meterbox_num":
                     roiparamList  [listbox_tools.SelectedIndex].meterbox_num = (int)numx_meterbox_num.Value;
                    break;
                case "numx_sigma":
                     roiparamList  [listbox_tools.SelectedIndex].edge_sigma = (double)numx_sigma.Value;
                    break;
                case "numx_threshold":
                     roiparamList  [listbox_tools.SelectedIndex].edge_threshold = (double)numx_threshold.Value;
                    break;
                case "numx_circle_startAng":
                    if (numx_circle_endAng.Value < (numx_circle_startAng.Value + 10))
                    {
                        numx_circle_startAng.Value =  roiparamList  [listbox_tools.SelectedIndex].circle_startAng;
                        break;
                    }
                     roiparamList  [listbox_tools.SelectedIndex].circle_startAng = (int)numx_circle_startAng.Value;
                    break;
                case "numx_circle_endAng":
                    if (numx_circle_endAng.Value < (numx_circle_startAng.Value + 10))
                    {
                        numx_circle_endAng.Value =  roiparamList  [listbox_tools.SelectedIndex].circle_endAng;
                        break;
                    }
                     roiparamList  [listbox_tools.SelectedIndex].circle_endAng = (int)numx_circle_endAng.Value;
                    break;
                case "numx_offset_x":
                     roiparamList  [listbox_tools.SelectedIndex].xOffset = (double)numx_offset_x.Value;
                    break;
                case "numx_offset_y":
                     roiparamList  [listbox_tools.SelectedIndex].yOffset = (double)numx_offset_y.Value;
                    break;
                default:
                    break;
            }
            listbox_tools_SelectedIndexChanged(sender, e);
        }
        private void cmb_param_update(object sender, EventArgs e)
        {
            if (listbox_tools.SelectedIndex<0)
            {
                return;
            }
            ComboBox cmb = sender as ComboBox;
            switch (cmb.Name)
            {
                case "cmb_select":                    
                    switch (cmb_select.Text)
                    {
                        case "所有":
                            roiparamList[listbox_tools.SelectedIndex].edge_select = "all";
                            break;
                        case "第一点":
                            roiparamList[listbox_tools.SelectedIndex].edge_select = "first";
                            break;
                        case "最后一点":
                            roiparamList[listbox_tools.SelectedIndex].edge_select = "last";
                            break;
                    }                   
                    break;
                case "cmb_transition":

                    switch (cmb_transition.Text)
                    {
                        case "所有":
                            roiparamList[listbox_tools.SelectedIndex].edge_transition = "all";
                            break;
                        case "由明到暗":
                            roiparamList[listbox_tools.SelectedIndex].edge_transition = "negative";
                            break;
                        case "由暗到明":
                            roiparamList[listbox_tools.SelectedIndex].edge_transition = "positive";
                            break;
                    }
                    break;
                case "com_circle_oriente":
                     roiparamList  [listbox_tools.SelectedIndex].circle_oriente = com_circle_oriente.Text;
                    break;
                default:
                    break;
            }
            listbox_tools_SelectedIndexChanged(sender, e);
        }

        HObject OrigImg; HObject OrigHeightImg;
        HTuple ImgWidth, ImgHeight;
        private void btn_load_img_Click(object sender, EventArgs e)
        {
            string filename = Global.Selectpath();
            if (filename != "")
            {
                for (int i = 0; i < 4; i++)
                {
                    if (filename.Contains((i+1).ToString()+ "I.tiff") || filename.Contains((i + 1).ToString() + "H.tiff"))
                    {
                     
                        comboBox5.SelectedIndex = i;
                    }
                }
                

                HObject tempImg = new HObject();
                HOperatorSet.ReadImage(out tempImg, filename);
                hWindow_Final1.viewWindow.setFitWindow(isFitWindow);
                hWindow_Final1.HobjectToHimage(tempImg);               
                HOperatorSet.GetImageSize(tempImg, out ImgWidth, out ImgHeight);
                HObject region = new HObject();
                HOperatorSet.GenRectangle1(out region, 0, 0, ImgHeight, ImgWidth);
                HTuple min, max, range;
                HOperatorSet.MinMaxGray(region, tempImg, 0, out min, out max, out range);
                if (max>200)
                {
                    if (OrigImg != null)
                    {
                        OrigImg.Dispose();
                     
                    }
                    OrigImg = tempImg;
                }
                else
                {
                    if (OrigHeightImg != null)
                    {
                        OrigHeightImg.Dispose();
                    }
                    OrigHeightImg = tempImg;
                }
                //HOperatorSet.SetMetrologyModelImageSize(Global.metrologyHandle, ImgWidth, ImgHeight);

                //this.hWindow_Final1.hWindowControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hWindow_Final1_MouseMove);
                this.hWindow_Final1.hWindowControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.hWindow_Final1_MouseMove);
                //this.hWindow_Final1.hWindowControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.hWindow_Final1_MouseMove);
                btn_add_tool.Enabled = true;
                btn_show_all_dot.Enabled = true;
                groupBox2.Enabled = true;
               
                IniParam();
                roiList.Clear();
                if (roiList.Count==0)
                {
                    if (File.Exists(roiPath))
                    {
                        hWindow_Final1.viewWindow.loadROI(roiPath, out roiList);

                    }
                }
                else
                {

                    hWindow_Final1.viewWindow.displayROI(ref roiList);
                }

                load();
            }
            
        }


        public List<ViewWindow.Model.ROI> roiList = new List<ViewWindow.Model.ROI>();
      
        private void btn_add_tool_Click(object sender, EventArgs e)
        {
            int tool_idx = listbox_tools.Items.Count;
            string tool_name = Global.ToolIndexer[0] + "--" + cmb_tool.Text;
            listbox_tools.Items.Add(tool_name);
           

            if (cmb_tool.Text.Contains("线"))
            {
                comboBox1.Items.Add(tool_name);
                comboBox2.Items.Add(tool_name);


                hWindow_Final1.viewWindow.genLine(ImgHeight/5, ImgWidth / 5, ImgHeight / 2, ImgWidth / 5, ref roiList);
                RoiParam roiparam = new RoiParam();

                roiparam.Set(true, ImgHeight / 5, ImgWidth / 5, ImgHeight / 2, ImgWidth / 5, listbox_tools.Items[tool_idx].ToString(), cmb_select.Text, cmb_transition.Text, (int)numx_meterbox_height.Value,
                    (int)numx_meterbox_width.Value, (int)numx_meterbox_num.Value, (double)numx_sigma.Value,
                    (double)numx_threshold.Value, 0, (int)numx_circle_startAng.Value,
                    (int)numx_circle_endAng.Value, com_circle_oriente.Text);
                 roiparamList  .Add(roiparam);
            }
            else if (cmb_tool.Text.Contains("弧"))
            {
                comboBox3.Items.Add(tool_name);

                hWindow_Final1.viewWindow.genCircle(500, 500, 200, ref roiList);
                RoiParam roiparam = new RoiParam();
                roiparam.Set(true, 500, 500, 200, 0, listbox_tools.Items[tool_idx].ToString(), cmb_select.Text, cmb_transition.Text, (int)numx_meterbox_height.Value,
                    (int)numx_meterbox_width.Value, (int)numx_meterbox_num.Value, (double)numx_sigma.Value,
                    (double)numx_threshold.Value, 0, 0,360, com_circle_oriente.Text);
                 roiparamList  .Add(roiparam);
            }
            listbox_tools.SelectedIndex = tool_idx;
            Global.ToolIndexer.RemoveAt(0);
        }


        OutputPoint Output= new OutputPoint();
        private void hWindow_Final1_MouseMove(object sender, MouseEventArgs e)
        {
          

           int activeROIIdx = hWindow_Final1.viewWindow._roiController.activeROIidx;
            HObject OrigImg = hWindow_Final1.Image;
            if (OrigImg==null ||!OrigImg.IsInitialized()|| activeROIIdx == -1)
            {
                return;
            }
            if (e.Button==MouseButtons.None)
            {
                return;
            }
            if (activeROIIdx >=listbox_tools.Items.Count)
            {
                return;
            }
            listbox_tools.SelectedIndex = activeROIIdx;

            if (roiList.Count==0)
            {
                return;

            }

            HTuple roiData = roiList[listbox_tools.SelectedIndex].getModelData();
            if (listbox_tools.SelectedItem.ToString().Contains("直线"))
            {
                if ( roiparamList  [listbox_tools.SelectedIndex].meterbox_width!= (int)numx_meterbox_width.Value)
                {
                    numx_meterbox_width.Value =  roiparamList  [listbox_tools.SelectedIndex].meterbox_width;
                }

                 roiparamList  [listbox_tools.SelectedIndex].roi_row_start = roiData[0];
                 roiparamList  [listbox_tools.SelectedIndex].roi_col_start = roiData[1];
                 roiparamList  [listbox_tools.SelectedIndex].roi_row_end = roiData[2];
                 roiparamList  [listbox_tools.SelectedIndex].roi_col_end = roiData[3];
                //HTuple hv_RowStart, hv_ColStart, hv_RowEnd, hv_ColEnd;
                //Output.GetVerticaLine(roiData[2], roiData[3], roiData[0], roiData[1],out hv_RowStart, out hv_ColStart, out hv_RowEnd, out hv_ColEnd);
                //HOperatorSet.SetColor(hWindow_Final1.hWindowControl.HalconWindow, "green");
                //HOperatorSet.DispArrow(hWindow_Final1.hWindowControl.HalconWindow, hv_RowEnd, hv_ColEnd, hv_RowStart, hv_ColStart, 8);
                HTuple rows, cols,linecoord;
                Output.GetMetrologyRect(OrigImg,  roiparamList  [listbox_tools.SelectedIndex], out rows, out cols,out linecoord, hWindow_Final1);
                
            }
            else if (listbox_tools.SelectedItem.ToString().Contains("定线"))
            {
                 roiparamList  [listbox_tools.SelectedIndex].roi_row_start = roiData[0];
                 roiparamList  [listbox_tools.SelectedIndex].roi_col_start = roiData[1];
                 roiparamList  [listbox_tools.SelectedIndex].roi_row_end = roiData[2];
                 roiparamList  [listbox_tools.SelectedIndex].roi_col_end = roiData[3];
                HTuple rows, cols;
                Output.GetMetrologyFixline( roiparamList  [listbox_tools.SelectedIndex], out rows, out cols, hWindow_Final1);
            }
            else if (listbox_tools.SelectedItem.ToString().Contains("圆弧"))
            {
                 roiparamList  [listbox_tools.SelectedIndex].roi_row_start = roiData[0];
                 roiparamList  [listbox_tools.SelectedIndex].roi_col_start = roiData[1];
                 roiparamList  [listbox_tools.SelectedIndex].roi_row_end = roiData[2];
                 roiparamList  [listbox_tools.SelectedIndex].roi_col_end = 0;
                HTuple rows, cols,circlecoord;
                Output.GetMetrologyCircle(OrigImg,  roiparamList  [listbox_tools.SelectedIndex], out rows, out cols,out circlecoord, hWindow_Final1);
            }
            else if (listbox_tools.SelectedItem.ToString().Contains("定弧"))
            {
                 roiparamList  [listbox_tools.SelectedIndex].roi_row_start = roiData[0];
                 roiparamList  [listbox_tools.SelectedIndex].roi_col_start = roiData[1];
                 roiparamList  [listbox_tools.SelectedIndex].roi_row_end = roiData[2];
                 roiparamList  [listbox_tools.SelectedIndex].roi_col_end = 0;
                HTuple rows, cols;
                Output.GetMetrologyFixCircle( roiparamList  [listbox_tools.SelectedIndex], out rows, out cols, hWindow_Final1);
            }
     
        }
        private void listbox_tools_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (listbox_tools.SelectedIndex == -1)
            {
                return;
            }
            HObject OrigImg = hWindow_Final1.Image;

            switch (roiparamList[listbox_tools.SelectedIndex].edge_select)
            {
                case "all":
                    cmb_select.SelectedItem = "所有";
                    break;
                case "first":
                    cmb_select.SelectedItem = "第一点";
                    break;
                case "last":
                    cmb_select.SelectedItem = "最后一点";
                    break;
            }
            switch (roiparamList[listbox_tools.SelectedIndex].edge_transition)
            {
                case "all":
                    cmb_transition.SelectedItem = "所有";
                    break;
                case "negative":
                    cmb_transition.SelectedItem = "由明到暗";
                    break;
                case "positive":
                    cmb_transition.SelectedItem = "由暗到明";
                    break;
            }

            //cmb_select.SelectedItem =  roiparamList  [listbox_tools.SelectedIndex].edge_select;
            //cmb_transition.SelectedItem =  roiparamList  [listbox_tools.SelectedIndex].edge_transition;
            com_circle_oriente.SelectedItem =  roiparamList  [listbox_tools.SelectedIndex].circle_oriente;
            numx_meterbox_height.Value =  roiparamList  [listbox_tools.SelectedIndex].meterbox_height;
            numx_meterbox_width.Value =  roiparamList  [listbox_tools.SelectedIndex].meterbox_width;
            numx_meterbox_num.Value =  roiparamList  [listbox_tools.SelectedIndex].meterbox_num;
            numx_circle_startAng.Value =  roiparamList  [listbox_tools.SelectedIndex].circle_startAng;
            numx_circle_endAng.Value =  roiparamList  [listbox_tools.SelectedIndex].circle_endAng;
            numx_sigma.Value = (decimal) roiparamList  [listbox_tools.SelectedIndex].edge_sigma;
            numx_threshold.Value = (decimal) roiparamList  [listbox_tools.SelectedIndex].edge_threshold;
            numx_offset_x.Value = (decimal) roiparamList  [listbox_tools.SelectedIndex].xOffset;
            numx_offset_y.Value = (decimal) roiparamList  [listbox_tools.SelectedIndex].yOffset;
            hWindow_Final1.viewWindow.selectROI(listbox_tools.SelectedIndex);
            if (listbox_tools.SelectedItem.ToString().Contains("圆弧"))
            {
                HTuple rows, cols,line;
                Output.GetMetrologyCircle(OrigImg,  roiparamList  [listbox_tools.SelectedIndex], out rows, out cols,out line, hWindow_Final1);
            }
            else if (listbox_tools.SelectedItem.ToString().Contains("直线"))
            {
                HTuple rows, cols,circle;
                Output.GetMetrologyRect(OrigImg,  roiparamList  [listbox_tools.SelectedIndex], out rows, out cols,out circle, hWindow_Final1);
            }
            else if (listbox_tools.SelectedItem.ToString().Contains("定线"))
            {
                HTuple rows, cols;
                Output.GetMetrologyFixline( roiparamList  [listbox_tools.SelectedIndex], out rows, out cols, hWindow_Final1);
            }
            else if (listbox_tools.SelectedItem.ToString().Contains("定弧"))
            {
                HTuple rows, cols;
                Output.GetMetrologyFixCircle( roiparamList  [listbox_tools.SelectedIndex], out rows, out cols, hWindow_Final1);
            }
        }





        #region 右键菜单
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int removeIdx = listbox_tools.SelectedIndex;
            string select = listbox_tools.SelectedItem.ToString();
            if (removeIdx < 0)
            {
                return;
            }

            hWindow_Final1.viewWindow.removeActiveROI(ref roiList);
            Global.ToolIndexer.Insert(0,int.Parse( roiparamList  [removeIdx].roi_name.Substring(0,1)));
             roiparamList  .RemoveAt(removeIdx);
            listbox_tools.Items.RemoveAt(removeIdx);
            if (listbox_tools.Items.Count != 0)
            {
                listbox_tools.SelectedIndex = 0;
            }

            if (select.Contains("线"))
            {
                comboBox1.Items.Remove(select);
                comboBox2.Items.Remove(select);
            }
            else
            {
                comboBox3.Items.Remove(select);
            }

          CLGlobal.DispMsg("工具"+ removeIdx + "清除完成");
        }

        private void 删除所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            roiList.Clear();
             roiparamList  .Clear();
            listbox_tools.Items.Clear();
            hWindow_Final1.viewWindow.notDisplayRoi();
            Global.ToolIndexer.Clear();
            for (int i = 1; i < 100; i++)
            {
                Global.ToolIndexer.Add(i);
            }
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            CLGlobal.DispMsg("所有工具清除完成");
        }

        private void 上移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int moveIdx = listbox_tools.SelectedIndex;
            if (moveIdx == 0)
            {
                return;
            }
            listbox_tools.Items.Insert(moveIdx - 1,  roiparamList  [moveIdx].roi_name);
             roiparamList  .Insert(moveIdx - 1,  roiparamList  [moveIdx]);
            roiList.Insert(moveIdx - 1, roiList[moveIdx]);

            listbox_tools.Items.RemoveAt(moveIdx + 1);
             roiparamList  .RemoveAt(moveIdx + 1);
            roiList.RemoveAt(moveIdx + 1);
            listbox_tools.SelectedIndex = moveIdx - 1;
        }
        private void 下移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int moveIdx = listbox_tools.SelectedIndex;
            if (moveIdx == listbox_tools.Items.Count - 1)
            {
                return;
            }
            listbox_tools.Items.Insert(moveIdx + 2,  roiparamList  [moveIdx].roi_name);
             roiparamList  .Insert(moveIdx + 2,  roiparamList  [moveIdx]);
            roiList.Insert(moveIdx + 2, roiList[moveIdx]);

            listbox_tools.Items.RemoveAt(moveIdx);
             roiparamList  .RemoveAt(moveIdx);
            roiList.RemoveAt(moveIdx);
            listbox_tools.SelectedIndex = moveIdx +1;
        }
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int copyIdx = listbox_tools.SelectedIndex;
            int newIdx = listbox_tools.Items.Count;
            string newTooName = Global.ToolIndexer[0] + "--" + cmb_tool.Text;

            listbox_tools.Items.Add(newTooName);
            if (newTooName.Contains("线"))
            {
                comboBox1.Items.Add(newTooName);
                comboBox2.Items.Add(newTooName);
            }
            else
            {
                comboBox3.Items.Add(newTooName);

            }
            HTuple data = roiList[copyIdx].getModelData();
            hWindow_Final1.viewWindow.genLine(data[0], data[1] + 100, data[2], data[3] + 100, ref roiList);

            RoiParam roiparam = new RoiParam();
            roiparam.Set(true, data[0], data[1] + 100, data[2], data[3] + 100, newTooName,
                cmb_select.Text, cmb_transition.Text,  roiparamList  [copyIdx].meterbox_height,
                 roiparamList  [copyIdx].meterbox_width,  roiparamList  [copyIdx].meterbox_num,  roiparamList  [copyIdx].edge_sigma,
                 roiparamList  [copyIdx].edge_threshold,  roiparamList  [copyIdx].roi_score,  roiparamList  [copyIdx].circle_startAng,
                 roiparamList  [copyIdx].circle_endAng,  roiparamList  [copyIdx].circle_oriente);
             roiparamList  .Add(roiparam);
            Global.ToolIndexer.RemoveAt(0);
        }
        #endregion
        private void btn_save_param_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 9; i++)
            {
                double x = (double)NumTempx[i].Value;
                double y = (double)NumTempy[i].Value;
                double z = (double)NumTempz[i].Value;

                //double PIX_x = (double)NumPIX_x[i].Value;
                //double PIX_y = (double)NumPIX_y[i].Value;

                double PIX_x = (double)Pix_X[i].D;
                double PIX_y = (double)Pix_Y[i].D;
                double PIX_z = (double)Pix_Z[i].D;

                NumPIX_x[i].Value = (decimal)PIX_x;
                NumPIX_y[i].Value = (decimal)PIX_y;
                NumPIX_z[i].Value = (decimal)PIX_z;

                //AxisX = AxisX.TupleConcat(x);
                //AxisY = AxisY.TupleConcat(y);
                //Pix_X = Pix_X.TupleConcat(PIX_x);
                //Pix_Y = Pix_Y.TupleConcat(PIX_y);
                string data = x.ToString() + "\t" + y.ToString() + "\t" + z.ToString() + "\t" + PIX_x.ToString() + "\t" + PIX_y.ToString() + "\t" + PIX_z.ToString() + "\r\n";
                sb.Append(data);
            }
            if (File.Exists(ParamPath.Path_txt))
            {
                File.Delete(ParamPath.Path_txt);
            }
            ParamPath.WriteTxt(ParamPath.Path_txt, sb.ToString());
            bool a = XmlSeriazlize.SaveToXML(settingPath,  roiparamList  , typeof(List<RoiParam>), null);            
            hWindow_Final1.viewWindow.saveROI(roiList, roiPath);
            bool b = XmlSeriazlize.SaveToXML(ParamPath.Path_Param, calibParam, typeof(RoiParam), null);
            bool c = a && b;
            CLGlobal.DispMsg(c ? "保存成功" : "保存失败");
        }

        private void btn_show_all_dot_Click(object sender, EventArgs e)
        {
           

            HObject OrigImg = hWindow_Final1.Image;
            //HOperatorSet.ClearMetrologyObject(Global.metrologyHandle, "all");
            List<HTuple> rows, cols,coord;
            Output.GetLcPoint(OrigImg, roiparamList, out rows, out cols,out coord, hWindow_Final1);
            DataTable dt = new DataTable();

            dataGridView1.DataSource = dt;

            dt.Columns.Add("id", typeof(string), "");
            dt.Columns.Add("x", typeof(string), "");
            dt.Columns.Add("y", typeof(string), "");
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            int cnt = 1;
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < rows[i].TupleLength(); j++)
                {
                    dt.Rows.Add(new string[] { cnt.ToString(), rows[i].TupleSelect(j).TupleString(".6"), cols[i].TupleSelect(j).TupleString(".6") });
                    cnt++;
                }
            }


            //HOperatorSet.CreateMetrologyModel(out Global.metrologyHandle);
        }
       
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Global.IsShowCircle = checkBox1.Checked;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        List<string> PointStr = new List<string>();
        HTuple[] rotateX = new HTuple[16];
        HTuple[] rotateY = new HTuple[16];

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || hWindow_Final1.Image ==null || calibParam.PointCount >= 16)
                {
                    return;
                }
                string selet1 = comboBox1.SelectedItem.ToString();
                string selet2 = comboBox2.SelectedItem.ToString();
                int iL1 = Convert.ToInt32(selet1.Substring(0, selet1.Length - 4)) - 1;
                int iL2 = Convert.ToInt32(selet2.Substring(0, selet2.Length - 4)) - 1 ;

                if (iL1 == iL2)
                {
                    return;
                }
                List<HTuple> hv_Row_all = new List<HTuple>();
                List<HTuple> hv_Column_all = new List<HTuple>();
                List<HTuple> Coord = new List<HTuple>();
                Output.GetLcPoint(OrigImg, roiparamList, out hv_Row_all, out hv_Column_all, out Coord, hWindow_Final1);
                if (Coord.Count > 1)
                {
                    HTuple Row, Col, isOverlapping;
                    if (Coord[iL1].Length != 4)
                    {
                        MessageBox.Show(comboBox1.SelectedItem.ToString() + "未找到！");
                        return;
                    }
                    if (Coord[iL2].Length != 4)
                    {
                        MessageBox.Show(comboBox2.SelectedItem.ToString() + "未找到！");
                        return;

                    }
                    HOperatorSet.IntersectionLines(Coord[iL1][0], Coord[iL1][1], Coord[iL1][2], Coord[iL1][3],
                            Coord[iL2][0], Coord[iL2][1], Coord[iL2][2], Coord[iL2][3], out Row, out Col, out isOverlapping);
                    if (Row.Length == 0)
                    {
                        MessageBox.Show("无交点！");
                        return;
                    }
                    HObject cross = new HObject();
                    HOperatorSet.GenCrossContourXld(out cross, Row, Col, 10, 0);
                    HOperatorSet.SetColor(hWindow_Final1.HWindowHalconID, "green");
                    HOperatorSet.DispObj(cross,hWindow_Final1.HWindowHalconID);


                    //高度
                    HTuple Zpoint = 0;

                    //if (OrigHeightImg!=null && OrigHeightImg.IsInitialized())
                    //{
                    //    //获取高度
                    //    //除去 -30 的点
                    //    HOperatorSet.GetGrayval(OrigHeightImg, Row, Col, out Zpoint);
                    //    if (Zpoint.D==-30)
                    //    {
                    //        HTuple row1 = Row.TupleConcat(Row.D + 4, Row.D + 8, Row.D - 4, Row.D - 8);
                    //        HTuple col1 = Col.TupleConcat(Col.D + 4, Col.D + 2, Col.D - 4, Col.D - 8);
                    //        HOperatorSet.GetGrayval(OrigHeightImg, row1, col1, out Zpoint);
                    //        //除去 -100 的点
                    //        HTuple eq100 = new HTuple();
                    //        HOperatorSet.TupleGreaterElem(Zpoint, -10, out eq100);
                    //        HTuple eq100Id = new HTuple();
                    //        HOperatorSet.TupleFind(eq100, 1, out eq100Id);
                    //        if (eq100Id != -1)
                    //        {
                    //            HOperatorSet.TupleMean(Zpoint[eq100Id], out Zpoint);
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("无有效Z！");
                    //            return;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("未加载高度图");
                    //    return;
                    //}


                    Button bt = (Button)sender;
                    tb_ix.Text = Math.Round(Row.D, 3).ToString();
                    tb_iy.Text = Math.Round(Col.D, 3).ToString();
                    tb_iz.Text = Math.Round(Zpoint.D, 3).ToString();

                    if (bt.Text.Contains("添加")|| bt.Text.Contains("高度"))
                    {
                        IntersetionCoord coord = new IntersetionCoord();
                        coord.Row = Math.Round(Row.D, 3);
                        coord.Col = Math.Round(Col.D, 3);
                        coord.Zpoint = Math.Round(Zpoint.D, 3);
                        coord.LorC = "L";
                        calibParam.PointCount += 1;
                        for (int i = 1; i <= calibParam.PointCount; i++)
                        {
                            if (!PointStr.Contains( i.ToString() + "_Point" ))
                            {
                                listBox1.Items.Add(i.ToString() + "_Point");

                                //排序
                                List<int> id = new List<int>();
                                for (int j = 0; j < listBox1.Items.Count; j++)
                                {
                                    string item = listBox1.Items[j].ToString();
                                    int itemId = Convert.ToInt32(item.Remove(item.Length - 6, 6));
                                    id.Add(itemId);
                                }
                                listBox1.Items.Clear();
                                id.Sort();
                                for (int k = 0; k < id.Count; k++)
                                {
                                    listBox1.Items.Add(id[k].ToString() + "_Point");
                                }

                                PointStr[i -1] = (i.ToString() + "_Point");

                                //机械高度
                                if (bt.Text.Contains("高度"))
                                {
                                    PanelTemp[i - 1].Visible = true;
                                    NumTempz[i - 1].Value = (decimal)coord.Zpoint;
                                    rotateX[i - 1] = coord.Row;
                                    rotateY[i - 1] = coord.Col;

                                }
                                else
                                {
                                    if (calibParam.PointCount <= NumPIX_x.Length)
                                    {
                                        PanelTemp[i - 1].Visible = true;
                                        NumPIX_x[i - 1].Value = (decimal)coord.Row;
                                        NumPIX_y[i - 1].Value = (decimal)coord.Col;
                                        NumPIX_z[i - 1].Value = (decimal)coord.Zpoint;
                                        calibParam.intersectCoord[i - 1] = (coord);
                                    }
                                }                             

                                break;
                            }
                        }
                
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        NumericUpDown[] NumTempx = new NumericUpDown[16];
        NumericUpDown[] NumTempy = new NumericUpDown[16];
        NumericUpDown[] NumTempz = new NumericUpDown[16];
        NumericUpDown[] NumPIX_x = new NumericUpDown[16];
        NumericUpDown[] NumPIX_y = new NumericUpDown[16];
        NumericUpDown[] NumPIX_z = new NumericUpDown[16];
        Panel[] PanelTemp = new Panel[16];
        public static object[] Do(params object[] param)
        {
            try
            {
                if (param == null) { return null; }
                List<object[]> LO = new List<object[]>();
                int sum = 1;
                int n = 0;
                foreach (object o in param)
                {
                    if (o is object[])
                    {
                        object[] _o = (object[])o;
                        LO.Add(_o);
                        sum *= _o.Length;
                        n++;
                    }
                }
                object[] __o = new object[sum];
                for (int i = 0; i < sum; i++)
                {
                    object[] _o = new object[n];
                    for (int j = 0; j < n; j++)
                    {
                        object[] o = LO[j];
                        int a = (i * Index(LO, j) / sum) % o.Length;

                        if (a < o.Length && a >= 0)
                        {
                            _o[j] = o[a];
                        }

                    }
                    __o[i] = _o;
                }
                return __o;
            }
            catch (Exception)
            {

                throw;
            }

        }
        private static int Index(List<object[]> LO, int lindex)
        {
            try
            {
                int n = 1;
                for (int i = 0; i < LO.Count; i++)
                {

                    if (i <= lindex)
                    {
                        n = n * LO[i].Length;
                    }
                    else
                    {
                        break;
                    }
                }
                return n;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void GetBest()
        {
            StringBuilder sb = new StringBuilder();

            AxisX = new HTuple();
            AxisY = new HTuple();
            AxisZ = new HTuple();
            Pix_X = new HTuple();
            Pix_Y = new HTuple();
            Pix_Z = new HTuple();

            HTuple rotatex = new HTuple();
            HTuple rotatey = new HTuple();

            int Count = 0;
            if (checkBox2.Checked)//手动输入
            {

                for (int i = 0; i < 16; i++)
                {
                    if (0 != (int)NumPIX_x[i].Value)
                    {
                        Count++;
                    }
                }
            }
            else
            {
                Count = calibParam.PointCount;
            }
            if (Count < 3)
            {
                return;
            }

            for (int i = 0; i < Count; i++)
            {
                double x = (double)NumTempx[i].Value;
                double y = (double)NumTempy[i].Value;
                double z = (double)NumTempz[i].Value;
                double PIX_x = calibParam.intersectCoord[i].Row;
                double PIX_y = calibParam.intersectCoord[i].Col;
                double PIX_z = calibParam.intersectCoord[i].Zpoint;

                AxisX = AxisX.TupleConcat(x);
                AxisY = AxisY.TupleConcat(y);
                AxisZ = AxisZ.TupleConcat(z);

                Pix_X = Pix_X.TupleConcat(PIX_x);
                Pix_Y = Pix_Y.TupleConcat(PIX_y);
                Pix_Z = Pix_Z.TupleConcat(PIX_z);
                if (rotateX[i] !=null)
                {
                    rotatex = rotatex.TupleConcat(rotateX[i]);
                    rotatey = rotatey.TupleConcat(rotateY[i]);
                }
                else
                {

                }
                

                string data = x.ToString() + "\t" + y.ToString() + "\t" + z.ToString() + "\t" + PIX_x.ToString() + "\t" + PIX_y.ToString() + "\t" + PIX_z.ToString() + "\r\n";
                sb.Append(data);
            }
            if (File.Exists(ParamPath.Path_txtBackUp))
            {
                File.Delete(ParamPath.Path_txtBackUp);
            }
            ParamPath.WriteTxt(ParamPath.Path_txtBackUp, sb.ToString());
            if (checkBox1.Checked)
            {
                int count = calibParam.PointCount;
                if (count > 6)
                {
                    AxisX = AxisX.TupleSelectRange(0, 5);
                    AxisY = AxisY.TupleSelectRange(0, 5);

                    count = 6;

                    for (int i = 6; i < 16; i++)
                    {
                        PanelTemp[i].Visible = false;
                    }
                }
                //选择最优
                object[] px = new object[count];
                object[] py = new object[count];
                object[] pxResult = new object[count];
                object[] pyResult = new object[count];
                for (int i = 0; i < count; i++)
                {
                    object[] x = { Pix_X[i].D - 2, Pix_X[i].D - 1, Pix_X[i].D - 0, Pix_X[i].D + 1, Pix_X[i].D + 2 };
                    object[] y = { Pix_Y[i].D - 2, Pix_Y[i].D - 1, Pix_Y[i].D - 0, Pix_Y[i].D + 1, Pix_Y[i].D + 2 };
                    px[i] = x;
                    py[i] = y;
                }
                pxResult = Do(px);
                pyResult = Do(py);
                object t = pxResult[pxResult.Length - 1];
                //pxResult = Do(px[0], px[1], px[2], px[3], px[4], px[5], px[6], px[7], px[8]);
                //pyResult = Do(py[0], py[1], py[2], py[3], py[4], py[5], py[6], py[7], py[8]);
                List<HTuple> hommat = new List<HTuple>(); HTuple Mx = new HTuple(); HTuple My = new HTuple();
                for (int i = 0; i < pxResult.Length; i++)
                {
                    HTuple row = new HTuple();
                    HTuple col = new HTuple();
                    object[] xx = (object[])pxResult[i];
                    object[] yy = (object[])pyResult[i];
                    for (int j = 0; j < xx.Length; j++)
                    {
                        row = row.TupleConcat((double)xx[j]);
                        col = col.TupleConcat((double)yy[j]);
                    }

                    HTuple hom = new HTuple();
                    HOperatorSet.VectorToHomMat2d(row, col, AxisX, AxisY, out hom);

                    HTuple real_x = new HTuple();
                    HTuple real_y = new HTuple();
                    HOperatorSet.AffineTransPoint2d(hom, row, col, out real_x, out real_y);
                    HTuple subx = AxisX - real_x;
                    HTuple suby = AxisY - real_y;
                    subx = subx.TupleAbs();
                    suby = suby.TupleAbs();
                    double max_x = subx.TupleMax();
                    double max_y = suby.TupleMax();
                    Mx = Mx.TupleConcat(max_x);
                    My = My.TupleConcat(max_y);
                    hommat.Add(hom);
                }
                HTuple Bestx = Mx.TupleMin();
                HTuple Idx = Mx.TupleFind(Bestx);
                HTuple Besty = My.TupleMin();
                HTuple Idy = My.TupleFind(Besty);
                HTuple bestrow = new HTuple();
                HTuple bestcol = new HTuple();
                object[] xxt = (object[])pxResult[Idx.I];
                object[] yyt = (object[])pyResult[Idy.I];
                for (int j = 0; j < xxt.Length; j++)
                {
                    bestrow = bestrow.TupleConcat((double)xxt[j]);
                    bestcol = bestcol.TupleConcat((double)yyt[j]);
                }
                Pix_X = bestrow;
                Pix_Y = bestcol;
            }

             HOperatorSet.VectorToHomMat2d(Pix_X, Pix_Y, AxisX, AxisY, out HomMat2Dxy);
            //HOperatorSet.VectorToHomMat2d(Pix_X, Pix_Z, AxisX, AxisZ, out HomMat2Dz);
            if (rotatex.Length>0)
            {
                HOperatorSet.VectorToHomMat3d("affine", Pix_X, Pix_Y, Pix_Z, rotatex, rotatey, AxisZ, out HomMat2Dz);
            }
           
        }



        HTuple AxisX = new HTuple();
        HTuple AxisY = new HTuple();
        HTuple AxisZ = new HTuple();

        HTuple Pix_X = new HTuple();
        HTuple Pix_Y = new HTuple();
        HTuple Pix_Z = new HTuple();
       

        private void LoadToUI()
        {
            try
            {
                
                foreach (Control Pa in panel2.Controls)
                {

                    if (Pa is Panel)
                    {
                        foreach (Control item in Pa.Controls)
                        {
                            if (item.Name.Contains("Axis_x"))
                            {
                                int Tag = Convert.ToInt32(item.Name.Substring(6, item.Name.Length - 6));
                                NumTempx[Tag - 1] = (NumericUpDown)item;
                            }
                            if (item.Name.Contains("Axis_y"))
                            {
                                int Tag = Convert.ToInt32(item.Name.Substring(6, item.Name.Length - 6));
                                NumTempy[Tag - 1] = (NumericUpDown)item;
                            }
                            if (item.Name.Contains("Axis_z"))
                            {
                                int Tag = Convert.ToInt32(item.Name.Substring(6, item.Name.Length - 6));
                                NumTempz[Tag - 1] = (NumericUpDown)item;
                            }
                            if (item.Name.Contains("Pix_x"))
                            {
                                int Tag = Convert.ToInt32(item.Name.Substring(5, item.Name.Length - 5));
                                NumPIX_x[Tag - 1] = (NumericUpDown)item;
                            }
                            if (item.Name.Contains("Pix_y"))
                            {
                                int Tag = Convert.ToInt32(item.Name.Substring(5, item.Name.Length - 5));
                                NumPIX_y[Tag - 1] = (NumericUpDown)item;
                            }
                            if (item.Name.Contains("Pix_z"))
                            {
                                int Tag = Convert.ToInt32(item.Name.Substring(5, item.Name.Length - 5));
                                NumPIX_z[Tag - 1] = (NumericUpDown)item;
                            }
                        }

                        int Tag1 = Convert.ToInt32(Pa.Name.Substring(6, Pa.Name.Length - 6));
                        PanelTemp[Tag1 - 1] = (Panel)Pa;
                        PanelTemp[Tag1 - 1].Visible = false;

                    }

                   
                }

                ParamPath.ParaName = comboBox5.SelectedItem.ToString();
                if (!Directory.Exists(ParamPath.ParamDir))
                {
                    Directory.CreateDirectory(ParamPath.ParamDir);
                }
               

                //string path = ParamPath.ParamDir + "template.tiff";
                //if (File.Exists(path))
                //{
                //    HOperatorSet.ReadImage(out Image, path);
                //    hWindow_Final1.HobjectToHimage(Image);

                //}

                if (File.Exists(ParamPath.Path_tup))
                {
                    HOperatorSet.ReadTuple(ParamPath.Path_tup, out HomMat2Dxy);
                }

                if (File.Exists(ParamPath.Path_txt))
                {
                    if (File.Exists(ParamPath.Path_Param))
                    {                        
                        calibParam = (RoiParam)XmlSeriazlize.LoadFormXml(ParamPath.Path_Param, typeof(RoiParam));
                    }
                    AxisX = new HTuple();
                    AxisY = new HTuple();
                    AxisZ = new HTuple();

                    Pix_X = new HTuple();
                    Pix_Y = new HTuple();
                    Pix_Z = new HTuple();

                    foreach (var item in File.ReadLines(ParamPath.Path_txt))
                    {
                        if (item== "")
                        {
                            continue;
                        }
                        string[] txtArray = item.Split('\t');
                        
                        double axisX = Convert.ToDouble(txtArray[0]);
                        double axisY = Convert.ToDouble(txtArray[1]);
                        double axisZ = Convert.ToDouble(txtArray[2]);

                        double pixX = Convert.ToDouble(txtArray[3]);
                        double pixY = Convert.ToDouble(txtArray[4]);
                        double pixZ = Convert.ToDouble(txtArray[5]);

                        AxisX = AxisX.TupleConcat(axisX);
                        AxisY = AxisY.TupleConcat(axisY);
                        AxisZ = AxisZ.TupleConcat(axisZ);

                        Pix_X = Pix_X.TupleConcat(pixX);
                        Pix_Y = Pix_Y.TupleConcat(pixY);
                        Pix_Z = Pix_Z.TupleConcat(pixZ);

                    }
                    listBox1.Items.Clear();
                    listBox2.Items.Clear();
                    //comboBox1.Items.Clear();
                    //comboBox2.Items.Clear();
                    //comboBox3.Items.Clear();
                    for (int i = 0; i < calibParam.PointCount; i++)
                    {
                       if( calibParam.intersectCoord[i].LorC =="L")
                        {
                            listBox1.Items.Add(((i + 1).ToString() + "_Point"));
                        }
                        else
                        {
                            listBox2.Items.Add(((i + 1).ToString() + "_Point"));
                        }
                        PointStr[i] = ((i+1).ToString() + "_Point");
                        PanelTemp[i].Visible = true;
                    }
                   
                    for (int i = 0; i < AxisX.Length; i++)
                    {
                        NumTempx[i].Value = (decimal)AxisX[i].D;
                        NumTempy[i].Value = (decimal)AxisY[i].D;
                        NumTempz[i].Value = (decimal)AxisZ[i].D;

                        NumPIX_x[i].Value = (decimal)Pix_X[i].D;
                        NumPIX_y[i].Value = (decimal)Pix_Y[i].D;
                        NumPIX_z[i].Value = (decimal)Pix_Z[i].D;

                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox3.SelectedIndex == -1 || hWindow_Final1.Image == null || calibParam.PointCount >=16)
                {
                    return;
                }
                string selet = comboBox3.SelectedItem.ToString();
                int cirId = Convert.ToInt32(selet.Substring(0, selet.Length - 4)) - 1;
                List<HTuple> hv_Row_all = new List<HTuple>();
                List<HTuple> hv_Column_all = new List<HTuple>();
                List<HTuple> Coord = new List<HTuple>();
                Output.GetLcPoint(OrigImg, roiparamList, out hv_Row_all, out hv_Column_all, out Coord, hWindow_Final1);
                if (Coord[cirId].Length != 3)
                {
                    MessageBox.Show("圆查找失败！");
                    return;
                }
                HObject cross = new HObject();
                HOperatorSet.GenCrossContourXld(out cross, Coord[cirId][0], Coord[cirId][1], 10, 0);
                HOperatorSet.SetColor(hWindow_Final1.HWindowHalconID, "green");
                HOperatorSet.DispObj(cross, hWindow_Final1.HWindowHalconID);

                Button bt = (Button)sender;
                tb_X.Text = Math.Round(Coord[cirId][0].D, 3).ToString();
                tb_Y.Text = Math.Round(Coord[cirId][1].D, 3).ToString();
                if (bt.Text.Contains("添加"))
                {
                    IntersetionCoord coord = new IntersetionCoord();
                    coord.Row = Math.Round(Coord[cirId][0].D, 3);
                    coord.Col = Math.Round(Coord[cirId][1].D, 3);
                    coord.LorC = "C";
                    //calibParam.intersectCoord.Add(coord);
                    calibParam.PointCount += 1;
                    for (int i = 1; i <= calibParam.PointCount; i++)
                    {
                        if (!PointStr.Contains(i.ToString() + "_Point"))
                        {
                            listBox2.Items.Add(i.ToString() + "_Point");
                            //排序
                            List<int> id = new List<int>();
                            for (int j = 0; j < listBox2.Items.Count; j++)
                            {
                                string item = listBox2.Items[j].ToString();
                                int itemId = Convert.ToInt32(item.Remove(item.Length - 6, 6));
                                id.Add(itemId);
                            }
                            listBox2.Items.Clear();
                            id.Sort();
                            for (int k = 0; k < id.Count; k++)
                            {
                                listBox2.Items.Add(id[k].ToString() + "_Point");
                            }

                            PointStr[i -1] = (i.ToString() + "_Point");

                            if (calibParam.PointCount <= NumPIX_x.Length)
                            {
                                PanelTemp[i - 1].Visible = true;
                                NumPIX_x[i - 1].Value = (decimal)coord.Row;
                                NumPIX_y[i - 1].Value = (decimal)coord.Col;
                                calibParam.intersectCoord[i - 1] = (coord);
                            }

                            break;
                        }
                    }
                   
                   
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem==null)
            {
                return;
            }
            string select = listBox1.SelectedItem.ToString();
            int selectID = Convert.ToInt32(select.Remove(select.Length - 6, 6)) - 1;
            calibParam.intersectCoord[selectID] = new IntersetionCoord();
            calibParam.PointCount -= 1;
            listBox1.Items.Remove(select);
            PointStr[selectID] = ("");
        }

        private void 删除ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
            {
                return;
            }
            string select = listBox2.SelectedItem.ToString();
            int selectID = Convert.ToInt32(select.Remove(select.Length - 6, 6)) - 1;
            calibParam.intersectCoord[selectID] = new IntersetionCoord();
            calibParam.PointCount -= 1;
            listBox2.Items.Remove(select);
            PointStr[selectID] = ("");
        }

        private void 删除所有ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < calibParam.PointCount; i++)
            {
                calibParam.intersectCoord[i] = new IntersetionCoord();
                PointStr[i] = ("");
               
            }
            calibParam.PointCount = 0;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
        }

        private void 删除所有ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < calibParam.intersectCoord.Count; i++)
            {
                calibParam.intersectCoord[i] = new IntersetionCoord();
                PointStr[i] = ("");
            }
            calibParam.PointCount = 0;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
        }


        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (IsLoading)
                {
                    return;
                }
                LoadToUI();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                
                
                ParamPath.ParaName = comboBox5.SelectedItem.ToString();
                if (MessageBox.Show("是否标定"+ comboBox5.SelectedItem.ToString(),"Warning",MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                if (calibParam.PointCount < 3)
                {
                    MessageBox.Show(string.Format("当前标定像素点数{0}太少", calibParam.PointCount));
                    return;
                }
                StringBuilder sb = new StringBuilder();
                AxisX = new HTuple();
                AxisY = new HTuple();
                AxisZ = new HTuple();

                Pix_X = new HTuple();
                Pix_Y = new HTuple();
                Pix_Z = new HTuple();
                GetBest();
                int count = calibParam.PointCount;
                if (checkBox1.Checked)
                {
                    if (calibParam.PointCount <= 6)
                    {
                        count = calibParam.PointCount;

                    }
                    else
                    {
                        count = 6;
                    }
                }
                for (int i = 0; i < count; i++)
                {
                    double x = (double)NumTempx[i].Value;
                    double y = (double)NumTempy[i].Value;
                    double z = (double)NumTempz[i].Value;

                    //double PIX_x = (double)NumPIX_x[i].Value;
                    //double PIX_y = (double)NumPIX_y[i].Value;

                    double PIX_x = (double)Pix_X[i].D;
                    double PIX_y = (double)Pix_Y[i].D;
                    double PIX_z = (double)Pix_Z[i].D;

                    NumPIX_x[i].Value = (decimal)PIX_x;
                    NumPIX_y[i].Value = (decimal)PIX_y;
                    NumPIX_z[i].Value = (decimal)PIX_z;

                    //AxisX = AxisX.TupleConcat(x);
                    //AxisY = AxisY.TupleConcat(y);
                    //Pix_X = Pix_X.TupleConcat(PIX_x);
                    //Pix_Y = Pix_Y.TupleConcat(PIX_y);
                    string data = x.ToString() + "\t" + y.ToString() + "\t"+ z.ToString() + "\t" + PIX_x.ToString() + "\t" + PIX_y.ToString() + "\t" + PIX_z.ToString() + "\r\n";
                    sb.Append(data);
                }
                //根据九个点计算反射变换矩阵  

                //HOperatorSet.VectorToHomMat2d(Pix_X, Pix_Y, AxisX, AxisY, out HomMat2D);
                HOperatorSet.WriteTuple(HomMat2Dxy, ParamPath.Path_tup);
                if (File.Exists(ParamPath.Path_txt))
                {
                    File.Delete(ParamPath.Path_txt);
                }
                ParamPath.WriteTxt(ParamPath.Path_txt, sb.ToString());

                //dispenser.txtHelper.WriteSerializer(Cp, ParamPath.Path_Param);

                MessageBox.Show("标定成功！");
                button7_Click(sender, e);
            }
            catch (Exception ex)
            {

                MessageBox.Show("标定失败！" + ex.Message);
            }
            

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                double virx = (double)vir_x.Value;
                double viry = (double)vir_y.Value;
                double virz = (double)vir_z.Value;

                if (HomMat2Dxy.Type != HTupleType.EMPTY)
                {
                    HTuple realx = 0, realy = 0, realz = 0;
                    HOperatorSet.AffineTransPoint2d(HomMat2Dxy, virx, viry, out realx, out realy);
                    HTuple tx = 0;
                    //HOperatorSet.AffineTransPoint2d(HomMat2Dz, 0, virz, out tx, out realz);

                    //HOperatorSet.AffineTransPoint3d(HomMat3D, virx, viry, virz, out realx, out realy,out realz);
                    HTuple real_x1, real_y1;
                    if (HomMat2Dz.Length>0)
                    {
                        HOperatorSet.AffineTransPoint3d(HomMat2Dz, virx, viry, virz, out real_x1, out real_y1, out realz);
                    }
                    Realx.Value = (decimal)realx.D;
                    Realy.Value = (decimal)realy.D;
                    Realz.Value = (decimal)realz.D;

                }

                HTuple real_x = new HTuple();
                HTuple real_y = new HTuple();
                HTuple real_z = new HTuple();
                HTuple tx1 = 0;

                HOperatorSet.AffineTransPoint2d(HomMat2Dxy, Pix_X, Pix_Y, out real_x, out real_y);

                //HOperatorSet.AffineTransPoint2d(HomMat2Dz, Pix_X, Pix_Z, out tx1, out real_z);

                //HOperatorSet.AffineTransPoint3d(HomMat2Dz, Pix_X, Pix_Y, Pix_Z, out real_x, out real_y, out real_z);
                HTuple subx = AxisX - real_x;
                HTuple suby = AxisY - real_y;
                HTuple subz = AxisZ - real_z;

                subx = subx.TupleAbs();
                suby = suby.TupleAbs();
                subz = subz.TupleAbs();

                double max_x = subx.TupleMax();
                double max_y = suby.TupleMax();

                double min_x = subx.TupleMin();
                double min_y = suby.TupleMin();

                double Meanx = subx.TupleMean();
                double Meany = suby.TupleMean();

                max_x = Math.Round(max_x, 3);
                max_y = Math.Round(max_y, 3);
                min_x = Math.Round(min_x, 3);
                min_y = Math.Round(min_y, 3);
                Meanx = Math.Round(Meanx, 3);
                Meany = Math.Round(Meany, 3);

                MessageBox.Show(string.Format("最大误差x：{0};y: {1};\r\n最小误差x：{2};y: {3};\r\n平均误差x：{4};y: {5};", max_x.ToString(), max_y.ToString(), min_x.ToString(), min_y.ToString(), Meanx.ToString(), Meany.ToString()));

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                for (int i = 0; i < 16; i++)
                {
                    NumPIX_x[i].Visible = true;
                    NumPIX_y[i].Visible = true;
                    NumTempx[i].Visible = true;
                    NumTempy[i].Visible = true;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hWindow_Final1.Image!=null && hWindow_Final1.Image.IsInitialized() && listBox1.SelectedItem != null)
            {
                string select = listBox1.SelectedItem.ToString();
                int selectID = Convert.ToInt32(select.Remove(select.Length - 6, 6)) - 1;
                HObject cross = new HObject();
                HOperatorSet.GenCrossContourXld(out cross, calibParam.intersectCoord[selectID].Row, calibParam.intersectCoord[selectID].Col, 60, 0);
                HOperatorSet.SetColor(hWindow_Final1.HWindowHalconID, "green");
                HOperatorSet.DispObj( cross,hWindow_Final1.HWindowHalconID);
                tb_ix.Text = calibParam.intersectCoord[selectID].Row.ToString();
                tb_iy.Text = calibParam.intersectCoord[selectID].Col.ToString();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hWindow_Final1.Image != null && hWindow_Final1.Image.IsInitialized() && listBox2.SelectedItem != null)
            {
                string select = listBox2.SelectedItem.ToString();
                int selectID = Convert.ToInt32(select.Remove(select.Length - 6, 6)) - 1;
                HObject cross = new HObject();
                HOperatorSet.GenCrossContourXld(out cross, calibParam.intersectCoord[selectID].Row, calibParam.intersectCoord[selectID].Col, 60, 0);
                HOperatorSet.SetColor(hWindow_Final1.HWindowHalconID, "green");
                HOperatorSet.DispObj(cross, hWindow_Final1.HWindowHalconID);
                tb_X.Text = calibParam.intersectCoord[selectID].Row.ToString();
                tb_Y.Text = calibParam.intersectCoord[selectID].Col.ToString();
            }
        }

        public void Close()
        {
            //HOperatorSet.ClearMetrologyObject(Global.metrologyHandle, "all");
        }


    }

    public class ParamPath
    {
        public static string ParaName = "";
        public static string ParamDir
        {
            //get { return AppDomain.CurrentDomain.BaseDirectory + "Calib" + "\\" + ParaName + "\\"; }
            get { return Application.StartupPath + "\\Calib" + "\\" + ParaName + "\\"; }
        }
        public static string Path_Param
        {
            get { return ParamDir + "Calib.xml"; }
        }
        public static string Path_txt
        {
            get { return ParamDir + "Calibrate" + ".txt"; }
        }
        public static string Path_txtBackUp
        {
            get { return ParamDir + "CalibrateBackUp" + ".txt"; }
        }
        public static string Path_tup
        {
            get { return ParamDir + "Calibrate" + ".tup"; }
        }

        public static void WriteTxt(string fileName,string value)
        {
            try
            {
                //string fileName = AppDomain.CurrentDomain.BaseDirectory + "data.txt";
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.WriteLine("{0}\n", value);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            catch (Exception)
            {
               
                throw;
            }
        }

    }

    
    

    

}
