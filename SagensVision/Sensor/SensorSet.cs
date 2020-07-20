using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace SagensVision.Sensor
{
    public partial class SensorSet : DevExpress.XtraEditors.XtraForm
    {
        public SensorSet()
        {
            InitializeComponent();
            textBox1.Text = MyGlobal.globalConfig.SensorIP;
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            try
            {
                string ip = textBox1.Text.ToString();
                MyGlobal.globalConfig.SensorIP = ip;
                StaticOperate.WriteXML(MyGlobal.globalConfig, MyGlobal.AllTypePath + "Global.xml");
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                string ip = textBox1.Text.ToString();
                string Error = "连接成功！";
                bool OK = MyGlobal.GoSDK.connect(ip, ref Error);
                

                if (OK)
                {

                    MyGlobal.globalConfig.dataContext.serialNumber = MyGlobal.GoSDK.context.serialNumber;
                    MyGlobal.globalConfig.dataContext.xOffset = MyGlobal.GoSDK.context.xOffset;
                    MyGlobal.globalConfig.dataContext.yOffset = MyGlobal.GoSDK.context.yOffset;
                    MyGlobal.globalConfig.dataContext.zOffset = MyGlobal.GoSDK.context.zOffset;
                    MyGlobal.globalConfig.dataContext.xResolution = MyGlobal.GoSDK.context.xResolution;
                    MyGlobal.globalConfig.dataContext.yResolution = MyGlobal.GoSDK.context.yResolution;
                    MyGlobal.globalConfig.dataContext.zResolution = MyGlobal.GoSDK.context.zResolution;


                    MyGlobal.globalConfig.dataContext.xResolution = MyGlobal.GoSDK.context.xResolution / 1;
                    MyGlobal.globalConfig.dataContext.yResolution = MyGlobal.GoSDK.context.yResolution / 4;
                    StaticOperate.WriteXML(MyGlobal.globalConfig, MyGlobal.AllTypePath + "Global.xml");
                    MyGlobal.SensorConnected = true;
                }
                MessageBox.Show(Error);
            }
            catch (Exception ex)
            {
                MyGlobal.SensorConnected = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string Error = "断开成功！";
                MyGlobal.GoSDK.DisConnect(ref Error);
                MessageBox.Show(Error);
                MyGlobal.SensorConnected = false;
            }
            catch (Exception ex)
            {
                MyGlobal.SensorConnected = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string Error = "打开成功！";
                MyGlobal.GoSDK.Start(ref Error);
                MessageBox.Show(Error);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                string Error = "关闭成功！";
                MyGlobal.GoSDK.Stop(ref Error);
                MessageBox.Show(Error);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                string Error = "下载成功，路径"+ MyGlobal.DataPath + "Download";
                MyGlobal.GoSDK.Download3dFile(MyGlobal.DataPath+"Download",ref Error);
                MessageBox.Show(Error);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                string JobName = textBox2.Text;
                string Error = "切换成功！" ;
                MyGlobal.GoSDK.CutJob(JobName, ref Error);
                MessageBox.Show(Error);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}