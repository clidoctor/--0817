using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SagensVision.VisionTool
{
    public partial class ImgRotateFrm : Form
    {
        public ImgRotateFrm()
        {
            InitializeComponent();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            MyGlobal.globalConfig.imgRotateArr[0] = (int)l1.Value;
            MyGlobal.globalConfig.imgRotateArr[1] = (int)t2.Value;
            MyGlobal.globalConfig.imgRotateArr[2] = (int)r3.Value;
            MyGlobal.globalConfig.imgRotateArr[3] = (int)d4.Value;
            try
            {
                StaticOperate.WriteXML(MyGlobal.globalConfig, MyGlobal.ConfigPath + "Global.xml");
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败-->" + ex.Message);
            }
            
        }

        private void ImgRotateFrm_Load(object sender, EventArgs e)
        {
            
            l1.Value = MyGlobal.globalConfig.imgRotateArr[0];
            t2.Value = MyGlobal.globalConfig.imgRotateArr[1];
            r3.Value = MyGlobal.globalConfig.imgRotateArr[2];
            d4.Value = MyGlobal.globalConfig.imgRotateArr[3];

        }
    }

 
}
