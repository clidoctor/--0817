namespace SagensVision.VisionTool
{
    partial class GlobalParam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalParam));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_Start = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_ColorMin = new System.Windows.Forms.TextBox();
            this.textBox_ColorMax = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox_totalZ = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_yOffset = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_xOffset = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_HeightMax = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_HeightMin = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.textBox_XYMax = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_XYMin = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.checkedListBox_save_data = new System.Windows.Forms.CheckedListBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cb_Features = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_Start);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(356, 308);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 72);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "点胶起始点设置";
            // 
            // textBox_Start
            // 
            this.textBox_Start.Location = new System.Drawing.Point(102, 36);
            this.textBox_Start.Name = "textBox_Start";
            this.textBox_Start.Size = new System.Drawing.Size(100, 22);
            this.textBox_Start.TabIndex = 3;
            this.textBox_Start.TextChanged += new System.EventHandler(this.textBox_ColorMin_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "起始点";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.textBox_ColorMin);
            this.groupBox6.Controls.Add(this.textBox_ColorMax);
            this.groupBox6.Location = new System.Drawing.Point(12, 58);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(266, 93);
            this.groupBox6.TabIndex = 66;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "颜色高度区间";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(177, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 14);
            this.label10.TabIndex = 49;
            this.label10.Text = "Max";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(40, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 14);
            this.label9.TabIndex = 48;
            this.label9.Text = "Min";
            // 
            // textBox_ColorMin
            // 
            this.textBox_ColorMin.Location = new System.Drawing.Point(6, 52);
            this.textBox_ColorMin.Name = "textBox_ColorMin";
            this.textBox_ColorMin.Size = new System.Drawing.Size(100, 22);
            this.textBox_ColorMin.TabIndex = 47;
            this.textBox_ColorMin.Text = "0";
            this.textBox_ColorMin.TextChanged += new System.EventHandler(this.textBox_ColorMin_TextChanged);
            // 
            // textBox_ColorMax
            // 
            this.textBox_ColorMax.Location = new System.Drawing.Point(150, 52);
            this.textBox_ColorMax.Name = "textBox_ColorMax";
            this.textBox_ColorMax.Size = new System.Drawing.Size(100, 22);
            this.textBox_ColorMax.TabIndex = 45;
            this.textBox_ColorMax.Text = "0";
            this.textBox_ColorMax.TextChanged += new System.EventHandler(this.textBox_ColorMin_TextChanged);
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(208, 33);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(27, 22);
            this.label18.TabIndex = 69;
            this.label18.Text = "mm";
            // 
            // textBox_totalZ
            // 
            this.textBox_totalZ.Location = new System.Drawing.Point(102, 33);
            this.textBox_totalZ.Name = "textBox_totalZ";
            this.textBox_totalZ.Size = new System.Drawing.Size(100, 22);
            this.textBox_totalZ.TabIndex = 68;
            this.textBox_totalZ.Text = "0";
            this.textBox_totalZ.TextChanged += new System.EventHandler(this.textBox_ColorMin_TextChanged);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(21, 36);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 22);
            this.label13.TabIndex = 67;
            this.label13.Text = "Z";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox1);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.textBox_yOffset);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.textBox_xOffset);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(356, 110);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(265, 192);
            this.groupBox3.TabIndex = 75;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "单边偏移";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Side1",
            "Side2",
            "Side3",
            "Side4"});
            this.comboBox1.Location = new System.Drawing.Point(102, 37);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 22);
            this.comboBox1.TabIndex = 78;
            this.comboBox1.Text = "Side1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(19, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 22);
            this.label6.TabIndex = 77;
            this.label6.Text = "切换边";
            // 
            // textBox_yOffset
            // 
            this.textBox_yOffset.Location = new System.Drawing.Point(102, 139);
            this.textBox_yOffset.Name = "textBox_yOffset";
            this.textBox_yOffset.Size = new System.Drawing.Size(100, 22);
            this.textBox_yOffset.TabIndex = 74;
            this.textBox_yOffset.Text = "0";
            this.textBox_yOffset.TextChanged += new System.EventHandler(this.textBox_ColorMin_TextChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(208, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 22);
            this.label4.TabIndex = 75;
            this.label4.Text = "mm";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(21, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 22);
            this.label5.TabIndex = 73;
            this.label5.Text = "y偏移";
            // 
            // textBox_xOffset
            // 
            this.textBox_xOffset.Location = new System.Drawing.Point(102, 84);
            this.textBox_xOffset.Name = "textBox_xOffset";
            this.textBox_xOffset.Size = new System.Drawing.Size(100, 22);
            this.textBox_xOffset.TabIndex = 71;
            this.textBox_xOffset.Text = "0";
            this.textBox_xOffset.TextChanged += new System.EventHandler(this.textBox_ColorMin_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(208, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 22);
            this.label3.TabIndex = 72;
            this.label3.Text = "mm";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 22);
            this.label2.TabIndex = 70;
            this.label2.Text = "x偏移";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_HeightMax);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBox_HeightMin);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(12, 292);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(266, 93);
            this.groupBox2.TabIndex = 76;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "点胶高度限制";
            // 
            // textBox_HeightMax
            // 
            this.textBox_HeightMax.Location = new System.Drawing.Point(150, 56);
            this.textBox_HeightMax.Name = "textBox_HeightMax";
            this.textBox_HeightMax.Size = new System.Drawing.Size(100, 22);
            this.textBox_HeightMax.TabIndex = 5;
            this.textBox_HeightMax.Text = "0";
            this.textBox_HeightMax.TextChanged += new System.EventHandler(this.textBox_ColorMin_TextChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(177, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 14);
            this.label8.TabIndex = 4;
            this.label8.Text = "Max";
            // 
            // textBox_HeightMin
            // 
            this.textBox_HeightMin.Location = new System.Drawing.Point(13, 56);
            this.textBox_HeightMin.Name = "textBox_HeightMin";
            this.textBox_HeightMin.Size = new System.Drawing.Size(100, 22);
            this.textBox_HeightMin.TabIndex = 3;
            this.textBox_HeightMin.Text = "0";
            this.textBox_HeightMin.TextChanged += new System.EventHandler(this.textBox_ColorMin_TextChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(40, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 14);
            this.label7.TabIndex = 1;
            this.label7.Text = "Min";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(172, 537);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(90, 23);
            this.simpleButton1.TabIndex = 77;
            this.simpleButton1.Text = "保存";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(356, 537);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(90, 23);
            this.simpleButton2.TabIndex = 78;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.textBox_totalZ);
            this.groupBox4.Location = new System.Drawing.Point(357, 386);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(265, 72);
            this.groupBox4.TabIndex = 79;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "整体高度偏移";
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(458, 472);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(90, 23);
            this.simpleButton3.TabIndex = 80;
            this.simpleButton3.Text = "获取基准高度";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBox_XYMax);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.textBox_XYMin);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Location = new System.Drawing.Point(12, 402);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(266, 93);
            this.groupBox5.TabIndex = 81;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "点胶XY限制";
            // 
            // textBox_XYMax
            // 
            this.textBox_XYMax.Location = new System.Drawing.Point(150, 56);
            this.textBox_XYMax.Name = "textBox_XYMax";
            this.textBox_XYMax.Size = new System.Drawing.Size(100, 22);
            this.textBox_XYMax.TabIndex = 5;
            this.textBox_XYMax.Text = "0";
            this.textBox_XYMax.TextChanged += new System.EventHandler(this.textBox_ColorMin_TextChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(177, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 14);
            this.label11.TabIndex = 4;
            this.label11.Text = "Max";
            // 
            // textBox_XYMin
            // 
            this.textBox_XYMin.Location = new System.Drawing.Point(13, 56);
            this.textBox_XYMin.Name = "textBox_XYMin";
            this.textBox_XYMin.Size = new System.Drawing.Size(100, 22);
            this.textBox_XYMin.TabIndex = 3;
            this.textBox_XYMin.Text = "0";
            this.textBox_XYMin.TextChanged += new System.EventHandler(this.textBox_ColorMin_TextChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(40, 30);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 14);
            this.label12.TabIndex = 1;
            this.label12.Text = "Min";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.checkedListBox_save_data);
            this.groupBox7.Location = new System.Drawing.Point(12, 157);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(266, 129);
            this.groupBox7.TabIndex = 82;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "数据保存选项";
            // 
            // checkedListBox_save_data
            // 
            this.checkedListBox_save_data.FormattingEnabled = true;
            this.checkedListBox_save_data.Items.AddRange(new object[] {
            "保存kdat",
            "保存filedat",
            "保存正常图像"});
            this.checkedListBox_save_data.Location = new System.Drawing.Point(13, 38);
            this.checkedListBox_save_data.Name = "checkedListBox_save_data";
            this.checkedListBox_save_data.Size = new System.Drawing.Size(237, 72);
            this.checkedListBox_save_data.TabIndex = 0;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Right",
            "Left"});
            this.comboBox2.Location = new System.Drawing.Point(84, 12);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(100, 22);
            this.comboBox2.TabIndex = 83;
            this.comboBox2.Text = "Right";
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(22, 15);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 14);
            this.label14.TabIndex = 84;
            this.label14.Text = "工位切换";
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(357, 15);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(90, 23);
            this.simpleButton4.TabIndex = 85;
            this.simpleButton4.Text = "型号设置";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Location = new System.Drawing.Point(459, 15);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(90, 23);
            this.simpleButton5.TabIndex = 86;
            this.simpleButton5.Text = "图像旋转设置";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(357, 68);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(122, 18);
            this.checkBox1.TabIndex = 87;
            this.checkBox1.Text = "启用角落曲面校正";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cb_Features
            // 
            this.cb_Features.AutoSize = true;
            this.cb_Features.Location = new System.Drawing.Point(485, 68);
            this.cb_Features.Name = "cb_Features";
            this.cb_Features.Size = new System.Drawing.Size(74, 18);
            this.cb_Features.TabIndex = 88;
            this.cb_Features.Text = "特征显示";
            this.cb_Features.UseVisualStyleBackColor = true;
            this.cb_Features.CheckedChanged += new System.EventHandler(this.cb_Features_CheckedChanged);
            // 
            // GlobalParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 572);
            this.Controls.Add(this.cb_Features);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GlobalParam";
            this.Text = "GlobalParam";
            this.Load += new System.EventHandler(this.GlobalParam_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_Start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox textBox_ColorMin;
        private System.Windows.Forms.TextBox textBox_ColorMax;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox_totalZ;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_yOffset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_xOffset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_HeightMax;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_HeightMin;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox textBox_XYMax;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_XYMin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckedListBox checkedListBox_save_data;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label14;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox cb_Features;
    }
}