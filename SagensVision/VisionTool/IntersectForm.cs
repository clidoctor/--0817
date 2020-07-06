using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using HalconDotNet;
using ChoiceTech.Halcon.Control;
using System.IO;
namespace SagensVision.VisionTool
{
    public partial class IntersectForm : DevExpress.XtraEditors.XtraForm
    {
        public event Action<string> GenDataSource;
        public List<HTuple> Lines_V = new List<HTuple>();
        public List<HTuple> Lines_H = new List<HTuple>();
        HWindow_Final hwindow_final = new HWindow_Final();
        HTuple HomMat2Dxy = new HTuple();
        bool isRight = false;
        public IntersectForm()
        {
            InitializeComponent();
            CalibFormMain.ChangeSide += CalibFormMain_ChangeSide;
            CalibFormMain.ChangeStation += CalibFormMain_ChangeStation;
        }

        private void CalibFormMain_ChangeStation(bool obj)
        {
            isRight = obj;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            try
            {
                if (MessageBox.Show("是否标定Side" + (SideID + 1).ToString(), "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                HTuple row, col;
                string ok = RunIntersection(out row, out col);
                if (ok != "OK")
                {
                    label_result.Text = ok;
                    label_result.BackColor = Color.Red;
                    return;
                }

                StringBuilder PixStr = new StringBuilder();
                dataGridView1.Rows.Clear();
                dataGridView1.AllowUserToAddRows = false;
                if (MyGlobal.globalPointSet_Left.imgRotateArr[SideID]==180)
                {
                    row = row.TupleInverse();
                    col = col.TupleInverse();
                }
               

                for (int i = 0; i < row.Length; i++)
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = i + 1;
                    dataGridView1.Rows[i].Cells[1].Value = Math.Round(col[i].D, 3);
                    dataGridView1.Rows[i].Cells[2].Value = Math.Round(row[i].D, 3);
                    hwindow_final.viewWindow.dispMessage("P" + (i + 1).ToString(), "green", row[i].D, col[i].D);
                    string data = Math.Round(row[i].D, 3).ToString() + "," + Math.Round(col[i].D, 3).ToString() + "\r\n";
                    PixStr.Append(data);
                }
                HObject cross = new HObject();
                HOperatorSet.GenCrossContourXld(out cross, row, col, 15, 0.5);
                hwindow_final.viewWindow.displayHobject(cross, "red", false, 3);

                //计算标定
                if (row.Length < 3)
                {
                    MessageBox.Show(string.Format("当前标定像素点数{0}太少", row.Length));
                    return;
                }

                if (AxisX == null || AxisX.Length == 0)
                {
                    return;
                }

                ParamPath.ToolType = MyGlobal.ToolType_Calib;
                ParamPath.ParaName = MyGlobal.FindPointType_FitLineSet + "_" + "Side" + (SideID + 1).ToString();
                ParamPath.IsRight = isRight;

                if (File.Exists(ParamPath.Path_CalibPix))
                {
                    File.Delete(ParamPath.Path_CalibPix);
                }
                ParamPath.WriteTxt(ParamPath.Path_CalibPix, PixStr.ToString());

                HOperatorSet.VectorToHomMat2d(row, col, AxisX, AxisY, out HomMat2Dxy);
                string Deviation = "";
                RunCalibDeviation(row, col, out Deviation);
                label_Score.Text = Deviation;
                HOperatorSet.WriteTuple(HomMat2Dxy, ParamPath.Path_tup);
                label_result.Text = "OK";
                label_result.BackColor = Color.Green;
            }
            catch (Exception ex)
            {

                MessageBox.Show("标定失败" + ex.Message);
            }

           
        }

        string RunCalibDeviation(double[] Pix_X,double[] Pix_Y,out string Deviation)
        {
            Deviation = "";
            try
            {              
                HTuple real_x = new HTuple();
                HTuple real_y = new HTuple();
                HTuple real_z = new HTuple();
                HTuple tx1 = 0;

                HOperatorSet.AffineTransPoint2d(HomMat2Dxy, Pix_X, Pix_Y, out real_x, out real_y);

                HTuple subx = AxisX - real_x;
                HTuple suby = AxisY - real_y;
             
                subx = subx.TupleAbs();
                suby = suby.TupleAbs();
                
                double max_x = subx.TupleMax();
                double max_y = suby.TupleMax();

                double min_x = subx.TupleMin();
                double min_y = suby.TupleMin();

                double Meanx = subx.TupleMean();
                double Meany = suby.TupleMean();

                max_x = Math.Round(max_x, 3);
                max_y = Math.Round(max_y, 3);
                min_x = Math.Round(min_x, 3);
                min_y = Math.Round(min_y, 3);
                Meanx = Math.Round(Meanx, 3);
                Meany = Math.Round(Meany, 3);

                Deviation = string.Format("最大误差x：{0};y: {1};\r\n最小误差x：{2};y: {3};\r\n平均误差x：{4};y: {5};", max_x.ToString(), max_y.ToString(), min_x.ToString(), min_y.ToString(), Meanx.ToString(), Meany.ToString());
                return "OK";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return ex.Message;
            }
        }

        string RunIntersection(out HTuple row, out HTuple col)
        {
            row = new HTuple(); col = new HTuple();
            try
            {
                //取 水平和竖直直线求交点
                int Vcount = Lines_V.Count;
                int Hcount = Lines_H.Count;
                if (Vcount == 0)
                {
                    return "至少需要包含一条水平方向直线";
                }
                if (Hcount == 0)
                {
                    return "至少需要包含一条竖直方向直线";
                }



                for (int i = 0; i < Hcount; i++)
                {
                    for (int j = 0; j < Vcount; j++)
                    {
                        HTuple Row, Col, isOver;
                        HOperatorSet.IntersectionLines(Lines_V[j][0], Lines_V[j][1], Lines_V[j][2], Lines_V[j][3],
                            Lines_H[i][0], Lines_H[i][1], Lines_H[i][2], Lines_H[i][3], out Row, out Col, out isOver);
                        row = row.TupleConcat(Row);
                        col = col.TupleConcat(Col);
                    }
                }

                return "OK";
            }
            catch (Exception ex)
            {
                string a = ex.StackTrace;
                int ind = a.IndexOf("行号");
                int start = ind;
                string RowNum = ind != -1 ? "--" + a.Substring(start, a.Length - start) : "";
                return "RunIntersection Error :" + ex.Message + RowNum;
            }
        }
        private void cb_lines1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_lines1.SelectedItem == null)
            {
                return;
            }
            if (cb_lines1.SelectedItem.ToString() == "Lines_V")
            {
                GenDataSource?.Invoke("Lines_V");
            }
            else
            {
                GenDataSource?.Invoke("Lines_H");
            }
        }

