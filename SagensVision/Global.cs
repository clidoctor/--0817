using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SagensSdk;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;
using ChoiceTech.Halcon.Control;
using System.Net;
using System.Threading;
using HalconDotNet;
using System.Runtime.Serialization;
using SagensVision.VisionTool;

namespace SagensVision
{
    public static class  MyGlobal
    {
        public static string DataPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\";
        public static string AllTypePath = AppDomain.CurrentDomain.BaseDirectory + "AllType\\";

        public static string ConfigPath_Right = AppDomain.CurrentDomain.BaseDirectory +  "Config_Right\\";
        public static string ConfigPath_Left = AppDomain.CurrentDomain.BaseDirectory + "Config_Left\\";

        //public static string imgRotatePath = AppDomain.CurrentDomain.BaseDirectory + "Config\\" + "imgRotate.txt";
        public static string BaseTxtPath_Right = AppDomain.CurrentDomain.BaseDirectory + "Config_Right\\" + "BaseHeight.xml";
        public static string BaseTxtPath_Left = AppDomain.CurrentDomain.BaseDirectory + "Config_Left\\" + "BaseHeight.xml";

        public static string SaveDatFileDirectory = "Data\\datfile\\";

        public static SagensSdk.GoSdkMaker GoSDK = new GoSdkMaker();
        
        public static Socket sktServer;
        public static Socket sktClient;        
        public static string ReceiveMsg = "";
        public static GlobalConfig globalConfig = new GlobalConfig();
        public static GlobalPointSet globalPointSet_Right = new GlobalPointSet();
        public static GlobalPointSet globalPointSet_Left = new GlobalPointSet();

        public static bool isShowHeightImg;
        public static HWindow_Final[] hWindow_Final = new HWindow_Final[4];
        public static Thread thdWaitForClientAndMessage;
        public static bool sktOK = false;
        public static HTuple[] HomMat3D = new HTuple[4];
        //public static Fixcs fix = new Fixcs();
        public static FitLineSet flset2 = new FitLineSet("Fix");
        public static List<HObject[]> ImageMulti = new List<HObject[]>();
        //public static int[] imgRotateArr = new int[4];
        //public static List<double[][]> ZCoord = new List<double[][]>();//z值基准高度
        public static XYZBaseCoord xyzBaseCoord_Right = new XYZBaseCoord();
        public static XYZBaseCoord xyzBaseCoord_Left = new XYZBaseCoord();
        public static SavePathName PathName = new SavePathName();
        public static FindPointTool Left_findPointTool_Find = new FindPointTool();
        public static FindPointTool Left_findPointTool_Fix = new FindPointTool();
        public static FindPointTool Right_findPointTool_Find = new FindPointTool();
        public static FindPointTool Right_findPointTool_Fix = new FindPointTool();
        public static bool IsRight = true;
    }

    public  class GlobalConfig
    {
        public  string SensorIP = "127.0.0.1";
        public  int SaveDays = 7;
        public  bool isChinese = true;
        public  string SendMsg = "";
        public  string MotorIpAddress = "127.0.0.1";
        public  int MotorPort = 8080;
        public DataContext dataContext = new DataContext();
        public int Count = 0;
        public double zRange;//扫描z范围
        public double zStart;
        public double Color_min = 0;
        public double Color_max = 0;//颜色区间
              
        //保存数据选项
        public bool isSaveKdat;
        public bool isSaveFileDat;
        public bool isSaveImg;
        //public bool isUseAnchorDeg = true;

        public bool enableAlign;
    }

    public class GlobalPointSet
    {
        public double TotalZoffset = 0;//整体Z
        public int Startpt = 1;//起始点
        public double HeightMin = 0;//最小高度
        public double HeightMax = 0;
        public double XYMin = 0;
        public double XYMax = 0;
        public GlobalParam[] gbParam = new GlobalParam[4];
        //图像旋转角度
        public int[] imgRotateArr = new int[4];

        //产能
        public int OkCnt = 0;
        public int AnchorErrorCnt = 0;
        public int FindEgdeErrorCnt = 0;
        public int ExploreHeightErrorCnt = 0;

