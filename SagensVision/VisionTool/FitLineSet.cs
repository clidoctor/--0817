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
using HalconDotNet;
using ChoiceTech.Halcon.Control;
using SagensSdk;
using System.IO;
using ViewWindow.Model;
using System.Collections;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace SagensVision.VisionTool
{
    public partial class FitLineSet : DevExpress.XtraEditors.XtraForm
    {
        int CurrentIndex = 0;
        HWindow_Final hwindow_final1 = new HWindow_Final();
        HWindow_Final hwindow_final2 = new HWindow_Final();
        //public FitProfileParam[] fpTool.fParam = new FitProfileParam[4];
        ROIController roiController;
        ROIController roiController2;

        //List<ROI>[] fpTool.roiList = new List<ROI>[4];
        //List<ROI>[] fpTool.fpTool.roiList2 = new List<ROI>[4];
        List<ROI> tempList = new List<ROI>();
        //List<ROI>[] fpTool.roiList3 = new List<ROI>[4];//校准区域
        HObject HeightImage = new HObject();
        HObject IntensityImage = new HObject();
        HObject RGBImage = new HObject();
        HObject OriginImage = new HObject();
        //public IntersetionCoord[] intersectCoordList = new IntersetionCoord[4];
        //计算输出交点
        IntersetionCoord intersection = new IntersetionCoord();
        FindPointTool fpTool = new FindPointTool();
        bool isRight = true;
        public FitLineSet(string text = "")
        {
            InitializeComponent();
            if (text != "")
            {
                this.Text = text;
            }
            
        }

        private void FitLineSet_Load(object sender, EventArgs e)
        {
            isRight = MyGlobal.IsRight;
            string ok = fpTool.Init(this.Text,isRight);
            if (ok != "OK")
            {
                MessageBox.Show(ok);
            }
            this.MaximizeBox = true;
            CurrentSide = "";
            isSave = true;
            isCloing = false;
            splitContainerControl4.Panel1.Controls.Add(hwindow_final2);
            splitContainerControl6.Panel1.Controls.Add(hwindow_final1);
            hwindow_final1.Dock = DockStyle.Fill;
            hwindow_final2.Dock = DockStyle.Fill;

            trackBarControl1.Properties.Minimum = 1;

            LoadToUI(0);
            roiController = hwindow_final1.viewWindow._roiController;
            roiController.NotifyRCObserver = new IconicDelegate(ROiMove);
            roiController2 = hwindow_final2.viewWindow._roiController;
            roiController2.NotifyRCObserver = new IconicDelegate(ROiMove2);
            comboBox2.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            
            CurrentSide = "";
            hwindow_final1.viewWindow.setEditModel(true);
            hwindow_final2.viewWindow.setEditModel(true);
            checkBoxRoi.Checked = true;
            ChangeSide();
        }

        public void ROiMove(int value)
        {
            try
            {
                switch (value)
                {
                    //case ROIController.EVENT_CHANGED_ROI_SIGN:
                    //case ROIController.EVENT_DELETED_ACTROI:
                    //case ROIController.EVENT_DELETED_ALL_ROIS:
                    case ROIController.EVENT_UPDATE_ROI:

                        int Id = Convert.ToInt32(SideName.Substring(4, 1));
                        if (dataGridView1.CurrentCell == null)
                        {
                            return;
                        }
                        int RowId = dataGridView1.CurrentCell.RowIndex;
                        RowId = CurrentRowIndex;

                        if (RowId < 0 || FindPointTool.RArray == null)
                        {
                            return;
                        }
                        fpTool.roiList[Id - 1][RowId] = temp[0];


                        //记录当前锚定点坐标

                        HTuple row, col;
                        fpTool.FindFirstAnchor(Id, out row, out col,CurrentIndex);
                        fpTool.fParam[Id - 1].roiP[RowId].AnchorRow = row.D;
                        fpTool.fParam[Id - 1].roiP[RowId].AnchorCol = col.D;

                        ROI pt = roiController.getActiveROI();
                        if (pt.Type == "ROIPoint")
                        {
                            HTuple PointTemp = pt.getModelData(); HTuple row1, col1;
                            fpTool.FindMaxPt(Id, CurrentIndex - 1, out row1, out col1, out row, out col, null, ShowSection, false,null);
                            switch (PtOrder)
                            {
                                case 0:

                                    fpTool.fParam[Id - 1].roiP[RowId].StartOffSet1.Y = (int)(PointTemp[0].D - row.D);
                                    fpTool.fParam[Id - 1].roiP[RowId].StartOffSet1.X = (int)(PointTemp[1].D - col.D);
                                    PtOrder = -1;
                                    break;
                                case 1:

                                    fpTool.fParam[Id - 1].roiP[RowId].EndOffSet1.Y = (int)(PointTemp[0].D - row.D);
                                    fpTool.fParam[Id - 1].roiP[RowId].EndOffSet1.X = (int)(PointTemp[1].D - col.D);
                                    PtOrder = -1;
                                    break;
                                case 2:

                                    fpTool.fParam[Id - 1].roiP[RowId].StartOffSet2.Y = (int)(PointTemp[0].D - row.D);
                                    fpTool.fParam[Id - 1].roiP[RowId].StartOffSet2.X = (int)(PointTemp[1].D - col.D);
                                    PtOrder = -1;
                                    break;
                                case 3:

                                    fpTool.fParam[Id - 1].roiP[RowId].EndOffSet2.Y = (int)(PointTemp[0].D - row.D);
                                    fpTool.fParam[Id - 1].roiP[RowId].EndOffSet2.X = (int)(PointTemp[1].D - col.D);
                                    PtOrder = -1;
                                    break;
                            }

                        }

                        break;
                    //case HWndCtrl.ERR_READING_IMG:
                    //    MessageBox.Show("Problem occured while reading file! \n", "Profile ",
                    //        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("RoiMove-->" + ex.Message);
            }
        }
        bool RoiIsMoving = false;
        public void ROiMove2(int value)
        {
            try
            {
                switch (value)
                {
                    //case ROIController.EVENT_CHANGED_ROI_SIGN:
                    //case ROIController.EVENT_DELETED_ACTROI:
                    //case ROIController.EVENT_DELETED_ALL_ROIS:
                    case ROIController.EVENT_UPDATE_ROI:
                        RoiIsMoving = true;
                        int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
                        ArrayList array = roiController2.ROIList;
                        int currentId = -1; string Name = "";
                        if (dataGridView1.CurrentCell == null || CurrentRowIndex == -1)
                        {
                            currentId = 0;
                        }
                        else
                        {
                            currentId = dataGridView1.CurrentCell.RowIndex;
                            currentId = CurrentRowIndex;
                            //ID
                            Name = dataGridView1.Rows[currentId].Cells[1].Value.ToString();
                        }

                        if (fpTool.roiList2[Id].Count != array.Count)
                        {

                            fpTool.roiList2[Id].Add(new ROIRectangle2());

                            //每个区域单独设置Roi
                            hwindow_final1.viewWindow.genRect1(400, 400, 600, 600, ref fpTool.roiList[Id]);
                        }



                        int ActiveId = roiController2.getActiveROIIdx();
                        if (array.Count == 1)
                        {
                            ROI te = (ROI)array[0];
                            if (te.Type != "ROIRectangle2")
                            {
                                return;
                            }
                        }
                        if (fpTool.roiList2[Id].Count == 0)
                        {
                            return;
                        }

                        //if ( isInsert)
                        //{
                        //    currentId = currentId;
                        //}
                        //if (!isInsert)
                        //{
                        //    currentId = ActiveId;
                        //}

                        //if (currentId != -1 && isInsert)
                        //{
                        //    isInsert = false;

                        //    fpTool.fpTool.roiList2[Id][currentId] = (ROI)array[ActiveId];
                        //    RoiParam RP = new RoiParam();  

                        //    RP = fpTool.fParam[Id].roiP[currentId].Clone();

                        //    fpTool.fParam[Id].roiP.Insert(currentId, RP);
                        //    fpTool.fParam[Id].roiP[currentId].LineOrCircle = comboBox2.SelectedItem.ToString();
                        //    //insert
                        //    dataGridView1.Rows.Insert(currentId);
                        //    dataGridView1.Rows[currentId].Cells[0].Value = currentId;
                        //    dataGridView1.Rows[currentId].Cells[1].Value = "Default";
                        //    dataGridView1.Rows[currentId].Cells[2].Value = fpTool.fParam[Id].roiP[currentId].LineOrCircle;
                        //    fpTool.fParam[Id].DicPointName.Insert(currentId, "Default");
                        //    //排序
                        //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        //    {
                        //        dataGridView1.Rows[i].Cells[0].Value = i;
                        //    }

                        //}
                        //else
                        //{
                        //if (isInsert)
                        //{
                        //    isInsert = false;
                        //    //fpTool.fpTool.roiList2[Id][currentId] = (ROI)array[ActiveId];
                        //}
                        //else
                        //{
                        //    fpTool.fpTool.roiList2[Id][ActiveId] = (ROI)array[ActiveId];
                        //}
                        fpTool.roiList2[Id][ActiveId] = (ROI)array[ActiveId];

                        //}

                        string key = "L";
                        if (fpTool.fParam[Id].roiP.Count != array.Count)
                        {
                            RoiParam RP = new RoiParam();

                            RP.AngleOfProfile = Convert.ToInt32(textBox_Deg.Text);
                            RP.NumOfSection = Convert.ToInt32(textBox_Num.Text);

                            fpTool.fParam[Id].roiP.Add(RP);
                            fpTool.fParam[Id].roiP[ActiveId].LineOrCircle = comboBox2.SelectedItem.ToString();

                            switch (fpTool.fParam[Id].roiP[ActiveId].LineOrCircle)
                            {
                                case "连接段":
                                    key = "LC";
                                    break;
                                case "直线段":
                                    key = "L";
                                    break;
                                case "圆弧段":
                                    key = "C";
                                    break;
                            }
                            key = key + (ActiveId + 1).ToString();
                            if (isGenSection)
                            {
                                isGenSection = false;
                                AddToDataGrid(key, fpTool.fParam[Id].roiP[ActiveId].LineOrCircle);
                                fpTool.fParam[Id].DicPointName.Add(key);
                            }

                            //操作DicPointName    

                        }
                        HTuple[] lineCoord = new HTuple[1];

                        ////全选
                        //if (SelectAll)
                        //{
                        //    for (int i = 0; i < fpTool.fParam[Id].roiP.Count; i++)
                        //    {
                        //        DispSection((ROIRectangle2)fpTool.fpTool.roiList2[Id][i], Id, i, out lineCoord, hwindow_final2);
                        //    }
                        //}
                        //else
                        //{
                        //    if (currentId != -1 && isInsert)
                        //    {
                        //        isInsert = false;
                        //        DispSection((ROIRectangle2)fpTool.fpTool.roiList2[Id][currentId], Id, currentId, out lineCoord, hwindow_final2);

                        //    }
                        //    else
                        //    {
                        //        DispSection((ROIRectangle2)fpTool.fpTool.roiList2[Id][ActiveId], Id, ActiveId, out lineCoord, hwindow_final2);

                        //    }
                        //}

                        //listBox1.SelectedItem = ActiveId;
                        HTuple ModelData = fpTool.roiList2[Id][ActiveId].getModelData();
                        fpTool.fParam[Id].roiP[ActiveId].Len1 = ModelData[3];
                        fpTool.fParam[Id].roiP[ActiveId].Len2 = ModelData[4];
                        fpTool.fParam[Id].roiP[ActiveId].CenterRow = ModelData[0];
                        fpTool.fParam[Id].roiP[ActiveId].CenterCol = ModelData[1];
                        fpTool.fParam[Id].roiP[ActiveId].phi = ModelData[2];

                        textBox_Len.Text = ((int)fpTool.fParam[Id].roiP[ActiveId].Len1).ToString();
                        textBox_Width.Text = ((int)fpTool.fParam[Id].roiP[ActiveId].Len2).ToString();
                        textBox_Row.Text = ((int)fpTool.fParam[Id].roiP[ActiveId].CenterRow).ToString();
                        textBox_Col.Text = ((int)fpTool.fParam[Id].roiP[ActiveId].CenterCol).ToString();
                        HTuple deg = new HTuple();
                        HOperatorSet.TupleDeg(ModelData[2], out deg);
                        textBox_phi.Text = ((int)deg.D).ToString();

                        //listBox1.SelectedItem = ActiveId;
                        //richTextBox1.SelectedText = key;
                        //int fId = richTextBox1.GetFirstCharIndexFromLine(currentId);
                        //string[] lines1 = richTextBox1.Lines;

                        //richTextBox1.Focus();
                        //richTextBox1.Select(fId, lines1[ActiveId].Length);
                        PreSelect = Name;


                        //刷新界面roi

                        //

                        if (fpTool.roiList2[Id].Count > 0)
                        {
                            if (SelectAll)
                            {
                                hwindow_final2.viewWindow.notDisplayRoi();
                                roiController2.viewController.ShowAllRoiModel = -1;
                                hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[Id]);
                            }
                            else
                            {
                                roiController2.viewController.ShowAllRoiModel = ActiveId;
                                roiController2.viewController.repaint(ActiveId);
                            }
                        }
                        hwindow_final2.viewWindow.selectROI(ActiveId);

                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[ActiveId].Selected = true;
                        CurrentRowIndex = ActiveId;
                        //记录当前锚定点坐标
                        RoiIsMoving = false;

                        break;
                    //case HWndCtrl.ERR_READING_IMG:
                    //    MessageBox.Show("Problem occured while reading file! \n", "Profile ",
                    //        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                isInsert = false;
                MessageBox.Show(ex.Message);
            }
        }


        bool isLoading = false;
        void LoadToUI(int Index = 0)
        {
            try
            {
                RoiIsMoving = true;
                isLoading = true;
                //刷新Roi参数
                UpdateRoiParam();
                ParamPath.ParaName = comboBox1.SelectedItem.ToString();
                ParamPath.IsRight = isRight;
                if (isRight)
                {
                    comboBox3.SelectedIndex = 0;
                    comboBox3.BackColor = Color.LimeGreen;
                }
                else
                {
                    comboBox3.SelectedIndex = 1;
                    comboBox3.BackColor = Color.Yellow;
                }

                string[] keys = fpTool.fParam[Index].DicPointName.ToArray();

                dataGridView1.Rows.Clear();
                for (int i = 0; i < keys.Length; i++)
                {
                    AddToDataGrid(keys[i], fpTool.fParam[Index].roiP[i].LineOrCircle);
                }

                if (keys.Length > 0)
                {
                    textBox_Num.Text = ((int)fpTool.fParam[Index].roiP[0].NumOfSection).ToString();
                    textBox_Len.Text = ((int)fpTool.fParam[Index].roiP[0].Len1).ToString();
                    textBox_Width.Text = ((int)fpTool.fParam[Index].roiP[0].Len2).ToString();
                    textBox_Row.Text = ((int)fpTool.fParam[Index].roiP[0].CenterRow).ToString();
                    textBox_Col.Text = ((int)fpTool.fParam[Index].roiP[0].CenterCol).ToString();
                    textBox_Offset.Text = ((int)fpTool.fParam[Index].roiP[0].offset).ToString();
                    textBox_OffsetX.Text = ((int)fpTool.fParam[Index].roiP[0].Xoffset).ToString();
                    textBox_OffsetY.Text = ((int)fpTool.fParam[Index].roiP[0].Yoffset).ToString();
                    textBox_OffsetZ.Text = ((int)fpTool.fParam[Index].roiP[0].Zoffset).ToString();
                    textBox_ZFtMax.Text = (fpTool.fParam[Index].roiP[0].ZftMax).ToString();
                    textBox_ZFtMin.Text = (fpTool.fParam[Index].roiP[0].ZftMin).ToString();
                    textBox_ZFtRad.Text = (fpTool.fParam[Index].roiP[0].ZftRad).ToString();

                    HTuple deg = new HTuple();
                    HOperatorSet.TupleDeg(fpTool.fParam[Index].roiP[0].phi, out deg);
                    textBox_phi.Text = ((int)deg.D).ToString();
                    textBox_Deg.Text = fpTool.fParam[Index].roiP[0].AngleOfProfile.ToString();
                    checkBox_useLeft.Checked = fpTool.fParam[Index].roiP[0].useLeft;
                    checkBox_midPt.Checked = fpTool.fParam[Index].roiP[0].useMidPt;
                    checkBox_Far.Checked = !fpTool.fParam[Index].roiP[0].useNear;
                    checkBox_center.Checked = fpTool.fParam[Index].roiP[0].useCenter;

                    textBox_downDist.Text = fpTool.fParam[Index].roiP[0].TopDownDist.ToString();
                    textBox_xDist.Text = fpTool.fParam[Index].roiP[0].xDist.ToString();
                    textBox_Clipping.Text = fpTool.fParam[Index].roiP[0].ClippingPer.ToString();
                    textBox_SmoothCont.Text = fpTool.fParam[Index].roiP[0].SmoothCont.ToString();
                    comboBox_GetPtType.SelectedIndex = 0;
                }

                RoiIsMoving = false;
                isLoading = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        void AddToDataGrid(string ID, string Type)
        {

            dataGridView1.Rows.Add();
            int count = dataGridView1.Rows.Count;
            dataGridView1.Rows[count - 1].Cells[0].Value = count;
            dataGridView1.Rows[count - 1].Cells[1].Value = ID;
            dataGridView1.Rows[count - 1].Cells[2].Value = Type;
        }

        private void trackBarControl1_ValueChanged(object sender, EventArgs e)
        {
            CurrentIndex = trackBarControl1.Value;
            textBox_Current.Text = CurrentIndex.ToString();
            if (FindPointTool.RArray != null && FindPointTool.RArray.GetLength(0) > 0)
            {

                //button11_Click(sender, e);
                int Id = Convert.ToInt32(SideName.Substring(4, 1));
                HTuple row, col; HTuple anchor, anchorc;
                int roiID = -1;
                for (int i = 0; i < fpTool.fParam[Id - 1].roiP.Count; i++)
                {

                    for (int j = 0; j < fpTool.fParam[Id - 1].roiP[i].NumOfSection; j++)
                    {
                        roiID++;
                        if (roiID == CurrentIndex - 1)
                        {
                            break;
                        }
                    }
                    if (roiID == CurrentIndex - 1)
                    {
                        roiID = i;
                        break;
                    }

                }
                if (fpTool.fParam[Id - 1].roiP[roiID].SelectedType == 0)
                {
                   
                    if (fpTool.fParam[Id - 1].roiP[roiID].TopDownDist != 0 && fpTool.fParam[Id - 1].roiP[roiID].xDist != 0)
                    {
                        //极值点下降
                        fpTool.FindMaxPtFallDown(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection,false,hwindow_final2);
                    }
                    else
                    {
                        fpTool.FindMaxPt(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final2, ShowSection,false,hwindow_final1);
                    }
                }
                else
                {
                    
                    fpTool.FindMaxPtFallDown(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection,false,hwindow_final2);
                }
            }
        }


        private void textBox_Current_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            string index = textBox_Current.Text.ToString();
            bool ok = Regex.IsMatch(index, @"(?i)^[0-9]+$");
            if (CurrentIndex == 0 || index == "0")
            {
                return;
            }
            if (ok)
            {
                int num = int.Parse(index);
                CurrentIndex = num;
                trackBarControl1.Value = CurrentIndex;

            }
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            MyGlobal.GoSDK.EnableProfle = true;
            string error = "";
            if (!MyGlobal.GoSDK.IsConnected(ref error))
            {
                MessageBox.Show("请先连接Sensor!");
                return;
            }
            bool ok = MyGlobal.GoSDK.Start(ref error);
            if (ok != true)
            {
                MessageBox.Show(error);
            }
            else
            {
                MessageBox.Show("打开成功！");
            }
        }
  

        ///// <summary>
        /////保存的数据
        ///// </summary>
        //public static double[,] RCAll;

        //public static double[][] RArray;
        //public static double[][] CArray;

        //public static double[][] Row;
        //public static double[][] Phi;

        //public static double StartFov = 0;
        //public static double Resolution;
        //public static double yResolution;

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string error = "";
            bool ok = MyGlobal.GoSDK.Stop(ref error);
            MyGlobal.GoSDK.EnableProfle = false;
            if (ok != true)
            {
                MessageBox.Show(error);
                return;
            }

            try
            {

                List<SagensSdk.Profile> profile = MyGlobal.GoSDK.ProfileList;
                if (profile != null)
                {

                    //MyGlobal.globalConfig.dataContext = MyGlobal.GoSDK.context;

                    long SurfaceWidth, SurfaceHeight;
                    SurfaceWidth = profile[0].points.Length;
                    SurfaceHeight = profile.Count;
                    float[] SurfacePointZ = new float[SurfaceWidth * SurfaceHeight];

                    HObject HeightImage = new HObject(); HObject IntensityImage = new HObject();

                    //fpTool.GenIntesityProfile(profile, out IntensityImage);
                    MyGlobal.GoSDK.ProfileListToArr(profile, SurfacePointZ);
                    MyGlobal.GoSDK.GenHalconImage(SurfacePointZ, SurfaceWidth, SurfaceHeight, out HeightImage);

                    hwindow_final2.HobjectToHimage(IntensityImage);
                    //HOperatorSet.WriteImage(HeightImage, "tiff", 0, MyGlobal.ModelPath + "\\" + SideName + "H.tiff");
                    //HOperatorSet.WriteImage(IntensityImage, "tiff", 0, MyGlobal.ModelPath + "\\" + SideName + "I.tiff");
                    MyGlobal.hWindow_Final[0].HobjectToHimage(IntensityImage);
                }
               
                if (MyGlobal.GoSDK.SurfaceDataZ == null)
                {
                    MessageBox.Show("未收到数据");
                    return;

                }

                //MyGlobal.globalConfig.dataContext = MyGlobal.GoSDK.context;
                //    byte[] SurfacePointZ = MyGlobal.GoSDK.SurfaceDataIntensity;
                //float[] SufaceHz = MyGlobal.GoSDK.SurfaceDataZ;
                //long SurfaceWidth, SurfaceHeight;
                //SurfaceWidth = MyGlobal.GoSDK.surfaceWidth;
                //SurfaceHeight = MyGlobal.GoSDK.surfaceHeight;
                //if (SurfacePointZ != null)
                //{
                //    MyGlobal.GoSDK.GenHalconImage(SurfacePointZ, SurfaceWidth, SurfaceHeight, out IntensityImage);
                //    hwindow_final2.HobjectToHimage(IntensityImage);
                //    HOperatorSet.WriteImage(IntensityImage, "tiff", 0, MyGlobal.DataPath + "ProfileTemp\\" + SideName + "I.tiff");
                //}
                //else
                //{
                //    MessageBox.Show("未收到亮度数据");
                //    return;
                //}
                //if (SufaceHz!=null)
                //{                    
                //    MyGlobal.GoSDK.GenHalconImage(SufaceHz, SurfaceWidth, SurfaceHeight, out HeightImage);
                //    HOperatorSet.WriteImage(HeightImage, "tiff", 0, MyGlobal.DataPath + "ProfileTemp\\" + SideName + "H.tiff");
                //}




                simpleButton3.Enabled = true;
                simpleButton4.Enabled = true;
                MessageBox.Show("OK");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        //List<HTuple> RowCoord = new List<HTuple>();
        //List<HTuple> ColCoord = new List<HTuple>();
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog opf = new FolderBrowserDialog();


                opf.SelectedPath =  "\\";
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    //string file = opf.FileName;
                    //if (!file.Contains(".tiff"))
                    //{
                    //    return;
                    //}
                    SelectedPath = opf.SelectedPath;
                    ChangeSide();

                    simpleButton3.Enabled = true;
                    simpleButton4.Enabled = true;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        string SelectedPath = "";
        bool NotUseFix = false;
        private void ChangeSide()
        {
            CurrentRowIndex = -1;
            fpTool.Init(this.Text,isRight);
            textBox_Current.Text = "0";
            textBox_Total.Text = "0";

            CurrentIndex = 0;

            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;

            string Path1 = "";
            if (MyGlobal.ImageMulti.Count >= Id + 1 && SelectedPath == "")
            {               
                IntensityImage = MyGlobal.ImageMulti[Id][0];
                if (MyGlobal.globalConfig.enableAlign)
                {
                    HeightImage = MyGlobal.ImageMulti[Id][2];
                    OriginImage = MyGlobal.ImageMulti[Id][1];
                }
                else
                {
                    HeightImage = MyGlobal.ImageMulti[Id][1];
                    OriginImage = MyGlobal.ImageMulti[Id][2];
                }
            }
            else
            {
                //读取指定路径下或选择路径下图片
                if (File.Exists(Path1 + "\\" + SideName + "H.tiff"))
                {
                    //HeightImage.Dispose();
                    HOperatorSet.ReadImage(out HeightImage, Path1 + "\\" + SideName + "H.tiff");
                }
                if (File.Exists(Path1 + "\\" + SideName + "I.tiff"))
                {
                    //IntensityImage.Dispose();
                    HOperatorSet.ReadImage(out IntensityImage, Path1 + "\\" + SideName + "I.tiff");
                }
            }

            if (!HeightImage.IsInitialized())
            {
                return;
            }

            //PseudoColor.GrayToPseudoColor(HeightImage, out RGBImage, true, -20, 10);
            //PseudoColor.HeightAreaToPseudoColor(HeightImage, out RGBImage, -20, 10, fpTool.fParam[Id].MinZ, fpTool.fParam[Id].MaxZ);
            //hwindow_final2.HobjectToHimage(RGBImage);
            hwindow_final2.HobjectToHimage(IntensityImage);



            if (!NotUseFix && this.Text != "Fix")
            {
                //IntersetionCoord intersect = new IntersetionCoord();
                string ok = "";
                if (isRight)
                {
                     ok = MyGlobal.Right_findPointTool_Fix.FindIntersectPoint(Id + 1, HeightImage, out intersection, hwindow_final2, true);
                }
                else
                {
                     ok = MyGlobal.Left_findPointTool_Fix.FindIntersectPoint(Id + 1, HeightImage, out intersection, hwindow_final2, true);
                }
                
                if (ok != "OK")
                {
                    MessageBox.Show(ok);
                }
                HTuple homMaxFix = new HTuple();
                double orignalDeg =  fpTool.intersectCoordList[Id].Angle ;
                double currentDeg = intersection.Angle ;
                HOperatorSet.VectorAngleToRigid(fpTool.intersectCoordList[Id].Row, fpTool.intersectCoordList[Id].Col,
                orignalDeg, intersection.Row, intersection.Col, currentDeg, out homMaxFix);

                //转换Roi
                if (fpTool.roiList2[Id].Count > 0 && homMaxFix.Length > 0)
                {
                    for (int i = 0; i < fpTool.roiList2[Id].Count; i++)
                    {
                        List<ROI> temproi = new List<ROI>();
                        HTuple tempR = new HTuple(); HTuple tempC = new HTuple();
                        HTuple orignal = fpTool.roiList2[Id][i].getModelData();
                        HOperatorSet.AffineTransPoint2d(homMaxFix, orignal[0], orignal[1], out tempR, out tempC);
                        roiController2.viewController.ShowAllRoiModel = -1;
                        hwindow_final2.viewWindow.genRect2(tempR, tempC, orignal[2], orignal[3], orignal[4], ref temproi);
                        fpTool.roiList2[Id][i] = temproi[0];
                    }
                }
            }
            else
            {
                if (fpTool.roiList2[Id].Count > 0)
                {
                    fpTool.roiList2[Id].Clear();
                    fpTool.Init(this.Text,isRight);
                    hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[Id]);
                }
            }

            RoiIsMoving = true;
            LoadToUI(Id);
            RoiIsMoving = false;
            //textBox_SingleOffset.Text = fpTool.fParam[Id].SigleZoffset.ToString();
            //textBox_Total.Text = MyGlobal.globalConfig.TotalZoffset.ToString();
            FindPointTool.RArray = null;
            FindPointTool.CArray = null;
            FindPointTool.Row = null;
            //if (fpTool.roiList[Id].Count > 0)
            //{
            //    hwindow_final1.viewWindow.displayROI(ref fpTool.roiList[Id]);
            //}


            //HSystem.SetSystem("flush_graphic", "true");

        }

        void UpdateRoiParam()
        {
            for (int i = 0; i < fpTool.roiList2.Length; i++)
            {
                for (int j = 0; j < fpTool.roiList2[i].Count; j++)
                {
                    HTuple roiData = fpTool.roiList2[i][j].getModelData();
                    fpTool.fParam[i].roiP[j].CenterRow = roiData[0];
                    fpTool.fParam[i].roiP[j].CenterCol = roiData[1];
                    fpTool.fParam[i].roiP[j].phi = roiData[2];
                    fpTool.fParam[i].roiP[j].Len1 = roiData[3];
                    fpTool.fParam[i].roiP[j].Len2 = roiData[4];                    
                }
            }
        }
        public void GetCurrentFix()
        {
            try
            {
                int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
                IntersetionCoord intersect = new IntersetionCoord();
                //string ok = MyGlobal.flset2.FindIntersectPoint(Id + 1, HeightImage, out intersect);
                HTuple homMaxFix = new HTuple();
                HOperatorSet.VectorAngleToRigid(fpTool.intersectCoordList[Id].Row, fpTool.intersectCoordList[Id].Col,
                    0, intersect.Row, intersect.Col, 0, out homMaxFix);
                fpTool.intersectCoordList[Id] = intersect;
            }
            catch (Exception)
            {

                throw;
            }
        }

        int trackBarValue1 = 0;
        int trackBarValue2 = 0;
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar tbar = (TrackBar)sender;
                //if (tbar.Name == "trackBar1")
                //{
                //    label_S.Text = trackBar1.Value.ToString();
                //    trackBarValue1 = trackBar1.Value;
                //}
                //else
                //{
                //    label_E.Text = trackBar2.Value.ToString();
                //    trackBarValue2 = trackBar2.Value;
                //}
                //button11_Click(sender, e);

                int Id = Convert.ToInt32(SideName.Substring(4, 1));
                HTuple row, col; HTuple anchor, anchorc;
                //fpTool.FindMaxPt(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection);
                int roiID = -1;
                for (int i = 0; i < fpTool.fParam[Id - 1].roiP.Count; i++)
                {

                    for (int j = 0; j < fpTool.fParam[Id - 1].roiP[i].NumOfSection; j++)
                    {
                        roiID++;
                        if (roiID == CurrentIndex - 1)
                        {
                            break;
                        }
                    }
                    if (roiID == CurrentIndex - 1)
                    {
                        roiID = i;
                        break;
                    }

                }
                if (fpTool.fParam[Id - 1].roiP[roiID].SelectedType == 0)
                {
                    if (fpTool.fParam[Id - 1].roiP[roiID].TopDownDist != 0 && fpTool.fParam[Id - 1].roiP[roiID].xDist != 0)
                    {
                        fpTool.FindMaxPtFallDown(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection,false,hwindow_final2);
                    }
                    else
                    {
                        fpTool.FindMaxPt(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final2, ShowSection,false,hwindow_final1);
                    }

                        
                }
                else
                {
                    fpTool.FindMaxPtFallDown(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection,false,hwindow_final2);
                }
                //if (trackBarValue1<trackBarValue2)
                //{
                //    HTuple row2, col2;
                //    ShowProfile(out row2,out col2);

                //    HTuple newRow = row2; HTuple newCol = col2;
                //    int start = 0; int end = 0;
                //    if (trackBarValue2 >= row2.Length)
                //    {
                //        trackBarValue2 = row2.Length - 1;
                //    }

                //    start = trackBarValue1;
                //    end = trackBarValue2;

                //    HTuple rowLast = newRow.TupleSelectRange(start, end);
                //    HTuple colLast = newCol.TupleSelectRange(start, end);
                //    HObject ContourSelect = new HObject();
                //    HOperatorSet.GenRegionPoints(out ContourSelect, rowLast, colLast);
                //    //HOperatorSet.GenContourPolygonXld(out ContourSelect, rowLast, colLast);
                //    hwindow_final1.viewWindow.displayHobject(ContourSelect, "blue");
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void button11_Click(object sender, EventArgs e)
        {
            if (trackBarValue1 < trackBarValue2 && FindPointTool.RArray.GetLength(0) > 0)
            {
                

                if (trackBarValue1 < trackBarValue2)
                {
                    HTuple row2 =new HTuple(), col2 = new HTuple();
                    //ShowProfile(CurrentIndex - 1, out row2, out col2);

                    HTuple newRow = row2; HTuple newCol = col2;
                    int start = 0; int end = 0;
                    if (trackBarValue2 >= row2.Length)
                    {
                        trackBarValue2 = row2.Length - 1;
                    }

                    start = trackBarValue1;
                    end = trackBarValue2;

                    HTuple rowLast = newRow.TupleSelectRange(start, end);
                    HTuple colLast = newCol.TupleSelectRange(start, end);
                    HObject ContourSelect = new HObject();
                    HOperatorSet.GenRegionPoints(out ContourSelect, rowLast, colLast);
                    //HOperatorSet.GenContourPolygonXld(out ContourSelect, rowLast, colLast);
                    if (ShowSection)
                    {
                        hwindow_final1.viewWindow.displayHobject(ContourSelect, "blue");
                    }

                }
            }

        }

        private void cb_LorR_CheckedChanged(object sender, EventArgs e)
        {
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            //fpTool.fParam[Id].BeLeft = cb_LorR.Checked;
        }

        private void textBox_Start_TextChanged(object sender, EventArgs e)
        {
            if (isLoading)
            {
                return;
            }
            TextBox tb = (TextBox)sender;
            string Num = tb.Text.ToString();
            bool ok = Regex.IsMatch(Num, @"(?i)^(\-[0-9]{1,}[.][0-9]*)+$") || Regex.IsMatch(Num, @"(?i)^(\-[0-9]{1,}[0-9]*)+$") || Regex.IsMatch(Num, @"(?i)^([0-9]{1,}[0-9]*)+$");
            if (ok)
            {
                int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
                switch (tb.Name)
                {
                    case "textBox_Start":
                        //fpTool.fParam[Id].StartPt = int.Parse(Num);
                        break;
                    case "textBox_End":
                        //fpTool.fParam[Id].EndPt = int.Parse(Num);
                        break;
                    case "tb_Updown":
                        //fpTool.fParam[Id].UpDownDist = double.Parse(Num);
                        break;

                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            HTuple Row = new HTuple(), Col = new HTuple();
            int Id = Convert.ToInt32(SideName.Substring(4, 1));
            //string ok =  Fit(Id ,out Row, out Col,hwindow_final1);

            for (int i = 0; i < FindPointTool.RArray.GetLength(0); i++)
            {
                HTuple row, col; HTuple anchor, anchorc;
                fpTool.FindMaxPt(Id, i, out row, out col, out anchor, out anchorc, hwindow_final1);

                Row = Row.TupleConcat(row);
                Col = Col.TupleConcat(col);
            }
            //if (ok!="OK")
            //{
            //    MessageBox.Show(ok);
            //}
            HObject line = new HObject();
            HOperatorSet.GenContourPolygonXld(out line, Row, Col);

            HTuple Rowbg, Colbg, RowEd, ColEd, Nr, Nc, Dist;
            HOperatorSet.FitLineContourXld(line, "tukey", -1, 0, 5, 2, out Rowbg, out Colbg, out RowEd, out ColEd, out Nr, out Nc, out Dist);
            HObject Contourline = new HObject();
            HOperatorSet.GenContourPolygonXld(out Contourline, Rowbg.TupleConcat(RowEd), Colbg.TupleConcat(ColEd));
            //HOperatorSet.SmoothContoursXld(line, out Contourline, 25);           
            hwindow_final2.viewWindow.displayHobject(line);

            hwindow_final2.viewWindow.displayHobject(Contourline, "green");
            MessageBox.Show("拟合成功！");
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (CurrentIndex == 0)
            {
                return;
            }
            if (CurrentIndex == 1)
            {
                CurrentIndex = 1;
                textBox_Current.Text = CurrentIndex.ToString();
                return;
            }
            if (CurrentIndex > 1)
            {
                CurrentIndex--;
                trackBarControl1.Value = CurrentIndex;
                textBox_Current.Text = CurrentIndex.ToString();

                //button11_Click(sender, e);
                int Id = Convert.ToInt32(SideName.Substring(4, 1));
                HTuple row, col; HTuple anchor, anchorc;
                //fpTool.FindMaxPt(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection);
                int roiID = -1;
                for (int i = 0; i < fpTool.fParam[Id - 1].roiP.Count; i++)
                {

                    for (int j = 0; j < fpTool.fParam[Id - 1].roiP[i].NumOfSection; j++)
                    {
                        roiID++;
                        if (roiID == CurrentIndex - 1)
                        {
                            break;
                        }
                    }
                    if (roiID == CurrentIndex - 1)
                    {
                        roiID = i;
                        break;
                    }

                }
                if (fpTool.fParam[Id - 1].roiP[roiID].SelectedType == 0)
                {
                    if (fpTool.fParam[Id - 1].roiP[roiID].TopDownDist != 0 && fpTool.fParam[Id - 1].roiP[roiID].xDist != 0)
                    {
                        fpTool.FindMaxPtFallDown(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection,false,hwindow_final2);
                    }
                    else
                    {
                        fpTool.FindMaxPt(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final2, ShowSection,false,hwindow_final1);
                    }
                   
                }
                else
                {
                    fpTool.FindMaxPtFallDown(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection,false,hwindow_final2);
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

            int Total = Convert.ToInt32(textBox_Total.Text.ToString());
            if (CurrentIndex >= Total)
            {
                CurrentIndex = Total;
                textBox_Current.Text = CurrentIndex.ToString();
                trackBarControl1.Value = CurrentIndex;
                return;
            }
            CurrentIndex++;
            trackBarControl1.Value = CurrentIndex;
            textBox_Current.Text = CurrentIndex.ToString();
            //HTuple row, col;
            //ShowProfile(out row, out col);
            //button11_Click(sender, e);
            int Id = Convert.ToInt32(SideName.Substring(4, 1));
            HTuple row, col; HTuple anchor, anchorc;
            int roiID = -1;
            for (int i = 0; i < fpTool.fParam[Id - 1].roiP.Count; i++)
            {

                for (int j = 0; j < fpTool.fParam[Id - 1].roiP[i].NumOfSection; j++)
                {
                    roiID++;
                    if (roiID == CurrentIndex - 1)
                    {
                        break;
                    }
                }
                if (roiID == CurrentIndex - 1)
                {
                    roiID = i;
                    break;
                }

            }
            if (fpTool.fParam[Id - 1].roiP[roiID].SelectedType == 0)
            {
                if (fpTool.fParam[Id - 1].roiP[roiID].TopDownDist != 0 && fpTool.fParam[Id - 1].roiP[roiID].xDist != 0)
                {
                    fpTool.FindMaxPtFallDown(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection,false,hwindow_final2);
                }
                else
                {
                    fpTool.FindMaxPt(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final2, ShowSection,false,hwindow_final1);
                }
                
            }
            else
            {
                fpTool.FindMaxPtFallDown(Id, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection,false,hwindow_final2);
            }

        }
        bool ShowSection = false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ShowSection = checkBox1.Checked;
        }
        string SideName = "Side1";

        public void FitLineParamSave()
        {
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            ParamPath.ParaName = SideName;
            ParamPath.IsRight = isRight;
            string Name = this.Text;
            if (Name != "Fix")
            {
                //GetCurrentFix();               
                //StaticOperate.WriteXML(MyGlobal.globalConfig, MyGlobal.AllTypePath + "Global.xml");
                if (isRight)
                {
                    StaticOperate.WriteXML(fpTool.fParam[Id], MyGlobal.ConfigPath_Right + SideName + ".xml");
                    hwindow_final1.viewWindow.saveROI(fpTool.roiList[Id], MyGlobal.ConfigPath_Right + SideName + "_Section.roi");
                    hwindow_final2.viewWindow.saveROI(fpTool.roiList2[Id], MyGlobal.ConfigPath_Right + SideName + "_Region.roi");
                }
                else
                {
                    StaticOperate.WriteXML(fpTool.fParam[Id], MyGlobal.ConfigPath_Left + SideName + ".xml");
                    hwindow_final1.viewWindow.saveROI(fpTool.roiList[Id], MyGlobal.ConfigPath_Left + SideName + "_Section.roi");
                    hwindow_final2.viewWindow.saveROI(fpTool.roiList2[Id], MyGlobal.ConfigPath_Left + SideName + "_Region.roi");
                }
                               
            }
            else
            {
                StaticOperate.WriteXML(fpTool.fParam[Id], ParamPath.ParamDir + SideName + ".xml");
                hwindow_final1.viewWindow.saveROI(fpTool.roiList[Id], ParamPath.ParamDir + SideName + "_Section.roi");
                hwindow_final2.viewWindow.saveROI(fpTool.roiList2[Id], ParamPath.ParamDir + SideName + "_Region.roi");
            }

           
            isSave = true;
           
            if (Name == "Fix")
            {
                if (MessageBox.Show("是否重新写入模板位置？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    fpTool.intersectCoordList[Id] = intersection;
                    StaticOperate.WriteXML(fpTool.intersectCoordList[Id], ParamPath.Path_Param);
                }
            }
            else
            {
                if (intersection.Row == 0 && intersection.Col == 0)
                {
                    MessageBox.Show("保存成功！");
                    return;
                }
                else
                {
                    fpTool.intersectCoordList[Id] = intersection;
                    StaticOperate.WriteXML(fpTool.intersectCoordList[Id], ParamPath.Path_Param);
                }
            }

            //MyGlobal.flset2.Init();
            MyGlobal.Right_findPointTool_Find.Init("FitLineSet", isRight);
            MyGlobal.Left_findPointTool_Find.Init("FitLineSet", isRight);
            MyGlobal.Right_findPointTool_Fix.Init("Fix", isRight);
            MyGlobal.Left_findPointTool_Fix.Init("Fix", isRight);
            MessageBox.Show("保存成功！");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isRight)
            {
                if (MessageBox.Show("是否保存右工位参数", "提示", MessageBoxButtons.YesNo)== DialogResult.Yes)
                {
                    FitLineParamSave();
                }
            }
            else
            {
                if (MessageBox.Show( "是否保存左工位参数", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    FitLineParamSave();
                }

            }
           
            //simpleButton7_Click(sender, e);

        }

        string CurrentSide = "";
        bool isSave = false;
        bool NoChange = false;
        bool isCloing = false;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (isCloing || NoChange || comboBox1.SelectedItem.ToString() == CurrentSide)
            {
                isCloing = false;
                NoChange = false;
                return;
            }
            if (comboBox1.SelectedItem != null)
            {
                if (isSave)
                {
                    isSave = false;

                }
                else
                {
                    DialogResult result = MessageBox.Show("当前参数未保存，是否切换?", "提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                    {

                        if (comboBox1.SelectedItem.ToString() != CurrentSide)
                        {
                            NoChange = true;
                            comboBox1.SelectedItem = CurrentSide;
                            NoChange = false;
                        }

                        return;
                    }
                }


                SideName = comboBox1.SelectedItem.ToString();
                ParamPath.ParaName = SideName;
                ParamPath.IsRight = isRight;
                hwindow_final1.viewWindow.notDisplayRoi();
                hwindow_final2.viewWindow.notDisplayRoi();
                hwindow_final1.ClearWindow();
                hwindow_final2.ClearWindow();

                ChangeSide();

                if (!HeightImage.IsInitialized())
                {
                    return;
                }


                CurrentSide = comboBox1.SelectedItem.ToString();

                //int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
                //if (File.Exists(MyGlobal.ConfigPath + SideName + ".xml"))
                //{
                //   fpTool.fParam[Id] = (FitProfileParam)StaticOperate.ReadXML(MyGlobal.ConfigPath + SideName + ".xml", typeof(FitProfileParam));
                //   LoadToUI(Id);
                //}
                //if (File.Exists(MyGlobal.ConfigPath + SideName[Id] + "_Section.roi"))
                //{
                //    hwindow_final1.viewWindow.loadROI(MyGlobal.ConfigPath + SideName[Id] + "_Section.roi", out fpTool.roiList[Id]);
                //}            
                //if (File.Exists(MyGlobal.ConfigPath + SideName[Id] + "_Region.roi"))
                //{
                //    hwindow_final2.viewWindow.loadROI(MyGlobal.ConfigPath + SideName[Id] + "_Region.roi", out fpTool.fpTool.roiList2[Id]);
                //}
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //if (roiCount==1 || RArray.GetLength(0)==0)
            //{
            //    hwindow_final1.viewWindow.displayROI(ref fpTool.roiList);
            //    return;
            //}
            //roiController.setROIShape(new ROIRectangle1());          
            //roiCount++;
        }
        bool isGenSection = false;
        private void button4_Click(object sender, EventArgs e)
        {
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            hwindow_final2.viewWindow.notDisplayRoi();
            if (fpTool.roiList2[Id].Count > 0)
            {
                if (SelectAll)
                {
                    roiController2.viewController.ShowAllRoiModel = -1;
                }
                hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[Id]);
            }


            if (hwindow_final2.Image == null || !hwindow_final2.Image.IsInitialized())
            {
                return;
            }
            roiController2.setROIShape(new ROIRectangle2());
            isGenSection = true;
            //fpTool.fpTool.roiList2[Id].Add(new ROIRectangle2());

            ////每个区域单独设置Roi
            //hwindow_final1.viewWindow.genRect1(400, 400, 600, 600, ref fpTool.roiList[Id]);

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;

            if (dataGridView1.CurrentCell == null)
            {
                return;
            }
            int RowId = dataGridView1.CurrentCell.RowIndex;
            RowId = CurrentRowIndex;
            string Name = dataGridView1.Rows[RowId].Cells[1].Value.ToString();

            if (fpTool.roiList2[Id].Count > 0 && roiController2.ROIList.Count == fpTool.roiList2[Id].Count)
            {
                if (RowId >= 0)
                {
                    fpTool.roiList2[Id].RemoveAt(RowId);
                    roiController2.setActiveROIIdx(RowId);
                    roiController2.removeActive();

                    fpTool.fParam[Id].roiP.RemoveAt(RowId);
                    fpTool.roiList[Id].RemoveAt(RowId);
                    fpTool.fParam[Id].DicPointName.RemoveAt(RowId);
                    dataGridView1.Rows.RemoveAt(RowId);
                    //排序
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = i + 1;
                    }

                    //int[] values = DicPointName[Id].Values.ToArray();
                    //string[] keys = DicPointName[Id].Keys.ToArray();
                    ////listBox1.Items.CopyTo(keys, 0);
                    //keys = richTextBox1.Lines;
                    //for (int i = Index; i < DicPointName[Id].Count; i++)
                    //{

                    //        int value1 = DicPointName[Id][keys[i]];
                    //    DicPointName[Id][keys[i]] = value1 - 1;//前移

                    //}      
                }
                //刷新界面roi


                hwindow_final2.viewWindow.notDisplayRoi();
                if (fpTool.roiList2[Id].Count > 0)
                {
                    hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[Id]);
                }
                if (fpTool.roiList2[Id].Count > 0)
                {
                    hwindow_final2.viewWindow.selectROI(0);
                }
                if (RowId == CopyId)
                {
                    CopyId = -1;
                }
            }
        }

        private void 删除所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            if (!(MessageBox.Show("是否删除所有区域", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes))
            {
                return;
            }
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            if (fpTool.roiList2[Id].Count > 0)
            {
                fpTool.roiList2[Id].Clear();
                hwindow_final2.viewWindow.notDisplayRoi();
                dataGridView1.Rows.Clear();


                fpTool.roiList[Id].Clear();
                hwindow_final1.viewWindow.notDisplayRoi();
                PreSelect = "";
                fpTool.fParam[Id].DicPointName.Clear();
                if (Id >= 0)
                {
                    fpTool.fParam[Id].roiP.Clear();
                }
            }
            CurrentRowIndex = -1;
            CopyId = -1;
        }
        List<ROI> temp = new List<ROI>();
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int SideId = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            TextBox tb = (TextBox)sender;
            string index = tb.Text.ToString();
            bool ok1 = Regex.IsMatch(index, @"^[-]?\d+[.]?\d*$");//是否为数字
            bool ok = Regex.IsMatch(index, @"^([-]?)\d*$");//是否为整数
            if (!(ok && ok1) || fpTool.roiList2[SideId].Count == 0 || RoiIsMoving)
            {
                return;
            }
            try
            {


                double a = Convert.ToDouble(index);
                int num = (int)a;

                int roiID = dataGridView1.CurrentCell.RowIndex;
                roiID = CurrentRowIndex;
                int count = dataGridView1.SelectedCells.Count;
                List<int> rowInd = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    int Ind = dataGridView1.SelectedCells[i].RowIndex;
                    if (!rowInd.Contains(Ind))
                    {
                        rowInd.Add(Ind);
                    }
                }
                for (int i = 0; i < rowInd.Count; i++)
                {
                    roiID = rowInd[i];
                    HTuple[] lineCoord = new HTuple[1];
                    switch (tb.Name)
                    {
                        case "textBox_Num":
                            fpTool.fParam[SideId].roiP[roiID].NumOfSection = num;

                            fpTool.DispSection((ROIRectangle2)fpTool.roiList2[SideId][roiID], SideId, roiID, out lineCoord, hwindow_final2);
                            break;
                        case "textBox_Len":
                            fpTool.fParam[SideId].roiP[roiID].Len1 = num;
                            HTuple temp = new HTuple();
                            List<ROI> temproi = new List<ROI>();
                            temp = fpTool.roiList2[SideId][roiID].getModelData();
                            hwindow_final2.viewWindow.genRect2(fpTool.fParam[SideId].roiP[roiID].CenterRow, fpTool.fParam[SideId].roiP[roiID].CenterCol, fpTool.fParam[SideId].roiP[roiID].phi, fpTool.fParam[SideId].roiP[roiID].Len1, fpTool.fParam[SideId].roiP[roiID].Len2, ref temproi);
                            //hwindow_final2.viewWindow.genRect2(temp[0].D, temp[1].D, temp[2].D, fpTool.fParam[SideId].roiP[roiID].Len1, fpTool.fParam[SideId].roiP[roiID].Len2, ref temproi);
                            fpTool.roiList2[SideId][roiID] = temproi[0];
                            //hwindow_final2.viewWindow.notDisplayRoi();
                            //hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[SideId]);
                            //hwindow_final2.viewWindow.selectROI(roiID);
                            //DispSection((ROIRectangle2)fpTool.roiList2[SideId][roiID], SideId, roiID, out lineCoord, hwindow_final2);
                            break;
                        case "textBox_Width":
                            fpTool.fParam[SideId].roiP[roiID].Len2 = num;
                            HTuple temp3 = new HTuple();
                            temp3 = fpTool.roiList2[SideId][roiID].getModelData();
                            List<ROI> temproi3 = new List<ROI>();
                            hwindow_final2.viewWindow.genRect2(fpTool.fParam[SideId].roiP[roiID].CenterRow, fpTool.fParam[SideId].roiP[roiID].CenterCol, fpTool.fParam[SideId].roiP[roiID].phi, fpTool.fParam[SideId].roiP[roiID].Len1, fpTool.fParam[SideId].roiP[roiID].Len2, ref temproi3);
                            fpTool.roiList2[SideId][roiID] = temproi3[0];
                            //hwindow_final2.viewWindow.notDisplayRoi();
                            //hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[SideId]);
                            //hwindow_final2.viewWindow.selectROI(roiID);
                            //DispSection((ROIRectangle2)fpTool.roiList2[SideId][roiID], SideId, roiID, out lineCoord, hwindow_final2);
                            break;
                        case "textBox_Row":
                            fpTool.fParam[SideId].roiP[roiID].CenterRow = num;
                            HTuple temp4 = new HTuple();
                            temp4 = fpTool.roiList2[SideId][roiID].getModelData();
                            List<ROI> temproi4 = new List<ROI>();
                            hwindow_final2.viewWindow.genRect2(fpTool.fParam[SideId].roiP[roiID].CenterRow, fpTool.fParam[SideId].roiP[roiID].CenterCol, fpTool.fParam[SideId].roiP[roiID].phi, fpTool.fParam[SideId].roiP[roiID].Len1, fpTool.fParam[SideId].roiP[roiID].Len2, ref temproi4);
                            fpTool.roiList2[SideId][roiID] = temproi4[0];
                            //hwindow_final2.viewWindow.notDisplayRoi();
                            //hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[SideId]);
                            //hwindow_final2.viewWindow.selectROI(roiID);
                            //DispSection((ROIRectangle2)fpTool.roiList2[SideId][roiID], SideId, roiID, out lineCoord, hwindow_final2);
                            break;
                        case "textBox_Col":
                            fpTool.fParam[SideId].roiP[roiID].CenterCol = num;
                            HTuple temp5 = new HTuple();
                            temp3 = fpTool.roiList2[SideId][roiID].getModelData();
                            List<ROI> temproi5 = new List<ROI>();
                            hwindow_final2.viewWindow.genRect2(fpTool.fParam[SideId].roiP[roiID].CenterRow, fpTool.fParam[SideId].roiP[roiID].CenterCol, fpTool.fParam[SideId].roiP[roiID].phi, fpTool.fParam[SideId].roiP[roiID].Len1, fpTool.fParam[SideId].roiP[roiID].Len2, ref temproi5);
                            fpTool.roiList2[SideId][roiID] = temproi5[0];
                            //hwindow_final2.viewWindow.notDisplayRoi();
                            //hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[SideId]);
                            //hwindow_final2.viewWindow.selectROI(roiID);
                            //DispSection((ROIRectangle2)fpTool.roiList2[SideId][roiID], SideId, roiID, out lineCoord, hwindow_final2);
                            break;
                        case "textBox_phi":
                            HTuple rad = new HTuple();
                            HOperatorSet.TupleRad(num, out rad);
                            fpTool.fParam[SideId].roiP[roiID].phi = rad;
                            HTuple temp6 = new HTuple();
                            temp3 = fpTool.roiList2[SideId][roiID].getModelData();
                            List<ROI> temproi6 = new List<ROI>();
                            hwindow_final2.viewWindow.genRect2(fpTool.fParam[SideId].roiP[roiID].CenterRow, fpTool.fParam[SideId].roiP[roiID].CenterCol, fpTool.fParam[SideId].roiP[roiID].phi, fpTool.fParam[SideId].roiP[roiID].Len1, fpTool.fParam[SideId].roiP[roiID].Len2, ref temproi6);
                            fpTool.roiList2[SideId][roiID] = temproi6[0];
                            //hwindow_final2.viewWindow.notDisplayRoi();
                            //hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[SideId]);
                            //hwindow_final2.viewWindow.selectROI(roiID);
                            //DispSection((ROIRectangle2)fpTool.roiList2[SideId][roiID], SideId, roiID, out lineCoord, hwindow_final2);
                            break;
                        case "textBox_Deg":
                            fpTool.fParam[SideId].roiP[roiID].AngleOfProfile = num;
                            break;
                    }
                }
                hwindow_final2.viewWindow.notDisplayRoi();
                hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[SideId]);
                hwindow_final2.viewWindow.selectROI(roiID);

            }
            catch (Exception)
            {

                throw;
            }

        }
        private int Ignore = 0;
        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }
        bool SelectAll = false;
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SelectAll = checkBox2.Checked;
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            try
            {
                if (/*!RGBImage.IsInitialized() || */!HeightImage.IsInitialized())
                {
                    return;
                }

                int Id = Convert.ToInt32(SideName.Substring(4, 1));
                double[][] Rcoord, Ccoord, Zcoord; string[][] Str;
                hwindow_final2.ClearWindow();
                //hwindow_final2.HobjectToHimage(RGBImage);

                hwindow_final2.HobjectToHimage(IntensityImage);
                if (this.Text == "Fix")
                {
                    //HObject image = new HObject();
                    //HOperatorSet.GenImageConst(out image, "byte", 1500, 20000);

                    string ok1 = "";
                    if (isRight)
                    {
                        ok1 = fpTool.FindIntersectPoint(Id, HeightImage, out intersection, hwindow_final2, true);
                    }
                    else
                    {
                        ok1 = fpTool.FindIntersectPoint(Id, HeightImage, out intersection, hwindow_final2, true);
                    }

                    if (ok1 != "OK")
                    {
                        MessageBox.Show(ok1);
                    }
                    //else
                    //{
                    //    intersectCoordList[Id - 1] = intersection;

                    //}
                }
                else
                {
                    //ChangeSide已定位Roi 不用定位     
                    HTuple[] original = new HTuple[2];
                    string ok = fpTool.FindPoint(Id,isRight, HeightImage, HeightImage, out Rcoord, out Ccoord, out Zcoord, out Str, out original, null, hwindow_final2, true,null,OriginImage);
                    if (ok != "OK")
                    {
                        MessageBox.Show(ok);
                    }
                }

                int Total = FindPointTool.RArray.GetLength(0);
                textBox_Total.Text = Total.ToString();
                textBox_Current.Text = "1";
                CurrentIndex = 1;
                trackBarControl1.Properties.Maximum = Total;
                trackBarControl1.Properties.Minimum = 1;
                trackBarControl1.Value = 1;
                trackBarControl1_ValueChanged(sender, e);
            }
            catch (Exception)
            {

                throw;
            }
        }


        //#region 弃用
        //[Obsolete]
        //public string Init()
        //{
        //    try
        //    {
        //        HWindow_Final hwnd = new HWindow_Final();
        //        string[] SideName = { "Side1", "Side2", "Side3", "Side4" };
        //        string initError = "OK";
        //        for (int i = 0; i < 4; i++)
        //        {
        //            fpTool.fParam[i] = new FitProfileParam();
        //            fpTool.roiList[i] = new List<ROI>();
        //            fpTool.roiList2[i] = new List<ROI>();
        //            //fpTool.roiList3[i] = new List<ROI>();
        //            fpTool.fParam[i].DicPointName = new List<string>();
        //            //hwindow_final2.viewWindow.genRect1(400, 400, 1000, 1000, ref fpTool.roiList3[i]);
        //            hwindow_final2.viewWindow.notDisplayRoi();

        //        }
        //        for (int i = 0; i < 4; i++)
        //        {
        //            ParamPath.ParaName = "Side" + (i + 1).ToString();
        //            if (!Directory.Exists(ParamPath.ParamDir))
        //            {
        //                Directory.CreateDirectory(ParamPath.ParamDir);
        //            }
        //        }
        //        if (this.Text == "Fix")
        //        {
        //            for (int i = 0; i < 4; i++)
        //            {
        //                ParamPath.ParaName = "Side" + (i + 1).ToString();
        //                if (File.Exists(ParamPath.ParamDir + SideName[i] + ".xml"))
        //                {
        //                    fpTool.fParam[i] = (FitProfileParam)StaticOperate.ReadXML(ParamPath.ParamDir + SideName[i] + ".xml", typeof(FitProfileParam));
        //                }
        //                else
        //                {
        //                    initError = "定位参数加载失败";
        //                    continue;
        //                }
        //                if (File.Exists(ParamPath.ParamDir + SideName[i] + "_Section.roi"))
        //                {
        //                    hwnd.viewWindow.loadROI(ParamPath.ParamDir + SideName[i] + "_Section.roi", out fpTool.roiList[i]);
        //                    hwnd.viewWindow.notDisplayRoi();
        //                }
        //                else
        //                {
        //                    initError = "截面设置Roi加载失败";
        //                    continue;
        //                }
        //                if (File.Exists(ParamPath.ParamDir + SideName[i] + "_Region.roi"))
        //                {
        //                    hwnd.viewWindow.loadROI(ParamPath.ParamDir + SideName[i] + "_Region.roi", out fpTool.roiList2[i]);
        //                    hwnd.viewWindow.notDisplayRoi();
        //                }
        //                else
        //                {
        //                    initError = "感兴趣区域加载失败";
        //                    continue;
        //                }

        //                if (File.Exists(ParamPath.Path_Param))
        //                {
        //                    fpTool.intersectCoordList[i] = (IntersetionCoord)StaticOperate.ReadXML(ParamPath.Path_Param, typeof(IntersetionCoord));
        //                }
        //                else
        //                {
        //                    initError = "定位设置参数.xml加载失败";
        //                    continue;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < 4; i++)
        //            {
        //                if (File.Exists(MyGlobal.ConfigPath + SideName[i] + ".xml"))
        //                {
        //                    fpTool.fParam[i] = (FitProfileParam)StaticOperate.ReadXML(MyGlobal.ConfigPath + SideName[i] + ".xml", typeof(FitProfileParam));
        //                }
        //                else
        //                {
        //                    initError = "抓边参数加载失败";
        //                    continue;
        //                }
        //                if (File.Exists(MyGlobal.ConfigPath + SideName[i] + "_Section.roi"))
        //                {
        //                    hwnd.viewWindow.loadROI(MyGlobal.ConfigPath + SideName[i] + "_Section.roi", out fpTool.roiList[i]);
        //                    hwnd.viewWindow.notDisplayRoi();
        //                }
        //                else
        //                {
        //                    initError = "截面设置Roi加载失败";
        //                    continue;
        //                }
        //                if (File.Exists(MyGlobal.ConfigPath + SideName[i] + "_Region.roi"))
        //                {
        //                    hwnd.viewWindow.loadROI(MyGlobal.ConfigPath + SideName[i] + "_Region.roi", out fpTool.roiList2[i]);
        //                    hwnd.viewWindow.notDisplayRoi();
        //                }
        //                else
        //                {
        //                    initError = "感兴趣区域加载失败";
        //                    continue;
        //                }

        //                ParamPath.ParaName = "Side" + (i + 1).ToString();
        //                if (File.Exists(ParamPath.Path_Param))
        //                {
        //                    fpTool.intersectCoordList[i] = (IntersetionCoord)StaticOperate.ReadXML(ParamPath.Path_Param, typeof(IntersetionCoord));
        //                }
        //                else
        //                {
        //                    initError = "定位设置参数.xml加载失败";
        //                    continue;
        //                }
        //            }
        //        }
        //        return initError;
        //    }
        //    catch (Exception ex)
        //    {

        //        return "参数设置加载失败:" + ex.Message;
        //    }

        //}
        //[Obsolete]
        //void GenIntesityProfile(List<SagensSdk.Profile> profile, out HObject Image)
        //{
        //    int len = profile.Count;
        //    int width = profile[0].points.Length;
        //    byte[] imageArray = new byte[width * len];
        //    int k = 0;
        //    for (int i = 0; i < len; i++)
        //    {
        //        for (int j = 0; j < width; j++)
        //        {
        //            imageArray[k] = profile[i].points[j].Intensity;
        //            k++;
        //        }
        //    }
        //    Image = new HObject();
        //    HOperatorSet.GenEmptyObj(out Image);
        //    MyGlobal.GoSDK.GenHalconImage(imageArray, width, len, out Image);

        //}
        //[Obsolete]
        //void GetRowCol(ProfilePoint[] point, out double[] Row, out double[] Col)
        //{
        //    long len = point.Length;
        //    Row = new double[len]; Col = new double[len];
        //    for (int i = 0; i < len; i++)
        //    {
        //        Row[i] = point[i].z / 0.006;
        //        Col[i] = point[i].x / 0.006;
        //    }
        //}
        //[Obsolete]
        //public static string GetXData(ref Dictionary<string, double[]> dic, out List<HTuple> RowCoord)
        //{
        //    try
        //    {

        //        RowCoord = new List<HTuple>();
        //        if (Resolution == 0)
        //        {
        //            return "No Resolution";
        //        }
        //        int Index = dic.Count;
        //        for (int i = 0; i < Index; i++)
        //        {
        //            RowCoord.Add(new HTuple());
        //            string key = i.ToString() + "_" + "0";
        //            if (dic.Keys.Contains(key))
        //            {
        //                int n = 0;

        //                for (int j = 0; j < dic[key].Length; j++)
        //                {
        //                    if (dic[key][j] == -32768)
        //                    {
        //                        int BB = 0;
        //                    }
        //                    else
        //                    {
        //                        RowCoord[i][n] = ((j) * Resolution + StartFov) * 200;
        //                        dic[key][n] = dic[key][j] * 200;
        //                        n++;
        //                    }

        //                }
        //            }

        //        }
        //        return "OK";
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //[Obsolete]
        //public static string GetArryData(List<Profile> Profile, out double[,] rc)
        //{

        //    int Num = Profile.Count;
        //    int len = Profile[0].points.Length;
        //    //将 X方向添加到最后； 
        //    rc = new double[Num + 1, len];

        //    try
        //    {
        //        for (int i = 0; i < Profile.Count; i++)
        //        {
        //            for (int j = 0; j < Profile[i].points.Length; j++)
        //            {
        //                rc[i, j] = Profile[i].points[j].z;
        //            }
        //        }
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "ProfileDataToArray error " + ex.Message;
        //    }
        //}
        //[Obsolete]
        //public static string GetRC(List<Profile> Profile, out double[][] rArray, out double[][] cArray)
        //{

        //    if (MyGlobal.globalConfig.dataContext.xResolution != 0)
        //    {
        //        MyGlobal.globalConfig.dataContext = MyGlobal.GoSDK.context;
        //        Resolution = MyGlobal.globalConfig.dataContext.xResolution;
        //    }

        //    int Num = Profile.Count;
        //    int len = Profile[0].points.Length;
        //    //将 X方向添加到最后； 
        //    //rc = new double[Num + 1, len];
        //    rArray = new double[Num][];
        //    cArray = new double[Num][];

        //    try
        //    {
        //        for (int i = 0; i < Profile.Count; i++)
        //        {
        //            rArray[i] = new double[Profile[i].points.Length];
        //            cArray[i] = new double[Profile[i].points.Length];
        //            for (int j = 0; j < Profile[i].points.Length; j++)
        //            {
        //                //rc[i, j] = Profile[i].points[j].z;
        //                if (Profile[i].points[j].z != -30)
        //                {
        //                    rArray[i][j] = Profile[i].points[j].z * 200;

        //                }
        //                else
        //                {
        //                    rArray[i][j] = -Profile[i].points[j].z * 200; // 取反
        //                }

        //                //rArray[i][j] = Profile[i].points[j].z * 200;
        //                cArray[i][j] = ((j) * Resolution + StartFov) * 200;
        //            }
        //        }
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "ProfileDataToArray error " + ex.Message;
        //    }
        //}
        //[Obsolete]
        //public static string GetC(double[][] rArray, out double[][] cArray)
        //{

        //    int Num = rArray.Rank;
        //    Resolution = MyGlobal.globalConfig.dataContext.xResolution;
        //    cArray = new double[Num][];
        //    try
        //    {
        //        for (int i = 0; i < Num; i++)
        //        {
        //            cArray[i] = new double[rArray[i].Length];
        //            for (int j = 0; j < rArray[i].Length; j++)
        //            {
        //                cArray[i][j] = ((j) * Resolution + StartFov) * 200;
        //            }
        //        }
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "ProfileDataToArray error " + ex.Message;
        //    }
        //}

        //[Obsolete]
        //public static float[] ArrayToFArr(double[,] Pots)
        //{
        //    float[] potArr = new float[(Pots.GetLength(0) - 1) * Pots.GetLength(1)];
        //    for (int i = 0; i < Pots.GetLength(0) - 1; i++)
        //    {
        //        int mulIndex = i * Pots.GetLength(1);
        //        for (int j = 0; j < Pots.GetLength(1); j++)
        //        {
        //            if (Pots[i, j] != -32768)
        //            {
        //                potArr[mulIndex + j] = (float)Pots[i, j];
        //            }
        //            else
        //            {
        //                potArr[mulIndex + j] = 0;
        //            }
        //        }
        //    }
        //    return potArr;
        //}
        //[Obsolete]
        //public string ProfileDataToArray(ref double[,] RCAll, out List<HTuple> RowCoord, out List<HTuple> ColCoord, bool load = false)
        //{
        //    RowCoord = new List<HTuple>();
        //    ColCoord = new List<HTuple>();

        //    try
        //    {


        //        if (load)
        //        {
        //            double start1 = RCAll[RCAll.GetLength(0) - 1, 0];
        //            double start2 = RCAll[RCAll.GetLength(0) - 1, 1];
        //            double sub = Math.Abs((start2 - start1) / 200);
        //            Resolution = Math.Round(sub, 3);
        //            //StartFov = start1 / 200;

        //        }
        //        else
        //        {
        //            MyGlobal.globalConfig.dataContext = MyGlobal.GoSDK.context;
        //            Resolution = MyGlobal.globalConfig.dataContext.xResolution;
        //        }
        //        //IniFileOperater IniOperate = new IniFileOperater(IniPath);
        //        //IniOperate.WriteEntry("GocatorSet", "Resolution", Resolution);
        //        //IniOperate.WriteEntry("GocatorSet", "StartFov", StartFov);

        //        for (int i = 0; i < RCAll.GetLength(0); i++)
        //        {
        //            if (i != RCAll.GetLength(0) - 1)
        //            {
        //                RowCoord.Add(new HTuple());
        //                ColCoord.Add(new HTuple());
        //            }

        //            int n = 0;
        //            int k = 0;
        //            for (int j = 0; j < RCAll.GetLength(1); j++)
        //            {

        //                if (RCAll[i, j] != -32768)
        //                {

        //                    if (i == RCAll.GetLength(0) - 1) //添加到最后
        //                    {
        //                        RCAll[i, n] = ((j) * Resolution + StartFov) * 200;
        //                    }
        //                    else
        //                    {

        //                        RowCoord[i][n] = RCAll[i, j] * 200;

        //                        ColCoord[i][n] = ((j) * Resolution + StartFov) * 200;

        //                    }

        //                    n++;

        //                }
        //                k++;
        //            }

        //        }

        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "ProfileDataToArray error " + ex.Message;
        //    }
        //}
        //[Obsolete]
        ///// <summary>
        ///// 由点获取轮廓
        ///// </summary>
        ///// <param name="isLeft">是否在左边</param>
        ///// <param name="IgnorePoints">起始结束忽略点数</param>
        ///// <param name="Row">输入轮廓行坐标</param>
        ///// <param name="Col">输入轮廓列坐标</param>
        ///// <param name="RowNew">输出转换后轮廓列坐标</param>
        ///// <param name="ColNew">输出转换后轮廓行坐标</param>
        ///// <param name="Region">输处转换后轮廓Region</param>
        ///// <returns></returns>
        //public string GenProfile(bool isLeft, HTuple Row, HTuple Col, out HTuple RowNew, out HTuple ColNew, out HObject Region, out HObject Contour)
        //{
        //    RowNew = new HTuple(); ColNew = new HTuple(); Region = new HObject();
        //    Contour = new HObject();
        //    try
        //    {

        //        HTuple Lenr = Row.Length;
        //        HTuple Lenc = Col.Length;
        //        HTuple min = Lenr.TupleMin2(Lenc);
        //        Row = -Row.TupleSelectRange(0, min.I - 1);
        //        Col = Col.TupleSelectRange(0, min.I - 1);
        //        HOperatorSet.GenContourPolygonXld(out Contour, Row, Col);
        //        HTuple hommat2DIdentity = new HTuple(); HTuple Hommat2DRotate = new HTuple();
        //        HTuple Hommat2DTranslate = new HTuple();
        //        HOperatorSet.HomMat2dIdentity(out hommat2DIdentity);

        //        HTuple ColMin = Col.TupleMin();
        //        HOperatorSet.HomMat2dTranslate(hommat2DIdentity, 500, -ColMin + 500, out Hommat2DTranslate);
        //        HOperatorSet.AffineTransContourXld(Contour, out Contour, Hommat2DTranslate);
        //        HOperatorSet.GetContourXld(Contour, out RowNew, out ColNew);
        //        HOperatorSet.GenRegionPoints(out Region, RowNew, ColNew);
        //        if (isLeft == false)
        //        {
        //            //找到最高点进行镜像变换
        //            HTuple Rowmin = RowNew.TupleMin();
        //            HTuple minInd = RowNew.TupleFindFirst(Rowmin);
        //            HTuple HomMat2DId = new HTuple(); HTuple HomMat2dReflect = new HTuple();
        //            HOperatorSet.HomMat2dIdentity(out HomMat2DId);
        //            HOperatorSet.HomMat2dReflect(HomMat2DId, Rowmin, ColNew[minInd] + 50, Rowmin + 100, ColNew[minInd] + 50, out HomMat2dReflect);
        //            HOperatorSet.AffineTransContourXld(Contour, out Contour, HomMat2dReflect);
        //            HOperatorSet.AffineTransRegion(Region, out Region, HomMat2dReflect, "nearest_neighbor");
        //            HOperatorSet.GetContourXld(Contour, out RowNew, out ColNew);
        //            HOperatorSet.GenRegionPoints(out Region, RowNew, ColNew);
        //        }
        //        return "OK";

        //    }
        //    catch (Exception ex)
        //    {
        //        return "GenProfile error " + ex.Message;
        //    }

        //}
        //[Obsolete]
        //public void ShowProfile(int ProfileId, out HTuple row, out HTuple col, HWindow_Final hwind = null)
        //{
        //    row = new HTuple(); col = new HTuple();
        //    try
        //    {
        //        //显示截取的轮廓
        //        if (hwind != null)
        //        {
        //            hwind.viewWindow.ClearWindow();
        //            HObject image = new HObject();
        //            HOperatorSet.GenImageConst(out image, "byte", 1000, 1000);
        //            hwind.HobjectToHimage(image);
        //        }

        //        if (RArray == null || RArray.GetLength(0) == 0)
        //        {
        //            return;
        //        }

        //        HTuple row1 = -new HTuple(RArray[ProfileId]);
        //        HTuple col1 = new HTuple(CArray[ProfileId]);
        //        if (row1.Length == 0)
        //        {

        //            return;
        //        }

        //        //分辨率 x 0.007--0.01    y 0.035 -- 0.05

        //        double xResolution = MyGlobal.globalConfig.dataContext.xResolution;
        //        double yResolution = MyGlobal.globalConfig.dataContext.yResolution;
        //        HTuple SeqC = new HTuple();
        //        //double s1 = Math.Abs(Math.Cos(Phi[ProfileId][4]));
        //        //double s2 = Math.Abs(Math.Sin(Phi[ProfileId][4]));

        //        double s1 = Math.Cos(Phi[ProfileId][4]);
        //        double s2 = Math.Sin(Phi[ProfileId][4]);

        //        double scale = 0;

        //        scale = xResolution * s1 + yResolution * s2;

        //        //if (s1 >= s2)
        //        //{
        //        //    scale = xResolution / s1;
        //        //}
        //        //else
        //        //{
        //        //    scale = yResolution * s2;
        //        //}

        //        //double scale = xResolution * Math.Cos(Phi[CurrentIndex - 1][4]) + yResolution * Math.Sin(Phi[CurrentIndex - 1][4]);
        //        scale = Math.Abs(scale);
        //        HOperatorSet.TupleGenSequence(scale, (row1.Length + 1) * scale, scale, out col1);
        //        col1 = col1 * 200;
        //        int len1 = row1.Length;
        //        col1 = col1.TupleSelectRange(0, len1 - 1);

        //        HTuple rowmin = row1.TupleMin();


        //        row = row1 - rowmin + 150;
        //        col = col1;
        //        if (hwind != null)
        //        {
        //            HObject contour = new HObject();
        //            HOperatorSet.GenRegionPoints(out contour, row, col);
        //            hwindow_final1.viewWindow.displayHobject(contour, "red", true);
        //        }


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        //[Obsolete]
        //public void FindFirstAnchor(int SideId, out HTuple Row, out HTuple Col)
        //{
        //    Row = new HTuple(); Col = new HTuple();
        //    try
        //    {
        //        int Id = SideId - 1;
        //        if (RArray == null)
        //        {
        //            return;
        //        }
        //        HTuple row = new HTuple(RArray[CurrentIndex - 1]);
        //        HTuple col = new HTuple(CArray[CurrentIndex - 1]);

        //        ShowProfile(CurrentIndex - 1, out row, out col);

        //        if (RArray[CurrentIndex - 1].Length == 0)
        //        {
        //            return;
        //        }
        //        //HTuple row1 = -row;
        //        //HTuple col1 = col;
        //        //row = row1 - 0 + 150;
        //        //col = col1;

        //        //除去 -30 *200 的点
        //        HTuple eq30_1 = row.TupleEqualElem(-6000 - 0 + 150);
        //        HTuple eqId_1 = eq30_1.TupleFind(1);
        //        HTuple temp_1 = row.TupleRemove(eqId_1);
        //        HTuple temp_2 = col.TupleRemove(eqId_1);

        //        //取最左
        //        HTuple maxZCol = true ? temp_2.TupleMin() : temp_2.TupleMax();
        //        //HTuple maxZCol = fpTool.fParam[Id].BeLeft ? temp_2.TupleMin() : temp_2.TupleMax();
        //        HTuple mZRowId = col.TupleFindFirst(maxZCol);
        //        HTuple mZRow = row[mZRowId];
        //        Row = mZRow;
        //        Col = maxZCol;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //[Obsolete]
        ///// <summary>
        ///// 由轮廓拟合点
        ///// </summary>
        ///// <param name="SideId">边序号 从1 开始</param>
        ///// <param name="LastR">输出行坐标</param>
        ///// <param name="LastC">输出列坐标</param>
        ///// <param name="hwnd">输入窗体（可选）</param>
        ///// <returns></returns>
        //public string Fit(int SideId, out HTuple LastR, out HTuple LastC, HWindow_Final hwnd = null)
        //{
        //    LastR = new HTuple(); LastC = new HTuple();
        //    try
        //    {
        //        int Id = SideId - 1;
        //        int k = 0;
        //        for (int i = 0; i < RArray.GetLength(0); i++)
        //        {

        //            HTuple row = new HTuple(RArray[i]);
        //            HTuple col = new HTuple(CArray[i]);
        //            if (RArray[i].Length == 0)
        //            {
        //                continue;
        //            }
        //            HTuple row1 = -row;
        //            HTuple col1 = col;


        //            ////除去 -30 *200 的点
        //            //HTuple eq30 = row1.TupleEqualElem(-6000);
        //            //HTuple eqId = eq30.TupleFind(1);
        //            //HTuple temp = row1.TupleRemove(eqId);

        //            //HTuple minrow = temp.TupleMin();
        //            row = row1 - 0 + 150;
        //            col = col1;
        //            //获取截取有效轮廓最大Z
        //            if (fpTool.fParam[Id].EndPt < row.Length)
        //            {
        //                row = row.TupleSelectRange(fpTool.fParam[Id].StartPt, fpTool.fParam[Id].EndPt);
        //                col = col.TupleSelectRange(fpTool.fParam[Id].StartPt, fpTool.fParam[Id].EndPt);
        //            }

        //            //除去 -30 *200 的点
        //            HTuple eq30_1 = row.TupleEqualElem(-6000 - 0 + 150);
        //            HTuple eqId_1 = eq30_1.TupleFind(1);
        //            HTuple temp_1 = row.TupleRemove(eqId_1);
        //            HTuple temp_2 = col.TupleRemove(eqId_1);
        //            //
        //            //取最上
        //            //HTuple maxZ = temp_1.TupleMin();
        //            //HTuple mZColId = row.TupleFindFirst(maxZ);
        //            //HTuple mZCol = col[mZColId];
        //            //HTuple rowStart = maxZ.D + fpTool.fParam[Id].UpDownDist;
        //            //HTuple rowEnd = rowStart;
        //            //HTuple colStart = mZCol;
        //            //HTuple colEnd = fpTool.fParam[Id].BeLeft ? mZCol - 500 : mZCol + 500;

        //            //取最左
        //            HTuple maxZ = temp_2.TupleMin();

        //            //HTuple maxZ = fpTool.fParam[Id].BeLeft ? temp_2.TupleMin() : temp_2.TupleMax();
        //            HTuple mZRowId = col.TupleFindFirst(maxZ);
        //            HTuple mZRow = row[mZRowId];
        //            HTuple rowStart = mZRow.D - 500;
        //            HTuple rowEnd = mZRow.D + 500;
        //            HTuple colStart = maxZ.D + fpTool.fParam[Id].UpDownDist;
        //            HTuple colEnd = maxZ.D + fpTool.fParam[Id].UpDownDist;

        //            //求交点
        //            HObject Line = new HObject(); HObject Profile = new HObject(); HObject Intersect = new HObject();
        //            HOperatorSet.GenContourPolygonXld(out Line, rowStart.TupleConcat(rowEnd), colStart.TupleConcat(colEnd));
        //            HOperatorSet.GenContourPolygonXld(out Profile, row, col);
        //            HTuple IntersecR, intersecC, isOver;
        //            HOperatorSet.IntersectionContoursXld(Profile, Line, "mutual", out IntersecR, out intersecC, out isOver);
        //            if (hwnd != null && i == 0)
        //            {
        //                hwnd.viewWindow.displayHobject(Line, "green");

        //            }
        //            if (IntersecR.Length == 0)
        //            {
        //                //return "Fit Fail " + "无交点";
        //                continue;
        //            }
        //            if (IntersecR.Length > 1)
        //            {
        //                ////取最上
        //                //HTuple minC = fpTool.fParam[Id].BeLeft ? intersecC.TupleMax() : intersecC.TupleMin();
        //                //HTuple minCid = intersecC.TupleFindFirst(minC);


        //                //LastR = LastR.TupleConcat(IntersecR[minCid]);
        //                //LastC = LastC.TupleConcat( minC);

        //                //取最左
        //                HTuple minR = IntersecR.TupleMin();
        //                HTuple minRid = IntersecR.TupleFindFirst(minR);


        //                LastR = LastR.TupleConcat(IntersecR[minRid]);
        //                LastC = LastC.TupleConcat(minR);
        //            }
        //            else
        //            {
        //                //取最上
        //                LastR = LastR.TupleConcat(IntersecR);
        //                LastC = LastC.TupleConcat(intersecC);

        //                //取最左

        //            }
        //            k++;
        //            HObject cross = new HObject();
        //            HOperatorSet.GenCrossContourXld(out cross, IntersecR, intersecC, 6, 0);
        //            if (hwnd != null && i == 1)
        //            {
        //                hwnd.viewWindow.displayHobject(cross, "green");
        //            }
        //        }

        //        if (LastR.Length == 0)
        //        {
        //            return "Fit fail";
        //        }
        //        HOperatorSet.TupleGenSequence(0, LastR.Length - 1, 1, out LastR);
        //        if (MyGlobal.globalConfig.dataContext.xResolution != 0)
        //        {
        //            LastC = LastC / 200 / MyGlobal.globalConfig.dataContext.xResolution;
        //        }
        //        else
        //        {
        //            LastC = LastC / 200 / 0.007;
        //        }
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "Fit error" + ex.Message;
        //    }

        //}

        //[Obsolete]
        ///// <summary>
        ///// 由轮廓拟合点
        ///// </summary>
        ///// <param name="SideId">边序号 从1 开始</param>
        ///// <param name="LastR">输出行坐标</param>
        ///// <param name="LastC">输出列坐标</param>
        ///// <param name="hwnd">输入窗体（可选）</param>
        ///// <returns></returns>
        //public string FindMaxPtFallDown(int SideId, int ProfileId, out HTuple LastR, out HTuple LastC, out HTuple AnchorR, out HTuple AnchorC, HWindow_Final hwnd = null, bool ShowFeatures = false, bool UseFit = false)
        //{
        //    LastR = new HTuple(); LastC = new HTuple(); AnchorR = new HTuple(); AnchorC = new HTuple();
        //    try
        //    {
        //        HTuple row, col;
        //        ShowProfile(ProfileId, out row, out col, hwnd);

        //        int Id = SideId - 1;
        //        if (RArray == null || RArray[ProfileId].Length == 0)
        //        {
        //            return "RArray is NULL";
        //        }

        //        //除去 -30 *200 的点
        //        HTuple eq30_1 = row.TupleEqualElem(-6000 - 0 + 150);
        //        HTuple eqId_1 = eq30_1.TupleFind(1);
        //        HTuple temp_1 = row.TupleRemove(eqId_1);
        //        HTuple temp_2 = col.TupleRemove(eqId_1);

        //        if (fpTool.roiList[Id].Count == 0)
        //        {
        //            return "fpTool.roiList is Null";
        //        }

        //        //取最左 作为初步锚定点
        //        HTuple maxZCol = true ? temp_2.TupleMin() : temp_2.TupleMax();

        //        //HTuple maxZCol = fpTool.fParam[Id].BeLeft ? temp_2.TupleMin() : temp_2.TupleMax();
        //        HTuple mZRowId = col.TupleFindFirst(maxZCol);
        //        HTuple mZRow = row[mZRowId];


        //        int roiID = -1;
        //        for (int i = 0; i < fpTool.fParam[Id].roiP.Count; i++)
        //        {

        //            for (int j = 0; j < fpTool.fParam[Id].roiP[i].NumOfSection; j++)
        //            {
        //                roiID++;
        //                if (roiID == ProfileId)
        //                {
        //                    break;
        //                }
        //            }
        //            if (roiID == ProfileId)
        //            {
        //                roiID = i;
        //                break;
        //            }
        //        }
        //        // 锚定 Roi
        //        HTuple RoiCoord = fpTool.roiList[Id][roiID].getModelData();
        //        double R1 = RoiCoord[0] + mZRow.D - fpTool.fParam[Id].roiP[roiID].AnchorRow;
        //        double C1 = RoiCoord[1] + maxZCol.D - fpTool.fParam[Id].roiP[roiID].AnchorCol;
        //        double R2 = RoiCoord[2] + mZRow.D - fpTool.fParam[Id].roiP[roiID].AnchorRow;
        //        double C2 = RoiCoord[3] + maxZCol.D - fpTool.fParam[Id].roiP[roiID].AnchorCol;
        //        if (C1 < 0)
        //        {
        //            C1 = 0;
        //        }

        //        //生成矩形框
        //        HObject left = new HObject();
        //        HObject right = new HObject();
        //        HOperatorSet.GenRegionLine(out left, 0, C1, 1000, C1);
        //        HOperatorSet.GenRegionLine(out right, 0, C2, 1000, C2);
        //        if (hwnd != null && ShowFeatures)
        //        {
        //            hwnd.viewWindow.displayHobject(left, "green");
        //            hwnd.viewWindow.displayHobject(right, "green");

        //        }


        //        //求矩形区域内轮廓 极值
        //        HTuple ColLess = temp_2.TupleLessElem(C2);
        //        HTuple ColGreater = temp_2.TupleGreaterElem(C1);
        //        HTuple sub = ColLess.TupleSub(ColGreater);
        //        HTuple IntersetID = sub.TupleFind(0);
        //        //HTuple IntersetID = eq0.TupleFind(1);
        //        if (IntersetID == -1)
        //        {
        //            return "区域内 无有效点";
        //        }
        //        HTuple RowNew = temp_1[IntersetID];
        //        HTuple ColNew = temp_2[IntersetID];

        //        HTuple maxZ = new HTuple(); HTuple mZCol = new HTuple();
        //        //最高点下降
        //        if (fpTool.fParam[Id].roiP[roiID].SelectedType == 1)
        //        {
        //            //取最上
        //            maxZ = RowNew.TupleMin();
        //            HTuple mZColId = RowNew.TupleFindFirst(maxZ);
        //            mZCol = ColNew[mZColId];
        //        }
        //        else
        //        {
        //            //取极值
        //            int deg = fpTool.fParam[Id].roiP[roiID].AngleOfProfile;
        //            GetInflection(RowNew, ColNew, deg, out maxZ, out mZCol, hwnd, true, true, true, ShowFeatures);

        //            if (maxZ.Length == 0)
        //            {
        //                return "Ignore";
        //            }
        //            HTuple Max1 = maxZ.TupleMax();
        //            HTuple maxId1 = maxZ.TupleFindFirst(Max1);

        //            mZCol = mZCol[maxId1];
        //            maxZ = Max1;

        //            if (hwnd != null && ShowFeatures)
        //            {
        //                HObject cross2 = new HObject();
        //                HOperatorSet.GenCrossContourXld(out cross2, maxZ, mZCol, 10, 0);
        //                hwnd.viewWindow.displayHobject(cross2, "blue");

        //            }

        //            if (maxZ.Length == 0)
        //            {
        //                return "Ignore";

        //                //return "FindMaxPt fail";
        //            }
        //        }





        //        double xresolution = MyGlobal.globalConfig.dataContext.xResolution;
        //        HTuple rowStart = maxZ.D + fpTool.fParam[Id].roiP[roiID].TopDownDist * 200;
        //        HTuple rowEnd = rowStart;
        //        HTuple colStart = mZCol;
        //        double dist = fpTool.fParam[Id].roiP[roiID].xDist == 0 ? 300 : fpTool.fParam[Id].roiP[roiID].xDist * 200;
        //        HTuple colEnd = new HTuple();
        //        if (!fpTool.fParam[Id].roiP[roiID].useCenter)
        //        {
        //            colEnd = fpTool.fParam[Id].roiP[roiID].useLeft ? mZCol - dist : mZCol + dist;
        //        }
        //        else
        //        {
        //            //取轮廓中心
        //            colStart = mZCol - dist;
        //            colEnd = mZCol + dist;
        //        }


        //        //求交点
        //        HObject Line = new HObject(); HObject Profile = new HObject(); HObject Intersect = new HObject();
        //        HOperatorSet.GenContourPolygonXld(out Line, rowStart.TupleConcat(rowEnd), colStart.TupleConcat(colEnd));
        //        HOperatorSet.GenContourPolygonXld(out Profile, row, col);
        //        HTuple IntersecR, intersecC, isOver;
        //        HOperatorSet.IntersectionContoursXld(Profile, Line, "mutual", out IntersecR, out intersecC, out isOver);
        //        if (hwnd != null && ShowFeatures)
        //        {
        //            hwnd.viewWindow.displayHobject(Line, "green");

        //        }
        //        if (IntersecR.Length == 0)
        //        {
        //            return "Ignore";

        //        }
        //        if (IntersecR.Length > 1)
        //        {
        //            //取最上
        //            //取距离 极值或最高点 最近的点/最远点
        //            HTuple distMin = new HTuple();

        //            HTuple minC = new HTuple(); HTuple minCid = new HTuple();
        //            if (!fpTool.fParam[Id].roiP[roiID].useCenter)
        //            {
        //                for (int i = 0; i < intersecC.Length; i++)
        //                {
        //                    double innerDis = Math.Abs(intersecC[i].D - mZCol.D);
        //                    distMin = distMin.TupleConcat(innerDis);
        //                }
        //                HTuple minDist = fpTool.fParam[Id].roiP[roiID].useNear ? distMin.TupleMin() : distMin.TupleMax();
        //                HTuple minId = distMin.TupleFindFirst(minDist);
        //                minC = intersecC[minId];
        //                minCid = intersecC.TupleFindFirst(minC);
        //                LastR = LastR.TupleConcat(IntersecR[minCid]);
        //                LastC = LastC.TupleConcat(minC);
        //            }
        //            else
        //            {
        //                ////取轮廓中心
        //                //取离最大值或极值最近的两个点
        //                HTuple distless = new HTuple(); HTuple distgreater = new HTuple();
        //                for (int i = 0; i < intersecC.Length; i++)
        //                {
        //                    double innerDis = intersecC[i].D - mZCol.D;
        //                    if (innerDis < 0)
        //                    {
        //                        distless = distless.TupleConcat(innerDis);
        //                    }
        //                    else
        //                    {
        //                        distgreater = distgreater.TupleConcat(innerDis);
        //                    }
        //                }
        //                HTuple min1 = distless.TupleMax();
        //                HTuple min2 = distgreater.TupleMin();
        //                HTuple minC1 = mZCol.D + min1; HTuple minC2 = mZCol.D + min2;
        //                //取 区间 minCId1 -- minCId2
        //                HTuple MinMaxCol = col.TupleGreaterEqualElem(minC1);
        //                HTuple MinMaxColID = MinMaxCol.TupleFind(1);
        //                HTuple tempcol = col[MinMaxColID];
        //                HTuple temprow = row[MinMaxColID];
        //                HTuple MinMaxCol2 = tempcol.TupleLessEqualElem(minC2);
        //                HTuple MinMaxColID2 = MinMaxCol2.TupleFind(1);
        //                HTuple SegCol = tempcol[MinMaxColID2];
        //                HTuple SegRow = temprow[MinMaxColID2];

        //                HTuple centerR = SegRow.TupleMean();
        //                HTuple centerC = SegCol.TupleMean();
        //                LastR = LastR.TupleConcat(centerR);
        //                LastC = LastC.TupleConcat(centerC);
        //            }


        //        }
        //        else
        //        {
        //            //取最上
        //            LastR = LastR.TupleConcat(IntersecR);
        //            LastC = LastC.TupleConcat(intersecC);

        //            //取最左

        //        }
        //        HObject cross = new HObject();
        //        HOperatorSet.GenCrossContourXld(out cross, IntersecR, intersecC, 6, 0);
        //        if (hwnd != null && ProfileId == 1)
        //        {
        //            hwnd.viewWindow.displayHobject(cross, "white");
        //        }


        //        if (LastR.Length == 0)
        //        {
        //            return "Ignore";
        //        }
        //        HTuple Max = LastR.TupleMax();
        //        HTuple maxId = LastR.TupleFindFirst(Max);

        //        LastC = LastC[maxId];
        //        LastR = Max;
        //        AnchorR = LastR;
        //        AnchorC = LastC;


        //        ////启用中间点
        //        //if (fpTool.fParam[Id].roiP[roiID].useMidPt)
        //        //{
        //        //    HTuple EdgeCol = fpTool.fParam[Id].roiP[roiID].useLeft ? temp_2.TupleMin() : temp_2.TupleMax();
        //        //    HTuple EdgColId = temp_2.TupleFindFirst(EdgeCol);
        //        //    HTuple EdgegRow = temp_1[EdgColId];
        //        //    //取中间点
        //        //    HTuple midR = (LastR + EdgegRow) / 2;
        //        //    HTuple midC = (LastC + EdgeCol) / 2;
        //        //    LastR = midR;
        //        //    LastC = midC;
        //        //}


        //        if (hwnd != null && ShowFeatures)
        //        {
        //            HObject cross1 = new HObject();
        //            HOperatorSet.GenCrossContourXld(out cross1, LastR, LastC, 10, 0);
        //            hwnd.viewWindow.displayHobject(cross1, "green");

        //        }

        //        if (LastR.Length == 0)
        //        {
        //            return "Ignore";

        //            //return "FindMaxPt fail";
        //        }
        //        //HOperatorSet.TupleGenSequence(0, LastR.Length - 1, 1, out LastR);
        //        HTuple PtID = new HTuple();
        //        HOperatorSet.TupleGreaterEqualElem(col, LastC, out PtID);
        //        PtID = PtID.TupleFindFirst(1);
        //        LastR = Row[ProfileId][PtID];
        //        LastC = CArray[ProfileId][PtID];
        //        //LastR = ProfileId;
        //        //if (MyGlobal.globalConfig.dataContext.xResolution != 0)
        //        //{
        //        //    LastC = LastC / 200 / MyGlobal.globalConfig.dataContext.xResolution;
        //        //}
        //        //else
        //        //{
        //        //    LastC = LastC / 200 / 0.007;
        //        //}


        //        if (hwnd != null)
        //        {
        //            HObject cross1 = new HObject();
        //            HOperatorSet.GenCrossContourXld(out cross1, LastR, LastC, 10, 0);
        //            HOperatorSet.SetColor(hwindow_final2.HWindowHalconID, "red");
        //            HOperatorSet.DispObj(cross1, hwindow_final2.HWindowHalconID);

        //        }


        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "FindMaxPt error" + ex.Message;
        //    }

        //}
        //[Obsolete]
        ///// <summary>
        ///// 由轮廓拟合点
        ///// </summary>
        ///// <param name="SideId">边序号 从1 开始</param>
        ///// <param name="LastR">输出行坐标</param>
        ///// <param name="LastC">输出列坐标</param>
        ///// <param name="hwnd">输入窗体（可选）</param>
        ///// <returns></returns>
        //public string FindMaxPt(int SideId, int ProfileId, out HTuple LastR, out HTuple LastC, out HTuple AnchorR, out HTuple AnchorC, HWindow_Final hwnd = null, bool ShowFeatures = false, bool UseFit = false)
        //{
        //    LastR = new HTuple(); LastC = new HTuple(); AnchorR = new HTuple(); AnchorC = new HTuple();
        //    try
        //    {
        //        HTuple row, col;
        //        ShowProfile(ProfileId, out row, out col, hwnd);

        //        int Id = SideId - 1;
        //        if (RArray == null || RArray[ProfileId].Length == 0)
        //        {
        //            return "RArray is NULL";
        //        }
        //        //HTuple row1 = -row;
        //        //HTuple col1 = col;

        //        //row = row1 - 0 + 150;
        //        //col = col1;


        //        //除去 -30 *200 的点
        //        HTuple eq30_1 = row.TupleEqualElem(-6000 - 0 + 150);
        //        HTuple eqId_1 = eq30_1.TupleFind(1);
        //        HTuple temp_1 = row.TupleRemove(eqId_1);
        //        HTuple temp_2 = col.TupleRemove(eqId_1);

        //        if (fpTool.roiList[Id].Count == 0)
        //        {
        //            return "fpTool.roiList is Null";
        //        }

        //        ////取最左 作为初步锚定点
        //        //HTuple maxZCol =  temp_2.TupleMin();

        //        //HTuple mZRowId = col.TupleFindFirst(maxZCol);
        //        //HTuple mZRow = row[mZRowId];
        //        HTuple mZRow = 0; HTuple maxZCol = 0;

        //        int roiID = -1;
        //        for (int i = 0; i < fpTool.fParam[Id].roiP.Count; i++)
        //        {

        //            for (int j = 0; j < fpTool.fParam[Id].roiP[i].NumOfSection; j++)
        //            {
        //                roiID++;
        //                if (roiID == ProfileId)
        //                {
        //                    break;
        //                }
        //            }
        //            if (roiID == ProfileId)
        //            {
        //                roiID = i;
        //                break;
        //            }
        //        }
        //        // 锚定 Roi
        //        HTuple RoiCoord = fpTool.roiList[Id][roiID].getModelData();
        //        double R1 = RoiCoord[0] + mZRow.D - fpTool.fParam[Id].roiP[roiID].AnchorRow;
        //        double C1 = RoiCoord[1] + maxZCol.D - fpTool.fParam[Id].roiP[roiID].AnchorCol;
        //        double R2 = RoiCoord[2] + mZRow.D - fpTool.fParam[Id].roiP[roiID].AnchorRow;
        //        double C2 = RoiCoord[3] + maxZCol.D - fpTool.fParam[Id].roiP[roiID].AnchorCol;

        //        if (C1 < 0)
        //        {
        //            C1 = 0;
        //        }

        //        //生成矩形框
        //        HObject left = new HObject();
        //        HObject right = new HObject();
        //        HOperatorSet.GenRegionLine(out left, 0, C1, 1000, C1);
        //        HOperatorSet.GenRegionLine(out right, 0, C2, 1000, C2);
        //        if (hwnd != null && ShowFeatures)
        //        {
        //            hwnd.viewWindow.displayHobject(left, "green");
        //            hwnd.viewWindow.displayHobject(right, "green");

        //        }

        //        //求矩形区域内轮廓 极值
        //        HTuple ColLess = temp_2.TupleLessElem(C2);
        //        HTuple ColGreater = temp_2.TupleGreaterElem(C1);
        //        HTuple sub = ColLess.TupleSub(ColGreater);
        //        HTuple IntersetID = sub.TupleFind(0);
        //        //HTuple IntersetID = eq0.TupleFind(1);
        //        if (IntersetID == -1)
        //        {
        //            return "区域内 无有效点";
        //        }
        //        HTuple RowNew = temp_1[IntersetID];
        //        HTuple ColNew = temp_2[IntersetID];

        //        int deg = fpTool.fParam[Id].roiP[roiID].AngleOfProfile;
        //        GetInflection(RowNew, ColNew, deg, out LastR, out LastC, hwnd, true, true, true, ShowFeatures);

        //        if (LastR.Length == 0)
        //        {
        //            return "Ignore";
        //        }
        //        HTuple Max = LastR.TupleMax();
        //        HTuple maxId = LastR.TupleFindFirst(Max);

        //        LastC = LastC[maxId];
        //        LastR = Max;
        //        AnchorR = LastR;
        //        AnchorC = LastC;

        //        ////启用中间点
        //        //if (fpTool.fParam[Id].roiP[roiID].useMidPt)
        //        //{
        //        //    HTuple EdgeCol = fpTool.fParam[Id].roiP[roiID].useLeft ? temp_2.TupleMin() : temp_2.TupleMax();
        //        //    HTuple EdgColId = temp_2.TupleFindFirst(EdgeCol);
        //        //    HTuple EdgegRow = temp_1[EdgColId];
        //        //    //取中间点
        //        //    HTuple midR = (LastR + EdgegRow) / 2;
        //        //    HTuple midC = (LastC + EdgeCol) / 2;
        //        //    LastR = midR;
        //        //    LastC = midC;
        //        //}


        //        if (hwnd != null && ShowFeatures)
        //        {
        //            HObject cross = new HObject();
        //            HOperatorSet.GenCrossContourXld(out cross, LastR, LastC, 10, 0);
        //            hwnd.viewWindow.displayHobject(cross, "green");

        //        }

        //        if (LastR.Length == 0)
        //        {
        //            return "Ignore";
        //        }
        //        HTuple PtID = new HTuple();
        //        HOperatorSet.TupleGreaterEqualElem(col, LastC, out PtID);
        //        PtID = PtID.TupleFindFirst(1);
        //        LastR = Row[ProfileId][PtID];
        //        LastC = CArray[ProfileId][PtID];

        //        //LastR = ProfileId;
        //        //if (MyGlobal.globalConfig.dataContext.xResolution != 0)
        //        //{
        //        //    LastC = LastC / 200 / MyGlobal.globalConfig.dataContext.xResolution;
        //        //}
        //        //else
        //        //{
        //        //    LastC = LastC / 200 / 0.007;
        //        //}

        //        if (fpTool.fParam[0].BeLeft)
        //        {
        //            // 作为锚定点
        //            //第一条
        //            int sX1 = fpTool.fParam[Id].roiP[roiID].StartOffSet1.X;
        //            int sY1 = fpTool.fParam[Id].roiP[roiID].StartOffSet1.Y;
        //            int eX1 = fpTool.fParam[Id].roiP[roiID].EndOffSet1.X;
        //            int eY1 = fpTool.fParam[Id].roiP[roiID].EndOffSet1.Y;
        //            //第二条
        //            int sX2 = fpTool.fParam[Id].roiP[roiID].StartOffSet2.X;
        //            int sY2 = fpTool.fParam[Id].roiP[roiID].StartOffSet2.Y;
        //            int eX2 = fpTool.fParam[Id].roiP[roiID].EndOffSet2.X;
        //            int eY2 = fpTool.fParam[Id].roiP[roiID].EndOffSet2.Y;
        //            //起点
        //            HTuple gX1 = sX1 < eX1 ? col.TupleGreaterEqualElem(AnchorC.D + sX1) : col.TupleGreaterEqualElem(AnchorC.D + eX1);
        //            HTuple gY1 = sY1 < eY1 ? row.TupleGreaterEqualElem(AnchorR.D + sY1) : row.TupleGreaterEqualElem(AnchorR.D + eY1);
        //            HTuple eqx1 = gX1.TupleFind(1);
        //            HTuple eqy1 = gY1.TupleFind(1);
        //            HTuple SID1 = eqx1.TupleIntersection(eqy1);
        //            //终点
        //            gX1 = sX1 < eX1 ? col.TupleLessEqualElem(AnchorC.D + eX1) : col.TupleLessEqualElem(AnchorC.D + sX1);
        //            gY1 = sY1 < eY1 ? row.TupleLessEqualElem(AnchorR.D + eY1) : row.TupleLessEqualElem(AnchorR.D + sY1);
        //            eqx1 = gX1.TupleFind(1);
        //            eqy1 = gY1.TupleFind(1);
        //            HTuple EID1 = eqx1.TupleIntersection(eqy1);
        //            HTuple FID1 = SID1.TupleIntersection(EID1);//第一条 选取点索引

        //            if (FID1.Length == 0)
        //            {

        //                return "OK";

        //            }
        //            //起点
        //            HTuple gX2 = sX2 < eX2 ? col.TupleGreaterEqualElem(AnchorC.D + sX2) : col.TupleGreaterEqualElem(AnchorC.D + eX2);
        //            HTuple gY2 = sY2 < eY2 ? row.TupleGreaterEqualElem(AnchorR.D + sY2) : row.TupleGreaterEqualElem(AnchorR.D + eY2);
        //            HTuple eqx2 = gX2.TupleFind(1);
        //            HTuple eqy2 = gY2.TupleFind(1);
        //            HTuple SID2 = eqx2.TupleIntersection(eqy2);
        //            //终点
        //            gX2 = sX2 < eX2 ? col.TupleLessEqualElem(AnchorC.D + eX2) : col.TupleLessEqualElem(AnchorC.D + sX2);
        //            gY2 = sY2 < eY2 ? row.TupleLessEqualElem(AnchorR.D + eY2) : row.TupleLessEqualElem(AnchorR.D + sY2);
        //            eqx2 = gX2.TupleFind(1);
        //            eqy2 = gY2.TupleFind(1);
        //            HTuple EID2 = eqx2.TupleIntersection(eqy2);
        //            HTuple FID2 = SID2.TupleIntersection(EID2);//第二条 选取点索引
        //            if (FID2.Length == 0)
        //            {
        //                return "OK";
        //            }

        //            HTuple intersectR, intersectC, isOverlapping;

        //            if (true)
        //            {
        //                HTuple Linr1 = row[FID1];
        //                HTuple Linc1 = col[FID1];
        //                HTuple Linr2 = row[FID2];
        //                HTuple Linc2 = col[FID2];

        //                HObject line1 = new HObject();
        //                HOperatorSet.GenContourPolygonXld(out line1, Linr1, Linc1);
        //                HTuple Rowbg1, Colbg1, RowEd1, ColEd1, Nr1, Nc1, Dist1;
        //                HOperatorSet.FitLineContourXld(line1, "tukey", -1, 0, 5, 2, out Rowbg1, out Colbg1, out RowEd1, out ColEd1, out Nr1, out Nc1, out Dist1);
        //                HOperatorSet.GenRegionLine(out line1, Rowbg1, Colbg1, RowEd1, ColEd1);

        //                HObject line2 = new HObject();
        //                HOperatorSet.GenContourPolygonXld(out line2, Linr2, Linc2);
        //                HTuple Rowbg2, Colbg2, RowEd2, ColEd2, Nr2, Nc2, Dist2;
        //                HOperatorSet.FitLineContourXld(line2, "tukey", -1, 0, 5, 2, out Rowbg2, out Colbg2, out RowEd2, out ColEd2, out Nr2, out Nc2, out Dist2);
        //                HOperatorSet.GenRegionLine(out line2, Rowbg2, Colbg2, RowEd2, ColEd2);


        //                HOperatorSet.IntersectionLines(Rowbg1, Colbg1, RowEd1, ColEd1, Rowbg2, Colbg2, RowEd2, ColEd2, out intersectR, out intersectC, out isOverlapping);


        //                HTuple PtID2 = new HTuple();
        //                HOperatorSet.TupleGreaterEqualElem(col, intersectC, out PtID2);
        //                PtID2 = PtID2.TupleFindFirst(1);
        //                LastR = Row[ProfileId][PtID2];
        //                LastC = CArray[ProfileId][PtID2];
        //                if (hwnd != null)
        //                {
        //                    HObject cross2 = new HObject();
        //                    HOperatorSet.GenCrossContourXld(out cross2, intersectR, intersectC, 10, 45);
        //                    HOperatorSet.SetColor(hwindow_final1.HWindowHalconID, "blue");
        //                    HOperatorSet.DispObj(cross2, hwindow_final1.HWindowHalconID);
        //                    HOperatorSet.DispObj(line1, hwindow_final1.HWindowHalconID);
        //                    HOperatorSet.DispObj(line2, hwindow_final1.HWindowHalconID);
        //                }
        //            }
        //        }


        //        if (hwnd != null)
        //        {
        //            HObject cross1 = new HObject();
        //            HOperatorSet.GenCrossContourXld(out cross1, LastR, LastC, 10, 0);
        //            HOperatorSet.SetColor(hwindow_final2.HWindowHalconID, "green");
        //            HOperatorSet.DispObj(cross1, hwindow_final2.HWindowHalconID);

        //        }


        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "FindMaxPt error" + ex.Message;
        //    }

        //}
        //[Obsolete]
        //public string FindEdge(int SideId, int ProfileId, out HTuple EdgeR, out HTuple EdgeC, HWindow_Final hwnd = null, bool ShowFeatures = false)
        //{
        //    EdgeR = new HTuple(); EdgeC = new HTuple();
        //    try
        //    {
        //        HTuple row, col;
        //        ShowProfile(ProfileId, out row, out col, hwnd);

        //        int Id = SideId - 1;
        //        if (RArray == null || RArray[ProfileId].Length == 0)
        //        {
        //            return "RArray is NULL";
        //        }

        //        if (fpTool.roiList[Id].Count == 0)
        //        {
        //            return "FindEdge error fpTool.roiList is Null";
        //        }
        //        int roiID = -1;
        //        for (int i = 0; i < fpTool.fParam[Id].roiP.Count; i++)
        //        {

        //            for (int j = 0; j < fpTool.fParam[Id].roiP[i].NumOfSection; j++)
        //            {
        //                roiID++;
        //                if (roiID == ProfileId)
        //                {
        //                    break;
        //                }
        //            }
        //            if (roiID == ProfileId)
        //            {
        //                roiID = i;
        //                break;
        //            }
        //        }

        //        EdgeC = fpTool.fParam[Id].roiP[roiID].useLeft ? col.TupleMin() : col.TupleMax();
        //        HTuple mZRowId = col.TupleFindFirst(EdgeC);
        //        EdgeR = row[mZRowId];

        //        if (hwnd != null && ShowFeatures)
        //        {
        //            HObject cross = new HObject();
        //            HOperatorSet.GenCrossContourXld(out cross, EdgeR, EdgeC, 10, 0);
        //            hwnd.viewWindow.displayHobject(cross, "green");

        //        }

        //        HTuple PtID = new HTuple();
        //        HOperatorSet.TupleGreaterEqualElem(col, EdgeC, out PtID);
        //        PtID = PtID.TupleFindFirst(1);
        //        EdgeR = Row[ProfileId][PtID];
        //        EdgeC = CArray[ProfileId][PtID];

        //        if (hwnd != null)
        //        {
        //            HObject cross1 = new HObject();
        //            HOperatorSet.GenCrossContourXld(out cross1, EdgeR, EdgeC, 10, 0);
        //            HOperatorSet.SetColor(hwnd.HWindowHalconID, "green");
        //            HOperatorSet.DispObj(cross1, hwnd.HWindowHalconID);
        //        }

        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "FindEdge error" + ex.Message;
        //    }

        //}

        //[Obsolete]
        ///// <summary>
        ///// 获取极大值或极小值
        ///// </summary>
        ///// <param name="rows">输入行坐标</param>
        ///// <param name="cols">输入列坐标</param>
        ///// <param name="sigma">输入平滑系数</param>
        ///// <param name="IsUpDown">以上下方向筛选还是左右方向筛选</param>
        ///// <param name="IsMaxOrMin">选择极大值还是极小值</param>
        ///// <param name="hv_rowout">输出极值行坐标</param>
        ///// <param name="hv_colout">输出极值列坐标</param>
        ///// <param name="UseLeft"> 是否选择最左侧点</param>
        ///// <returns></returns>
        //public string PeakTroughOfWave(HTuple rows, HTuple cols, HTuple sigma, bool IsUpDown, bool IsMaxOrMin, out HTuple hv_rowout, out HTuple hv_colout, bool UseLeft = false)
        //{
        //    try
        //    {
        //        hv_rowout = new HTuple(); hv_colout = new HTuple();
        //        HTuple hv_Function = null, hv_SmoothedFunction = null, hv_Derivative = null, hv_ZeroCrossings = null, hv_Y = null, hv_Min = null, hv_Max = null;
        //        HTuple hv_Indices1 = null, hv_Indices2 = null;

        //        if (rows.Type == HTupleType.EMPTY || rows.Length < 5)
        //        {
        //            hv_rowout = new HTuple();
        //            hv_colout = new HTuple();
        //            return "NG";
        //        }
        //        HOperatorSet.CreateFunct1dArray(rows, out hv_Function);
        //        if (sigma > ((rows.Length - 2) / 7.8))
        //        {
        //            sigma = 0.5;
        //        }

        //        HOperatorSet.SmoothFunct1dGauss(hv_Function, sigma, out hv_SmoothedFunction);
        //        HOperatorSet.DerivateFunct1d(hv_SmoothedFunction, "first", out hv_Derivative);
        //        HOperatorSet.ZeroCrossingsFunct1d(hv_Derivative, out hv_ZeroCrossings);
        //        HTuple hv_indCol = ((hv_ZeroCrossings.TupleString(".0f"))).TupleNumber();

        //        if (hv_indCol.Length == 0)
        //        {
        //            //if (UseLeft)
        //            //{
        //            //    hv_colout = cols.TupleMin();
        //            //    HTuple minId = cols.TupleFind(hv_colout);
        //            //    hv_rowout = rows[minId];
        //            //}
        //            //else
        //            //{
        //            hv_colout = new HTuple();
        //            hv_rowout = new HTuple();
        //            //}                   

        //            return "OK";
        //        }
        //        HOperatorSet.GetYValueFunct1d(hv_SmoothedFunction, hv_ZeroCrossings, "constant",
        //            out hv_Y);
        //        HTuple max = new HTuple(); HTuple min = new HTuple();
        //        HTuple hv_indColMax = new HTuple(); HTuple hv_indColMin = new HTuple();
        //        //轮廓最左点
        //        HTuple colLeft = cols.TupleMin();

        //        if (true)
        //        {
        //            for (int i = 0; i < hv_Y.Length; i++)
        //            {
        //                if (hv_indCol[i].I + 3 + 5 >= hv_SmoothedFunction.Length)
        //                {
        //                    break;
        //                }
        //                if (hv_indCol[i].I < 2)
        //                {
        //                    continue;
        //                }
        //                if (hv_SmoothedFunction[hv_indCol[i].I + 3] < hv_SmoothedFunction[hv_indCol[i].I + 3 - 5] && hv_SmoothedFunction[hv_indCol[i].I + 3] < hv_SmoothedFunction[hv_indCol[i].I + 3 + 5])
        //                {

        //                    max = max.TupleConcat(hv_Y[i]);
        //                    hv_indColMax = hv_indColMax.TupleConcat(hv_indCol[i]);

        //                }
        //                if (hv_SmoothedFunction[hv_indCol[i].I + 3] > hv_SmoothedFunction[hv_indCol[i].I + 3 - 5] && hv_SmoothedFunction[hv_indCol[i].I + 3] > hv_SmoothedFunction[hv_indCol[i].I + 3 + 5])
        //                {
        //                    if (cols[hv_indCol[i].I] > colLeft) //不取最左侧点
        //                    {
        //                        min = min.TupleConcat(hv_Y[i]);
        //                        hv_indColMin = hv_indColMin.TupleConcat(hv_indCol[i]);
        //                    }
        //                    else
        //                    {
        //                        if (UseLeft)
        //                        {
        //                            min = min.TupleConcat(hv_Y[i]);
        //                            hv_indColMin = hv_indColMin.TupleConcat(hv_indCol[i]);
        //                        }
        //                    }
        //                }
        //            }

        //            if (IsMaxOrMin)//极大值
        //            {
        //                if (max.Length == 0)
        //                {

        //                    return "PeakTroughOfWave：" + "No MaxValue";
        //                }
        //                hv_Y = max;
        //                hv_indCol = hv_indColMax;
        //            }
        //            else
        //            {
        //                if (min.Length == 0)
        //                {
        //                    return "PeakTroughOfWave：" + "No MinValue";
        //                }
        //                hv_Y = min;
        //                hv_indCol = hv_indColMin;
        //            }
        //        }


        //        if (IsUpDown)//上下方向 Row 方向最大和最小值
        //        {
        //            HOperatorSet.TupleMin(hv_Y, out hv_Min);
        //            HOperatorSet.TupleMax(hv_Y, out hv_Max);
        //            HOperatorSet.TupleFindFirst(hv_Y, hv_Min, out hv_Indices1);
        //            HOperatorSet.TupleFindFirst(hv_Y, hv_Max, out hv_Indices2);

        //            hv_rowout = new HTuple();
        //            hv_rowout = hv_rowout.TupleConcat(hv_Y.TupleSelect(
        //                hv_Indices1));
        //            hv_rowout = hv_rowout.TupleConcat(hv_Y.TupleSelect(hv_Indices2));
        //            hv_colout = new HTuple();
        //            hv_colout = hv_colout.TupleConcat(cols.TupleSelect(
        //                hv_indCol.TupleSelect(hv_Indices1)));
        //            hv_colout = hv_colout.TupleConcat(cols.TupleSelect(
        //                hv_indCol.TupleSelect(hv_Indices2)));

        //        }
        //        else//左右方向 Col 方向 最大和最小值
        //        {
        //            HTuple Col = cols[hv_indCol];
        //            HOperatorSet.TupleMin(Col, out hv_Min);//最左侧
        //            HOperatorSet.TupleMax(Col, out hv_Max);//最右侧
        //            HOperatorSet.TupleFindFirst(Col, hv_Min, out hv_Indices1);
        //            HOperatorSet.TupleFindFirst(Col, hv_Max, out hv_Indices2);


        //            hv_rowout = new HTuple();
        //            hv_rowout = hv_rowout.TupleConcat(hv_Y.TupleSelect(
        //                hv_Indices1));
        //            hv_rowout = hv_rowout.TupleConcat(hv_Y.TupleSelect(hv_Indices2));
        //            hv_colout = new HTuple();
        //            hv_colout = hv_colout.TupleConcat(hv_Min);
        //            hv_colout = hv_colout.TupleConcat(hv_Max);
        //        }


        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        hv_colout = new HTuple(); hv_rowout = new HTuple();
        //        return "PeakTroughOfWave：" + ex.Message;
        //    }
        //}
        //[Obsolete]
        ///// <summary>
        ///// 获取拐点
        ///// </summary>
        ///// <param name="clipRow">轮廓行坐标</param>
        ///// <param name="clipCol">轮廓列坐标</param>
        ///// <param name="Deg">轮廓旋转角度</param>
        ///// <param name="pRow">拐点行坐标</param>
        ///// <param name="pCol">拐点列坐标</param>
        ///// <param name="Hwindow">显示窗口</param>
        ///// <param name="isUpDown">以上下方向筛选还是以左右方向筛选</param>
        ///// <param name="isMax">是否启用极大值(true 极大值，false 极小值）</param>
        ///// <returns></returns>
        //private string GetInflection(HTuple clipRow, HTuple clipCol, HTuple Deg, out HTuple pRow, out HTuple pCol, HWindow_Final Hwindow = null, bool isUpDown = true, bool isMax = true, bool useLeft = true, bool ifShowFeatures = false)
        //{
        //    pRow = new HTuple(); pCol = new HTuple();
        //    try
        //    {
        //        if (clipRow.Length == 0)
        //        {
        //            return "No Clip";
        //        }
        //        if (!ifShowFeatures)
        //        {
        //            Hwindow = null;
        //        }
        //        HObject IntersectionO = new HObject(); HObject crossO = new HObject();
        //        HObject ContourO = new HObject();
        //        HTuple RowO, ColO, rowPeak, colPeak;
        //        HTuple area, row, col, homMat2D;
        //        HTuple Phi = Deg.TupleRad();

        //        HOperatorSet.GenRegionPoints(out IntersectionO, clipRow, clipCol);

        //        HOperatorSet.GenContourPolygonXld(out ContourO, clipRow, clipCol);


        //        HOperatorSet.AreaCenter(IntersectionO, out area, out row, out col);
        //        HOperatorSet.VectorAngleToRigid(row, col, 0, row, col, Phi, out homMat2D);
        //        HOperatorSet.AffineTransContourXld(ContourO, out ContourO, homMat2D);
        //        HOperatorSet.GetContourXld(ContourO, out RowO, out ColO);
        //        if (Hwindow != null)
        //        {
        //            HOperatorSet.SetDraw(Hwindow.HWindowHalconID, "margin");
        //            Hwindow.viewWindow.displayHobject(ContourO, "white", true);
        //            HOperatorSet.SetDraw(Hwindow.HWindowHalconID, "margin");
        //        }


        //        string ware = PeakTroughOfWave(RowO, ColO, 0.5, isUpDown, isMax, out rowPeak, out colPeak, useLeft);
        //        string msg = "";
        //        //if (ware=="NG")
        //        //{
        //        //    msg = "轮廓点异常";
        //        //    return msg;
        //        //}
        //        if (rowPeak.Length == 0)
        //        {
        //            pRow = new HTuple(); pCol = new HTuple();
        //            return "OK";
        //            msg = "OK";
        //            if (true)
        //            {
        //                colPeak = ColO.TupleMin();
        //                HTuple ind = ColO.TupleFindFirst(colPeak);
        //                rowPeak = RowO[ind];
        //                rowPeak = rowPeak.TupleConcat(rowPeak);
        //                colPeak = colPeak.TupleConcat(colPeak);

        //            }
        //            //else
        //            //{
        //            //    colPeak = ColO.TupleMax();
        //            //    HTuple ind = ColO.TupleFindFirst(colPeak);
        //            //    rowPeak = RowO[ind];
        //            //    rowPeak = rowPeak.TupleConcat(rowPeak);
        //            //    colPeak = colPeak.TupleConcat(colPeak);
        //            //}

        //        }
        //        else
        //        {
        //            msg = "OK";
        //        }

        //        HTuple Max = rowPeak.TupleMin();
        //        HTuple maxId = rowPeak.TupleFindFirst(Max);

        //        colPeak = colPeak[maxId];
        //        rowPeak = Max;

        //        HOperatorSet.VectorAngleToRigid(row, col, Phi, row, col, 0, out homMat2D);
        //        HOperatorSet.AffineTransPoint2d(homMat2D, rowPeak, colPeak, out pRow, out pCol);

        //        if (Hwindow != null)
        //        {
        //            HOperatorSet.GenCrossContourXld(out crossO, pRow, pCol, 12, 0);
        //            HOperatorSet.SetDraw(Hwindow.HWindowHalconID, "margin");

        //            Hwindow.viewWindow.displayHobject(crossO, "red", true);
        //            Hwindow.viewWindow.displayHobject(IntersectionO, "yellow", true);
        //            HOperatorSet.SetDraw(Hwindow.HWindowHalconID, "margin");
        //        }


        //        return msg;
        //    }
        //    catch (Exception ex)
        //    {

        //        return "GetInflection error " + ex.Message;
        //    }
        //}

        //[Obsolete]
        //private void DispSection___1(ROIRectangle2 roi, int SideID, int RoiId, out HTuple[] LineCoord, HWindow_Final hwnd = null)
        //{
        //    LineCoord = new HTuple[1];
        //    try
        //    {
        //        HTuple RoiCoord = roi.getModelData();

        //        double cosa = Math.Cos(RoiCoord[2].D);
        //        double sina = Math.Sin(RoiCoord[2].D);
        //        double r1 = RoiCoord[0].D + RoiCoord[4].D * cosa;
        //        double c1 = RoiCoord[1].D - RoiCoord[4].D * sina;
        //        double r2 = RoiCoord[0].D - RoiCoord[4].D * cosa;
        //        double c2 = RoiCoord[1].D + RoiCoord[4].D * sina;

        //        //

        //        int Num = fpTool.fParam[SideID].roiP[RoiId].NumOfSection;
        //        LineCoord = new HTuple[Num];
        //        double Average = RoiCoord[4].D * 2 / (Num + 1);
        //        for (int i = 0; i < Num; i++)
        //        {
        //            LineCoord[i] = new HTuple();
        //            double AverageLen = Average * (i + 1);
        //            double Row2 = r2 + AverageLen * cosa + RoiCoord[3].D * sina;
        //            double Col2 = c2 - AverageLen * sina + RoiCoord[3].D * cosa;

        //            double Row1 = r2 + AverageLen * cosa - RoiCoord[3].D * sina;
        //            double Col1 = c2 - AverageLen * sina - RoiCoord[3].D * cosa;

        //            LineCoord[i] = LineCoord[i].TupleConcat(Row1).TupleConcat(Col1).TupleConcat(Row2).TupleConcat(Col2).TupleConcat(RoiCoord[2].D);
        //            if (hwnd != null)
        //            {
        //                HObject Line = new HObject();
        //                HOperatorSet.GenRegionLine(out Line, LineCoord[i][0], LineCoord[i][1], LineCoord[i][2], LineCoord[i][3]);
        //                HOperatorSet.SetColor(hwindow_final2.HWindowHalconID, "red");
        //                HOperatorSet.DispObj(Line, hwindow_final2.HWindowHalconID);
        //            }

        //        }


        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //[Obsolete]
        //string GenSection(HObject Image, HTuple lineCoord, out HTuple row, out HTuple col, out HTuple IgnorePt)
        //{
        //    row = new HTuple(); col = new HTuple(); IgnorePt = 0;
        //    try
        //    {
        //        HTuple width, height, lessId1, lessId2;
        //        HOperatorSet.GetImageSize(Image, out width, out height);

        //        HObject Rline; HObject Rline1 = new HObject();
        //        HOperatorSet.GenRegionLine(out Rline, lineCoord[0], lineCoord[1], lineCoord[2], lineCoord[3]);
        //        HOperatorSet.GenContourRegionXld(Rline, out Rline1, "center");
        //        HOperatorSet.GetContourXld(Rline1, out row, out col);

        //        if (row.Length == 0)
        //        {
        //            IgnorePt = 1;
        //            return "GenSection截面在图像之外";
        //        }

        //        HTuple deg = -(new HTuple(lineCoord[4].D)).TupleDeg();
        //        double tan = Math.Abs(Math.Tan(-lineCoord[4].D));

        //        int len1 = Math.Abs((int)(lineCoord[1].D - lineCoord[3].D));
        //        int len2 = Math.Abs((int)(lineCoord[0].D - lineCoord[2].D));
        //        int len = len1 > len2 ? len1 : len2;
        //        HTuple x0 = lineCoord[1].D;
        //        HTuple y0 = lineCoord[0].D;
        //        HTuple newr = y0; HTuple newc = x0;
        //        for (int i = 1; i < len; i++)
        //        {
        //            HTuple row1 = new HTuple(); HTuple col1 = new HTuple();
        //            if (len1 > len2)
        //            {
        //                row1 = lineCoord[0].D > lineCoord[2].D ? y0 - i * tan : y0 + i * tan;
        //                col1 = lineCoord[1].D > lineCoord[3].D ? x0 - i : x0 + i;
        //            }
        //            else
        //            {
        //                row1 = lineCoord[0].D < lineCoord[2].D ? y0 + i : y0 - i;
        //                col1 = lineCoord[1].D < lineCoord[3].D ? x0 + i / tan : x0 - i / tan;
        //            }

        //            newr = newr.TupleConcat(row1);
        //            newc = newc.TupleConcat(col1);
        //        }
        //        row = newr;
        //        col = newc;





        //        HOperatorSet.TupleLessElem(row, height, out lessId1);
        //        HOperatorSet.TupleFind(lessId1, 1, out lessId1);
        //        if (lessId1.D == -1)
        //        {
        //            IgnorePt = 1;
        //            row = 0;
        //            col = 0;
        //            return "GenSection截面在图像之外";
        //        }

        //        row = row[lessId1];
        //        col = col[lessId1];
        //        HOperatorSet.TupleLessElem(col, width, out lessId2);
        //        HOperatorSet.TupleFind(lessId2, 1, out lessId2);
        //        if (lessId2.D == -1)
        //        {
        //            IgnorePt = 1;
        //            row = 0;
        //            col = 0;
        //            return "GenSection截面在图像之外";
        //        }

        //        row = row[lessId2];
        //        col = col[lessId2];

        //        //且 行列大于零
        //        HTuple lessId3, lessId4;
        //        HOperatorSet.TupleGreaterElem(col, 0, out lessId3);
        //        HOperatorSet.TupleFind(lessId3, 1, out lessId3);
        //        row = row[lessId3];
        //        col = col[lessId3];

        //        HOperatorSet.TupleGreaterElem(row, 0, out lessId4);
        //        HOperatorSet.TupleFind(lessId4, 1, out lessId4);
        //        row = row[lessId4];
        //        col = col[lessId4];

        //        //HOperatorSet.GetGrayval(Image, row, col, out Zpoint);
        //        //HTuple EqId = new HTuple();
        //        //HOperatorSet.TupleNotEqualElem(Zpoint, -30, out EqId);
        //        //HOperatorSet.TupleFind(EqId, 1, out EqId);
        //        //if (EqId.D != -1)
        //        //{
        //        //    Zpoint = Zpoint[EqId];
        //        //    Cpoint = col[EqId];
        //        //    Rpoint = row[EqId];
        //        //    IgnorePt = 0;

        //        //}
        //        //else
        //        //{
        //        //    IgnorePt = 1;
        //        //}


        //        Rline.Dispose();
        //        Rline1.Dispose();
        //        //ConstImage.Dispose();

        //        //Contour.Dispose();

        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "GenSection error" + ex.Message;
        //    }
        //}
        //[Obsolete]
        //private string GenProfileCoord(int SideId, HObject HeightImage, out double[][] Rarray, out double[][] Row, out double[][] Carray, out double[][] Phi, out int ignorePt, HTuple Fix = null)
        //{
        //    Rarray = null; Carray = null; Row = null; Phi = null; ignorePt = 0;
        //    try
        //    {
        //        int SId = SideId - 1;
        //        int k = 0;
        //        //
        //        for (int i = 0; i < fpTool.roiList2[SId].Count; i++)
        //        {
        //            for (int j = 0; j < fpTool.fParam[SId].roiP[i].NumOfSection; j++)
        //            {
        //                k++;
        //            }
        //        }
        //        int n = 0;
        //        Rarray = new double[k][]; Carray = new double[k][]; Row = new double[k][]; Phi = new double[k][];
        //        double[] Rarray1;
        //        double[] Carray1;

        //        HTuple HPhi = new HTuple();

        //        int[] profileNum = new int[k];
        //        for (int i = 0; i < fpTool.roiList2[SId].Count; i++)
        //        {
        //            //Debug.WriteLine(n);
        //            HTuple orignal = fpTool.roiList2[SId][i].getModelData();
        //            ROIRectangle2 temp = new ROIRectangle2(orignal[0], orignal[1], orignal[2], orignal[3], orignal[4]);
        //            HTuple[] lineCoord = new HTuple[1];
        //            //将矩形进行定位
        //            if (Fix != null)
        //            {
        //                HTuple recCoord = temp.getModelData();
        //                HTuple CenterR = new HTuple(); HTuple CenterC = new HTuple();
        //                List<ROI> temproi = new List<ROI>();
        //                HTuple tempR = new HTuple(); HTuple tempC = new HTuple();
        //                HOperatorSet.AffineTransPoint2d(Fix, recCoord[0], recCoord[1], out CenterR, out CenterC);
        //                temp.Row = CenterR; temp.Column = CenterC;
        //            }

        //            fpTool.DispSection(temp, SId, i, out lineCoord);
        //            for (int j = 0; j < lineCoord.Length; j++)
        //            {
        //                /* HTuple Sigle = new HTuple();*/
        //                HTuple col = new HTuple(); HTuple row = new HTuple(); HTuple ignore = new HTuple();
        //                string ok = GenSection(HeightImage, lineCoord[j], out row, out col, out ignore);
        //                ignorePt += ignore;
        //                profileNum[n] = row.Length;
        //                Rarray[n] = row;
        //                Carray[n] = col;
        //                //HRarray = HRarray.TupleConcat(row);
        //                //HCarray = HCarray.TupleConcat(col);
        //                //HRow = HRow.TupleConcat(row);

        //                Phi[n] = lineCoord[j];
        //                n++;
        //                //if (n == 358)
        //                //{
        //                //    Debug.WriteLine("error");
        //                //}
        //            }
        //        }
        //        int total = 0;
        //        for (int i = 0; i < n; i++)
        //        {
        //            for (int j = 0; j < profileNum[i]; j++)
        //            {
        //                //if (Rarray[i][j] != 0 && Carray[i][j] != 0)
        //                //{
        //                total++;
        //                //}

        //            }
        //        }
        //        Rarray1 = new double[total];
        //        Carray1 = new double[total];

        //        int tt = 0;
        //        for (int i = 0; i < n; i++)
        //        {
        //            for (int j = 0; j < profileNum[i]; j++)
        //            {
        //                Rarray1[tt] = Rarray[i][j];
        //                Carray1[tt] = Carray[i][j];
        //                tt++;

        //            }
        //        }

        //        HTuple Zpoint = new HTuple(); HTuple HRow = new HTuple(); HTuple HRarray = new HTuple(); HTuple HCarray = new HTuple();
        //        try
        //        {
        //            HOperatorSet.GetGrayval(HeightImage, Rarray1, Carray1, out Zpoint);
        //        }
        //        catch (Exception)
        //        {

        //            return "GenProfileCoord error: 区域位于图像之外";
        //        }

        //        HRarray = Rarray1; HCarray = Carray1;
        //        int m = 0;

        //        for (int i = 0; i < profileNum.Length; i++)
        //        {
        //            if (profileNum[i] == 0)
        //            {
        //                continue;
        //            }
        //            HTuple z = Zpoint.TupleSelectRange(m, m + profileNum[i] - 1);
        //            HTuple r = HRarray.TupleSelectRange(m, m + profileNum[i] - 1);
        //            HTuple c = HCarray.TupleSelectRange(m, m + profileNum[i] - 1);


        //            HTuple EqId = new HTuple();
        //            HOperatorSet.TupleGreaterElem(z, -10, out EqId);
        //            HOperatorSet.TupleFind(EqId, 1, out EqId);
        //            if (EqId.D != -1)
        //            {
        //                z = z[EqId];
        //                r = r[EqId];
        //                c = c[EqId];
        //            }
        //            else
        //            {
        //                ignorePt += 1;

        //            }

        //            Rarray[i] = z * 200;
        //            Carray[i] = c;
        //            Row[i] = r;
        //            //Phi[m] = HPhi[j];
        //            m += profileNum[i];

        //        }

        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "GenProfileCoord error:" + ex.Message;
        //    }
        //}
        //[Obsolete]
        //public string FindIntersectPoint(int Side, HObject HeightImage, out IntersetionCoord intersectCoord, HWindow_Final hwnd = null, bool debug = false)
        //{
        //    List<HTuple> Hlines = new List<HTuple>(); intersectCoord = new IntersetionCoord();
        //    try
        //    {
        //        string ok = FindPoint(Side, HeightImage, out Hlines, hwnd, debug);
        //        if (ok != "OK")
        //        {
        //            return ok;
        //        }
        //        if (Hlines.Count != 4)
        //        {
        //            return "定位找线区域数量错误";
        //        }
        //        for (int i = 0; i < Hlines.Count; i++)
        //        {
        //            if (Hlines[i].Length == 0)
        //            {
        //                return $"定位找线区域{i + 1}运行失败";
        //            }
        //        }

        //        //1,4 取中线  2，3取拟合线 
        //        HTuple rb1 = (Hlines[0][0].D + Hlines[3][0].D) / 2;
        //        HTuple cb1 = (Hlines[0][1].D + Hlines[3][1].D) / 2;
        //        HTuple re1 = (Hlines[0][2].D + Hlines[3][2].D) / 2;
        //        HTuple ce1 = (Hlines[0][3].D + Hlines[3][3].D) / 2;

        //        //取1 4 中点
        //        HTuple midR = (rb1.D + re1.D) / 2;
        //        HTuple midC = (cb1.D + ce1.D) / 2;
        //        HObject crossMid = new HObject();
        //        HOperatorSet.GenCrossContourXld(out crossMid, midR, midC, 30, 0.5);
        //        rb1 = midR.D;
        //        re1 = midR.D;
        //        cb1 = 0;
        //        ce1 = 5000;

        //        HObject contHorizon = new HObject();
        //        HOperatorSet.GenContourPolygonXld(out contHorizon, rb1.TupleConcat(re1), cb1.TupleConcat(ce1));

        //        HTuple rb2 = (Hlines[1][0].D + Hlines[2][0].D) / 2;
        //        HTuple cb2 = (Hlines[1][1].D + Hlines[2][1].D) / 2;
        //        HTuple re2 = (Hlines[1][2].D + Hlines[2][2].D) / 2;
        //        HTuple ce2 = (Hlines[1][3].D + Hlines[2][3].D) / 2;
        //        HTuple rowVer = new HTuple(); HTuple colVer = new HTuple();
        //        rowVer = rowVer.TupleConcat(Hlines[1][0].D).TupleConcat(Hlines[1][2].D).TupleConcat(Hlines[2][0].D).TupleConcat(Hlines[2][2].D);
        //        colVer = colVer.TupleConcat(Hlines[1][1].D).TupleConcat(Hlines[1][3].D).TupleConcat(Hlines[2][1].D).TupleConcat(Hlines[2][3].D);

        //        HObject contVer = new HObject();
        //        HOperatorSet.GenContourPolygonXld(out contVer, rowVer, colVer);

        //        //拟合线
        //        //HObject line = new HObject();
        //        HTuple Nr, Nc, Dist;
        //        HOperatorSet.FitLineContourXld(contVer, "tukey", -1, 0, 5, 2, out rb2, out cb2, out re2, out ce2, out Nr, out Nc, out Dist);
        //        HOperatorSet.GenContourPolygonXld(out contVer, rb2.TupleConcat(re2), cb2.TupleConcat(ce2));

        //        //HOperatorSet.GenContourPolygonXld(out contVer, rb2.TupleConcat(re2), cb2.TupleConcat(ce2));
        //        HTuple Row, Col, Angle;
        //        HOperatorSet.AngleLx(rb2, cb2, re2, ce2, out Angle);
        //        HTuple isOver;
        //        //HOperatorSet.IntersectionContoursXld(contHorizon, contVer, "mutual", out Row, out Col, out isOver);
        //        HOperatorSet.IntersectionLines(rb1, cb1, re1, ce1, rb2, cb2, re2, ce2, out Row, out Col, out isOver);
        //        if (hwnd != null)
        //        {
        //            //hwnd.viewWindow.displayHobject(contHorizon, "red");
        //            hwnd.viewWindow.displayHobject(crossMid, "red");
        //            HObject Cross = new HObject();
        //            HOperatorSet.GenCrossContourXld(out Cross, Row, Col, 30, 0.5);
        //            hwnd.viewWindow.displayHobject(contVer, "blue");
        //            hwnd.viewWindow.displayHobject(Cross, "red");


        //            double xResolution = MyGlobal.globalConfig.dataContext.xResolution;
        //            double yResolution = MyGlobal.globalConfig.dataContext.yResolution;
        //            HTuple row1 = Row.D * xResolution;
        //            HTuple col1 = Col.D * yResolution;

        //            string Rowstr = (Math.Round(row1.D, 3)).ToString();
        //            string Colstr = (Math.Round(col1.D, 3)).ToString();
        //            string Anglestr = (Math.Round(Angle.D, 3)).ToString();

        //            hwnd.viewWindow.dispMessage(Rowstr, "red", Row, Col + 100);
        //            hwnd.viewWindow.dispMessage(Colstr, "red", Row.D + 50, Col + 100);
        //            hwnd.viewWindow.dispMessage(Anglestr, "red", Row.D + 100, Col + 100);

        //        }
        //        intersectCoord.Row = Row.D;
        //        intersectCoord.Col = Col.D;
        //        intersectCoord.Angle = Angle.D;
        //        if (ok != "OK")
        //        {
        //            MessageBox.Show(ok);
        //        }


        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "FindIntersectPoint error" + ex.Message;
        //    }
        //}
        //[Obsolete]
        //private void RotateImage(int SideId, HObject IntensityImage, HObject HeightImage, out HObject RIntesity, out HObject RHeight, out HTuple homMatRotate, out HTuple homMatRotateInvert, HWindow_Final hwind = null)
        //{
        //    RIntesity = new HObject(); RHeight = new HObject();

        //    //将图像转正‘
        //    int Sid = SideId - 1;
        //    HObject ReduceImage = new HObject(); HObject Rec = new HObject();
        //    //HTuple Coor = fpTool.roiList3[Sid][0].getModelData();
        //    HTuple Coor = new HTuple();
        //    HOperatorSet.GenRectangle1(out Rec, Coor[0], Coor[1], Coor[2], Coor[3]);
        //    HOperatorSet.ReduceDomain(IntensityImage, Rec, out ReduceImage);
        //    HOperatorSet.Threshold(ReduceImage, out Rec, 5, 255);
        //    HOperatorSet.ErosionRectangle1(Rec, out Rec, 5, 5);
        //    HOperatorSet.DilationRectangle1(Rec, out Rec, 5, 5);
        //    HOperatorSet.Connection(Rec, out Rec);
        //    HTuple area = new HTuple();
        //    HOperatorSet.RegionFeatures(Rec, "area", out area);
        //    HTuple maxId = new HTuple();
        //    HOperatorSet.TupleMax(area, out maxId);
        //    HOperatorSet.TupleFind(area, maxId, out maxId);
        //    if (hwind != null)
        //    {
        //        //hwind.viewWindow.displayHobject(Rec);
        //    }

        //    HOperatorSet.SelectObj(Rec, out Rec, maxId + 1);
        //    HTuple phi = new HTuple(); HTuple rad90 = new HTuple();
        //    HOperatorSet.RegionFeatures(Rec, "phi", out phi);
        //    HOperatorSet.TupleRad(90, out rad90);
        //    HTuple sub = new HTuple();
        //    if (phi < 0)
        //    {
        //        phi = phi.TupleAbs();
        //        sub = phi - rad90;
        //    }
        //    else
        //    {
        //        sub = rad90 - phi;
        //    }


        //    homMatRotate = new HTuple(); homMatRotateInvert = new HTuple();
        //    HOperatorSet.HomMat2dIdentity(out homMatRotate);
        //    HOperatorSet.HomMat2dRotate(homMatRotate, sub, 0, 0, out homMatRotate);
        //    HOperatorSet.HomMat2dIdentity(out homMatRotateInvert);
        //    HOperatorSet.HomMat2dRotate(homMatRotateInvert, -sub, 0, 0, out homMatRotateInvert);
        //    HOperatorSet.AffineTransImage(IntensityImage, out RIntesity, homMatRotate, "nearest_neighbor", "false");
        //    HOperatorSet.AffineTransImage(HeightImage, out RHeight, homMatRotate, "nearest_neighbor", "false");
        //    if (hwind != null)
        //    {
        //        //hwind.ClearWindow();
        //        //hwind.HobjectToHimage(RIntesity);
        //    }
        //}
        //[Obsolete]
        //public string FindPoint(int SideId, HObject IntesityImage, HObject HeightImage, out double[][] RowCoord, out double[][] ColCoord, out double[][] ZCoord, out string[][] StrLineOrCircle, out HTuple[] originalPoint, HTuple HomMat3D = null, HWindow_Final hwind = null, bool debug = false, HTuple homMatFix = null)
        //{
        //    //HObject RIntesity = new HObject(), RHeight = new HObject();
        //    StringBuilder Str = new StringBuilder();
        //    originalPoint = new HTuple[2];
        //    RowCoord = null; ColCoord = null; ZCoord = null; StrLineOrCircle = null;
        //    try
        //    {
        //        int Sid = SideId - 1;
        //        string ok1 = GenProfileCoord(Sid + 1, HeightImage, out RArray, out Row, out CArray, out Phi, out Ignore, homMatFix);
        //        if (ok1 != "OK")
        //        {
        //            return ok1;
        //        }
        //        int Len = fpTool.roiList2[Sid].Count;// 区域数量
        //        if (Len == 0)
        //        {

        //            //RIntesity.Dispose();
        //            //RHeight.Dispose();
        //            return "参数设置错误";
        //        }
        //        RowCoord = new double[Len][]; ColCoord = new double[Len][]; ZCoord = new double[Len][]; StrLineOrCircle = new string[Len][];
        //        double[][] RowCoordt = new double[Len][]; double[][] ColCoordt = new double[Len][]; /*double[][] ZCoordt = new double[Len][];*/

        //        int Num = 0; int Add = 0; HTuple NewZ = new HTuple();
        //        HTuple origRow = new HTuple();
        //        HTuple origCol = new HTuple();
        //        for (int i = 0; i < Len; i++)
        //        {
        //            if (i == 11)
        //            {
        //                Debug.WriteLine(i);
        //            }
        //            HTuple row = new HTuple(), col = new HTuple();
        //            HTuple edgeRow = new HTuple(); HTuple edgeCol = new HTuple();//边缘点
        //            for (int j = Num; j < Add + fpTool.fParam[Sid].roiP[i].NumOfSection; j++)
        //            {
        //                if (Num == 99)
        //                {
        //                    Debug.WriteLine(Num);
        //                }
        //                //Debug.WriteLine(Num);
        //                HTuple row1, col1; HTuple anchor, anchorc; HTuple edgeR1, edgeC1;
        //                if (fpTool.fParam[Sid].roiP[i].SelectedType == 0)
        //                {
        //                    if (fpTool.fParam[Sid].roiP[i].TopDownDist != 0 && fpTool.fParam[Sid].roiP[i].xDist != 0)
        //                    {
        //                        string ok = fpTool.FindMaxPtFallDown(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
        //                    }
        //                    else
        //                    {
        //                        string ok = fpTool.FindMaxPt(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
        //                    }

        //                }
        //                else
        //                {
        //                    //取最高点下降
        //                    string ok = fpTool.FindMaxPtFallDown(Sid + 1, j, out row1, out col1, out anchor, out anchorc);

        //                }
        //                if (fpTool.fParam[Sid].roiP[i].useMidPt)
        //                {
        //                    //取边缘点
        //                    string ok = FindEdge(Sid + 1, j, out edgeR1, out edgeC1);
        //                    edgeRow = edgeRow.TupleConcat(edgeR1);
        //                    edgeCol = edgeCol.TupleConcat(edgeC1);
        //                }

        //                row = row.TupleConcat(row1);
        //                col = col.TupleConcat(col1);

        //                Num++;
        //            }

        //            string msg = "";
        //            if (hwind != null)
        //            {
        //                msg = fpTool.fParam[Sid].DicPointName[i];
        //            }

        //            if (row.Length < 2)
        //            {
        //                return "区域" + msg + "拟合点数过少";
        //            }


        //            Add = Num;
        //            if (row.Length == 0) //区域之外
        //            {
        //                continue;
        //            }
        //            HObject siglePart = new HObject();



        //            int Clipping = 0;
        //            int iNum = row.Length;
        //            double clipping = (double)fpTool.fParam[Sid].roiP[i].ClippingPer / 100;
        //            Clipping = (int)(iNum * clipping);
        //            if (Clipping == iNum / 2)
        //            {
        //                Clipping = iNum / 2 - 1;
        //            }

        //            HTuple lineAngle;
        //            //取Roi角度
        //            lineAngle = 1.571 - fpTool.fParam[Sid].roiP[i].phi;
        //            double angle1 = 1.571 + fpTool.fParam[Sid].roiP[i].phi;
        //            double Smoothcont = fpTool.fParam[Sid].roiP[i].SmoothCont;
        //            string IgnoreStr = IgnorPoint(row, col, angle1, Smoothcont, out row, out col);
        //            if (IgnoreStr != "OK")
        //            {
        //                return "忽略点处理失败" + IgnoreStr;
        //            }

        //            if (hwind != null && debug)
        //            {
        //                HObject Cross = new HObject();
        //                HOperatorSet.GenCrossContourXld(out Cross, row, col, 5, 0.5);

        //                Action sw = () =>
        //                {
        //                    hwind.viewWindow.displayHobject(Cross, "green", false);
        //                };
        //                hwind.Invoke(sw);

        //                if (fpTool.fParam[Sid].roiP[i].useMidPt)
        //                {
        //                    HObject Cross1 = new HObject();
        //                    HOperatorSet.GenCrossContourXld(out Cross1, edgeRow, edgeCol, 5, 0.5);

        //                    Action sw2 = () =>
        //                    {
        //                        hwind.viewWindow.displayHobject(Cross1, "green", false);
        //                    };
        //                    hwind.Invoke(sw2);
        //                }
        //            }


        //            //直线段拟合
        //            //if (fpTool.fParam[Sid].roiP[i].LineOrCircle != "圆弧段")
        //            //{
        //            HObject line = new HObject();
        //            HOperatorSet.GenContourPolygonXld(out line, row, col);

        //            HTuple Rowbg, Colbg, RowEd, ColEd, Nr, Nc, Dist;
        //            HOperatorSet.FitLineContourXld(line, "tukey", -1, Clipping, 5, 2, out Rowbg, out Colbg, out RowEd, out ColEd, out Nr, out Nc, out Dist);
        //            HOperatorSet.GenContourPolygonXld(out line, Rowbg.TupleConcat(RowEd), Colbg.TupleConcat(ColEd));

        //            HObject lineEdge = new HObject();
        //            if (fpTool.fParam[Sid].roiP[i].useMidPt)
        //            {
        //                HOperatorSet.GenContourPolygonXld(out lineEdge, edgeRow, edgeCol);
        //                HTuple Rowbg1, Colbg1, RowEd1, ColEd1, Nr1, Nc1, Dist1;
        //                HOperatorSet.FitLineContourXld(lineEdge, "tukey", -1, Clipping, 5, 2, out Rowbg1, out Colbg1, out RowEd1, out ColEd1, out Nr1, out Nc1, out Dist1);
        //                HOperatorSet.GenContourPolygonXld(out lineEdge, Rowbg1.TupleConcat(RowEd1), Colbg1.TupleConcat(ColEd1));

        //                HTuple midRbg = (Rowbg + Rowbg1) / 2;
        //                HTuple midRed = (RowEd + RowEd1) / 2;
        //                HTuple midCbg = (Colbg + Colbg1) / 2;
        //                HTuple midCed = (ColEd + ColEd1) / 2;
        //                Rowbg = midRbg;
        //                RowEd = midRed;
        //                Colbg = midCbg;
        //                ColEd = midCed;
        //            }

        //            //取拟合线与ROI中心交点
        //            //HObject RoiCenter = new HObject();
        //            HTuple recCoord = fpTool.roiList2[Sid][i].getModelData();
        //            HTuple CenterR = new HTuple(); HTuple CenterC = new HTuple();
        //            if (homMatFix != null)
        //            {
        //                HOperatorSet.AffineTransPoint2d(homMatFix, recCoord[0], recCoord[1], out CenterR, out CenterC);
        //            }
        //            else
        //            {
        //                CenterR = recCoord[0];
        //                CenterC = recCoord[1];
        //            }

        //            double EndR = CenterR + 100 * Math.Sin(fpTool.fParam[Sid].roiP[i].phi);
        //            double EndC = CenterC + 100 * Math.Cos(fpTool.fParam[Sid].roiP[i].phi);
        //            //HOperatorSet.GenRegionLine(out RoiCenter, fpTool.fParam[Sid].roiP[i].CenterRow, fpTool.fParam[Sid].roiP[i].CenterCol, EndR, EndC);
        //            //HOperatorSet.GenRegionLine(out line, Rowbg, Colbg, RowEd, ColEd);
        //            //if (hwind != null && debug)
        //            //{
        //            //    hwind.viewWindow.displayHobject(RoiCenter);
        //            //    hwind.viewWindow.displayHobject(line);
        //            //}
        //            HTuple isOverlapping = new HTuple();
        //            HOperatorSet.IntersectionLines(CenterR, CenterC, EndR, EndC, Rowbg, Colbg, RowEd, ColEd, out row, out col, out isOverlapping);

        //            double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
        //            double Yresolution = MyGlobal.globalConfig.dataContext.yResolution;

        //            if (Xresolution == 0)
        //            {
        //                return "XResolution=0";
        //            }
        //            double DisX = fpTool.fParam[Sid].roiP[i].offset * Math.Sin(lineAngle.D) / Xresolution;
        //            double DisY = fpTool.fParam[Sid].roiP[i].offset * Math.Cos(lineAngle.D) / Yresolution;

        //            double D = Math.Sqrt(DisX * DisX + DisY * DisY);
        //            if (fpTool.fParam[Sid].roiP[i].offset > 0)
        //            {
        //                D = -D;
        //            }
        //            double distR = D * Math.Cos(lineAngle.D);
        //            double distC = D * Math.Sin(lineAngle.D);

        //            //row = (Rowbg.D + RowEd.D) / 2 - distR;
        //            //col = (Colbg.D + ColEd.D) / 2 - distC;
        //            row = row.D - distR;
        //            col = col.D - distC;

        //            double xOffset = fpTool.fParam[Sid].roiP[i].Xoffset / Xresolution;
        //            double yOffset = fpTool.fParam[Sid].roiP[i].Yoffset / Yresolution;
        //            row = row - yOffset;
        //            col = col - xOffset;

        //            if (hwind != null)
        //            {
        //                HObject cross1 = new HObject();
        //                HOperatorSet.GenCrossContourXld(out cross1, row, col, 30, 0.5);

        //                Action sw = () =>
        //                {
        //                    hwind.viewWindow.displayHobject(cross1, "cadet blue", false);
        //                };
        //                hwind.Invoke(sw);
        //            }


        //            //HTuple linephi = new HTuple();
        //            //HOperatorSet.LineOrientation(Rowbg, Colbg, RowEd, ColEd, out linephi);
        //            //HTuple deg = -(new HTuple(linephi.D)).TupleDeg();
        //            //double tan = Math.Tan(-linephi.D);

        //            //int len1 = Math.Abs((int)(Rowbg.D - RowEd.D));
        //            //int len2 = Math.Abs((int)(Colbg.D - ColEd.D));
        //            //int len = len1 > len2 ? len1 : len2;
        //            //HTuple x0 = Colbg.D;
        //            //HTuple y0 = Rowbg.D;
        //            //double[] newr = new double[len]; double[] newc = new double[len];
        //            //for (int m = 0; m < len; m++)
        //            //{
        //            //    HTuple row1 = new HTuple(); HTuple col1 = new HTuple();
        //            //    if (len1 > len2)
        //            //    {
        //            //        row1 = Rowbg.D > RowEd.D ? y0 - m : y0 + m;
        //            //        col1 = Rowbg.D > RowEd.D ? x0 - m / tan : x0 + m / tan;

        //            //    }
        //            //    else
        //            //    {
        //            //        row1 = Colbg.D > ColEd.D ? y0 - m * tan : y0 + m * tan;
        //            //        col1 = Colbg.D > ColEd.D ? x0 - m : x0 + m;
        //            //    }
        //            //    newr[m] = row1;
        //            //    newc[m] = col1;
        //            //}
        //            //row = newr;col = newc;

        //            //HOperatorSet.GenContourPolygonXld(out line, row, col);


        //            if (hwind != null && debug)
        //            {
        //                Action sw = () =>
        //                {
        //                    hwind.viewWindow.displayHobject(line, "red");
        //                    hwind.viewWindow.displayHobject(lineEdge, "red");
        //                };
        //                hwind.Invoke(sw);


        //            }
        //            siglePart = line;
        //            //}
        //            //else
        //            //{
        //            //    HObject ArcObj = new HObject();
        //            //    HOperatorSet.GenContourPolygonXld(out ArcObj, row, col);
        //            //    HTuple Rb, Cb, Re, Ce, Nr1, Nc1, Ptorder;
        //            //    HOperatorSet.FitLineContourXld(ArcObj, "tukey", -1, Clipping, 5, 2, out Rb, out Cb, out Re, out Ce, out Nr1, out Nc1, out Ptorder);
        //            //    HOperatorSet.GenContourPolygonXld(out ArcObj, Rb.TupleConcat(Re), Cb.TupleConcat(Ce));

        //            //    HTuple lineAngle;
        //            //    HOperatorSet.AngleLx(Rb, Cb, Re, Ce, out lineAngle);
        //            //    double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
        //            //    double Yresolution = MyGlobal.globalConfig.dataContext.yResolution;

        //            //    if (Xresolution == 0)
        //            //    {
        //            //        return "XResolution = 0";
        //            //    }
        //            //    double DisX = fpTool.fParam[Sid].roiP[i].offset * Math.Sin(lineAngle.D) / Xresolution;
        //            //    double DisY = fpTool.fParam[Sid].roiP[i].offset * Math.Cos(lineAngle.D) / Yresolution;
        //            //    double D = Math.Sqrt(DisX * DisX + DisY * DisY);
        //            //    if (fpTool.fParam[Sid].roiP[i].offset < 0)
        //            //    {
        //            //        D = -D;
        //            //    }
        //            //    double distR = D * Math.Cos(lineAngle.D);
        //            //    double distC = D * Math.Sin(lineAngle.D);

        //            //    row = (Rb.D + Re.D) / 2 - distR;
        //            //    col = (Cb.D + Ce.D) / 2 - distC;

        //            //    double xOffset = fpTool.fParam[Sid].roiP[i].Xoffset / Xresolution;
        //            //    double yOffset = fpTool.fParam[Sid].roiP[i].Yoffset / Yresolution;
        //            //    row = row - yOffset;
        //            //    col = col - xOffset;

        //            //    if (hwind != null)
        //            //    {
        //            //        HObject CrossArc = new HObject();
        //            //        HOperatorSet.GenCrossContourXld(out CrossArc, row, col, 30, 0.5);
        //            //        hwind.viewWindow.displayHobject(CrossArc, "blue");
        //            //    }
        //            //    if (hwind != null && debug)
        //            //    {

        //            //        hwind.viewWindow.displayHobject(ArcObj, "red");
        //            //        hwind.viewWindow.dispMessage(msg, "blue", row.D, col.D);
        //            //    }

        //            //    siglePart = ArcObj;
        //            //}



        //            //加上 x y z 偏移
        //            //row = row + fpTool.fParam[Sid].roiP[i].Yoffset;
        //            //col = col + fpTool.fParam[Sid].roiP[i].Xoffset;


        //            //旋转至原图
        //            //HOperatorSet.AffineTransPoint2d(homMatRotateInvert, row, col, out row, out col);



        //            HTuple AvraRow = new HTuple(), AvraCol = new HTuple(), AvraZ = new HTuple();
        //            int PointCount = fpTool.fParam[Sid].roiP[i].NumOfSection;

        //            //取Z值 
        //            HTuple zcoord = new HTuple();
        //            try
        //            {
        //                HOperatorSet.GetGrayval(HeightImage, row, col, out zcoord);
        //            }
        //            catch (Exception)
        //            {

        //                return "偏移点" + msg + "设置在图像之外";
        //            }

        //            #region MyRegion
        //            ////除去 -100 的点
        //            //HTuple eq100 = new HTuple();
        //            //HOperatorSet.TupleLessElem(zcoord, -5, out eq100);
        //            //HTuple eq100Id = new HTuple();
        //            //HOperatorSet.TupleFind(eq100, 1, out eq100Id);
        //            //HTuple not5 = eq100.TupleFind(0);
        //            //HTuple Total = zcoord[not5];
        //            //HTuple Mean = Total.TupleMean();
        //            //if (eq100Id != -1)
        //            //{
        //            //    for (int m = 0; m < eq100Id.Length; m++)
        //            //    {
        //            //        if (eq100Id[m] == 0)
        //            //        {
        //            //            HTuple meanZ = new HTuple();
        //            //            meanZ = meanZ.TupleConcat(zcoord[eq100Id[m].D], zcoord[eq100Id[m].D + 1], zcoord[eq100Id[m].D + 2], zcoord[eq100Id[m].D + 3], zcoord[eq100Id[m].D + 4], zcoord[eq100Id[m].D + 5]);
        //            //            HTuple eq30 = new HTuple();
        //            //            HOperatorSet.TupleGreaterElem(meanZ, -5, out eq30);
        //            //            HTuple eq30Id = new HTuple();
        //            //            HOperatorSet.TupleFind(eq30, 1, out eq30Id);
        //            //            if (eq30Id != -1)
        //            //            {
        //            //                meanZ = meanZ[eq30Id];
        //            //                meanZ = meanZ.TupleMean();
        //            //                zcoord[eq100Id[m].D] = meanZ;
        //            //            }
        //            //            else
        //            //            {
        //            //                meanZ = Mean;
        //            //                zcoord[eq100Id[m].D] = meanZ;
        //            //            }

        //            //        }
        //            //        if (eq100Id[m] - 1 >= 0)
        //            //        {
        //            //            if (zcoord[eq100Id[m].D - 1].D < -5)
        //            //            {
        //            //                zcoord[eq100Id[m].D] = Mean;
        //            //            }
        //            //            else
        //            //            {
        //            //                zcoord[eq100Id[m].D] = zcoord[eq100Id[m].D - 1];
        //            //            }

        //            //        }
        //            //        else
        //            //        {
        //            //            zcoord[eq100Id[m].D] = Mean;
        //            //        }

        //            //    }
        //            //}


        //            ////将行列Z 均分
        //            //double  Div = row.Length / (double) ((PointCount - 1));
        //            //if (Div <1)
        //            //{
        //            //    Div = 1;
        //            //}
        //            //for (int k = 0; k < PointCount; k++)
        //            //{
        //            //    if (k == 0)
        //            //    {
        //            //        AvraRow = AvraRow.TupleConcat(row[0]);
        //            //        AvraCol = AvraCol.TupleConcat(col[0]);
        //            //        AvraZ = AvraZ.TupleConcat(zcoord[0]);
        //            //        continue;
        //            //    }

        //            //    if (k == 529)
        //            //    {
        //            //        //Debug.WriteLine("k" + k);
        //            //    }
        //            //    //Debug.WriteLine("k" + k);


        //            //    int id = (int)(Div * k);
        //            //    if (id >= row.Length)
        //            //    {
        //            //        break;
        //            //    }
        //            //    AvraRow = AvraRow.TupleConcat(row[id]);
        //            //    AvraCol = AvraCol.TupleConcat(col[id]);
        //            //    AvraZ = AvraZ.TupleConcat(zcoord[id]);
        //            //}
        //            //row = AvraRow;
        //            //col = AvraCol;
        //            //zcoord = AvraZ;


        //            //HTuple xc = new HTuple();
        //            //HOperatorSet.TupleGenSequence(0, zcoord.Length - 1, 1, out xc);
        //            //HObject Cont = new HObject();
        //            //HOperatorSet.GenContourPolygonXld(out Cont, zcoord, xc);
        //            //if (fpTool.fParam[Sid].roiP[i].LineOrCircle == "直线段")
        //            //{

        //            //    HTuple Rowbg1, Colbg1, RowEd1, ColEd1, Nr1, Nc1, Dist1;
        //            //    HOperatorSet.FitLineContourXld(Cont, "tukey", -1, 0, 5, 2, out Rowbg1, out Colbg1, out RowEd1, out ColEd1, out Nr1, out Nc1, out Dist1);

        //            //    HTuple linephi1 = new HTuple();
        //            //    HOperatorSet.LineOrientation(Rowbg1, Colbg1, RowEd1, ColEd1, out linephi1);
        //            //    //HTuple deg = -(new HTuple(linephi1.D)).TupleDeg();
        //            //    double tan1 = Math.Tan(-linephi1.D);

        //            //    int len11 = Math.Abs((int)(Rowbg1.D - RowEd1.D));
        //            //    int len21 = Math.Abs((int)(Colbg1.D - ColEd1.D));
        //            //    int len0 = zcoord.Length;
        //            //    HTuple x01 = Colbg1.D;
        //            //    HTuple y01 = Rowbg1.D;
        //            //    double[] newr1 = new double[len0]; double[] newc1 = new double[len0];
        //            //    for (int m = 0; m < len0; m++)
        //            //    {
        //            //        HTuple row1 = new HTuple(); HTuple col1 = new HTuple();
        //            //        if (len11 > len21)
        //            //        {
        //            //            row1 = Rowbg1.D > RowEd1.D ? y01 - m : y01 + m;
        //            //            col1 = Rowbg1.D > RowEd1.D ? x01 - m / tan1 : x01 + m / tan1;

        //            //        }
        //            //        else
        //            //        {
        //            //            row1 = Colbg1.D > ColEd1.D ? y01 - m * tan1 : y01 + m * tan1;
        //            //            col1 = Colbg1.D > ColEd1.D ? x01 - m : x01 + m;
        //            //        }
        //            //        newr1[m] = row1;
        //            //        newc1[m] = col1;
        //            //    }
        //            //    zcoord = newr1; xc = newc1;

        //            //}
        //            //else
        //            //{
        //            //    HOperatorSet.GenContourPolygonXld(out Cont, xc, zcoord);
        //            //    HOperatorSet.SmoothContoursXld(Cont, out Cont, 15);
        //            //    HOperatorSet.GetContourXld(Cont, out xc, out zcoord);
        //            //}


        //            //HTuple origRow = row;
        //            //HTuple origCol = col;

        //            #region 单边去重
        //            //if (i > 0)
        //            //{
        //            //        HObject regXld = new HObject();
        //            //        HOperatorSet.GenRegionContourXld(siglePart, out regXld, "filled");
        //            //        HTuple phi;
        //            //        HOperatorSet.RegionFeatures(regXld, "phi", out phi);
        //            //        phi = phi.TupleDeg();
        //            //        phi = phi.TupleAbs();
        //            //        //HTuple last1 = RowCoordt[i - 1][0];//x1
        //            //        //HTuple lastc1 = ColCoordt[i - 1][0];//x1
        //            //        //HTuple sub1 = Math.Abs(row[0].D - last1.D);
        //            //        //HTuple sub2 = Math.Abs(col[0].D - lastc1.D);
        //            //        //HTuple pt1 = sub1.D > sub2.D ? RowCoordt[i - 1][RowCoordt[i - 1].Length - 1] : ColCoordt[i - 1][ColCoordt[i - 1].Length - 1];
        //            //        HTuple pt1 = phi.D > 75 ? RowCoordt[i - 1][RowCoordt[i - 1].Length - 1] : ColCoordt[i - 1][ColCoordt[i - 1].Length - 1];
        //            //        HTuple colbase = Sid == 0 || Sid == 2 ? col.TupleLessEqualElem(pt1) : col.TupleGreaterEqualElem(pt1);
        //            //        HTuple Grater1 = phi.D > 75 ? row.TupleGreaterEqualElem(pt1) : colbase;

        //            //        switch (Sid)
        //            //    {
        //            //        case 0: //x2>x1

        //            //            HTuple Grater1id = Grater1.TupleFind(1);
        //            //            row = row.TupleRemove(Grater1id);
        //            //            col = col.TupleRemove(Grater1id);
        //            //            zcoord = zcoord.TupleRemove(Grater1id);

        //            //            break;
        //            //        case 1: //y2>y1

        //            //            HTuple Grater2id = Grater1.TupleFind(1);
        //            //            row = row.TupleRemove(Grater2id);
        //            //            col = col.TupleRemove(Grater2id);
        //            //            //origRow = origRow.TupleRemove(Grater2id);
        //            //            //origCol = origCol.TupleRemove(Grater2id);
        //            //            zcoord = zcoord.TupleRemove(Grater2id);
        //            //            break;
        //            //        case 2: //x2<x1

        //            //            HTuple Grater3id = Grater1.TupleFind(1);
        //            //            row = row.TupleRemove(Grater3id);
        //            //                col = col.TupleRemove(Grater3id);
        //            //                //origRow = origRow.TupleRemove(Grater3id);
        //            //                //origCol = origCol.TupleRemove(Grater3id);
        //            //                zcoord = zcoord.TupleRemove(Grater3id);
        //            //                break;
        //            //        case 3: //y2<y1

        //            //            HTuple Grater4id = Grater1.TupleFind(1);
        //            //            row = row.TupleRemove(Grater4id);
        //            //            col = col.TupleRemove(Grater4id);
        //            //            //origRow = origRow.TupleRemove(Grater4id);
        //            //            //origCol = origCol.TupleRemove(Grater4id);
        //            //            zcoord = zcoord.TupleRemove(Grater4id);
        //            //            break;
        //            //    }
        //            //}
        //            //if (row.Length == 0)
        //            //{
        //            //    return "单边设置 区域重复点数过大";
        //            //}
        //            #endregion 单边去重
        //            #endregion
        //            //

        //            if (hwind != null && debug == false)
        //            {
        //                HObject NewSide = new HObject();
        //                HOperatorSet.GenContourPolygonXld(out NewSide, row, col);

        //                Action sw = () =>
        //                {
        //                    hwind.viewWindow.displayHobject(NewSide, "red");
        //                };
        //                hwind.Invoke(sw);
        //            }
        //            origRow = origRow.TupleConcat(row);
        //            origCol = origCol.TupleConcat(col);
        //            RowCoordt[i] = row;
        //            ColCoordt[i] = col;
        //            //进行矩阵变换
        //            if (HomMat3D != null)
        //            {
        //                //HOperatorSet.AffineTransPoint3d(HomMat3D, row, col,zcoord, out row, out col,out zcoord);
        //                HOperatorSet.AffineTransPoint2d(HomMat3D, row, col, out row, out col);
        //                zcoord = zcoord + fpTool.fParam[Sid].roiP[i].Zoffset + fpTool.fParam[Sid].SigleZoffset + MyGlobal.globalConfig.TotalZoffset;
        //                row = row + MyGlobal.globalConfig.gbParam[Sid].Xoffset;
        //                col = col + MyGlobal.globalConfig.gbParam[Sid].Yoffset;

        //            }

        //            for (int n = 0; n < row.Length; n++)
        //            {
        //                Str.Append(row[n].D.ToString() + "," + col[n].D.ToString() + "," + zcoord[n].D.ToString() + "\r\n");
        //            }
        //            RowCoord[i] = row;
        //            ColCoord[i] = col;
        //            ZCoord[i] = zcoord;
        //            StrLineOrCircle[i] = new string[zcoord.Length];


        //            //ColCoordt[i] = col;
        //            //ZCoordt[i] = zcoord;


        //            //StrLineOrCircle[i][0] = fpTool.fParam[Sid].roiP[i].LineOrCircle == "直线段" ? "1;" : "2;";
        //            switch (fpTool.fParam[Sid].roiP[i].LineOrCircle)
        //            {
        //                case "连接段":
        //                    StrLineOrCircle[i][0] = "0;";
        //                    break;
        //                case "直线段":
        //                    StrLineOrCircle[i][0] = "1;";
        //                    break;
        //                case "圆弧段":
        //                    StrLineOrCircle[i][0] = "2;";
        //                    break;
        //            }
        //            //for (int n = 1; n < zcoord.Length; n++)
        //            //{

        //            //    if (n== zcoord.Length -1 &&  i == Len -1 && Sid !=3) //最后一段 第四边不给
        //            //    {
        //            //        StrLineOrCircle[i][n] = StrLineOrCircle[i][0];
        //            //    }
        //            //    else
        //            //    {
        //            //        StrLineOrCircle[i][n] = "0;";

        //            //    }
        //            //}

        //            //NewZ = NewZ.TupleConcat(ZCoord[i][0]);
        //        }
        //        //HTuple zId =  NewZ.TupleGreaterElem(-10);
        //        //zId = zId.TupleFind(1);
        //        //NewZ = NewZ[zId];
        //        //for (int i = 0; i < ZCoord.GetLength(0); i++)
        //        //{
        //        //    if (ZCoord[i]== null)
        //        //    {
        //        //        return "Find Point 第" + (i + 1).ToString() + "点未取到";
        //        //    }
        //        //    if (ZCoord[i][0]==-30)
        //        //    {

        //        //        ZCoord[i][0] = NewZ.TupleMean();
        //        //    }
        //        //}
        //        originalPoint[0] = origRow;
        //        originalPoint[1] = origCol;

        //        for (int i = 0; i < ZCoord.GetLength(0); i++)
        //        {
        //            //在当前点附近取圆 
        //            HObject Circle = new HObject();
        //            double radius = fpTool.fParam[Sid].roiP[i].ZftRad;
        //            if (ZCoord[i][0] < -10 && radius == 0)
        //            {
        //                radius = 0.05;
        //            }
        //            double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
        //            radius = radius / Xresolution;
        //            if (radius != 0)
        //            {
        //                HOperatorSet.GenCircle(out Circle, origRow[i], origCol[i], radius);
        //                //if (debug && hwind !=null)
        //                //{
        //                //    hwind.viewWindow.displayHobject(Circle, "red");
        //                //}

        //                HTuple rows, cols; HTuple Zpt = new HTuple();
        //                HOperatorSet.GetRegionPoints(Circle, out rows, out cols);
        //                try
        //                {
        //                    HOperatorSet.GetGrayval(HeightImage, rows, cols, out Zpt);
        //                    HTuple greater = new HTuple();
        //                    HOperatorSet.TupleGreaterElem(Zpt, -10, out greater);
        //                    HTuple greaterId = greater.TupleFind(1);
        //                    if (greaterId.D == -1)
        //                    {
        //                        return "高度滤波区域" + fpTool.fParam[Sid].DicPointName[i] + "无有效z值";
        //                    }
        //                    HTuple Zgreater = Zpt[greaterId];
        //                    HTuple maxPer = fpTool.fParam[Sid].roiP[i].ZftMax / 100;
        //                    HTuple minPer = fpTool.fParam[Sid].roiP[i].ZftMin / 100;

        //                    int imax = (int)maxPer.D * Zgreater.Length;
        //                    int imin = (int)minPer.D * Zgreater.Length;
        //                    //if (imax != imin)
        //                    //{
        //                    HTuple Down = Zgreater.TupleSelectRange(imin, Zgreater.Length - imax - 1);
        //                    ZCoord[i][0] = Down.TupleMean();
        //                    //}
        //                    //else
        //                    //{
        //                    //    ZCoord[i][0] = Zgreater.TupleMean();
        //                    //}

        //                }
        //                catch (Exception)
        //                {

        //                    return "偏移点" + fpTool.fParam[Sid].DicPointName[i] + "设置在图像之外";
        //                }
        //            }
        //            else
        //            {

        //            }
        //            string msg = fpTool.fParam[Sid].DicPointName[i] + "(" + Math.Round(ZCoord[i][0], 3).ToString() + ")";
        //            if (hwind != null)
        //            {

        //                Action sw = () =>
        //                {
        //                    hwind.viewWindow.dispMessage(msg, "blue", origRow[i], origCol[i]);
        //                };
        //                hwind.Invoke(sw);
        //            }

        //            ////判断 Z 值高度
        //            if (MyGlobal.xyzBaseCoord.ZCoord != null && MyGlobal.xyzBaseCoord.ZCoord.Count != 0)
        //            {
        //                if (ZCoord[i][0] - MyGlobal.xyzBaseCoord.ZCoord[Sid][i][0] > MyGlobal.globalConfig.HeightMax || ZCoord[i][0] - MyGlobal.xyzBaseCoord.ZCoord[Sid][i][0] < MyGlobal.globalConfig.HeightMin)
        //                {
        //                    if (hwind != null)
        //                    {

        //                        Action sw = () =>
        //                        {
        //                            hwind.viewWindow.dispMessage(msg + "-Height NG", "red", origRow[i], origCol[i]);
        //                        };
        //                        hwind.Invoke(sw);
        //                    }
        //                    return $"{msg}高度超出范围" + Math.Round(ZCoord[i][0], 3);
        //                }

        //            }

        //        }

        //        if (HomMat3D == null)
        //        {
        //            StaticOperate.writeTxt("D:\\Laser3DOrign.txt", Str.ToString());
        //        }

        //        //RIntesity.Dispose();
        //        //RHeight.Dispose();
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        //RIntesity.Dispose();
        //        //RHeight.Dispose();
        //        return "FindPoint error:" + ex.Message;
        //    }
        //}
        //[Obsolete]
        //public string FindPoint(int SideId, HObject HeightImage, out List<HTuple> HLines, HWindow_Final hwind = null, bool debug = false)
        //{
        //    //HObject RIntesity = new HObject(), RHeight = new HObject();
        //    StringBuilder Str = new StringBuilder();
        //    HLines = new List<HTuple>();

        //    double[][] RowCoord = null; double[][] ColCoord = null; double[][] ZCoord = null; string[][] StrLineOrCircle = null;
        //    try
        //    {
        //        int Sid = SideId - 1;
        //        string ok1 = GenProfileCoord(Sid + 1, HeightImage, out RArray, out Row, out CArray, out Phi, out Ignore, null);
        //        if (ok1 != "OK")
        //        {
        //            return ok1;
        //        }
        //        int Len = fpTool.roiList2[Sid].Count;// 区域数量
        //        if (Len == 0)
        //        {

        //            //RIntesity.Dispose();
        //            //RHeight.Dispose();
        //            return "参数设置错误";
        //        }
        //        RowCoord = new double[Len][]; ColCoord = new double[Len][]; ZCoord = new double[Len][]; StrLineOrCircle = new string[Len][];
        //        double[][] RowCoordt = new double[Len][]; double[][] ColCoordt = new double[Len][]; /*double[][] ZCoordt = new double[Len][];*/

        //        int Num = 0; int Add = 0; HTuple NewZ = new HTuple();
        //        for (int i = 0; i < Len; i++)
        //        {

        //            HTuple row = new HTuple(), col = new HTuple();

        //            for (int j = Num; j < Add + fpTool.fParam[Sid].roiP[i].NumOfSection; j++)
        //            {
        //                if (Num == 99)
        //                {
        //                    Debug.WriteLine(Num);
        //                }
        //                //Debug.WriteLine(Num);
        //                HTuple row1, col1; HTuple anchor, anchorc;
        //                if (fpTool.fParam[Sid].roiP[i].SelectedType == 0)
        //                {
        //                    if (fpTool.fParam[Sid].roiP[i].TopDownDist != 0 && fpTool.fParam[Sid].roiP[i].xDist != 0)
        //                    {
        //                        string ok = fpTool.FindMaxPtFallDown(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
        //                    }
        //                    else
        //                    {
        //                        string ok = fpTool.FindMaxPt(Sid + 1, j, out row1, out col1, out anchor, out anchorc);

        //                    }
        //                }
        //                else
        //                {
        //                    //取最高点下降
        //                    string ok = fpTool.FindMaxPtFallDown(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
        //                }



        //                row = row.TupleConcat(row1);
        //                col = col.TupleConcat(col1);
        //                Num++;
        //            }
        //            Add = Num;
        //            if (row.Length < 2)
        //            {
        //                //return "区域" + (i + 1).ToString() + "拟合点数过少";
        //                HLines.Add(new HTuple());
        //                continue;
        //            }



        //            if (row.Length == 0) //区域之外
        //            {
        //                continue;
        //            }
        //            HObject siglePart = new HObject();

        //            int Clipping = 0;
        //            int iNum = row.Length;
        //            double clipping = fpTool.fParam[Sid].roiP[i].ClippingPer / 100;
        //            Clipping = (int)(iNum * clipping);
        //            if (Clipping == iNum / 2)
        //            {
        //                Clipping = iNum / 2 - 1;
        //            }
        //            ////直线段拟合
        //            //if (fpTool.fParam[Sid].roiP[i].LineOrCircle != "圆弧段")
        //            //{
        //            //除去偏离较大点
        //            HTuple lineAngle;
        //            lineAngle = 1.571 - fpTool.fParam[Sid].roiP[i].phi;
        //            double angle1 = 1.571 + fpTool.fParam[Sid].roiP[i].phi;
        //            double Smoothcont = fpTool.fParam[Sid].roiP[i].SmoothCont;
        //            string IgnoreStr = IgnorPoint(row, col, angle1, Smoothcont, out row, out col);
        //            if (IgnoreStr != "OK")
        //            {
        //                return "忽略点处理失败" + IgnoreStr;
        //            }
        //            if (hwind != null && debug)
        //            {
        //                HObject Cross = new HObject();
        //                HOperatorSet.GenCrossContourXld(out Cross, row, col, 5, 0.5);
        //                Action sw = () =>
        //                {
        //                    hwind.viewWindow.displayHobject(Cross, "green", false);
        //                };
        //                hwind.Invoke(sw);
        //            }

        //            HObject line = new HObject();
        //            HOperatorSet.GenContourPolygonXld(out line, row, col);

        //            HTuple Rowbg, Colbg, RowEd, ColEd, Nr, Nc, Dist;
        //            HOperatorSet.FitLineContourXld(line, "tukey", -1, Clipping, 5, 2, out Rowbg, out Colbg, out RowEd, out ColEd, out Nr, out Nc, out Dist);
        //            HOperatorSet.GenContourPolygonXld(out line, Rowbg.TupleConcat(RowEd), Colbg.TupleConcat(ColEd));

        //            //取拟合线与ROI中心交点
        //            //HObject RoiCenter = new HObject();
        //            HTuple recCoord = fpTool.roiList2[Sid][i].getModelData();
        //            HTuple CenterR = new HTuple(); HTuple CenterC = new HTuple();

        //            CenterR = recCoord[0];
        //            CenterC = recCoord[1];

        //            double EndR = CenterR + 100 * Math.Sin(fpTool.fParam[Sid].roiP[i].phi);
        //            double EndC = CenterC + 100 * Math.Cos(fpTool.fParam[Sid].roiP[i].phi);
        //            //HOperatorSet.GenRegionLine(out RoiCenter, fpTool.fParam[Sid].roiP[i].CenterRow, fpTool.fParam[Sid].roiP[i].CenterCol, EndR, EndC);
        //            //HOperatorSet.GenRegionLine(out line, Rowbg, Colbg, RowEd, ColEd);
        //            //if (hwind!=null && debug)
        //            //{
        //            //    hwind.viewWindow.displayHobject(RoiCenter);
        //            //    hwind.viewWindow.displayHobject(line);
        //            //}
        //            HTuple isOverlapping = new HTuple();
        //            HOperatorSet.IntersectionLines(CenterR, CenterC, EndR, EndC, Rowbg, Colbg, RowEd, ColEd, out row, out col, out isOverlapping);

        //            double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
        //            double Yresolution = MyGlobal.globalConfig.dataContext.yResolution;

        //            if (Xresolution == 0)
        //            {
        //                return "XResolution=0";
        //            }
        //            double DisX = fpTool.fParam[Sid].roiP[i].offset * Math.Sin(lineAngle.D) / Xresolution;
        //            double DisY = fpTool.fParam[Sid].roiP[i].offset * Math.Cos(lineAngle.D) / Yresolution;

        //            double D = Math.Sqrt(DisX * DisX + DisY * DisY);
        //            if (fpTool.fParam[Sid].roiP[i].offset > 0)
        //            {
        //                D = -D;
        //            }
        //            double distR = D * Math.Cos(lineAngle.D);
        //            double distC = D * Math.Sin(lineAngle.D);

        //            double xOffset = fpTool.fParam[Sid].roiP[i].Xoffset / Xresolution;
        //            double yOffset = fpTool.fParam[Sid].roiP[i].Yoffset / Yresolution;

        //            Rowbg = Rowbg.D - distR + yOffset;
        //            RowEd = RowEd.D - distR + yOffset;
        //            Colbg = Colbg.D - distC + xOffset;
        //            ColEd = ColEd.D - distC + xOffset;

        //            HTuple lineCoord = Rowbg.TupleConcat(Colbg).TupleConcat(RowEd).TupleConcat(ColEd);
        //            HLines.Add(lineCoord);

        //            //row = (Rowbg.D + RowEd.D) / 2;
        //            //col = (Colbg.D + ColEd.D) / 2;
        //            row = row - distR + yOffset;
        //            col = col - distC + xOffset;

        //            if (hwind != null)
        //            {
        //                HObject cross1 = new HObject();
        //                HOperatorSet.GenCrossContourXld(out cross1, row, col, 30, 0.5);

        //                Action sw = () =>
        //                {
        //                    hwind.viewWindow.displayHobject(cross1, "yellow");
        //                };

        //                hwind.Invoke(sw);
        //            }

        //            //HTuple linephi = new HTuple();
        //            //HOperatorSet.LineOrientation(Rowbg, Colbg, RowEd, ColEd, out linephi);
        //            //HTuple deg = -(new HTuple(linephi.D)).TupleDeg();
        //            //double tan = Math.Tan(-linephi.D);

        //            //int len1 = Math.Abs((int)(Rowbg.D - RowEd.D));
        //            //int len2 = Math.Abs((int)(Colbg.D - ColEd.D));
        //            //int len = len1 > len2 ? len1 : len2;
        //            //HTuple x0 = Colbg.D;
        //            //HTuple y0 = Rowbg.D;
        //            //double[] newr = new double[len]; double[] newc = new double[len];
        //            //for (int m = 0; m < len; m++)
        //            //{
        //            //    HTuple row1 = new HTuple(); HTuple col1 = new HTuple();
        //            //    if (len1 > len2)
        //            //    {
        //            //        row1 = Rowbg.D > RowEd.D ? y0 - m : y0 + m;
        //            //        col1 = Rowbg.D > RowEd.D ? x0 - m / tan : x0 + m / tan;

        //            //    }
        //            //    else
        //            //    {
        //            //        row1 = Colbg.D > ColEd.D ? y0 - m * tan : y0 + m * tan;
        //            //        col1 = Colbg.D > ColEd.D ? x0 - m : x0 + m;
        //            //    }
        //            //    newr[m] = row1;
        //            //    newc[m] = col1;
        //            //}
        //            //row = newr;col = newc;

        //            //HOperatorSet.GenContourPolygonXld(out line, row, col);


        //            if (hwind != null && debug)
        //            {

        //                Action sw = () =>
        //                {
        //                    hwind.viewWindow.displayHobject(line, "red");
        //                };
        //                hwind.Invoke(sw);
        //            }
        //            siglePart = line;
        //            //}
        //            //else
        //            //{
        //            //    HObject ArcObj = new HObject();
        //            //    HOperatorSet.GenContourPolygonXld(out ArcObj, row, col);
        //            //    HTuple Rowbg, Colbg, RowEd, ColEd, Nr1, Nc1, Ptorder;
        //            //    HOperatorSet.FitLineContourXld(ArcObj, "tukey", -1, Clipping, 5, 2, out Rowbg, out Colbg, out RowEd, out ColEd, out Nr1, out Nc1, out Ptorder);
        //            //    HOperatorSet.GenContourPolygonXld(out ArcObj, Rowbg.TupleConcat(RowEd), Colbg.TupleConcat(ColEd));


        //            //    HTuple lineAngle;
        //            //    HOperatorSet.AngleLx(Rowbg, Colbg, RowEd, ColEd, out lineAngle);
        //            //    double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
        //            //    double Yresolution = MyGlobal.globalConfig.dataContext.yResolution;

        //            //    if (Xresolution == 0)
        //            //    {
        //            //        return "XResolution=0";
        //            //    }
        //            //    double DisX = fpTool.fParam[Sid].roiP[i].offset * Math.Sin(lineAngle.D) / Xresolution;
        //            //    double DisY = fpTool.fParam[Sid].roiP[i].offset * Math.Cos(lineAngle.D) / Yresolution;

        //            //    double D = Math.Sqrt(DisX * DisX + DisY * DisY);
        //            //    if (fpTool.fParam[Sid].roiP[i].offset < 0)
        //            //    {
        //            //        D = -D;
        //            //    }
        //            //    double distR = D * Math.Cos(lineAngle.D);
        //            //    double distC = D * Math.Sin(lineAngle.D);

        //            //    double xOffset = fpTool.fParam[Sid].roiP[i].Xoffset / Xresolution;
        //            //    double yOffset = fpTool.fParam[Sid].roiP[i].Yoffset / Yresolution;

        //            //    Rowbg = Rowbg.D - distR + yOffset;
        //            //    RowEd = RowEd.D - distR + yOffset;
        //            //    Colbg = Colbg.D - distC + xOffset;
        //            //    ColEd = ColEd.D - distC + xOffset;


        //            //    row = (Rowbg.D + RowEd.D) / 2 ;
        //            //    col = (Colbg.D + ColEd.D) / 2 ;
        //            //    HTuple lineCoord = Rowbg.TupleConcat(Colbg).TupleConcat(RowEd).TupleConcat(ColEd);
        //            //    HLines.Add(lineCoord);

        //            //    if (hwind != null)
        //            //    {
        //            //        HObject CrossArc = new HObject();
        //            //        HOperatorSet.GenCrossContourXld(out CrossArc, row, col, 30, 0.5);
        //            //        hwind.viewWindow.displayHobject(CrossArc, "blue");
        //            //    }
        //            //    if (hwind != null && debug)
        //            //    {
        //            //        hwind.viewWindow.displayHobject(ArcObj, "red");

        //            //    }

        //            //    siglePart = ArcObj;
        //            //}

        //            if (hwind != null)
        //            {
        //                //string[] color = { "red", "blue", "green", "lime green", "black" };
        //                if (row.Length != 0)
        //                {
        //                    //Random rad = new Random();
        //                    //int radi = rad.Next(4);
        //                    string msg = "";
        //                    //foreach (var item in DicPointName[Sid])
        //                    //{
        //                    //    if (item.Value == i)
        //                    //    {
        //                    //        msg = item.Key;
        //                    //    }
        //                    //}
        //                    msg = fpTool.fParam[Sid].DicPointName[i];

        //                    Action sw = () =>
        //                    {
        //                        hwind.viewWindow.dispMessage("Fix" + msg, "red", row.D, col.D);
        //                    };
        //                    hwind.Invoke(sw);

        //                }
        //            }
        //            if (hwind != null)
        //            {
        //                HObject cross1 = new HObject();
        //                HOperatorSet.GenCrossContourXld(out cross1, row, col, 30, 0.5);
        //                Action sw = () =>
        //                {
        //                    hwind.viewWindow.displayHobject(cross1, "red");
        //                };
        //                hwind.Invoke(sw);
        //            }

        //            //加上 x y z 偏移
        //            //row = row + fpTool.fParam[Sid].roiP[i].Yoffset;
        //            //col = col + fpTool.fParam[Sid].roiP[i].Xoffset;


        //            //旋转至原图
        //            //HOperatorSet.AffineTransPoint2d(homMatRotateInvert, row, col, out row, out col);



        //            HTuple AvraRow = new HTuple(), AvraCol = new HTuple(), AvraZ = new HTuple();
        //            int PointCount = fpTool.fParam[Sid].roiP[i].NumOfSection;

        //            ////取Z值 
        //            //HTuple zcoord = new HTuple();
        //            //try
        //            //{
        //            //    HOperatorSet.GetGrayval(HeightImage, row, col, out zcoord);
        //            //}
        //            //catch (Exception)
        //            //{

        //            //    return "偏移点设置在图像之外";
        //            //}
        //            //

        //            if (hwind != null && debug == false)
        //            {
        //                HObject NewSide = new HObject();
        //                HOperatorSet.GenContourPolygonXld(out NewSide, row, col);

        //                Action sw = () =>
        //                {
        //                    hwind.viewWindow.displayHobject(NewSide, "red");
        //                };
        //                hwind.Invoke(sw);
        //            }

        //            RowCoord[i] = row;
        //            ColCoord[i] = col;
        //            //ZCoord[i] = zcoord;
        //            //StrLineOrCircle[i] = new string[zcoord.Length];


        //            //ColCoordt[i] = col;
        //            //ZCoordt[i] = zcoord;


        //            //StrLineOrCircle[i][0] = fpTool.fParam[Sid].roiP[i].LineOrCircle == "直线段" ? "1;" : "2;";
        //            //for (int n = 1; n < zcoord.Length; n++)
        //            //{

        //            //    if (n == zcoord.Length - 1 && i == Len - 1 && Sid != 3) //最后一段 第四边不给
        //            //    {
        //            //        StrLineOrCircle[i][n] = StrLineOrCircle[i][0];
        //            //    }
        //            //    else
        //            //    {
        //            //        StrLineOrCircle[i][n] = "0;";

        //            //    }
        //            //}

        //            //NewZ = NewZ.TupleConcat(ZCoord[i][0]);
        //        }
        //        //HTuple zId = NewZ.TupleGreaterElem(-10);
        //        //zId = zId.TupleFind(1);
        //        //NewZ = NewZ[zId];
        //        //for (int i = 0; i < ZCoord.GetLength(0); i++)
        //        //{
        //        //    if (ZCoord[i] == null)
        //        //    {
        //        //        return "Find Point 第" + (i + 1).ToString() + "点未取到";
        //        //    }
        //        //    if (ZCoord[i][0] == -30)
        //        //    {

        //        //        ZCoord[i][0] = NewZ.TupleMean();
        //        //    }
        //        //}
        //        //RIntesity.Dispose();
        //        //RHeight.Dispose();
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        //RIntesity.Dispose();
        //        //RHeight.Dispose();
        //        return "FindPoint error:" + ex.Message;
        //    }
        //}
        //[Obsolete]
        //string IgnorPoint(HTuple row, HTuple col, double Phi, double SmoothCont, out HTuple Row, out HTuple Col)
        //{
        //    Row = new HTuple(); Col = new HTuple();
        //    try
        //    {
        //        HTuple RowMean, ColMean, RowMedian, ColMedian;
        //        HOperatorSet.TupleMean(row, out RowMean);
        //        HOperatorSet.TupleMean(col, out ColMean);
        //        HOperatorSet.TupleMedian(row, out RowMedian);
        //        HOperatorSet.TupleMedian(col, out ColMedian);
        //        double SubR = Math.Abs(RowMean.D - RowMedian.D);
        //        double SubC = Math.Abs(ColMean.D - ColMedian.D);
        //        double Sin = Math.Sin(Phi);
        //        double Cos = Math.Cos(Phi);
        //        double Standard = SubR * Cos + SubC * Sin;

        //        int Num = (int)(row.Length * SmoothCont);
        //        if (SmoothCont == 0 || SmoothCont == 1)
        //        {
        //            Row = row;
        //            Col = col;
        //            return "OK";
        //        }
        //        HTuple SUB = new HTuple();
        //        for (int i = 0; i < row.Length; i++)
        //        {
        //            double rowsub = Math.Abs(row[i].D - RowMedian.D);
        //            double colsub = Math.Abs(col[i].D - ColMedian.D);
        //            //double sub1 = rowsub * Cos + colsub * Sin;
        //            //SUB = SUB.TupleConcat(sub1);
        //            double Add = rowsub * Cos * rowsub * Cos + colsub * Sin * colsub * Sin;
        //            double Sqrt = Math.Sqrt(Add);
        //            SUB = SUB.TupleConcat(Sqrt);
        //        }
        //        HTuple NewId = new HTuple();
        //        HTuple Less = new HTuple();
        //        HTuple Indices = new HTuple();
        //        HOperatorSet.TupleSortIndex(SUB, out Indices);
        //        HTuple selected = new HTuple();
        //        HOperatorSet.TupleSelectRange(Indices, row.Length - Num, row.Length - 1, out selected);
        //        HOperatorSet.TupleRemove(row, selected, out Row);
        //        HOperatorSet.TupleRemove(col, selected, out Col);
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "IgnorPoint error" + ex.Message;
        //    }
        //}
        //#endregion




        public string FindPoint_BF(int SideId, HObject IntesityImage, HObject HeightImage, out double[][] RowCoord, out double[][] ColCoord, out double[][] ZCoord, out string[][] StrLineOrCircle, HTuple HomMat3D = null, HWindow_Final hwind = null, bool debug = false, HTuple homMatFix = null)
        {
            //HObject RIntesity = new HObject(), RHeight = new HObject();
            StringBuilder Str = new StringBuilder();
            RowCoord = null; ColCoord = null; ZCoord = null; StrLineOrCircle = null;
            try
            {
                int Sid = SideId - 1;
                string ok1 = fpTool.GenProfileCoord(Sid + 1, HeightImage, out FindPointTool.RArray, out FindPointTool.Row, out FindPointTool.CArray, out FindPointTool.Phi, out Ignore, homMatFix);
                if (ok1 != "OK")
                {
                    return ok1;
                }
                int Len = fpTool.roiList2[Sid].Count;// 区域数量
                if (Len == 0)
                {

                    //RIntesity.Dispose();
                    //RHeight.Dispose();
                    return "参数设置错误";
                }
                RowCoord = new double[Len][]; ColCoord = new double[Len][]; ZCoord = new double[Len][]; StrLineOrCircle = new string[Len][];
                double[][] RowCoordt = new double[Len][]; double[][] ColCoordt = new double[Len][]; /*double[][] ZCoordt = new double[Len][];*/

                int Num = 0; int Add = 0;
                for (int i = 0; i < Len; i++)
                {

                    HTuple row = new HTuple(), col = new HTuple();
                    for (int j = Num; j < Add + fpTool.fParam[Sid].roiP[i].NumOfSection; j++)
                    {
                        if (Num == 99)
                        {
                            Debug.WriteLine(Num);
                        }
                        Debug.WriteLine(Num);
                        HTuple row1, col1; HTuple anchor, anchorc;
                        string ok = fpTool.FindMaxPt(Sid + 1, j, out row1, out col1, out anchor, out anchorc);
                        //if (ok!="OK")
                        //{
                        //    return ok;
                        //}

                        row = row.TupleConcat(row1);
                        col = col.TupleConcat(col1);
                        Num++;
                    }
                    Add = Num;
                    if (row.Length == 0) //区域之外
                    {
                        continue;
                    }
                    HObject siglePart = new HObject();
                    //直线段拟合
                    if (fpTool.fParam[Sid].roiP[i].LineOrCircle == "直线段")
                    {
                        HObject line = new HObject();
                        HOperatorSet.GenContourPolygonXld(out line, row, col);
                        HTuple Rowbg, Colbg, RowEd, ColEd, Nr, Nc, Dist;
                        HOperatorSet.FitLineContourXld(line, "tukey", -1, 0, 5, 2, out Rowbg, out Colbg, out RowEd, out ColEd, out Nr, out Nc, out Dist);
                        Rowbg = Rowbg + fpTool.fParam[Sid].roiP[i].offset;
                        Colbg = Colbg + fpTool.fParam[Sid].roiP[i].Xoffset;
                        RowEd = RowEd + fpTool.fParam[Sid].roiP[i].Yoffset2;
                        ColEd = ColEd + fpTool.fParam[Sid].roiP[i].Xoffset2;

                        row = (Rowbg.D + RowEd.D) / 2 + fpTool.fParam[Sid].roiP[i].offset;
                        col = (Colbg.D + ColEd.D) / 2 + fpTool.fParam[Sid].roiP[i].Xoffset;


                        HTuple linephi = new HTuple();
                        HOperatorSet.LineOrientation(Rowbg, Colbg, RowEd, ColEd, out linephi);
                        HTuple deg = -(new HTuple(linephi.D)).TupleDeg();
                        double tan = Math.Tan(-linephi.D);

                        int len1 = Math.Abs((int)(Rowbg.D - RowEd.D));
                        int len2 = Math.Abs((int)(Colbg.D - ColEd.D));
                        int len = len1 > len2 ? len1 : len2;
                        HTuple x0 = Colbg.D;
                        HTuple y0 = Rowbg.D;
                        double[] newr = new double[len]; double[] newc = new double[len];
                        for (int m = 0; m < len; m++)
                        {
                            HTuple row1 = new HTuple(); HTuple col1 = new HTuple();
                            if (len1 > len2)
                            {
                                row1 = Rowbg.D > RowEd.D ? y0 - m : y0 + m;
                                col1 = Rowbg.D > RowEd.D ? x0 - m / tan : x0 + m / tan;

                            }
                            else
                            {
                                row1 = Colbg.D > ColEd.D ? y0 - m * tan : y0 + m * tan;
                                col1 = Colbg.D > ColEd.D ? x0 - m : x0 + m;
                            }
                            newr[m] = row1;
                            newc[m] = col1;
                        }
                        row = newr; col = newc;

                        HOperatorSet.GenContourPolygonXld(out line, row, col);


                        ////两条线做判断
                        //if (Rowbg > RowEd)//从上往下 反向
                        //{
                        //    row = row.TupleSelectRange(row.Length / 2, row.Length - 1);
                        //    col = col.TupleSelectRange(col.Length / 2, col.Length - 1);
                        //}
                        //else
                        //{
                        //    row = row.TupleSelectRange(0, row.Length / 2);
                        //    col = col.TupleSelectRange(0, col.Length / 2);
                        //}

                        if (hwind != null && debug)
                        {
                            hwind.viewWindow.displayHobject(line, "red");
                        }
                        siglePart = line;
                    }
                    else
                    {
                        HObject ArcObj = new HObject();
                        HTuple tempR = new HTuple(row), tempC = new HTuple(col);
                        for (int n = 0; n < row.Length; n++)
                        {
                            if (n > 1)
                            {
                                double sub = (row[n].D - row[n - 1].D);
                                if (sub > 20)
                                {
                                    tempR[n] = tempR[n - 1].D + 1;
                                    tempC[n] = tempC[n - 1].D + 1;

                                }
                                if (sub < -20)
                                {
                                    tempR[n] = tempR[n - 1].D - 1;
                                    tempC[n] = tempC[n - 1].D + 1;

                                }
                            }
                        }
                        row = tempR; col = tempC;
                        HOperatorSet.GenContourPolygonXld(out ArcObj, row, col);

                        HOperatorSet.SmoothContoursXld(ArcObj, out ArcObj, 15);
                        if (fpTool.fParam[Sid].roiP[i].Xoffset2 != 0)
                        {
                            HTuple homMat;
                            HTuple deg1 = fpTool.fParam[Sid].roiP[i].Xoffset2;
                            HTuple Phi1 = deg1.TupleRad();
                            HOperatorSet.VectorAngleToRigid(row[0], col[0], 0, row[0], col[0], Phi1, out homMat);
                            HOperatorSet.AffineTransContourXld(ArcObj, out ArcObj, homMat);
                        }

                        HTuple Rb, Cb, Re, Ce, Nr1, Nc1, Ptorder;
                        HOperatorSet.FitLineContourXld(ArcObj, "tukey", -1, 0, 5, 2, out Rb, out Cb, out Re, out Ce, out Nr1, out Nc1, out Ptorder);

                        HOperatorSet.GetContourXld(ArcObj, out row, out col);

                        row = row + fpTool.fParam[Sid].roiP[i].offset;

                        col = col + fpTool.fParam[Sid].roiP[i].Xoffset;

                        if (hwind != null && debug)
                        {
                            hwind.viewWindow.displayHobject(ArcObj, "blue");
                        }
                        siglePart = ArcObj;
                    }




                    //加上 x y z 偏移
                    //row = row + fpTool.fParam[Sid].roiP[i].Yoffset;
                    //col = col + fpTool.fParam[Sid].roiP[i].Xoffset;


                    //旋转至原图
                    //HOperatorSet.AffineTransPoint2d(homMatRotateInvert, row, col, out row, out col);



                    HTuple AvraRow = new HTuple(), AvraCol = new HTuple(), AvraZ = new HTuple();
                    int PointCount = fpTool.fParam[Sid].roiP[i].NumOfSection;

                    //取Z值 
                    HTuple zcoord = new HTuple();
                    HOperatorSet.GetGrayval(HeightImage, row, col, out zcoord);


                    //除去 -100 的点
                    HTuple eq100 = new HTuple();
                    HOperatorSet.TupleLessElem(zcoord, -5, out eq100);
                    HTuple eq100Id = new HTuple();
                    HOperatorSet.TupleFind(eq100, 1, out eq100Id);
                    HTuple not5 = eq100.TupleFind(0);
                    HTuple Total = zcoord[not5];
                    HTuple Mean = Total.TupleMean();
                    if (eq100Id != -1)
                    {
                        for (int m = 0; m < eq100Id.Length; m++)
                        {
                            if (eq100Id[m] == 0)
                            {
                                HTuple meanZ = new HTuple();
                                meanZ = meanZ.TupleConcat(zcoord[eq100Id[m].D], zcoord[eq100Id[m].D + 1], zcoord[eq100Id[m].D + 2], zcoord[eq100Id[m].D + 3], zcoord[eq100Id[m].D + 4], zcoord[eq100Id[m].D + 5]);
                                HTuple eq30 = new HTuple();
                                HOperatorSet.TupleGreaterElem(meanZ, -5, out eq30);
                                HTuple eq30Id = new HTuple();
                                HOperatorSet.TupleFind(eq30, 1, out eq30Id);
                                if (eq30Id != -1)
                                {
                                    meanZ = meanZ[eq30Id];
                                    meanZ = meanZ.TupleMean();
                                    zcoord[eq100Id[m].D] = meanZ;
                                }
                                else
                                {
                                    meanZ = Mean;
                                    zcoord[eq100Id[m].D] = meanZ;
                                }

                            }
                            if (eq100Id[m] - 1 >= 0)
                            {
                                if (zcoord[eq100Id[m].D - 1].D < -5)
                                {
                                    zcoord[eq100Id[m].D] = Mean;
                                }
                                else
                                {
                                    zcoord[eq100Id[m].D] = zcoord[eq100Id[m].D - 1];
                                }

                            }
                            else
                            {
                                zcoord[eq100Id[m].D] = Mean;
                            }

                        }
                    }


                    //将行列Z 均分
                    double Div = row.Length / (double)((PointCount - 1));
                    if (Div < 1)
                    {
                        Div = 1;
                    }
                    for (int k = 0; k < PointCount; k++)
                    {
                        if (k == 0)
                        {
                            AvraRow = AvraRow.TupleConcat(row[0]);
                            AvraCol = AvraCol.TupleConcat(col[0]);
                            AvraZ = AvraZ.TupleConcat(zcoord[0]);
                            continue;
                        }

                        if (k == 529)
                        {
                            Debug.WriteLine("k" + k);
                        }
                        //Debug.WriteLine("k" + k);


                        int id = (int)(Div * k);
                        if (id >= row.Length)
                        {
                            break;
                        }
                        AvraRow = AvraRow.TupleConcat(row[id]);
                        AvraCol = AvraCol.TupleConcat(col[id]);
                        AvraZ = AvraZ.TupleConcat(zcoord[id]);
                    }
                    row = AvraRow;
                    col = AvraCol;
                    zcoord = AvraZ;


                    HTuple xc = new HTuple();
                    HOperatorSet.TupleGenSequence(0, zcoord.Length - 1, 1, out xc);
                    HObject Cont = new HObject();
                    HOperatorSet.GenContourPolygonXld(out Cont, zcoord, xc);
                    if (fpTool.fParam[Sid].roiP[i].LineOrCircle == "直线段")
                    {

                        HTuple Rowbg1, Colbg1, RowEd1, ColEd1, Nr1, Nc1, Dist1;
                        HOperatorSet.FitLineContourXld(Cont, "tukey", -1, 0, 5, 2, out Rowbg1, out Colbg1, out RowEd1, out ColEd1, out Nr1, out Nc1, out Dist1);

                        HTuple linephi1 = new HTuple();
                        HOperatorSet.LineOrientation(Rowbg1, Colbg1, RowEd1, ColEd1, out linephi1);
                        //HTuple deg = -(new HTuple(linephi1.D)).TupleDeg();
                        double tan1 = Math.Tan(-linephi1.D);

                        int len11 = Math.Abs((int)(Rowbg1.D - RowEd1.D));
                        int len21 = Math.Abs((int)(Colbg1.D - ColEd1.D));
                        int len0 = zcoord.Length;
                        HTuple x01 = Colbg1.D;
                        HTuple y01 = Rowbg1.D;
                        double[] newr1 = new double[len0]; double[] newc1 = new double[len0];
                        for (int m = 0; m < len0; m++)
                        {
                            HTuple row1 = new HTuple(); HTuple col1 = new HTuple();
                            if (len11 > len21)
                            {
                                row1 = Rowbg1.D > RowEd1.D ? y01 - m : y01 + m;
                                col1 = Rowbg1.D > RowEd1.D ? x01 - m / tan1 : x01 + m / tan1;

                            }
                            else
                            {
                                row1 = Colbg1.D > ColEd1.D ? y01 - m * tan1 : y01 + m * tan1;
                                col1 = Colbg1.D > ColEd1.D ? x01 - m : x01 + m;
                            }
                            newr1[m] = row1;
                            newc1[m] = col1;
                        }
                        zcoord = newr1; xc = newc1;

                    }
                    else
                    {
                        HOperatorSet.GenContourPolygonXld(out Cont, xc, zcoord);
                        HOperatorSet.SmoothContoursXld(Cont, out Cont, 15);
                        HOperatorSet.GetContourXld(Cont, out xc, out zcoord);
                    }


                    HTuple origRow = row;
                    HTuple origCol = col;

                    if (true)

                        #region 单边去重
                        if (i > 0)
                        {
                            HObject regXld = new HObject();
                            HOperatorSet.GenRegionContourXld(siglePart, out regXld, "filled");
                            HTuple phi;
                            HOperatorSet.RegionFeatures(regXld, "phi", out phi);
                            phi = phi.TupleDeg();
                            phi = phi.TupleAbs();
                            //HTuple last1 = RowCoordt[i - 1][0];//x1
                            //HTuple lastc1 = ColCoordt[i - 1][0];//x1
                            //HTuple sub1 = Math.Abs(row[0].D - last1.D);
                            //HTuple sub2 = Math.Abs(col[0].D - lastc1.D);
                            //HTuple pt1 = sub1.D > sub2.D ? RowCoordt[i - 1][RowCoordt[i - 1].Length - 1] : ColCoordt[i - 1][ColCoordt[i - 1].Length - 1];
                            HTuple pt1 = phi.D > 75 ? RowCoordt[i - 1][RowCoordt[i - 1].Length - 1] : ColCoordt[i - 1][ColCoordt[i - 1].Length - 1];
                            HTuple colbase = Sid == 0 || Sid == 2 ? col.TupleLessEqualElem(pt1) : col.TupleGreaterEqualElem(pt1);
                            HTuple Grater1 = phi.D > 75 ? row.TupleGreaterEqualElem(pt1) : colbase;

                            switch (Sid)
                            {
                                case 0: //x2>x1

                                    HTuple Grater1id = Grater1.TupleFind(1);
                                    row = row.TupleRemove(Grater1id);
                                    col = col.TupleRemove(Grater1id);
                                    zcoord = zcoord.TupleRemove(Grater1id);

                                    break;
                                case 1: //y2>y1

                                    HTuple Grater2id = Grater1.TupleFind(1);
                                    row = row.TupleRemove(Grater2id);
                                    col = col.TupleRemove(Grater2id);
                                    //origRow = origRow.TupleRemove(Grater2id);
                                    //origCol = origCol.TupleRemove(Grater2id);
                                    zcoord = zcoord.TupleRemove(Grater2id);
                                    break;
                                case 2: //x2<x1

                                    HTuple Grater3id = Grater1.TupleFind(1);
                                    row = row.TupleRemove(Grater3id);
                                    col = col.TupleRemove(Grater3id);
                                    //origRow = origRow.TupleRemove(Grater3id);
                                    //origCol = origCol.TupleRemove(Grater3id);
                                    zcoord = zcoord.TupleRemove(Grater3id);
                                    break;
                                case 3: //y2<y1

                                    HTuple Grater4id = Grater1.TupleFind(1);
                                    row = row.TupleRemove(Grater4id);
                                    col = col.TupleRemove(Grater4id);
                                    //origRow = origRow.TupleRemove(Grater4id);
                                    //origCol = origCol.TupleRemove(Grater4id);
                                    zcoord = zcoord.TupleRemove(Grater4id);
                                    break;
                            }
                        }
                    #endregion 单边去重


                    if (row.Length == 0)
                    {
                        return "单边设置 区域重复点数过大";
                    }

                    if (hwind != null && debug == false)
                    {
                        HObject NewSide = new HObject();
                        HOperatorSet.GenContourPolygonXld(out NewSide, row, col);
                        hwind.viewWindow.displayHobject(NewSide, "red");
                    }

                    RowCoordt[i] = row;
                    ColCoordt[i] = col;
                    //进行矩阵变换
                    if (HomMat3D != null)
                    {
                        //HOperatorSet.AffineTransPoint3d(HomMat3D, row, col,zcoord, out row, out col,out zcoord);
                        HOperatorSet.AffineTransPoint2d(HomMat3D, row, col, out row, out col);
                        //zcoord = zcoord + fpTool.fParam[Sid].roiP[i].Zoffset + fpTool.fParam[Sid].SigleZoffset + MyGlobal.globalConfig.TotalZoffset;
                    }

                    for (int n = 0; n < row.Length; n++)
                    {
                        Str.Append(row[n].D.ToString() + "," + col[n].D.ToString() + "," + zcoord[n].D.ToString() + "\r\n");
                    }
                    RowCoord[i] = row;
                    ColCoord[i] = col;
                    ZCoord[i] = zcoord;
                    StrLineOrCircle[i] = new string[zcoord.Length];


                    //ColCoordt[i] = col;
                    //ZCoordt[i] = zcoord;


                    StrLineOrCircle[i][0] = fpTool.fParam[Sid].roiP[i].LineOrCircle == "直线段" ? "1;" : "2;";
                    for (int n = 1; n < zcoord.Length; n++)
                    {

                        if (n == zcoord.Length - 1 && i == Len - 1 && Sid != 3) //最后一段 第四边不给
                        {
                            StrLineOrCircle[i][n] = StrLineOrCircle[i][0];
                        }
                        else
                        {
                            StrLineOrCircle[i][n] = "0;";

                        }
                    }
                    //if (hwind != null && debug == false)
                    //{
                    //    HObject NewSide = new HObject();
                    //    HOperatorSet.GenContourPolygonXld(out NewSide, row, col);
                    //    hwind.viewWindow.displayHobject(NewSide, "red");
                    //}

                }


                if (HomMat3D == null)
                {
                    StaticOperate.writeTxt("D:\\Laser3D.txt", Str.ToString());
                }

                //RIntesity.Dispose();
                //RHeight.Dispose();
                return "OK";
            }
            catch (Exception ex)
            {

                //RIntesity.Dispose();
                //RHeight.Dispose();
                return "FindPoint error:" + ex.Message;
            }
        }


        private void textBox_Num_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            //IntersetionCoord intersect = new IntersetionCoord();
            //string ok1 = fix.FixLine.FixLine(Intesity, Side, out intersect);
            //HTuple homMaxFix = new HTuple();
            //HOperatorSet.VectorAngleToRigid(fix.FixLine.intersectCoordList[Side - 1].Row, fix.FixLine.intersectCoordList[Side - 1].Col,
            //    0, intersect.Row, intersect.Col, 0, out homMaxFix);
            //string OK = flset.FindPoint(Side, Intesity, HeightImage, out X, out Y, out Z, out Str, Homat3D, Hwnd, false, homMaxFix);
            //return OK;
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {

        }

        private void 更改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
                if (dataGridView1.CurrentCell == null)
                {
                    return;
                }
                if (fpTool.roiList2[Id].Count > 0 && roiController2.ROIList.Count == fpTool.roiList2[Id].Count)
                {
                    int roiID = dataGridView1.CurrentCell.RowIndex;
                    roiID = CurrentRowIndex;
                    if (roiID < 0)
                    {
                        return;
                    }
                    if (roiID >= 0)
                    {

                        //fpTool.fParam[Id].roiP[roiId].LineOrCircle = fpTool.fParam[Id].roiP[roiId].LineOrCircle == "直线段" ? "圆弧段" : "直线段";
                        //comboBox2.SelectedItem = fpTool.fParam[Id].roiP[roiId].LineOrCircle;
                        switch (fpTool.fParam[Id].roiP[roiID].LineOrCircle)
                        {
                            case "直线段":
                                if (MessageBox.Show("更改为圆弧段(是)更改为连接段(否)", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    fpTool.fParam[Id].roiP[roiID].LineOrCircle = "圆弧段";
                                }
                                else
                                {
                                    fpTool.fParam[Id].roiP[roiID].LineOrCircle = "连接段";
                                }
                                break;
                            case "圆弧段":
                                if (MessageBox.Show("更改为直线段(是)更改为连接段(否)", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    fpTool.fParam[Id].roiP[roiID].LineOrCircle = "直线段";
                                }
                                else
                                {
                                    fpTool.fParam[Id].roiP[roiID].LineOrCircle = "连接段";
                                }
                                break;
                            case "连接段":
                                if (MessageBox.Show("更改为直线段(是)更改为圆弧段(否)", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    fpTool.fParam[Id].roiP[roiID].LineOrCircle = "直线段";
                                }
                                else
                                {
                                    fpTool.fParam[Id].roiP[roiID].LineOrCircle = "圆弧段";
                                }
                                break;
                        }
                        comboBox2.SelectedItem = fpTool.fParam[Id].roiP[roiID].LineOrCircle;
                        dataGridView1.Rows[roiID].Cells[2].Value = fpTool.fParam[Id].roiP[roiID].LineOrCircle;

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void FitLineSet_FormClosing(object sender, FormClosingEventArgs e)
        {

            //MyGlobal.Right_findPointTool_Find.Init("FitLineSet", isRight);
            //MyGlobal.Left_findPointTool_Find.Init("FitLineSet", isRight);
            //MyGlobal.Right_findPointTool_Fix.Init("Fix", isRight);
            //MyGlobal.Left_findPointTool_Fix.Init("Fix", isRight);

            //MyGlobal.flset2.Init();
            if (hwindow_final1.Image != null)
            {
                hwindow_final1.Image.Dispose();
                hwindow_final1.ClearWindow();
            }
            if (hwindow_final2.Image != null)
            {
                hwindow_final2.Image.Dispose();
                hwindow_final2.ClearWindow();
            }
 
            RGBImage.Dispose();
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = false;
            checkBoxRoi.Checked = false;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            isCloing = true;
            CurrentSide = "";
        }
        bool isInsert = false;
        private void 插入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isGenSection = false;
            if (dataGridView1.CurrentCell == null)
            {
                return;
            }
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;



            if (hwindow_final2.Image == null || !hwindow_final2.Image.IsInitialized())
            {
                return;
            }

            int currentId = dataGridView1.CurrentCell.RowIndex;
            currentId = CurrentRowIndex;
            if (currentId < 0)
            {
                return;
            }
            isInsert = true;
            HTuple coord1 = fpTool.roiList[Id][currentId].getModelData();
            HTuple coord2 = fpTool.roiList2[Id][currentId].getModelData();

            List<ROI> roiListTemp = new List<ROI>();
            List<ROI> roiListTemp1 = new List<ROI>();
            HWindow_Final tempWindow = new HWindow_Final();
            tempWindow.viewWindow.genRect2(coord2[0].D + 45, coord2[1].D + 45, coord2[2].D, coord2[3].D, coord2[4].D, ref roiListTemp);
            ROI temp2 = roiListTemp[0];
            tempWindow.viewWindow.genRect1(coord1[0].D, coord1[1].D, coord1[2].D, coord1[3].D, ref roiListTemp1);
            ROI rec = roiListTemp1[0];

            fpTool.roiList2[Id].Insert(currentId, temp2);
            fpTool.roiList[Id].Insert(currentId, rec);

            hwindow_final1.viewWindow.notDisplayRoi();
            hwindow_final2.viewWindow.notDisplayRoi();

            if (fpTool.roiList2[Id].Count > 0)
            {
                if (SelectAll)
                {
                    hwindow_final2.viewWindow.notDisplayRoi();
                    roiController2.viewController.ShowAllRoiModel = -1;
                    hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[Id]);
                }
                else
                {
                    hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[Id]);
                    roiController2.viewController.ShowAllRoiModel = currentId + 1;
                    roiController2.viewController.repaint(currentId + 1);
                }
                hwindow_final2.viewWindow.setActiveRoi(currentId + 1);
            }
            if (fpTool.roiList[Id].Count > 0)
            {
                temp.Clear();
                ROI roi = fpTool.roiList[Id][currentId + 1];
                temp.Add(roi);
                hwindow_final1.viewWindow.notDisplayRoi();
                hwindow_final1.viewWindow.displayROI(ref temp);
            }

            RoiParam RP = new RoiParam();
            RP = fpTool.fParam[Id].roiP[currentId].Clone();
            //新位置的
            HTuple NewCoord = temp2.getModelData();
            RP.CenterRow = NewCoord[0]; RP.CenterCol = NewCoord[1];
            fpTool.fParam[Id].roiP.Insert(currentId, RP);
            fpTool.fParam[Id].roiP[currentId].LineOrCircle = comboBox2.SelectedItem.ToString();
            //insert
            dataGridView1.Rows.Insert(currentId);
            dataGridView1.Rows[currentId].Cells[0].Value = currentId;
            dataGridView1.Rows[currentId].Cells[1].Value = "Default";
            dataGridView1.Rows[currentId].Cells[2].Value = fpTool.fParam[Id].roiP[currentId].LineOrCircle;
            fpTool.fParam[Id].DicPointName.Insert(currentId, "Default");
            //排序
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i;
            }

            //roiController2.setROIShape(new ROIRectangle2());

        }

        int PtOrder = -1;
        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            if (hwindow_final1.Image == null || !hwindow_final1.Image.IsInitialized())
            {
                return;
            }
            SimpleButton sbtn = (SimpleButton)sender;
            hwindow_final1.viewWindow.notDisplayRoi();
            switch (sbtn.Text)
            {
                case "偏移起点1":
                    PtOrder = 0;

                    roiController.setROIShape(new ROIPoint());
                    break;
                case "偏移终点1":
                    PtOrder = 1;
                    roiController.setROIShape(new ROIPoint());
                    break;
                case "偏移起点2":
                    PtOrder = 2;
                    roiController.setROIShape(new ROIPoint());
                    break;
                case "偏移终点2":
                    PtOrder = 3;
                    roiController.setROIShape(new ROIPoint());
                    break;
            }
        }

        bool edit = false;
        private void checkBoxRoi_CheckedChanged(object sender, EventArgs e)
        {
            edit = checkBoxRoi.Checked;
            if (edit)
            {
                hwindow_final1.viewWindow.setEditModel(true);
                hwindow_final2.viewWindow.setEditModel(true);
            }
            else
            {
                hwindow_final1.viewWindow.setEditModel(false);
                hwindow_final2.viewWindow.setEditModel(false);
            }
        }
        bool ReName = false;
        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ReName = true;
                //richTextBox1.ReadOnly = false;
                //int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
                //if (richTextBox1.SelectedText == "")
                //{
                //    return;
                //}
                //PreSelect = richTextBox1.SelectedText;
            }
            catch (Exception)
            {

                throw;
            }
        }


        bool isSelectOne = false;
        string PreSelect = "";

        private void textBox_OffsetX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (isLoading)
                {
                    return;
                }

                int SideId = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
                TextBox tb = (TextBox)sender;
                string index = tb.Text.ToString();
                //bool ok = Regex.IsMatch(index, @"(?i)^(\-[0-9]{1,}[.][0-9]*)+$") || Regex.IsMatch(index, @"(?i)^(\-[0-9]{1,}[0-9]*)+$") || Regex.IsMatch(index, @"(?i)^([0-9]{1,}[0-9]*)+$") || Regex.IsMatch(index, @"(?i)^(\[0-9]{1,}[0-9]*)+$");
                bool ok = Regex.IsMatch(index, @"^[-]?\d+[.]?\d*$");//是否为数字
                //bool ok = Regex.IsMatch(index, @"^([-]?)\d*$");//是否为整数
                if (!ok || RoiIsMoving)
                {
                    return;
                }
                double num = double.Parse(index);
                int roiID = dataGridView1.CurrentCell.RowIndex;
                roiID = CurrentRowIndex;
                if (roiID == -1)
                {
                    return;
                }
                int count = dataGridView1.SelectedCells.Count;
                List<int> rowInd = new List<int>();
                for (int i = 0; i < count; i++)
                {
                    int Ind = dataGridView1.SelectedCells[i].RowIndex;
                    if (!rowInd.Contains(Ind))
                    {
                        rowInd.Add(Ind);
                    }
                }
                for (int i = 0; i < rowInd.Count; i++)
                {
                    switch (tb.Name)
                    {
                        case "textBox_OffsetX":
                            fpTool.fParam[SideId].roiP[rowInd[i]].Xoffset = num;                          
                            break;
                        case "textBox_OffsetY":
                            
                            fpTool.fParam[SideId].roiP[rowInd[i]].Yoffset = num;          
                            break;
                        case "textBox_Offset":
                           
                            fpTool.fParam[SideId].roiP[rowInd[i]].offset = num;
                            break;
                        case "textBox_ZFtMax":
                            fpTool.fParam[SideId].roiP[rowInd[i]].ZftMax = (int)num;
                            break;
                        case "textBox_ZFtMin":
                            fpTool.fParam[SideId].roiP[rowInd[i]].ZftMin = (int)num;
                            break;
                        case "textBox_ZFtRad":
                            fpTool.fParam[SideId].roiP[rowInd[i]].ZftRad = num;
                            break;
                        case "textBox_downDist":
                            fpTool.fParam[SideId].roiP[rowInd[i]].TopDownDist = num;
                            break;
                        case "textBox_xDist":
                            fpTool.fParam[SideId].roiP[rowInd[i]].xDist = num;
                            break;
                        case "textBox_Clipping":
                            if (num > 50)
                            {
                                textBox_Clipping.Text = "50";
                                num = 50;
                            }
                            if (num < 0)
                            {
                                textBox_Clipping.Text = "0";
                                num = 0;
                            }
                            fpTool.fParam[SideId].roiP[rowInd[i]].ClippingPer = num;
                            break;

                        case "textBox_SmoothCont":
                            if (num > 1)
                            {
                                textBox_SmoothCont.Text = "1";
                                num = 50;
                            }
                            if (num < 0)
                            {
                                textBox_SmoothCont.Text = "0";
                                num = 0;
                            }
                            fpTool.fParam[SideId].roiP[rowInd[i]].SmoothCont = num;
                            break;
                        case "textBox_OffsetZ":
                            fpTool.fParam[SideId].roiP[rowInd[i]].Zoffset = num;
                            break;
                        case "textBox_IndStart1":
                            //fpTool.fParam[SideId].roiP[roiID].StartOffSet1 = (int)num;
                            break;
                        case "textBox_IndEnd1":
                            //fpTool.fParam[SideId].roiP[roiID].EndOffSet1 = (int)num;
                            break;
                        case "textBox_IndStart2":
                            //fpTool.fParam[SideId].roiP[roiID].StartOffSet2 = (int)num;
                            break;
                        case "textBox_IndEnd2":
                            //fpTool.fParam[SideId].roiP[roiID].EndOffSet2 = (int)num;
                            break;
                    }
                }
                
            }

            catch (Exception)
            {

                throw;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            NotUseFix = checkBox4.Checked;
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //int SideId = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            //if (PreSelect!="" && DicPointName[SideId].Keys.Contains(PreSelect))
            //{               
            //    richTextBox1.Focus();
            //    int id = DicPointName[SideId][PreSelect];
            //    int fId = richTextBox1.GetFirstCharIndexFromLine(id);
            //    richTextBox1.Select(fId, PreSelect.Length);
            //}

        }

        bool isSelecting = false;
        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentCell == null || RoiIsMoving || isSelecting)
            {
                return;
            }
            int roiID = dataGridView1.CurrentCell.RowIndex;
            if (roiID < 0)
            {
                return;
            }
            isSelecting = true;
            if (SelectAll)
            {

            }
            else
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[roiID].Selected = true;
            }

            isSelecting = false;
            try
            {
                isGenSection = false;
                RoiIsMoving = true;
                isSelectOne = false;
                int SideId = Convert.ToInt32(SideName.Substring(4, 1)) - 1;

                int id = roiID;

                //hwindow_final2.viewWindow.notDisplayRoi();
                if (fpTool.roiList2[SideId].Count > 0)
                {
                    if (SelectAll)
                    {
                        hwindow_final2.viewWindow.notDisplayRoi();
                        roiController2.viewController.ShowAllRoiModel = -1;
                        hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[SideId]);
                        hwindow_final2.viewWindow.selectROI(id);
                    }
                    else
                    {
                        if (roiController2.ROIList.Count != fpTool.roiList2[SideId].Count)
                        {
                            roiController2.viewController.ShowAllRoiModel = -1;
                            hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[SideId]);
                        }

                        roiController2.viewController.ShowAllRoiModel = id;
                        roiController2.viewController.repaint(id);
                    }

                    //if (SelectAll)
                    //{
                    //roiController2.viewController.repaint(id);

                    //}
                    //else
                    //{
                    //    tempList.Clear();
                    //    for (int i = 0; i < fpTool.roiList2[SideId].Count; i++)
                    //    {
                    //        ROI temp = (ROI)fpTool.roiList2[SideId][i];
                    //        tempList.Add(temp);
                    //    }
                    //    hwindow_final2.viewWindow.displayROI(ref tempList, id);
                    //}

                }
                else
                {
                    return;
                }


                textBox_Num.Text = fpTool.fParam[SideId].roiP[id].NumOfSection.ToString();
                HTuple[] lineCoord = new HTuple[1];
                //if (SelectAll)
                //{
                fpTool.DispSection((ROIRectangle2)fpTool.roiList2[SideId][id], SideId, id, out lineCoord, hwindow_final2);

                //}
                //else
                //{
                //    DispSection((ROIRectangle2)tempList[0], SideId, id, out lineCoord, hwindow_final2);
                //}
                int pID = 1;
                for (int i = 0; i < id; i++)
                {
                    for (int j = 0; j < fpTool.fParam[SideId].roiP[i].NumOfSection; j++)
                    {
                        pID++;
                    }
                }

                CurrentIndex = pID;
                CurrentRowIndex = roiID;
                textBox_Current.Text = CurrentIndex.ToString();
                HTuple row, col; HTuple anchor, anchorc;
                //fpTool.FindMaxPt(SideId + 1, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection);

                if (fpTool.fParam[SideId].roiP[CurrentRowIndex].SelectedType == 0)
                {
                    if (fpTool.fParam[SideId].roiP[CurrentRowIndex].TopDownDist != 0 && fpTool.fParam[SideId].roiP[CurrentRowIndex].xDist != 0)
                    {
                        fpTool.FindMaxPtFallDown(SideId + 1, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection,false,hwindow_final2);

                    }
                    else
                    {
                        fpTool.FindMaxPt(SideId + 1, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final2, ShowSection,false,hwindow_final1);

                    }
                }
                else
                {
                    fpTool.FindMaxPtFallDown(SideId + 1, CurrentIndex - 1, out row, out col, out anchor, out anchorc, hwindow_final1, ShowSection,false,hwindow_final2);
                }

                //string[] color = {"red","blue","green", "lime green", "black" };
                //if (row.Length != 0)
                //{
                //    //Random rad = new Random();
                //    //int i = rad.Next(4);
                //    PreSelect = dataGridView1.Rows[id].Cells[1].Value.ToString();
                //    hwindow_final2.viewWindow.dispMessage(PreSelect, "blue", row.D, col.D);
                //}



                //HTuple tempData = new HTuple();
                //List<ROI> temproi = new List<ROI>();
                //tempData = fpTool.roiList2[SideId][id].getModelData();

                //hwindow_final2.viewWindow.genRect2(tempData[0].D, tempData[1].D, tempData[2].D, fpTool.fParam[SideId].roiP[id].Len1, fpTool.fParam[SideId].roiP[id].Len2, ref temproi);
                //fpTool.roiList2[SideId][id] = temproi[0];
                //hwindow_final2.viewWindow.notDisplayRoi();
                //hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[SideId]);

                temp.Clear();
                ROI roi = fpTool.roiList[SideId][id];
                temp.Add(roi);
                hwindow_final1.viewWindow.notDisplayRoi();
                hwindow_final1.viewWindow.displayROI(ref temp);


                textBox_Num.Text = ((int)fpTool.fParam[SideId].roiP[id].NumOfSection).ToString();
                textBox_Len.Text = ((int)fpTool.fParam[SideId].roiP[id].Len1).ToString();
                textBox_Width.Text = ((int)fpTool.fParam[SideId].roiP[id].Len2).ToString();
                textBox_Row.Text = ((int)fpTool.fParam[SideId].roiP[id].CenterRow).ToString();
                textBox_Col.Text = ((int)fpTool.fParam[SideId].roiP[id].CenterCol).ToString();
                HTuple deg = new HTuple();
                HOperatorSet.TupleDeg(fpTool.fParam[SideId].roiP[id].phi, out deg);
                textBox_phi.Text = ((int)deg.D).ToString();
                textBox_Deg.Text = fpTool.fParam[SideId].roiP[id].AngleOfProfile.ToString();
                textBox_OffsetY.Text = fpTool.fParam[SideId].roiP[id].Yoffset.ToString();
                textBox_OffsetX.Text = fpTool.fParam[SideId].roiP[id].Xoffset.ToString();
                textBox_Offset.Text = fpTool.fParam[SideId].roiP[id].offset.ToString();

                textBox_OffsetZ.Text = fpTool.fParam[SideId].roiP[id].Zoffset.ToString();
                textBox_ZFtMax.Text = fpTool.fParam[SideId].roiP[id].ZftMax.ToString();
                textBox_ZFtMin.Text = fpTool.fParam[SideId].roiP[id].ZftMin.ToString();
                textBox_ZFtRad.Text = fpTool.fParam[SideId].roiP[id].ZftRad.ToString();
                textBox_Clipping.Text = fpTool.fParam[SideId].roiP[id].ClippingPer.ToString();
                textBox_SmoothCont.Text = fpTool.fParam[SideId].roiP[roiID].SmoothCont.ToString();
                //textBox_IndEnd2.Text = fpTool.fParam[SideId].roiP[id].EndOffSet2.ToString();

                comboBox2.SelectedItem = fpTool.fParam[SideId].roiP[id].LineOrCircle;
                //label_xoffset2.Text = fpTool.fParam[SideId].roiP[id].LineOrCircle == "圆弧段" ? "旋转角度" : "x终点偏移";
                //label20.Text = fpTool.fParam[SideId].roiP[id].LineOrCircle == "圆弧段" ? "度" : "pix";
                checkBox_useLeft.Checked = fpTool.fParam[SideId].roiP[id].useLeft;
                checkBox_center.Checked = fpTool.fParam[SideId].roiP[id].useCenter;

                checkBox_midPt.Checked = fpTool.fParam[SideId].roiP[id].useMidPt;
                checkBox_Far.Checked = !fpTool.fParam[SideId].roiP[id].useNear;
                textBox_downDist.Text = fpTool.fParam[SideId].roiP[id].TopDownDist.ToString();
                textBox_xDist.Text = fpTool.fParam[SideId].roiP[id].xDist.ToString();
                comboBox_GetPtType.SelectedIndex = fpTool.fParam[SideId].roiP[id].SelectedType;

                RoiIsMoving = false;
            }
            catch (Exception)
            {
                RoiIsMoving = false;
                //Debug.WriteLine(richTextBox1.SelectedText);
                throw;
            }
        }

        ROI CopyTemp;
        ROI CopyTemp2;
        int CopyId = -1;
        int CurrentRowIndex = -1;
        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isGenSection = false;
                if (dataGridView1.CurrentCell == null)
                {
                    return;
                }
                int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;

                if (hwindow_final2.Image == null || !hwindow_final2.Image.IsInitialized())
                {
                    return;
                }

                int currentId = CurrentRowIndex;
                if (currentId < 0)
                {
                    return;
                }

                HTuple coord1 = fpTool.roiList[Id][currentId].getModelData();
                HTuple coord2 = fpTool.roiList2[Id][currentId].getModelData();

                List<ROI> roiListTemp = new List<ROI>();
                List<ROI> roiListTemp1 = new List<ROI>();
                HWindow_Final tempWindow = new HWindow_Final();
                tempWindow.viewWindow.genRect2(coord2[0].D + 45, coord2[1].D + 45, coord2[2].D, coord2[3].D, coord2[4].D, ref roiListTemp);
                CopyTemp2 = roiListTemp[0];
                tempWindow.viewWindow.genRect1(coord1[0].D, coord1[1].D, coord1[2].D, coord1[3].D, ref roiListTemp1);
                CopyTemp = roiListTemp1[0];

                CopyId = currentId;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                isGenSection = false;
                if (CopyId == -1)
                {
                    return;
                }
                int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
                int currentId = dataGridView1.CurrentCell.RowIndex;
                currentId = CurrentRowIndex;
                //添加到当前Id之后
                fpTool.roiList2[Id].Insert(currentId + 1, CopyTemp2);
                fpTool.roiList[Id].Insert(currentId + 1, CopyTemp);

                hwindow_final1.viewWindow.notDisplayRoi();
                hwindow_final2.viewWindow.notDisplayRoi();
                if (fpTool.roiList2[Id].Count > 0)
                {
                    if (SelectAll)
                    {
                        //hwindow_final2.viewWindow.notDisplayRoi();
                        roiController2.viewController.ShowAllRoiModel = -1;
                        hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[Id]);
                    }
                    else
                    {
                        hwindow_final2.viewWindow.displayROI(ref fpTool.roiList2[Id]);
                        roiController2.viewController.ShowAllRoiModel = CopyId + 1;
                        roiController2.viewController.repaint(CopyId + 1);
                    }
                    hwindow_final2.viewWindow.setActiveRoi(currentId + 1);
                }
                if (fpTool.roiList[Id].Count > 0)
                {
                    temp.Clear();
                    ROI roi = fpTool.roiList[Id][CopyId + 1];
                    temp.Add(roi);
                    hwindow_final1.viewWindow.notDisplayRoi();
                    hwindow_final1.viewWindow.displayROI(ref temp);
                }

                RoiParam RP = new RoiParam();
                RP = fpTool.fParam[Id].roiP[CopyId].Clone();
                //新位置的
                HTuple NewCoord = CopyTemp2.getModelData();
                RP.CenterRow = NewCoord[0]; RP.CenterCol = NewCoord[1];
                fpTool.fParam[Id].roiP.Insert(currentId + 1, RP);
                fpTool.fParam[Id].roiP[currentId + 1].LineOrCircle = comboBox2.SelectedItem.ToString();
                //insert
                dataGridView1.Rows.Insert(currentId + 1);
                dataGridView1.Rows[currentId + 1].Cells[0].Value = currentId + 1;
                dataGridView1.Rows[currentId + 1].Cells[1].Value = "Default";
                dataGridView1.Rows[currentId + 1].Cells[2].Value = fpTool.fParam[Id].roiP[currentId + 1].LineOrCircle;
                fpTool.fParam[Id].DicPointName.Insert(currentId + 1, "Default");
                //排序
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = (i + 1);
                }
                //dataGridView1.ClearSelection();
                //dataGridView1.Rows[currentId + 1].Selected = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isLoading)
                {
                    return;
                }
                bool newID = dataGridView1.IsCurrentCellInEditMode;
                if (!newID)
                {
                    return;
                }
                int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
                int currentId = CurrentRowIndex;
                if (currentId != -1)
                {
                    string Value = dataGridView1.Rows[currentId].Cells[1].Value.ToString();
                    if (fpTool.fParam[Id].DicPointName.Contains(Value))
                    {
                        MessageBox.Show("ID已存在");
                        dataGridView1.Rows[currentId].Cells[1].Value = "Default";
                        return;
                    }
                    fpTool.fParam[Id].DicPointName[currentId] = dataGridView1.Rows[currentId].Cells[1].Value.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void comboBox_GetPtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
            {
                return;
            }
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            int currentId = CurrentRowIndex;

            int roiID = dataGridView1.CurrentCell.RowIndex;
            roiID = CurrentRowIndex;

            int count = dataGridView1.SelectedCells.Count;
            List<int> rowInd = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int Ind = dataGridView1.SelectedCells[i].RowIndex;
                if (!rowInd.Contains(Ind))
                {
                    rowInd.Add(Ind);
                }
            }
            for (int i = 0; i < rowInd.Count; i++)
            {
                currentId = rowInd[i];
                if (currentId != -1)
                {
                    fpTool.fParam[Id].roiP[currentId].SelectedType = comboBox_GetPtType.SelectedIndex;

                }
            }

           
        }

        private void checkBox_useLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading)
            {
                return;
            }
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            int currentId = CurrentRowIndex;
            int roiID = dataGridView1.CurrentCell.RowIndex;
            roiID = CurrentRowIndex;

            int count = dataGridView1.SelectedCells.Count;
            List<int> rowInd = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int Ind = dataGridView1.SelectedCells[i].RowIndex;
                if (!rowInd.Contains(Ind))
                {
                    rowInd.Add(Ind);
                }
            }
            for (int i = 0; i < rowInd.Count; i++)
            {
                currentId = rowInd[i];
                if (currentId != -1)
                {
                    fpTool.fParam[Id].roiP[currentId].useLeft = checkBox_useLeft.Checked;
                }
            }
        }

        private void checkBox_midPt_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading)
            {
                return;
            }
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            int currentId = CurrentRowIndex;
            int roiID = dataGridView1.CurrentCell.RowIndex;
            roiID = CurrentRowIndex;

            int count = dataGridView1.SelectedCells.Count;
            List<int> rowInd = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int Ind = dataGridView1.SelectedCells[i].RowIndex;
                if (!rowInd.Contains(Ind))
                {
                    rowInd.Add(Ind);
                }
            }
            for (int i = 0; i < rowInd.Count; i++)
            {
                currentId = rowInd[i];
                if (currentId != -1)
                {
                    fpTool.fParam[Id].roiP[currentId].useMidPt = checkBox_midPt.Checked;
                }
            }
        }

        private void checkBox_Near_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading)
            {
                return;
            }
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            int currentId = CurrentRowIndex;
            int roiID = dataGridView1.CurrentCell.RowIndex;
            roiID = CurrentRowIndex;

            int count = dataGridView1.SelectedCells.Count;
            List<int> rowInd = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int Ind = dataGridView1.SelectedCells[i].RowIndex;
                if (!rowInd.Contains(Ind))
                {
                    rowInd.Add(Ind);
                }
            }
            for (int i = 0; i < rowInd.Count; i++)
            {
                currentId = rowInd[i];
                if (currentId != -1)
                {
                    fpTool.fParam[Id].roiP[currentId].useNear = !checkBox_Far.Checked;
                }
            }
        }

        private void checkBox_center_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading)
            {
                return;
            }
            int Id = Convert.ToInt32(SideName.Substring(4, 1)) - 1;
            int currentId = CurrentRowIndex;

            int roiID = dataGridView1.CurrentCell.RowIndex;
            roiID = CurrentRowIndex;

            int count = dataGridView1.SelectedCells.Count;
            List<int> rowInd = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int Ind = dataGridView1.SelectedCells[i].RowIndex;
                if (!rowInd.Contains(Ind))
                {
                    rowInd.Add(Ind);
                }
            }
            for (int i = 0; i < rowInd.Count; i++)
            {
                currentId = rowInd[i];

                if (currentId != -1)
                {
                    fpTool.fParam[Id].roiP[currentId].useCenter = checkBox_center.Checked;
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading)
            {
                return;
            }
            if (comboBox3.SelectedItem.ToString() == "Right" )
            {
                isRight = true;
                comboBox3.BackColor = Color.LimeGreen;
            }
            else
            {
                isRight = false;
                comboBox3.BackColor = Color.Yellow;
            }
            LoadToUI();
            ChangeSide();
            MessageBox.Show("切换成功!");
        }
    }

    public class ParamPath
    {
        public static string ParaName = "";
        public static bool IsRight = false;
        public static string ParamDir
        {           
            get
            {
                if (IsRight)
                {
                    return MyGlobal.ConfigPath_Right + ParaName + "\\";
                }
                else
                {
                    return MyGlobal.ConfigPath_Left + ParaName + "\\";
                }
                //return Application.StartupPath + "\\Config" + "\\" + ParaName + "\\";
            }
        }
        public static string Path_Param
        {
            get { return ParamDir + "Fix.xml"; }
        }
        public static string Path_Setting
        {
            get { return ParamDir + "setting.xml"; }
        }
        public static string Path_roi
        {
            get { return ParamDir + "RoiLineCircle.roi"; }
        }
        public static string Path_tup
        {
            get { return ParamDir + "Calibrate" + ".tup"; }
        }

        public static void WriteTxt(string fileName, string value)
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
    [Serializable]
    public struct IntersetionCoord
    {
        public double Row;
        public double Col;
        public double Angle;
    }

    [Serializable]
    public class FitProfileParam
    {
        /// <summary>
        /// 选择最高点左侧
        /// </summary>
        public bool BeLeft = false;
        /// <summary>
        /// 有效轮廓起始点
        /// </summary>
        public int StartPt = 0;
        /// <summary>
        /// 有效轮廓结束点
        /// </summary>
        public int EndPt = 0;
        /// <summary>
        /// 最高点下降距离
        /// </summary>
        public double UpDownDist = 0;
        /// <summary>
        /// 单边高度偏移
        /// </summary>
        public double SigleZoffset = 0;
        /// <summary>
        /// 最小区间高度
        /// </summary>
        public double MinZ = 0;
        /// <summary>
        /// 最大区间高度
        /// </summary>
        public double MaxZ = 0.5;

        public List<RoiParam> roiP = new List<RoiParam>();
        public List<string> DicPointName = new List<string>();
        /// <summary>
        /// 使用定位
        /// </summary>
        public bool UseFix = false;
    }

    [Serializable]
    public class RoiParam
    {
        /// <summary>
        /// 矩形区域截面数量
        /// </summary>
        public int NumOfSection = 2;
        /// <summary>
        /// 矩形长轴
        /// </summary>
        public double Len1 = 100;
        /// <summary>
        /// 矩形短轴
        /// </summary>
        public double Len2 = 50;
        /// <summary>
        /// 矩形中心行坐标
        /// </summary>
        public double CenterRow = 0;
        /// <summary>
        /// 矩形中心列坐标
        /// </summary>
        public double CenterCol = 0;
        /// <summary>
        /// 矩形角度
        /// </summary>
        public double phi = 0;
        /// <summary>
        /// 轮廓旋转角度
        /// </summary>
        public int AngleOfProfile = -20;

        public double AnchorRow = 0;
        public double AnchorCol = 0;
        /// <summary>
        /// 选择直线段 还是圆弧段 连接段
        /// </summary>
        public string LineOrCircle = "直线段";
        public double Xoffset = 0;
        public double Yoffset = 0;
        public double offset = 0;
        public double Zoffset = 0;
        public double Xoffset2 = 0;
        public double Yoffset2 = 0;
        public Point StartOffSet1 = new Point(0, 0);//拟合直线相对锚定点偏移
        public Point EndOffSet1 = new Point(0, 0);
        public Point StartOffSet2 = new Point(0, 0);
        public Point EndOffSet2 = new Point(0, 0);
        public int ZftMax = 0;
        public int ZftMin = 0;
        public double ZftRad = 0;
        public double TopDownDist = 0;
        public double xDist = 0;
        public double ClippingPer = 0;//忽略点百分比
        public double SmoothCont = 0;//滤波系数
        /// <summary>
        ///  0 极值 1 最高点下降
        /// </summary>
        public int SelectedType = 0;
        /// <summary>
        /// 取左侧值
        /// </summary>
        public bool useLeft = true;
        /// <summary>
        /// 取中间点
        /// </summary>
        public bool useMidPt = false;
        /// <summary>
        /// 取最近点还是最远点
        /// </summary>
        public bool useNear = false;
        /// <summary>
        /// 圆弧处取区域中心点
        /// </summary>
        public bool useCenter = false;

        public RoiParam Clone()
        {
            RoiParam temp = new RoiParam();
            temp = (RoiParam)this.MemberwiseClone();
            return temp;
        }
    }
}