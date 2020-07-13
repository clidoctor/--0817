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
        public static Verify CurrentUser = Verify.操作员;
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
                
                UserMsg user = null;
                edit.UserDic.TryGetValue(AdminName, out user);
                if (textBox1.Text == user.password)
                {
                    MessageBox.Show($"登录成功,权限为<{user.verify.ToString()}>");
                    simpleButton4.Visible = true;
                    CurrentUser = user.verify;
                    if (user.verify == Verify.管理员)
                    {
                        simpleButton3.Visible = true;
                    }
                    else { simpleButton3.Visible = false; }
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
            finally { /*textBox1.Text = "";*/ }
        }
        UserEdit edit = new UserEdit();
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentUser == Verify.管理员)
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

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            CurrentUser = Verify.操作员;
            simpleButton4.Visible = false;
            simpleButton3.Visible = false;
        }
    }
}