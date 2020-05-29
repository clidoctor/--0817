using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using HalconDotNet;

namespace Calibration
{
    public class CLGlobal
    {
        public  HTuple metrologyHandle;      
        public  List<int> ToolIndexer;
        public static  RichTextBox MsgBox;
       
        public  CLGlobal()
        {
            ToolIndexer = new List<int>();
            for (int i = 1; i < 100; i++)
            {
                ToolIndexer.Add(i);
            }
        }
        

        public  string Selectpath()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "所有文件(*.*)|*.*||";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(ofd.FileName))
                {
                    return "";
                }
            }
            return ofd.FileName;
        }
        public static  void DispMsg(string msg)
        {
            if (MsgBox == null)
            {
                return;
            }
            MsgBox.Text = DateTime.Now.ToLongTimeString().ToString() + "  " + msg;
        }

        #region 圆心显示
        public  bool IsShowCircle { set; get; }
        private  string circleCenterX;
        public  string CircleCenterX
        {
            get
            {
                return circleCenterX;
            }

            set
            {
                circleCenterX = value;
                ShowXval?.Invoke(value);
            }
        }
        public  event Action<string> ShowXval;
        private  string circleCenterY;
        public  string CircleCenterY
        {
            get
            {
                return circleCenterY;
            }

            set
            {
                circleCenterY = value;
                ShowYval?.Invoke(value);
            }
        }
        public  event Action<string> ShowYval;
        private  string circleRadius;
        public  string CircleRadius
        {
            get
            {
                return circleRadius;
            }
            set
            {
                circleRadius = value;
                ShowRadiusval?.Invoke(value);
            }
        }
        public  event Action<string> ShowRadiusval;

        #endregion
    }
}
