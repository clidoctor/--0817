using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;
using System.IO;

namespace SagensVision
{
    public partial class NewProduct : DevExpress.XtraEditors.XtraForm
    {
        public NewProduct()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SimpleButton simpleBtn = new SimpleButton();
            simpleBtn = (SimpleButton)sender;
            if (textEdit_type.Text != null)
            {
                string text = textEdit_type.Text.ToString();
                bool ok = Regex.IsMatch(text, @"^[\u4E00-\u9FA5A-Za-z0-9_]+$");
                if (ok)
                {
                    switch (simpleBtn.Text)
                    {
                        case "simpleButton1":
                            if (MyGlobal.PathName.ProductType.Contains(text))
                            {
                                MessageBox.Show("型号已存在！");
                                return;
                            }
                            MyGlobal.PathName.CurrentType = text;
                            MyGlobal.PathName.ProductType.Add(text);
                            Directory.CreateDirectory(MyGlobal.ConfigPath);
                            break;
                        case "btn_Copy":
                            if (listBox_AllType.SelectedItem==null)
                            {
                                MessageBox.Show("请选择一个需要复制的型号！");
                                return;
                            }
                            if (MyGlobal.PathName.ProductType.Contains(text))
                            {
                                MessageBox.Show("型号已存在！");
                                return;
                            }
                            MyGlobal.PathName.ProductType.Add(text);
                            string pathCurrent = MyGlobal.ConfigPath;
                            MyGlobal.PathName.CurrentType = text;
                            Directory.CreateDirectory(MyGlobal.ConfigPath);
                            File.Copy(pathCurrent, MyGlobal.ConfigPath);
                            break;
                        case "btn_Delete":

                            break;
                    }

                }
                else
                {
                    MessageBox.Show("请输入正确的产品型号格式（中文，英文，字母，数字，下划线其中的几种）");
                }
            }
        }

        private void NewProduct_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < MyGlobal.PathName.ProductType.Count; i++)
            {
                listBox_AllType.Items.Add(MyGlobal.PathName.ProductType[i]);
            }
        }
    }
}