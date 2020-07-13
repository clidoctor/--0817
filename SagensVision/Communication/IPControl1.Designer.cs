namespace SagensVision.Communication
{
    partial class IPControl1
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.textBox_MotorIp = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cb_Client = new System.Windows.Forms.CheckBox();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cb_Client);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.textBox1);
            this.groupBox6.Controls.Add(this.textBox_port);
            this.groupBox6.Controls.Add(this.textBox_MotorIp);
            this.groupBox6.Controls.Add(this.label14);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox6.Size = new System.Drawing.Size(314, 258);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "IP";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 32;
            this.label1.Text = "Ok发送消息";
            this.label1.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 196);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 23);
            this.textBox1.TabIndex = 31;
            this.textBox1.Text = "Chat|ok";
            this.textBox1.Visible = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox_MotorIp_TextChanged);
            // 
            // textBox_port
            // 
            this.textBox_port.AcceptsReturn = true;
            this.textBox_port.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox_port.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_port.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox_port.Location = new System.Drawing.Point(14, 98);
            this.textBox_port.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(113, 16);
            this.textBox_port.TabIndex = 29;
            this.textBox_port.Text = "8080";
            this.textBox_port.TextChanged += new System.EventHandler(this.textBox_MotorIp_TextChanged);
            // 
            // textBox_MotorIp
            // 
            this.textBox_MotorIp.AcceptsReturn = true;
            this.textBox_MotorIp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBox_MotorIp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_MotorIp.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox_MotorIp.Location = new System.Drawing.Point(14, 56);
            this.textBox_MotorIp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox_MotorIp.Name = "textBox_MotorIp";
            this.textBox_MotorIp.Size = new System.Drawing.Size(254, 16);
            this.textBox_MotorIp.TabIndex = 27;
            this.textBox_MotorIp.Text = "127.0.0.1";
            this.textBox_MotorIp.TextChanged += new System.EventHandler(this.textBox_MotorIp_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(10, 30);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(91, 17);
            this.label14.TabIndex = 26;
            this.label14.Text = "运动控制IP地址";
            // 
            // cb_Client
            // 
            this.cb_Client.AutoSize = true;
            this.cb_Client.Location = new System.Drawing.Point(14, 134);
            this.cb_Client.Name = "cb_Client";
            this.cb_Client.Size = new System.Drawing.Size(63, 21);
            this.cb_Client.TabIndex = 33;
            this.cb_Client.Text = "客户端";
            this.cb_Client.UseVisualStyleBackColor = true;
            this.cb_Client.CheckedChanged += new System.EventHandler(this.cb_Client_CheckedChanged);
            // 
            // IPControl1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox6);
            this.Name = "IPControl1";
            this.Size = new System.Drawing.Size(314, 258);
            this.Load += new System.EventHandler(this.IPControl1_Load);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.TextBox textBox_MotorIp;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox cb_Client;
    }
}
