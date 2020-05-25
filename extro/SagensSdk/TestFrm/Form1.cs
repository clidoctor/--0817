using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SagensSdk;
using System.Runtime.InteropServices;

namespace TestFrm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void surfaceRecOk()
        {
            MessageBox.Show("Nice");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //        Private go_api As IntPtr = IntPtr.Zero
            //Private go_system As IntPtr = IntPtr.Zero
            //Private go_sensor As IntPtr = IntPtr.Zero
            //Private go_setup As IntPtr = IntPtr.Zero

            //Dim addr As New address
            //Dim addrPtr As IntPtr = IntPtr.Zero

            GoSdkMaker maker = new GoSdkMaker("127.0.0.1");
            string a = "";
            MessageBox.Show(maker.connect(ref a).ToString());
            //maker.SurfaceIntensityRecFinish += surfaceRecOk;
            string errMsg = "";
            maker.Start(ref errMsg);

            //IntPtr go_api = IntPtr.Zero;
            //IntPtr go_system = IntPtr.Zero;
            //IntPtr go_sensor = IntPtr.Zero;
            //IntPtr go_setuo = IntPtr.Zero;
            //DataContext context = new DataContext();

            //address addr = new address();
            //IntPtr addrPtr = IntPtr.Zero;

            //GoSdkWrapper.GoSdk_Construct(ref go_api);
            //GoSdkWrapper.GoSystem_Construct(ref go_system, IntPtr.Zero);
            //addrPtr = Marshal.AllocHGlobal(Marshal.SizeOf(addr));
            //Marshal.StructureToPtr(addr, addrPtr, false);

            //GoSdkWrapper.kIpAddress_Parse(addrPtr, "127.0.0.1");
            //GoSdkWrapper.GoSystem_FindSensorByIpAddress(go_system, addrPtr, ref go_sensor);
            //kStatus a = GoSdkWrapper.GoSystem_Connect(go_system);
            //GoSdkWrapper.GoSystem_EnableData(go_system, true);
            //GoSdkWrapper.GoSystem_SetDataHandler(go_system, OnData, context);
        }

        public int OnData(DataContext ctx, IntPtr sys, IntPtr data)
        {
            IntPtr dataObj = IntPtr.Zero;
            for (uint i = 0; i < GoSdkWrapper.GoDataSet_Count(data); i++)
            {
                dataObj = GoSdkWrapper.GoDataSet_At(data, i);
                switch (GoSdkWrapper.GoDataMsg_Type(dataObj))
                {
                    case GoDataMessageTypes.GO_DATA_MESSAGE_TYPE_STAMP:
                        //MessageBox.Show(GoDataMessageTypes.GO_DATA_MESSAGE_TYPE_STAMP.ToString());
                        break;
                    case GoDataMessageTypes.GO_DATA_MESSAGE_TYPE_SURFACE:
                        MessageBox.Show(GoDataMessageTypes.GO_DATA_MESSAGE_TYPE_SURFACE.ToString());
                        break;
                    default:
                        break;
                }
            }
            return 1;
        }
    }
}