        public GlobalPointSet()
        {
            for (int i = 0; i < 4; i++)
            {
                gbParam[i] = new GlobalParam();
            }
        }

    }

    public class SavePathName
    {       
        string currentType = "";
        public string CurrentType
        {
            get
            {
                return currentType;
            }
            set
            {
                this.currentType = value;
                if (currentType!="")
                {
                    MyGlobal.ConfigPath_Right = MyGlobal.AllTypePath + currentType + "\\Config_Right\\" ;
                    MyGlobal.ConfigPath_Left = MyGlobal.AllTypePath + currentType + "\\Config_Left\\"  ;

                    MyGlobal.BaseTxtPath_Right = MyGlobal.AllTypePath + currentType + "\\Config_Right\\"  + "BaseHeight.xml";
                    MyGlobal.BaseTxtPath_Left = MyGlobal.AllTypePath + currentType + "\\Config_Left\\"  + "BaseHeight.xml";

                    if (!Directory.Exists(MyGlobal.ConfigPath_Right))
                    {
                        Directory.CreateDirectory(MyGlobal.ConfigPath_Right);
                    }
                    if (!Directory.Exists(MyGlobal.ConfigPath_Left))
                    {
                        Directory.CreateDirectory(MyGlobal.ConfigPath_Left);
                    }

                    string[] SideName = { "Side1", "Side2", "Side3", "Side4" };
                    for (int i = 0; i < 4; i++)
                    {
                        if (!Directory.Exists(MyGlobal.ConfigPath_Right + SideName[i]+"\\"))
                        {
                            Directory.CreateDirectory(MyGlobal.ConfigPath_Right + SideName[i] + "\\");
                        }
                        if (!Directory.Exists(MyGlobal.ConfigPath_Left + SideName[i] + "\\"))
                        {
                            Directory.CreateDirectory(MyGlobal.ConfigPath_Left + SideName[i] + "\\");
                        }
                    }
                }
                
            }
        }
    }



    public class GlobalParam
    {
       public double Xoffset = 0;
       public double Yoffset = 0;
    }

    public class XYZBaseCoord
    {
        public List<double[]> XCoord;
        public List<double[]> YCoord;
        public List<double[][]> ZCoord;
        public List<double[]> Dist;
        public List<IntersetionCoord> intersectCoordList = new List<IntersetionCoord>();
    }

    public static class StaticOperate
    {

