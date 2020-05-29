﻿using ChoiceTech.Halcon.Control;
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

            hWindow_Final1.HobjectToHimage(img);
            if (!MyGlobal.isShowHeightImg)
            {
                hWindow_Final1.hWindowControl.HMouseUp += OnHMouseUp;
            }
        }

        private void OnHMouseUp(object sender, HMouseEventArgs e)
        {
            HObject reg;
            HOperatorSet.Threshold(MyGlobal.ImageMulti[MyGlobal.ImageMulti.Count - 1][1], out reg, -20, 50);
            HTuple per, min, max, range;
            HOperatorSet.MinMaxGray(reg, MyGlobal.ImageMulti[MyGlobal.ImageMulti.Count - 1][1],0, out min, out max, out range);
            double z_byte = MyGlobal.GoSDK.z_byte_resolution == 0 ? ((int)255/ MyGlobal.globalConfig.zRange ): MyGlobal.GoSDK.z_byte_resolution;
            double z_start = MyGlobal.GoSDK.zStart == 0 ? MyGlobal.globalConfig.zStart : MyGlobal.GoSDK.zStart;
            byte[] grayArr = new byte[5];
            grayArr[0] = (byte)Math.Ceiling(((double)min + (double)(range / 5) - z_start) * z_byte);
            grayArr[1] = (byte)Math.Ceiling(((double)min + (double)(range / 4) - z_start) * z_byte);
            grayArr[2] = (byte)Math.Ceiling(((double)min + (double)(range / 4 * 2) - z_start) * z_byte);
            grayArr[3] = (byte)Math.Ceiling(((double)min + (double)(range / 4 * 3) - z_start) * z_byte);
            grayArr[4] = (byte)Math.Ceiling(((double)max - (double)(range / 5) - z_start) * z_byte);
            PseudoColor.markColor(pictureBox1, pictureBox2, grayArr, z_byte ,z_start);
            reg.Dispose();
            hWindow_Final1.hWindowControl.HMouseUp -= OnHMouseUp;
        }

        private void EnlargeFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox1.Image = null;
        }
    }
}
