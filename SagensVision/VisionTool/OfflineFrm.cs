﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SagensVision
{
    public partial class OfflineFrm : DevExpress.XtraEditors.XtraForm
    {
        FormMain formMain = null;
        public OfflineFrm()
        {
            InitializeComponent();
            Run = new RunOff(dmmy);
        }
       
        string path = "";
        public delegate void RunOff(string Path);
        public RunOff Run;
        public void dmmy(string Path)
        {

        }

        //选择离线数据存储路径
        private void btn_select_Click(object sender, EventArgs e)
        {
            path = "";
            try
            {
                if (tb_PathName != null)
                {
                    path = tb_PathName.Text;
                    listBox1.Items.Clear();
                    string[] ImportpathfileNames = Directory.GetDirectories(path);

                    tb_FileNum.Text = ImportpathfileNames.Length.ToString();

                    //遍历路径下的所有物料文件夹，并Add到Listbox上
                    foreach (string tempFileNames in ImportpathfileNames)
                    {
                        listBox1.Items.Add(tempFileNames);
                    }    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("请确认选择正确的路径！");
            }
           
        }

        //手动切换到下一片物料路径
        private void btn_next_Click(object sender, EventArgs e)
        {
            if (!(listBox1.SelectedItem == null))
            {
                if ((listBox1.SelectedIndex + 1) == listBox1.Items.Count)
                {
                    MessageBox.Show("已到最后一个文件！");
                }
                else
                {
                    path = (listBox1.Items[listBox1.SelectedIndex + 1]).ToString();
                    listBox1.SetSelected(listBox1.SelectedIndex + 1, true);
                    tb_checkNum.Text = (listBox1.SelectedIndex + 1).ToString();
                }

            }
            else
                MessageBox.Show("Please slect a path!");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem!=null)
                path = listBox1.SelectedItem.ToString();
            tb_checkNum.Text = (listBox1.SelectedIndex + 1).ToString();
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            try
            {
                //run之前先判断该路径下面的文件是否齐全
                path = (listBox1.Items[listBox1.SelectedIndex]).ToString();
                string[] fileNames = Directory.GetFiles(path,"*.tiff");
                if (fileNames.Length != 8 && fileNames.Length!=12)
                    MessageBox.Show("当前物料文件不齐全，请确认目录缺失文件");
                else
                {
                    if (!(listBox1.SelectedItem == null))
                    {

                        //MessageBox.Show("当前运行物料ID：" + (listBox1.SelectedIndex + 1).ToString());
                        //run（path）
                        Run(path);
                        if (cb_runMode.Checked)
                        {
                            if ((listBox1.SelectedIndex + 1) == listBox1.Items.Count)
                            {
                                MessageBox.Show("已到最后一个文件！");
                            }
                            else
                            {
                                path = (listBox1.Items[listBox1.SelectedIndex + 1]).ToString();
                                listBox1.SetSelected(listBox1.SelectedIndex + 1, true);

                            }
                            //MessageBox.Show("当前选中物料ID:" + (listBox1.SelectedIndex + 1).ToString());

                        }
                        tb_checkNum.Text = (listBox1.SelectedIndex + 1).ToString();

                    }
                    else
                    {
                        MessageBox.Show("请确认已在ListBox选择需要测试的物料");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cb_runMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!cb_runMode.Checked)
            {
                cb_runMode.Text = "单次运行";
            }
            cb_runMode.Text = "连续运行";
        }

        private void sBtn_pathSelect_Click(object sender, EventArgs e)
        {
            path = "";
            try
            {
                listBox1.Items.Clear();
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = folderBrowserDialog1.SelectedPath.ToString();

                }

                string[] fileNames = Directory.GetDirectories(path);

                tb_FileNum.Text = fileNames.Length.ToString();

                //遍历路径下的所有物料文件夹，并Add到Listbox上
                foreach (string tempFileNames in fileNames)
                {
                    listBox1.Items.Add(tempFileNames);
                }
     
                }
             catch (Exception ex)
             {
                MessageBox.Show("请确认选择正确的路径！");
              }

        }
    }
}