        public static bool CreateServer(ref string errorMsg)
        {
            errorMsg = "";
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(MyGlobal.globalConfig.MotorIpAddress), MyGlobal.globalConfig.MotorPort);
                MyGlobal.sktServer = new Socket(ipPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                MyGlobal.sktServer.Bind(ipPoint);
                MyGlobal.sktServer.Listen(1);
                MyGlobal.thdWaitForClientAndMessage.Start();
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public static void SaveImage(HObject Image,string Count, string Product ,bool isRight)
        {
            string RorL = isRight ? "Right" : "Left";
            string path = MyGlobal.DataPath + "Image\\" + string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + "\\"+ RorL + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
           
            string[] dirs = Directory.GetDirectories(MyGlobal.DataPath + "Image\\");

            for (int i = 0; i < dirs.Length; i++)
            {
                DateTime dt = File.GetCreationTime(dirs[i]);
                TimeSpan ts = DateTime.Now - dt;
                if (ts.Days > 2)
                {
                    Directory.Delete(dirs[i], true);

                }
            }
            //数量大于500时

            DirectoryInfo dirinf = new DirectoryInfo(path);
            FileInfo[] files = dirinf.GetFiles();
            Array.Sort<FileInfo>(files, new FileCompare());

            if (files.Length > 17)
            {
                for (int i = 0; i < files.Length - 17; i++)
                {
                    File.Delete(files[i].FullName);
                }

            }

            if (!Directory.Exists(path + FormMain.saveImageTime))
            {
                Directory.CreateDirectory(path + FormMain.saveImageTime);
            }
            string filePath = path + FormMain.saveImageTime + "\\" + Product ;
           HOperatorSet.WriteImage(Image, "tiff", 0, filePath);


        }

        public static void SaveErrorImage(HObject Image, string Count, string Product,bool isRight)
        {
            string RorL = isRight ? "Right" : "Left";
            string path = MyGlobal.DataPath + "ErrorImage\\" + string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + "\\" + RorL + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string[] dirs = Directory.GetDirectories(MyGlobal.DataPath + "ErrorImage\\");

            for (int i = 0; i < dirs.Length; i++)
            {
                DateTime dt = File.GetCreationTime(dirs[i]);
                TimeSpan ts = DateTime.Now - dt;
                if (ts.Days > 2)
                {
                    Directory.Delete(dirs[i], true);

                }
            }
            //数量大于500时

            DirectoryInfo dirinf = new DirectoryInfo(path);
            FileInfo[] files = dirinf.GetFiles();
            Array.Sort<FileInfo>(files, new FileCompare());

            if (files.Length > 17)
            {
                for (int i = 0; i < files.Length - 17; i++)
                {
                    File.Delete(files[i].FullName);
                }

            }

            if (!Directory.Exists(path + FormMain.saveImageTime))
            {
                Directory.CreateDirectory(path + FormMain.saveImageTime);
            }
            string filePath = path + FormMain.saveImageTime + "\\" + Product;
            HOperatorSet.WriteImage(Image, "tiff", 0, filePath);


        }
        public class FileCompare : IComparer<FileInfo>
        {
            public int Compare(FileInfo x, FileInfo y)
            {
                return x.LastWriteTime.CompareTo(y.LastWriteTime);//递增
            }
        }
        public static void writeTxt(string path, string data)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

                sw.Write(data);
                sw.Flush();
                sw.Close();
                fs.Close();

            }
            catch (Exception)
            {

                throw;
            }

        }

        public static void SaveLog(string info)
        {
            string dir = MyGlobal.DataPath + "Log";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string[] files = Directory.GetFiles(dir);
            for (int i = 0; i < files.Length; i++)
            {
                DateTime dt = File.GetCreationTime(files[i]);
                TimeSpan ts = DateTime.Now - dt;
                if (ts.Days > MyGlobal.globalConfig.SaveDays)
                {
                    File.Delete(files[i]);
                }
            }

            string filename = dir + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            StreamWriter sw = new StreamWriter(filename, true);
            sw.WriteLine(info);
            sw.Flush();
            sw.Close();
        }

        public static void SaveErrorLog(string info)
        {
            string dir = MyGlobal.DataPath + "ErrorLog";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string[] files = Directory.GetFiles(dir);
            for (int i = 0; i < files.Length; i++)
            {
                DateTime dt = File.GetCreationTime(files[i]);
                TimeSpan ts = DateTime.Now - dt;
                if (ts.Days > MyGlobal.globalConfig.SaveDays)
                {
                    File.Delete(files[i]);
                }
            }

            string filename = dir + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            StreamWriter sw = new StreamWriter(filename, true);
            sw.WriteLine(info);
            sw.Flush();
            sw.Close();
        }

