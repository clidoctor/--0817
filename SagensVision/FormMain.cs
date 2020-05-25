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
using System.Data.SQLite;
using System.Diagnostics;
using SagensVision.VisionTool;

namespace SagensVision
{
    public partial class FormMain : DevExpress.XtraEditors.XtraForm
    {

        List<double[][]> XCoord = new List<double[][]>();
        List<double[][]> YCoord = new List<double[][]>();
        List<double[][]> ZCoord = new List<double[][]>();
        List<string[][]> StrLorC = new List<string[][]>();
       
        public FormMain()
        {
            HOperatorSet.SetSystem("width", 20512);
            HOperatorSet.SetSystem("height", 20512);
            InitializeComponent();
            Init();
            // Handling the QueryControl event that will populate all automatically generated Documents     
        }
        void LoadConfig()
        {
            try
            {
                if (File.Exists(MyGlobal.ConfigPath + "Global.xml"))
                {
                    MyGlobal.globalConfig = (GlobalConfig)StaticOperate.ReadXML(MyGlobal.ConfigPath + "Global.xml", MyGlobal.globalConfig.GetType());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        void Init()
        {
            if (!Directory.Exists(MyGlobal.ModelPath))
            {
                Directory.CreateDirectory(MyGlobal.ModelPath);
            }
            if (!Directory.Exists(MyGlobal.ConfigPath))
            {
                Directory.CreateDirectory(MyGlobal.ConfigPath);
            }
            if (!Directory.Exists(MyGlobal.DataPath))
            {
                Directory.CreateDirectory(MyGlobal.DataPath);
            }

            string dbcreate = SQLiteHelper.NewDbFile();
            if (dbcreate == "OK")
            {

                ShowAndSaveMsg("数据库创建成功!");
            }
            else
            {
                ShowAndSaveMsg("数据库创建失败!" + dbcreate);
            }
            LoadDataDB();

            //loadMathParam();
            for (int i = 0; i < 4; i++)
            {
                MyGlobal.hWindow_Final[i] = new HWindow_Final();
                MyGlobal.hWindow_Final[i].viewWindow.setFitWindow(true);
                //MyGlobal.parameterSet[i] = new MatchingModule.MatchingParam();

                //FindMax[i] = new VisionTool.FindMax(i + 1);
                //FindMax[i].inParam = FindMax[i].LoadXml(i + 1);
                //FindMax[i].roiList= FindMax[i].LoadROI(i + 1);
            }

            dockPanel3.Controls.Add(MyGlobal.hWindow_Final[0]);
            MyGlobal.hWindow_Final[0].Height = 640;
            MyGlobal.hWindow_Final[0].Width = 80;

            MyGlobal.hWindow_Final[0].Dock = DockStyle.Fill;
            dockPanel5.Controls.Add(MyGlobal.hWindow_Final[1]);
            MyGlobal.hWindow_Final[1].Height = 600;
            MyGlobal.hWindow_Final[1].Width = 100;
            MyGlobal.hWindow_Final[1].Dock = DockStyle.Fill;
            dockPanel4.Controls.Add(MyGlobal.hWindow_Final[2]);
            MyGlobal.hWindow_Final[2].Height = 640;
            MyGlobal.hWindow_Final[2].Width = 80;
            MyGlobal.hWindow_Final[2].Dock = DockStyle.Fill;
            dockPanel6.Controls.Add(MyGlobal.hWindow_Final[3]);
            MyGlobal.hWindow_Final[3].Height = 600;
            MyGlobal.hWindow_Final[3].Width = 100;
            MyGlobal.hWindow_Final[3].Dock = DockStyle.Fill;

            if (File.Exists(MyGlobal.ConfigPath + "Global.xml"))
            {
                MyGlobal.globalConfig = (GlobalConfig)StaticOperate.ReadXML(MyGlobal.ConfigPath + "Global.xml", MyGlobal.globalConfig.GetType());
            }


            for (int i = 0; i < 4; i++)
            {
                Calibration.ParamPath.ParaName = SideName[i];
                if (File.Exists(Calibration.ParamPath.Path_tup))
                {
                    HOperatorSet.ReadTuple(Calibration.ParamPath.Path_tup, out MyGlobal.HomMat3D[i]);
                }

            }

            //match.load = new Matching.Form1.LoadParam(loadMathParam);
            //match2.load = new Matching.Form1.LoadParam(loadMathParam);
            //OffFram.Run = new OfflineFrm.RunOff(RunOffline);
        }

        void LoadDataDB()
        {
            if (SQLiteHelper.OpenDb() == "OK")
            {
                DataTable dt = SQLiteHelper.GetSchema();
                string[] tableNames = SQLiteHelper.GetTableName().Trim().Split(',');
                int len = tableNames.Length;
                string tabelname = "";
                if (len > 1)
                {
                    tabelname = tableNames[len - 2];
                    if (tabelname != "")
                    {
                        SQLiteHelper.OpenDb();
                        DataSet Ds = new DataSet();
                        SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("select * from " + "[" + tabelname + "]", SQLiteHelper._SQLiteConn);
                        dataAdapter.Fill(Ds);
                        int rCount = Ds.Tables[0].Rows.Count;
                        if (rCount > 1)
                        {
                            DataTable dt1 = Ds.Tables[0];

                            total = (double)dt1.Rows[rCount - 1].ItemArray[6];
                            Per = (double)dt1.Rows[rCount - 1].ItemArray[5];
                            ng = total - total * Per / 100;
                        }

                        SQLiteHelper.CloseDb();
                    }

                }

            }
        }

        string RunFindPoint(int Side,HObject Intesity, HObject HeightImage, out double[][] X, out double[][] Y, out double[][] Z,out string[][] Str, HTuple Homat3D, HWindow_Final Hwnd)
        {
            X = null; Y = null; Z = null;Str = null;
            try
            {
                IntersetionCoord intersect = new IntersetionCoord();
                string ok1 = MyGlobal.flset2.FindIntersectPoint(Side, HeightImage, out intersect, Hwnd, false);
                HTuple homMaxFix = new HTuple();
                HOperatorSet.VectorAngleToRigid(MyGlobal.flset2.intersectCoordList[Side - 1].Row, MyGlobal.flset2.intersectCoordList[Side -1].Col,
                    0, intersect.Row, intersect.Col, 0,out homMaxFix);
                string OK = flset.FindPoint(Side, Intesity, HeightImage, out X, out Y, out Z,out Str, Homat3D, Hwnd,false, homMaxFix);
                return OK;
            }
            catch (Exception ex)
            {

                return "RunFindPoint Error :" + ex.Message;
            }
        }

        string RunDetect(int Index, HObject Image)
        {
            try
            {

                //string pre = FindMax[Index].PreHandle(Image, out ScaleImage);
                //if (pre!="OK")
                //{
                //    return pre;
                //}
                HObject ScaleImage = new HObject();
                if (!MyGlobal.mAssistant[Index].PreHandle(Image, MyGlobal.mAssistant[Index].PreHandleRoi, out ScaleImage))
                {
                    return "预处理失败！";
                }
                HOperatorSet.WriteImage(ScaleImage, "tiff", 0, MyGlobal.ModelPath + (Index).ToString() + "CurrentPreHandle.tiff");
                MyGlobal.mAssistant[Index].SetImageToAssistant(Image);
                bool Ok = MyGlobal.mAssistant[Index].detectShapeModel();
                if (!Ok)
                {
                    return "匹配失败！";
                }

                HXLD MatchXLD = MyGlobal.mAssistant[Index].getDetectionResults();
                MyGlobal.tResult[Index] = MyGlobal.mAssistant[Index].getMatchingResults();
                MyGlobal.hWindow_Final[Index].HobjectToHimage(ScaleImage);
                MyGlobal.hWindow_Final[Index].viewWindow.displayHobject(MatchXLD, "red");

                HTuple maxZ = new HTuple();

                //StaticOperate.SaveExcelData(Index,  result.Max[0].D.ToString(),result.Max[1].D.ToString(),result.Max[2].D.ToString());
                //if (result.DetectOK)
                //{
                //    ShowAndSaveMsg("检测OK！");
                //}
                //else
                //{
                //    ShowAndSaveMsg("检测NG！");
                //}

                //数据统计
                //WriteToTable(Index, result.Max[0].D, result.Max[1].D, result.Max[2].D, result.DetectOK);
                return "OK";
            }
            catch (Exception ex)
            {

                return "RunDetect Error" + ex.Message;
            }


        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //dockPanel5.Show();
            dockPanel4.Show();
            dockPanel3.Show();
            //dockPanel5.DockedAsTabbedDocument = true;
            dockPanel4.DockedAsTabbedDocument = true;
            dockPanel3.DockedAsTabbedDocument = true;


        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            dockPanel1.Show();
            dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
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
            dockPanel2.Show();
            dockPanel2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;

            dockPanel4.Show();
            dockPanel3.Show();
            //dockPanel5.Show();
            dockPanel4.DockedAsTabbedDocument = true;
            dockPanel3.DockedAsTabbedDocument = true;
            //dockPanel5.DockedAsTabbedDocument = true;
            bar2.Reset();
            bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;

        }

        private void navBarItem2_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            UserLoginIn.UserLogin user = new UserLoginIn.UserLogin();
            user.Show();
        }
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

        VisionTool.Display3D show3D = new VisionTool.Display3D();
        private void FormMain_Load(object sender, EventArgs e)
        {

            //this.dockPanel5.Controls.Add(show3D);
            //show3D.Dock = DockStyle.Fill;
            //dockPanel5.Show();


            //dockPanel4.Show();
            //dockPanel3.Show();
            //dockPanel5.DockedAsTabbedDocument = true;
            //dockPanel4.DockedAsTabbedDocument = true;
            //dockPanel3.DockedAsTabbedDocument = true;

            MyGlobal.thdWaitForClientAndMessage = new Thread(TcpClientListen);
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
            label_TotalNum.Text = MyGlobal.globalConfig.Count.ToString();
            string Msg = "";
            if (MyGlobal.GoSDK.connect(MyGlobal.globalConfig.SensorIP, ref Msg))
            {
                ShowAndSaveMsg("Sensor连接成功！");
                MyGlobal.globalConfig.dataContext = MyGlobal.GoSDK.context;
                if (!SecretKey.License.SnOk)
                {
                    ShowAndSaveMsg("Sn Fail！");
                }

            }
            else
            {
                ShowAndSaveMsg("Sensor连接失败！" + Msg);

            }
            //MyGlobal.GoSDK.SurfaceZRecFinish += GoSDK_SurfaceZRecFinish;
            cmu.Conn = ConnectTcp;
        }

        void ConnectTcp()
        {
            MyGlobal.thdWaitForClientAndMessage = new Thread(TcpClientListen);
            MyGlobal.thdWaitForClientAndMessage.IsBackground = true;

            MyGlobal.thdWaitForClientAndMessage.Name = "以太网通信线程";
            MyGlobal.thdWaitForClientAndMessage.Start();
        }
        private void GoSDK_SurfaceZRecFinish()
        {
            if (MyGlobal.globalConfig.SensorIP == "127.0.0.1")
            {
                if (SecretKey.License.SnOk)
                {
                    string ok = Run(1);
                    if (ok != "OK")
                    {
                        ShowAndSaveMsg(ok);
                    }
                }

            }

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
                    MyGlobal.globalConfig.Count++;
                    label_TotalNum.Text = MyGlobal.globalConfig.Count.ToString();

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

                    MyGlobal.hWindow_Final[Station -1].HobjectToHimage(IntensityImage);
                    if (Station == 1)
                        saveImageTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                    StaticOperate.SaveImage(IntensityImage, MyGlobal.globalConfig.Count.ToString(), SideName[Station - 1] + "I.tiff");
                    StaticOperate.SaveImage(HeightImage, MyGlobal.globalConfig.Count.ToString(), SideName[Station - 1] + "H.tiff");

                    string OK = RunSide(Station, IntensityImage, HeightImage);
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

        private string RunOutLine(int Station,int id)
        {

            if (Station == 1)
            {
                XCoord.Clear();
                YCoord.Clear();
                ZCoord.Clear();
                StrLorC.Clear();
            }
            if (MyGlobal.ImageMulti.Count==0)
            {
                return "加载高度图和亮度图 Ng";
            }
            string Ok = RunSide(Station, MyGlobal.ImageMulti[id][0], MyGlobal.ImageMulti[id][1]);
           
            if (Ok!="OK")
            {
                return Ok;
            }
           // double[][] x, y, z; string[][] LorC;
           //string OK = RunFindPoint(Station, MyGlobal.ImageMulti[id][0], MyGlobal.ImageMulti[id][1], out x, out y, out z,out LorC, HomMat3D[Station - 1], MyGlobal.hWindow_Final[0]);
           // if (OK!="OK")
           // {
           //     return OK;
           // }
           // XCoord.Add(x);
           // YCoord.Add(y);
           // ZCoord.Add(z);
           // StrLorC.Add(LorC);
           // int count = 0;
           // if (Station > 0 && XCoord.Count == Station)
           // {
           //     //写入到文本
           //     StringBuilder Str = new StringBuilder();
           //     for (int i = 0; i < Station; i++)
           //     {
           //         for (int j = 0; j < XCoord[i].GetLength(0); j++)
           //         {
           //             for (int k = 0; k < XCoord[i][j].Length; k++)
           //             {
           //                 double X1 = Math.Round(XCoord[i][j][k], 3);
           //                 double Y1 = Math.Round(YCoord[i][j][k], 3);
           //                 double Z1 = Math.Round(ZCoord[i][j][k], 3);
           //                 string lorc = StrLorC[i][j][k];
           //                 count++;
           //                 Str.Append(count.ToString() + ","+ X1.ToString("0.000") + "," + Y1.ToString("0.000") + "," + Z1.ToString("0.000") +","+ lorc + "\r\n");
           //             }
           //         }
           //     }
           //     StaticOperate.writeTxt("D:\\Laser3D.txt", Str.ToString());
           // }
           // else
           // {
           //     OK = "第" + (Station - 1).ToString()+ "边 点位获取失败";
           // }
            return Ok;
        }
   
        private string RunSide111(int Station,HObject IntensityImage,HObject HeightImage)
        {
            try
            {
                double[][] x, y, z; string[][] Strlorc;
                string OK = RunFindPoint(Station, IntensityImage, HeightImage, out x, out y, out z, out Strlorc, MyGlobal.HomMat3D[Station - 1], MyGlobal.hWindow_Final[0]);
                XCoord.Add(x);
                YCoord.Add(y);
                ZCoord.Add(z);
                StrLorC.Add(Strlorc);
                #region 除去起始位重复部分 并均分 
                for (int i = 0; i < Station; i++)
                {
                    HTuple  firstPt,order =0, lastPt,fpt, Grater,Less, GraterId =new HTuple(), LessId = new HTuple(); string first = "";
                    HTuple ResultX = new HTuple(), ResultY = new HTuple(), ResultZ = new HTuple(), ResultLorC = new HTuple();
                    HTuple ResultX2 = new HTuple(), ResultY2 = new HTuple(), ResultZ2 = new HTuple(), ResultLorC2 = new HTuple();
                    HTuple Lx1 = new HTuple(), Ly1 = new HTuple(), Lz1 = new HTuple(), Gx1 = new HTuple(), Gy1 = new HTuple(), Gz1 = new HTuple(); int Len1 = 0; int Len2 = 0;int Len = 0;
                    HTuple tempx, tempy, tempz;HTuple x1, y1, x2, y2;
                    switch (i)
                    {
                        case 0:
                            if (Station ==4) //Y1<Y4  // 去掉第一条重叠 保留第4条
                            {

                                ResultY = YCoord[i][0];//第1条第一段
                                ResultX = XCoord[i][0];//第1条第一段
                                ResultZ = ZCoord[i][0];//第1条第一段
                                ResultLorC = StrLorC[i][0];//第一段

                                if (ResultY.Length==0)
                                {
                                    break;
                                }
                                firstPt = ResultY[0];//第1条第一点
                                order = YCoord[3].GetLength(0) - 1;
                                ResultY2 = YCoord[3][order];//第四边最后段
                                ResultX2 = XCoord[3][order];//第四边最后段
                                //ResultLorC2 = StrLorC[3][order];


                                lastPt = ResultY2[ResultY2.Length - 1];//第4条最后一点
                                Less = ResultY.TupleLessEqualElem(lastPt);//第1条小于第4条重叠部分
                                Grater = ResultY2.TupleGreaterEqualElem(firstPt);//第4条大于于第1边不重叠部分

                                GraterId = Grater.TupleFind(1);
                                LessId = Less.TupleFind(1);
                                if (GraterId == -1)
                                {
                                    break;
                                }

                                //重叠部分取均值
                                Len2 =  GraterId.Length ;
                                Len1 = LessId.Length;
                                Len = Len1 < Len2 ? Len1 : Len2;
                                //Len2 添加到第4条
                                if (Len >= ResultY.Length || Len >= ResultY2.Length)
                                {
                                    break;
                                }
                                y1 = ResultY2.TupleSelectRange(ResultY2.Length - Len, ResultY2.Length - 1);
                                x1 = ResultX2.TupleSelectRange(ResultX2.Length - Len, ResultX2.Length - 1);
                                y2 = ResultY.TupleSelectRange(0, Len - 1);
                                x2 = ResultX.TupleSelectRange(0, Len - 1);
                                Lx1 = (x1 + x2) / 2;
                                Ly1 = (y1 + y2) / 2;
                                HOperatorSet.TupleGenSequence(0, Len - 1, 1, out Less);
                                HOperatorSet.TupleGenSequence(ResultY2.Length - 1, ResultY2.Length - Len, -1, out Grater);


                                ResultY2 = ResultY2.TupleReplace(Grater, Ly1);
                                ResultX2 = ResultX2.TupleReplace(Grater, Lx1);

                                ResultY = ResultY.TupleRemove(Less);
                                ResultX = ResultX.TupleRemove(Less);
                                ResultZ = ResultZ.TupleRemove(Less);
                                //首位 
                                first = ResultLorC[0];
                                ResultLorC = ResultLorC.TupleRemove(Less);
                                ResultLorC[0] = first;

                                XCoord[3][order] = ResultX2;
                                YCoord[3][order] = ResultY2;
                            }
                            
                            break;
                        case 1:
                            if (Station>= 2) //X2>X1  去掉第二条重叠 保留第1条
                            {

                                ResultX = XCoord[i][0];//第2条第第一段
                                ResultY = YCoord[i][0];//第2条第第一段
                                ResultZ = ZCoord[i][0];//第2条第第一段
                                ResultLorC = StrLorC[i][0];//第一段

                                if (ResultX.Length==0)
                                {
                                    break;
                                }
                                firstPt = ResultX[0];//第2条第一点
                                order = XCoord[0].GetLength(0) - 1;
                                ResultX2 = XCoord[0][order];//第1边最后段
                                ResultY2 = YCoord[0][order];//第1边最后段
   
                                lastPt = ResultX2[ResultX2.Length - 1];//第1条最后一点
                                Grater = ResultX.TupleGreaterEqualElem(lastPt);//第2条大于第1条重叠部分
                                Less = ResultX2.TupleLessEqualElem(firstPt);//第1条小于第2边重叠部分

                                GraterId = Grater.TupleFind(1);
                                LessId = Less.TupleFind(1);

                                if (GraterId == -1 || LessId == -1)
                                {
                                    break;
                                }

                                //重叠部分取均值    
                                //Gx1 = ResultX[LessId];
                                //Gy1 = ResultY[LessId];
                                //Gz1 = ResultZ[LessId];

                                Len2 = LessId.Length;Len1 = GraterId.Length;
                                Len = Len1<Len2 ?Len1:Len2;
                                //Len2 添加到第4条
                                if (Len >= ResultY.Length || Len>= ResultY2.Length)
                                {
                                    break;
                                }
                                y1 = ResultY2.TupleSelectRange(ResultY2.Length - Len, ResultY2.Length - 1);
                                x1 = ResultX2.TupleSelectRange(ResultX2.Length - Len, ResultX2.Length - 1);
                                y2 = ResultY.TupleSelectRange(0, Len -1);
                                x2 = ResultX.TupleSelectRange(0, Len -1);
                                Lx1 = (x1 + x2) / 2;
                                Ly1 = (y1 + y2) / 2;

                                HOperatorSet.TupleGenSequence(0, Len -1, 1, out Grater);
                                HOperatorSet.TupleGenSequence(ResultY2.Length - 1, ResultY2.Length - Len, -1, out Less);


                                ResultY2 = ResultY2.TupleReplace(Less, Ly1);
                                ResultX2 = ResultX2.TupleReplace(Less, Lx1);

                                ResultY = ResultY.TupleRemove(Grater);
                                ResultX = ResultX.TupleRemove(Grater);
                                ResultZ = ResultZ.TupleRemove(Grater);

                                //首位 
                                first = ResultLorC[0];
                                ResultLorC = ResultLorC.TupleRemove(Grater);
                                ResultLorC[0] = first;

                                XCoord[0][order] = ResultX2;
                                YCoord[0][order] = ResultY2;
                            }
                            break;
                        case 2:
                            if (Station >= 3) //Y3>Y2 去掉第三条重叠 保留第2条
                            {

                                ResultY = YCoord[i][0];//第3条第一段
                                ResultX = XCoord[i][0];//第3条第一段
                                ResultZ = ZCoord[i][0];//第3条第一段
                                ResultLorC = StrLorC[i][0];//第一段

                                if (ResultY.Length == 0)
                                {
                                    break;
                                }

                                firstPt = ResultY[0];//第3条第一点
                                order = YCoord[1].GetLength(0) - 1;
                                ResultY2 = YCoord[1][order];//第2边最后段
                                ResultX2 = XCoord[1][order];//第2边最后段
                                //ResultLorC2 = StrLorC[3][order];


                                lastPt = ResultY2[ResultY2.Length - 1];//第2条最后一点
                                Less = ResultY.TupleGreaterEqualElem(lastPt);//第3条大于第2条重叠部分
                                Grater = ResultY2.TupleLessEqualElem(firstPt);//第2条小于第3边重叠部分

                                GraterId = Grater.TupleFind(1);
                                LessId = Less.TupleFind(1);
                                if (GraterId == -1 || LessId == -1)
                                {
                                    break;
                                }

                                //重叠部分取均值
                                Len2 = LessId.Length; Len1 = GraterId.Length;
                                Len = Len1 < Len2 ? Len1 : Len2;
                                //Len2 添加到第4条
                                if (Len >= ResultY.Length || Len >= ResultY2.Length)
                                {
                                    break;
                                }
                                y1 = ResultY2.TupleSelectRange(ResultY2.Length - Len, ResultY2.Length - 1);
                                x1 = ResultX2.TupleSelectRange(ResultX2.Length - Len, ResultX2.Length - 1);
                                y2 = ResultY.TupleSelectRange(0, Len - 1);
                                x2 = ResultX.TupleSelectRange(0, Len - 1);
                                Lx1 = (x1 + x2) / 2;
                                Ly1 = (y1 + y2) / 2;

                                HOperatorSet.TupleGenSequence(0, Len - 1, 1, out Less);
                                HOperatorSet.TupleGenSequence(ResultY2.Length - 1, ResultY2.Length - Len, -1, out Grater);


                                ResultY2 = ResultY2.TupleReplace(Grater, Ly1);
                                ResultX2 = ResultX2.TupleReplace(Grater, Lx1);

                                ResultY = ResultY.TupleRemove(Less);
                                ResultX = ResultX.TupleRemove(Less);
                                ResultZ = ResultZ.TupleRemove(Less);
                                //首位 
                                first = ResultLorC[0];
                                ResultLorC = ResultLorC.TupleRemove(Less);
                                ResultLorC[0] = first;

                                XCoord[1][order] = ResultX2;
                                YCoord[1][order] = ResultY2;
                            }
                            break;
                        case 3:
                            if (Station >= 4) //X4<X3  //Y3>Y2 去掉第四条重叠 保留第3条
                            {

                                ResultX = XCoord[i][0];//第4条第一段
                                ResultY = YCoord[i][0];//第4条第一段
                                ResultZ = ZCoord[i][0];//第4条第一段
                                ResultLorC = StrLorC[i][0]; //第4条//第一段

                                if (ResultX.Length==0)
                                {
                                    break;
                                }
                                firstPt = ResultX[0];//第4条第一点
                                order = XCoord[2].GetLength(0) - 1;
                                ResultX2 = XCoord[2][order];//第3边最后段
                                ResultY2 = YCoord[2][order];//第3边最后段

                                lastPt = ResultX2[ResultX2.Length - 1];//第3条最后一点
                                Grater = ResultX.TupleLessEqualElem(lastPt);//第4条小于第1条重叠部分
                                Less = ResultX2.TupleGreaterEqualElem(firstPt);//第3条大于第2边重叠部分

                                GraterId = Grater.TupleFind(1);
                                LessId = Less.TupleFind(1);

                                if (GraterId == -1 || LessId == -1)
                                {
                                    break;
                                }

                                //重叠部分取均值    
                                Len2 = LessId.Length; Len1 = GraterId.Length;
                                Len = Len1 < Len2 ? Len1 : Len2;
                                //Len2 添加到第4条
                                if (Len >= ResultY.Length || Len >= ResultY2.Length)
                                {
                                    break;
                                }
                                y1 = ResultY2.TupleSelectRange(ResultY2.Length - Len, ResultY2.Length - 1);
                                x1 = ResultX2.TupleSelectRange(ResultX2.Length - Len, ResultX2.Length - 1);
                                y2 = ResultY.TupleSelectRange(0, Len - 1);
                                x2 = ResultX.TupleSelectRange(0, Len - 1);
                                Lx1 = (x1 + x2) / 2;
                                Ly1 = (y1 + y2) / 2;

                                HOperatorSet.TupleGenSequence(0, Len - 1, 1, out Grater);
                                HOperatorSet.TupleGenSequence(ResultY2.Length - 1, ResultY2.Length - Len, -1, out Less);


                                ResultY2 = ResultY2.TupleReplace(Less, Ly1);
                                ResultX2 = ResultX2.TupleReplace(Less, Lx1);

                                ResultY = ResultY.TupleRemove(Grater);
                                ResultX = ResultX.TupleRemove(Grater);
                                ResultZ = ResultZ.TupleRemove(Grater);

                                //首位 
                                first = ResultLorC[0];
                                ResultLorC = ResultLorC.TupleRemove(Grater);
                                ResultLorC[0] = first;

                                XCoord[2][order] = ResultX2;
                                YCoord[2][order] = ResultY2;
                            }
                            break;
                    }
                    if (GraterId.Length != 0 && GraterId.D == -1)
                    {
                        //XCoord[i][0] = null;
                        //YCoord[i][0] = null;
                        //ZCoord[i][0] = null;
                        //StrLorC[i][0] = null;

                        //return string.Format("第{0}边,第一段重合点数过多", i + 1);
                    }
                    else if(GraterId.Length != 0)
                    {

                        //XCoord[i][0] = ResultX;
                        //YCoord[i][0] = ResultY;
                        //ZCoord[i][0] = ResultZ;
                        //StrLorC[i][0] = ResultLorC;                        
                    }

                }
                #endregion

                Dictionary<int, string> everySeg = new Dictionary<int, string>();
                double[] xcoord, ycoord, zcoord;string[] keypt ;
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
                xcoord = new double[totalNum];ycoord = new double[totalNum];zcoord = new double[totalNum];
                keypt = new string[totalNum];
                int ind = 0;
                double x0 = 0, y0 = 0, z0 = 0;
                for (int i = 0; i < XCoord.Count; i++)
                {
                    for (int j = 0; j < XCoord[i].GetLength(0); j++)
                    {
                        if (XCoord[i][j] == null)
                        {
                            continue;
                        }

                        HTuple row = XCoord[i][j];
                        HTuple col = YCoord[i][j];

                        if (Station == 4)
                        {
                            HObject NewSide = new HObject();
                            HOperatorSet.GenContourPolygonXld(out NewSide, row, col);
                            MyGlobal.hWindow_Final[0].viewWindow.displayHobject(NewSide, "red");
                        }

                        for (int k = 0; k < XCoord[i][j].Length; k++)
                        {
                            if (k>0)
                            {
                                if (xcoord[ind] == xcoord[ind -1] && ycoord[ind] == ycoord[ind - 1])
                                {
                                    //存在重复点；
                                    MessageBox.Show("重复点");
                                }
                            }
                            xcoord[ind] = Math.Round(XCoord[i][j][k],3);
                            ycoord[ind] = Math.Round(YCoord[i][j][k],3);
                            zcoord[ind] = Math.Round(ZCoord[i][j][k],3);
                            keypt[ind] = StrLorC[i][j][k];
                            if (k == 0)
                            {
                                everySeg.Add(ind, keypt[ind]);
                            }
                            ind++;
                            if (ind == 2044)
                            {
                                Debug.WriteLine("tt+" + ind);
                            }
                            Debug.WriteLine("tt+" + ind);
                        }
                    }
                }

                //排列起点
                //写入到文本
                StringBuilder Str = new StringBuilder();
                int Start = MyGlobal.globalConfig.Startpt;
                for (int i = 0; i < xcoord.Length; i++)
                {
                    
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
                    if (i == 0)
                    {
                        int[] keys = everySeg.Keys.ToArray();
                        for (int m = 0; m < keys.Length; m++)
                        {
                            if (Start >= keys[m])
                            {
                                lorc = everySeg[keys[m]];
                                break;
                            }
                        }
                    }
                    if (i == 0)
                    {
                        x0 = X1;
                        y0 = Y1;
                        z0 = Z1;
                    }
                    Str.Append((i+1).ToString() + "," + X1.ToString("0.000") + "," + Y1.ToString("0.000") + "," + Z1.ToString("0.000") + "," + lorc + "\r\n");

                }
                string strlast = "0;";
                int len1 = XCoord[Station - 1].GetLength(0);

                //int count = 0;
                //if (Station > 0)
                //{
                //    double x0 =0, y0 =0, z0 =0;                   

                //    //写入到文本
                //    StringBuilder Str = new StringBuilder();
                //    for (int i = 0; i < Station; i++)
                //    {
                //        for (int j = 0; j < XCoord[i].GetLength(0); j++)
                //        {
                //            if (XCoord[i][j] == null)
                //            {
                //                continue;
                //            }
                //            HTuple row = XCoord[i][j];
                //            HTuple col = YCoord[i][j];

                //            if (Station ==4)
                //            {
                //                HObject NewSide = new HObject();
                //                HOperatorSet.GenContourPolygonXld(out NewSide,row, col);
                //                MyGlobal.hWindow_Final[0].viewWindow.displayHobject(NewSide, "red");
                //            }
                //            for (int k = 0; k < XCoord[i][j].Length; k++)
                //            {
                               
                //                double X1 = Math.Round(XCoord[i][j][k], 3);
                //                double Y1 = Math.Round(YCoord[i][j][k], 3);
                //                double Z1 = Math.Round(ZCoord[i][j][k], 3);
                //                if (i == j  && j==k && k == 0)
                //                {
                //                     x0 = X1;
                //                     y0 = Y1;
                //                     z0 = Z1;
                //                }
                //                string lorc = StrLorC[i][j][k];
                //                count++;
                //                Str.Append(count.ToString() + "," + X1.ToString("0.000") + "," + Y1.ToString("0.000") + "," + Z1.ToString("0.000") + "," + lorc + "\r\n");
                                
                //            }
                //        }
                //    }
                    if (XCoord[Station -1][len1 -1] !=null)
                    {
                        strlast = StrLorC[Station - 1][len1 - 1][0];
                    }
                    else
                    {
                        strlast = StrLorC[Station - 1][len1 - 2][0];

                    }
                    Str.Append((totalNum + 1).ToString() + "," + x0.ToString("0.000") + "," + y0.ToString("0.000") + "," + z0.ToString("0.000") + "," + strlast + "\r\n");

                    StaticOperate.writeTxt("D:\\Laser3D_1.txt", Str.ToString());
                //}
                return "OK";
            }
            catch (Exception ex)
            {

                return "RunSide error :"+ ex.Message;
            }
        }

        private string RunSide(int Station, HObject IntensityImage, HObject HeightImage)
        {
            try
            {
                double[][] x, y, z; string[][] Strlorc;
                string OK = RunFindPoint(Station, IntensityImage, HeightImage, out x, out y, out z, out Strlorc, MyGlobal.HomMat3D[Station - 1], MyGlobal.hWindow_Final[Station - 1]);
                if (OK != "OK")
                {
                    return "第" + Station + "边"+ OK;
                }
                if (x[0] == null)
                {
                    return "第" + Station + "边未找到边";
                }
               
                XCoord.Add(x);
                YCoord.Add(y);
                ZCoord.Add(z);
                StrLorC.Add(Strlorc);
                #region 除去起始位重复部分 并均分 
                for (int i = 0; i < Station; i++)
                {
                    HTuple firstPt, order, last, lastPt, Grater, GraterId = new HTuple(); string first = "";
                    HTuple ResultX = new HTuple(), ResultY = new HTuple(), ResultZ = new HTuple(), ResultLorC = new HTuple();
                    switch (i)
                    {
                        case 0:
                            if (Station == 4) //Y1<Y4
                            {

                                ResultY = YCoord[i][0];//第一段
                                ResultX = XCoord[i][0];//第一段
                                ResultZ = ZCoord[i][0];//第一段
                                ResultLorC = StrLorC[i][0];//第一段

                                if (ResultY.Length == 0)
                                {
                                    break;
                                }
                                firstPt = ResultY[0];//第一点
                                order = YCoord[3].GetLength(0) - 1;
                                last = YCoord[3][order];//第四边最后段
                                lastPt = last[last.Length - 1];//最后一点
                                Grater = ResultY.TupleGreaterEqualElem(lastPt);//大于第四条不重叠部分
                                GraterId = Grater.TupleFind(1);
                                if (GraterId == -1)
                                {
                                    break;
                                }
                                ResultY = ResultY[GraterId];
                                ResultX = ResultX[GraterId];
                                ResultZ = ResultZ[GraterId];
                                //首位 
                                first = ResultLorC[0];
                                ResultLorC = ResultLorC[GraterId];
                                ResultLorC[0] = first;

                            }

                            break;
                        case 1:
                            if (Station >= 2) //X2>X1
                            {

                                ResultX = XCoord[i][0];//第一段
                                ResultY = YCoord[i][0];//第一段
                                ResultZ = ZCoord[i][0];//第一段
                                ResultLorC = StrLorC[i][0];//第一段

                                if (ResultX.Length == 0)
                                {
                                    break;
                                }
                                firstPt = ResultX[0];//第一点
                                order = XCoord[0].GetLength(0) - 1;
                                last = XCoord[0][order];//第1边最后段
                                lastPt = last[last.Length - 1];//最后一点
                                Grater = ResultX.TupleLessEqualElem(lastPt);//小于第一条不重叠部分
                                GraterId = Grater.TupleFind(1);
                                if (GraterId == -1)
                                {
                                    break;
                                }
                                ResultY = ResultY[GraterId];
                                ResultX = ResultX[GraterId];
                                ResultZ = ResultZ[GraterId];

                                //首位 
                                first = ResultLorC[0];
                                ResultLorC = ResultLorC[GraterId];
                                ResultLorC[0] = first;
                            }
                            break;
                        case 2:
                            if (Station >= 3) //Y3>Y2
                            {

                                ResultY = YCoord[i][0];//第一段
                                ResultX = XCoord[i][0];//第一段
                                ResultZ = ZCoord[i][0];//第一段
                                ResultLorC = StrLorC[i][0];//第一段

                                if (ResultY.Length == 0)
                                {
                                    break;
                                }

                                firstPt = ResultY[0];//第一点
                                order = YCoord[1].GetLength(0) - 1;
                                last = YCoord[1][order];//第2边最后段
                                lastPt = last[last.Length - 1];//最后一点
                                Grater = ResultY.TupleLessEqualElem(lastPt);//不重叠部分
                                GraterId = Grater.TupleFind(1);
                                if (GraterId == -1)
                                {
                                    break;
                                }
                                ResultY = ResultY[GraterId];
                                ResultX = ResultX[GraterId];
                                ResultZ = ResultZ[GraterId];

                                //首位 
                                first = ResultLorC[0];
                                ResultLorC = ResultLorC[GraterId];
                                ResultLorC[0] = first;
                            }
                            break;
                        case 3:
                            if (Station >= 4) //X4<X3
                            {

                                ResultX = XCoord[i][0];//第一段
                                ResultY = YCoord[i][0];//第一段
                                ResultZ = ZCoord[i][0];//第一段
                                ResultLorC = StrLorC[i][0];//第一段

                                if (ResultX.Length == 0)
                                {
                                    break;
                                }
                                firstPt = ResultX[0];//第一点
                                order = XCoord[2].GetLength(0) - 1;
                                last = XCoord[2][order];//第3边最后段
                                lastPt = last[last.Length - 1];//最后一点
                                Grater = ResultX.TupleGreaterEqualElem(lastPt);//不重叠部分
                                GraterId = Grater.TupleFind(1);
                                if (GraterId == -1)
                                {
                                    break;
                                }
                                ResultY = ResultY[GraterId];
                                ResultX = ResultX[GraterId];
                                ResultZ = ResultZ[GraterId];

                                //首位 
                                first = ResultLorC[0];
                                ResultLorC = ResultLorC[GraterId];
                                ResultLorC[0] = first;
                            }
                            break;
                    }
                    if (GraterId.Length != 0 && GraterId.D == -1)
                    {
                        XCoord[i][0] = null;
                        YCoord[i][0] = null;
                        ZCoord[i][0] = null;
                        StrLorC[i][0] = null;

                        return string.Format("第{0}边,第一段重合点数过多", i + 1);
                    }
                    else if (GraterId.Length != 0)
                    {
                        XCoord[i][0] = ResultX;
                        YCoord[i][0] = ResultY;
                        ZCoord[i][0] = ResultZ;
                        StrLorC[i][0] = ResultLorC;
                    }

                }
                #endregion

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
                xcoord = new double[totalNum]; ycoord = new double[totalNum]; zcoord = new double[totalNum];
                keypt = new string[totalNum];
                int ind = 0;
                double x0 = 0, y0 = 0, z0 = 0;
                for (int i = 0; i < XCoord.Count; i++)
                {
                    for (int j = 0; j < XCoord[i].GetLength(0); j++)
                    {
                        if (XCoord[i][j] == null)
                        {
                            continue;
                        }

                        HTuple row = XCoord[i][j];
                        HTuple col = YCoord[i][j];

                        if (Station == 4)
                        {
                            HObject NewSide = new HObject();
                            HOperatorSet.GenContourPolygonXld(out NewSide, row, col);
                            MyGlobal.hWindow_Final[0].viewWindow.displayHobject(NewSide, "red");
                        }

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
                int Start = MyGlobal.globalConfig.Startpt;
                for (int i = 0; i < xcoord.Length; i++)
                {

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
                    if (i == 0)
                    {
                        int[] keys = everySeg.Keys.ToArray();
                        for (int m = 0; m < keys.Length; m++)
                        {
                            if (Start >= keys[m])
                            {
                                lorc = everySeg[keys[m]];
                                break;
                            }
                        }
                    }
                    if (i == 0)
                    {
                        x0 = X1;
                        y0 = Y1;
                        z0 = Z1;
                    }
                    Str.Append((i + 1).ToString() + "," + X1.ToString("0.000") + "," + Y1.ToString("0.000") + "," + Z1.ToString("0.000") + "," + lorc + "\r\n");

                }
                string strlast = "0;";
                int len1 = XCoord[Station - 1].GetLength(0);

                //int count = 0;
                //if (Station > 0)
                //{
                //    double x0 =0, y0 =0, z0 =0;                   

                //    //写入到文本
                //    StringBuilder Str = new StringBuilder();
                //    for (int i = 0; i < Station; i++)
                //    {
                //        for (int j = 0; j < XCoord[i].GetLength(0); j++)
                //        {
                //            if (XCoord[i][j] == null)
                //            {
                //                continue;
                //            }
                //            HTuple row = XCoord[i][j];
                //            HTuple col = YCoord[i][j];

                //            if (Station ==4)
                //            {
                //                HObject NewSide = new HObject();
                //                HOperatorSet.GenContourPolygonXld(out NewSide,row, col);
                //                MyGlobal.hWindow_Final[0].viewWindow.displayHobject(NewSide, "red");
                //            }
                //            for (int k = 0; k < XCoord[i][j].Length; k++)
                //            {

                //                double X1 = Math.Round(XCoord[i][j][k], 3);
                //                double Y1 = Math.Round(YCoord[i][j][k], 3);
                //                double Z1 = Math.Round(ZCoord[i][j][k], 3);
                //                if (i == j  && j==k && k == 0)
                //                {
                //                     x0 = X1;
                //                     y0 = Y1;
                //                     z0 = Z1;
                //                }
                //                string lorc = StrLorC[i][j][k];
                //                count++;
                //                Str.Append(count.ToString() + "," + X1.ToString("0.000") + "," + Y1.ToString("0.000") + "," + Z1.ToString("0.000") + "," + lorc + "\r\n");

                //            }
                //        }
                //    }
                if (XCoord[Station - 1][len1 - 1] != null)
                {
                    strlast = StrLorC[Station - 1][len1 - 1][0];
                }
                else
                {
                    strlast = StrLorC[Station - 1][len1 - 2][0];

                }
                Str.Append((totalNum + 1).ToString() + "," + x0.ToString("0.000") + "," + y0.ToString("0.000") + "," + z0.ToString("0.000") + "," + strlast + "\r\n");

                StaticOperate.writeTxt("D:\\Laser3D_1.txt", Str.ToString());
                //}
                return "OK";
            }
            catch (Exception ex)
            {

                return "RunSide error :" + ex.Message;
            }
        }


        bool TcpIsConnect = false;

        int Side = 0;
        public  void TcpClientListen()
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
                        ShowAndSaveMsg(string.Format("连接服务器失败！" +ex.Message));                   
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
                        if (MyGlobal.ReceiveMsg.Contains("1")|| MyGlobal.ReceiveMsg.Contains("2")||MyGlobal.ReceiveMsg.Contains("3")|| MyGlobal.ReceiveMsg.Contains("4"))
                        {
                            Side = Convert.ToInt32(MyGlobal.ReceiveMsg.Substring(0, 1));
                            ReturnStr = MyGlobal.ReceiveMsg.Remove(0,1);
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
                                if (MyGlobal.GoSDK.ProfileList!=null)
                                {
                                    MyGlobal.GoSDK.ProfileList.Clear();

                                }
                              
                                //打开激光
                                MyGlobal.GoSDK.EnableProfle = true;
                                string Msg = "开始扫描:" + Side.ToString();
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
                                            if (Side==4)
                                            {
                                                //SaveSend = Encoding.UTF8.GetBytes("SAVE_NG");
                                                ShowAndSaveMsg("输出点位失败！");                                                
                                                MyGlobal.sktClient.Send(ng);
                                            }
                                           
                                        }
                                        else
                                        {
                                            if (Side==4)
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
                        if (len==0)
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
                            ok = Encoding.UTF8.GetBytes(MyGlobal.ReceiveMsg+"_OK");
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
                                       string ok1 =  Run(0);
                                         if (ok1!="OK")
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
        }

        //Matching.Form1 match = new Matching.Form1(MyGlobal.fileName1);

       
        private void navBarItem11_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {

            MyGlobal.flset2.ShowDialog();          
            MyGlobal.flset2.Text = "Fix";
                   


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

        public void loadMathParam()
        {
            string[] path2 = { MyGlobal.fileName1.Replace(".shm", ".roi"), MyGlobal.fileName2.Replace(".shm", ".roi") };
            string[] path3 = { MyGlobal.fileName1.Replace(".shm", ".xml"), MyGlobal.fileName2.Replace(".shm", ".xml") };

            for (int i = 0; i < 2; i++)
            {
                MyGlobal.parameterSet[i] = new MatchingModule.MatchingParam();
                if (File.Exists(path2[i]))
                {
                    if (File.Exists(path3[i]))
                    {
                        MyGlobal.parameterSet[i] = (MatchingModule.MatchingParam)StaticOperate.ReadXML(path3[i], MyGlobal.parameterSet[0].GetType());
                    }

                    MyGlobal.mAssistant[i] = new MatchingModule.MatchingAssistant(MyGlobal.parameterSet[i]);
                    List<ViewWindow.Model.ROI> roilist = new List<ViewWindow.Model.ROI>();
                    HWindow_Final temp = new HWindow_Final();
                    temp.viewWindow.loadROI(path2[i], out roilist);
                    MyGlobal.mAssistant[i].PreHandleRoi = roilist[0];
                    MyGlobal.mAssistant[i].Roi = roilist[1];
                }
                else
                {
                    MyGlobal.mAssistant[i] = new MatchingModule.MatchingAssistant(MyGlobal.parameterSet[i]);
                }
                 //path3 = MyGlobal.fileName1.Replace(".shm", ".xml");
              

                string fileName = MyGlobal.ModelPath + i.ToString() + ".shm";
                if (File.Exists(fileName))
                {
                    if (MyGlobal.mAssistant[i].loadShapeModel(fileName))
                    {
                        ShowAndSaveMsg("模板" + i.ToString() + "加载成功....");
                    }
                    else
                    {
                        ShowAndSaveMsg("模板" + i.ToString() + "加载失败....");
                    }
                }
                else
                {
                    ShowAndSaveMsg("模板" + i.ToString() + "未找到....");
                }
            }
            

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


        public void RunOffline(string ImagePath)
        {
            try
            {
                for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        MyGlobal.ImageMulti[i][j].Dispose();
                    }

                }
                MyGlobal.ImageMulti.Clear();
                sidelist.Clear();

                string[] HeightStr = { "Side1H.tiff", "Side2H.tiff", "Side3H.tiff", "Side4H.tiff" };
                string[] Intensity = { "Side1I.tiff", "Side2I.tiff", "Side3I.tiff", "Side4I.tiff" };

                for (int i = 0; i < 4; i++)
                {
                    HObject[] image = new HObject[2];

                    HOperatorSet.ReadImage(out image[0], ImagePath +"\\" + Intensity[i]);
                    HOperatorSet.ReadImage(out image[1], ImagePath + "\\" + HeightStr[i]);
                    MyGlobal.hWindow_Final[i].HobjectToHimage(image[0]);
                    MyGlobal.ImageMulti.Add(image);
                   
                }

                for (int i = 0; i < 4; i++)
                {
                    string OK = RunOutLine(i+1, i);
                    if (OK != "OK")
                    {
                        ShowAndSaveMsg(OK);
                        //for (int j = 0; j < MyGlobal.ImageMulti.Count; j++)
                        //{
                        //    for (int k = 0; j < k; j++)
                        //    {
                        //        if (MyGlobal.ImageMulti[j][j]!=null)
                        //        {
                        //            MyGlobal.ImageMulti[j][j].Dispose();
                        //        }                               
                        //    }

                        //}
                        //MyGlobal.ImageMulti.Clear();
                        //sidelist.Clear();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

                ShowAndSaveMsg("RunOffline :"+ ex.Message);
            }
            
        }      
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //手动测试
            //for (int i = 0; i < sidelist.Count; i++)
            //{
            //    string OK = RunOutLine(sidelist[i], i);
            //    if (OK != "OK")
            //    {
            //        ShowAndSaveMsg(OK);
            //    }
            //}
            //手动测试
            for (int i = 0; i < MyGlobal.ImageMulti.Count; i++)
            {
                string OK = RunOutLine(i+1, i);
                if (OK != "OK")
                {
                    ShowAndSaveMsg(OK);
                }
            }

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
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //打开图片
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Multiselect = true;
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                
                for (int i = 0; i <MyGlobal.ImageMulti.Count; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        if (MyGlobal.ImageMulti[i][j]!=null)
                        {
                            MyGlobal.ImageMulti[i][j].Dispose();
                        }
                      
                    }
                                      
                }
                MyGlobal.ImageMulti.Clear();
                sidelist.Clear();
                int len = openfile.FileNames.Length;
               
                string[] namesI = new string[len/2];
                string[] namesH = new string[len/2];
                if (len<2)
                {
                    return;
                }
                int orderi = 0; int orderh = 0;
                foreach (var item in openfile.FileNames)
                {
                    for (int i = orderi; i < namesH.Length; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (item.Contains((j + 1).ToString() + "I.tiff"))
                            {
                                namesI[i] = item;
                                orderi = i+1;
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
                            if (item.Contains((j + 1).ToString() + "H.tiff"))
                            {
                                namesH[i] = item;
                                orderh = i+1;
                                break;
                            }
                        }
                        break;
                    }
                }


                if (namesH[0] == null)
                {
                    return;
                }
                for (int i = 0; i < namesH.Length; i++)
                {
                    HObject[] image = new HObject[2];

                    HOperatorSet.ReadImage(out image[0], namesI[i]);
                    HOperatorSet.ReadImage(out image[1], namesH[i]);

                    MyGlobal.ImageMulti.Add(image);
                    HObject rgbImage = new HObject();
                    //PseudoColor.GrayToPseudoColor(image[1], out rgbImage, true, -20, 10);

                    //MyGlobal.hWindow_Final[i].HobjectToHimage(rgbImage);
                    MyGlobal.hWindow_Final[i].HobjectToHimage(image[0]);
                    //rgbImage.Dispose();
                    GC.Collect();
                }

            }
        }

        private void navBarItem13_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //FindMax[1].ShowDialog();
        }
        //Matching.Form1 match2 = new Matching.Form1(MyGlobal.fileName2);
        private void navBarItem6_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //string fileName = MyGlobal.ModelPath + "1.shm";
            //Matching.Form1 match = new Matching.Form1(fileName);
            //match.ShowDialog();

            string path2 = MyGlobal.fileName2.Replace(".shm", ".roi");
            List<ViewWindow.Model.ROI> roilist = new List<ViewWindow.Model.ROI>();
            if (File.Exists(path2))
            {
                HWindow_Final temp = new HWindow_Final();
                temp.viewWindow.loadROI(path2, out roilist);
                MyGlobal.mAssistant[1].PreHandleRoi = roilist[0];
                MyGlobal.mAssistant[1].Roi = roilist[1];
            }
            //match2.ShowDialog();
        }
        double total = 0;
        double ng = 0;
        double Per = 0;
        private void navBarItem7_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (total == 0)
            {
                Per = 0;
            }
            else
            {
                Per = Math.Round((total - ng) / total * 100, 3);
            }
            Frm_sql fs = new Frm_sql(total, ng, Per);
            fs.ShowDialog();
        }

        string namestart = "时间," + "工位,"  + "高度1," + "高度2," + "高度3," + "实时良率," + "总计," + "结果";
        string namestartid = "@时间," + "@工位," + "@高度1,"  + "@高度2," + "@高度3," + "@实时良率," + "@总计," + "@结果";
        void  WriteToTable(int Sta,double H1,double H2,double H3,bool OK)
        {
            string StationName = Sta == 0 ? "左工位 ": "右工位";

            string Okstring = OK ? "OK" : "NG";
            total++;
            if (!OK)
            {
                ng++;
            }
            Per = Math.Round((100 * (total - ng) / total), 2);

            SQLiteHelper.NewTable(namestart);
            string tableName = DateTime.Now.ToString("[yyyy/MM/dd]");
            string time = string.Format("{0}/{1}/{2} {3}", DateTime.Now.Year, DateTime.Now.Month,
                DateTime.Now.Day, DateTime.Now.ToShortTimeString());
            time = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            string sqlstr = string.Format("insert into {0} ({1}) values ({2})", tableName, namestart, namestartid);
            SQLiteParameter[] sqlPamram = {new SQLiteParameter("@时间",time),
                new SQLiteParameter("@工位",StationName),new SQLiteParameter("@高度1",H1),
                new SQLiteParameter("@高度2", H2),new SQLiteParameter("@高度3", H3),
                new SQLiteParameter("@实时良率", Per),
                new SQLiteParameter("@总计", total),
                new SQLiteParameter("@结果",Okstring)};
            int a = SQLiteHelper.ExecuteNonQuery(sqlstr, sqlPamram);
        }
        VisionTool.FitLineSet flset = new VisionTool.FitLineSet();
        private void navBarItem12_LinkPressed(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {           
            flset.ShowDialog();           
        }
       
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OfflineFrm OffFram = new OfflineFrm();
            OffFram.Run = new OfflineFrm.RunOff(RunOffline);
            OffFram.Show();

        }

        private void btn_MessageClean_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
