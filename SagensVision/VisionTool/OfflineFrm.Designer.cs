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
            this.btn_select = new System.Windows.Forms.Button();
            this.btn_next = new System.Windows.Forms.Button();
            this.btn_run = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tb_checkNum = new System.Windows.Forms.TextBox();
            this.tb_FileNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_PathName = new System.Windows.Forms.TextBox();
            this.cb_runMode = new System.Windows.Forms.CheckBox();
            this.btn_pathImport = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_select
            // 
            this.btn_select.Location = new System.Drawing.Point(32, 28);
            this.btn_select.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(142, 28);
            this.btn_select.TabIndex = 0;
            this.btn_select.Tag = "0";
            this.btn_select.Text = "路径选择";
            this.btn_select.UseVisualStyleBackColor = true;
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(451, 28);
            this.btn_next.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(75, 28);
            this.btn_next.TabIndex = 1;
            this.btn_next.Text = "Next";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.btn_next_Click);
            // 
            // btn_run
            // 
            this.btn_run.Location = new System.Drawing.Point(642, 28);
            this.btn_run.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_run.Name = "btn_run";
            this.btn_run.Size = new System.Drawing.Size(80, 49);
            this.btn_run.TabIndex = 2;
            this.btn_run.Text = "运行";
            this.btn_run.UseVisualStyleBackColor = true;
            this.btn_run.Click += new System.EventHandler(this.btn_run_Click);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 18;
            this.listBox1.Location = new System.Drawing.Point(29, 34);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(737, 290);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // tb_checkNum
            // 
            this.tb_checkNum.Location = new System.Drawing.Point(634, 454);
            this.tb_checkNum.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tb_checkNum.Name = "tb_checkNum";
            this.tb_checkNum.Size = new System.Drawing.Size(119, 26);
            this.tb_checkNum.TabIndex = 10;
            // 
            // tb_FileNum
            // 
            this.tb_FileNum.Location = new System.Drawing.Point(223, 455);
            this.tb_FileNum.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tb_FileNum.Name = "tb_FileNum";
            this.tb_FileNum.Size = new System.Drawing.Size(115, 26);
            this.tb_FileNum.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(510, 458);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "选中物料ID：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 460);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 18);
            this.label1.TabIndex = 9;
            this.label1.Text = "当前文件夹物料数量：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_PathName);
            this.groupBox1.Controls.Add(this.cb_runMode);
            this.groupBox1.Controls.Add(this.btn_pathImport);
            this.groupBox1.Controls.Add(this.btn_select);
            this.groupBox1.Controls.Add(this.btn_next);
            this.groupBox1.Controls.Add(this.btn_run);
            this.groupBox1.Location = new System.Drawing.Point(29, 330);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(744, 203);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作";
            // 
            // tb_PathName
            // 
            this.tb_PathName.Location = new System.Drawing.Point(194, 79);
            this.tb_PathName.Name = "tb_PathName";
            this.tb_PathName.Size = new System.Drawing.Size(332, 26);
            this.tb_PathName.TabIndex = 10;
            // 
            // cb_runMode
            // 
            this.cb_runMode.Location = new System.Drawing.Point(533, 57);
            this.cb_runMode.Margin = new System.Windows.Forms.Padding(4);
            this.cb_runMode.Name = "cb_runMode";
            this.cb_runMode.Size = new System.Drawing.Size(116, 30);
            this.cb_runMode.TabIndex = 9;
            this.cb_runMode.Text = "单次运行";
            this.cb_runMode.CheckedChanged += new System.EventHandler(this.cb_runMode_CheckedChanged);
            // 
            // btn_pathImport
            // 
            this.btn_pathImport.Location = new System.Drawing.Point(32, 77);
            this.btn_pathImport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_pathImport.Name = "btn_pathImport";
            this.btn_pathImport.Size = new System.Drawing.Size(142, 28);
            this.btn_pathImport.TabIndex = 0;
            this.btn_pathImport.Tag = "1";
            this.btn_pathImport.Text = "路径手动导入：";
            this.btn_pathImport.UseVisualStyleBackColor = true;
            this.btn_pathImport.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // OfflineFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 559);
            this.Controls.Add(this.tb_checkNum);
            this.Controls.Add(this.tb_FileNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OfflineFrm";
            this.Text = "OfflineFrm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_select;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.Button btn_run;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox tb_checkNum;
        private System.Windows.Forms.TextBox tb_FileNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_runMode;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox tb_PathName;
        private System.Windows.Forms.Button btn_pathImport;
    }
}