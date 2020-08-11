using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SagensVision.Communication
{
    public partial class TcpTest : UserControl
    {
        public TcpTest()
        {
            InitializeComponent();          

        }
         ~TcpTest()
        {
            timer1.Stop();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = textBox_send.Text.ToString();
                byte[] sendbytes = Encoding.UTF8.GetBytes(msg);
                if (MyGlobal.sktClient==null)
                {
                    MessageBox.Show("未连接!");
                    return;
                }
                MyGlobal.sktClient.Send(sendbytes);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox_Receive.Text = MyGlobal.ReceiveMsg;
        }

        private void TcpTest_Load(object sender, EventArgs e)
        {
            if (MyGlobal.globalConfig.isChinese)
            {
                label14.Text = "数据接收";
                label15.Text = "数据发送";
                button8.Text = "发送";
            }
            else
            {
                label14.Text = "Data Receive";
                label15.Text = "Data Send";
                button8.Text = "Send";
            }
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                timer1.Start();
                timer1.Interval = 100;
            }
            else
            {
                timer1.Stop();
            }
        }
    }
}
