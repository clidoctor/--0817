using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChoiceTech.Halcon.Control;
using HalconDotNet;
namespace SagensVision.VisionTool
{

    public partial class Display3D : UserControl
    {
        HWindowControl Window = new HWindowControl();
 
        public Display3D()
        {
            InitializeComponent();
            this.Controls.Add(Window);
            Window.Dock = DockStyle.Fill;
           
            Window.MouseMove += Window_MouseMove;
            Window.MouseWheel += Window_MouseWheel;
        }
        bool breakUp = false;
        public void LoadZRec()
        {
            breakUp = false;
            MyGlobal.GoSDK.SurfaceZRecFinish += GoSDK_SurfaceZRecFinish;
           
        }
        public void ShieldRec()
        {
            breakUp = true;
            MyGlobal.GoSDK.SurfaceZRecFinish -= GoSDK_SurfaceZRecFinish;
            

        }
        private void Window_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }
        ClassShow3D Show3d = new ClassShow3D();
        float[] surfaceX;
        float[] surfaceY;
        float[] surfaceZ;
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button==MouseButtons.Left &&surfaceZ!=null)
                {
                    //Show.breakOut = true;
                }
                else
                {
                   
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void GoSDK_SurfaceZRecFinish()
        {
            if (breakUp)
            {
                return;
            }
            surfaceX = MyGlobal.GoSDK.SurfaceDataX;
            surfaceY = MyGlobal.GoSDK.SurfaceDataY;
            surfaceZ = MyGlobal.GoSDK.SurfaceDataZ;
            //Show3d.breakOut = true;
            Show3d.Show3D(surfaceX, surfaceY, surfaceZ, Window.HalconWindow);
        }

    }
}
