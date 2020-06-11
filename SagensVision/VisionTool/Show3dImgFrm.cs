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
    public partial class Show3dImgFrm : Form
    {
        private float[] dz, dx, dy;
        private int w, h;
        public Show3dImgFrm(float[] dz,int w,int h)
        {
            InitializeComponent();
            this.dz = dz;
            this.w = w;
            this.h = h;
            dx = new float[dz.Length];
            dy = new float[dz.Length];
        }

        private void Show3dImgFrm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    dx[i * w + j] = j;
                    dy[i*w + j] = i;
                }
            }
            ClassShow3D cs3d = new ClassShow3D();
            cs3d.Show3D(dx, dy, dz, hWindowControl1.HalconWindow);
        }
    }
}
