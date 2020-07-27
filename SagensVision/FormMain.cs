using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.LookAndFeel;
using HalconDotNet;
using ChoiceTech.Halcon.Control;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using SagensVision.VisionTool;
using SagensSdk;
using System.Runtime.InteropServices;
using DevExpress.XtraEditors.Repository;
using SagensVision.UserLoginIn;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;

namespace SagensVision
{
    public partial class FormMain : DevExpress.XtraEditors.XtraForm
    { 
        public static List<double[][]> XCoord = new List<double[][]>();
        public static List<double[][]> YCoord = new List<double[][]>();
        public static List<double[][]> ZCoord = new List<double[][]>();
        public static List<string[][]> StrLorC = new List<string[][]>();
        public static List<double[]> Xorigin = new List<double[]>();

        public static List<double[]> Yorigin = new List<double[]>();

        public static List<double[]> Zorigin = new List<double[]>();

        public static List<string[]> NameOrigin = new List<string[]>();

        public static List<IntersetionCoord> AnchorList = new List<IntersetionCoord>();

        private static StringBuilder errstr = new StringBuilder();
        bool isLoading = false;
        public FormMain()
        {
            InitializeComponent();
            Init();
            // Handling the QueryControl event that will populate all automatically generated Documents     
        }
        void LoadConfig()
        {
            try
            {
                if (File.Exists(MyGlobal.AllTypePath + "Global.xml"))
                {
                    MyGlobal.globalConfig = (GlobalConfig)StaticOperate.ReadXML(MyGlobal.AllTypePath + "Global.xml", MyGlobal.globalConfig.GetType());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        private HWindowControl ShowProfile = new HWindowControl();
        void Init()
        {
            MyGlobal.GoSDK.SaveKdatDirectoy = "SaveKdatDirectoy//";
            if (!Directory.Exists(MyGlobal.GoSDK.SaveKdatDirectoy))
            {
                Directory.CreateDirectory(MyGlobal.GoSDK.SaveKdatDirectoy);
            }
            if (!Directory.Exists(MyGlobal.SaveDatFileDirectory))
            {
                Directory.CreateDirectory(MyGlobal.SaveDatFileDirectory);
            }
            if (!Directory.Exists(MyGlobal.DataPath))
            {
                Directory.CreateDirectory(MyGlobal.DataPath);
            }
            if (!Directory.Exists(MyGlobal.AllTypePath))
            {
                Directory.CreateDirectory(MyGlobal.AllTypePath);
            }
            if (File.Exists(MyGlobal.AllTypePath + "AllType.xml"))
            {
                MyGlobal.PathName = (SavePathName)StaticOperate.ReadXML(MyGlobal.AllTypePath + "AllType.xml", typeof(SavePathName));
            }
            if (MyGlobal.PathName.CurrentType != "")
            {
                //读取Z值基准高度】
                if (File.Exists(MyGlobal.BaseTxtPath_Right))
                {
                    MyGlobal.xyzBaseCoord_Right = (XYZBaseCoord)StaticOperate.ReadXML(MyGlobal.BaseTxtPath_Right, typeof(XYZBaseCoord));
                }
                else
                {
                    ShowAndSaveMsg("右工位基准值配置文件加载失败!");
                }
                if (File.Exists(MyGlobal.BaseTxtPath_Left))
                {
                    MyGlobal.xyzBaseCoord_Left = (XYZBaseCoord)StaticOperate.ReadXML(MyGlobal.BaseTxtPath_Left, typeof(XYZBaseCoord));
                }
                else
                {
                    ShowAndSaveMsg("左工位基准值配置文件加载失败!");
                }

            }
            else
            {
                ShowAndSaveMsg("基准值配置文件加载失败!");
            }

            string ok1 = MyGlobal.Right_findPointTool_Find.Init(MyGlobal.FindPointType_FitLineSet, true, MyGlobal.ToolType_GlueGuide);
            if (ok1 != "OK")
            {
                ShowAndSaveMsg("右工位引导-" + ok1);
            }
            else
            {
                ShowAndSaveMsg("右工位引导-参数" + ok1);
            }
            string ok2 = MyGlobal.Right_findPointTool_Fix.Init(MyGlobal.FindPointType_Fix, true, MyGlobal.ToolType_GlueGuide);
            if (ok2 != "OK")
            {
                ShowAndSaveMsg("右工位引导-" + ok2);
            }
            else
            {
                ShowAndSaveMsg("右工位引导-定位参数" + ok2);
            }

            string ok3 = MyGlobal.Left_findPointTool_Find.Init(MyGlobal.FindPointType_FitLineSet, false, MyGlobal.ToolType_GlueGuide);
            if (ok3 != "OK")
            {
                ShowAndSaveMsg("左工位引导-" + ok3);
            }
            else
            {
                ShowAndSaveMsg("左工位引导-参数" + ok3);
            }
            string ok4 = MyGlobal.Left_findPointTool_Fix.Init(MyGlobal.FindPointType_Fix, false, MyGlobal.ToolType_GlueGuide);
            if (ok4 != "OK")
            {
                ShowAndSaveMsg("左工位引导-" + ok4);
            }
            else
            {
                ShowAndSaveMsg("左工位引导-定位参数" + ok4);
            }
            string ok5 = MyGlobal.Left_Calib_Fix.Init(MyGlobal.FindPointType_Fix, false, MyGlobal.ToolType_Calib);
            if (ok5 != "OK")
            {
                ShowAndSaveMsg("左工位标定-" + ok5);
            }
            else
            {
                ShowAndSaveMsg("左工位标定-定位参数" + ok5);
            }
            string ok6 = MyGlobal.Right_Calib_Fix.Init(MyGlobal.FindPointType_Fix, true, MyGlobal.ToolType_Calib);
            if (ok6 != "OK")
            {
                ShowAndSaveMsg("右工位标定-" + ok6);
            }
            else
            {
                ShowAndSaveMsg("右工位标定-定位参数" + ok6);
            }

            //string dbcreate = SQLiteHelper.NewDbFile();
            //if (dbcreate == "OK")
            //{

            //    ShowAndSaveMsg("数据库创建成功!");
            //}
            //else
            //{
            //    ShowAndSaveMsg("数据库创建失败!" + dbcreate);
            //}
            //LoadDataDB();

            if (File.Exists(MyGlobal.AllTypePath + "Global.xml"))
            {
                MyGlobal.globalConfig = (GlobalConfig)StaticOperate.ReadXML(MyGlobal.AllTypePath + "Global.xml", MyGlobal.globalConfig.GetType());
            }
            else
            {
                ShowAndSaveMsg("通信参数加载失败!");
            }
            if (File.Exists(MyGlobal.AllTypePath + "GlobalPoint_Right.xml"))
            {
                MyGlobal.globalPointSet_Right = (GlobalPointSet)StaticOperate.ReadXML(MyGlobal.AllTypePath + "GlobalPoint_Right.xml", typeof(GlobalPointSet));
            }
            else
            {
                ShowAndSaveMsg("右工位全局参数加载失败!");

            }

            if (File.Exists(MyGlobal.AllTypePath + "GlobalPoint_Left.xml"))
            {
                MyGlobal.globalPointSet_Left = (GlobalPointSet)StaticOperate.ReadXML(MyGlobal.AllTypePath + "GlobalPoint_Left.xml", typeof(GlobalPointSet));

            }
            else
            {
                ShowAndSaveMsg("左工位全局参数加载失败!");
            }

            for (int i = 0; i < 4; i++)
            {
                Calibration.ParamPath.ParaName = SideName[i];
                Calibration.ParamPath.LeftOrRight = "Right";
                if (File.Exists(Calibration.ParamPath.Path_tup))
                {
                    HOperatorSet.ReadTuple(Calibration.ParamPath.Path_tup, out MyGlobal.HomMat3D_Right[i]);
                }
                else
                {
                    ShowAndSaveMsg("右工位标定文件加载失败!");
                    //break;
                }
                Calibration.ParamPath.LeftOrRight = "Left";
                if (File.Exists(Calibration.ParamPath.Path_tup))
                {
                    HOperatorSet.ReadTuple(Calibration.ParamPath.Path_tup, out MyGlobal.HomMat3D_Left[i]);
                }
                else
                {
                    ShowAndSaveMsg("左工位标定文件加载失败!");
                    //break;
                }

            }
            if (MyGlobal.PathName.CurrentType.Contains("_SurfaceCurvature"))
            {
                MyGlobal.globalConfig.enableAlign = true;
            }
            else
            {
                MyGlobal.globalConfig.enableAlign = false;
            }
            //match.load = new Matching.Form1.LoadParam(loadMathParam);
            //match2.load = new Matching.Form1.LoadParam(loadMathParam);
            //OffFram.Run = new OfflineFrm.RunOff(RunOffline);
            gbParamSet.Run = new VisionTool.GlobalParam.RunOff(RunBaseHeight);
        }

        void InitControl()
        {
            //loadMathParam();
            for (int i = 0; i < 4; i++)
            {
                MyGlobal.hWindow_Final[i] = new HWindow_Final();
                MyGlobal.hWindow_Final[i].Name = $"hwindow_final{i + 1}";
                MyGlobal.hWindow_Final[i].viewWindow.setFitWindow(true);
                //MyGlobal.parameterSet[i] = new MatchingModule.MatchingParam();

                //FindMax[i] = new VisionTool.FindMax(i + 1);
                //FindMax[i].inParam = FindMax[i].LoadXml(i + 1);
                //FindMax[i].roiList= FindMax[i].LoadROI(i + 1);
            }

            //dockPanel3.Controls.Add(MyGlobal.hWindow_Final[0]);

            MyGlobal.hWindow_Final[0].Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Controls.Add(MyGlobal.hWindow_Final[0], 0, 1);
            MyGlobal.hWindow_Final[1].Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Controls.Add(MyGlobal.hWindow_Final[1], 1, 1);
            MyGlobal.hWindow_Final[2].Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Controls.Add(MyGlobal.hWindow_Final[2], 3, 1);
            MyGlobal.hWindow_Final[3].Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Controls.Add(MyGlobal.hWindow_Final[3], 4, 1);

            MyGlobal.hWindow_Final[0].hWindowControl.HMouseUp += OnHMouseUp;
            MyGlobal.hWindow_Final[1].hWindowControl.HMouseUp += OnHMouseUp1;
            MyGlobal.hWindow_Final[2].hWindowControl.HMouseUp += OnHMouseUp2;
            MyGlobal.hWindow_Final[3].hWindowControl.HMouseUp += OnHMouseUp3;

            ShowProfile.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Controls.Add(ShowProfile, 2, 1);
            //ShowProfile.Show3dTrackDel += Show3dPointFrm;
            ShowProfile.ContextMenuStrip = contextMenuStrip1;
            MyGlobal.hWindow_Final[0].Show3dTrackDel += Show3dImg;
            MyGlobal.hWindow_Final[1].Show3dTrackDel += Show3dImg;
            MyGlobal.hWindow_Final[2].Show3dTrackDel += Show3dImg;
            MyGlobal.hWindow_Final[3].Show3dTrackDel += Show3dImg;

            ShowProfile.HMouseDown += ShowProfile_HMouseDown;
        }

        private void ShowProfile_HMouseDown(object sender, HMouseEventArgs e)
        {
            //if (ClassShow3D.breakOut == false)
            //{
            //    return;
            //}
            //ShowProfileToWindow(this.recordXCoord, this.recordYCoord, this.recordZCoord, this.recordSigleTitle, false, true);
        }







        private void Show3dImg(HWindow_Final obj)
        {
            //string hwind_id = obj.Name.Substring(obj.Name.Length - 1, 1);
            //int hwind_idx = int.Parse(hwind_id) - 1;
            //HObject img = MyGlobal.ImageMulti[hwind_idx][1];
            //HTuple pointer, type, w, h;
            //HOperatorSet.GetImagePointer1(img, out pointer, out type, out w, out h);
            //float[] z = new float[w * h];
            //Marshal.Copy(pointer, z, 0, w * h);
            //Show3dImgFrm sif = new Show3dImgFrm(z, w, h);
            //sif.Show();
        }


        #region 窗口双击放大
        private int MouseClickCnt = 0;
        private int MouseClickCnt1 = 0;
        private int MouseClickCnt2 = 0;
        private int MouseClickCnt3 = 0;



        private void OnHMouseUp(object sender, EventArgs e)
        {
            MouseClickCnt++;
            if (MouseClickCnt == 2)
            {
                ShowEnlargeFrm(0);
                MouseClickCnt = 0;
            }
        }
        private void OnHMouseUp1(object sender, EventArgs e)
        {
            MouseClickCnt1++;
            if (MouseClickCnt1 == 2)
            {
                ShowEnlargeFrm(1);
                MouseClickCnt1 = 0;
            }
        }
        private void OnHMouseUp2(object sender, EventArgs e)
        {
            MouseClickCnt2++;
            if (MouseClickCnt2 == 2)
            {
                ShowEnlargeFrm(2);
                MouseClickCnt2 = 0;
            }
        }
        private void OnHMouseUp3(object sender, EventArgs e)
        {
            MouseClickCnt3++;
            if (MouseClickCnt3 == 2)
            {
                ShowEnlargeFrm(3);
                MouseClickCnt3 = 0;
            }
        }
        public void ShowEnlargeFrm(int idx)
        {
            if (MyGlobal.globalConfig.enableFeature)
            {
                EnlargeFrm enlargefrm = new EnlargeFrm(MyGlobal.hWindow_Final[idx].Image, idx);
                enlargefrm.Show();
            }
            
        }

        private void 点位详情ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (recordXCoord == null || recordXCoord.Length == 0)
            {
                return;
            }
            if (recordYCoord == null || recordYCoord.Length == 0)
            {
                return;
            }
            if (recordZCoord == null || recordZCoord.Length == 0)
            {
                return;
            }
            if (recordXCoord.Length != recordYCoord.Length || recordXCoord.Length != recordZCoord.Length)
            {
                return;
            }
            Show3dPointFrm spf = new Show3dPointFrm(recordXCoord, recordYCoord, recordZCoord, recordSigleTitle);
            spf.Show();
        }
        private void Show3dPointFrm(HWindow_Final hwindowfinal)
        {
            //if (recordXCoord == null || recordXCoord.Length == 0)
            //{
            //    return;
            //}
            //if (recordYCoord == null || recordYCoord.Length == 0)
            //{
            //    return;
            //}
            //if (recordZCoord == null || recordZCoord.Length == 0)
            //{
            //    return;
            //}
            //if (recordXCoord.Length != recordYCoord.Length || recordXCoord.Length != recordZCoord.Length)
            //{
            //    return;
            //}
            //Show3dPointFrm spf = new Show3dPointFrm(recordXCoord, recordYCoord, recordZCoord);
            //spf.Show();
        }
        #endregion

        //void LoadDataDB()
        //{
        //    if (SQLiteHelper.OpenDb() == "OK")
        //    {
        //        DataTable dt = SQLiteHelper.GetSchema();
        //        string[] tableNames = SQLiteHelper.GetTableName().Trim().Split(',');
        //        int len = tableNames.Length;
        //        string tabelname = "";
        //        if (len > 1)
        //        {
        //            tabelname = tableNames[len - 2];
        //            if (tabelname != "")
        //            {
        //                SQLiteHelper.OpenDb();
        //                DataSet Ds = new DataSet();
        //                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("select * from " + "[" + tabelname + "]", SQLiteHelper._SQLiteConn);
        //                dataAdapter.Fill(Ds);
        //                int rCount = Ds.Tables[0].Rows.Count;
        //                if (rCount > 1)
        //                {
        //                    DataTable dt1 = Ds.Tables[0];

        //                    total = (double)dt1.Rows[rCount - 1].ItemArray[6];
        //                    Per = (double)dt1.Rows[rCount - 1].ItemArray[5];
        //                    ng = total - total * Per / 100;
        //                }

        //                SQLiteHelper.CloseDb();
        //            }

        //        }

        //    }
        //}

        string RunFindPoint(int Side, bool isRight, HObject Intesity, HObject HeightImage, out double[][] X, out double[][] Y, out double[][] Z, out string[][] Str, out HTuple[] original,HWindow_Final Hwnd, HObject OriginImage = null)
        {
            X = null; Y = null; Z = null; Str = null; original = new HTuple[2];
            try
            {
                IntersetionCoord intersect = new IntersetionCoord();
                string ok1 = "";
                if (isRight)
                {
                    ok1 = MyGlobal.Right_findPointTool_Fix.FindIntersectPoint(Side, HeightImage, out intersect, Hwnd, false,MyGlobal.globalConfig.enableFeature);
                }
                else
                {
                    ok1 = MyGlobal.Left_findPointTool_Fix.FindIntersectPoint(Side, HeightImage, out intersect, Hwnd, false, MyGlobal.globalConfig.enableFeature);
                }

                if (ok1 != "OK")
                {
                    errstr.Append($"<side{Side}:-定位-{ok1}>");
                    return ok1;
                }
                AnchorList.Add(intersect);
                HTuple homMaxFix = new HTuple();
                string OK = "";
                double orignalDeg = 0;
                double currentDeg = intersect.Angle;
               
                if (isRight)
                {
                    orignalDeg = MyGlobal.Right_findPointTool_Fix.intersectCoordList[Side - 1].Angle;
                    HOperatorSet.VectorAngleToRigid(MyGlobal.Right_findPointTool_Fix.intersectCoordList[Side - 1].Row, MyGlobal.Right_findPointTool_Fix.intersectCoordList[Side - 1].Col,
                    orignalDeg, intersect.Row, intersect.Col, currentDeg, out homMaxFix);
                    if (!MyGlobal.globalConfig.isUseFix)
                    {
                        homMaxFix = null;
                    }
                    OK = MyGlobal.Right_findPointTool_Find.FindPoint(Side, isRight, Intesity, HeightImage, out X, out Y, out Z, out Str, out original, MyGlobal.HomMat3D_Right[Side -1], Hwnd, false, homMaxFix, OriginImage, MyGlobal.globalConfig.enableFeature);

                }
                else
                {
                    orignalDeg = MyGlobal.Left_findPointTool_Fix.intersectCoordList[Side - 1].Angle;
                    HOperatorSet.VectorAngleToRigid(MyGlobal.Left_findPointTool_Fix.intersectCoordList[Side - 1].Row, MyGlobal.Left_findPointTool_Fix.intersectCoordList[Side - 1].Col,
                    orignalDeg, intersect.Row, intersect.Col, currentDeg, out homMaxFix);
                    if (!MyGlobal.globalConfig.isUseFix)
                    {
                        homMaxFix = null;
                }
                    OK = MyGlobal.Left_findPointTool_Find.FindPoint(Side, isRight, Intesity, HeightImage, out X, out Y, out Z, out Str, out original, MyGlobal.HomMat3D_Left[Side - 1], Hwnd, false, homMaxFix, OriginImage, MyGlobal.globalConfig.enableFeature);
                }


                if (OK != "OK")
                {
                    if (OK.Contains("高度超出范围"))
                        errstr.Append($"<side{Side}:-探高-{OK}>");
                    else
                        errstr.Append($"<side{Side}:-抓边-{OK}>");
                    return OK;
                }


                return OK;
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                errstr.Append($"<side{Side}:-RunFindPoint-{ex.Message}>" + RowNum);
                return "RunFindPoint Error :" + ex.Message + RowNum;
            }
        }



        #region 窗口按钮

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //dockPanel5.Show();
            //dockPanel4.Show();
            //dockPanel3.Show();
            //dockPanel5.DockedAsTabbedDocument = true;
            //dockPanel4.DockedAsTabbedDocument = true;
            //dockPanel3.DockedAsTabbedDocument = true;
            dockPanel8.Show();
            dockPanel8.DockedAsTabbedDocument = true;
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel1.Show();
            dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            dockPanel7.Show();
            dockPanel7.DockTo(dockPanel1, DevExpress.XtraBars.Docking.DockingStyle.Bottom);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel2.Show();
            dockPanel2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel1.Show();
            dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            dockPanel8.Show();
            dockPanel8.DockedAsTabbedDocument = true;
            dockPanel7.Show();
            dockPanel7.DockTo(dockPanel1, DevExpress.XtraBars.Docking.DockingStyle.Bottom);
            dockPanel2.Show();
            dockPanel2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;

            bar2.Reset();
            bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;

        }

        private void navBarItem2_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            UserLoginIn.UserLogin user = new UserLoginIn.UserLogin();
            user.Show();
        }

        #endregion
        public void ShowAndSaveMsg(string msg)
        {
            Action<string> fp = (string msg1) =>
            {
                if (this.textBox1.Lines.Length > 300)
                {
                    int index = this.textBox1.Text.IndexOf(Environment.NewLine);
                    this.textBox1.Text = this.textBox1.Text.Remove(0, index + Environment.NewLine.Length);
                }
                string formatStr = "HH:mm:ss:ffff";

                string longMsg = string.Format("[{0}]", DateTime.Now.ToString(formatStr)) + msg1;

                this.textBox1.AppendText(Environment.NewLine + longMsg);
                this.textBox1.Select(this.textBox1.Text.Length, 0);
                this.textBox1.ScrollToCaret();
                this.textBox1.ScrollBars = ScrollBars.Both;
                //Misc.SaveLog(longMsg);
                StaticOperate.SaveLog(longMsg);
            };
            if (this.InvokeRequired)
            {
                this.Invoke(fp, msg);
            }
            else
            {
                fp(msg);
            }
        }
        public void ShowAndSaveErrorMsg(string msg, string date)
        {
            Action<string> fp = (string msg1) =>
            {
                string longMsg = string.Format("运行时间：[{0}]  图像路径： [{1}]   错误消息： [{2}]\r\n", DateTime.Now.ToString("hh:mm:ss:ff"), date, msg1);

                //Misc.SaveLog(longMsg);
                StaticOperate.SaveErrorLog(longMsg);
            };
            if (this.InvokeRequired)
            {
                this.Invoke(fp, msg);
            }
            else
            {
                fp(msg);
            }
        }

        void DispBar( bool Disp)
        {
            bar2.Visible = Disp;
            navBarGroup1.Visible = Disp;
            navBarGroup3.Visible = Disp;
            if (!MyGlobal.globalConfig.enableFeature)
            {
                this.Icon = null;
                pictureBox2.Visible = false;
            }
        }
       
        VisionTool.Display3D show3D = new VisionTool.Display3D();
        private void FormMain_Load(object sender, EventArgs e)
        {

            DispBar(false);
            isLoading = true;
            InitControl();
            //MyGlobal.thdWaitForClientAndMessage = new Thread(TcpClientListen);
            //MyGlobal.thdWaitForClientAndMessage = new Thread(TcpClientListen_Surface);
            MyGlobal.thdWaitForClientAndMessage = new Thread(Listen_Surface);
            MyGlobal.thdWaitForClientAndMessage.IsBackground = true;

            MyGlobal.thdWaitForClientAndMessage.Name = "以太网通信线程";
            MyGlobal.thdWaitForClientAndMessage.Start();
            //string sktMsg = "";
            //MyGlobal.sktOK =StaticOperate.CreateServer(ref sktMsg);
            //if (!MyGlobal.sktOK)
            //{
            //    ShowAndSaveMsg(sktMsg);
            //}
            MyGlobal.GoSDK.isStartCheck = false;
            MyGlobal.GoSDK.expirationTime = "2020-03-30 18:00:00";
            string Msg = "";
            if (MyGlobal.GoSDK.connect(MyGlobal.globalConfig.SensorIP, ref Msg))
            {
                MyGlobal.SensorConnected = true;
                ShowAndSaveMsg("Sensor连接成功！");
                //MyGlobal.globalConfig.dataContext.serialNumber = MyGlobal.GoSDK.context.serialNumber;
                //MyGlobal.globalConfig.dataContext.xOffset = MyGlobal.GoSDK.context.xOffset;
                //MyGlobal.globalConfig.dataContext.yOffset = MyGlobal.GoSDK.context.yOffset;
                //MyGlobal.globalConfig.dataContext.zOffset = MyGlobal.GoSDK.context.zOffset;
                ////MyGlobal.globalConfig.dataContext.xResolution = MyGlobal.GoSDK.context.xResolution;
                ////MyGlobal.globalConfig.dataContext.yResolution = MyGlobal.GoSDK.context.yResolution;
                ////MyGlobal.globalConfig.dataContext.zResolution = MyGlobal.GoSDK.context.zResolution;


                //MyGlobal.globalConfig.dataContext.xResolution = MyGlobal.GoSDK.context.xResolution / 1;
                //MyGlobal.globalConfig.dataContext.yResolution = MyGlobal.GoSDK.context.yResolution / 4;

                if (!SecretKey.License.SnOk)
                {
                    ShowAndSaveMsg("Sn Fail！");
                }
                MyGlobal.globalConfig.zStart = MyGlobal.GoSDK.zStart;
                MyGlobal.globalConfig.zRange = MyGlobal.GoSDK.zRange;
                //StaticOperate.WriteXML(MyGlobal.globalConfig, MyGlobal.ConfigPath + "Global.xml");
            }
            else
            {
                MyGlobal.SensorConnected = false;
                ShowAndSaveMsg("Sensor连接失败！" + Msg);

            }

            //MyGlobal.GoSDK.SurfaceZRecFinish += GoSDK_SurfaceZRecFinish;
            //MyGlobal.GoSDK.SurfaceIntensityRecFinish += GoSDK_SurfaceIntensityFinish;
            cmu.Conn = ConnectTcp;
            NewProduct.AddToFormain += ChangeBarEditValue;
            VisionTool.GlobalParam.ChangeType += ChangeBarEditValue;
            //加载config
            DirectoryInfo dirinf = new DirectoryInfo(MyGlobal.AllTypePath);
            DirectoryInfo[] dirinfo = dirinf.GetDirectories();
            RepositoryItemComboBox q = (RepositoryItemComboBox)barEditItem_CurrentType.Edit;
            for (int i = 0; i < dirinfo.Length; i++)
            {
                q.Items.Add(dirinfo[i].Name);
            }
            barEditItem_CurrentType.EditValue = MyGlobal.PathName.CurrentType;
            isLoading = false;
            setValue(true);
            setValue(false);

            RepositoryItemComboBox w = (RepositoryItemComboBox)barEditItem2.Edit;
            w.Items.Add("亮度图");
            w.Items.Add("曲面图");
            w.Items.Add("彩色图");
            barEditItem2.EditValueChanged += ShowImgTypeChanged;
            barEditItem2.EditValue = "亮度图";
        }

        private void ShowImgTypeChanged(object sender, EventArgs e)
        {
            if ("亮度图" == barEditItem2.EditValue.ToString())
            {
                MyGlobal.isShowHeightImg = true;
                MyGlobal.isShowSurfaceImg = false;
            }
            else if ("曲面图" == barEditItem2.EditValue.ToString())
            {
                MyGlobal.isShowHeightImg = true;
                MyGlobal.isShowSurfaceImg = true;
            }
            else
            {
                MyGlobal.isShowHeightImg = false;
                MyGlobal.isShowSurfaceImg = false;
            }
        }

        void ConnectTcp()
        {
            MyGlobal.thdWaitForClientAndMessage = new Thread(Listen_Surface);
            MyGlobal.thdWaitForClientAndMessage.IsBackground = true;

            MyGlobal.thdWaitForClientAndMessage.Name = "以太网通信线程";
            MyGlobal.thdWaitForClientAndMessage.Start();
        }

        private int recStateCode;
        public int RecStateCode
        {
            get { return recStateCode; }
            set
            {
                recStateCode = value;
                //if (recStateCode >= 2)
                //{
                //    recStateCode = 0;
                //    HIRecFinish();
                //}
            }

        }
        private void HIRecFinish()
        {
            if (MyGlobal.globalConfig.SensorIP == "127.0.0.1")
            {
                //if (SecretKey.License.SnOk)
                //{
                Action run = () =>
                {
                    string ok = RunSuface(1, true);
                    if (ok != "OK")
                    {
                        ShowAndSaveMsg(ok);
                    }
                };
                this.Invoke(run);
                //}

            }
        }

