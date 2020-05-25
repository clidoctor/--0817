using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace SagensVision
{
    public partial class Frm_sql : Form
    {
        string tabelName = "";
        //private static string dbPath = AppDomain.CurrentDomain.BaseDirectory + "nba.db3";
        string namestart = "";
        string namestartid = "";
        double _Total, _NgNum, _Percent;
        public string dbPath = AppDomain.CurrentDomain.BaseDirectory + "Data\\" + "glue.db3";
        public Frm_sql(double Total, double NgNum,double Percent)
        {
            _Total = Total;
            _NgNum = NgNum;
            _Percent = Percent;
            InitializeComponent();
           
        }

        private void Frm_sql_Load(object sender, EventArgs e)
        {
            //namestart = "时间," + "ID," + "工位,"+ "D1胶厚," + "D2胶厚," + "D1胶宽," +
            //    "D2胶宽," + "D1胶高," + "D2胶高," + "结果" ;
            //namestartid = "@时间," + "@ID," + "@工位," + "@D1胶厚," + "@D2胶厚," + "@D1胶宽," +
            //    "@D2胶宽," + "@D1胶高," + "@D2胶高," + "@结果";
            //bool Exist;
            //System.Threading.Mutex newMutex = new System.Threading.Mutex(true, "Onece",out Exist);
            //if (Exist)
            //{
            //    newMutex.ReleaseMutex();
            //}
            //else
            //{
            //    MessageBox.Show("only onece");
            //    this.Close();
            //}
           
            ShowTableNames(combox_table_names);
            if (combox_table_names.Items.Count!=0)
            {
                tabelName = combox_table_names.SelectedItem.ToString();
                databind(combox_table_names.SelectedItem.ToString());
            }
            
        }
        private void ShowTableNames(ComboBox combox)
        {
            DataTable dt = SQLiteHelper.GetSchema();
            
            string[] tableNames = SQLiteHelper.GetTableName().Trim().Split(',');
            
            foreach (var item in tableNames)
            {
                if (item!="")
                {
                    //string name1 = item.TrimStart('[');
                    //string name = name1.TrimEnd(']');
                                       
                    //    DateTime tableTime = DateTime.Parse(name);
                    //    TimeSpan subTime = DateTime.Now - tableTime;
                    //    if (subTime.Days > 3)
                    //    {
                    //        string ok = SQLiteHelper.DeleteTable(item);                           
                    //    }
                    //else
                    //{
                        combox_table_names.Items.Add(item);
                    //}                 
                }
               
            }
            if (tableNames[0] !="")
            {
                string tableName = DateTime.Now.ToString("[yyyy/MM/dd]");
                if (SQLiteHelper.IsHaveTable(tableName) == "OK")
                {
                    string name1 = tableName.TrimStart('[');
                    string name = name1.TrimEnd(']');
                    combox_table_names.SelectedItem = name;
                }
                else
                {
                    combox_table_names.SelectedIndex = 0;
                }
                
            }
           
            //combox_table_names.Text = tableNames[0];
            
        }

        private static void AddToTable(List<object[]> datalist)
        {
            
            //SQLiteHelper.SQLiteConnString = GlueExtractor.GlueExtractor.dbPath;
            //SQLiteHelper.OpenDb();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
          
            SQLiteHelper.NewDbFile();          
            string ok = SQLiteHelper.NewTable( namestart);
            if (ok!="OK")
            {
                MessageBox.Show("创建失败！" + ok);
            }
            SQLiteHelper.OpenDb();
            DataSet Ds = new DataSet();
            
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("select * from "+ tabelName, SQLiteHelper._SQLiteConn);
            dataAdapter.Fill(Ds, "t");
            dataGridView1.DataSource = Ds.Tables["t"];
            SQLiteHelper.CloseDb();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string time = string.Format("{0}/{1}/{2} {3}", DateTime.Now.Year, DateTime.Now.Month,
                DateTime.Now.Day, DateTime.Now.ToShortTimeString());
             time = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
             
            string sqlstr =string.Format("insert into {0} ({1}) values ({2})", tabelName,namestart,namestartid);
            SQLiteParameter[] sqlPamram = {new SQLiteParameter("@时间",time),
                new SQLiteParameter("@ID",1),new SQLiteParameter("@工位",4),
                new SQLiteParameter("@D1胶厚",2),new SQLiteParameter("@D2胶厚",3),
                new SQLiteParameter("@D1胶宽",4),new SQLiteParameter("@D2胶宽",5),
                new SQLiteParameter("@D1胶高",6),new SQLiteParameter("@D2胶高",7),
                new SQLiteParameter("@结果",8) };
             int a = SQLiteHelper.ExecuteNonQuery(sqlstr, sqlPamram);
            if (a ==-999)
            {
                return;
            }
            
            databind(tabelName);
            //sqlhelp.OpenDb(dbPath);
            //DataSet Ds = new DataSet();

            //SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("select * from " + tabelName, sqlhelp._SQLiteConn);
            //dataAdapter.Fill(Ds, "t");
            //dataGridView1.DataSource = Ds.Tables["t"];
            //sqlhelp.CloseDb();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int con = dataGridView1.Rows.Count;
            if (dataGridView1.SelectedRows.Count<1 || dataGridView1.SelectedRows[0].Cells[1].Value == null)
            {
                MessageBox.Show("请先选择要删除的行！");
                return;
            }
            else
            {
                object oid = dataGridView1.SelectedRows[0].Cells[2].Value;
                if (DialogResult.No == MessageBox.Show("将删除第"+ (dataGridView1.CurrentCell.RowIndex + 1).ToString() + " 行，确定？", "提示", MessageBoxButtons.YesNo))
                {
                    return;
                }
                else
                {
                    SQLiteHelper.Delete(tabelName, "工位", oid);

                }
            }
            databind(tabelName);
            //sqlhelp.OpenDb(dbPath);
            //DataSet Ds = new DataSet();
            //SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("select * from " + tabelName, sqlhelp._SQLiteConn);
            //dataAdapter.Fill(Ds, "t");
            //dataGridView1.DataSource = Ds.Tables["t"];
            //sqlhelp.CloseDb();
        }

        private void databind(string tabelName)
        {
           
            if (SQLiteHelper.IsHaveTable(tabelName) == "OK")
            {
               string ok =  SQLiteHelper.OpenDb();
                if (ok!="OK")
                {
                 
                    return;
                }
                DataSet Ds = new DataSet();
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("select * from " + "[" + tabelName + "]", SQLiteHelper._SQLiteConn);
                dataAdapter.Fill(Ds);
                dataGridView1.DataSource = Ds.Tables[0];
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                int rstIdx = -1;
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    if (dataGridView1.Columns[i].HeaderText == "结果" || dataGridView1.Columns[i].HeaderText == "result")
                    {
                        rstIdx = i;
                    }
                }
                double NgCnt = 0;
                double total = -1;
                if (rstIdx != -1)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string rst = "";
                        if (dataGridView1.Rows[i].Cells[rstIdx].Value != null)
                        {
                             rst = dataGridView1.Rows[i].Cells[rstIdx].Value.ToString();
                        }
                       
                        if (rst == "NG")
                        {
                            NgCnt++;
                        }
                        total++;
                    }

                    //label_ng.Text =( Global.NgNum1+Global.NgNum2).ToString();
                    //label_Total.Text = (Global.TotalNum1+Global.TotalNum2).ToString();
                    //double yield = (Global.NgNum1 + Global.NgNum2) / (Global.TotalNum1 + Global.TotalNum2);
                    //label_Percent.Text = ((1 - yield) * 100).ToString("0.00") + "%";
                    label_ng.Text = _NgNum.ToString();
                    label_Total.Text = _Total.ToString();
                    label_Percent.Text = _Percent.ToString();

                     dataGridView1.DataBindings.Clear();
                }
                else
                {
                    label_ng.Text = "0";
                    label_Total.Text = "0";
                    label_Percent.Text = "0%";
                }               
            }
            else
            {
                MessageBox.Show("未查询到数据！");
            }
            SQLiteHelper.CloseDb();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string Startname = textBox1.Text;
                string Queryname = textBox2.Text;

                //DataTable Ds = SQLiteHelper.QueryDate(combox_table_names.SelectedItem.ToString(),"2019")
                //dataGridView1.DataSource = Ds;

                if (Startname == "" && Queryname != "")//按表名查询
                {
                    databind(Queryname);
                }
                else
                {
                    DataTable Ds = SQLiteHelper.Query(tabelName, Startname, Queryname);
                    dataGridView1.DataSource = Ds;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("未查询到数据！"+ex.Message);
            }
            
           
        }

        private void combox_table_names_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (combox_table_names.Items.Count != 0)
            {
                tabelName = combox_table_names.SelectedItem.ToString();
                databind(combox_table_names.SelectedItem.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable data = (DataTable)dataGridView1.DataSource;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sb.Append(data.Columns[i].Caption.ToString());
                    sb.Append("\t");
                }
                sb.Append("\r\n");
                for (int ind = 0; ind < data.Rows.Count; ind++)
                {
                    for (int n = 0; n < data.Rows[ind].ItemArray.Length; n++)
                    {
                        sb.Append(data.Rows[ind].ItemArray[n].ToString());
                        sb.Append("\t");
                    }
                    sb.Append("\r\n");
                }


                DateTime time = Convert.ToDateTime(combox_table_names.SelectedItem.ToString());
                string path = AppDomain.CurrentDomain.BaseDirectory + "Data\\" + time.ToString("yy_MM_dd") + ".xls";
                FileStream FS = new FileStream(path, FileMode.Create);
                using (StreamWriter sw = new StreamWriter(FS))
                {
                    sw.Write(sb);
                    sw.Close();
                    MessageBox.Show("导出成功 位置" + path);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败！" + ex.Message);
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否删除当前表？","提示",MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                tabelName = combox_table_names.SelectedItem.ToString();
                string ok = SQLiteHelper.DeleteTable(tabelName);
                if (ok=="OK")
                {
                    combox_table_names.Items.Remove(tabelName);
                    //databind(tabelName);
                  
                    dataGridView1.DataSource = new DataTable();
                    MessageBox.Show("删除成功！");
                }
                else
                {
                    MessageBox.Show("删除失败！"+ ok);

                }
            }
        }
    }
}
