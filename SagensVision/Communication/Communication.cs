using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Net.Sockets;

namespace SagensVision.Communication
{
    public partial class Communication : DevExpress.XtraEditors.XtraForm
    {
        IPControl1 IpSet = new IPControl1();
        TcpTest tcpTest = new TcpTest();
        public delegate void Connect();
        public Connect Conn;
        public Communication()
        {
            InitializeComponent();
            splitContainerControl2.Panel1.Controls.Add(IpSet);
            splitContainerControl2.Panel2.Controls.Add(tcpTest);
            IpSet.Dock = DockStyle.Fill;
            tcpTest.Dock = DockStyle.Fill;
            
            Conn = new Connect(dmmy);
        }
        public void dmmy()
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StaticOperate.WriteXML(MyGlobal.globalConfig, MyGlobal.AllTypePath + "Global.xml");
            string ok = "";
            if (!MyGlobal.sktOK)
            {
                //StaticOperate.CreateServer(ref ok);
                Conn();
            }            
            MessageBox.Show("保存成功！");
        }

        private void Communication_Load(object sender, EventArgs e)
        {
            IpSet.GetData();
            textBox1.Text = MyGlobal.globalConfig.SensorIP;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MyGlobal.globalConfig.IsTcpClient) //客户端
            {
                if (MyGlobal.sktClient == null)
                    return;

                try
                {
                    MyGlobal.sktClient.Disconnect(false);
                    MessageBox.Show("客户端 关闭成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("客户端 关闭失败！\r\n" + ex.StackTrace);
                }
            }
            else
            {
                if (MyGlobal.sktServer == null)
                    return;
                
                try
                {                  
                    MyGlobal.sktServer.Close();
                    MessageBox.Show("服务器 关闭成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("服务器 关闭失败！\r\n" + ex.StackTrace);
                }
            }
                
        }


      
        #region sensor设置
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

        private void simpleButton1_Click(object sender, EventArgs e)
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
                string Error = "下载成功，路径" + MyGlobal.DataPath + "Download";
                MyGlobal.GoSDK.Download3dFile(MyGlobal.DataPath + "Download", ref Error);
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
                string Error = "切换成功！";
                MyGlobal.GoSDK.CutJob(JobName, ref Error);
                MessageBox.Show(Error);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}