        private void GoSDK_SurfaceIntensityFinish()
        {
            RecStateCode++;
            ShowAndSaveMsg($"RecStateCode-->{RecStateCode}" + "{ 亮度数据接收成功 }");
        }
        private void GoSDK_SurfaceZRecFinish()
        {
            RecStateCode++;
            ShowAndSaveMsg($"RecStateCode-->{RecStateCode}" + "{ 高度数据接收成功 }");
        }
        void GenIntesityProfile(List<SagensSdk.Profile> profile, out HObject Image)
        {
            int len = profile.Count;
            int width = profile[0].points.Length;
            byte[] imageArray = new byte[width * len];
            int k = 0;
            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    imageArray[k] = profile[i].points[j].Intensity;
                    k++;
                }
            }
            Image = new HObject();
            HOperatorSet.GenEmptyObj(out Image);
            MyGlobal.GoSDK.GenHalconImage(imageArray, width, len, out Image);

        }


        string[] SideName = { "Side1", "Side2", "Side3", "Side4" };

        //设定图像保存路径命名，以扫描的第一条边时间为当前物料保存路径名
        public static string saveImageTime = "";

        private string Run(int Station)
        {
            try
            {

                //byte[] SurfaceIntensity = MyGlobal.GoSDK.SurfaceDataIntensity;
                List<SagensSdk.Profile> profilet = MyGlobal.GoSDK.ProfileList;
                if (Station == 1)
                {

                    XCoord.Clear();
                    YCoord.Clear();
                    ZCoord.Clear();
                    StrLorC.Clear();
                    Xorigin.Clear();
                    Yorigin.Clear();
                    Zorigin.Clear();
                    NameOrigin.Clear();
                    AnchorList.Clear();
                    MyGlobal.globalConfig.Count++;

                    for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            MyGlobal.ImageMulti[i][j].Dispose();
                        }

                    }
                    MyGlobal.ImageMulti.Clear();

                }

                if (profilet != null)
                {

                    List<SagensSdk.Profile> profile = new List<SagensSdk.Profile>();

                    MyGlobal.GoSDK.FillingRow(profilet, true, 0.002, MyGlobal.globalConfig.dataContext.yResolution, out profile);
                    long SurfaceWidth, SurfaceHeight;
                    SurfaceWidth = profile[0].points.Length;
                    SurfaceHeight = profile.Count;
                    float[] SurfacePointZ = new float[SurfaceWidth * SurfaceHeight];


                    HObject HeightImage = new HObject(); HObject IntensityImage = new HObject();

                    GenIntesityProfile(profile, out IntensityImage);
                    MyGlobal.GoSDK.ProfileListToArr(profile, SurfacePointZ);

                    MyGlobal.GoSDK.GenHalconImage(SurfacePointZ, SurfaceWidth, SurfaceHeight, out HeightImage);

                    //if (!File.Exists(MyGlobal.ModelPath + "\\" + SideName[Station - 1] + "H.tiff")|| !File.Exists(MyGlobal.ModelPath + "\\" + SideName[Station - 1] + "I.tiff"))
                    //{
                    //    HOperatorSet.WriteImage(HeightImage, "tiff", 0, MyGlobal.ModelPath + "\\" + SideName[Station - 1] + "H.tiff");
                    //    HOperatorSet.WriteImage(IntensityImage, "tiff", 0, MyGlobal.ModelPath + "\\" + SideName[Station - 1] + "I.tiff");
                    //}
                    //HOperatorSet.WriteImage(HeightImage, "tiff", 0, MyGlobal.DataPath + "ProfileTemp\\" + SideName[Station - 1] + "H.tiff");
                    //HOperatorSet.WriteImage(IntensityImage, "tiff", 0, MyGlobal.DataPath + "ProfileTemp\\" + SideName[Station - 1] + "I.tiff");

                    //HObject rgbImage = new HObject();
                    //PseudoColor.GrayToPseudoColor(HeightImage, out rgbImage, true, -20, 10);
                    //MyGlobal.hWindow_Final[Station -1].HobjectToHimage(rgbImage);
                    Action sw = () =>
                    {
                        MyGlobal.hWindow_Final[Station - 1].HobjectToHimage(IntensityImage);
                    };
                    this.Invoke(sw);

                    if (Station == 1)
                        saveImageTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                    StaticOperate.SaveImage(IntensityImage, MyGlobal.globalConfig.Count.ToString(), SideName[Station - 1] + "I.tiff", true);
                    StaticOperate.SaveImage(HeightImage, MyGlobal.globalConfig.Count.ToString(), SideName[Station - 1] + "H.tiff", true);


                    string OK = RunSide(Station, true, IntensityImage, HeightImage);

                    HObject[] temp = { IntensityImage, HeightImage };
                    MyGlobal.ImageMulti.Add(temp);

                    //double[][] x, y, z;string[][] Strlorc;
                    //string OK = RunFindPoint(Station, IntensityImage, HeightImage, out x, out y, out z,out Strlorc, HomMat3D[Station - 1], MyGlobal.hWindow_Final[0]);
                    //XCoord.Add(x);
                    //YCoord.Add(y);
                    //ZCoord.Add(z);
                    //StrLorC.Add(Strlorc);
                    //int count = 0;
                    //if (Station > 0)
                    //{
                    //    //写入到文本
                    //    StringBuilder Str = new StringBuilder();
                    //    for (int i = 0; i < Station; i++)
                    //    {
                    //        for (int j = 0; j < XCoord[i].GetLength(0); j++)
                    //        {
                    //            for (int k = 0; k < XCoord[i][j].Length; k++)
                    //            {
                    //                double X1 = Math.Round(XCoord[i][j][k], 3);
                    //                double Y1 = Math.Round(YCoord[i][j][k], 3);
                    //                double Z1 = Math.Round(ZCoord[i][j][k], 3);
                    //                string lorc = StrLorC[i][j][k];
                    //                count++;
                    //                Str.Append(count.ToString() + "," + X1.ToString("0.000") + "," + Y1.ToString("0.000") + "," + Z1.ToString("0.000") + "," + lorc + "\r\n");
                    //            }
                    //        }
                    //    }
                    //    StaticOperate.writeTxt("D:\\Laser3D.txt", Str.ToString());
                    //}
                    return OK;
                }
                else
                {
                    return "未收到亮度数据";
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }

        //点云
        private string RunSuface(int Station, bool isRight, bool outline = false)
        {
            try
            {
                if (Station == 1)
                {

                    XCoord.Clear();
                    YCoord.Clear();
                    ZCoord.Clear();
                    StrLorC.Clear();
                    Xorigin.Clear();
                    Yorigin.Clear();
                    Zorigin.Clear();
                    NameOrigin.Clear();
                    AnchorList.Clear();
                    errstr.Clear();

                    MyGlobal.globalConfig.Count++;

                    if (!outline)
                    {
                        for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                MyGlobal.ImageMulti[i][j].Dispose();
                            }

                        }
                        MyGlobal.ImageMulti.Clear();
                        for (int i = 0; i < MyGlobal.hWindow_Final.Length; i++)
                        {
                            MyGlobal.hWindow_Final[i].ClearWindow();
                        }
                    }

                }

                if (MyGlobal.GoSDK.SurfaceDataZ != null)
                {

                    long SurfaceWidth, SurfaceHeight;
                    SurfaceWidth = MyGlobal.GoSDK.SurfaceWidth;
                    SurfaceHeight = MyGlobal.GoSDK.SurfaceHeight;
                    HObject HeightImage, rotateHeightImg, ZoomHeightImg;
                    HObject tempInteImg, ZoomIntensityImg, IntensityImage;
                    HObject tempByteImg, byteImg;
                    HObject rgbImg;
                    HObject zoomRgbImg;
                    HObject tempAlignImg, zoomAlignImg, tileImg, rotateAlignImg;

                    HOperatorSet.GenEmptyObj(out rotateHeightImg);
                    HOperatorSet.GenEmptyObj(out tempInteImg);
                    HOperatorSet.GenEmptyObj(out tempByteImg);
                    HOperatorSet.GenEmptyObj(out HeightImage);
                    HOperatorSet.GenEmptyObj(out IntensityImage);
                    HOperatorSet.GenEmptyObj(out byteImg);
                    HOperatorSet.GenEmptyObj(out ZoomHeightImg);
                    HOperatorSet.GenEmptyObj(out ZoomIntensityImg);
                    HOperatorSet.GenEmptyObj(out rgbImg);
                    HOperatorSet.GenEmptyObj(out zoomRgbImg);

                    HOperatorSet.GenEmptyObj(out tempAlignImg);
                    HOperatorSet.GenEmptyObj(out rotateAlignImg);
                    HOperatorSet.GenEmptyObj(out zoomAlignImg);
                    HOperatorSet.GenEmptyObj(out tileImg);
                    try
                    {


                        float[] SurfacePointZ = MyGlobal.GoSDK.SurfaceDataZ;
                        byte[] IntesitySurfacePointZ = MyGlobal.GoSDK.SurfaceDataIntensity;
                        float[] SurfaceAlignData = MyGlobal.GoSDK.SurfaceAlignData;
                        uint surfaceAlignWidth = MyGlobal.GoSDK.SurfaceAlignWidth;
                        uint surfaceAlignHeight = MyGlobal.GoSDK.SurfaceAlignHeight;

                        isLastImgRecOK = true;
                        if (SurfacePointZ != null)
                        {
                            long encoder = MyGlobal.GoSDK.Stamp.encoder;
                            ShowAndSaveMsg($"Stamp 结束位编码器数值{encoder.ToString()}");
                            //ShowAndSaveMsg($"结束编码器数值2 --- 》{ MyGlobal.GoSDK.GetSensorEncode()}");
                            HeightImage.Dispose();
                            MyGlobal.GoSDK.GenHalconImage(SurfacePointZ, SurfaceWidth, SurfaceHeight, out HeightImage);
                            MyGlobal.GoSDK.SurfaceDataZ = null;
                        }
                        else { return "高度值为空"; }

                        if (IntesitySurfacePointZ != null)
                        {
                            tempInteImg.Dispose();
                            MyGlobal.GoSDK.GenHalconImage(IntesitySurfacePointZ, SurfaceWidth, SurfaceHeight, out tempInteImg);
                            MyGlobal.GoSDK.SurfaceDataIntensity = null;
                        }
                        else { return "亮度值为空"; }



                        ////
                        ////生成并显示伪彩色图
                        if (!MyGlobal.isShowHeightImg)
                        {
                            byte[] surfaceDataZByte = MyGlobal.GoSDK.SurfaceDataZByte;
                            tempByteImg.Dispose();
                            MyGlobal.GoSDK.GenHalconImage(surfaceDataZByte, SurfaceWidth, SurfaceHeight, out tempByteImg);
                            MyGlobal.GoSDK.SurfaceDataZByte = null;

                            byteImg.Dispose();
                            if (isRight)
                            {
                                HOperatorSet.RotateImage(tempByteImg, out byteImg, MyGlobal.globalPointSet_Right.imgRotateArr[Station - 1], "constant");

                            }
                            else
                            {
                                HOperatorSet.RotateImage(tempByteImg, out byteImg, MyGlobal.globalPointSet_Left.imgRotateArr[Station - 1], "constant");

                            }
                            rgbImg.Dispose();
                            PseudoColor.GrayToPseudoColor(byteImg, out rgbImg);
                            zoomRgbImg.Dispose();
                            HOperatorSet.ZoomImageFactor(rgbImg, out zoomRgbImg, 1, 4, "constant");
                        }
                        ////


                        ZoomHeightImg.Dispose();
                        HOperatorSet.ZoomImageFactor(HeightImage, out ZoomHeightImg, 1, 4, "constant");
                        if (MyGlobal.globalConfig.enableAlign && SurfaceAlignData != null)//使用角落校正的图像
                        {
                            tempAlignImg.Dispose();
                            MyGlobal.GoSDK.GenHalconImage(SurfaceAlignData, surfaceAlignWidth, surfaceAlignHeight, out tempAlignImg);
                            zoomAlignImg.Dispose();
                            HOperatorSet.ZoomImageFactor(tempAlignImg, out zoomAlignImg, 2, 2, "constant");
                            //bool isUp = Station == 4 || Station == 2;
                            bool isUp = false;
                            if (MyGlobal.IsRight)
                            {
                                isUp = MyGlobal.globalPointSet_Right.IsUp[Station -1];
                            }
                            else
                            {
                                isUp = MyGlobal.globalPointSet_Left.IsUp[Station - 1];
                            }
                            tileImg.Dispose();
                            TileImg(zoomAlignImg, ZoomHeightImg, out tileImg, isUp);


                            rotateAlignImg.Dispose();
                            if (isRight)
                            {
                                HOperatorSet.RotateImage(tileImg, out rotateAlignImg, MyGlobal.globalPointSet_Right.imgRotateArr[Station - 1], "constant");
                            }
                            else
                            {
                                HOperatorSet.RotateImage(tileImg, out rotateAlignImg, MyGlobal.globalPointSet_Left.imgRotateArr[Station - 1], "constant");
                            }

                            MyGlobal.GoSDK.SurfaceAlignData = null;
                        }


                        rotateHeightImg.Dispose();
                        if (isRight)
                        {
                            HOperatorSet.RotateImage(ZoomHeightImg, out rotateHeightImg, MyGlobal.globalPointSet_Right.imgRotateArr[Station - 1], "constant");
                        }
                        else
                        {
                            HOperatorSet.RotateImage(ZoomHeightImg, out rotateHeightImg, MyGlobal.globalPointSet_Left.imgRotateArr[Station - 1], "constant");
                        }





                        IntensityImage.Dispose();
                        if (isRight)
                        {
                            HOperatorSet.RotateImage(tempInteImg, out IntensityImage, MyGlobal.globalPointSet_Right.imgRotateArr[Station - 1], "constant");
                        }
                        else
                        {
                            HOperatorSet.RotateImage(tempInteImg, out IntensityImage, MyGlobal.globalPointSet_Left.imgRotateArr[Station - 1], "constant");
                        }
                        ZoomIntensityImg.Dispose();
                        HOperatorSet.ZoomImageFactor(IntensityImage, out ZoomIntensityImg, 1, 4, "constant");





                        if (Station == 1)

                            saveImageTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                        //bool isSaveImgOK = false;
                        //ThreadPool.QueueUserWorkItem(delegate
                        //{
                        //    StaticOperate.SaveImage(ZoomIntensityImg, MyGlobal.globalConfig.Count.ToString(), SideName[Station - 1] + "I.tiff");
                        //    StaticOperate.SaveImage(ZoomHeightImg, MyGlobal.globalConfig.Count.ToString(), SideName[Station - 1] + "H.tiff");
                        //    if (!MyGlobal.isShowHeightImg)
                        //    {
                        //        StaticOperate.SaveImage(zoomRgbImg, MyGlobal.globalConfig.Count.ToString(), SideName[Station - 1] + "B.tiff");

                        //    }
                        //    isSaveImgOK = true;
                        //});


                        if (MyGlobal.isShowHeightImg)
                        {
                            //if (rotateAlignImg.CountObj() != 0)
                            //{
                            //    Action sw = () =>
                            //    {
                            //        MyGlobal.hWindow_Final[Station - 1].HobjectToHimage(rotateAlignImg);                                    
                            //    };
                            //    this.Invoke(sw);

                            //}
                            //else
                            //{
                            Action sw = () =>
                            {
                                if (rotateAlignImg.CountObj() != 0 && MyGlobal.isShowSurfaceImg)
                                {
                                    MyGlobal.hWindow_Final[Station - 1].HobjectToHimage(rotateAlignImg);
                                }
                                else
                                {
                                    MyGlobal.hWindow_Final[Station - 1].HobjectToHimage(ZoomIntensityImg);

                                }
                            };
                            this.Invoke(sw);

                            //}

                        }
                        else
                        {
                            Action asd = () => { MyGlobal.hWindow_Final[Station - 1].HobjectToHimage(zoomRgbImg); };
                            this.Invoke(asd);
                        }
                        string OK = "";
                        if (MyGlobal.globalConfig.enableAlign)
                        {
                            if (rotateAlignImg.CountObj() != 0)
                            {
                                OK = RunSide(Station, isRight, ZoomIntensityImg, rotateAlignImg, false, rotateHeightImg);
                            }
                            else
                            {
                                OK = "曲面拟合图像接收失败";
                            }
                        }
                        else
                        {
                            OK = RunSide(Station, isRight, ZoomIntensityImg, rotateHeightImg, false, null);
                        }

                        HObject[] temp = new HObject[3];
                        temp[0] = MyGlobal.hWindow_Final[Side - 1].Image;
                        temp[1] = rotateHeightImg;
                        temp[2] = MyGlobal.globalConfig.enableAlign ? rotateAlignImg : null;

                        MyGlobal.ImageMulti.Add(temp);

                        //while (!isSaveImgOK);//等待图片保存完成

                        if (Side == 4)
                        {
                            if (errstr.Length > 0)
                            {
                                if (errstr.ToString().Contains("定位"))
                                {
                                    if (isRight)
                                    {
                                        MyGlobal.globalPointSet_Right.AnchorErrorCnt++;
                                    }
                                    else
                                    {
                                        MyGlobal.globalPointSet_Left.AnchorErrorCnt++;

                                    }
                                }
                                else if (errstr.ToString().Contains("抓边") || errstr.ToString().Contains("RunFindPoint"))
                                {

                                    if (isRight)
                                    {
                                        MyGlobal.globalPointSet_Right.FindEgdeErrorCnt++;
                                    }
                                    else
                                    {
                                        MyGlobal.globalPointSet_Left.FindEgdeErrorCnt++;

                                    }
                                }
                                else if (errstr.ToString().Contains("探高"))
                                {

                                    if (isRight)
                                    {
                                        MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt++;
                                    }
                                    else
                                    {
                                        MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt++;
                                    }
                                }
                                else
                                {
                                    if (isRight)
                                    {
                                        MyGlobal.globalPointSet_Right.FindEgdeErrorCnt++;
                                    }
                                    else
                                    {
                                        MyGlobal.globalPointSet_Left.FindEgdeErrorCnt++;
                                    }

                                }
                                //string path = MyGlobal.DataPath + "ErrorImage\\" + string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + "\\";
                                //ShowAndSaveErrorMsg(errstr.ToString(), path + saveImageTime);
                                //for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
                                //{
                                //    StaticOperate.SaveErrorImage(MyGlobal.ImageMulti[i][0], MyGlobal.globalConfig.Count.ToString(), SideName[i] + "I.tiff", isRight);
                                //    StaticOperate.SaveErrorImage(MyGlobal.ImageMulti[i][1], MyGlobal.globalConfig.Count.ToString(), SideName[i] + "L_H.tiff", isRight);
                                //    if (rotateAlignImg.CountObj() > 0)
                                //    {
                                //        StaticOperate.SaveErrorImage(MyGlobal.ImageMulti[i][2], MyGlobal.globalConfig.Count.ToString(), SideName[i] + "S_H.tiff", isRight);
                                //    }
                                //}


                                string errorDatDate = MyGlobal.DataPath + "ErrorDat\\" + string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + "\\";
                                if (!Directory.Exists(errorDatDate))
                                {
                                    Directory.CreateDirectory(errorDatDate);
                                }
                                string errorDatTime = DateTime.Now.ToString("hhmmssff");
                                string errorDatPath = errorDatDate + errorDatTime+"\\";
                                if (!Directory.Exists(errorDatPath))
                                {
                                    Directory.CreateDirectory(errorDatPath);
                                }
                                ShowAndSaveErrorMsg(errstr.ToString(), errorDatPath + saveImageTime);
                                for (int i = 0; i < MyGlobal.GoSDK.sddList_L.Count; i++)
                                {
                                    StaticTool.WriteSerializable($"{errorDatPath}Side{i+1}_L_H.dat", MyGlobal.GoSDK.sddList_L[i]);
                                    if (MyGlobal.GoSDK.sddList_I.Count>i)
                                    {
                                        StaticTool.WriteSerializable($"{errorDatPath}Side{i + 1}_I.dat", MyGlobal.GoSDK.sddList_I[i]);
                                    }
                                    if (MyGlobal.GoSDK.sddList_S.Count>i)
                                        {
                                        StaticTool.WriteSerializable($"{errorDatPath}Side{i + 1}_S_H.dat", MyGlobal.GoSDK.sddList_S[i]);
                                        }
                                    }
                                MyGlobal.GoSDK.sddList_L.Clear();
                                MyGlobal.GoSDK.sddList_I.Clear();
                                MyGlobal.GoSDK.sddList_S.Clear();
                            }
                            else
                            {
                                if (isRight)
                                {
                                    MyGlobal.globalPointSet_Right.OkCnt++;
                                }
                                else
                                {
                                    MyGlobal.globalPointSet_Left.OkCnt++;

                                }
                                if (MyGlobal.globalConfig.isSaveImg)
                                {
                                    ThreadPool.QueueUserWorkItem(delegate
                                    {
                                        for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
                                        {
                                            StaticOperate.SaveImage(MyGlobal.ImageMulti[i][0], MyGlobal.globalConfig.Count.ToString(), SideName[i] + "I.tiff", isRight);
                                            StaticOperate.SaveImage(MyGlobal.ImageMulti[i][1], MyGlobal.globalConfig.Count.ToString(), SideName[i] + "L_H.tiff", isRight);
                                            if (rotateAlignImg.CountObj() > 0)
                                            {
                                                StaticOperate.SaveImage(MyGlobal.ImageMulti[i][2], MyGlobal.globalConfig.Count.ToString(), SideName[i] + "S_H.tiff", isRight);
                                            }
                                        }
                                    });
                                }
                            }
                            setValue(isRight);
                            if (isRight)
                            {
                                StaticOperate.WriteXML(MyGlobal.globalPointSet_Right, MyGlobal.AllTypePath + "GlobalPoint_Right.xml");
                            }
                            else
                            {
                                StaticOperate.WriteXML(MyGlobal.globalPointSet_Right, MyGlobal.AllTypePath + "GlobalPoint_Left.xml");

                            }

                        }

                        return OK;
                    }
                    catch (Exception ex)
                    {
                        string a = ex.StackTrace;
                        int ind = a.IndexOf("行号");
                        int start = ind;
                        string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                        return "RunSurfae --> " + ex.Message + RowNum;
                    }
                    finally
                    {
                        tempByteImg.Dispose();
                        rgbImg.Dispose();
                        byteImg.Dispose();
                        zoomRgbImg.Dispose();

                        ZoomHeightImg.Dispose();
                        HeightImage.Dispose();

                        tempInteImg.Dispose();
                        IntensityImage.Dispose();
                        //ZoomIntensityImg.Dispose();

                        tempAlignImg.Dispose();
                        zoomAlignImg.Dispose();
                        tileImg.Dispose();
                    }
                }
                else
                {
                    //判断是否离线测试
                    if (outline)
                    {
                        string OK = "";
                        if (MyGlobal.globalConfig.enableAlign)
                        {
                            OK = RunSide(Station, isRight, MyGlobal.ImageMulti[Station - 1][0], MyGlobal.ImageMulti[Station - 1][2], false, MyGlobal.ImageMulti[Station - 1][1]);
                        }
                        else
                        {
                            OK = RunSide(Station, isRight, MyGlobal.ImageMulti[Station - 1][0], MyGlobal.ImageMulti[Station - 1][1], false);
                        }

                        if (Station == 4)
                        {
                            outline = false;
                            if (errstr.Length > 0)
                            {
                                if (errstr.ToString().Contains("定位"))
                                {
                                    if (isRight)
                                    {
                                        MyGlobal.globalPointSet_Right.AnchorErrorCnt++;
                                    }
                                    else
                                    {
                                        MyGlobal.globalPointSet_Left.AnchorErrorCnt++;

                                    }
                                }
                                else if (errstr.ToString().Contains("抓边") || errstr.ToString().Contains("RunFindPoint"))
                                {

                                    if (isRight)
                                    {
                                        MyGlobal.globalPointSet_Right.FindEgdeErrorCnt++;
                                    }
                                    else
                                    {
                                        MyGlobal.globalPointSet_Left.FindEgdeErrorCnt++;

                                    }
                                }
                                else if (errstr.ToString().Contains("探高"))
                                {

                                    if (isRight)
                                    {
                                        MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt++;
                                    }
                                    else
                                    {
                                        MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt++;
                                    }
                                }
                                else
                                {
                                    if (isRight)
                                    {
                                        MyGlobal.globalPointSet_Right.FindEgdeErrorCnt++;
                                    }
                                    else
                                    {
                                        MyGlobal.globalPointSet_Left.FindEgdeErrorCnt++;
                                    }

                                }

                            }
                            else
                            {
                                if (isRight)
                                {
                                    MyGlobal.globalPointSet_Right.OkCnt++;
                                }
                                else
                                {
                                    MyGlobal.globalPointSet_Left.OkCnt++;

                                }

                            }
                            setValue(isRight);
                            if (isRight)
                            {
                                StaticOperate.WriteXML(MyGlobal.globalPointSet_Right, MyGlobal.AllTypePath + "GlobalPoint_Right.xml");
                            }
                            else
                            {
                                StaticOperate.WriteXML(MyGlobal.globalPointSet_Right, MyGlobal.AllTypePath + "GlobalPoint_Left.xml");

                            }
                        }
                        return OK;
                    }
                    else
                    {
                        return "RunSurfae --> 高度数据为空";
                    }


                }
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "RunSurfae Exception -->" + ex.Message + RowNum;
            }
            finally
            {
                isLastImgRecOK = true;
            }
        }



        public void TileImg(HObject ho_Plane, HObject ho_Zoomheightimg, out HObject ho_TiledImage, bool isUp)
        {
            HObject ho_ObjectsConcat;

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null, hv_Width1 = null;
            HTuple hv_Height1 = null, hv_row = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_TiledImage);
            HOperatorSet.GenEmptyObj(out ho_ObjectsConcat);
            HOperatorSet.GetImageSize(ho_Plane, out hv_Width, out hv_Height);
            HOperatorSet.GetImageSize(ho_Zoomheightimg, out hv_Width1, out hv_Height1);
            if (isUp)
            {
                hv_row = 0;
            }
            else { hv_row = hv_Height1 - hv_Height; }


            ho_ObjectsConcat.Dispose();
            HOperatorSet.ConcatObj(ho_Zoomheightimg, ho_Plane, out ho_ObjectsConcat);

            ho_TiledImage.Dispose();
            HOperatorSet.TileImagesOffset(ho_ObjectsConcat, out ho_TiledImage, (new HTuple(0)).TupleConcat(
                hv_row), (new HTuple(0)).TupleConcat(0), (new HTuple(-1)).TupleConcat(0),
                (new HTuple(-1)).TupleConcat(0), (new HTuple(-1)).TupleConcat(hv_Height - 30), (new HTuple(-1)).TupleConcat(
                hv_Width), hv_Width1, hv_Height1);
            ho_ObjectsConcat.Dispose();

            return;
        }
        private string RunOutLine(int Station, int id, bool SaveBase = false)
        {
            if (Station == 1)
            {
                XCoord.Clear();
                YCoord.Clear();
                ZCoord.Clear();
                StrLorC.Clear();
                Xorigin.Clear();
                Yorigin.Clear();
                Zorigin.Clear();
                NameOrigin.Clear();
                AnchorList.Clear();
                errstr.Clear();
            }
            if (MyGlobal.ImageMulti.Count == 0)
            {
                return "加载高度图和亮度图 Ng";
            }

            string Ok = "";

            if (MyGlobal.ImageMulti[id].Length == 3)
            {
                Ok = RunSide(Station, MyGlobal.IsRight, MyGlobal.ImageMulti[id][0], MyGlobal.ImageMulti[id][2], SaveBase, MyGlobal.ImageMulti[id][1]);
            }
            else
            {
                Ok = RunSide(Station, MyGlobal.IsRight, MyGlobal.ImageMulti[id][0], MyGlobal.ImageMulti[id][1], SaveBase);
            }

            if (Ok != "OK")
            {
                return Ok;
            }

            return Ok;
        }

        #region RunSide111
        //[Obsolete]
        //private string RunSide111(int Station, HObject IntensityImage, HObject HeightImage)
        //{
        //    try
        //    {
        //        double[][] x, y, z; string[][] Strlorc; HTuple[] original = new HTuple[2];
        //        string OK = RunFindPoint(Station, IntensityImage, HeightImage, out x, out y, out z, out Strlorc, out original, MyGlobal.HomMat3D[Station - 1], MyGlobal.hWindow_Final[0]);
        //        XCoord.Add(x);
        //        YCoord.Add(y);
        //        ZCoord.Add(z);
        //        StrLorC.Add(Strlorc);
        //        #region 除去起始位重复部分 并均分 
        //        for (int i = 0; i < Station; i++)
        //        {
        //            HTuple firstPt, order = 0, lastPt, fpt, Grater, Less, GraterId = new HTuple(), LessId = new HTuple(); string first = "";
        //            HTuple ResultX = new HTuple(), ResultY = new HTuple(), ResultZ = new HTuple(), ResultLorC = new HTuple();
        //            HTuple ResultX2 = new HTuple(), ResultY2 = new HTuple(), ResultZ2 = new HTuple(), ResultLorC2 = new HTuple();
        //            HTuple Lx1 = new HTuple(), Ly1 = new HTuple(), Lz1 = new HTuple(), Gx1 = new HTuple(), Gy1 = new HTuple(), Gz1 = new HTuple(); int Len1 = 0; int Len2 = 0; int Len = 0;
        //            HTuple tempx, tempy, tempz; HTuple x1, y1, x2, y2;
        //            switch (i)
        //            {
        //                case 0:
        //                    if (Station == 4) //Y1<Y4  // 去掉第一条重叠 保留第4条
        //                    {

        //                        ResultY = YCoord[i][0];//第1条第一段
        //                        ResultX = XCoord[i][0];//第1条第一段
        //                        ResultZ = ZCoord[i][0];//第1条第一段
        //                        ResultLorC = StrLorC[i][0];//第一段

        //                        if (ResultY.Length == 0)
        //                        {
        //                            break;
        //                        }
        //                        firstPt = ResultY[0];//第1条第一点
        //                        order = YCoord[3].GetLength(0) - 1;
        //                        ResultY2 = YCoord[3][order];//第四边最后段
        //                        ResultX2 = XCoord[3][order];//第四边最后段
        //                        //ResultLorC2 = StrLorC[3][order];


        //                        lastPt = ResultY2[ResultY2.Length - 1];//第4条最后一点
        //                        Less = ResultY.TupleLessEqualElem(lastPt);//第1条小于第4条重叠部分
        //                        Grater = ResultY2.TupleGreaterEqualElem(firstPt);//第4条大于于第1边不重叠部分

        //                        GraterId = Grater.TupleFind(1);
        //                        LessId = Less.TupleFind(1);
        //                        if (GraterId == -1)
        //                        {
        //                            break;
        //                        }

        //                        //重叠部分取均值
        //                        Len2 = GraterId.Length;
        //                        Len1 = LessId.Length;
        //                        Len = Len1 < Len2 ? Len1 : Len2;
        //                        //Len2 添加到第4条
        //                        if (Len >= ResultY.Length || Len >= ResultY2.Length)
        //                        {
        //                            break;
        //                        }
        //                        y1 = ResultY2.TupleSelectRange(ResultY2.Length - Len, ResultY2.Length - 1);
        //                        x1 = ResultX2.TupleSelectRange(ResultX2.Length - Len, ResultX2.Length - 1);
        //                        y2 = ResultY.TupleSelectRange(0, Len - 1);
        //                        x2 = ResultX.TupleSelectRange(0, Len - 1);
        //                        Lx1 = (x1 + x2) / 2;
        //                        Ly1 = (y1 + y2) / 2;
        //                        HOperatorSet.TupleGenSequence(0, Len - 1, 1, out Less);
        //                        HOperatorSet.TupleGenSequence(ResultY2.Length - 1, ResultY2.Length - Len, -1, out Grater);


        //                        ResultY2 = ResultY2.TupleReplace(Grater, Ly1);
        //                        ResultX2 = ResultX2.TupleReplace(Grater, Lx1);

        //                        ResultY = ResultY.TupleRemove(Less);
        //                        ResultX = ResultX.TupleRemove(Less);
        //                        ResultZ = ResultZ.TupleRemove(Less);
        //                        //首位 
        //                        first = ResultLorC[0];
        //                        ResultLorC = ResultLorC.TupleRemove(Less);
        //                        ResultLorC[0] = first;

        //                        XCoord[3][order] = ResultX2;
        //                        YCoord[3][order] = ResultY2;
        //                    }

        //                    break;
        //                case 1:
        //                    if (Station >= 2) //X2>X1  去掉第二条重叠 保留第1条
        //                    {

        //                        ResultX = XCoord[i][0];//第2条第第一段
        //                        ResultY = YCoord[i][0];//第2条第第一段
        //                        ResultZ = ZCoord[i][0];//第2条第第一段
        //                        ResultLorC = StrLorC[i][0];//第一段

        //                        if (ResultX.Length == 0)
        //                        {
        //                            break;
        //                        }
        //                        firstPt = ResultX[0];//第2条第一点
        //                        order = XCoord[0].GetLength(0) - 1;
        //                        ResultX2 = XCoord[0][order];//第1边最后段
        //                        ResultY2 = YCoord[0][order];//第1边最后段

        //                        lastPt = ResultX2[ResultX2.Length - 1];//第1条最后一点
        //                        Grater = ResultX.TupleGreaterEqualElem(lastPt);//第2条大于第1条重叠部分
        //                        Less = ResultX2.TupleLessEqualElem(firstPt);//第1条小于第2边重叠部分

        //                        GraterId = Grater.TupleFind(1);
        //                        LessId = Less.TupleFind(1);

        //                        if (GraterId == -1 || LessId == -1)
        //                        {
        //                            break;
        //                        }

        //                        //重叠部分取均值    
        //                        //Gx1 = ResultX[LessId];
        //                        //Gy1 = ResultY[LessId];
        //                        //Gz1 = ResultZ[LessId];

        //                        Len2 = LessId.Length; Len1 = GraterId.Length;
        //                        Len = Len1 < Len2 ? Len1 : Len2;
        //                        //Len2 添加到第4条
        //                        if (Len >= ResultY.Length || Len >= ResultY2.Length)
        //                        {
        //                            break;
        //                        }
        //                        y1 = ResultY2.TupleSelectRange(ResultY2.Length - Len, ResultY2.Length - 1);
        //                        x1 = ResultX2.TupleSelectRange(ResultX2.Length - Len, ResultX2.Length - 1);
        //                        y2 = ResultY.TupleSelectRange(0, Len - 1);
        //                        x2 = ResultX.TupleSelectRange(0, Len - 1);
        //                        Lx1 = (x1 + x2) / 2;
        //                        Ly1 = (y1 + y2) / 2;

        //                        HOperatorSet.TupleGenSequence(0, Len - 1, 1, out Grater);
        //                        HOperatorSet.TupleGenSequence(ResultY2.Length - 1, ResultY2.Length - Len, -1, out Less);


        //                        ResultY2 = ResultY2.TupleReplace(Less, Ly1);
        //                        ResultX2 = ResultX2.TupleReplace(Less, Lx1);

        //                        ResultY = ResultY.TupleRemove(Grater);
        //                        ResultX = ResultX.TupleRemove(Grater);
        //                        ResultZ = ResultZ.TupleRemove(Grater);

        //                        //首位 
        //                        first = ResultLorC[0];
        //                        ResultLorC = ResultLorC.TupleRemove(Grater);
        //                        ResultLorC[0] = first;

        //                        XCoord[0][order] = ResultX2;
        //                        YCoord[0][order] = ResultY2;
        //                    }
        //                    break;
        //                case 2:
        //                    if (Station >= 3) //Y3>Y2 去掉第三条重叠 保留第2条
        //                    {

        //                        ResultY = YCoord[i][0];//第3条第一段
        //                        ResultX = XCoord[i][0];//第3条第一段
        //                        ResultZ = ZCoord[i][0];//第3条第一段
        //                        ResultLorC = StrLorC[i][0];//第一段

        //                        if (ResultY.Length == 0)
        //                        {
        //                            break;
        //                        }

        //                        firstPt = ResultY[0];//第3条第一点
        //                        order = YCoord[1].GetLength(0) - 1;
        //                        ResultY2 = YCoord[1][order];//第2边最后段
        //                        ResultX2 = XCoord[1][order];//第2边最后段
        //                        //ResultLorC2 = StrLorC[3][order];


        //                        lastPt = ResultY2[ResultY2.Length - 1];//第2条最后一点
        //                        Less = ResultY.TupleGreaterEqualElem(lastPt);//第3条大于第2条重叠部分
        //                        Grater = ResultY2.TupleLessEqualElem(firstPt);//第2条小于第3边重叠部分

        //                        GraterId = Grater.TupleFind(1);
        //                        LessId = Less.TupleFind(1);
        //                        if (GraterId == -1 || LessId == -1)
        //                        {
        //                            break;
        //                        }

        //                        //重叠部分取均值
        //                        Len2 = LessId.Length; Len1 = GraterId.Length;
        //                        Len = Len1 < Len2 ? Len1 : Len2;
        //                        //Len2 添加到第4条
        //                        if (Len >= ResultY.Length || Len >= ResultY2.Length)
        //                        {
        //                            break;
        //                        }
        //                        y1 = ResultY2.TupleSelectRange(ResultY2.Length - Len, ResultY2.Length - 1);
        //                        x1 = ResultX2.TupleSelectRange(ResultX2.Length - Len, ResultX2.Length - 1);
        //                        y2 = ResultY.TupleSelectRange(0, Len - 1);
        //                        x2 = ResultX.TupleSelectRange(0, Len - 1);
        //                        Lx1 = (x1 + x2) / 2;
        //                        Ly1 = (y1 + y2) / 2;

        //                        HOperatorSet.TupleGenSequence(0, Len - 1, 1, out Less);
        //                        HOperatorSet.TupleGenSequence(ResultY2.Length - 1, ResultY2.Length - Len, -1, out Grater);


        //                        ResultY2 = ResultY2.TupleReplace(Grater, Ly1);
        //                        ResultX2 = ResultX2.TupleReplace(Grater, Lx1);

        //                        ResultY = ResultY.TupleRemove(Less);
        //                        ResultX = ResultX.TupleRemove(Less);
        //                        ResultZ = ResultZ.TupleRemove(Less);
        //                        //首位 
        //                        first = ResultLorC[0];
        //                        ResultLorC = ResultLorC.TupleRemove(Less);
        //                        ResultLorC[0] = first;

        //                        XCoord[1][order] = ResultX2;
        //                        YCoord[1][order] = ResultY2;
        //                    }
        //                    break;
        //                case 3:
        //                    if (Station >= 4) //X4<X3  //Y3>Y2 去掉第四条重叠 保留第3条
        //                    {

        //                        ResultX = XCoord[i][0];//第4条第一段
        //                        ResultY = YCoord[i][0];//第4条第一段
        //                        ResultZ = ZCoord[i][0];//第4条第一段
        //                        ResultLorC = StrLorC[i][0]; //第4条//第一段

        //                        if (ResultX.Length == 0)
        //                        {
        //                            break;
        //                        }
        //                        firstPt = ResultX[0];//第4条第一点
        //                        order = XCoord[2].GetLength(0) - 1;
        //                        ResultX2 = XCoord[2][order];//第3边最后段
        //                        ResultY2 = YCoord[2][order];//第3边最后段

        //                        lastPt = ResultX2[ResultX2.Length - 1];//第3条最后一点
        //                        Grater = ResultX.TupleLessEqualElem(lastPt);//第4条小于第1条重叠部分
        //                        Less = ResultX2.TupleGreaterEqualElem(firstPt);//第3条大于第2边重叠部分

        //                        GraterId = Grater.TupleFind(1);
        //                        LessId = Less.TupleFind(1);

        //                        if (GraterId == -1 || LessId == -1)
        //                        {
        //                            break;
        //                        }

        //                        //重叠部分取均值    
        //                        Len2 = LessId.Length; Len1 = GraterId.Length;
        //                        Len = Len1 < Len2 ? Len1 : Len2;
        //                        //Len2 添加到第4条
        //                        if (Len >= ResultY.Length || Len >= ResultY2.Length)
        //                        {
        //                            break;
        //                        }
        //                        y1 = ResultY2.TupleSelectRange(ResultY2.Length - Len, ResultY2.Length - 1);
        //                        x1 = ResultX2.TupleSelectRange(ResultX2.Length - Len, ResultX2.Length - 1);
        //                        y2 = ResultY.TupleSelectRange(0, Len - 1);
        //                        x2 = ResultX.TupleSelectRange(0, Len - 1);
        //                        Lx1 = (x1 + x2) / 2;
        //                        Ly1 = (y1 + y2) / 2;

        //                        HOperatorSet.TupleGenSequence(0, Len - 1, 1, out Grater);
        //                        HOperatorSet.TupleGenSequence(ResultY2.Length - 1, ResultY2.Length - Len, -1, out Less);


        //                        ResultY2 = ResultY2.TupleReplace(Less, Ly1);
        //                        ResultX2 = ResultX2.TupleReplace(Less, Lx1);

        //                        ResultY = ResultY.TupleRemove(Grater);
        //                        ResultX = ResultX.TupleRemove(Grater);
        //                        ResultZ = ResultZ.TupleRemove(Grater);

        //                        //首位 
        //                        first = ResultLorC[0];
        //                        ResultLorC = ResultLorC.TupleRemove(Grater);
        //                        ResultLorC[0] = first;

        //                        XCoord[2][order] = ResultX2;
        //                        YCoord[2][order] = ResultY2;
        //                    }
        //                    break;
        //            }
        //            if (GraterId.Length != 0 && GraterId.D == -1)
        //            {
        //                //XCoord[i][0] = null;
        //                //YCoord[i][0] = null;
        //                //ZCoord[i][0] = null;
        //                //StrLorC[i][0] = null;

        //                //return string.Format("第{0}边,第一段重合点数过多", i + 1);
        //            }
        //            else if (GraterId.Length != 0)
        //            {

        //                //XCoord[i][0] = ResultX;
        //                //YCoord[i][0] = ResultY;
        //                //ZCoord[i][0] = ResultZ;
        //                //StrLorC[i][0] = ResultLorC;                        
        //            }

        //        }
        //        #endregion

        //        Dictionary<int, string> everySeg = new Dictionary<int, string>();
        //        double[] xcoord, ycoord, zcoord; string[] keypt;
        //        int totalNum = 0;
        //        for (int i = 0; i < XCoord.Count; i++)
        //        {
        //            for (int j = 0; j < XCoord[i].GetLength(0); j++)
        //            {
        //                if (XCoord[i][j] == null)
        //                {
        //                    continue;
        //                }
        //                for (int k = 0; k < XCoord[i][j].Length; k++)
        //                {
        //                    totalNum++;
        //                }

        //            }
        //        }
        //        xcoord = new double[totalNum]; ycoord = new double[totalNum]; zcoord = new double[totalNum];
        //        keypt = new string[totalNum];
        //        int ind = 0;
        //        double x0 = 0, y0 = 0, z0 = 0;
        //        for (int i = 0; i < XCoord.Count; i++)
        //        {
        //            for (int j = 0; j < XCoord[i].GetLength(0); j++)
        //            {
        //                if (XCoord[i][j] == null)
        //                {
        //                    continue;
        //                }

        //                HTuple row = XCoord[i][j];
        //                HTuple col = YCoord[i][j];

        //                if (Station == 4)
        //                {
        //                    HObject NewSide = new HObject();
        //                    HOperatorSet.GenContourPolygonXld(out NewSide, row, col);
        //                    MyGlobal.hWindow_Final[0].viewWindow.displayHobject(NewSide, "red");
        //                }

        //                for (int k = 0; k < XCoord[i][j].Length; k++)
        //                {
        //                    if (k > 0)
        //                    {
        //                        if (xcoord[ind] == xcoord[ind - 1] && ycoord[ind] == ycoord[ind - 1])
        //                        {
        //                            //存在重复点；
        //                            MessageBox.Show("重复点");
        //                        }
        //                    }
        //                    xcoord[ind] = Math.Round(XCoord[i][j][k], 3);
        //                    ycoord[ind] = Math.Round(YCoord[i][j][k], 3);
        //                    zcoord[ind] = Math.Round(ZCoord[i][j][k], 3);
        //                    keypt[ind] = StrLorC[i][j][k];
        //                    if (k == 0)
        //                    {
        //                        everySeg.Add(ind, keypt[ind]);
        //                    }
        //                    ind++;
        //                    if (ind == 2044)
        //                    {
        //                        Debug.WriteLine("tt+" + ind);
        //                    }
        //                    Debug.WriteLine("tt+" + ind);
        //                }
        //            }
        //        }

        //        //排列起点
        //        //写入到文本
        //        StringBuilder Str = new StringBuilder();
        //        int Start = MyGlobal.globalConfig.Startpt;
        //        for (int i = 0; i < xcoord.Length; i++)
        //        {

        //            int start = Start;
        //            if (Start - 1 + i >= xcoord.Length)
        //            {
        //                start = Start - 1 + i - xcoord.Length;
        //            }
        //            else
        //            {
        //                start = Start - 1 + i;
        //            }
        //            double X1 = xcoord[start];
        //            double Y1 = ycoord[start];
        //            double Z1 = zcoord[start];
        //            string lorc = keypt[start];
        //            if (i == 0)
        //            {
        //                int[] keys = everySeg.Keys.ToArray();
        //                for (int m = 0; m < keys.Length; m++)
        //                {
        //                    if (Start >= keys[m])
        //                    {
        //                        lorc = everySeg[keys[m]];
        //                        break;
        //                    }
        //                }
        //            }
        //            if (i == 0)
        //            {
        //                x0 = X1;
        //                y0 = Y1;
        //                z0 = Z1;
        //            }
        //            Str.Append((i + 1).ToString() + "," + X1.ToString("0.000") + "," + Y1.ToString("0.000") + "," + Z1.ToString("0.000") + "," + lorc + "\r\n");

        //        }
        //        string strlast = "0;";
        //        int len1 = XCoord[Station - 1].GetLength(0);


        //        //if (Station > 0)
        //        //{
        //        //    double x0 =0, y0 =0, z0 =0;                   

        //        //    //写入到文本
        //        //    StringBuilder Str = new StringBuilder();
        //        //    for (int i = 0; i < Station; i++)
        //        //    {
        //        //        for (int j = 0; j < XCoord[i].GetLength(0); j++)
        //        //        {
        //        //            if (XCoord[i][j] == null)
        //        //            {
        //        //                continue;
        //        //            }
        //        //            HTuple row = XCoord[i][j];
        //        //            HTuple col = YCoord[i][j];

        //        //            if (Station ==4)
        //        //            {
        //        //                HObject NewSide = new HObject();
        //        //                HOperatorSet.GenContourPolygonXld(out NewSide,row, col);
        //        //                MyGlobal.hWindow_Final[0].viewWindow.displayHobject(NewSide, "red");
        //        //            }
        //        //            for (int k = 0; k < XCoord[i][j].Length; k++)
        //        //            {

        //        //                double X1 = Math.Round(XCoord[i][j][k], 3);
        //        //                double Y1 = Math.Round(YCoord[i][j][k], 3);
        //        //                double Z1 = Math.Round(ZCoord[i][j][k], 3);
        //        //                if (i == j  && j==k && k == 0)
        //        //                {
        //        //                     x0 = X1;
        //        //                     y0 = Y1;
        //        //                     z0 = Z1;
        //        //                }
        //        //                string lorc = StrLorC[i][j][k];
        //        //                count++;
        //        //                Str.Append(count.ToString() + "," + X1.ToString("0.000") + "," + Y1.ToString("0.000") + "," + Z1.ToString("0.000") + "," + lorc + "\r\n");

        //        //            }
        //        //        }
        //        //    }
        //        if (XCoord[Station - 1][len1 - 1] != null)
        //        {
        //            strlast = StrLorC[Station - 1][len1 - 1][0];
        //        }
        //        else
        //        {
        //            strlast = StrLorC[Station - 1][len1 - 2][0];

        //        }
        //        Str.Append((totalNum + 1).ToString() + "," + x0.ToString("0.000") + "," + y0.ToString("0.000") + "," + z0.ToString("0.000") + "," + strlast + "\r\n");

        //        StaticOperate.writeTxt("D:\\Laser3D_1.txt", Str.ToString());
        //        //}
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {

        //        return "RunSide error :" + ex.Message;
        //    }
        //}
        #endregion



        private string RunSide(int Side, bool isRight, HObject IntensityImage, HObject HeightImage, bool SaveBase = false, HObject OriginImage = null)
        {
            try
            {

                double[][] x, y, z; string[][] Strlorc; HTuple[] original = new HTuple[2];
                if (SaveBase)
                {
                    if (isRight)
                    {
                        MyGlobal.xyzBaseCoord_Right.ZCoord = null;
                    }
                    else
                    {
                        MyGlobal.xyzBaseCoord_Left.ZCoord = null;
                    }

                }
                string OK = RunFindPoint(Side, isRight, IntensityImage, HeightImage, out x, out y, out z, out Strlorc, out original, MyGlobal.hWindow_Final[Side - 1], OriginImage);

                double[] tz = null;
                if (x == null)
                {
                    x = new double[2][];
                }
                if (y == null)
                {
                    y = new double[2][];
                }
                if (z == null)
                {
                    tz = null;
                    z = new double[2][];
                }
                if (original[0] == null)
                {
                    original[0] = new HTuple();
                }
                if (original[1] == null)
                {
                    original[1] = new HTuple();
                }

                if (OK != "OK")
                {
                    Zorigin.Add(tz);
                    string[] tt = new string[1];
                    NameOrigin.Add(tt);
                    Yorigin.Add(original[0]);
                    Xorigin.Add(original[1]);
                    return "第" + Side + "边" + OK;
                }
                if (x[0] == null)
                {
                    return "第" + Side + "边未找到边";
                }


                XCoord.Add(x);
                YCoord.Add(y);
                ZCoord.Add(z);
                StrLorC.Add(Strlorc);
                if (isRight)
                {
                    NameOrigin.Add(MyGlobal.Right_findPointTool_Find.fParam[Side - 1].DicPointName.ToArray());
                }
                else
                {
                    NameOrigin.Add(MyGlobal.Left_findPointTool_Find.fParam[Side - 1].DicPointName.ToArray());

                }

                Yorigin.Add(original[0]);
                Xorigin.Add(original[1]);




                double[] zval = new double[z.Length];
                for (int i = 0; i < zval.Length; i++)
                {
                    zval[i] = z[i][0];
                }
                Zorigin.Add(zval);
                if (Side != 4)
                {
                    return "OK";
                }
                else
                {
                    //if (MyGlobal.IsRight)
                    //{
                    //    if (MyGlobal.globalPointSet_Right.IsReverse)
                    //    {
                    //        //逆时针排序
                    //        XCoord.Reverse();
                    //        YCoord.Reverse();
                    //        ZCoord.Reverse();
                    //        StrLorC.Reverse();
                    //        Xorigin.Reverse();
                    //        Yorigin.Reverse();
                    //        Zorigin.Reverse();
                    //        NameOrigin.Reverse();
                    //        AnchorList.Reverse();
                    //    }
                    //}
                    //else
                    //{
                    //    if (MyGlobal.globalPointSet_Left.IsReverse)
                    //    {
                    //        //逆时针排序
                    //        XCoord.Reverse();
                    //        YCoord.Reverse();
                    //        ZCoord.Reverse();
                    //        StrLorC.Reverse();
                    //        Xorigin.Reverse();
                    //        Yorigin.Reverse();
                    //        Zorigin.Reverse();
                    //        NameOrigin.Reverse();
                    //        AnchorList.Reverse();
                    //    }
                    //}


                }
                if (XCoord.Count != 4)
                {
                    return "运行失败";
                }
                #region 重复点
                //#region 除去起始位重复部分 并均分 
                //for (int i = 0; i < Station; i++)
                //{
                //    HTuple firstPt, order, last, lastPt, Grater, GraterId = new HTuple(); string first = "";
                //    HTuple ResultX = new HTuple(), ResultY = new HTuple(), ResultZ = new HTuple(), ResultLorC = new HTuple();
                //    switch (i)
                //    {
                //        case 0:
                //            if (Station == 4) //Y1<Y4
                //            {

                //                ResultY = YCoord[i][0];//第一段
                //                ResultX = XCoord[i][0];//第一段
                //                ResultZ = ZCoord[i][0];//第一段
                //                ResultLorC = StrLorC[i][0];//第一段

                //                if (ResultY.Length == 0)
                //                {
                //                    break;
                //                }
                //                firstPt = ResultY[0];//第一点
                //                order = YCoord[3].GetLength(0) - 1;
                //                last = YCoord[3][order];//第四边最后段
                //                lastPt = last[last.Length - 1];//最后一点
                //                Grater = ResultY.TupleGreaterEqualElem(lastPt);//大于第四条不重叠部分
                //                GraterId = Grater.TupleFind(1);
                //                if (GraterId == -1)
                //                {
                //                    break;
                //                }
                //                ResultY = ResultY[GraterId];
                //                ResultX = ResultX[GraterId];
                //                ResultZ = ResultZ[GraterId];
                //                //首位 
                //                first = ResultLorC[0];
                //                ResultLorC = ResultLorC[GraterId];
                //                ResultLorC[0] = first;

                //            }

                //            break;
                //        case 1:
                //            if (Station >= 2) //X2>X1
                //            {

                //                ResultX = XCoord[i][0];//第一段
                //                ResultY = YCoord[i][0];//第一段
                //                ResultZ = ZCoord[i][0];//第一段
                //                ResultLorC = StrLorC[i][0];//第一段

                //                if (ResultX.Length == 0)
                //                {
                //                    break;
                //                }
                //                firstPt = ResultX[0];//第一点
                //                order = XCoord[0].GetLength(0) - 1;
                //                last = XCoord[0][order];//第1边最后段
                //                lastPt = last[last.Length - 1];//最后一点
                //                Grater = ResultX.TupleLessEqualElem(lastPt);//小于第一条不重叠部分
                //                GraterId = Grater.TupleFind(1);
                //                if (GraterId == -1)
                //                {
                //                    break;
                //                }
                //                ResultY = ResultY[GraterId];
                //                ResultX = ResultX[GraterId];
                //                ResultZ = ResultZ[GraterId];

                //                //首位 
                //                first = ResultLorC[0];
                //                ResultLorC = ResultLorC[GraterId];
                //                ResultLorC[0] = first;
                //            }
                //            break;
                //        case 2:
                //            if (Station >= 3) //Y3>Y2
                //            {

                //                ResultY = YCoord[i][0];//第一段
                //                ResultX = XCoord[i][0];//第一段
                //                ResultZ = ZCoord[i][0];//第一段
                //                ResultLorC = StrLorC[i][0];//第一段

                //                if (ResultY.Length == 0)
                //                {
                //                    break;
                //                }

                //                firstPt = ResultY[0];//第一点
                //                order = YCoord[1].GetLength(0) - 1;
                //                last = YCoord[1][order];//第2边最后段
                //                lastPt = last[last.Length - 1];//最后一点
                //                Grater = ResultY.TupleLessEqualElem(lastPt);//不重叠部分
                //                GraterId = Grater.TupleFind(1);
                //                if (GraterId == -1)
                //                {
                //                    break;
                //                }
                //                ResultY = ResultY[GraterId];
                //                ResultX = ResultX[GraterId];
                //                ResultZ = ResultZ[GraterId];

                //                //首位 
                //                first = ResultLorC[0];
                //                ResultLorC = ResultLorC[GraterId];
                //                ResultLorC[0] = first;
                //            }
                //            break;
                //        case 3:
                //            if (Station >= 4) //X4<X3
                //            {

                //                ResultX = XCoord[i][0];//第一段
                //                ResultY = YCoord[i][0];//第一段
                //                ResultZ = ZCoord[i][0];//第一段
                //                ResultLorC = StrLorC[i][0];//第一段

                //                if (ResultX.Length == 0)
                //                {
                //                    break;
                //                }
                //                firstPt = ResultX[0];//第一点
                //                order = XCoord[2].GetLength(0) - 1;
                //                last = XCoord[2][order];//第3边最后段
                //                lastPt = last[last.Length - 1];//最后一点
                //                Grater = ResultX.TupleGreaterEqualElem(lastPt);//不重叠部分
                //                GraterId = Grater.TupleFind(1);
                //                if (GraterId == -1)
                //                {
                //                    break;
                //                }
                //                ResultY = ResultY[GraterId];
                //                ResultX = ResultX[GraterId];
                //                ResultZ = ResultZ[GraterId];

                //                //首位 
                //                first = ResultLorC[0];
                //                ResultLorC = ResultLorC[GraterId];
                //                ResultLorC[0] = first;
                //            }
                //            break;
                //    }
                //    if (GraterId.Length != 0 && GraterId.D == -1)
                //    {
                //        XCoord[i][0] = null;
                //        YCoord[i][0] = null;
                //        ZCoord[i][0] = null;
                //        StrLorC[i][0] = null;

                //        return string.Format("第{0}边,第一段重合点数过多", i + 1);
                //    }
                //    else if (GraterId.Length != 0)
                //    {
                //        XCoord[i][0] = ResultX;
                //        YCoord[i][0] = ResultY;
                //        ZCoord[i][0] = ResultZ;
                //        StrLorC[i][0] = ResultLorC;
                //    }

                //}
                //#endregion
                #endregion

                double Xresolution = MyGlobal.globalConfig.dataContext.xResolution;
                double Yresolution = MyGlobal.globalConfig.dataContext.yResolution;

                Dictionary<int, string> everySeg = new Dictionary<int, string>();
                double[] xcoord, ycoord, zcoord; string[] keypt;
                int totalNum = 0;
                for (int i = 0; i < XCoord.Count; i++)
                {
                    for (int j = 0; j < XCoord[i].GetLength(0); j++)
                    {
                        if (XCoord[i][j] == null)
                        {
                            continue;
                        }
                        for (int k = 0; k < XCoord[i][j].Length; k++)
                        {
                            totalNum++;
                        }

                    }
                }
                double[] RecX = new double[totalNum]; double[] RecY = new double[totalNum];
                int nID = 0;
                for (int i = 0; i < Yorigin.Count; i++)
                {
                    for (int j = 0; j < Yorigin[i].Length; j++)
                    {
                        RecX[nID] = Xorigin[i][j];
                        RecY[nID] = Yorigin[i][j];
                        nID++;
                    }

                }

                #region 判断离中心点距离是否超出范围
                //计算 中心
                //HObject Cnt = new HObject();
                //HOperatorSet.GenContourPolygonXld(out Cnt, new HTuple(RecY), new HTuple(RecX));
                //HTuple centerR, centerC, phi, Len1, Len2, ptorder;
                //HOperatorSet.FitRectangle2ContourXld(Cnt, "tukey", -1, 0, 0, 3, 2, out centerR, out centerC, out phi, out Len1, out Len2, out ptorder);

                if (SaveBase)
                {
                    //计算到中心点距离
                    double xyResolution = Math.Sqrt(Xresolution * Xresolution + Yresolution * Yresolution);
                    List<double[]> tempDist = new List<double[]>();
                    List<double[]> tempY = new List<double[]>();
                    for (int i = 0; i < Xorigin.Count; i++)
                    {
                        double[] x1 = new double[Xorigin[i].Length];
                        for (int j = 0; j < Xorigin[i].Length; j++)
                        {
                            HTuple dist = new HTuple();
                            HOperatorSet.DistancePp(Yorigin[i][j], Xorigin[i][j], AnchorList[i].Row, AnchorList[i].Col, out dist);
                            x1[j] = Math.Round(dist.D * xyResolution, 3);
                        }
                        tempDist.Add(x1);
                    }
                    if (isRight)
                    {

                        MyGlobal.xyzBaseCoord_Right.Dist = tempDist;
                        MyGlobal.xyzBaseCoord_Right.intersectCoordList = AnchorList;

                        MyGlobal.xyzBaseCoord_Right.XCoord = Xorigin;
                        MyGlobal.xyzBaseCoord_Right.YCoord = Yorigin;
                        MyGlobal.xyzBaseCoord_Right.ZCoord = ZCoord;

                        StaticOperate.WriteXML(MyGlobal.xyzBaseCoord_Right, MyGlobal.BaseTxtPath_Right);
                        //读取Z值基准高度】
                        if (File.Exists(MyGlobal.BaseTxtPath_Right))
                        {
                            MyGlobal.xyzBaseCoord_Right = (XYZBaseCoord)StaticOperate.ReadXML(MyGlobal.BaseTxtPath_Right, typeof(XYZBaseCoord));
                        }
                    }
                    else
                    {
                        MyGlobal.xyzBaseCoord_Left.Dist = tempDist;
                        MyGlobal.xyzBaseCoord_Left.intersectCoordList = AnchorList;

                        MyGlobal.xyzBaseCoord_Left.XCoord = Xorigin;
                        MyGlobal.xyzBaseCoord_Left.YCoord = Yorigin;
                        MyGlobal.xyzBaseCoord_Left.ZCoord = ZCoord;

                        StaticOperate.WriteXML(MyGlobal.xyzBaseCoord_Left, MyGlobal.BaseTxtPath_Left);
                        //读取Z值基准高度】
                        if (File.Exists(MyGlobal.BaseTxtPath_Left))
                        {
                            MyGlobal.xyzBaseCoord_Left = (XYZBaseCoord)StaticOperate.ReadXML(MyGlobal.BaseTxtPath_Left, typeof(XYZBaseCoord));
                        }
                    }
                }
                //判断X Y 

               
                //计算到中心点距离               
                if (isRight)
                {
                    
                    if (MyGlobal.xyzBaseCoord_Right.Dist != null && !SaveBase)
                    {
                        for (int i = 0; i < Xorigin.Count; i++)
                        {
                            if (MyGlobal.xyzBaseCoord_Right.Dist[i].Length != Xorigin[i].Length)
                            {
                                return "模板基准高度顺序与点位顺序不匹配";
                            }
                            for (int j = 0; j < Xorigin[i].Length; j++)
                            {
                                HTuple Dist = 0;
                                HOperatorSet.DistancePp(Yorigin[i][j], Xorigin[i][j], AnchorList[i].Row, AnchorList[i].Col, out Dist);
                                double xyResolution = Math.Sqrt(Xresolution * Xresolution + Yresolution * Yresolution);
                                double Dist1 = Dist.D * xyResolution;
                                double Sub = (Dist1 - MyGlobal.xyzBaseCoord_Right.Dist[i][j]);
                               
                                if (Sub > MyGlobal.globalPointSet_Right.XYMax || Sub < MyGlobal.globalPointSet_Right.XYMin)
                                {
                                    if (MyGlobal.globalConfig.isUseSelfOffset)// 启用偏移校正
                                    {
                                        HTuple hom = new HTuple();
                                        HOperatorSet.VectorAngleToRigid(MyGlobal.xyzBaseCoord_Right.intersectCoordList[i].Row, MyGlobal.xyzBaseCoord_Right.intersectCoordList[i].Col,
                                            MyGlobal.xyzBaseCoord_Right.intersectCoordList[i].Angle, AnchorList[i].Row, AnchorList[i].Col, AnchorList[i].Angle, out hom);
                                        HTuple affineX, affineY;
                                        HOperatorSet.AffineTransPoint2d(hom, MyGlobal.xyzBaseCoord_Right.XCoord[i][j], MyGlobal.xyzBaseCoord_Right.YCoord[i][j], out affineY, out affineX);
                                        HOperatorSet.AffineTransPoint2d(MyGlobal.HomMat3D_Right[i], affineX, affineY, out affineX, out affineY);
                                        XCoord[i][j][0] = affineX;
                                        YCoord[i][j][0] = affineY;
                                        string msg = NameOrigin[i][j] + $"XY--{Math.Round(Sub, 3)}超出范围已补正";
                                        ShowAndSaveMsg(msg);
                                    }
                                    else
                                    {
                                        if (MyGlobal.hWindow_Final[i] != null)
                                        {
                                            Action sw = () =>
                                            {
                                                if (MyGlobal.globalConfig.enableFeature)
                                                {
                                                    MyGlobal.hWindow_Final[i].viewWindow.dispMessage(NameOrigin[i][j] + "-XY NG", "red", Yorigin[i][j], Xorigin[i][j]);
                                                }
                                            };
                                            this.Invoke(sw);
                                        }
                                        return NameOrigin[i][j] + $"XY--{Math.Round(Sub, 3)}超出范围";
                                    }

                                }



                                #region 判断高度范围
                                                                   ////判断 Z 值高度
                                    if (MyGlobal.xyzBaseCoord_Right.ZCoord != null && MyGlobal.xyzBaseCoord_Right.ZCoord.Count != 0)
                                    {

                                        if (i >= MyGlobal.xyzBaseCoord_Right.ZCoord[i].GetLength(0))
                                        {
                                            return "请重新设置基准值";
                                        }
                                        double sub = ZCoord[i][j][0] - MyGlobal.xyzBaseCoord_Right.ZCoord[i][j][0];
                                        if (sub > MyGlobal.globalPointSet_Right.HeightMax || sub < MyGlobal.globalPointSet_Right.HeightMin)
                                        {
                                            if (MyGlobal.globalConfig.isUseSelfOffset) //启用自动补偿
                                            {
                                                ZCoord[i][j][0] = MyGlobal.xyzBaseCoord_Right.ZCoord[i][j][0];
                                                string msg = NameOrigin[i][j] + $"Z--{Math.Round(sub, 3)}超出范围已补正";
                                                ShowAndSaveMsg(msg);
                                            }
                                            else
                                            {
                                                if (MyGlobal.hWindow_Final[i] != null )
                                                {

                                                    Action sw = () =>
                                                    {
                                                        MyGlobal.hWindow_Final[i].viewWindow.dispMessage(NameOrigin[i][j] + "-Height NG", "red", Yorigin[i][j], Xorigin[i][j]);
                                                    };
                                                    MyGlobal.hWindow_Final[i].Invoke(sw);
                                                }
                                                return $"{NameOrigin[i][j]}高度超出范围" + Math.Round(sub, 3);
                                            }
                                        }
                                    }
                                
                                
                                #endregion



                            }
                        }
                    }
                }
                else
                {
                    if (MyGlobal.xyzBaseCoord_Left.Dist != null && !SaveBase)
                    {
                        for (int i = 0; i < Xorigin.Count; i++)
                        {
                            if (MyGlobal.xyzBaseCoord_Left.Dist[i].Length != Xorigin[i].Length)
                            {
                                return "模板基准高度顺序与点位顺序不匹配";
                            }
                            for (int j = 0; j < Xorigin[i].Length; j++)
                            {
                                HTuple Dist = 0;
                                HOperatorSet.DistancePp(Yorigin[i][j], Xorigin[i][j], AnchorList[i].Row, AnchorList[i].Col, out Dist);
                                double xyResolution = Math.Sqrt(Xresolution * Xresolution + Yresolution * Yresolution);
                                double Dist1 = Dist.D * xyResolution;
                                double Sub = (Dist1 - MyGlobal.xyzBaseCoord_Left.Dist[i][j]);
                               
                                if (Sub > MyGlobal.globalPointSet_Left.XYMax || Sub < MyGlobal.globalPointSet_Left.XYMin)
                                {
                                    if (MyGlobal.globalConfig.isUseSelfOffset)// 启用偏移校正
                                    {
                                        HTuple hom = new HTuple();
                                        HOperatorSet.VectorAngleToRigid(MyGlobal.xyzBaseCoord_Left.intersectCoordList[i].Row, MyGlobal.xyzBaseCoord_Left.intersectCoordList[i].Col,
                                            MyGlobal.xyzBaseCoord_Left.intersectCoordList[i].Angle, AnchorList[i].Row, AnchorList[i].Col, AnchorList[i].Angle, out hom);
                                        HTuple affineX, affineY;
                                        HOperatorSet.AffineTransPoint2d(hom, MyGlobal.xyzBaseCoord_Left.XCoord[i][j], MyGlobal.xyzBaseCoord_Left.YCoord[i][j], out affineY, out affineX);
                                        HOperatorSet.AffineTransPoint2d(MyGlobal.HomMat3D_Left[i], affineX, affineY, out affineX, out affineY);
                                        XCoord[i][j][0] = affineX;
                                        YCoord[i][j][0] = affineY;
                                        string msg = NameOrigin[i][j] + $"XY--{Math.Round(Sub, 3)}超出范围已补正";
                                        ShowAndSaveMsg(msg);
                                    }
                                    else
                                    {
                                        if (MyGlobal.hWindow_Final[i] != null && MyGlobal.globalConfig.enableFeature)
                                        {
                                            Action sw = () =>
                                            {
                                                MyGlobal.hWindow_Final[i].viewWindow.dispMessage(NameOrigin[i][j] + "-XY NG", "red", Yorigin[i][j], Xorigin[i][j]);
                                            };
                                            this.Invoke(sw);
                                        }
                                        return NameOrigin[i][j] + $"XY--{Math.Round(Sub, 3)}超出范围";
                                    }

                                    #region 判断高度范围

                                    ////判断 Z 值高度
                                    if (MyGlobal.xyzBaseCoord_Left.ZCoord != null && MyGlobal.xyzBaseCoord_Left.ZCoord.Count != 0)
                                    {

                                        if (i >= MyGlobal.xyzBaseCoord_Left.ZCoord[i].GetLength(0))
                                        {
                                            return "请重新设置基准值";
                                        }
                                        double sub = ZCoord[i][j][0] - MyGlobal.xyzBaseCoord_Left.ZCoord[i][j][0];
                                        if (sub > MyGlobal.globalPointSet_Left.HeightMax || sub < MyGlobal.globalPointSet_Left.HeightMin)
                                        {
                                            if (MyGlobal.globalConfig.isUseSelfOffset) //启用自动补偿
                                            {
                                                ZCoord[i][j][0] = MyGlobal.xyzBaseCoord_Left.ZCoord[i][j][0];
                                                string msg = NameOrigin[i][j] + $"Z--{Math.Round(sub, 3)}超出范围已补正";
                                                ShowAndSaveMsg(msg);
                                            }
                                            else
                                            {
                                                if (MyGlobal.hWindow_Final[i] != null)
                                                {

                                                    Action sw = () =>
                                                    {
                                                        MyGlobal.hWindow_Final[i].viewWindow.dispMessage(NameOrigin[i][j] + "-Height NG", "red", Yorigin[i][j], Xorigin[i][j]);
                                                    };
                                                    MyGlobal.hWindow_Final[i].Invoke(sw);
                                                }
                                                return $"{NameOrigin[i][j]}高度超出范围" + Math.Round(sub, 3);
                                            }
                                        }
                                    }

                                    #endregion


                                }
                            }
                        }
                    }
                }
                #endregion

                HTuple SubX = new HTuple(); HTuple SubY = new HTuple();
                if (isRight)
                {
                    if (MyGlobal.xyzBaseCoord_Right.Dist != null)
                    {
                        #region 重复性数据
                        //将当前数据转换到模板数据取差值                 
                        for (int i = 0; i < 4; i++)
                        {
                            HTuple HomMat = new HTuple();
                            HTuple ModelX = new HTuple(); HTuple ModelY = new HTuple();
                            HOperatorSet.VectorAngleToRigid(AnchorList[i].Row, AnchorList[i].Col, AnchorList[i].Angle, MyGlobal.xyzBaseCoord_Right.intersectCoordList[i].Row,
                                MyGlobal.xyzBaseCoord_Right.intersectCoordList[i].Col, MyGlobal.xyzBaseCoord_Right.intersectCoordList[i].Angle, out HomMat);
                            HOperatorSet.AffineTransPoint2d(HomMat, new HTuple(Yorigin[i]), new HTuple(Xorigin[i]), out ModelY, out ModelX);

                            //HTuple subX = (ModelX - MyGlobal.xyzBaseCoord.XCoord[i])*Xresolution;
                            //HTuple subY = (ModelY - MyGlobal.xyzBaseCoord.YCoord[i])*Yresolution;
                            HTuple subX = (ModelX - MyGlobal.xyzBaseCoord_Right.intersectCoordList[i].Col) * Xresolution;
                            HTuple subY = (ModelY - MyGlobal.xyzBaseCoord_Right.intersectCoordList[i].Row) * Yresolution;                          

                            SubX = SubX.TupleConcat(subX);
                            SubY = SubY.TupleConcat(subY);
                        }
                        #endregion
                    }
                }
                else
                {
                    if (MyGlobal.xyzBaseCoord_Left.Dist != null)
                    {
                        #region 重复性数据
                        //将当前数据转换到模板数据取差值                 
                        for (int i = 0; i < 4; i++)
                        {
                            HTuple HomMat = new HTuple();
                            HTuple ModelX = new HTuple(); HTuple ModelY = new HTuple();
                            HOperatorSet.VectorAngleToRigid(AnchorList[i].Row, AnchorList[i].Col, AnchorList[i].Angle, MyGlobal.xyzBaseCoord_Left.intersectCoordList[i].Row,
                                MyGlobal.xyzBaseCoord_Left.intersectCoordList[i].Col, MyGlobal.xyzBaseCoord_Left.intersectCoordList[i].Angle, out HomMat);
                            HOperatorSet.AffineTransPoint2d(HomMat, new HTuple(Yorigin[i]), new HTuple(Xorigin[i]), out ModelY, out ModelX);

                            //HTuple subX = (ModelX - MyGlobal.xyzBaseCoord.XCoord[i])*Xresolution;
                            //HTuple subY = (ModelY - MyGlobal.xyzBaseCoord.YCoord[i])*Yresolution;
                            HTuple subX = (ModelX - MyGlobal.xyzBaseCoord_Left.intersectCoordList[i].Col) * Xresolution;
                            HTuple subY = (ModelY - MyGlobal.xyzBaseCoord_Left.intersectCoordList[i].Row) * Yresolution;
                            SubX = SubX.TupleConcat(subX);
                            SubY = SubY.TupleConcat(subY);

                        }
                        #endregion
                    }
                }



                xcoord = new double[totalNum]; ycoord = new double[totalNum]; zcoord = new double[totalNum];
                keypt = new string[totalNum]; double[] orginalR = new double[totalNum]; double[] orginalC = new double[totalNum];
                int ind = 0; int ind2 = 0;
                double x0 = 0, y0 = 0, z0 = 0; string str0 = "";
                string[] sigleTitle = new string[totalNum];
                for (int i = 0; i < XCoord.Count; i++)
                {

                    for (int j = 0; j < XCoord[i].GetLength(0); j++)
                    {
                        if (XCoord[i][j] == null)
                        {
                            return $"第{i + 1}边 Points  NG";

                        }

                        HTuple row = XCoord[i][j];
                        HTuple col = YCoord[i][j];


                        //if (i == 89)
                        //{
                        //    Debug.WriteLine("xcoord" + i+"j"+j);
                        //}
                        //Debug.WriteLine("xcoord" + i + "j" + j);
                        for (int k = 0; k < XCoord[i][j].Length; k++)
                        {
                            if (k > 0)
                            {
                                if (xcoord[ind] == xcoord[ind - 1] && ycoord[ind] == ycoord[ind - 1])
                                {
                                    //存在重复点；
                                    MessageBox.Show("重复点");
                                }
                            }
                            xcoord[ind] = Math.Round(XCoord[i][j][k], 3);
                            ycoord[ind] = Math.Round(YCoord[i][j][k], 3);
                            zcoord[ind] = Math.Round(ZCoord[i][j][k], 3);

                            orginalR[ind] = Yorigin[i][j];
                            orginalC[ind] = Xorigin[i][j];

                            sigleTitle[ind] = NameOrigin[i][j];

                            //if (isRight)
                            //{
                            //    sigleTitle[ind] = MyGlobal.Right_findPointTool_Find.fParam[i].DicPointName[j];
                            //}
                            //else
                            //{
                            //    sigleTitle[ind] = MyGlobal.Left_findPointTool_Find.fParam[i].DicPointName[j];

                            //}

                            ind2++;

                            keypt[ind] = StrLorC[i][j][k];
                            if (k == 0)
                            {
                                everySeg.Add(ind, keypt[ind]);
                            }
                            ind++;
                        }

                    }

                }

                //排列起点
                //写入到文本
                StringBuilder Str = new StringBuilder();
                StringBuilder StrOrginalHeader = new StringBuilder();
                StringBuilder StrOrginalData = new StringBuilder();
                StringBuilder StrAxisData = new StringBuilder();
                StringBuilder StrRelative = new StringBuilder();
                //StringBuilder csvStr = new StringBuilder();
                StringBuilder StrWorldData = new StringBuilder();
                //重复性
                StringBuilder Repeat = new StringBuilder();
                StringBuilder RepeatHeader = new StringBuilder();
                //test
                StringBuilder pix = new StringBuilder();


                string saveTime = DateTime.Now.ToString("HHmmss");

                int Start = isRight ? MyGlobal.globalPointSet_Right.Startpt : MyGlobal.globalPointSet_Left.Startpt;
                double[] OrginalX1 = orginalC; double[] OrginalY1 = orginalR;

                #region 锚定点转换机械坐标
                HTuple[] AxisAnchorR = new HTuple[4]; HTuple[] AxisAnchorC = new HTuple[4];
                HTuple[] AnchorR = new HTuple[4]; HTuple[] AnchorC = new HTuple[4];
                HTuple[] AnchorAngle = new HTuple[4];
                for (int n = 0; n < AnchorList.Count; n++)
                {
                    AnchorR[n] = AnchorList[n].Row;
                    AnchorC[n] = AnchorList[n].Col;
                    AnchorAngle[n] = AnchorList[n].Angle;
                    HTuple homMat = isRight ? MyGlobal.HomMat3D_Right[n] : MyGlobal.HomMat3D_Left[n];
                    if (homMat == null)
                    {
                        return $"第{n + 1}边标定文件数据为空";
                    }
                    HOperatorSet.AffineTransPoint2d(homMat, AnchorList[n].Row, AnchorList[n].Col, out AxisAnchorR[n], out AxisAnchorC[n]);

                }
                #endregion
                int Acount = 0;
                for (int i = 0; i < xcoord.Length; i++)
                {
                    //if (i==89)
                    //{
                    //    Debug.WriteLine("xcoord" + i);
                    //}
                    //Debug.WriteLine(i);
                    int start = Start;
                    if (Start - 1 + i >= xcoord.Length)
                    {
                        start = Start - 1 + i - xcoord.Length;
                    }
                    else
                    {
                        start = Start - 1 + i;
                    }
                    double X1 = xcoord[start];
                    double Y1 = ycoord[start];
                    double Z1 = zcoord[start];
                    string lorc = keypt[start];

                    double PixC = 0; double PixR = 0;
                    double AxisC = 0; double AxisR = 0;
                    double PixAngle = 0;
                    if (Acount >= 0 && Acount < XCoord[0].Length)
                    {
                        PixC = AnchorC[0];
                        PixR = AnchorR[0];
                        PixAngle =AnchorAngle[0];
                        AxisC = AxisAnchorC[0];
                        AxisR = AxisAnchorR[0];
                    }
                    else if (Acount >= XCoord[0].Length && Acount < XCoord[0].Length + XCoord[1].Length)
                    {
                        PixC = AnchorC[1];
                        PixR = AnchorR[1];
                        PixAngle = AnchorAngle[1];
                        AxisC = AxisAnchorC[1];
                        AxisR = AxisAnchorR[1];
                    }
                    else if (Acount >= XCoord[0].Length + XCoord[1].Length && Acount < XCoord[0].Length + XCoord[1].Length + XCoord[2].Length)
                    {
                        PixC = AnchorC[2];
                        PixR = AnchorR[2];
                        PixAngle = AnchorAngle[2];
                        AxisC = AxisAnchorC[2];
                        AxisR = AxisAnchorR[2];
                    }
                    else if (Acount >= XCoord[0].Length + XCoord[1].Length + XCoord[2].Length && Acount < XCoord[0].Length + XCoord[1].Length + XCoord[2].Length + XCoord[3].Length)
                    {
                        PixC = AnchorC[3];
                        PixR = AnchorR[3];
                        PixAngle = AnchorAngle[3];
                        AxisC = AxisAnchorC[3];
                        AxisR = AxisAnchorR[3];
                    }
                    Acount++;




                    double xorigin = (OrginalX1[start] * Xresolution - PixC * Xresolution);
                    double yorigin = (OrginalY1[start] * Yresolution - PixR * Yresolution);

                    //test
                    double Pix_x = OrginalX1[start] * Xresolution;
                    double Pix_y = OrginalY1[start] * Yresolution;

                    double Xrelative = X1 - AxisC;
                    double Yrelative = Y1 - AxisR;
                    double Xrelative1 = 0;
                    double Yrelative1 = 0;

                    HTuple angle1, angle2;
                    HOperatorSet.AngleLx(OrginalY1[start], OrginalX1[start], PixR, PixC, out angle1);//线角度
                    HOperatorSet.LineOrientation(OrginalY1[start], OrginalX1[start], PixR, PixC, out angle1);
                    angle2 = PixAngle;//料角度
                    if (angle2<-Math.PI/2)
                    {
                        angle2 = angle2 + Math.PI;
                    }
                    else if(angle2> Math.PI/2)
                    {
                        angle2 = Math.PI - angle2 ;
                    }
                    if (angle1<0)
                    {
                        angle1 = angle1 + Math.PI;
                    }

                    double subAngle =Math.Abs( Math.Abs(angle1.D) - Math.Abs(angle2.D));
                    HTuple dist = 0;
                    HOperatorSet.DistancePp(OrginalY1[start], OrginalX1[start], PixR, PixC, out dist);
                    Xrelative1 = dist.D * Math.Sin(subAngle)* Xresolution;
                    Yrelative1 = dist.D * Math.Cos(subAngle) * Yresolution;

                    //if (isRight)
                    //{
                        

                    //    Xrelative1 = MyGlobal.xyzBaseCoord_Right.Dist == null ? 0 : SubX[i].D;
                    //    Yrelative1 = MyGlobal.xyzBaseCoord_Right.Dist == null ? 0 : SubY[i].D;
                        
                    //}
                    //else
                    //{
                    //    Xrelative1 = MyGlobal.xyzBaseCoord_Left.Dist == null ? 0 : SubX[i].D;
                    //    Yrelative1 = MyGlobal.xyzBaseCoord_Left.Dist == null ? 0 : SubY[i].D;
                   
                    //}

                 
                    if (i == 0)
                    {
                        x0 = X1;
                        y0 = Y1;
                        z0 = Z1;
                    }
                    if (i == xcoord.Length - 2)
                    {
                        str0 = lorc;
                    }


                    Str.Append((i + 1).ToString() + "," + X1.ToString("0.000") + "," + Y1.ToString("0.000") + "," + Z1.ToString("0.000") + "," + lorc + "\r\n");
                    //csvStr.Append(X1.ToString("0.000") + "," + Y1.ToString("0.000") + "," + Z1.ToString("0.000") + ",");

                    if (i == 0)
                    {
                        StrOrginalHeader.Append("Time" + "\t" + sigleTitle[start] + "_X" + "\t" + sigleTitle[start] + "_Y" + "\t" + sigleTitle[start] + "_Z" + "\t");
                        StrOrginalData.Append(saveTime + "\t" + xorigin.ToString("0.000") + "\t" + yorigin.ToString("0.000") + "\t" + Z1.ToString("0.000") + "\t");
                        StrAxisData.Append(saveTime + "\t" + Xrelative.ToString("0.000") + "\t" + Yrelative.ToString("0.000") + "\t" + Z1.ToString("0.000") + "\t");
                        StrRelative.Append(saveTime + "\t" + Xrelative1.ToString("0.000") + "\t" + Yrelative1.ToString("0.000") + "\t" + Z1.ToString("0.000") + "\t");
                        StrWorldData.Append(saveTime + "\t" + X1.ToString("0.000") + "\t" + Y1.ToString("0.000") + "\t" + "0.000" + "\t");
                        //重复性
                        if (sigleTitle[start].Contains("C"))
                        {
                            RepeatHeader.Append("Time" + "\t" + sigleTitle[start] + "_X" + "\t" + sigleTitle[start] + "_Y" + "\t");
                            Repeat.Append(saveTime + "\t" + Xrelative1.ToString("0.000") + "\t" + Yrelative1.ToString("0.000") + "\t");
                        }
                        else
                        {
                            RepeatHeader.Append("Time" + "\t" + sigleTitle[start] + "_X" + "\t");
                            Repeat.Append(saveTime + "\t" + Xrelative1.ToString("0.000") + "\t");
                        }


                        //test
                        pix.Append(saveTime + "\t" + Pix_x.ToString("0.000") + "\t" + Pix_y.ToString("0.000") + "\t" + Z1.ToString("0.000") + "\t");
                    }
                    else
                    {

                        StrOrginalHeader.Append(sigleTitle[start] + "_X" + "\t" + sigleTitle[start] + "_Y" + "\t" + sigleTitle[start] + "_Z" + "\t");
                        StrOrginalData.Append(xorigin.ToString("0.000") + "\t" + yorigin.ToString("0.000") + "\t" + Z1.ToString("0.000") + "\t");
                        StrAxisData.Append(Xrelative.ToString("0.000") + "\t" + Yrelative.ToString("0.000") + "\t" + Z1.ToString("0.000") + "\t");
                        StrRelative.Append(Xrelative1.ToString("0.000") + "\t" + Yrelative1.ToString("0.000") + "\t" + Z1.ToString("0.000") + "\t");

                        StrWorldData.Append(X1.ToString("0.000") + "\t" + Y1.ToString("0.000") + "\t" + "0.000" + "\t");
                        //重复性
                        if (sigleTitle[start].Contains("C"))
                        {
                            RepeatHeader.Append(sigleTitle[start] + "_X" + "\t" + sigleTitle[start] + "_Y" +"\t");
                            Repeat.Append(Xrelative1.ToString("0.000") + "\t" + Yrelative1.ToString("0.000")  + "\t");
                        }
                        else
                        {
                            RepeatHeader.Append(sigleTitle[start] + "_X" + "\t");
                            Repeat.Append(Xrelative1.ToString("0.000") + "\t");
                        }

                        //test
                        pix.Append(Pix_x.ToString("0.000") + "\t" + Pix_y.ToString("0.000") + "\t" + Z1.ToString("0.000") + "\t");
                    }

                    if (Side == 4 && i == xcoord.Length - 1)
                    {
                        StrOrginalHeader.Append("\r\n");
                        StrOrginalData.Append("\r\n");
                        StrAxisData.Append("\r\n");
                        StrRelative.Append("\r\n");
                        StrWorldData.Append("\r\n");
                        //重复性
                        RepeatHeader.Append("\r\n");
                        Repeat.Append("\r\n");

                        //test
                        pix.Append("\r\n");
                    }
                }
                string strlast = str0;
                //int len1 = XCoord[Side - 1].GetLength(0);

                //if (XCoord[Side - 1][len1 - 1] != null)
                //{
                //    strlast = StrLorC[Side - 1][len1 - 1][0];
                //}
                //else
                //{
                //    strlast = StrLorC[Side - 1][len1 - 2][0];

                //}

                Str.Append((totalNum + 1).ToString() + "," + x0.ToString("0.000") + "," + y0.ToString("0.000") + "," + z0.ToString("0.000") + "," + strlast + "\r\n");
                //csvStr.Append(x0.ToString("0.000") + "," + y0.ToString("0.000") + "," + z0.ToString("0.000")+"\r\n");

                //File.AppendAllText($"Data//Excel//{DateTime.Now.ToString("yyyy年7月dd日")}//Index.csv", csvStr.ToString());
                //StaticOperate.writeTxt($"Data//Excel//{DateTime.Now.ToString("yyyy年7月dd日")}//Index.csv", csvStr.ToString());
                //StaticOperate.writeTxt("D:\\Laser3D_1.txt", Str.ToString());
                //C:\IT7000\data\11\C#@Users@AR9XX@Desktop@PK@guiji@3d
                //if (!Directory.Exists("C:\\Twinwin\\data\\Right"))
                //{
                //    Directory.CreateDirectory("C:\\Twinwin\\data\\Right");
                //}
                //StaticOperate.writeTxt("C:\\Twinwin\\data\\Right\\Laser3D_1.txt", Str.ToString());

                string LorR = MyGlobal.IsRight ? "Right" : "Left";
                string Path = MyGlobal.IsRight ? "C:\\IT7000\\data\\11\\C#@IT7000@db@RightStationFile\\" : "C:\\IT7000\\data\\11\\C#@IT7000@db@LeftStationFile\\";
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                StaticOperate.writeTxt(Path + "Laser3D_1.txt", Str.ToString());

                if (Side == 4)
                {
                    StaticOperate.SaveExcelData(StrOrginalHeader.ToString(), StrOrginalData.ToString(), $"{LorR}_Origin");
                    StaticOperate.SaveExcelData(StrOrginalHeader.ToString(), StrAxisData.ToString(), $"{LorR}_Axis");
                    StaticOperate.SaveExcelData(StrOrginalHeader.ToString(), StrRelative.ToString(), $"{LorR}_Relative");
                    StaticOperate.SaveExcelData(StrOrginalHeader.ToString(), pix.ToString(), $"{LorR}_pix");
                    StaticOperate.SaveExcelData(RepeatHeader.ToString(), Repeat.ToString(), $"{LorR}_Repeatability");
                    StaticOperate.SaveExcelData(StrOrginalHeader.ToString(), StrWorldData.ToString(), $"{LorR}_WorldIndex");

                    for (int i = 0; i < 4; i++)
                    {
                        HTuple deg = 0;
                        HOperatorSet.TupleDeg(AnchorList[i].Angle, out deg);

                        string AnchorX = Math.Round(AnchorList[i].Col * Xresolution, 3).ToString(); string AnchorY = Math.Round(AnchorList[i].Row * Yresolution, 3).ToString();

                        if (i == 3)
                        {
                            StaticOperate.SaveExcelData(LorR, AnchorX, AnchorY, deg.D.ToString() + "\r\n");
                        }
                        else
                        {
                            StaticOperate.SaveExcelData(LorR, AnchorX, AnchorY, deg.D.ToString() + "\t");
                        }
                    }


                    if (MyGlobal.globalConfig.enableFeature)
                    {
                        ShowProfileToWindow(xcoord, ycoord, zcoord, sigleTitle, true, true);
                    }

                    //HObject cross = new HObject();
                    //HOperatorSet.GenCrossContourXld(out cross, centerR * 20 - 4000, centerC * 20, 30, 0);                   
                    //Action sw = () =>
                    //{
                    //    ShowProfile.viewWindow.displayHobject(cross, "red", true, 10);
                    //};
                    //this.Invoke(sw);
                }



                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "RunSide error :" + ex.Message + RowNum + "  请检查生成点位顺序" ;
            }
        }


        private double[] recordXCoord; double[] recordYCoord; double[] recordZCoord; string[] recordSigleTitle;
        public void ShowProfileToWindow(double[] xcoord, double[] ycoord, double[] zcoord, string[] sigleTitle, bool isRun, bool showMsg)
        {
            if (isRun)
            {
                this.recordXCoord = xcoord;
                this.recordYCoord = ycoord;
                this.recordZCoord = zcoord;
                this.recordSigleTitle = sigleTitle;
            }
            if (recordXCoord == null || recordYCoord == null || recordSigleTitle == null || recordXCoord.Length == 0 || recordYCoord.Length == 0 || recordSigleTitle.Length == 0)
            {
                return;
            }
            ClassShow3D cs3d = new ClassShow3D();
            float[] dx = new float[recordXCoord.Length];
            float[] dy = new float[recordYCoord.Length];
            float[] dz = new float[recordZCoord.Length];
            for (int i = 0; i < dx.Length; i++)
            {
                dx[i] = Convert.ToSingle((recordXCoord[i] - 200).ToString());
                dy[i] = Convert.ToSingle(recordYCoord[i].ToString());
                dz[i] = Convert.ToSingle(recordZCoord[i].ToString());
            }
            ClassShow3D.breakOut = true;
            ShowProfile.HalconWindow.ClearWindow();
            cs3d.Show3D(dx, dy, dz, ShowProfile.HalconWindow);


            /*
            HObject regpot;
            HTuple newRecordX = new HTuple(recordXCoord) * 20 - 4000;
            HOperatorSet.GenRegionPoints(out regpot, new HTuple(newRecordX) , new HTuple(recordYCoord) * 20);
          
            HObject ImageConst;
            HOperatorSet.GenImageConst(out ImageConst, "byte", 5000, 5000);
            Action sw = () =>
            {
                ShowProfile.HobjectToHimage(ImageConst);
                ShowProfile.viewWindow.displayHobject(regpot, "green", true, 20);
            };
            this.Invoke(sw);
            


            if (!showMsg)
            {
                for (int i = 0; i < recordSigleTitle.Length; i++)
                {
                    
                    Action sw1 = () =>
                    {
                        ShowProfile.viewWindow.dispMessage($"{recordSigleTitle[i]} +({recordZCoord[i]})", "blue", newRecordX[i], recordYCoord[i] * 20);
                    };
                    this.Invoke(sw1);
                }
            }
            HObject contour;
            newRecordX = newRecordX.TupleConcat(recordXCoord[0] * 20 - 4000);
            HOperatorSet.GenContourPolygonXld(out contour, new HTuple(newRecordX), new HTuple(recordYCoord, recordYCoord[0]) * 20);
            //HOperatorSet.GenContourPolygonXld(out contour1, new HTuple(recordXCoord[recordXCoord.Length - 1],new HTuple(recordYCoord[0));
            Action sw2 = () =>
            {
                //ShowProfile.viewWindow.displayHobject(contour, "white");


                
            };
            this.Invoke(sw2);
            

            contour.Dispose();

            regpot.Dispose();
            ImageConst.Dispose();
            */
        }




        bool TcpIsConnect = false;
        //string[] JobName = { "R_1_zi", "R_2_zi", "R_3_zi", "R_4_zi" };
        string[] JobName;
        int Side = 0;
        Stopwatch sp = new Stopwatch();


        private bool isLastImgRecOK = true;
        bool OutlineTest = false;
        [Obsolete]
        public void TcpClientListen_Surface_IOTrigger()
        {
            int nSent = 0;
            //while (true)
            //{


            MyGlobal.sktClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(MyGlobal.globalConfig.MotorIpAddress);
            try
            {
                MyGlobal.sktClient.Connect(ip, MyGlobal.globalConfig.MotorPort);
                ShowAndSaveMsg(string.Format("已连接{0}:{1}", MyGlobal.globalConfig.MotorIpAddress, MyGlobal.globalConfig.MotorPort.ToString()));
                TcpIsConnect = true;
                MyGlobal.sktOK = true;
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                ShowAndSaveMsg(string.Format("本地服务器连接失败！"));
                TcpIsConnect = false;
                MyGlobal.sktOK = false;
                return;
            }


            byte[] buffer = new byte[128];
            byte[] ok = new byte[128];
            byte[] ng = new byte[128];
            //Sendmsg = "Chat|ok";

            Stopwatch spRunTime = new Stopwatch();

            //ok = Encoding.UTF8.GetBytes(Sendmsg);
            while (true)
            {
                int len = MyGlobal.sktClient.Receive(buffer);
                try
                {
                    if (MyGlobal.globalConfig.enableAlign)
                    {
                        JobName = new string[] { "R_1_zi_align", "R_2_zi_align", "R_3_zi_align", "R_4_zi_align" };
                    }
                    else { JobName = new string[] { "R_1_zi", "R_2_zi", "R_3_zi", "R_4_zi" }; }

                    byte[] temp = new byte[len];
                    Array.Copy(buffer, temp, len);
                    MyGlobal.ReceiveMsg = Encoding.UTF8.GetString(temp);
                    if (MyGlobal.ReceiveMsg == "Test")
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            string test = RunSuface(i + 1, true, true);
                            ShowAndSaveMsg(test);
                        }
                    }

                    if (MyGlobal.ReceiveMsg.Contains("POS"))
                    {
                        continue;
                    }
                    if (len == 0)
                    {
                        ShowAndSaveMsg(string.Format("服务器已断开连接！"));
                        MyGlobal.sktOK = false;
                        break;
                    }
                    else
                    {
                        ShowAndSaveMsg(string.Format("收到数据{0}", MyGlobal.ReceiveMsg));
                    }
                 

                    if (true)
                    {

                        string ReturnStr = "";
                        if (MyGlobal.ReceiveMsg.Contains("1") || MyGlobal.ReceiveMsg.Contains("2") || MyGlobal.ReceiveMsg.Contains("3") || MyGlobal.ReceiveMsg.Contains("4"))
                        {
                            Side = Convert.ToInt32(MyGlobal.ReceiveMsg.Substring(0, 1));
                            ReturnStr = MyGlobal.ReceiveMsg.Remove(0, 1);
                        }

                        if (MyGlobal.ReceiveMsg.Contains("1"))
                        {
                            for (int i = 0; i < MyGlobal.hWindow_Final.Length; i++)
                            {
                                MyGlobal.hWindow_Final[i].ClearWindow();
                            }
                            ShowProfile.HalconWindow.ClearWindow();
                            if (MyGlobal.ReceiveMsg.Contains("Start"))
                            {
                                string LorR = MyGlobal.IsRight ? "Right" : "Left";
                                if (MyGlobal.globalConfig.isSaveFileDat)
                                {
                                    MyGlobal.GoSDK.SaveDatFileDirectory = MyGlobal.SaveDatFileDirectory + LorR + "\\" + DateTime.Now.ToString("yyyy年MM月dd日") + "\\" + DateTime.Now.ToString("HHmmssff") + "\\";
                                }
                                else { MyGlobal.GoSDK.SaveDatFileDirectory = null; }
                                if (MyGlobal.globalConfig.isSaveKdat)
                                {
                                    MyGlobal.GoSDK.SaveKdatDirectoy = MyGlobal.SaveKdatDirectoy + LorR + "\\";
                                }
                                else { MyGlobal.GoSDK.SaveKdatDirectoy = null; }
                                spRunTime.Start();
                            }
                        }
                        ok = Encoding.UTF8.GetBytes(ReturnStr + "_OK");
                        ng = Encoding.UTF8.GetBytes(ReturnStr + "_NG");

                        switch (ReturnStr)
                        {
                            case "Start":
                                MyGlobal.GoSDK.IsRecSurfaceDataZByte = !MyGlobal.isShowHeightImg;
                                Stopwatch sp1 = new Stopwatch();
                                sp1.Start();
                                if (MyGlobal.GoSDK.ProfileList != null)
                                {
                                    MyGlobal.GoSDK.ProfileList.Clear();
                                }

                                while (!isLastImgRecOK)
                                {

                                }
                                ShowAndSaveMsg(" Wait Data time:--->" + sp1.ElapsedMilliseconds.ToString());
                                //打开激光
                                MyGlobal.GoSDK.EnableProfle = false;
                                if (Side == 1)
                                {
                                    string Msg3 = "关闭激光";
                                    if (MyGlobal.GoSDK.Stop(ref Msg3))
                                    {
                                        ShowAndSaveMsg($"关闭激光成功！");
                                    }
                                    string Cutjob1 = "切换作业";
                                    if (MyGlobal.GoSDK.CutJob(JobName[Side - 1], ref Cutjob1))
                                    {
                                        ShowAndSaveMsg($"切换作业 {JobName[Side - 1]} 成功！");
                                    }
                                    string Msg20 = "";
                                    if (MyGlobal.GoSDK.Start(ref Msg20))
                                    {
                                        ShowAndSaveMsg($"打开激光成功！----");
                                        //Thread.Sleep(200);
                                    }
                                }


                                ShowAndSaveMsg($"起始编码器数值 --- 》{ MyGlobal.GoSDK.GetSensorEncode()}");
                                string Msg = "开始扫描:" + Side.ToString();
                                if ((!Directory.Exists(MyGlobal.GoSDK.SaveDatFileDirectory)) && MyGlobal.globalConfig.isSaveFileDat)
                                {
                                    Directory.CreateDirectory(MyGlobal.GoSDK.SaveDatFileDirectory);
                                }
                                if ((!Directory.Exists(MyGlobal.GoSDK.SaveKdatDirectoy)) && MyGlobal.globalConfig.isSaveKdat)
                                {
                                    Directory.CreateDirectory(MyGlobal.GoSDK.SaveKdatDirectoy);
                                }

                                MyGlobal.GoSDK.RunSide = Side.ToString();
                                
                                ShowAndSaveMsg(Msg);

                                ShowAndSaveMsg(" Start space time:--->" + sp1.ElapsedMilliseconds.ToString());
                                sp1.Reset();
                                nSent = MyGlobal.sktClient.Send(ok);
                                break;
                            case "Stop":
                                isLastImgRecOK = false;
                                //关闭激光
                                ShowAndSaveMsg($"结束编码器数值1 --- 》{ MyGlobal.GoSDK.GetSensorEncode()}");



                                if (Side < 4)//给运动机构信号，执行下一次扫描
                                {
                                    MyGlobal.sktClient.Send(Encoding.UTF8.GetBytes("Stop_OK"));
                                }

                                MyGlobal.GoSDK.EnableProfle = false;
                                sp.Start();
                                while (MyGlobal.GoSDK.SurfaceDataZ == null || MyGlobal.GoSDK.SurfaceDataIntensity == null)
                                {
                                    if (sp.ElapsedMilliseconds > 10000)
                                    {
                                        sp.Reset();
                                        ShowAndSaveMsg($"图像接收超时！");
                                        break;
                                    }
                                }
                                if (MyGlobal.globalConfig.enableAlign)
                                {
                                    sp.Start();
                                    while (MyGlobal.GoSDK.SurfaceAlignData == null)
                                    {
                                        if (sp.ElapsedMilliseconds > 10000)
                                        {
                                            sp.Reset();
                                            ShowAndSaveMsg($"图像接收超时！");
                                            break;
                                        }
                                    }
                                }
                                ShowAndSaveMsg($"接收图像耗时-->{sp.ElapsedMilliseconds}");
                                sp.Reset();

                                string Msg2 = "扫描结束";
                                if (MyGlobal.GoSDK.Stop(ref Msg2))
                                {
                                    ShowAndSaveMsg($"关闭激光成功！");
                                }

                                
                                if (Side < 4)
                                {
                                    int jobIdx = Side == 4 ? 0 : Side;
                                    string Cutjob = "切换作业";
                                    if (MyGlobal.GoSDK.CutJob(JobName[jobIdx], ref Cutjob))
                                    {
                                        ShowAndSaveMsg($"切换作业 {JobName[jobIdx]} 成功！");
                                    }
                        
                                    string Msg1 = "";
                                    if (MyGlobal.GoSDK.Start(ref Msg1))
                                    {
                                        ShowAndSaveMsg($"打开激光成功！----");
                                        //Thread.Sleep(200);
                                    }
                                }
                                
                                ShowAndSaveMsg(Msg2);
                                Action RunDetect = () =>
                                {
                                    sp.Restart();
                                    string ok1 = RunSuface(Side, true);
                                    sp.Stop();
                                    ShowAndSaveMsg(sp.ElapsedMilliseconds.ToString());
                                    if (ok1 != "OK")
                                    {
                                        ShowAndSaveMsg(ok1);                                        
                                        ShowAndSaveMsg("计算点位失败！");
                                        MyGlobal.sktClient.Send(ng);
                                       

                                    }
                                    else
                                    {
                                        if (Side == 4)
                                        {
                                            ShowAndSaveMsg($"输出点位成功！运行时间：{spRunTime.ElapsedMilliseconds.ToString()}");
                                            spRunTime.Reset();
                                            MyGlobal.sktClient.Send(ok);
                                        }
                                    }
                                };
                                //if (Side == 4)
                                //{
                                //}
                                this.Invoke(RunDetect);

                                break;

                        }
                    }

                }
                catch (Exception ex)
                {
                    string a = ex.StackTrace;
                    int ind = a.IndexOf("行号");
                    int start = ind;
                    string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                    ShowAndSaveMsg("TCP_ListenSurface-->" + ex.Message + RowNum);
                }

            }

            //}
        }
        [Obsolete]
        public void TcpClientListen_Surface()
        {
            int nSent = 0;
            //while (true)
            //{


            MyGlobal.sktClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(MyGlobal.globalConfig.MotorIpAddress);
            try
            {
                MyGlobal.sktClient.Connect(ip, MyGlobal.globalConfig.MotorPort);
                ShowAndSaveMsg(string.Format("已连接{0}:{1}", MyGlobal.globalConfig.MotorIpAddress, MyGlobal.globalConfig.MotorPort.ToString()));
                TcpIsConnect = true;
                MyGlobal.sktOK = true;
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                ShowAndSaveMsg(string.Format("本地服务器连接失败！"));
                TcpIsConnect = false;
                MyGlobal.sktOK = false;
                return;
            }


            byte[] buffer = new byte[128];
            byte[] ok = new byte[128];
            byte[] ng = new byte[128];
            //Sendmsg = "Chat|ok";

            Stopwatch spRunTime = new Stopwatch();

            //ok = Encoding.UTF8.GetBytes(Sendmsg);
            while (true)
            {
                int len = MyGlobal.sktClient.Receive(buffer);
                try
                {
                    if (MyGlobal.globalConfig.enableAlign)
                    {
                        JobName = new string[] { "R_1_zi_align", "R_2_zi_align", "R_3_zi_align", "R_4_zi_align" };
                    }
                    else { JobName = new string[] { "R_1_zi", "R_2_zi", "R_3_zi", "R_4_zi" }; }

                    byte[] temp = new byte[len];
                    Array.Copy(buffer, temp, len);
                    MyGlobal.ReceiveMsg = Encoding.UTF8.GetString(temp);
                    if (MyGlobal.ReceiveMsg == "Test")
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            string test = RunSuface(i + 1, true, true);
                            ShowAndSaveMsg(test);
                        }
                    }

                    if (MyGlobal.ReceiveMsg.Contains("POS"))
                    {
                        continue;
                    }
                    if (len == 0)
                    {
                        ShowAndSaveMsg(string.Format("服务器已断开连接！"));
                        MyGlobal.sktOK = false;
                        break;
                    }
                    else
                    {
                        ShowAndSaveMsg(string.Format("收到数据{0}", MyGlobal.ReceiveMsg));
                    }


                    if (true)
                    {

                        string ReturnStr = "";
                        if (MyGlobal.ReceiveMsg.Contains("1") || MyGlobal.ReceiveMsg.Contains("2") || MyGlobal.ReceiveMsg.Contains("3") || MyGlobal.ReceiveMsg.Contains("4"))
                        {
                            Side = Convert.ToInt32(MyGlobal.ReceiveMsg.Substring(0, 1));
                            ReturnStr = MyGlobal.ReceiveMsg.Remove(0, 1);
                        }

                        if (MyGlobal.ReceiveMsg.Contains("1"))
                        {
                            for (int i = 0; i < MyGlobal.hWindow_Final.Length; i++)
                            {
                                MyGlobal.hWindow_Final[i].ClearWindow();
                            }
                            ShowProfile.HalconWindow.ClearWindow();
                            if (MyGlobal.ReceiveMsg.Contains("Start"))
                            {
                                string LorR = MyGlobal.IsRight ? "Right" : "Left";
                                if (MyGlobal.globalConfig.isSaveFileDat)
                                {
                                    MyGlobal.GoSDK.SaveDatFileDirectory = MyGlobal.SaveDatFileDirectory + LorR + "\\" + DateTime.Now.ToString("yyyy年MM月dd日") + "\\" + DateTime.Now.ToString("HHmmssff") + "\\";
                                }
                                else { MyGlobal.GoSDK.SaveDatFileDirectory = null; }
                                if (MyGlobal.globalConfig.isSaveKdat)
                                {
                                    MyGlobal.GoSDK.SaveKdatDirectoy = MyGlobal.SaveKdatDirectoy + LorR + "\\";
                                }
                                else { MyGlobal.GoSDK.SaveKdatDirectoy = null; }
                                spRunTime.Start();
                            }
                        }
                        ok = Encoding.UTF8.GetBytes(ReturnStr + "_OK");
                        ng = Encoding.UTF8.GetBytes(ReturnStr + "_NG");

                        switch (ReturnStr)
                        {
                            case "Start":
                                MyGlobal.GoSDK.IsRecSurfaceDataZByte = !MyGlobal.isShowHeightImg;
                                Stopwatch sp1 = new Stopwatch();
                                sp1.Start();
                                if (MyGlobal.GoSDK.ProfileList != null)
                                {
                                    MyGlobal.GoSDK.ProfileList.Clear();
                                }

                                while (!isLastImgRecOK)
                                {

                                }
                                ShowAndSaveMsg(" Wait Data time:--->" + sp1.ElapsedMilliseconds.ToString());
                                //打开激光
                                MyGlobal.GoSDK.EnableProfle = false;
                                if (Side == 1)
                                {
                                    //string Msg3 = "关闭激光";
                                    //if (MyGlobal.GoSDK.Stop(ref Msg3))
                                    //{
                                    //    ShowAndSaveMsg($"关闭激光成功！");
                                    //}
                                    string Cutjob1 = "切换作业";
                                    if (MyGlobal.GoSDK.CutJob(JobName[Side - 1], ref Cutjob1))
                                    {
                                        ShowAndSaveMsg($"切换作业 {JobName[Side - 1]} 成功！");
                                    }
                                }


                                ShowAndSaveMsg($"起始编码器数值 --- 》{ MyGlobal.GoSDK.GetSensorEncode()}");
                                string Msg = "开始扫描:" + Side.ToString();
                                if ((!Directory.Exists(MyGlobal.GoSDK.SaveDatFileDirectory)) && MyGlobal.globalConfig.isSaveFileDat)
                                {
                                    Directory.CreateDirectory(MyGlobal.GoSDK.SaveDatFileDirectory);
                                }
                                if ((!Directory.Exists(MyGlobal.GoSDK.SaveKdatDirectoy)) && MyGlobal.globalConfig.isSaveKdat)
                                {
                                    Directory.CreateDirectory(MyGlobal.GoSDK.SaveKdatDirectoy);
                                }

                                MyGlobal.GoSDK.RunSide = Side.ToString();
                                if (MyGlobal.GoSDK.Start(ref Msg))
                                {
                                    ShowAndSaveMsg($"打开激光成功！----");
                                    //Thread.Sleep(200);
                                }
                                ShowAndSaveMsg(Msg);

                                ShowAndSaveMsg(" Start space time:--->" + sp1.ElapsedMilliseconds.ToString());
                                sp1.Reset();
                                nSent = MyGlobal.sktClient.Send(ok);
                                break;
                            case "Stop":
                                isLastImgRecOK = false;
                                //关闭激光
                                ShowAndSaveMsg($"结束编码器数值1 --- 》{ MyGlobal.GoSDK.GetSensorEncode()}");



                                if (Side < 4)//给运动机构信号，执行下一次扫描
                                {
                                    MyGlobal.sktClient.Send(Encoding.UTF8.GetBytes("Stop_OK"));
                                }

                                MyGlobal.GoSDK.EnableProfle = false;
                                sp.Start();
                                while (MyGlobal.GoSDK.SurfaceDataZ == null || MyGlobal.GoSDK.SurfaceDataIntensity == null)
                                {
                                    if (sp.ElapsedMilliseconds > 10000)
                                    {
                                        sp.Reset();
                                        ShowAndSaveMsg($"图像接收超时！");
                                        break;
                                    }
                                }
                                if (MyGlobal.globalConfig.enableAlign)
                                {
                                    sp.Start();
                                    while (MyGlobal.GoSDK.SurfaceAlignData == null)
                                    {
                                        if (sp.ElapsedMilliseconds > 10000)
                                        {
                                            sp.Reset();
                                            ShowAndSaveMsg($"图像接收超时！");
                                            break;
                                        }
                                    }
                                }
                                ShowAndSaveMsg($"接收图像耗时-->{sp.ElapsedMilliseconds}");
                                sp.Reset();

                                string Msg2 = "扫描结束";
                                if (MyGlobal.GoSDK.Stop(ref Msg2))
                                {
                                    ShowAndSaveMsg($"关闭激光成功！");
                                }

                                int jobIdx = Side == 4 ? 0 : Side;
                                string Cutjob = "切换作业";
                                if (MyGlobal.GoSDK.CutJob(JobName[jobIdx], ref Cutjob))
                                {
                                    ShowAndSaveMsg($"切换作业 {JobName[jobIdx]} 成功！");
                                }

                                ShowAndSaveMsg(Msg2);
                                Action RunDetect = () =>
                                {
                                    sp.Restart();
                                    string ok1 = RunSuface(Side, true);
                                    sp.Stop();
                                    ShowAndSaveMsg(sp.ElapsedMilliseconds.ToString());
                                    if (ok1 != "OK")
                                    {
                                        ShowAndSaveMsg(ok1);
                                        //if (Side == 4)
                                        //{
                                        ShowAndSaveMsg("输出点位失败！");
                                        MyGlobal.sktClient.Send(ng);
                                        //}

                                    }
                                    else
                                    {
                                        if (Side == 4)
                                        {
                                            ShowAndSaveMsg($"输出点位成功！运行时间：{spRunTime.ElapsedMilliseconds.ToString()}");
                                            spRunTime.Reset();
                                            MyGlobal.sktClient.Send(ok);
                                        }
                                    }
                                };

                                this.Invoke(RunDetect);

                                break;

                        }
                    }

                }
                catch (Exception ex)
                {
                    string a = ex.StackTrace;
                    int ind = a.IndexOf("行号");
                    int start = ind;
                    string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                    ShowAndSaveMsg("TCP_ListenSurface-->" + ex.Message + RowNum);
                }

            }

            //}
        }
        [Obsolete]
        private void TcpSeverListen_Surface()
        {
            int nSent = 0;
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(MyGlobal.globalConfig.MotorIpAddress), MyGlobal.globalConfig.MotorPort);
                MyGlobal.sktServer = new Socket(ipPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                MyGlobal.sktServer.Bind(ipPoint);
                MyGlobal.sktServer.Listen(1);
                ShowAndSaveMsg(string.Format("服务器启动成功！IP:<{0}> Port:<{1}>", MyGlobal.globalConfig.MotorIpAddress, MyGlobal.globalConfig.MotorPort));
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                ShowAndSaveMsg(string.Format("服务器启动失败！"));
                TcpIsConnect = false;
                MyGlobal.sktOK = false;
                return;
            }
            while (true)
            {
                try
                {

                    MyGlobal.sktClient = MyGlobal.sktServer.Accept();
                    IPEndPoint ipEP = (IPEndPoint)MyGlobal.sktClient.RemoteEndPoint;
                    //MyGlobal.sktClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  
                    //EndPoint ipEP = MyGlobal.sktClient.RemoteEndPoint;
                    //MyGlobal.sktClient.Connect(ipEP);

                    TcpIsConnect = true;
                    ShowAndSaveMsg(string.Format("客户端已连接{0}:{1}", ipEP.Address.ToString(), ipEP.Port));
                    byte[] buffer = new byte[128];
                    byte[] ok = new byte[128];
                    byte[] ng = new byte[128];
                    //Sendmsg = "Chat|ok";
                    Stopwatch spRunTime = new Stopwatch();
                    //ok = Encoding.UTF8.GetBytes(Sendmsg);
                    while (true)
                    {
                        int len = MyGlobal.sktClient.Receive(buffer);

                        if (len == 0)
                        {
                            ShowAndSaveMsg(string.Format("客户端已断开连接！"));
                            break;
                        }
                        else
                        {
                            ShowAndSaveMsg(string.Format("收到数据{0}", MyGlobal.ReceiveMsg));
                        }

                        try
                        {
                            if (MyGlobal.globalConfig.enableAlign)
                            {
                                JobName = new string[] { "R_1_zi_align", "R_2_zi_align", "R_3_zi_align", "R_4_zi_align" };
                            }
                            else { JobName = new string[] { "R_1_zi", "R_2_zi", "R_3_zi", "R_4_zi" }; }

                            byte[] temp = new byte[len];
                            Array.Copy(buffer, temp, len);
                            MyGlobal.ReceiveMsg = Encoding.UTF8.GetString(temp);
                            if (MyGlobal.ReceiveMsg == "Test")
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    string test = RunSuface(i + 1, true, true);
                                    ShowAndSaveMsg(test);
                                }
                            }

                            if (MyGlobal.ReceiveMsg.Contains("POS"))
                            {
                                continue;
                            }
                            //if (len == 0)
                            //{
                            //    ShowAndSaveMsg(string.Format("服务器已断开连接！"));
                            //    MyGlobal.sktOK = false;
                            //    break;
                            //}
                            //else
                            //{
                            //    ShowAndSaveMsg(string.Format("收到数据{0}", MyGlobal.ReceiveMsg));
                            //}


                            if (true)
                            {

                                string ReturnStr = "";
                                if (MyGlobal.ReceiveMsg.Contains("1") || MyGlobal.ReceiveMsg.Contains("2") || MyGlobal.ReceiveMsg.Contains("3") || MyGlobal.ReceiveMsg.Contains("4"))
                                {
                                    Side = Convert.ToInt32(MyGlobal.ReceiveMsg.Substring(0, 1));
                                    ReturnStr = MyGlobal.ReceiveMsg.Remove(0, 1);
                                }

                                if (MyGlobal.ReceiveMsg.Contains("1"))
                                {
                                    for (int i = 0; i < MyGlobal.hWindow_Final.Length; i++)
                                    {
                                        MyGlobal.hWindow_Final[i].ClearWindow();
                                    }
                                    ShowProfile.HalconWindow.ClearWindow();
                                    if (MyGlobal.ReceiveMsg.Contains("Start"))
                                    {
                                        string LorR = MyGlobal.IsRight ? "Right" : "Left";
                                        if (MyGlobal.globalConfig.isSaveFileDat)
                                        {
                                            //MyGlobal.GoSDK.SaveDatFileDirectory = MyGlobal.SaveDatFileDirectory + LorR + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\\";
                                            MyGlobal.GoSDK.SaveDatFileDirectory = MyGlobal.SaveDatFileDirectory + LorR + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + DateTime.Now.ToString("HHmmssff") + "\\";
                                        }
                                        else { MyGlobal.GoSDK.SaveDatFileDirectory = null; }
                                        if (MyGlobal.globalConfig.isSaveKdat)
                                        {
                                            MyGlobal.GoSDK.SaveKdatDirectoy = MyGlobal.SaveKdatDirectoy + LorR + "\\";
                                        }
                                        else { MyGlobal.GoSDK.SaveKdatDirectoy = null; }
                                        spRunTime.Start();
                                    }
                                }
                                ok = Encoding.UTF8.GetBytes(ReturnStr + "_OK");
                                ng = Encoding.UTF8.GetBytes(ReturnStr + "_NG");

                                switch (ReturnStr)
                                {
                                    case "Start":
                                        MyGlobal.GoSDK.IsRecSurfaceDataZByte = !MyGlobal.isShowHeightImg;
                                        Stopwatch sp1 = new Stopwatch();
                                        sp1.Start();
                                        if (MyGlobal.GoSDK.ProfileList != null)
                                        {
                                            MyGlobal.GoSDK.ProfileList.Clear();
                                        }

                                        while (!isLastImgRecOK)
                                        {

                                        }
                                        ShowAndSaveMsg(" Wait Data time:--->" + sp1.ElapsedMilliseconds.ToString());
                                        //打开激光
                                        MyGlobal.GoSDK.EnableProfle = false;
                                        if (Side == 1)
                                        {
                                            //string Msg3 = "关闭激光";
                                            //if (MyGlobal.GoSDK.Stop(ref Msg3))
                                            //{
                                            //    ShowAndSaveMsg($"关闭激光成功！");
                                            //}
                                            string Cutjob1 = "切换作业";
                                            if (MyGlobal.GoSDK.CutJob(JobName[Side - 1], ref Cutjob1))
                                            {
                                                ShowAndSaveMsg($"切换作业 {JobName[Side - 1]} 成功！");
                                            }
                                        }


                                        ShowAndSaveMsg($"起始编码器数值 --- 》{ MyGlobal.GoSDK.GetSensorEncode()}");
                                        string Msg = "开始扫描:" + Side.ToString();
                                        if ((!Directory.Exists(MyGlobal.GoSDK.SaveDatFileDirectory)) && MyGlobal.globalConfig.isSaveFileDat)
                                        {
                                            Directory.CreateDirectory(MyGlobal.GoSDK.SaveDatFileDirectory);
                                        }
                                        if ((!Directory.Exists(MyGlobal.GoSDK.SaveKdatDirectoy)) && MyGlobal.globalConfig.isSaveKdat)
                                        {
                                            Directory.CreateDirectory(MyGlobal.GoSDK.SaveKdatDirectoy);
                                        }

                                        MyGlobal.GoSDK.RunSide = Side.ToString();
                                        if (MyGlobal.GoSDK.Start(ref Msg))
                                        {
                                            ShowAndSaveMsg($"打开激光成功！----");
                                            //Thread.Sleep(200);
                                        }
                                        ShowAndSaveMsg(Msg);

                                        ShowAndSaveMsg(" Start space time:--->" + sp1.ElapsedMilliseconds.ToString());
                                        sp1.Reset();
                                        nSent = MyGlobal.sktClient.Send(ok);
                                        break;
                                    case "Stop":
                                        isLastImgRecOK = false;
                                        //关闭激光
                                        ShowAndSaveMsg($"结束编码器数值1 --- 》{ MyGlobal.GoSDK.GetSensorEncode()}");



                                        if (Side < 4)//给运动机构信号，执行下一次扫描
                                        {
                                            MyGlobal.sktClient.Send(Encoding.UTF8.GetBytes("Stop_OK"));
                                        }

                                        MyGlobal.GoSDK.EnableProfle = false;
                                        sp.Start();
                                        while (MyGlobal.GoSDK.SurfaceDataZ == null || MyGlobal.GoSDK.SurfaceDataIntensity == null)
                                        {
                                            if (sp.ElapsedMilliseconds > 10000)
                                            {
                                                sp.Reset();
                                                ShowAndSaveMsg($"图像接收超时！");
                                                break;
                                            }
                                        }
                                        if (MyGlobal.globalConfig.enableAlign)
                                        {
                                            sp.Start();
                                            while (MyGlobal.GoSDK.SurfaceAlignData == null)
                                            {
                                                if (sp.ElapsedMilliseconds > 10000)
                                                {
                                                    sp.Reset();
                                                    ShowAndSaveMsg($"图像接收超时！");
                                                    break;
                                                }
                                            }
                                        }
                                        ShowAndSaveMsg($"接收图像耗时-->{sp.ElapsedMilliseconds}");
                                        sp.Reset();

                                        string Msg2 = "扫描结束";
                                        if (MyGlobal.GoSDK.Stop(ref Msg2))
                                        {
                                            ShowAndSaveMsg($"关闭激光成功！");
                                        }

                                        int jobIdx = Side == 4 ? 0 : Side;
                                        string Cutjob = "切换作业";
                                        if (MyGlobal.GoSDK.CutJob(JobName[jobIdx], ref Cutjob))
                                        {
                                            ShowAndSaveMsg($"切换作业 {JobName[jobIdx]} 成功！");
                                        }

                                        ShowAndSaveMsg(Msg2);
                                        Action RunDetect = () =>
                                        {
                                            sp.Restart();
                                            string ok1 = RunSuface(Side, true);
                                            sp.Stop();
                                            ShowAndSaveMsg(sp.ElapsedMilliseconds.ToString());
                                            if (ok1 != "OK")
                                            {
                                                ShowAndSaveMsg(ok1);
                                                //if (Side == 4)
                                                //{
                                                ShowAndSaveMsg("输出点位失败！");
                                                MyGlobal.sktClient.Send(ng);
                                                //}

                                            }
                                            else
                                            {
                                                if (Side == 4)
                                                {
                                                    ShowAndSaveMsg($"输出点位成功！运行时间：{spRunTime.ElapsedMilliseconds.ToString()}");
                                                    spRunTime.Reset();
                                                    MyGlobal.sktClient.Send(ok);
                                                }
                                            }
                                        };

                                        this.Invoke(RunDetect);

                                        break;

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            string a = ex.StackTrace;
                            int ind = a.IndexOf("行号");
                            int start = ind;
                            string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                            ShowAndSaveMsg("TCP_ListenSurface-->" + ex.Message + RowNum);
                        }

                    }

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        bool ShowOnece = false;
        private void Listen_Surface()
        {
            int nSent = 0;
            TcpStart();
            while (true)
            {
                try
                {
                    if (!MyGlobal.globalConfig.IsTcpClient) //Server
                    {
                        MyGlobal.sktClient = MyGlobal.sktServer.Accept();
                        IPEndPoint ipEP = (IPEndPoint)MyGlobal.sktClient.RemoteEndPoint;

                        TcpIsConnect = true;
                        MyGlobal.sktOK = true;
                        ShowAndSaveMsg(string.Format("客户端已连接{0}:{1}", ipEP.Address.ToString(), ipEP.Port));
                    }
                    
                    byte[] buffer = new byte[128];
                    byte[] ok = new byte[128];
                    byte[] ng = new byte[128];
                    //Sendmsg = "Chat|ok";
                    Stopwatch spRunTime = new Stopwatch();
                    //ok = Encoding.UTF8.GetBytes(Sendmsg);
                    while (true)
                    {
                        int len = MyGlobal.sktClient.Receive(buffer);

                        if (len == 0)
                        {
                            if (MyGlobal.globalConfig.IsTcpClient)
                            {
                                if (!ShowOnece)
                                {
                                    ShowOnece = true;
                                    ShowAndSaveMsg(string.Format("服务器已断开连接！"));
                                    TcpIsConnect = false;
                                    MyGlobal.sktOK = false;
                                }
                                
                            }
                            else
                            {
                                if (!ShowOnece)
                                {
                                    ShowOnece = true;
                                    ShowAndSaveMsg(string.Format("客户端已断开连接！"));
                                    TcpIsConnect = false;
                                    MyGlobal.sktOK = false;
                                }
                               
                            }
                            break;
                        }
                        else
                        {
                            ShowAndSaveMsg(string.Format("收到数据{0}", MyGlobal.ReceiveMsg));
                        }

                        try
                        {
                           

                            byte[] temp = new byte[len];
                            Array.Copy(buffer, temp, len);
                            MyGlobal.ReceiveMsg = Encoding.UTF8.GetString(temp);
                            if (MyGlobal.ReceiveMsg == "Test")
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    string test = RunSuface(i + 1, true, true);
                                    ShowAndSaveMsg(test);
                                }
                            }

                            if (MyGlobal.ReceiveMsg.Contains("POS"))
                            {
                                continue;
                            }
                            //if (len == 0)
                            //{
                            //    ShowAndSaveMsg(string.Format("服务器已断开连接！"));
                            //    MyGlobal.sktOK = false;
                            //    break;
                            //}
                            //else
                            //{
                            //    ShowAndSaveMsg(string.Format("收到数据{0}", MyGlobal.ReceiveMsg));
                            //}
                            if (!MyGlobal.SensorConnected)
                            {
                                ShowAndSaveMsg("Sensor 未连接！");
                                continue;
                            }
                            if (MyGlobal.ReceiveMsg.Contains("Right"))
                            {
                                while (!isLastImgRecOK)
                                {

                                }
                                byte[] ok1 = Encoding.UTF8.GetBytes(MyGlobal.ReceiveMsg + "_OK");
                                MyGlobal.sktClient.Send(ok1);
                                MyGlobal.IsRight = true;
                            }
                            if (MyGlobal.ReceiveMsg.Contains("Left"))
                            {
                                while (!isLastImgRecOK)
                                {

                                }
                                byte[] ok2 = Encoding.UTF8.GetBytes(MyGlobal.ReceiveMsg + "_OK");
                                MyGlobal.sktClient.Send(ok2);
                                MyGlobal.IsRight = false;
                            }

                            if (MyGlobal.globalConfig.enableAlign)
                            {
                                if (MyGlobal.IsRight)
                                {
                                    JobName = new string[] { "R_1_zi_align", "R_2_zi_align", "R_3_zi_align", "R_4_zi_align" };
                                }
                                else
                                {
                                    JobName = new string[] { "L_1_zi_align", "L_2_zi_align", "L_3_zi_align", "L_4_zi_align" };

                                }
                            }
                            else
                            {
                                if (MyGlobal.IsRight)
                                {
                                    JobName = new string[] { "R_1_zi", "R_2_zi", "R_3_zi", "R_4_zi" };
                                }
                                else
                                {
                                    JobName = new string[] { "L_1_zi", "L_2_zi", "L_3_zi", "L_4_zi" };

                                }
                            }



                            if (true)
                            {

                                string ReturnStr = "";
                                if (MyGlobal.ReceiveMsg.Contains("1") || MyGlobal.ReceiveMsg.Contains("2") || MyGlobal.ReceiveMsg.Contains("3") || MyGlobal.ReceiveMsg.Contains("4"))
                                {
                                    Side = Convert.ToInt32(MyGlobal.ReceiveMsg.Substring(0, 1));
                                    ReturnStr = MyGlobal.ReceiveMsg.Remove(0, 1);
                                }

                                if (MyGlobal.ReceiveMsg.Contains("1"))
                                {
                                    for (int i = 0; i < MyGlobal.hWindow_Final.Length; i++)
                                    {
                                        MyGlobal.hWindow_Final[i].ClearWindow();
                                    }
                                    ShowProfile.HalconWindow.ClearWindow();
                                    if (MyGlobal.ReceiveMsg.Contains("Start"))
                                    {
                                        string LorR = MyGlobal.IsRight ? "Right" : "Left";
                                        if (MyGlobal.globalConfig.isSaveFileDat)
                                        {
                                            //MyGlobal.GoSDK.SaveDatFileDirectory = MyGlobal.SaveDatFileDirectory + LorR + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\\";
                                            MyGlobal.GoSDK.SaveDatFileDirectory = MyGlobal.SaveDatFileDirectory + LorR + "\\" + DateTime.Now.ToString("yyyy年MM月dd日") + "\\" + DateTime.Now.ToString("HHmmssff") + "\\";
                                        }
                                        else { MyGlobal.GoSDK.SaveDatFileDirectory = null; }
                                        if (MyGlobal.globalConfig.isSaveKdat)
                                        {
                                            MyGlobal.GoSDK.SaveKdatDirectoy = MyGlobal.SaveKdatDirectoy + LorR + "\\";
                                        }
                                        else { MyGlobal.GoSDK.SaveKdatDirectoy = null; }
                                        spRunTime.Start();
                                        MyGlobal.GoSDK.sddList_L.Clear();
                                        MyGlobal.GoSDK.sddList_I.Clear();
                                        MyGlobal.GoSDK.sddList_S.Clear();
                                    }

                                    
                                }
                                ok = Encoding.UTF8.GetBytes(ReturnStr + "_OK");
                                ng = Encoding.UTF8.GetBytes(ReturnStr + "_NG");

                                switch (ReturnStr)
                                {
                                    case "Start":
                                        MyGlobal.GoSDK.IsRecSurfaceDataZByte = !MyGlobal.isShowHeightImg;
                                        Stopwatch sp1 = new Stopwatch();
                                        sp1.Start();
                                        if (MyGlobal.GoSDK.ProfileList != null)
                                        {
                                            MyGlobal.GoSDK.ProfileList.Clear();
                                        }

                                        while (!isLastImgRecOK)
                                        {

                                        }
                                        ShowAndSaveMsg(" Wait Data time:--->" + sp1.ElapsedMilliseconds.ToString());
                                        //打开激光
                                        MyGlobal.GoSDK.EnableProfle = false;
                                        MyGlobal.GoSDK.IsOnline = true;
                                        if (Side == 1)
                                        {
                                            //string Msg3 = "关闭激光";
                                            //if (MyGlobal.GoSDK.Stop(ref Msg3))
                                            //{
                                            //    ShowAndSaveMsg($"关闭激光成功！");
                                            //}
                                            string Cutjob1 = "切换作业";
                                            if (MyGlobal.GoSDK.CutJob(JobName[Side - 1], ref Cutjob1))
                                            {
                                                ShowAndSaveMsg($"切换作业 {JobName[Side - 1]} 成功！");
                                            }
                                            else
                                            {
                                                ShowAndSaveMsg($"切换作业 {JobName[Side - 1]} 失败！");
                                            }
                                        }


                                        ShowAndSaveMsg($"起始编码器数值 --- 》{ MyGlobal.GoSDK.GetSensorEncode()}");
                                        string Msg = "开始扫描:" + Side.ToString();
                                        if ((!Directory.Exists(MyGlobal.GoSDK.SaveDatFileDirectory)) && MyGlobal.globalConfig.isSaveFileDat)
                                        {
                                            Directory.CreateDirectory(MyGlobal.GoSDK.SaveDatFileDirectory);
                                        }
                                        if ((!Directory.Exists(MyGlobal.GoSDK.SaveKdatDirectoy)) && MyGlobal.globalConfig.isSaveKdat)
                                        {
                                            Directory.CreateDirectory(MyGlobal.GoSDK.SaveKdatDirectoy);
                                        }

                                        MyGlobal.GoSDK.RunSide = Side.ToString();
                                        if (MyGlobal.GoSDK.Start(ref Msg))
                                        {
                                            ShowAndSaveMsg($"打开激光成功！----");
                                            //Thread.Sleep(200);
                                        }
                                        ShowAndSaveMsg(Msg);

                                        ShowAndSaveMsg(" Start space time:--->" + sp1.ElapsedMilliseconds.ToString());
                                        sp1.Reset();
                                        nSent = MyGlobal.sktClient.Send(ok);
                                        break;
                                    case "Stop":
                                        isLastImgRecOK = false;
                                        //关闭激光
                                        ShowAndSaveMsg($"结束编码器数值1 --- 》{ MyGlobal.GoSDK.GetSensorEncode()}");



                                        if (Side < 4)//给运动机构信号，执行下一次扫描
                                        {
                                            MyGlobal.sktClient.Send(Encoding.UTF8.GetBytes("Stop_OK"));
                                        }

                                        MyGlobal.GoSDK.EnableProfle = false;
                                        sp.Start();
                                        while (MyGlobal.GoSDK.SurfaceDataZ == null || MyGlobal.GoSDK.SurfaceDataIntensity == null)
                                        {
                                            if (sp.ElapsedMilliseconds > 10000)
                                            {
                                                sp.Reset();
                                                ShowAndSaveMsg($"图像接收超时！");
                                                break;
                                            }
                                        }
                                        if (MyGlobal.globalConfig.enableAlign)
                                        {
                                            sp.Start();
                                            while (MyGlobal.GoSDK.SurfaceAlignData == null)
                                            {
                                                if (sp.ElapsedMilliseconds > 10000)
                                                {
                                                    sp.Reset();
                                                    ShowAndSaveMsg($"图像接收超时！");
                                                    break;
                                                }
                                            }
                                        }
                                        ShowAndSaveMsg($"接收图像耗时-->{sp.ElapsedMilliseconds}");
                                        sp.Reset();

                                        string Msg2 = "扫描结束";
                                        if (MyGlobal.GoSDK.Stop(ref Msg2))
                                        {
                                            MyGlobal.GoSDK.IsOnline = false;
                                            ShowAndSaveMsg($"关闭激光成功！");
                                        }

                                        int jobIdx = Side == 4 ? 0 : Side;
                                        string Cutjob = "切换作业";
                                        if (MyGlobal.GoSDK.CutJob(JobName[jobIdx], ref Cutjob))
                                        {
                                            ShowAndSaveMsg($"切换作业 {JobName[jobIdx]} 成功！");
                                        }
                                        else
                                        {                                           
                                            ShowAndSaveMsg($"切换作业 {JobName[jobIdx]} 失败！");
                                           
                                        }

                                        ShowAndSaveMsg(Msg2);
                                        Action RunDetect = () =>
                                        {
                                            sp.Restart();
                                            string ok1 = RunSuface(Side, MyGlobal.IsRight);
                                            sp.Stop();
                                            ShowAndSaveMsg(sp.ElapsedMilliseconds.ToString());
                                            if (ok1 != "OK")
                                            {
                                                ShowAndSaveMsg(ok1);
                                                if (Side == 4)
                                                {
                                                ShowAndSaveMsg("输出点位失败！");
                                                MyGlobal.sktClient.Send(ng);
                                                }

                                            }
                                            else
                                            {
                                                if (Side == 4)
                                                {
                                                    ShowAndSaveMsg($"输出点位成功！运行时间：{spRunTime.ElapsedMilliseconds.ToString()}");
                                                    spRunTime.Reset();
                                                    MyGlobal.sktClient.Send(ok);
                                                }
                                            }
                                        };

                                        this.Invoke(RunDetect);

                                        break;

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            string a = ex.StackTrace;
                            int ind = a.IndexOf("行号");
                            int start = ind;
                            string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                            ShowAndSaveMsg("Listen_Surface-->" + ex.Message + RowNum);
                            TcpIsConnect = false;
                            MyGlobal.sktOK = false;
                        }

                    }

                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }

        void TcpStart()
        {
            if (MyGlobal.globalConfig.IsTcpClient) //客户端
            {
                MyGlobal.sktClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(MyGlobal.globalConfig.MotorIpAddress);
                try
                {
                    MyGlobal.sktClient.Connect(ip, MyGlobal.globalConfig.MotorPort);
                    ShowAndSaveMsg(string.Format("已连接{0}:{1}", MyGlobal.globalConfig.MotorIpAddress, MyGlobal.globalConfig.MotorPort.ToString()));
                    TcpIsConnect = true;
                    MyGlobal.sktOK = true;
                }
                catch (Exception ex)
                {
                    string a = ex.StackTrace;
                    int ind = a.IndexOf("行号");
                    int start = ind;
                    string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                    ShowAndSaveMsg(string.Format("本地服务器连接失败！"));
                    TcpIsConnect = false;
                    MyGlobal.sktOK = false;
                    return;
                }
            }
            else
            {
                try
                {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(MyGlobal.globalConfig.MotorIpAddress), MyGlobal.globalConfig.MotorPort);
                    MyGlobal.sktServer = new Socket(ipPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    MyGlobal.sktServer.Bind(ipPoint);
                    MyGlobal.sktServer.Listen(1);
                    ShowAndSaveMsg(string.Format("服务器启动成功！IP:<{0}> Port:<{1}>", MyGlobal.globalConfig.MotorIpAddress, MyGlobal.globalConfig.MotorPort));
                }
                catch (Exception ex)
                {
                    string a = ex.StackTrace;
                    int ind = a.IndexOf("行号");
                    int start = ind;
                    string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                    ShowAndSaveMsg(string.Format("服务器启动失败！"));
                    TcpIsConnect = false;
                    MyGlobal.sktOK = false;
                    return;
                }
            }
        }



        [Obsolete]
        public void TcpClientListen()
        {
            int nSent = 0;
            //while (true)
            //{
            try
            {

                MyGlobal.sktClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(MyGlobal.globalConfig.MotorIpAddress);
                try
                {
                    MyGlobal.sktClient.Connect(ip, MyGlobal.globalConfig.MotorPort);
                    ShowAndSaveMsg(string.Format("已连接{0}:{1}", MyGlobal.globalConfig.MotorIpAddress, MyGlobal.globalConfig.MotorPort.ToString()));
                    TcpIsConnect = true;
                    MyGlobal.sktOK = true;
                }
                catch (Exception ex)
                {
                    string a = ex.StackTrace;
                    int ind = a.IndexOf("行号");
                    int start = ind;
                    string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                    ShowAndSaveMsg(string.Format("连接服务器失败！" + ex.Message + RowNum));
                    TcpIsConnect = false;
                    MyGlobal.sktOK = false;
                    return;
                }


                byte[] buffer = new byte[128];
                byte[] ok = new byte[128];
                byte[] ng = new byte[128];
                //Sendmsg = "Chat|ok";

                //ok = Encoding.UTF8.GetBytes(Sendmsg);
                while (true)
                {
                    int len = MyGlobal.sktClient.Receive(buffer);

                    byte[] temp = new byte[len];
                    Array.Copy(buffer, temp, len);
                    MyGlobal.ReceiveMsg = Encoding.UTF8.GetString(temp);
                    if (MyGlobal.ReceiveMsg.Contains("POS"))
                    {
                        continue;
                    }
                    if (len == 0)
                    {
                        ShowAndSaveMsg(string.Format("服务器已断开连接！"));
                        MyGlobal.sktOK = false;
                        break;
                    }
                    else
                    {
                        ShowAndSaveMsg(string.Format("收到数据{0}", MyGlobal.ReceiveMsg));
                    }
                    if (true)
                    {

                        string ReturnStr = "";
                        if (MyGlobal.ReceiveMsg.Contains("1") || MyGlobal.ReceiveMsg.Contains("2") || MyGlobal.ReceiveMsg.Contains("3") || MyGlobal.ReceiveMsg.Contains("4"))
                        {
                            Side = Convert.ToInt32(MyGlobal.ReceiveMsg.Substring(0, 1));
                            ReturnStr = MyGlobal.ReceiveMsg.Remove(0, 1);
                        }

                        ok = Encoding.UTF8.GetBytes(ReturnStr + "_OK");
                        ng = Encoding.UTF8.GetBytes(ReturnStr + "_NG");

                        switch (ReturnStr)
                        {

                            //case "1":
                            //    Side = 1;
                            //    break;
                            //case "2":
                            //    Side =2;
                            //    break;
                            //case "3":
                            //    Side = 3;
                            //    break;
                            //case "4":
                            //    Side = 4;
                            //    break;
                            case "Start":
                                if (MyGlobal.GoSDK.ProfileList != null)
                                {
                                    MyGlobal.GoSDK.ProfileList.Clear();

                                }

                                //打开激光
                                MyGlobal.GoSDK.EnableProfle = true;
                                string Msg = "开始扫描:" + Side.ToString();
                                MyGlobal.GoSDK.RunSide = Side.ToString();
                                MyGlobal.GoSDK.Start(ref Msg);
                                ShowAndSaveMsg(Msg);
                                nSent = MyGlobal.sktClient.Send(ok);
                                break;
                            case "Stop":
                                //关闭激光
                                string Msg2 = "扫描结束";
                                MyGlobal.GoSDK.Stop(ref Msg2);
                                MyGlobal.GoSDK.EnableProfle = false;
                                ShowAndSaveMsg(Msg2);
                                Action RunDetect = () =>
                                {
                                    string ok1 = Run(Side);
                                    //byte[] SaveSend = new byte[128];

                                    if (ok1 != "OK")
                                    {
                                        ShowAndSaveMsg(ok1);
                                        if (Side == 4)
                                        {
                                            //SaveSend = Encoding.UTF8.GetBytes("SAVE_NG");
                                            ShowAndSaveMsg("输出点位失败！");
                                            MyGlobal.sktClient.Send(ng);
                                        }

                                    }
                                    else
                                    {
                                        if (Side == 4)
                                        {
                                            //SaveSend = Encoding.UTF8.GetBytes("SAVE_OK");
                                            ShowAndSaveMsg("输出点位成功！");
                                            MyGlobal.sktClient.Send(ok);
                                        }
                                    }
                                };

                                this.Invoke(RunDetect);
                                nSent = MyGlobal.sktClient.Send(ok);
                                break;
                                //case "Start_Right":
                                //    //打开激光
                                //    string Msg3 = "开始扫描";
                                //    MyGlobal.GoSDK.Start(ref Msg3);
                                //    ShowAndSaveMsg(Msg3);

                                //    nSent = MyGlobal.sktClient.Send(ok);
                                //    break;
                                //case "Stop_Right":
                                //    //关闭激光
                                //    string Msg4 = "扫描结束";
                                //    MyGlobal.GoSDK.Start(ref Msg4);
                                //    ShowAndSaveMsg(Msg4);
                                //    Action RunDetect1 = () =>
                                //    {
                                //        string ok2 = Run(1);
                                //        if (ok2 != "OK")
                                //        {
                                //            ShowAndSaveMsg(ok2);
                                //        }
                                //    };
                                //    this.Invoke(RunDetect1);
                                //    nSent = MyGlobal.sktClient.Send(ok);
                                //    break;

                        }

                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            //}
        }
        [Obsolete]
        private void TcpListen()
        {
            int nSent = 0;
            while (true)
            {
                try
                {

                    MyGlobal.sktClient = MyGlobal.sktServer.Accept();
                    IPEndPoint ipEP = (IPEndPoint)MyGlobal.sktClient.RemoteEndPoint;
                    //MyGlobal.sktClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  
                    //EndPoint ipEP = MyGlobal.sktClient.RemoteEndPoint;
                    //MyGlobal.sktClient.Connect(ipEP);

                    TcpIsConnect = true;
                    ShowAndSaveMsg(string.Format("客户端已连接{0}:{1}", ipEP.Address.ToString(), ipEP.Port));
                    byte[] buffer = new byte[128];
                    byte[] ok = new byte[128];
                    //Sendmsg = "Chat|ok";

                    //ok = Encoding.UTF8.GetBytes(Sendmsg);
                    while (true)
                    {
                        int len = MyGlobal.sktClient.Receive(buffer);

                        byte[] temp = new byte[len];
                        Array.Copy(buffer, temp, len);
                        MyGlobal.ReceiveMsg = Encoding.UTF8.GetString(temp);
                        if (len == 0)
                        {
                            ShowAndSaveMsg(string.Format("客户端已断开连接！"));
                            break;
                        }
                        else
                        {
                            ShowAndSaveMsg(string.Format("收到数据{0}", MyGlobal.ReceiveMsg));
                        }
                        if (true)
                        {
                            ok = Encoding.UTF8.GetBytes(MyGlobal.ReceiveMsg + "_OK");
                            switch (MyGlobal.ReceiveMsg)
                            {
                                case "Start_Left":
                                    //打开激光
                                    string Msg = "开始扫描";
                                    MyGlobal.GoSDK.Start(ref Msg);
                                    ShowAndSaveMsg(Msg);

                                    nSent = MyGlobal.sktClient.Send(ok);
                                    break;
                                case "Stop_Left":
                                    //关闭激光
                                    string Msg2 = "扫描结束";
                                    MyGlobal.GoSDK.Stop(ref Msg2);
                                    ShowAndSaveMsg(Msg2);
                                    Action RunDetect = () =>
                                    {
                                        string ok1 = Run(0);
                                        if (ok1 != "OK")
                                        {
                                            ShowAndSaveMsg(ok1);
                                        }

                                    };
                                    this.Invoke(RunDetect);

                                    nSent = MyGlobal.sktClient.Send(ok);
                                    break;
                                case "Start_Right":
                                    //打开激光
                                    string Msg3 = "开始扫描";
                                    MyGlobal.GoSDK.Start(ref Msg3);
                                    ShowAndSaveMsg(Msg3);

                                    nSent = MyGlobal.sktClient.Send(ok);
                                    break;
                                case "Stop_Right":
                                    //关闭激光
                                    string Msg4 = "扫描结束";
                                    MyGlobal.GoSDK.Start(ref Msg4);
                                    ShowAndSaveMsg(Msg4);
                                    Action RunDetect1 = () =>
                                    {
                                        string ok2 = Run(1);
                                        if (ok2 != "OK")
                                        {
                                            ShowAndSaveMsg(ok2);
                                        }
                                    };
                                    this.Invoke(RunDetect1);
                                    nSent = MyGlobal.sktClient.Send(ok);
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
        }

        Communication.Communication cmu = new Communication.Communication();
        private void navBarItem1_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //通信设置
            cmu.ShowDialog();

        }

        private void navBarItem8_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Sensor.ShowForm Online = new Sensor.ShowForm();
            Online.Show();
        }

        private void navBarItem3_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Sensor.SensorSet sensor = new Sensor.SensorSet();
            sensor.ShowDialog();
        }

        private void navBarItem9_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            try
            {
                VisionTool.BlobForm blobForm = new VisionTool.BlobForm();
                blobForm.Show();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void navBarItem4_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            VisionTool.LineCircleForm lcForm = new VisionTool.LineCircleForm();
            lcForm.Show();
        }

        private void navBarItem10_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            VisionTool.CalibrationForm calibration = new VisionTool.CalibrationForm();
            calibration.Show();
            //VisionTool.CalibFormMain calib = new CalibFormMain();
            //calib.ShowDialog();
        }

        //Matching.Form1 match = new Matching.Form1(MyGlobal.fileName1);


        private void navBarItem11_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            MyGlobal.flset2.ToolType = "GlueGuide";
            MyGlobal.flset2.FindType = "Fix";
            MyGlobal.flset2.ShowDialog();



            //string path2 = MyGlobal.fileName1.Replace(".shm", ".roi");
            //List<ViewWindow.Model.ROI> roilist = new List<ViewWindow.Model.ROI>();
            //if (File.Exists(path2))
            //{
            //    HWindow_Final temp = new HWindow_Final();
            //    temp.viewWindow.loadROI(path2, out roilist);
            //    MyGlobal.mAssistant[0].PreHandleRoi = roilist[0];
            //    MyGlobal.mAssistant[0].Roi = roilist[1];
            //}
            //match.ShowDialog();

        }


        private void tabbedView1_DocumentActivated(object sender, DevExpress.XtraBars.Docking2010.Views.DocumentEventArgs e)
        {
            //if (tabbedView1.ActiveDocument.Caption == document6.Caption)
            //{
            //    show3D.LoadZRec();
            //}
            //else
            //{
            //    show3D.ShieldRec();
            //}

        }
        //VisionTool.FindMax[] FindMax = new VisionTool.FindMax[2];

        private void navBarItem5_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //FindMax[0].ShowDialog();
        }


        public void RunOffline(string ImagePath,bool ReduceKdata)
        {


            try
            {


                string OK1 = LoadImageData(ImagePath,ReduceKdata);
                if (OK1 != "OK")
                {
                    MessageBox.Show(OK1);
                }
                for (int i = 0; i < 4; i++)
                {
                    string OK = RunOutLine(i + 1, i);

                    if (OK != "OK")
                    {
                        ShowAndSaveMsg(OK);
                        if (errstr.Length > 0)
                        {
                            ShowAndSaveErrorMsg(errstr.ToString(), ImagePath);
                        }
                        break;
                    }

                    if (i == 3)
                    {
                        ShowAndSaveMsg(OK);
                        if (errstr.Length > 0)
                        {
                            ShowAndSaveErrorMsg(errstr.ToString(), ImagePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                ShowAndSaveMsg("RunOffline :" + ex.Message + RowNum);
            }

        }

        public void RunBaseHeight()
        {
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    for (int i = 0; i < 4; i++)
                    {
                        string OK = RunOutLine(i + 1, i, true);
                        if (OK != "OK")
                        {
                            ShowAndSaveMsg(OK);
                            break;
                        }
                        if (i == 3)
                        {
                            ShowAndSaveMsg(OK);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                ShowAndSaveMsg("RunOffline :" + ex.Message + RowNum);
            }

        }


        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            barButtonItem9.Enabled = false;
            ThreadPool.QueueUserWorkItem(delegate
            {

                //手动测试
                if (MyGlobal.ImageMulti.Count == 0)
                    MessageBox.Show("请加载选择手动运行图片！");
                for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
                {
                    string OK = RunOutLine(i + 1, i);
                    if (OK != "OK")
                    {
                        ShowAndSaveMsg(OK);
                        barButtonItem9.Enabled = true;
                    }
                    if (i == 3)
                    {
                        ShowAndSaveMsg(OK);
                        barButtonItem9.Enabled = true;
                    }
                }

            });

            //if (OK == "OK" && i == 3)
            //{
            //    MyGlobal.sktClient.Send(Encoding.UTF8.GetBytes("Stop" + "_OK"));
            //}
            //else
            //{
            //    MyGlobal.sktClient.Send(Encoding.UTF8.GetBytes("Stop" + "_NG"));

            //}


            //for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
            //{
            //    //MyGlobal.hWindow_Final[0].HobjectToHimage(MyGlobal.ImageMulti[i]);
            //    //string OK = RunDetect(1, MyGlobal.ImageMulti[i]);


            //}

            //if (true)
            //{
            //    if (!Directory.Exists(MyGlobal.DataPath + "Image\\"))
            //    {
            //        Directory.CreateDirectory(MyGlobal.DataPath + "Image\\");
            //    }
            //    string path = MyGlobal.DataPath + "Image\\Image" + DateTime.Now.ToString("HH_mm_ss_ffff");
            //    HOperatorSet.WriteImage(MyGlobal.hWindow_Final[0].Image, "tiff", 0, path + ".tiff");

            //}

        }

        List<int> sidelist = new List<int>();
        //private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    //打开图片
        //    OpenFileDialog openfile = new OpenFileDialog();
        //    openfile.Multiselect = true;
        //    if (openfile.ShowDialog() == DialogResult.OK)
        //    {
        //        ShowProfile.HalconWindow.ClearWindow();
        //        for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
        //        {
        //            for (int j = 0; j < 2; j++)
        //            {
        //                if (MyGlobal.ImageMulti[i][j] != null)
        //                {
        //                    MyGlobal.ImageMulti[i][j].Dispose();
        //                }

        //            }

        //        }
        //        MyGlobal.ImageMulti.Clear();
        //        sidelist.Clear();
        //        int len = openfile.FileNames.Length;

        //        if (openfile.FileName.Contains("Left"))
        //        {
        //            MyGlobal.IsRight = false;
        //        }
        //        else
        //        {
        //            MyGlobal.IsRight = true;
        //        }

        //        string[] namesI;
        //        string[] namesH;
        //        string[] namesB;
        //        if (len < 10)
        //        {
        //            namesI = new string[len / 2];
        //            namesH = new string[len / 2];
        //            namesB = null;
        //        }
        //        else
        //        {
        //            namesI = new string[len / 3];
        //            namesH = new string[len / 3];
        //            namesB = new string[len / 3];
        //        }
        //        if (len < 2)
        //        {
        //            return;
        //        }

        //        if (Path.GetExtension(openfile.FileNames[0]) == ".dat")
        //        {
        //            int orderi = 0; int orderh = 0;
        //            foreach (var item in openfile.FileNames)
        //            {
        //                if (item.Contains("H.dat"))
        //                {
        //                    namesH[orderh] = item;
        //                    orderh++;
        //                }
        //                else if (item.Contains("I.dat"))
        //                {
        //                    namesI[orderi] = item;
        //                    orderi++;
        //                }
        //            }
        //            if (namesH[0] == null)
        //            {
        //                return;
        //            }

        //            for (int i = 0; i < namesH.Length; i++)
        //            {
        //                HObject[] image = new HObject[2];

        //                HObject zoomRgbImg, zoomHeightImg, zoomIntensityImg;

        //                SurfaceZSaveDat ssd = (SurfaceZSaveDat)StaticTool.ReadSerializable(namesH[i], typeof(SurfaceZSaveDat));

        //                SurfaceIntensitySaveDat sid = (SurfaceIntensitySaveDat)StaticTool.ReadSerializable(namesI[i], typeof(SurfaceIntensitySaveDat));
        //                StaticTool.GetUnlineRunImg(ssd, sid, MyGlobal.globalConfig.zStart, 255 / MyGlobal.globalConfig.zRange, out zoomHeightImg, out zoomIntensityImg, out zoomRgbImg);

        //                image[1] = zoomHeightImg;
        //                if (!MyGlobal.isShowHeightImg)
        //                {
        //                    image[0] = zoomRgbImg;
        //                    zoomIntensityImg.Dispose();
        //                }
        //                else
        //                {
        //                    image[0] = zoomIntensityImg;
        //                    zoomRgbImg.Dispose();
        //                }
        //                MyGlobal.ImageMulti.Add(image);
        //                MyGlobal.hWindow_Final[i].HobjectToHimage(image[0]);
        //                GC.Collect();
        //            }
        //        }
        //        else
        //        {
        //            int orderi = 0; int orderh = 0; int orderB = 0;
        //            foreach (var item in openfile.FileNames)
        //            {
        //                for (int i = orderi; i < namesH.Length; i++)
        //                {
        //                    for (int j = 0; j < 4; j++)
        //                    {
        //                        if (item.Contains((j + 1).ToString() + "I.tiff"))
        //                        {
        //                            namesI[i] = item;
        //                            orderi = i + 1;
        //                            sidelist.Add(j + 1);
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                }
        //                for (int i = orderh; i < namesH.Length; i++)
        //                {
        //                    for (int j = 0; j < 4; j++)
        //                    {
        //                        if (item.Contains((j + 1).ToString() + "H.tiff"))
        //                        {
        //                            namesH[i] = item;
        //                            orderh = i + 1;
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                }

        //                if (item.Contains("B.tiff"))
        //                {
        //                    namesB[orderB] = item;
        //                    orderB++;
        //                }
        //            }


        //            if (namesH[0] == null)
        //            {
        //                return;
        //            }
        //            for (int i = 0; i < namesH.Length; i++)
        //            {
        //                HObject[] image = new HObject[2];

        //                if (MyGlobal.isShowHeightImg || namesB == null)
        //                {
        //                    HOperatorSet.ReadImage(out image[0], namesI[i]);
        //                }
        //                else
        //                {
        //                    HOperatorSet.ReadImage(out image[0], namesB[i]);
        //                }

        //                HOperatorSet.ReadImage(out image[1], namesH[i]);

        //                MyGlobal.ImageMulti.Add(image);

        //                MyGlobal.hWindow_Final[i].HobjectToHimage(image[0]);

        //            }
        //        }



        //    }
        //}

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //打开图片
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Multiselect = true;
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                ShowProfile.HalconWindow.ClearWindow();

                for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (MyGlobal.ImageMulti[i][j] != null)
                        {
                            MyGlobal.ImageMulti[i][j].Dispose();
                        }

                    }

                }
                MyGlobal.ImageMulti.Clear();
                sidelist.Clear();
                int len = openfile.FileNames.Length;
                double icount = len / 3.0;
                if (MyGlobal.globalConfig.enableAlign && !Regex.IsMatch(icount.ToString(), @"^([-]?)\d*$"))
                {
                    MessageBox.Show("文件数目缺失");
                    return;
                }
                if (openfile.FileName.Contains("Left"))
                {
                    MyGlobal.IsRight = false;
                }
                else
                {
                    MyGlobal.IsRight = true;
                }

                if (openfile.FileNames[len - 1].Contains("Side1_"))
                {
                    len = MyGlobal.globalConfig.enableAlign ? 3 : 2;
                }
                else if (openfile.FileNames[len - 1].Contains("Side2_"))
                {
                    len = MyGlobal.globalConfig.enableAlign ? 6 : 4;
                }
                else if (openfile.FileNames[len - 1].Contains("Side3_"))
                {
                    len = MyGlobal.globalConfig.enableAlign ? 9 : 6;
                }
                else if (openfile.FileNames[len - 1].Contains("Side4_"))
                {
                    len = MyGlobal.globalConfig.enableAlign ? 12 : 8;
                }


                string[] namesI;
                string[] namesH;
                string[] namesB;
                if (len < 9)
                {
                    if (MyGlobal.globalConfig.enableAlign)
                    {
                        namesI = new string[len / 3];
                        namesH = new string[len / 3];
                        namesB = new string[len / 3];
                    }
                    else
                    {
                        namesI = new string[len / 2];
                        namesH = new string[len / 2];
                        namesB = new string[len / 2];
                    }                    
                }
                else
                {
                    namesI = new string[(int)Math.Ceiling((double)len / 3)];
                    namesH = new string[(int)Math.Ceiling((double)len / 3)];
                    namesB = new string[(int)Math.Ceiling((double)len / 3)];
                }
                if (len < 2)
                {
                    return;
                }

                if (Path.GetExtension(openfile.FileNames[0]) == ".dat")
                {
                    int orderi = 0; int orderh = 0; int orderb = 0;
                    foreach (var item in openfile.FileNames)
                    {
                        if (item.Contains("L_H.dat"))
                        {
                            namesH[orderh] = item;
                            orderh++;
                        }
                        else if (item.Contains("I.dat"))
                        {
                            namesI[orderi] = item;
                            orderi++;
                        }
                        else if (item.Contains("S_H.dat") && MyGlobal.globalConfig.enableAlign)
                        {
                            namesB[orderb] = item;
                            orderb++;
                        }
                    }
                    if (namesH[0] == null)
                    {
                        return;
                    }

                    


                    for (int i = 0; i < namesH.Length; i++)
                    {
                        int CurrentSide = 0;
                        HObject[] image;
                        if (orderb > 0 && MyGlobal.globalConfig.enableAlign)
                        {
                            image = new HObject[3];
                        }
                        else { image = new HObject[2]; }

                        HObject zoomRgbImg, zoomHeightImg, zoomIntensityImg, planeImg, tileImg;
                        if (namesH[i] == null)
                        {
                            continue;
                        }
                        string Side = "";
                        if (namesH[i].Contains("Side1_"))
                        {
                            CurrentSide = 0;
                            Side = "Side1";
                        }
                        else if (namesH[i].Contains("Side2_"))
                        {
                            CurrentSide = 1;
                            Side = "Side2";
                        }
                        else if (namesH[i].Contains("Side3_"))
                        {
                            CurrentSide = 2;
                            Side = "Side3";
                        }
                        else if (namesH[i].Contains("Side4_"))
                        {
                            CurrentSide = 3;
                            Side = "Side4";
                        }
                       
                            SurfaceZSaveDat ssd = (SurfaceZSaveDat)StaticTool.ReadSerializable(namesH[i], typeof(SurfaceZSaveDat));
                        SurfaceIntensitySaveDat sid = (SurfaceIntensitySaveDat)StaticTool.ReadSerializable(namesI[i], typeof(SurfaceIntensitySaveDat));
                        SurfaceZSaveDat szd = null;
                        if (namesB[i] != null)
                        {
                            szd = (SurfaceZSaveDat)StaticTool.ReadSerializable(namesB[i], typeof(SurfaceZSaveDat));
                        }

                       

                        StaticTool.GetUnlineRunImg(ssd, sid, szd, MyGlobal.globalConfig.zStart, 255 / MyGlobal.globalConfig.zRange, out zoomHeightImg, out zoomIntensityImg, out zoomRgbImg, out planeImg);
                        //bool isUp = i == 3 || i == 1;
                        bool isUp = false;
                        if (MyGlobal.IsRight)
                        {
                            isUp = MyGlobal.globalPointSet_Right.IsUp[i];
                        }
                        else
                        {
                            isUp = MyGlobal.globalPointSet_Left.IsUp[i];
                        }
                        if (planeImg != null )
                        {
                            TileImg(planeImg, zoomHeightImg, out tileImg, isUp);
                        }
                        else
                        {
                            tileImg = null;
                        }



                        HObject rotate = new HObject();
                        HObject rotate_Height = new HObject();
                        if (MyGlobal.IsRight)
                        {
                            if (tileImg != null)
                            {
                                HOperatorSet.RotateImage(tileImg, out rotate, MyGlobal.globalPointSet_Right.imgRotateArr[i], "constant");
                            }
                            HOperatorSet.RotateImage(zoomHeightImg, out rotate_Height, MyGlobal.globalPointSet_Right.imgRotateArr[i], "constant");

                            HOperatorSet.RotateImage(zoomIntensityImg, out zoomIntensityImg, MyGlobal.globalPointSet_Right.imgRotateArr[i], "constant");
                            if (zoomRgbImg != null)
                            {
                                HOperatorSet.RotateImage(zoomRgbImg, out zoomRgbImg, MyGlobal.globalPointSet_Right.imgRotateArr[i], "constant");

                            }
                        }
                        else
                        {
                            if (tileImg != null)
                            {
                                HOperatorSet.RotateImage(tileImg, out rotate, MyGlobal.globalPointSet_Left.imgRotateArr[i], "constant");
                            }
                            HOperatorSet.RotateImage(zoomHeightImg, out rotate_Height, MyGlobal.globalPointSet_Left.imgRotateArr[i], "constant");

                            HOperatorSet.RotateImage(zoomIntensityImg, out zoomIntensityImg, MyGlobal.globalPointSet_Left.imgRotateArr[i], "constant");
                            if (zoomRgbImg != null)
                            {
                                HOperatorSet.RotateImage(zoomRgbImg, out zoomRgbImg, MyGlobal.globalPointSet_Left.imgRotateArr[i], "constant");

                            }
                        }

                        if (!MyGlobal.isShowHeightImg)
                        {
                            image[0] = zoomRgbImg;
                            zoomIntensityImg.Dispose();
                        }
                        else
                        {
                            image[0] = zoomIntensityImg;
                            zoomRgbImg.Dispose();
                        }

                        if (MyGlobal.globalConfig.enableAlign)
                        {
                            image[1] = rotate_Height;
                            image[2] = rotate;
                        }
                        else
                        {
                            image[1] = rotate_Height;
                        }

                        Action sw = () =>
                        {                                                        
                            if (image.Length== 3 && MyGlobal.isShowSurfaceImg)
                            {
                                MyGlobal.hWindow_Final[CurrentSide].HobjectToHimage(image[2]);
                            }
                            else
                            {
                                MyGlobal.hWindow_Final[CurrentSide].HobjectToHimage(image[0]);
                            }
                        };
                        this.Invoke(sw);
                        if (i < CurrentSide)
                        {
                            HObject[] image1 = new HObject[2];
                            image1[0] = new HObject();
                            image1[1] = new HObject();   
                            MyGlobal.ImageMulti.Add(image1);
                        }
                        MyGlobal.ImageMulti.Add(image);

                        if (planeImg != null)
                        {
                            planeImg.Dispose();
                        }
                        zoomHeightImg.Dispose();
                        GC.Collect();
                    }
                }
                else
                {
                    int orderi = 0; int orderh = 0; int orderB = 0;
                    foreach (var item in openfile.FileNames)
                    {
                        for (int i = orderi; i < namesH.Length; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (item.Contains((j + 1).ToString() + "I.tiff") || item.Contains((j + 1).ToString() + "B.tiff"))
                                {
                                    namesI[i] = item;
                                    orderi = i + 1;
                                    sidelist.Add(j + 1);
                                    break;
                                }
                            }
                            break;
                        }
                        for (int i = orderh; i < namesH.Length; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (item.Contains((j + 1).ToString() + "L_H.tiff"))
                                {
                                    namesH[i] = item;
                                    orderh = i + 1;
                                    break;
                                }
                            }
                            break;
                        }

                        if (item.Contains("S_H.tiff"))
                        {
                            namesB[orderB] = item;
                            orderB++;
                        }
                    }


                    if (namesH[0] == null)
                    {
                        return;
                    }
                    for (int i = 0; i < namesH.Length; i++)
                    {
                        HObject[] image;
                        if (orderB > 0 && MyGlobal.globalConfig.enableAlign)
                        {
                            image = new HObject[3];
                        }
                        else { image = new HObject[2]; }

                        HOperatorSet.ReadImage(out image[0], namesI[i]);

                        HOperatorSet.ReadImage(out image[1], namesH[i]);

                        if (orderB > 0)
                        {
                            HOperatorSet.ReadImage(out image[2], namesB[i]);
                        }



                        MyGlobal.ImageMulti.Add(image);

                        MyGlobal.hWindow_Final[i].HobjectToHimage(image[0]);

                    }
                }



            }
        }

        //void LoadImageData(string FilePath)
        //{
        //    try
        //    {
        //        ShowProfile.HalconWindow.ClearWindow();
        //        for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
        //        {
        //            for (int j = 0; j < 2; j++)
        //            {
        //                if (MyGlobal.ImageMulti[i][j] != null)
        //                {
        //                    MyGlobal.ImageMulti[i][j].Dispose();
        //                }

        //            }

        //        }
        //        MyGlobal.ImageMulti.Clear();
        //        sidelist.Clear();
        //        DirectoryInfo dinfo = new DirectoryInfo(FilePath);
        //        FileInfo[] finfo = dinfo.GetFiles();

        //        int len = finfo.Length;
        //        if (FilePath.Contains("Left"))
        //        {
        //            MyGlobal.IsRight = false;
        //        }
        //        else
        //        {
        //            MyGlobal.IsRight = true;
        //        }

        //        string[] namesI;
        //        string[] namesH;
        //        string[] namesB;
        //        if (len < 10)
        //        {
        //            namesI = new string[len / 2];
        //            namesH = new string[len / 2];
        //            namesB = null;
        //        }
        //        else
        //        {
        //            namesI = new string[len / 3];
        //            namesH = new string[len / 3];
        //            namesB = new string[len / 3];
        //        }
        //        if (len < 2)
        //        {
        //            return;
        //        }

        //        if (Path.GetExtension(finfo[0].FullName) == ".dat")
        //        {
        //            int orderi = 0; int orderh = 0;
        //            foreach (var item in finfo)
        //            {
        //                if (item.FullName.Contains("H.dat"))
        //                {
        //                    namesH[orderh] = item.FullName;
        //                    orderh++;
        //                }
        //                else if (item.FullName.Contains("I.dat"))
        //                {
        //                    namesI[orderi] = item.FullName;
        //                    orderi++;
        //                }
        //            }
        //            if (namesH[0] == null)
        //            {
        //                return;
        //            }

        //            for (int i = 0; i < namesH.Length; i++)
        //            {
        //                HObject[] image = new HObject[2];

        //                HObject zoomRgbImg, zoomHeightImg, zoomIntensityImg;

        //                SurfaceZSaveDat ssd = (SurfaceZSaveDat)StaticTool.ReadSerializable(namesH[i], typeof(SurfaceZSaveDat));

        //                SurfaceIntensitySaveDat sid = (SurfaceIntensitySaveDat)StaticTool.ReadSerializable(namesI[i], typeof(SurfaceIntensitySaveDat));
        //                StaticTool.GetUnlineRunImg(ssd, sid, MyGlobal.globalConfig.zStart, 255 / MyGlobal.globalConfig.zRange, out zoomHeightImg, out zoomIntensityImg, out zoomRgbImg);

        //                image[1] = zoomHeightImg;
        //                if (!MyGlobal.isShowHeightImg)
        //                {
        //                    image[0] = zoomRgbImg;
        //                    zoomIntensityImg.Dispose();
        //                }
        //                else
        //                {
        //                    image[0] = zoomIntensityImg;
        //                    zoomRgbImg.Dispose();
        //                }
        //                MyGlobal.ImageMulti.Add(image);
        //                MyGlobal.hWindow_Final[i].HobjectToHimage(image[0]);
        //                GC.Collect();
        //            }
        //        }
        //        else
        //        {
        //            int orderi = 0; int orderh = 0; int orderB = 0;
        //            foreach (var item in finfo)
        //            {
        //                for (int i = orderi; i < namesH.Length; i++)
        //                {
        //                    for (int j = 0; j < 4; j++)
        //                    {
        //                        if (item.FullName.Contains((j + 1).ToString() + "I.tiff"))
        //                        {
        //                            namesI[i] = item.FullName;
        //                            orderi = i + 1;
        //                            sidelist.Add(j + 1);
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                }
        //                for (int i = orderh; i < namesH.Length; i++)
        //                {
        //                    for (int j = 0; j < 4; j++)
        //                    {
        //                        if (item.FullName.Contains((j + 1).ToString() + "H.tiff"))
        //                        {
        //                            namesH[i] = item.FullName;
        //                            orderh = i + 1;
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                }

        //                if (item.FullName.Contains("B.tiff"))
        //                {
        //                    namesB[orderB] = item.FullName;
        //                    orderB++;
        //                }
        //            }


        //            if (namesH[0] == null)
        //            {
        //                return;
        //            }
        //            for (int i = 0; i < namesH.Length; i++)
        //            {
        //                HObject[] image = new HObject[2];

        //                if (MyGlobal.isShowHeightImg || namesB == null)
        //                {
        //                    HOperatorSet.ReadImage(out image[0], namesI[i]);
        //                }
        //                else
        //                {
        //                    HOperatorSet.ReadImage(out image[0], namesB[i]);
        //                }

        //                HOperatorSet.ReadImage(out image[1], namesH[i]);

        //                MyGlobal.ImageMulti.Add(image);

        //                MyGlobal.hWindow_Final[i].HobjectToHimage(image[0]);

        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        string LoadImageData(string FilePath,bool ReduceKdata)
        {
            try
            {
                ShowProfile.HalconWindow.ClearWindow();
                for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (MyGlobal.ImageMulti[i][j] != null)
                        {
                            MyGlobal.ImageMulti[i][j].Dispose();
                        }

                    }

                }
                MyGlobal.ImageMulti.Clear();
                sidelist.Clear();
                DirectoryInfo dinfo = new DirectoryInfo(FilePath);
                FileInfo[] finfo = dinfo.GetFiles();

                int len = finfo.Length;
                if (MyGlobal.globalConfig.enableAlign && len != 12)
                {
                    MessageBox.Show("文件数不足12");
                    return "文件数不足12";
                }
                if (FilePath.Contains("Left"))
                {
                    MyGlobal.IsRight = false;
                }
                else
                {
                    MyGlobal.IsRight = true;
                }

                string[] namesI;
                string[] namesH;
                string[] namesB;
                if (len < 10)
                {
                    namesI = new string[len / 2];
                    namesH = new string[len / 2];
                    namesB = null;
                }
                else
                {
                    namesI = new string[len / 3];
                    namesH = new string[len / 3];
                    namesB = new string[len / 3];
                }

                if (Path.GetExtension(finfo[0].FullName) == ".dat")
                {
                    int orderi = 0; int orderh = 0; int orderb = 0;
                    foreach (var item in finfo)
                    {
                        if (item.FullName.Contains("L_H.dat"))
                        {
                            namesH[orderh] = item.FullName;
                            orderh++;
                        }
                        else if (item.FullName.Contains("I.dat"))
                        {
                            namesI[orderi] = item.FullName;
                            orderi++;
                        }
                        else if (item.FullName.Contains("S_H.dat"))
                        {
                            namesB[orderb] = item.FullName;
                            orderb++;
                        }
                    }
                    if (namesH[0] == null)
                    {
                        return "高度数据加载失败！";
                    }
                  

                    for (int i = 0; i < namesH.Length; i++)
                    {

                        string Side = "";
                        if (namesH[i].Contains("Side1_"))
                        {                          
                            Side = "Side1";
                        }
                        else if (namesH[i].Contains("Side2_"))
                        {                          
                            Side = "Side2";
                        }
                        else if (namesH[i].Contains("Side3_"))
                        {                           
                            Side = "Side3";
                        }
                        else if (namesH[i].Contains("Side4_"))
                        {                           
                            Side = "Side4";
                        }

                        HObject[] image;
                        if (orderb > 0 && MyGlobal.globalConfig.enableAlign)
                        {
                            image = new HObject[3];
                        }
                        else { image = new HObject[2]; }

                        HObject zoomRgbImg, zoomHeightImg, zoomIntensityImg, planeImg, tileImg;

                        SurfaceZSaveDat ssd = (SurfaceZSaveDat)StaticTool.ReadSerializable(namesH[i], typeof(SurfaceZSaveDat));

                        SurfaceIntensitySaveDat sid = (SurfaceIntensitySaveDat)StaticTool.ReadSerializable(namesI[i], typeof(SurfaceIntensitySaveDat));
                        SurfaceZSaveDat szd = null;
                        if (namesB != null)
                        {
                            szd = (SurfaceZSaveDat)StaticTool.ReadSerializable(namesB[i], typeof(SurfaceZSaveDat));
                        }
                        if (!Directory.Exists("Data\\Kdatfile\\"))
                        {
                            Directory.CreateDirectory("Data\\Kdatfile\\");
                        }
                        string[] names = namesH[i].Split('\\');
                        string KdataName1 = names[names.Length - 2] +"_"+ names[names.Length - 1];
                        KdataName1 = KdataName1.Replace(".dat", "");
                        if (ssd != null && ReduceKdata)
                        {
                            StaticTool.ConvertDataToKdata(ssd, "Data\\Kdatfile\\", KdataName1);
                        }
                        string[] names2 = namesB[i].Split('\\');
                        string KdataName2 = names2[names2.Length - 2] + "_" + names2[names2.Length - 1];
                        KdataName2 = KdataName2.Replace(".dat", "");
                        if (szd != null && ReduceKdata)
                        {
                            StaticTool.ConvertDataToKdata(szd, "Data\\Kdatfile\\", KdataName2);
                        }

                        StaticTool.GetUnlineRunImg(ssd, sid, szd, MyGlobal.globalConfig.zStart, 255 / MyGlobal.globalConfig.zRange, out zoomHeightImg, out zoomIntensityImg, out zoomRgbImg, out planeImg);
                        //bool isUp = i == 3 || i == 1;
                        bool isUp = false;
                        if (MyGlobal.IsRight)
                        {
                            isUp = MyGlobal.globalPointSet_Right.IsUp[i];
                        }
                        else
                        {
                            isUp = MyGlobal.globalPointSet_Left.IsUp[i];
                        }
                        if (planeImg != null)
                        {
                            TileImg(planeImg, zoomHeightImg, out tileImg, isUp);
                        }
                        else
                        {
                            tileImg = null;
                        }



                        HObject rotate = new HObject();
                        HObject rotate_Height = new HObject();
                        if (MyGlobal.IsRight)
                        {
                            if (tileImg != null)
                            {
                                HOperatorSet.RotateImage(tileImg, out rotate, MyGlobal.globalPointSet_Right.imgRotateArr[i], "constant");
                            }
                            HOperatorSet.RotateImage(zoomHeightImg, out rotate_Height, MyGlobal.globalPointSet_Right.imgRotateArr[i], "constant");

                            HOperatorSet.RotateImage(zoomIntensityImg, out zoomIntensityImg, MyGlobal.globalPointSet_Right.imgRotateArr[i], "constant");
                            if (zoomRgbImg!=null)
                            {
                                HOperatorSet.RotateImage(zoomRgbImg, out zoomRgbImg, MyGlobal.globalPointSet_Right.imgRotateArr[i], "constant");

                            }
                        }
                        else
                        {
                            if (tileImg != null)
                            {
                                HOperatorSet.RotateImage(tileImg, out rotate, MyGlobal.globalPointSet_Left.imgRotateArr[i], "constant");
                            }
                            HOperatorSet.RotateImage(zoomHeightImg, out rotate_Height, MyGlobal.globalPointSet_Left.imgRotateArr[i], "constant");

                            HOperatorSet.RotateImage(zoomIntensityImg, out zoomIntensityImg, MyGlobal.globalPointSet_Left.imgRotateArr[i], "constant");
                            if (zoomRgbImg != null)
                            {
                                HOperatorSet.RotateImage(zoomRgbImg, out zoomRgbImg, MyGlobal.globalPointSet_Left.imgRotateArr[i], "constant");

                            }
                        }

                        if (!MyGlobal.isShowHeightImg)
                        {
                            image[0] = zoomRgbImg;
                            zoomIntensityImg.Dispose();
                        }
                        else
                        {
                            image[0] = zoomIntensityImg;
                            zoomRgbImg.Dispose();
                        }

                        if (MyGlobal.globalConfig.enableAlign)
                        {
                            image[1] = rotate_Height;
                            image[2] = rotate;
                        }
                        else
                        {
                            image[1] = rotate_Height;
                        }
                        Action sw = () =>
                        {
                            if (image.Length == 3 && MyGlobal.isShowSurfaceImg)
                            {
                                MyGlobal.hWindow_Final[i].HobjectToHimage(image[2]);
                            }
                            else
                            {
                                MyGlobal.hWindow_Final[i].HobjectToHimage(image[0]);
                            }
                        };
                        this.Invoke(sw);

                        MyGlobal.ImageMulti.Add(image);

                        if (planeImg != null)
                        {
                            planeImg.Dispose();
                        }
                        zoomHeightImg.Dispose();
                        GC.Collect();
                    }
                }
                else
                {
                    int orderi = 0; int orderh = 0; int orderB = 0;
                    foreach (var item in finfo)
                    {
                        for (int i = orderi; i < namesH.Length; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (item.FullName.Contains((j + 1).ToString() + "I.tiff"))
                                {
                                    namesI[i] = item.FullName;
                                    orderi = i + 1;
                                    sidelist.Add(j + 1);
                                    break;
                                }
                            }
                            break;
                        }
                        for (int i = orderh; i < namesH.Length; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (item.FullName.Contains((j + 1).ToString() + "H.tiff"))
                                {
                                    namesH[i] = item.FullName;
                                    orderh = i + 1;
                                    break;
                                }
                            }
                            break;
                        }

                        if (item.FullName.Contains("B.tiff"))
                        {
                            namesB[orderB] = item.FullName;
                            orderB++;
                        }
                    }


                    if (namesH[0] == null)
                    {
                        return "高度数据加载失败！";
                    }
                    for (int i = 0; i < namesH.Length; i++)
                    {
                        HObject[] image;
                        if (orderB > 0 && MyGlobal.globalConfig.enableAlign)
                        {
                            image = new HObject[3];
                        }
                        else { image = new HObject[2]; }

                        HOperatorSet.ReadImage(out image[0], namesI[i]);

                        HOperatorSet.ReadImage(out image[1], namesH[i]);

                        if (orderB > 0)
                        {
                            HOperatorSet.ReadImage(out image[2], namesB[i]);
                        }



                        MyGlobal.ImageMulti.Add(image);

                        Action sw = () =>
                        {
                            MyGlobal.hWindow_Final[i].HobjectToHimage(image[0]);
                        };
                        this.Invoke(sw);


                    }
                }
                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "LoadImageData Error" + ex.Message + RowNum;
            }
        }


        double total = 0;
        double ng = 0;
        double Per = 0;
        private void navBarItem7_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //if (total == 0)
            //{
            //    Per = 0;
            //}
            //else
            //{
            //    Per = Math.Round((total - ng) / total * 100, 3);
            //}
            //Frm_sql fs = new Frm_sql(total, ng, Per);
            //fs.ShowDialog();
        }

        //string namestart = "时间," + "工位," + "高度1," + "高度2," + "高度3," + "实时良率," + "总计," + "结果";
        //string namestartid = "@时间," + "@工位," + "@高度1," + "@高度2," + "@高度3," + "@实时良率," + "@总计," + "@结果";
        //void WriteToTable(int Sta, double H1, double H2, double H3, bool OK)
        //{
        //    string StationName = Sta == 0 ? "左工位 " : "右工位";

        //    string Okstring = OK ? "OK" : "NG";
        //    total++;
        //    if (!OK)
        //    {
        //        ng++;
        //    }
        //    Per = Math.Round((100 * (total - ng) / total), 2);

        //    SQLiteHelper.NewTable(namestart);
        //    string tableName = DateTime.Now.ToString("[yyyy/MM/dd]");
        //    string time = string.Format("{0}/{1}/{2} {3}", DateTime.Now.Year, DateTime.Now.Month,
        //        DateTime.Now.Day, DateTime.Now.ToShortTimeString());
        //    time = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        //    string sqlstr = string.Format("insert into {0} ({1}) values ({2})", tableName, namestart, namestartid);
        //    SQLiteParameter[] sqlPamram = {new SQLiteParameter("@时间",time),
        //        new SQLiteParameter("@工位",StationName),new SQLiteParameter("@高度1",H1),
        //        new SQLiteParameter("@高度2", H2),new SQLiteParameter("@高度3", H3),
        //        new SQLiteParameter("@实时良率", Per),
        //        new SQLiteParameter("@总计", total),
        //        new SQLiteParameter("@结果",Okstring)};
        //    int a = SQLiteHelper.ExecuteNonQuery(sqlstr, sqlPamram);
        //}
        VisionTool.FitLineSet flset = new VisionTool.FitLineSet();
        private void navBarItem12_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            flset.ToolType = "GlueGuide";
            flset.FindType = "FitLineSet";
            flset.ShowDialog();
        }

        public static bool runOffLineFrmTag = false;
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (runOffLineFrmTag == false)
            {
                runOffLineFrmTag = true;
                OfflineFrm OffFram = new OfflineFrm();
                OffFram.Run = new OfflineFrm.RunOff(RunOffline);
                OffFram.Show();
            }
            else
            {
                MessageBox.Show("离线Frm已打开！");
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
        DateTime datetime = DateTime.Now;
        bool firstLogin = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            MouseClickCnt = 0;
            MouseClickCnt1 = 0;
            MouseClickCnt2 = 0;
            MouseClickCnt3 = 0;
            barStaticItem3.Caption = $"当前用户：{UserLogin.CurrentUser.ToString()}";
           
            if (UserLogin.CurrentUser!=Verify.操作员 && !MyGlobal.globalConfig.enableFeature)
            {
                if (firstLogin)
                {
                    firstLogin = false;
                    datetime = DateTime.Now; //开始时间
                }
                TimeSpan ts = DateTime.Now - datetime;
                if (ts.Minutes >2)//注销用户
                {
                    UserLogin.CurrentUser = Verify.操作员;
                    DispBar(false);
                    firstLogin = true;
                }
            }
            
            

            switch (UserLogin.CurrentUser)
            {
                case Verify.操作员:
                    DispBar(true);
                    break;
                case Verify.技术员:
                    DispBar(true);
                    break;
                case Verify.工程师:
                    DispBar(true);
                    break;
                case Verify.管理员:
                    DispBar(true);
                    break;
                default:
                    break;
            }
        }



        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //ImgRotateFrm imgrotatefrm = new ImgRotateFrm();
            //imgrotatefrm.Show();
        }

        private void btn_clearbuffer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
                {
                    for (int j = 0; j < MyGlobal.ImageMulti[i].Length; j++)
                    {
                        MyGlobal.ImageMulti[i][j].Dispose();
                    }
                }
                MyGlobal.ImageMulti.Clear();
                for (int i = 0; i < MyGlobal.hWindow_Final.Length; i++)
                {
                    MyGlobal.hWindow_Final[i].ClearWindow();
                }
                ShowAndSaveMsg("清理缓存成功！");
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = start != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                ShowAndSaveMsg("清理缓存失败-->" + ex.Message + RowNum);
            }

        }
        VisionTool.GlobalParam gbParamSet = new VisionTool.GlobalParam();
        private void navBarItem5_LinkPressed_1(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            gbParamSet.ShowDialog();
        }

        private void barSubItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //NewProduct nProduct = new NewProduct();
            //nProduct.ShowDialog();
        }

        ShowCapacityFrm scf;
        private void btn_show_capacity_Click(object sender, EventArgs e)
        {
            if (btn_show_capacity.Text == "显示生产数据")
            {
                scf = new ShowCapacityFrm();
                btn_show_capacity.Text = "关闭生产数据";
                scf.SetDesktopLocation(Screen.PrimaryScreen.Bounds.Width - 100 - scf.Width, Screen.PrimaryScreen.Bounds.Height - 100 - scf.Height);
                scf.setValue(true);
                scf.Show();
            }
            else
            {
                btn_show_capacity.Text = "显示生产数据";
                scf.Close();
                scf.Dispose();
            }
        }

        private void barEditItem_CurrentType_EditValueChanged(object sender, EventArgs e)
        {
            if (isLoading)
            {
                return;
            }
            string Currenttype = barEditItem_CurrentType.EditValue.ToString();
            MyGlobal.PathName.CurrentType = Currenttype;
            StaticOperate.WriteXML(MyGlobal.PathName, MyGlobal.AllTypePath + "AllType.xml");
            Init();
            if (MyGlobal.PathName.CurrentType.Contains("_SurfaceCurvature"))
            {
                MyGlobal.globalConfig.enableAlign = true;
            }
            else
            {
                MyGlobal.globalConfig.enableAlign = false;

            }


        }

        public void ChangeBarEditValue(string Name)
        {
            //barEditItem_CurrentType.
            //RepositoryItemComboBox q =(RepositoryItemComboBox)barEditItem_CurrentType.Edit;
            //q.Items.Add(Name);
            //加载config

            DirectoryInfo dirinf = new DirectoryInfo(MyGlobal.AllTypePath);
            DirectoryInfo[] dirinfo = dirinf.GetDirectories();
            RepositoryItemComboBox q = (RepositoryItemComboBox)barEditItem_CurrentType.Edit;
            q.Items.Clear();
            for (int i = 0; i < dirinfo.Length; i++)
            {
                q.Items.Add(dirinfo[i].Name);
            }
            barEditItem_CurrentType.EditValue = MyGlobal.PathName.CurrentType;
            if (MyGlobal.PathName.CurrentType.Contains("_SurfaceCurvature"))
            {
                MyGlobal.globalConfig.enableAlign = true;
            }
            else
            {
                MyGlobal.globalConfig.enableAlign = false;

            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void setValue(bool IsRight)
        {
            if (IsRight)
            {
                chartControl3.Series[0].Points[0].Values = new double[] { MyGlobal.globalPointSet_Right.OkCnt };
                chartControl3.Series[0].Points[1].Values = new double[] { MyGlobal.globalPointSet_Right.AnchorErrorCnt };
                chartControl3.Series[0].Points[2].Values = new double[] { MyGlobal.globalPointSet_Right.FindEgdeErrorCnt };
                chartControl3.Series[0].Points[3].Values = new double[] { MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt };
                //Pie3DSeriesView pie3DSeriesView = (Pie3DSeriesView)chartControl3.Series[0].View;
                PieSeriesView pie3DSeriesView = (PieSeriesView)chartControl3.Series[0].View;
                int totalCnt = MyGlobal.globalPointSet_Right.OkCnt + MyGlobal.globalPointSet_Right.AnchorErrorCnt +
                    MyGlobal.globalPointSet_Right.FindEgdeErrorCnt + MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt;
                if (totalCnt == 0)
                {
                    chartControl3.Series[0].Points[0].Values = new double[] { 1 };
                }
                pie3DSeriesView.Titles[0].Text = $"右工位：{totalCnt}";
            }
            else
            {
                chartControl2.Series[0].Points[0].Values = new double[] { MyGlobal.globalPointSet_Left.OkCnt };
                chartControl2.Series[0].Points[1].Values = new double[] { MyGlobal.globalPointSet_Left.AnchorErrorCnt };
                chartControl2.Series[0].Points[2].Values = new double[] { MyGlobal.globalPointSet_Left.FindEgdeErrorCnt };
                chartControl2.Series[0].Points[3].Values = new double[] { MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt };
                //Pie3DSeriesView pie3DSeriesView = (Pie3DSeriesView)chartControl3.Series[0].View;
                PieSeriesView pie3DSeriesView = (PieSeriesView)chartControl2.Series[0].View;
                int totalCnt = MyGlobal.globalPointSet_Left.OkCnt + MyGlobal.globalPointSet_Left.AnchorErrorCnt +
                    MyGlobal.globalPointSet_Left.FindEgdeErrorCnt + MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt;
                if (totalCnt == 0)
                {
                    chartControl2.Series[0].Points[0].Values = new double[] { 1 };
                }
                pie3DSeriesView.Titles[0].Text = $"左工位：{totalCnt}";
            }
            chartControl1.Series[0].Points[0].Values = new double[] { MyGlobal.globalPointSet_Left.OkCnt + MyGlobal.globalPointSet_Right.OkCnt };
            chartControl1.Series[0].Points[1].Values = new double[] { MyGlobal.globalPointSet_Left.AnchorErrorCnt + MyGlobal.globalPointSet_Right.AnchorErrorCnt };
            chartControl1.Series[0].Points[2].Values = new double[] { MyGlobal.globalPointSet_Left.FindEgdeErrorCnt + MyGlobal.globalPointSet_Right.FindEgdeErrorCnt };
            chartControl1.Series[0].Points[3].Values = new double[] { MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt + MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt };
            //Pie3DSeriesView pie3DSeriesView1 = (Pie3DSeriesView)chartControl3.Series[0].View;
            PieSeriesView pie3DSeriesView1 = (PieSeriesView)chartControl1.Series[0].View;
                int totalCnt1 = MyGlobal.globalPointSet_Left.OkCnt + MyGlobal.globalPointSet_Left.AnchorErrorCnt +
                MyGlobal.globalPointSet_Left.FindEgdeErrorCnt + MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt +
                MyGlobal.globalPointSet_Right.OkCnt + MyGlobal.globalPointSet_Right.AnchorErrorCnt +
                MyGlobal.globalPointSet_Right.FindEgdeErrorCnt + MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt;
            if (totalCnt1 == 0)
            {
                chartControl1.Series[0].Points[0].Values = new double[] { 1 };
            }
            pie3DSeriesView1.Titles[0].Text = $"总产能：{totalCnt1}";

        }

        private void btn_clear_curr_capacity_Click(object sender, EventArgs e)
        {
            SimpleButton btn = (SimpleButton)sender;
            if (MessageBox.Show("确认清空生产数据?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                switch (btn.Name)
                {
                    case "btn_clear_curr_capacity1":
                        MyGlobal.globalPointSet_Right.OkCnt = 0;
                        MyGlobal.globalPointSet_Right.AnchorErrorCnt = 0;
                        MyGlobal.globalPointSet_Right.FindEgdeErrorCnt = 0;
                        MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt = 0;
                        MyGlobal.globalPointSet_Left.OkCnt = 0;
                        MyGlobal.globalPointSet_Left.AnchorErrorCnt = 0;
                        MyGlobal.globalPointSet_Left.FindEgdeErrorCnt = 0;
                        MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt = 0;
                        setValue(true);
                        setValue(false);
                        break;
                    case "btn_clear_curr_capacity2":
                        MyGlobal.globalPointSet_Left.OkCnt = 0;
                        MyGlobal.globalPointSet_Left.AnchorErrorCnt = 0;
                        MyGlobal.globalPointSet_Left.FindEgdeErrorCnt = 0;
                        MyGlobal.globalPointSet_Left.ExploreHeightErrorCnt = 0;
                        setValue(false);
                        break;
                    case "btn_clear_curr_capacity3":
                        MyGlobal.globalPointSet_Right.OkCnt = 0;
                        MyGlobal.globalPointSet_Right.AnchorErrorCnt = 0;
                        MyGlobal.globalPointSet_Right.FindEgdeErrorCnt = 0;
                        MyGlobal.globalPointSet_Right.ExploreHeightErrorCnt = 0;
                        setValue(true);
                        break;
                    default:
                        break;
                }
            }
            StaticOperate.WriteXML(MyGlobal.globalPointSet_Right, MyGlobal.AllTypePath + "GlobalPoint_Right.xml");
            StaticOperate.WriteXML(MyGlobal.globalPointSet_Left, MyGlobal.AllTypePath + "GlobalPoint_Left.xml");

        }

        private void 适应窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //HObject obj;
            //HOperatorSet.GenImageConst(out obj, "byte", ShowProfile.Width, ShowProfile.Height);
            //ClassShow3D.breakOut = true;
            //HOperatorSet.DispObj(obj, ShowProfile.HalconWindow);
            //obj.Dispose();
            if (MyGlobal.globalConfig.enableFeature)
            {
                ShowProfileToWindow(this.recordXCoord, this.recordYCoord, this.recordZCoord, this.recordSigleTitle, false, true);

            }
        }
    }
}
