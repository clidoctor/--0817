namespace SagensVision
{
    partial class OfflineFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfflineFrm));
            this.tb_checkNum = new System.Windows.Forms.TextBox();
            this.tb_FileNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.listBox1 = new DevExpress.XtraEditors.ListBoxControl();
            this.cb_runMode = new System.Windows.Forms.CheckBox();
            this.tb_PathName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_kdata = new System.Windows.Forms.CheckBox();
            this.cb_RunAll = new System.Windows.Forms.CheckBox();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.sBtn_next = new DevExpress.XtraEditors.SimpleButton();
            this.sBtn_pathImport = new DevExpress.XtraEditors.SimpleButton();
            this.sBtn_pathSelect = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.listBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_checkNum
            // 
            this.tb_checkNum.Location = new System.Drawing.Point(526, 119);
            this.tb_checkNum.Margin = new System.Windows.Forms.Padding(4);
            this.tb_checkNum.Name = "tb_checkNum";
            this.tb_checkNum.Size = new System.Drawing.Size(105, 22);
            this.tb_checkNum.TabIndex = 10;
            // 
            // tb_FileNum
            // 
            this.tb_FileNum.Location = new System.Drawing.Point(170, 119);
            this.tb_FileNum.Margin = new System.Windows.Forms.Padding(4);
            this.tb_FileNum.Name = "tb_FileNum";
            this.tb_FileNum.Size = new System.Drawing.Size(101, 22);
            this.tb_FileNum.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(421, 122);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 8;
            this.label2.Text = "选中物料ID：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(25, 122);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 14);
            this.label1.TabIndex = 9;
            this.label1.Text = "当前文件夹物料数量：";
            // 
            // listBox1
            // 
            this.listBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.listBox1.Location = new System.Drawing.Point(25, 16);
            this.listBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(651, 234);
            this.listBox1.TabIndex = 13;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged_1);
            // 
            // cb_runMode
            // 
            this.cb_runMode.ForeColor = System.Drawing.Color.Black;
            this.cb_runMode.Location = new System.Drawing.Point(556, 62);
            this.cb_runMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cb_runMode.Name = "cb_runMode";
            this.cb_runMode.Size = new System.Drawing.Size(79, 23);
            this.cb_runMode.TabIndex = 9;
            this.cb_runMode.Text = "单次运行";
            this.cb_runMode.CheckedChanged += new System.EventHandler(this.cb_runMode_CheckedChanged);
            // 
            // tb_PathName
            // 
            this.tb_PathName.Location = new System.Drawing.Point(170, 61);
            this.tb_PathName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_PathName.Name = "tb_PathName";
            this.tb_PathName.Size = new System.Drawing.Size(257, 22);
            this.tb_PathName.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.cb_kdata);
            this.groupBox1.Controls.Add(this.cb_RunAll);
            this.groupBox1.Controls.Add(this.tb_checkNum);
            this.groupBox1.Controls.Add(this.simpleButton2);
            this.groupBox1.Controls.Add(this.tb_FileNum);
            this.groupBox1.Controls.Add(this.sBtn_next);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.sBtn_pathImport);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.sBtn_pathSelect);
            this.groupBox1.Controls.Add(this.tb_PathName);
            this.groupBox1.Controls.Add(this.cb_runMode);
            this.groupBox1.Location = new System.Drawing.Point(25, 257);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(651, 158);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作";
            // 
            // cb_kdata
            // 
            this.cb_kdata.ForeColor = System.Drawing.Color.Black;
            this.cb_kdata.Location = new System.Drawing.Point(455, 91);
            this.cb_kdata.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cb_kdata.Name = "cb_kdata";
            this.cb_kdata.Size = new System.Drawing.Size(93, 23);
            this.cb_kdata.TabIndex = 16;
            this.cb_kdata.Text = "生成Kdata";
            // 
            // cb_RunAll
            // 
            this.cb_RunAll.ForeColor = System.Drawing.Color.Black;
            this.cb_RunAll.Location = new System.Drawing.Point(455, 62);
            this.cb_RunAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cb_RunAll.Name = "cb_RunAll";
            this.cb_RunAll.Size = new System.Drawing.Size(80, 23);
            this.cb_RunAll.TabIndex = 15;
            this.cb_RunAll.Text = "运行所有";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(536, 21);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(95, 36);
            this.simpleButton2.TabIndex = 14;
            this.simpleButton2.Text = "运行";
            this.simpleButton2.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // sBtn_next
            // 
            this.sBtn_next.Location = new System.Drawing.Point(424, 21);
            this.sBtn_next.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sBtn_next.Name = "sBtn_next";
            this.sBtn_next.Size = new System.Drawing.Size(91, 36);
            this.sBtn_next.TabIndex = 13;
            this.sBtn_next.Text = "Next";
            this.sBtn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // sBtn_pathImport
            // 
            this.sBtn_pathImport.Location = new System.Drawing.Point(28, 61);
            this.sBtn_pathImport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sBtn_pathImport.Name = "sBtn_pathImport";
            this.sBtn_pathImport.Size = new System.Drawing.Size(124, 20);
            this.sBtn_pathImport.TabIndex = 12;
            this.sBtn_pathImport.Text = "路径手动导入：";
            this.sBtn_pathImport.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // sBtn_pathSelect
            // 
            this.sBtn_pathSelect.Location = new System.Drawing.Point(28, 21);
            this.sBtn_pathSelect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sBtn_pathSelect.Name = "sBtn_pathSelect";
            this.sBtn_pathSelect.Size = new System.Drawing.Size(124, 22);
            this.sBtn_pathSelect.TabIndex = 11;
            this.sBtn_pathSelect.Text = "路径选择";
            this.sBtn_pathSelect.Click += new System.EventHandler(this.sBtn_pathSelect_Click);
            // 
            // OfflineFrm
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.Appearance.ForeColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 435);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OfflineFrm";
            this.Text = "离线测试";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OfflineFrm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OfflineFrm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.listBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox tb_checkNum;
        private System.Windows.Forms.TextBox tb_FileNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private DevExpress.XtraEditors.ListBoxControl listBox1;
        private System.Windows.Forms.CheckBox cb_runMode;
        private System.Windows.Forms.TextBox tb_PathName;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton sBtn_next;
        private DevExpress.XtraEditors.SimpleButton sBtn_pathImport;
        private DevExpress.XtraEditors.SimpleButton sBtn_pathSelect;
        private System.Windows.Forms.CheckBox cb_RunAll;
        private System.Windows.Forms.CheckBox cb_kdata;
    }
}