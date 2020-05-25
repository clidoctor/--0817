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

namespace SagensVision.UserLoginIn
{
    public partial class UserLogin : DevExpress.XtraEditors.XtraForm
    {
        public string CurrentUser = "无权限";
        public UserLogin()
        {
            InitializeComponent();
            simpleButton3.Visible = false;
            if (edit.UserDic!=null)
            {
                foreach (var item in edit.UserDic)
                {
                    comboBoxEdit1.Properties.Items.Add(item.Key);
                }
            }
        }

       

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string AdminName = comboBoxEdit1.SelectedItem.ToString();
                if (AdminName == "Admin")
                {
                        if (textBox1.Text == "S2020")
                        {
                            MessageBox.Show("登录成功");
                            CurrentUser = "Admin";
                            simpleButton3.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("密码错误！");
                        }
                    return;                       
                }
                string value = "";
                edit.UserDic.TryGetValue(AdminName, out value);
                if (textBox1.Text == value)
                {
                    MessageBox.Show("登录成功");
                    CurrentUser = "Admin";                    
                }
                else
                {
                    MessageBox.Show("密码错误！");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        UserEdit edit = new UserEdit();
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentUser == "Admin")
                {
                   
                    edit.ShowDialog();

                }
                else
                {
                    MessageBox.Show("请先登录！");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        bool onece = false;
        private void comboBoxEdit1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (edit.UserDic != null)
                {
                  
                    foreach (var item in edit.UserDic)
                    {
                        if (!comboBoxEdit1.Properties.Items.Contains(item.Key))
                        {
                            comboBoxEdit1.Properties.Items.Add(item.Key);
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}