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

        public int[] arr;
        private void btn_save_Click(object sender, EventArgs e)
        {
            arr[0] = (int)l1.Value;
            arr[1] = (int)t2.Value;
            arr[2] = (int)r3.Value;
            arr[3] = (int)d4.Value;
            try
            {
                StaticOperate.WriteXML(arr, MyGlobal.imgRotatePath);
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败-->" + ex.Message);
            }
            
        }

        private void ImgRotateFrm_Load(object sender, EventArgs e)
        {
            if (File.Exists(MyGlobal.imgRotatePath))
            {
                arr = (int[])StaticOperate.ReadXML(MyGlobal.imgRotatePath, typeof(int[]));
            }
            else
            {
                arr = new int[4] { 0, 0, 0, 0 };
                StaticOperate.WriteXML(arr, MyGlobal.imgRotatePath);
            }

            l1.Value = arr[0];
            t2.Value = arr[1];
            r3.Value = arr[2];
            d4.Value = arr[3];

        }
    }

 
}