        public static void SaveExcelData(int boardNum, string Height, string Width, string Area, string index ="")
        {
            try
            {
                string filePath = MyGlobal.DataPath + "Excel\\" + string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + "\\";
                string fileName = filePath + string.Format("第{0}工位", boardNum) + ".xls";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string[] files = Directory.GetFiles(filePath);
                for (int i = 0; i < files.Length; i++)
                {
                    DateTime dt = File.GetCreationTime(files[i]);
                    TimeSpan ts = DateTime.Now - dt;
                    if (ts.Days > MyGlobal.globalConfig.SaveDays)
                    {
                        File.Delete(files[i]);
                    }
                }

                if (!Directory.Exists(filePath)) 
                {
                    Directory.CreateDirectory(filePath);
                }
                if (!File.Exists(fileName))
                {
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        if (MyGlobal.globalConfig.isChinese)
                        {
                            sw.Write("AnchorX1" + "\t" + "AnchorY1" + "\t" + "AnchorAngle1" + "\t" + "AnchorX2" + "\t"+ "AnchorY2" + "\t" + "AnchorAngle2" + "\t"+
                                     "AnchorX3" + "\t" + "AnchorY3" + "\t" + "AnchorAngle3" + "\t" + "AnchorX4" + "\t" + "AnchorY4" + "\t" + "AnchorAngle4" + "\t" + "\r\n");
                        }
                        else
                        {                           
                            sw.Write("Height1" + "\t" + "Height2" + "\t" + "Height3" + "\t" + "\r\n");
                        }
                        sw.Flush();
                        sw.Close();
                    }
                }
                using (StreamWriter swrite = File.AppendText(fileName))
                {
                    //swrite.Write(Height + "\t" + Width + "\t" + Area + "\t" + "\r\n");
                    swrite.Write(Height + "\t" + Width + "\t" + Area );

                    //if (MyGlobal.globalConfig.isChinese)
                    //{
                    //    //swrite.Write("高度1" + "\t" + Height + "\r\n" + "高度2" + "\t" + Width + "\r\n" + "高度3" + "\t" + Area + "\r\n");


                    //}
                    //else
                    //{
                    //    swrite.Write("Height1" + "\t" + "Height2" + "\t" + "Height3" + "\t" + "\r\n");
                    //    //swrite.Write("Glue Height1" + "\t" + Height + "\r\n" + "Glue Height" + "\t" + Width + "\r\n" + "Glue Area" + "\t" + Area + "\r\n");

                    //}
                    //swrite.Write("------------------------------------" + "\r\n");

                    swrite.Flush();
                    swrite.Close();
                }

            }
            catch (Exception )
            {


            }
        }

        public static void SaveExcelData( string Header, string data,string FileName)
        {
            try
            {
                string filePath = MyGlobal.DataPath + "Excel\\" + string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + "\\";
                string fileName = filePath + FileName + ".xls";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string[] files = Directory.GetFiles(filePath);
                for (int i = 0; i < files.Length; i++)
                {
                    DateTime dt = File.GetCreationTime(files[i]);
                    TimeSpan ts = DateTime.Now - dt;
                    if (ts.Days > MyGlobal.globalConfig.SaveDays)
                    {
                        File.Delete(files[i]);
                    }
                }

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (!File.Exists(fileName))
                {
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        if (MyGlobal.globalConfig.isChinese)
                        {
                            sw.Write(Header);
                        }                        
                        sw.Flush();
                        sw.Close();
                    }
                }
                using (StreamWriter swrite = File.AppendText(fileName))
                {
                    //swrite.Write(Height + "\t" + Width + "\t" + Area + "\t" + "\r\n");
                    swrite.Write(data);

                    //if (MyGlobal.globalConfig.isChinese)
                    //{
                    //    //swrite.Write("高度1" + "\t" + Height + "\r\n" + "高度2" + "\t" + Width + "\r\n" + "高度3" + "\t" + Area + "\r\n");


                    //}
                    //else
                    //{
                    //    swrite.Write("Height1" + "\t" + "Height2" + "\t" + "Height3" + "\t" + "\r\n");
                    //    //swrite.Write("Glue Height1" + "\t" + Height + "\r\n" + "Glue Height" + "\t" + Width + "\r\n" + "Glue Area" + "\t" + Area + "\r\n");

                    //}
                    //swrite.Write("------------------------------------" + "\r\n");

                    swrite.Flush();
                    swrite.Close();
                }

            }
            catch (Exception)
            {


            }
        }


        public static void WriteXML(object obj, string fileName)
        {
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.OmitXmlDeclaration = true;
            ws.Indent = true;
            StreamWriter sw = new StreamWriter(fileName, false);
            try
            {
                if (null == obj)
                {
                    throw new ArgumentNullException("obj", "obj对象不能为null");
                }

                
                using (XmlWriter writer = XmlWriter.Create(sw, ws))
                {
                    //去除默认命名空间xmlns:xsd和xmlns:xsi
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, string.Empty);
                    XmlSerializer formatter = new XmlSerializer(obj.GetType());
                    formatter.Serialize(writer, obj, ns);
                    writer.Close();
                }
                sw.Close();
            }
            catch (Exception)
            {
                sw.Close();
                throw;
            }
            
        }
        public static object ReadXML(string fileName, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type不能为null");
            }
            XmlSerializer serializer = new XmlSerializer(type);
            StreamReader sr = new StreamReader(fileName, Encoding.UTF8);
            try
            {               
                object obj = serializer.Deserialize(sr);
                sr.Close();
                return obj;
            }
            catch (Exception)
            {
                sr.Close();
                throw;
            }
            
        }


        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data">轮廓数据</param>
        /// <param name="iRet">工位（1，2)</param>
        /// <param name="name">（左边/右边/上边/下边）</param>
        /// <param name="Showtime">是否显示时间</param>
        static public void WriteInSerializable(string path, object data, int iRet, string name, bool Showtime = true, bool OK = false)
        {
            try
            {
                string tm = string.Format("{0}年{1}月{2}日", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                string PathD = path;
                string okstring = OK ? "OK" : "NG";
                if (Showtime)
                {
                    //path = path + tm + "\\";
                    path = path + okstring + "\\" + tm + "\\";
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string time = "";

                if (Showtime)
                {
                    time = System.DateTime.Now.ToString("HH_mm_ss");
                    string[] dirs = Directory.GetDirectories(PathD);
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        DateTime dt = File.GetCreationTime(dirs[i]);
                        TimeSpan ts = DateTime.Now - dt;
                        if (ts.Days > 3)
                        {
                            Directory.Delete(dirs[i], true);
                        }
                    }

                }

                //数量大于5000时
                //string[] files = Directory.GetFiles(path);
                DirectoryInfo dirinf = new DirectoryInfo(path);
                FileInfo[] files = dirinf.GetFiles();
                Array.Sort<FileInfo>(files, new FileCompare());

                if (files.Length > 999)
                {
                    for (int i = 0; i < files.Length - 999; i++)
                    {
                        File.Delete(files[i].FullName);
                    }

                }
                string filePath = path + iRet + "_" + time + "_" + name;
                if (time == "")
                {
                    filePath = path + iRet + "_" + name;
                }
                
               
                //if (Showtime)
                //{
                //    DataCurrentName = "data_" + iRet + "_" + name + "_" + time + ".dat";
                //}
                BinaryFormatter binaryFomat = new BinaryFormatter();
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    binaryFomat.Serialize(fs, data);
                    fs.Dispose();
                }

            }
            catch (Exception ex)
            {
                //Debug.WriteLine("文件打开失败{0}", ex.ToString());
            }
            finally
            {

            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="filePath">文件路径及名称</param>
        /// <returns></returns>
        static public object ReadInSerializable(string filePath)
        {
            try
            {

                if (File.Exists(filePath))
                {

                    BinaryFormatter binaryFomat = new BinaryFormatter();
                    object data;
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    data = (object)binaryFomat.Deserialize(fs);
                    fs.Dispose();
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("文件打开失败{0}", ex.ToString());
                return null;
            }

        }



     //   /// <summary>
     //   /// 标题：支持 XML 序列化的 Dictionary
     //   /// </summary>
     //   /// <typeparam name="TKey"></typeparam>
     //   /// <typeparam name="TValue"></typeparam>
     //   [XmlRoot("SerializableDictionary")]
     //   public class SerializableDictionary<TKey, TValue>
     //     : Dictionary<TKey, TValue>, IXmlSerializable
     //   {
     //       #region 构造函数

     //       public SerializableDictionary()
     //           : base()
     //       {
     //       }

     //       public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
     //           : base(dictionary)
     //       {
     //       }

     //       public SerializableDictionary(IEqualityComparer<TKey> comparer)
     //           : base(comparer)
     //       {
     //       }

     //       public SerializableDictionary(int capacity)
     //           : base(capacity)
     //       {
     //       }

     //       public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
     //           : base(capacity, comparer)
     //       {
     //       }

     //       protected SerializableDictionary(SerializationInfo info, StreamingContext context)
     //           : base(info, context)
     //       {
     //       }

     //       #endregion 构造函数

     //       #region IXmlSerializable Members

     //       public System.Xml.Schema.XmlSchema GetSchema()
     //       {
     //           return null;
     //       }

     //}

    }

    public class IniFileOperater
    {
        [DllImport("Kernel32.dll")]
        private static extern bool WritePrivateProfileString(string root, string key, string value, string filePath);
        [DllImport("Kernel32.dll")]
        private static extern int GetPrivateProfileString(string root, string key, string defaultStr, StringBuilder value, int size, string filePath);

        // <summary>

        /// 获取某个指定节点(Section)中所有KEY和Value

        /// </summary>

        /// <param name="root">节点名称</param>

        /// <param name="lpReturnedString">返回值的内存地址,每个之间用\0分隔</param>

        /// <param name="nSize">内存大小(characters)</param>

        /// <param name="lpFileName">Ini文件</param>

        /// <returns>内容的实际长度,为0表示没有内容,为nSize-2表示内存大小不够</returns>

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]

        private static extern uint GetPrivateProfileSection(string root, IntPtr lpReturnedString, uint nSize, string lpFileName);

        private string _IniFileName = string.Empty;

        public IniFileOperater(string filename)
        {
            _IniFileName = filename;
            if (!Directory.Exists(filename.Remove(filename.LastIndexOf("\\"))))
                Directory.CreateDirectory(filename.Remove(filename.LastIndexOf("\\")));

        }
        /// <summary>

        /// 获取INI文件中指定节点(Section)中的所有条目(key=value形式)

        /// </summary>

        /// <param name="section">节点名称</param>

        /// <returns>指定节点中的所有项目,没有内容返回string[0]</returns>

        public string[] INIGetAllItems(string section)

        {

            //返回值形式为 key=value,例如 Color=Red

            uint MAX_BUFFER = 32767;    //默认为32767



            string[] items = new string[0];      //返回值



            //分配内存

            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));



            uint bytesReturned = GetPrivateProfileSection(section, pReturnedString, MAX_BUFFER, _IniFileName);



            if (!(bytesReturned == MAX_BUFFER - 2) || (bytesReturned == 0))

            {

                string returnedString = Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned);

                items = returnedString.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);

            }



            Marshal.FreeCoTaskMem(pReturnedString);     //释放内存



            return items;

        }

        public bool WriteEntry(string root, string key, string value)
        {
            return WritePrivateProfileString(root, key, value, _IniFileName);
        }

        public bool WriteEntry(string root, string key, double value)
        {
            return WritePrivateProfileString(root, key, value.ToString(), _IniFileName);
        }

        public bool WriteEntry(string root, string key, int value)
        {
            return WritePrivateProfileString(root, key, value.ToString(), _IniFileName);
        }
        public bool WriteEntry(string root, string key, bool value)
        {
            return WritePrivateProfileString(root, key, value.ToString(), _IniFileName);
        }

        public void ReadEntry(string root, string key, out string value)
        {
            try
            {
                StringBuilder tvalue = new StringBuilder();
                GetPrivateProfileString(root, key, "0", tvalue, 100, _IniFileName);
                value = tvalue.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReadEntry(string root, string key, out double value)
        {
            try
            {
                StringBuilder tvalue = new StringBuilder();
                GetPrivateProfileString(root, key, "0", tvalue, 100, _IniFileName);
                if (tvalue.Length == 0)
                {
                    value = 0;
                    return;
                }
                value = double.Parse(tvalue.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReadEntry(string root, string key, out int value)
        {
            try
            {
                StringBuilder tvalue = new StringBuilder();
                GetPrivateProfileString(root, key, "0", tvalue, 100, _IniFileName);
                if (tvalue.Length == 0)
                {
                    value = 0;
                    return;
                }
                value = int.Parse(tvalue.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ReadEntry(string root, string key, out bool value)
        {
            try
            {
                StringBuilder tvalue = new StringBuilder();
                GetPrivateProfileString(root, key, "false", tvalue, 100, _IniFileName);
                if (tvalue.Length == 0)
                {
                    value = false;
                    return;
                }
                value = bool.Parse(tvalue.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
