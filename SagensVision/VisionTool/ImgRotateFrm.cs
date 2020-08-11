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
        public ImgRotateFrm(bool _isRight)
        {
            InitializeComponent();
            isRight = _isRight;
        }

        bool isRight = false;
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (isRight)
            {
                MyGlobal.globalPointSet_Right.imgRotateArr[0] = (int)l1.Value;
                MyGlobal.globalPointSet_Right.imgRotateArr[1] = (int)t2.Value;
                MyGlobal.globalPointSet_Right.imgRotateArr[2] = (int)r3.Value;
                MyGlobal.globalPointSet_Right.imgRotateArr[3] = (int)d4.Value;

            }
            else
            {
                MyGlobal.globalPointSet_Left.imgRotateArr[0] = (int)l1.Value;
                MyGlobal.globalPointSet_Left.imgRotateArr[1] = (int)t2.Value;
                MyGlobal.globalPointSet_Left.imgRotateArr[2] = (int)r3.Value;
                MyGlobal.globalPointSet_Left.imgRotateArr[3] = (int)d4.Value;
            }
            try
            {
                if (isRight)
                {
                    StaticOperate.WriteXML(MyGlobal.globalPointSet_Right, MyGlobal.AllTypePath + "GlobalPoint_Right.xml");
                }
                else
                {
                    StaticOperate.WriteXML(MyGlobal.globalPointSet_Left, MyGlobal.AllTypePath + "GlobalPoint_Left.xml");
                }
               
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败-->" + ex.Message);
            }
            
        }

        private void ImgRotateFrm_Load(object sender, EventArgs e)
        {
            if (isRight)
            {
                l1.Value = MyGlobal.globalPointSet_Right.imgRotateArr[0];
                t2.Value = MyGlobal.globalPointSet_Right.imgRotateArr[1];
                r3.Value = MyGlobal.globalPointSet_Right.imgRotateArr[2];
                d4.Value = MyGlobal.globalPointSet_Right.imgRotateArr[3];
                this.Text = "右工位旋转设置";
            }
            else
            {
                l1.Value = MyGlobal.globalPointSet_Left.imgRotateArr[0];
                t2.Value = MyGlobal.globalPointSet_Left.imgRotateArr[1];
                r3.Value = MyGlobal.globalPointSet_Left.imgRotateArr[2];
                d4.Value = MyGlobal.globalPointSet_Left.imgRotateArr[3];
                this.Text = "左工位旋转设置";
            }

        }
    }

 
}
