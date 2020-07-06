using ChoiceTech.Halcon.Control;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SagensVision.VisionTool
{
    public partial class EnlargeFrm : Form
    {
        HObject img;
        int idx;
        public EnlargeFrm(HObject img,int idx)
        {
            InitializeComponent();
            this.img = img;
            this.idx = idx;
        }

        private void EnlargeFrm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            this.MinimizeBox = false;
            hWindow_Final1.HobjectToHimage(img);
            if (FormMain.Yorigin.Count > (idx) && FormMain.Yorigin[idx].Length > 0)
            {
                HObject cross;
                HOperatorSet.GenCrossContourXld(out cross, new HTuple(FormMain.Yorigin[idx],FormMain.AnchorList[idx].Row), new HTuple(FormMain.Xorigin[idx], FormMain.AnchorList[idx].Col), 26, 1.5);
                hWindow_Final1.viewWindow.displayHobject(cross,"green");
                cross.Dispose();
                if (FormMain.Yorigin.Count!= FormMain.NameOrigin.Count)
                {
                    return;
                }
                for (int j = 0; j < FormMain.NameOrigin[idx].Length; j++)
                {
                    if (FormMain.NameOrigin[idx][j] ==null)
                    {
                        break;
                    }
                    hWindow_Final1.viewWindow.dispMessage($"{FormMain.NameOrigin[idx][j]}({Math.Round( FormMain.Zorigin[idx][j],3)})", "blue", FormMain.Yorigin[idx][j], FormMain.Xorigin[idx][j]);
                }
                if (FormMain.AnchorList[idx].Row !=0 || FormMain.AnchorList[idx].Col != 0)
                {
                    hWindow_Final1.viewWindow.dispMessage("Y:" + (FormMain.AnchorList[idx].Row * MyGlobal.globalConfig.dataContext.yResolution).ToString(), "red", FormMain.AnchorList[idx].Row, FormMain.AnchorList[idx].Col);
                    hWindow_Final1.viewWindow.dispMessage("X:" + (FormMain.AnchorList[idx].Col * MyGlobal.globalConfig.dataContext.xResolution).ToString(), "red", FormMain.AnchorList[idx].Row + 60, FormMain.AnchorList[idx].Col);
                    hWindow_Final1.viewWindow.dispMessage("Rad:" + FormMain.AnchorList[idx].Angle.ToString(), "red", FormMain.AnchorList[idx].Row + 120, FormMain.AnchorList[idx].Col);

                }

            }

            if (!MyGlobal.isShowHeightImg)
            {
                hWindow_Final1.hWindowControl.HMouseUp += OnHMouseUp;
                hWindow_Final1.hWindowControl.HMouseWheel += OnHMouseUp;
            }
            else
            {
                hWindow_Final1.hWindowControl.HMouseUp -= OnHMouseUp;
                hWindow_Final1.hWindowControl.HMouseWheel -= OnHMouseUp;
            }
        }

        private void OnHMouseUp(object sender, HMouseEventArgs e)
        {
            double z_byte = MyGlobal.GoSDK.z_byte_resolution == 0 ? ((int)255/ MyGlobal.globalConfig.zRange ): MyGlobal.GoSDK.z_byte_resolution;
            double z_start = MyGlobal.GoSDK.zStart == 0 ? MyGlobal.globalConfig.zStart : MyGlobal.GoSDK.zStart;
            HObject reg;
            HOperatorSet.Threshold(MyGlobal.ImageMulti[idx][1], out reg, z_start, 50);
            HTuple per, min, max, range;
            HOperatorSet.MinMaxGray(reg, MyGlobal.ImageMulti[idx][1],0, out min, out max, out range);
            byte[] grayArr = new byte[5];
            grayArr[0] = (byte)Math.Ceiling(((double)min + (double)(range / 5) - z_start) * z_byte);
            grayArr[1] = (byte)Math.Ceiling(((double)min + (double)(range / 4) - z_start) * z_byte);
            grayArr[2] = (byte)Math.Ceiling(((double)min + (double)(range / 4 * 2) - z_start) * z_byte);
            grayArr[3] = (byte)Math.Ceiling(((double)min + (double)(range / 4 * 3) - z_start) * z_byte);
            grayArr[4] = (byte)Math.Ceiling(((double)max - (double)(range / 5) - z_start) * z_byte);
            PseudoColor.markColor(pictureBox1, pictureBox2, grayArr, z_byte ,z_start);
            reg.Dispose();
            hWindow_Final1.hWindowControl.HMouseUp -= OnHMouseUp;
            hWindow_Final1.hWindowControl.HMouseWheel -= OnHMouseUp;
        }

        private void EnlargeFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void EnlargeFrm_SizeChanged(object sender, EventArgs e)
        {
            hWindow_Final1.hWindowControl.HMouseUp += OnHMouseUp;
            hWindow_Final1.hWindowControl.HMouseWheel += OnHMouseUp;
        }

     
    }
}