        private void cb_lines2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_lines2.SelectedItem == null)
            {
                return;
            }
            if (cb_lines2.SelectedItem.ToString() == "Lines_V")
            {
                GenDataSource?.Invoke("Lines_V");
            }
            else
            {
                GenDataSource?.Invoke("Lines_H");
            }
        }

        private void IntersectForm_Load(object sender, EventArgs e)
        {
            splitContainer1.Panel1.Controls.Add(hwindow_final);
            hwindow_final.Dock = DockStyle.Fill;
        }
        int SideID = -1;
        private void CalibFormMain_ChangeSide(int SideId,HObject arg1, HObject arg2, HObject arg3)
        {
            if (SideId!=-1)
            {
                SideID = SideId;
            }
            if (arg1 != null)
            {
                hwindow_final.HobjectToHimage(arg1);
            }
        }
        double[] AxisX; double[] AxisY;
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string ok = ReadAxisPoints(SideID,out AxisX, out AxisY);
            if (ok!="OK")
            {
                MessageBox.Show(ok);
                return;
            }
            dataGridView2.Rows.Clear();
            dataGridView2.AllowUserToAddRows = false;
            for (int i = 0; i < AxisX.Length; i++)
            {
                dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Cells[0].Value = i + 1;
                dataGridView2.Rows[i].Cells[1].Value = AxisX[i];
                dataGridView2.Rows[i].Cells[2].Value = AxisY[i];
            }
            MessageBox.Show("加载成功!");
        }

        string ReadAxisPoints(int SideID, out double[] AxisX,out double[] AxisY)
        {
            AxisX = null; AxisY = null;
            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + $"Axis{(SideID + 1).ToString()}.txt"))
                {
                    return "机械坐标文件未找到";
                }
                List<string> AxisList = File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + $"Axis{(SideID + 1).ToString()}.txt").ToList();
                if (AxisList.Count==0)
                {
                    return "机械坐标数量为0";
                }
                AxisX = new double[AxisList.Count];AxisY = new double[AxisList.Count];
                for (int i = 0; i < AxisList.Count; i++)
                {
                    if (AxisList[i] == "")
                    {
                        continue;
                    }
                    string[] txtArray = AxisList[i].Split(',');
                    if (txtArray.Length<2)
                    {
                        return "格式不正确";
                    }
                    AxisX[i] = Convert.ToDouble(txtArray[0]);
                    AxisY[i] = Convert.ToDouble(txtArray[1]);
                }
                
                return "OK";
            }
            catch (Exception ex)
            {

                return "ReadAxisPoints error " + ex.Message;
            }
            
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                double virx = (double)vir_x.Value;
                double viry = (double)vir_y.Value;
              
                if (HomMat2Dxy.Type != HTupleType.EMPTY)
                {
                    HTuple realx = 0, realy = 0, realz = 0;
                    HOperatorSet.AffineTransPoint2d(HomMat2Dxy, virx, viry, out realx, out realy);
                    HTuple tx = 0;
                    HTuple real_x1, real_y1;
                    if (HomMat2Dxy.Length > 0)
                    {
                        HOperatorSet.AffineTransPoint2d(HomMat2Dxy, virx, viry, out real_x1, out real_y1);
                    }
                    Realx.Value = (decimal)realx.D;
                    Realy.Value = (decimal)realy.D;
                  
                }      
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}