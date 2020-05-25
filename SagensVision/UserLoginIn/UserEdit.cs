using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SagensVision.UserLoginIn
{
    public partial class UserEdit : DevExpress.XtraEditors.XtraForm
    {
        public Dictionary<string, string> UserDic = new Dictionary<string, string>();
        string path = AppDomain.CurrentDomain.BaseDirectory + "S.pwd";
        public UserEdit()
        {
            InitializeComponent();
            if (File.Exists(path))
            {
                UserDic = (Dictionary<string, string>)ReadInSerializable(path);
                foreach (var item in UserDic)
                {
                    comboBoxEdit1.Properties.Items.Add(item.Key);
                }
            }
           
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string SelectedUser = comboBoxEdit1.SelectedItem.ToString();
                if (UserDic.Keys.Contains(SelectedUser))
                {
                    if (textBox1.Text != "")
                    {
                        if (MessageBox.Show("是否修改？", "提示",  MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            UserDic.Remove(SelectedUser);
                            UserDic.Add(SelectedUser, textBox1.Text);
                            WriteInSerializable(path, UserDic);
                            MessageBox.Show("修改成功！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string SelectedUser = comboBoxEdit1.SelectedItem.ToString();
                if (!UserDic.Keys.Contains(SelectedUser))
                {
                    if (textBox1.Text != "" && SelectedUser!= "Admin")
                    {
                        if (MessageBox.Show("是否增加用户？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            UserDic.Add(SelectedUser, textBox1.Text);
                            comboBoxEdit1.Properties.Items.Add(SelectedUser);
                            WriteInSerializable(path, UserDic);
                            MessageBox.Show("OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string SelectedUser = comboBoxEdit1.SelectedItem.ToString();
                if (SelectedUser == "Admin")
                {
                    MessageBox.Show("不能移除管理员！");
                    return;
                }
                if (UserDic.Keys.Contains(SelectedUser))
                {
                    string PWD = "";
                    UserDic.TryGetValue(SelectedUser, out PWD);
                    if (textBox1.Text == PWD)
                    {
                        if (MessageBox.Show("是否删除用户？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            UserDic.Remove(SelectedUser);
                            comboBoxEdit1.Properties.Items.Remove(SelectedUser);
                            WriteInSerializable(path, UserDic);
                            MessageBox.Show("移除成功！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("请输入要移除的用户密码！");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 序列化
        /// </summary>
       public string WriteInSerializable(string path, object data)
        {
            try
            {               
                                
                BinaryFormatter binaryFomat = new BinaryFormatter();
                if (File.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                }
               
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    binaryFomat.Serialize(fs, data);
                    fs.Dispose();
                }
                File.SetAttributes(path, FileAttributes.Hidden);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="filePath">文件路径及名称</param>
        /// <returns></returns>
        static public object ReadInSerializable(string filePath)
        {
            try
            {

                if (File.Exists(filePath))
                {

                    BinaryFormatter binaryFomat = new BinaryFormatter();
                    object data;
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    data = (object)binaryFomat.Deserialize(fs);
                    fs.Dispose();
                    return data;
                }
                return null;
            }
            catch (Exception)
            {
            
                return null;
            }

        }

  
    }
}