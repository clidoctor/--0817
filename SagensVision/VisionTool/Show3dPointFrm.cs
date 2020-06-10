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
    public partial class Show3dPointFrm : Form
    {
        double[] x, y, z;
        public Show3dPointFrm(double[] x,double[] y,double[] z)
        {
            InitializeComponent();
            this.x = x;
            this.y = y;
            this.z = z;
        }

        private void Show3dPointFrm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            ClassShow3D cs3d = new ClassShow3D();
            float[] dx = new float[x.Length];
            float[] dy = new float[y.Length];
            float[] dz = new float[z.Length];
            for (int i = 0; i < dx.Length; i++)
            {
                dx[i] = Convert.ToSingle((x[i]-200).ToString());
                dy[i] = Convert.ToSingle(y[i].ToString());
                dz[i] = Convert.ToSingle(z[i].ToString());
            }
            cs3d.Show3D(dx, dy, dz, hWindowControl1.HalconWindow);
        }
    }
}
