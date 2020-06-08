using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GoVision3D.SaveLoad;

namespace SagensSdk
{
    public class Constants
    {
        public const string GOSDKDLLPATH = "GoSdk.dll";
        public const string KAPIDLLPATH = "kApi.dll";
    }
    public class GoSdkWrapper
    {
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSdk_Construct")]
        public static extern  kStatus GoSdk_Construct(ref IntPtr assembly);
        [DllImport(Constants.KAPIDLLPATH, EntryPoint = "kApiLib_Construct")]
        public static extern kStatus KApi_Construct(ref IntPtr assembly);
        [DllImport(Constants.KAPIDLLPATH, EntryPoint = "kIpAddress_Parse")]
        public static extern kStatus kIpAddress_Parse(IntPtr addressPointer, string text);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSystem_Construct")]
        public static extern kStatus GoSystem_Construct(ref IntPtr system, IntPtr allocator);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSystem_FindSensorByIpAddress")]
        public static extern kStatus GoSystem_FindSensorByIpAddress(IntPtr system, IntPtr addressPointer, ref IntPtr sensor);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSystem_Connect")]
        public static extern kStatus GoSystem_Connect(IntPtr system);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSystem_Disconnect")]
        public static extern kStatus GoSystem_Disconnect(IntPtr system);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSensor_IsConnected")]
        public static extern bool GoSensor_IsConnected(IntPtr sensor);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSystem_EnableData")]
        public static extern kStatus GoSystem_EnableData(IntPtr system, bool enable);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSystem_SetDataHandler")]
        public static extern kStatus GoSystem_SetDataHandler(IntPtr system, onDataType callback, DataContext context);
      
        
          
        #region 传感器操作
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSystem_Start")]
        public static extern kStatus GoSystem_Start(IntPtr system);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSystem_Stop")]
        public static extern kStatus GoSystem_Stop(IntPtr system);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSensor_CopyFile")]
        public static extern kStatus GoSensor_CopyFile(IntPtr sensor, string sourceName, string destName);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSensor_DownloadFile")]
        public static extern kStatus GoSensor_DownloadFile(IntPtr sensor, string sourceName, string destPath);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSensor_Id")]
        public static extern UInt32 GoSensor_Id(IntPtr sensor);

        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSensor_Setup")]
        public static extern IntPtr GoSensor_Setup(IntPtr sensor);

        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSetup_SpacingIntervalSystemValue")]
        public static extern double GoSetup_SpacingIntervalSystemValue(IntPtr setup, GoRole role);

        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSetup_EncoderSpacing")]
        public static extern  double  GoSetup_EncoderSpacing(IntPtr setup);
        

        #region Destroy
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoDestroy")]
        public static extern kStatus GoDestroy(IntPtr obj);  

        #endregion


