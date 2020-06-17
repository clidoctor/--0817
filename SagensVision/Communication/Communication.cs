using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

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
        }
    }
}