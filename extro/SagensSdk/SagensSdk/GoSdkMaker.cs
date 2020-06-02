using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GoVision3D.SaveLoad;
using System.Diagnostics;

namespace SagensSdk
{
    /*
     *使用说明
     * 1.操作传感器：连接（Connect),断开（Disconnect），开始扫描（Start），
     * 结束扫描（Stop，调用此方法标志轮廓与测量数据接收完成），切换作业（CutJob),下载REC文件（DownLoad3dFile)
     * 2.接收数据：数据存放在 （Region接收数据）中，轮廓数据接收完成事件（ProfileRecFinish），其它一致
     * 3.加密模块：将（Region加密模块中的属性设置完成即可开启加密），是否开始（isStartCheck),
     * 到期时间（expirationTime），联系电话（tel),提示开始日期（warnStartDay），每天提示时间（warnTime）
     * 
     **/


    public class GoSdkMaker
    {
        public GoSdkMaker() { }
        public GoSdkMaker(string gocatorIp)
        {
            this.gocatorIp = gocatorIp;
        }
        private string gocatorIp;



        IntPtr go_api = IntPtr.Zero;
        IntPtr go_system = IntPtr.Zero;
        IntPtr go_sensor = IntPtr.Zero;
        IntPtr go_setuo = IntPtr.Zero;
        IntPtr Setup = IntPtr.Zero;
        IntPtr go_role = IntPtr.Zero;
        public DataContext context = new DataContext();

        address addr = new address();
        IntPtr addrPtr = IntPtr.Zero;
        kStatus state;

        public bool connect(string gocatorIp, ref string errMsg)
        {
            this.gocatorIp = gocatorIp;
            return connect(ref errMsg);
        }
        private static onDataType ondatatype;
        public bool connect(ref string errMsg)
        {
            if (gocatorIp != null)
            {
                state = GoSdkWrapper.GoSdk_Construct(ref go_api);
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSystem_Construct Error:{0}", state);
                    return false;
                }

                state = GoSdkWrapper.GoSystem_Construct(ref go_system, IntPtr.Zero);
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSystem_Construct Error:{0}", state);
                    return false;
                }
                addrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(addr));
                Marshal.StructureToPtr(addr, addrPtr, false);