        #endregion
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoDataSet_Count")]
        public static extern int GoDataSet_Count(IntPtr data);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoDataSet_At")]
        public static extern IntPtr GoDataSet_At(IntPtr dataset,uint index);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoDataMsg_Type")]
        public static extern GoDataMessageTypes GoDataMsg_Type(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoStampMsg_At")]
        public static extern IntPtr GoStampMsg_At(IntPtr msg, uint index);
        #region 轮廓高度

        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoResampledProfileMsg_At")]
        public static extern IntPtr GoResampledProfileMsg_At(IntPtr msg, UInt32 index);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoProfileMsg_Width")]
        public static extern UInt32 GoProfileMsg_Width(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoProfileMsg_XResolution")]
        public static extern UInt32 GoProfileMsg_XResolution(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoProfileMsg_ZResolution")]
        public static extern UInt32 GoProfileMsg_ZResolution(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoProfileMsg_XOffset")]
        public static extern Int32 GoProfileMsg_XOffset(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoProfileMsg_ZOffset")]
        public static extern Int32 GoProfileMsg_ZOffset(IntPtr msg);
        #endregion
        #region 轮廓亮度
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoProfileIntensityMsg_At")]
        public static extern IntPtr GoProfileIntensityMsg_At(IntPtr msg, UInt32 index);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoProfileIntensityMsg_Width")]
        public static extern UInt32 GoProfileIntensityMsg_Width(IntPtr msg);
        #endregion

        #region 点云高度
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceMsg_XResolution")]
        public static extern UInt32 GoSurfaceMsg_XResolution(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceMsg_ZResolution")]
        public static extern UInt32 GoSurfaceMsg_ZResolution(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceMsg_YResolution")]
        public static extern UInt32 GoSurfaceMsg_YResolution(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceMsg_XOffset")]
        public static extern Int32 GoSurfaceMsg_XOffset(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceMsg_ZOffset")]
        public static extern Int32 GoSurfaceMsg_ZOffset(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceMsg_YOffset")]
        public static extern Int32 GoSurfaceMsg_YOffset(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceMsg_Width")]
        public static extern UInt32 GoSurfaceMsg_Width(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceMsg_Length")]
        public static extern UInt32 GoSurfaceMsg_Length(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceMsg_RowAt")]
        public static extern IntPtr GoSurfaceMsg_RowAt(IntPtr msg,int idx);
        #endregion
        #region 点云亮度
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceIntensityMsg_XResolution")]
        public static extern UInt32 GoSurfaceIntensityMsg_XResolution(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceMsg_YResolution")]
        public static extern UInt32 GoSurfaceIntensityMsg_YResolution(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceIntensityMsg_XOffset")]
        public static extern Int32 GoSurfaceIntensityMsg_XOffset(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceIntensityMsg_YOffset")]
        public static extern Int32 GoSurfaceIntensityMsg_YOffset(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceIntensityMsg_Width")]
        public static extern UInt32 GoSurfaceIntensityMsg_Width(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceIntensityMsg_Length")]
        public static extern UInt32 GoSurfaceIntensityMsg_Length(IntPtr msg);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSurfaceIntensityMsg_RowAt")]
        public static extern IntPtr GoSurfaceIntensityMsg_RowAt(IntPtr msg, int idx);
        #endregion



        #region 测量值
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoMeasurementMsg_At")]
        public static extern IntPtr GoMeasurementMsg_At(IntPtr msg, int idx);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoMeasurementMsg_Id")]
        public static extern ushort GoMeasurementMsg_Id(IntPtr msg);
        #endregion
        [DllImport("GoFileLib.dll", EntryPoint = "SaveSufaceToFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SaveSufaceToFile(string filepath,ref GoSurface insurface,bool flag);

        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSetup_ActiveAreaHeight")]
        public static extern double GoSetup_ActiveAreaHeight(IntPtr setup, GoRole role);
        [DllImport(Constants.GOSDKDLLPATH,EntryPoint = "GoSetup_ActiveAreaZ")]
        public static extern double GoSetup_ActiveAreaZ(IntPtr setup, GoRole role);

        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSensor_Role")]
        public static extern IntPtr GoSensor_Role(IntPtr sensor);
        [DllImport(Constants.GOSDKDLLPATH, EntryPoint = "GoSensor_Encoder")]
        public static extern kStatus GoSensor_Encoder(IntPtr sensor,ref long encoder);
    }
    public delegate int onDataType(DataContext ctx, IntPtr sys, IntPtr data);
    public class DataContext
    {
        public double xResolution;
        public double zResolution;
        public double yResolution;
        public double xOffset;
        public double zOffset;
        public double yOffset;
        public uint serialNumber;
    }
    public struct address
    {
        public char version;
        [MarshalAs(UnmanagedType.ByValArray,SizeConst =16)]
        public byte[] IPaddress;
    }
    public enum GoDataMessageTypes
    {
        GO_DATA_MESSAGE_TYPE_UNKNOWN = -1,
        GO_DATA_MESSAGE_TYPE_STAMP = 0,
        GO_DATA_MESSAGE_TYPE_HEALTH = 1,
        GO_DATA_MESSAGE_TYPE_VIDEO = 2,
        GO_DATA_MESSAGE_TYPE_RANGE = 3,
        GO_DATA_MESSAGE_TYPE_RANGE_INTENSITY = 4,
        GO_DATA_MESSAGE_TYPE_PROFILE = 5,
        GO_DATA_MESSAGE_TYPE_PROFILE_INTENSITY = 6,
        GO_DATA_MESSAGE_TYPE_RESAMPLED_PROFILE = 7,
        GO_DATA_MESSAGE_TYPE_SURFACE = 8,
        GO_DATA_MESSAGE_TYPE_SURFACE_INTENSITY = 9,
        GO_DATA_MESSAGE_TYPE_MEASUREMENT = 10,
        GO_DATA_MESSAGE_TYPE_ALIGNMENT = 11,
        GO_DATA_MESSAGE_TYPE_EXPOSURE_CAL = 12
    }

    public enum GoRole
    {
        GO_ROLE_MAIN = 0,
        GO_ROLE_BUDDY = 1,
    }

    public enum kStatus
    {
        kERROR_STATE = -1000,                                               // Invalid state.
        kERROR_NOT_FOUND = -999,                                            // Item is not found.
        kERROR_COMMAND = -998,                                              // Command not recognized.
        kERROR_PARAMETER = -997,                                            // Parameter is invalid.
        kERROR_UNIMPLEMENTED = -996,                                        // Feature not implemented.
        kERROR_HANDLE = -995,                                               // Handle is invalid.
        kERROR_MEMORY = -994,                                               // Out of memory.
        kERROR_TIMEOUT = -993,                                              // Action timed out.
        kERROR_INCOMPLETE = -992,                                           // Buffer not large enough for data.
        kERROR_STREAM = -991,                                               // Error in stream.
        kERROR_CLOSED = -990,                                               // Resource is no longer avaiable. 
        kERROR_VERSION = -989,                                              // Invalid version number.
        kERROR_ABORT = -988,                                                // Operation aborted.
        kERROR_ALREADY_EXISTS = -987,                                       // Conflicts with existing item.
        kERROR_NETWORK = -986,                                              // Network setup/resource error.
        kERROR_HEAP = -985,                                                 // Heap error (leak/double-free).
        kERROR_FORMAT = -984,                                               // Data parsing/formatting error. 
        kERROR_READ_ONLY = -983,                                            // Object is read-only (cannot be written).
        kERROR_WRITE_ONLY = -982,                                           // Object is write-only (cannot be read). 
        kERROR_BUSY = -981,                                                 // Agent is busy (cannot service request).
        kERROR_CONFLICT = -980,                                             // State conflicts with another object.
        kERROR_OS = -979,                                                   // Generic error reported by underlying OS.
        kERROR_DEVICE = -978,                                               // Hardware device error.
        kERROR_FULL = -977,                                                 // Resource is already fully utilized.
        kERROR_IN_PROGRESS = -976,                                          // Operation is in progress, but not yet complete.
        kERROR = 0,                                                         // General error. 
        kOK = 1                                                             // Operation successful. 
    }


    public struct GoMeasurementData
    {
        public double Value;
    }

   
}
