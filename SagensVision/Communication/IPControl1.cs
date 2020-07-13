using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SagensVision.Communication
{
    public partial class IPControl1 : UserControl
    {
        public string MotorIpAddress="", SENSOR_IP="";
        public int MotorPort=0;
        private string okmsg = "";

        public string OKmsg
        {
            get {
                return okmsg;
                 }
            set {
                this.textBox1.Text = value;
                okmsg = value;
                }
        }
        public IPControl1()
        {
            InitializeComponent();
          
        }
        bool load = false;
        public void GetData()
        {
            load = true;
            textBox_MotorIp.Text = MyGlobal.globalConfig.MotorIpAddress.ToString();
            textBox_port.Text = MyGlobal.globalConfig.MotorPort.ToString();
            cb_Client.Checked = MyGlobal.globalConfig.IsTcpClient;
            load = false;

        }

        private void IPControl1_Load(object sender, EventArgs e)
        {
            if (MyGlobal.globalConfig.isChinese)
            {
                label1.Text = "OK 发送消息";
                label14.Text = "运动控制IP 地址";                       
            }
            else
            {
                label1.Text = "OK Send Message";
                label14.Text = "Motion control ip address";
            }
        }

        private void cb_Client_CheckedChanged(object sender, EventArgs e)
        {
            MyGlobal.globalConfig.IsTcpClient = cb_Client.Checked;
        }

        private void textBox_MotorIp_TextChanged(object sender, EventArgs e)
        {
            if (load)
            {
                return;
            }
            try
            {
                bool ok = Regex.IsMatch(textBox_port.Text.ToString(), @"^([-]?)\d*$");//是否为整数
                if (!ok)
                {
                    return;
                }
                MotorIpAddress = textBox_MotorIp.Text.ToString();
                MotorPort = Convert.ToInt32(textBox_port.Text.ToString());
                okmsg = textBox1.Text.ToString();
                MyGlobal.globalConfig.MotorIpAddress = MotorIpAddress;
                MyGlobal.globalConfig.MotorPort = MotorPort;
            }
            catch (Exception)
            {
                
            }
            
        }

        
    }
}