                state = GoSdkWrapper.kIpAddress_Parse(addrPtr, gocatorIp);
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("kIpAddress_Parse Error:{0}", state);
                    return false;
                }
                state = GoSdkWrapper.GoSystem_FindSensorByIpAddress(go_system, addrPtr, ref go_sensor);
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSystem_FindSensorByIpAddress Error:{0}", state);
                    return false;
                }
                state = GoSdkWrapper.GoSystem_Connect(go_system);
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSystem_Connect Error:{0}", state);
                    return false;
                }
                state = GoSdkWrapper.GoSystem_EnableData(go_system, true);
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSystem_EnableData Error:{0}", state);
                    return false;
                }

               
                Setup = GoSdkWrapper.GoSensor_Setup(go_sensor);
                zRange = GoSdkWrapper.GoSetup_ActiveAreaHeight(Setup, GoRole.GO_ROLE_MAIN);
                z_byte_resolution = 255 / zRange;
                zStart = GoSdkWrapper.GoSetup_ActiveAreaZ(Setup, GoRole.GO_ROLE_MAIN);

                ondatatype = new onDataType(OnData);
                state = GoSdkWrapper.GoSystem_SetDataHandler(go_system, ondatatype, context);
                GC.KeepAlive(ondatatype);
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSystem_SetDataHandler Error:{0}", state);
                    return false;
                }
                profileDataZ = new List<Profile>();
                measurementDataList = new List<MeasurementData>();

                #region 加密模块
                SecretKey.License l = new SecretKey.License("license.dat", expirationTime, isStartCheck, tel);
                l.GocatorId = GoSdkWrapper.GoSensor_Id(go_sensor).ToString();
                l.Warn(warnStartDay, warnTime);
                #endregion
                context.serialNumber = GoSdkWrapper.GoSensor_Id(go_sensor);
                GoRole role = GoRole.GO_ROLE_MAIN;
                //context.yResolution =(double) GoSdkWrapper.GoSetup_YSpacingCount(Setup, role);
                context.yResolution = GoSdkWrapper.GoSetup_EncoderSpacing(Setup);
                context.xResolution = Math.Round(GoSdkWrapper.GoSetup_SpacingIntervalSystemValue(Setup,role), 3);
                return true;
            }
            else
            {
                errMsg = "Gocator ip is Empty";
                return false;
            }
        }

        #region 加密模块
        
        /// <summary>
        /// Format: "yyyy-mm-dd hh:mm:ss"
        /// </summary>
        public string expirationTime { set; get; } = "2020-02-25 18:00:00";

        public bool isStartCheck { set; get; } = true;
        private string tel { set; get; } = "17748696626";
        /// <summary>
        /// Format: "yyyy-mm-dd"
        /// </summary>
        private string warnStartDay { set; get; } = "2020-1-13";
        /// <summary>
        /// Format: "hh:mm:ss"
        /// </summary>
        private string warnTime { set; get; } = "16:01:00";
        #endregion
        private int OnData(DataContext ctx, IntPtr sys, IntPtr data)
        {
            
            if (!SecretKey.License.SnOk)
            {
                string errMsg = "";
                string warnMsg = "传感器试用权限已到期，传感器将停止接收数据,如有疑问请联系：" + tel;
                if (DisConnect(ref errMsg))
                {
                    warnMsg += " !";
                }
                MessageBox.Show(warnMsg);
                return 0;
            }
            IntPtr dataObj = IntPtr.Zero;

            IntPtr StampMsg = IntPtr.Zero;
            IntPtr StampPtr = IntPtr.Zero;
            GoStamp stamp = new GoStamp();

            IntPtr ProfileMsgZ = IntPtr.Zero;
            Profile mProfile = new Profile();

            IntPtr ProfileMsgIntensity = IntPtr.Zero;

            IntPtr SurfaceMsgZ = IntPtr.Zero;
            IntPtr SurfaceMsgIntensity = IntPtr.Zero;

            IntPtr MeasurementMsg = IntPtr.Zero;
            MeasurementData measurementData = new MeasurementData();
            for (uint i = 0; i < GoSdkWrapper.GoDataSet_Count(data); i++)
            {
                dataObj = GoSdkWrapper.GoDataSet_At(data, i);
                switch (GoSdkWrapper.GoDataMsg_Type(dataObj))
                {
                    case GoDataMessageTypes.GO_DATA_MESSAGE_TYPE_STAMP:
                        StampMsg = dataObj;
                        StampPtr = GoSdkWrapper.GoStampMsg_At(StampMsg, 0);
                        
                        stamp = (GoStamp)Marshal.PtrToStructure(StampPtr, typeof(GoStamp));
                        //if (stamp.encoder == 0)
                        //{
                        //    stamp.encoder = 0;
                        //}
                        Stamp = stamp;
                        mProfile.encoder = stamp.encoder;
                     
                        break;
                    case GoDataMessageTypes.GO_DATA_MESSAGE_TYPE_SURFACE:

                        SurfaceMsgZ = dataObj;
                        ctx.xResolution = (double)GoSdkWrapper.GoSurfaceMsg_XResolution(SurfaceMsgZ) / 1000000;
                        ctx.zResolution = (double)GoSdkWrapper.GoSurfaceMsg_ZResolution(SurfaceMsgZ) / 1000000;
                        ctx.yResolution = (double)GoSdkWrapper.GoSurfaceMsg_YResolution(SurfaceMsgZ) / 1000000;
                        ctx.xOffset = (double)GoSdkWrapper.GoSurfaceMsg_XOffset(SurfaceMsgZ) / 1000;
                        ctx.yOffset = (double)GoSdkWrapper.GoSurfaceMsg_YOffset(SurfaceMsgZ) / 1000;
                        ctx.zOffset = (double)GoSdkWrapper.GoSurfaceMsg_ZOffset(SurfaceMsgZ) / 1000;
                        surfaceWidth = GoSdkWrapper.GoSurfaceMsg_Width(SurfaceMsgZ);
                        surfaceHeight = GoSdkWrapper.GoSurfaceMsg_Length(SurfaceMsgZ);

                        IntPtr surfacePtr = GoSdkWrapper.GoSurfaceMsg_RowAt(SurfaceMsgZ,0);
                        short[] surfacePoints = new short[surfaceWidth * surfaceHeight];
                        float[] surfaceData = new float[surfaceWidth * surfaceHeight];
                        float[] surfaceDataX = new float[surfaceWidth * surfaceHeight];
                        float[] surfaceDataY = new float[surfaceWidth * surfaceHeight];
                        byte[] surfaceDataZByte = new byte[surfaceWidth * surfaceHeight];

                        //保存Kdat
                        if (SaveKdatDirectoy != null && !string.IsNullOrEmpty(SaveKdatDirectoy))
                        {
                            Point3d64f poffset = new Point3d64f() { x = ctx.xOffset, y = ctx.yOffset, z = ctx.zOffset };
                            Point3d64f pscale = new Point3d64f() { x = ctx.xResolution, y = ctx.yResolution, z = ctx.zResolution };
                            GoSurface insurface = new GoSurface() { data = surfacePtr, width = (int)surfaceWidth, height = (int)surfaceHeight, offset = poffset, scale = pscale };
                            GoSdkWrapper.SaveSufaceToFile($"{SaveKdatDirectoy}/{DateTime.Now.ToFileTimeUtc().ToString()}.kdat", ref insurface, false);
                        }

                        Marshal.Copy(surfacePtr, surfacePoints, 0, surfacePoints.Length);

                        //保存dat
                        if (SaveDatFileDirectory != null && !string.IsNullOrEmpty(SaveDatFileDirectory))
                        {
                            PointMsg pmoffset = new PointMsg() { x = ctx.xOffset, y = ctx.yOffset, z = ctx.zOffset };
                            PointMsg pmscale = new PointMsg() { x = ctx.xResolution, y = ctx.yResolution, z = ctx.zResolution };
                            SurfaceZSaveDat ssd = new SurfaceZSaveDat() { points = surfacePoints, resolution = pmscale, offset = pmoffset,width = (int)surfaceWidth,height = (int)surfaceHeight };
                            StaticTool.WriteSerializable($"{SaveDatFileDirectory}Side{RunSide}_H.dat", ssd);
                        }


                        for (int j = 0; j < surfaceHeight; j++)
                        {
                            for (int k = 0; k < surfaceWidth; k++)
                            {
                                surfaceData[j*surfaceWidth +k ] = surfacePoints[j * surfaceWidth + k] == -32768 ? -12 : (float)(ctx.zOffset + ctx.zResolution * surfacePoints[j * surfaceWidth + k]);
                                surfaceDataX[j * surfaceWidth + k] = (float)(ctx.xOffset + ctx.xResolution * k);
                                surfaceDataY[j * surfaceWidth + k] = (float)(ctx.yOffset + ctx.yResolution * j);
                                if (IsRecSurfaceDataZByte)
                                {
                                    if (surfacePoints[j * surfaceWidth + k] != -32768)
                                    {
                                        surfaceDataZByte[j * surfaceWidth + k] = (byte)Math.Ceiling(((ctx.zOffset + ctx.zResolution * surfacePoints[j * surfaceWidth + k]) - zStart) * z_byte_resolution);
                                    }
                                    else
                                    {
                                        surfaceDataZByte[j * surfaceWidth + k] = 0;
                                    }
                                }
                            }
                        }

                        this.SurfaceDataZByte = surfaceDataZByte;
                        this.surfaceDataX = surfaceDataX;
                        this.surfaceDataY = surfaceDataY;
                        this.SurfaceDataZ = surfaceData;
                        
                        break;
                    case GoDataMessageTypes.GO_DATA_MESSAGE_TYPE_SURFACE_INTENSITY:
                        SurfaceMsgIntensity = dataObj;
                        IntPtr intensityPtr = GoSdkWrapper.GoSurfaceIntensityMsg_RowAt(SurfaceMsgIntensity, 0);
                        surfaceWidth = GoSdkWrapper.GoSurfaceIntensityMsg_Width(SurfaceMsgIntensity);
                        surfaceHeight = GoSdkWrapper.GoSurfaceIntensityMsg_Length(SurfaceMsgIntensity);
                        byte[] intensityArr = new byte[surfaceWidth * surfaceHeight];
                        Marshal.Copy(intensityPtr, intensityArr, 0, intensityArr.Length);
                        SurfaceDataIntensity = intensityArr;
                        if (SaveDatFileDirectory != null && !string.IsNullOrEmpty(SaveDatFileDirectory))
                        {
                            SurfaceIntensitySaveDat ssd = new SurfaceIntensitySaveDat() { points = intensityArr, width = (int)surfaceWidth, height = (int)surfaceHeight };
                            StaticTool.WriteSerializable($"{SaveDatFileDirectory}Side{RunSide}_I.dat", ssd);
                        }
                        break;
                    case GoDataMessageTypes.GO_DATA_MESSAGE_TYPE_RESAMPLED_PROFILE:
                        if (!EnableProfle)
                        {
                            break;

                        }
                        ProfileMsgZ = dataObj;
                        ctx.xResolution = ((double)GoSdkWrapper.GoProfileMsg_XResolution(ProfileMsgZ))/1000000;
                        ctx.zResolution = ((double)GoSdkWrapper.GoProfileMsg_ZResolution(ProfileMsgZ))/1000000;
                        ctx.xOffset = ((double)GoSdkWrapper.GoProfileMsg_XOffset(ProfileMsgZ))/1000;
                        ctx.zOffset = ((double)GoSdkWrapper.GoProfileMsg_ZOffset(ProfileMsgZ))/1000;
                        profileWidth = GoSdkWrapper.GoProfileMsg_Width(ProfileMsgZ);

                        IntPtr pointPtr = GoSdkWrapper.GoResampledProfileMsg_At(ProfileMsgZ, 0);
                        short[] points = new short[profileWidth];
                        Marshal.Copy(pointPtr, points, 0, points.Length);
                        ProfilePoint[] profile = new ProfilePoint[profileWidth];
                        for (int j = 0; j < points.Length; j++)
                        {
                            profile[j].x = (float)(ctx.xOffset + ctx.xResolution * j);
                            profile[j].z = points[j] == -32768 ? -12 : (float)(ctx.zOffset + ctx.zResolution * points[j]);
                        }
                        mProfile.points = profile;
                        isRecProfileZ = true;
                        break;
                    case GoDataMessageTypes.GO_DATA_MESSAGE_TYPE_PROFILE_INTENSITY:
                        if (isRecProfileZ)
                        {
                            ProfileMsgIntensity = dataObj;
                            profileWidth = GoSdkWrapper.GoProfileIntensityMsg_Width(ProfileMsgIntensity);
                            IntPtr profileIntensityPtr = GoSdkWrapper.GoProfileIntensityMsg_At(ProfileMsgIntensity, 0);
                            byte[] profileIntensityArr = new byte[profileWidth];
                            Marshal.Copy(profileIntensityPtr, profileIntensityArr, 0, profileIntensityArr.Length);
                            for (int j = 0; j < profileIntensityArr.Length; j++)
                            {
                                mProfile.points[j].Intensity = profileIntensityArr[j];
                            }

                        }
                        break;
                    case GoDataMessageTypes.GO_DATA_MESSAGE_TYPE_MEASUREMENT:
                        MeasurementMsg = dataObj;
                        IntPtr measurementPtr = GoSdkWrapper.GoMeasurementMsg_At(MeasurementMsg, 0);
                        GoMeasurementData mData = (GoMeasurementData)Marshal.PtrToStructure(measurementPtr, typeof(GoMeasurementData));
                        measurementData.id =GoSdkWrapper.GoMeasurementMsg_Id(MeasurementMsg);
                        measurementData.value = mData.Value;
                        measurementDataList.Add(measurementData);
                        break;
                    default:
                        break;
                }
            }
            if (isRecProfileZ)
            {
                profileDataZ.Add(mProfile);
                this.SigleProfile = mProfile;
                isRecProfileZ = false;
            }
            GoSdkWrapper.GoDestroy(data);
            GC.Collect();
            return 1;
        }

        #region 操作传感器
        public bool Stop(ref string errMsg)
        {
            try
            {
                state = GoSdkWrapper.GoSystem_Stop(go_system);
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSystem_Stop Error:{0}", state);
                    return false;
                }
                ProfileRecFinish?.Invoke();
                MeasurementRecFinish?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format("GoSystem_Stop Error:{0}", ex.Message);
                return false;
            }
            
        }
        public bool Start(ref string errMsg)
        {
            try
            {
                state = GoSdkWrapper.GoSystem_Start(go_system);
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSystem_Start Error:{0}", state);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format("GoSystem_Start Error:{0}", ex.Message);
                return false;
            }
        }

        public bool CutJob(string jobName, ref string errMsg)
        {
            try
            {
                state = GoSdkWrapper.GoSensor_CopyFile(go_sensor, jobName + ".job", "_live.job");
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSensor_CopyFile Error:{0}", state);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format("GoSensor_CopyFile Error:{0}", ex.Message);
                return false;
            }
        }
        public bool Download3dFile(string destpath, ref string errMsg)
        {
            try
            {
                state = GoSdkWrapper.GoSensor_DownloadFile(go_sensor, "_live.job", destpath+".job");
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSensor_DownloadJob Error:{0}", state);
                    return false;
                }
                state = GoSdkWrapper.GoSensor_DownloadFile(go_sensor, "_live.rec", destpath + ".rec");
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSensor_DownloadRec Error:{0}", state);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format("GoSensor_DownloadFile Error:{0}", ex.Message);
                return false;
            }
        }
        public bool DisConnect(ref string errMsg)
        {
            try
            {
                state = GoSdkWrapper.GoSystem_Disconnect(go_system);
                if (state != kStatus.kOK)
                {
                    errMsg = string.Format("GoSystem_DisConnect Error:{0}", state);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format("GoSystem_DisConnect Error:{0}", ex.Message);
                return false;
            }
        }
        public bool IsConnected(ref string errMsg)
        {
            //设置掉线报警
            if (go_sensor == IntPtr.Zero)
            {
                return false;
            }
            return GoSdkWrapper.GoSensor_IsConnected(go_sensor);
        }
        #endregion

        #region 接收数据
        private GoStamp stamp;
        private float[] surfaceDataZ;//点云数据
        private float[] surfaceDataX;
        private float[] surfaceDataY;

        private byte[] surfaceDataIntensity;//亮度数据
        private Profile sigleProfile;//单个轮廓数据
        private List<Profile> profileDataZ { set; get; }//轮廓数据
        private List<MeasurementData> measurementDataList { set; get; }//测量数据
        public GoStamp Stamp
        {
            get
            {
                return stamp;
            }
            set
            {
                stamp = value;
                StampRecFinish?.Invoke();
            }
        }

        public float[] SurfaceDataZ
        {
            get
            {
                return surfaceDataZ;
            }

            set
            {
                surfaceDataZ = value;
                if (surfaceDataZ != null)
                {
                    SurfaceZRecFinish?.Invoke();
                }
                
            }
        }
        public float[] SurfaceDataX
        {
            get { return surfaceDataX; }
        }

        public float[] SurfaceDataY
        {
            get { return surfaceDataY; }
        }

        public byte[] SurfaceDataIntensity
        {
            get
            {
                return surfaceDataIntensity;
            }

            set
            {
                surfaceDataIntensity = value;
                if (surfaceDataIntensity != null)
                {
                    SurfaceIntensityRecFinish?.Invoke();
                }
                
            }
        }

        public Profile SigleProfile
        {
            get { return sigleProfile; }
            set
            {
                sigleProfile = value;
                SigleProfileRecFinish?.Invoke();
            }
        }

        public double z_byte_resolution;
        public double zRange { set; get; }
        public double zStart { set; get; }
        public Byte[] SurfaceDataZByte;

        public bool IsRecSurfaceDataZByte { set; get; }

        /// <summary>
        /// 获取轮廓点
        /// </summary>
        public List<Profile> ProfileList
        {
            get
            {
                return profileDataZ;
            }
        }
        #endregion

        public string SaveKdatDirectoy { set; get; }

        public string SaveDatFileDirectory { set; get; }
        public string RunSide { set; get; }

        #region 生成Halcon图像
        public void GenHalconImage(object pointsArr, long width, long height, out HObject image)
        {
            string typeName = pointsArr.GetType().Name;
            string type = "";
            int size = -1;
            switch (typeName)
            {
                case "Double[]":
                    type = "real";
                    size = Marshal.SizeOf(typeof(double));
                    double[] potArr1 = (double[])pointsArr;

                    IntPtr ptImage = Marshal.AllocHGlobal(size * potArr1.Length);
                    Marshal.Copy(potArr1, 0, ptImage, potArr1.Length);
                    HOperatorSet.GenImage1(out image, type, width, height, ptImage);
                    Marshal.FreeHGlobal(ptImage);
                    break;
                case "Byte[]":
                    type = "byte";
                    size = Marshal.SizeOf(typeof(byte));
                    byte[] potArr2 = (byte[])pointsArr;

                    IntPtr ptImage1 = Marshal.AllocHGlobal(size * potArr2.Length);
                    Marshal.Copy(potArr2, 0, ptImage1, potArr2.Length);
                    HOperatorSet.GenImage1(out image, type, width, height, ptImage1);
                    Marshal.FreeHGlobal(ptImage1);
                    break;
                case "Single[]":
                    type = "real";
                    size = Marshal.SizeOf(typeof(float));
                    float[] potArr3 = (float[])pointsArr;

                    IntPtr ptImage3 = Marshal.AllocHGlobal(size * potArr3.Length);
                    Marshal.Copy(potArr3, 0, ptImage3, potArr3.Length);
                    HOperatorSet.GenImage1(out image, type, width, height, ptImage3);
                    Marshal.FreeHGlobal(ptImage3);
                    break;
                default:
                    image = null;
                    break;
            }
        }
        public HObject GenHalconImage(object pointsArr, long width, long height)
        {
            HObject image = null;
            try
            {
                string typeName = pointsArr.GetType().Name;
                string type = "";
                switch (typeName)
                {
                    case "Double[]":
                        type = "real";
                        break;
                    case "Byte[]":
                        type = "byte";
                        break;
                    case "Single[]":
                        type = "real";

                        break;
                    default:
                        //image = null;
                        return image;
                }
                GCHandle hObject = GCHandle.Alloc(pointsArr, GCHandleType.Pinned);
                IntPtr pObject = hObject.AddrOfPinnedObject();
                if (hObject.IsAllocated)
                    hObject.Free();
                image = new HObject();
                HOperatorSet.GenImage1(out image, type, width, height, pObject);
                return image;
            }
            catch
            {
                return image;
            }
        }
        public void ProfileListToArr(List<Profile> profile,float[] arr)
        {
            for (int i = 0; i < profile.Count; i++)
            {
                for (int j = 0; j < profile[i].points.Length; j++)
                {
                    arr[i * profile[i].points.Length + j] = profile[i].points[j].z;
                }
            }
        }

        /// <summary>
        /// 根据编码器数据补缺行数据
        /// </summary>
        /// <param name="ProfileList"></param>
        /// <param name="isUpsideDownImage">图像是否倒置</param>
        /// <param name="encoderResolution">编码器分辨率</param>
        /// <param name="YResolution">图像Y分辨率</param>
        /// <param name="list">补缺后轮廓集合</param>
        public void FillingRow(List<Profile> ProfileList, bool isUpsideDownImage, double encoderResolution, double YResolution, out List<Profile> list)
        {
            list = new List<Profile>();
            int profileCnt = 0;
            int startIndex = 0;
            int endIndex = ProfileList.Count;

            double lastEncoder = isUpsideDownImage ? ProfileList[endIndex - 1].encoder * encoderResolution : ProfileList[startIndex].encoder * encoderResolution;
            for (int i = (isUpsideDownImage ? endIndex - 1 : startIndex); (isUpsideDownImage ? i > startIndex : i < endIndex); i = (isUpsideDownImage ? i - 1 : i + 1))
            {
                double currentEncoder = ProfileList[i].encoder * encoderResolution;
                double subEncoder = Math.Round((currentEncoder - lastEncoder) / YResolution);
                
                for (int j = 0; j < subEncoder - 1; j++)
                {
                    Profile mProfile = new Profile();
                    mProfile = ProfileList[i];
                    //float[] Pots = new float[ProfileList[i].points.Length];
                    for (int k = 0; k < ProfileList[i].points.Length; k++)
                    {
                        float z = 0;byte intensity = new byte();
                        z = (ProfileList[i].points[k].z + ProfileList[i - 1].points[k].z) / 2;
                        intensity =(byte)( (ProfileList[i].points[k].Intensity + ProfileList[i - 1].points[k].Intensity) / 2);                        
                        mProfile.points[k].z = z;
                        mProfile.points[k].Intensity = intensity;
                    }                  
                    list.Add(mProfile);
                }
                list.Add(ProfileList[i]);
                profileCnt++;
                lastEncoder = currentEncoder;
            }
        }


        //public string GenFillingImage(List<ProfileByte> _ProfileByteList, bool isUpsideDownImage, double EncoderResolution, double YResolution, out HObject Image)
        //{
        //    Image = null;
        //    try
        //    {
        //        List<ProfileByte> LAST = new List<ProfileByte>();
        //        FillingRow(_ProfileByteList, isUpsideDownImage, EncoderResolution, YResolution, out LAST);
        //        long Length = LAST.Count;
        //        long width = LAST[0].zByte.Length;
        //        byte[] imageArray = new byte[Length * width];
        //        int k = 0;
        //        for (int i = 0; i < Length; i++)
        //        {
        //            for (int ind = 0; ind < width; ind++)
        //            {
        //                imageArray[k] = LAST[i].zByte[ind];
        //                k++;
        //            }
        //        }
        //        HOperatorSet.GenEmptyObj(out Image);
        //        GenHalconImage(imageArray, width, Length, out Image);
        //        LAST.Clear();
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "GenFillingImage:" + ex.Message;
        //    }
        //}


        #endregion


        public uint profileWidth { set; get; }
        public uint surfaceWidth { set; get; }
        public uint surfaceHeight { set; get; }

        private bool isRecProfileZ;

        public bool EnableProfle = false;
       
        public event Action StampRecFinish;
        public event Action ProfileRecFinish;
        public event Action SurfaceZRecFinish;
        public event Action SurfaceIntensityRecFinish;
        public event Action MeasurementRecFinish;
        public event Action SigleProfileRecFinish;
        
    }

    public struct GoStamp
    {
        public UInt64 frameIndex;
        public UInt64 timestamp;
        public long encoder;
        public UInt64 encoderAtZ;
        public UInt64 reserved;
    }
    public struct Profile
    {
        public ProfilePoint[] points;
        public long encoder;
    }
    [Serializable]
    public struct ProfileByte
    {
        public byte[] zByte;
        public long encoder;
    }
    public struct ProfilePoint
    {
        public float x;
        public float z;
        public byte Intensity;
    }
    public struct MeasurementData
    {
        public ushort id;
        public double value;
    }

    [Serializable]
    public class SurfaceZSaveDat
    {
        public short[] points;
        public PointMsg resolution;
        public PointMsg offset;
        public int width;
        public int height;
    }
    [Serializable]
    public struct PointMsg
    {
        public double x;
        public double y;
        public double z;
    }

    [Serializable]
    public class SurfaceIntensitySaveDat
    {
        public byte[] points;
        public int width;
        public int height;
    }
   
}
