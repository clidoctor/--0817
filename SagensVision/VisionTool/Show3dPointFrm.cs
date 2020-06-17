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
    public partial class Show3dPointFrm : Form
    {
        private double[] recordXCoord; double[] recordYCoord; double[] recordZCoord; string[] recordSigleTitle;
        public Show3dPointFrm(double[] recordXCoord, double[] recordYCoord, double[] recordZCoord, string[] recordSigleTitle)
        {
            InitializeComponent();
            this.recordXCoord = recordXCoord;
            this.recordYCoord = recordYCoord;
            this.recordZCoord = recordZCoord;
            this.recordSigleTitle = recordSigleTitle;
            hWindow_Final1.hWindowControl.HMouseUp += OnHMouseUp4;
        }

        private void Show3dPointFrm_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            ShowProfileToWindow(recordXCoord, recordYCoord, recordZCoord, recordSigleTitle, false);
        }


        public void ShowProfileToWindow(double[] xcoord, double[] ycoord, double[] zcoord, string[] sigleTitle, bool showMsg)
        {
           
            if (recordXCoord == null || recordYCoord == null || recordSigleTitle == null || recordXCoord.Length == 0 || recordYCoord.Length == 0 || recordSigleTitle.Length == 0)
            {
                return;
            }
            HObject regpot;
            HTuple newRecordX = new HTuple(recordXCoord) * 20 - 4000;
            HOperatorSet.GenRegionPoints(out regpot, new HTuple(newRecordX) , new HTuple(recordYCoord) * 20);
          
            HObject ImageConst;
            HOperatorSet.GenImageConst(out ImageConst, "byte", 5000, 5000);
            Action sw = () =>
            {
                hWindow_Final1.HobjectToHimage(ImageConst);
                hWindow_Final1.viewWindow.displayHobject(regpot, "green", true, 20);
            };
            this.Invoke(sw);
            


            if (!showMsg)
            {
                for (int i = 0; i < recordSigleTitle.Length; i++)
                {
                    
                    Action sw1 = () =>
                    {
                        hWindow_Final1.viewWindow.dispMessage($"{recordSigleTitle[i]} +({recordZCoord[i]})", "blue", newRecordX[i], recordYCoord[i] * 20);
                    };
                    this.Invoke(sw1);
                }
            }
            HObject contour;
            newRecordX = newRecordX.TupleConcat(recordXCoord[0] * 20 - 4000);
            HOperatorSet.GenContourPolygonXld(out contour, new HTuple(newRecordX), new HTuple(recordYCoord, recordYCoord[0]) * 20);
            Action sw2 = () =>
            {
                hWindow_Final1.viewWindow.displayHobject(contour, "white");



            };
            this.Invoke(sw2);
            

            contour.Dispose();

            regpot.Dispose();
            ImageConst.Dispose();
        }
        private int MouseClickCnt4 = 0;
        private bool ShowMsg = true; 

        private void OnHMouseUp4(object sender, HMouseEventArgs e)
        {
            MouseClickCnt4++;
            if (MouseClickCnt4 == 2)
            {
                ShowProfileToWindow(null, null, null, null, ShowMsg);
                ShowMsg = !ShowMsg;
                MouseClickCnt4 = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MouseClickCnt4 = 0;
        }
    }
}
