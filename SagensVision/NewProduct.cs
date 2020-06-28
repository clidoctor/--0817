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
        public static event Action<string>  AddToFormain;
        public NewProduct(bool isRight)
        {
            InitializeComponent();
            this.isRight = isRight;
        }
        bool isRight = false;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SimpleButton simpleBtn = new SimpleButton();
            simpleBtn = (SimpleButton)sender;

            if (simpleBtn.Name == "btn_Delete")
            {

                if (listBox_AllType.SelectedItem == null)
                {
                    MessageBox.Show("请选择一个需要删除的型号！");
                    return;
                }
                if (Directory.Exists(MyGlobal.AllTypePath + listBox_AllType.SelectedItem.ToString() + "\\"))
                {
                    Directory.Delete(MyGlobal.AllTypePath + listBox_AllType.SelectedItem.ToString() + "\\", true);
                }
                listBox_AllType.Items.Remove(listBox_AllType.SelectedItem.ToString());
                MessageBox.Show("删除成功！");
                AddToFormain?.Invoke("");
                return;
            }

            if (textEdit_type.Text != "")
            {
                string text = textEdit_type.Text.ToString();
                bool ok = Regex.IsMatch(text, @"^[\u4E00-\u9FA5A-Za-z0-9_]+$");
                if (ok)
                {
                    switch (simpleBtn.Name)
                    {
                        case "simpleButton1":
                            if (listBox_AllType.Items.Contains(text))
                            {
                                MessageBox.Show("型号已存在！");
                                return;
                            }
                            MyGlobal.PathName.CurrentType = text;
                            listBox_AllType.Items.Add(text);
                            MessageBox.Show("新建成功");
                            break;
                        case "btn_Copy":
                            if (listBox_AllType.SelectedItem == null)
                            {
                                MessageBox.Show("请选择一个需要复制的型号！");
                                return;
                            }
                            if (listBox_AllType.Items.Contains(text))
                            {
                                MessageBox.Show("型号已存在！");
                                return;
                            }
                            MyGlobal.PathName.CurrentType = listBox_AllType.SelectedItem.ToString();
                            string pathCurrent = isRight ?  MyGlobal.ConfigPath_Right : MyGlobal.ConfigPath_Left;
                            MyGlobal.PathName.CurrentType = text;
                            
                                CopyFiles(pathCurrent, MyGlobal.ConfigPath_Right);
                            
                                CopyFiles(pathCurrent, MyGlobal.ConfigPath_Left);

                           

                            listBox_AllType.Items.Add(text);
                            MessageBox.Show("复制成功");
                            break;
                        case "btn_ReName":
                            if (listBox_AllType.SelectedItem == null)
                            {
                                MessageBox.Show("请选择一个需要重命名的型号！");
                                return;
                            }
                            if (listBox_AllType.Items.Contains(text))
                            {
                                MessageBox.Show("型号已存在！");
                                return;
                            }
                            if (Directory.Exists(MyGlobal.AllTypePath + listBox_AllType.SelectedItem.ToString() + "\\"))
                            {
                                string SourceName = MyGlobal.AllTypePath + listBox_AllType.SelectedItem.ToString() + "\\";
                                string DestName = MyGlobal.AllTypePath + text + "\\";
                                Directory.Move(SourceName, DestName);
                                listBox_AllType.Items.Remove(listBox_AllType.SelectedItem.ToString());
                                listBox_AllType.Items.Add(text);
                                MyGlobal.PathName.CurrentType = text;
                            }
                            break;
                    }
                    AddToFormain?.Invoke(text);
                    StaticOperate.WriteXML(MyGlobal.PathName, MyGlobal.AllTypePath + "AllType.xml");
                }
                else
                {
                    MessageBox.Show("请输入正确的产品型号格式（中文，英文，字母，数字，下划线其中的几种）");
                }
            }
        }

        void CopyFiles( string FilePath,string NewPath)
        {
            DirectoryInfo dirinf = new DirectoryInfo(FilePath);
            FileSystemInfo[] fileinfo = dirinf.GetFileSystemInfos();
            for (int i = 0; i < fileinfo.Length; i++)
            {
                if (fileinfo[i] is FileInfo)
                {
                    File.Copy(fileinfo[i].FullName, NewPath + fileinfo[i].Name);
                }
                else
                {
                    string dir = "\\" + fileinfo[i].Name+ "\\";
                    if (!Directory.Exists(NewPath + dir))
                    {
                        Directory.CreateDirectory(NewPath + dir);
                    }
                    CopyFiles(FilePath + dir, NewPath + dir);
                }           
            }
            
        }

        private void NewProduct_Load(object sender, EventArgs e)
        {
            DirectoryInfo dirinf = new DirectoryInfo(MyGlobal.AllTypePath);
            DirectoryInfo[] dirinfo = dirinf.GetDirectories();
            for (int i = 0; i < dirinfo.Length; i++)
            {
                listBox_AllType.Items.Add(dirinfo[i].Name);
            }
        }
    }


}