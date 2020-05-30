namespace SagensVision.VisionTool
{
    partial class ImgRotateFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImgRotateFrm));
            this.l1 = new System.Windows.Forms.NumericUpDown();
            this.t2 = new System.Windows.Forms.NumericUpDown();
            this.r3 = new System.Windows.Forms.NumericUpDown();
            this.d4 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.l1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.t2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.r3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.d4)).BeginInit();
            this.SuspendLayout();
            // 
            // l1
            // 
            this.l1.Location = new System.Drawing.Point(112, 39);
            this.l1.Margin = new System.Windows.Forms.Padding(4);
            this.l1.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.l1.Name = "l1";
            this.l1.Size = new System.Drawing.Size(160, 25);
            this.l1.TabIndex = 0;
            // 
            // t2
            // 
            this.t2.Location = new System.Drawing.Point(112, 90);
            this.t2.Margin = new System.Windows.Forms.Padding(4);
            this.t2.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.t2.Name = "t2";
            this.t2.Size = new System.Drawing.Size(160, 25);
            this.t2.TabIndex = 1;
            // 
            // r3
            // 
            this.r3.Location = new System.Drawing.Point(112, 139);
            this.r3.Margin = new System.Windows.Forms.Padding(4);
            this.r3.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.r3.Name = "r3";
            this.r3.Size = new System.Drawing.Size(160, 25);
            this.r3.TabIndex = 2;
            // 
            // d4
            // 
            this.d4.Location = new System.Drawing.Point(112, 189);
            this.d4.Margin = new System.Windows.Forms.Padding(4);
            this.d4.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.d4.Name = "d4";
            this.d4.Size = new System.Drawing.Size(160, 25);
            this.d4.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "第一条边：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 92);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "第二条边：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 141);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "第三条边：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 193);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "第四条边：";
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(356, 186);
            this.btn_save.Margin = new System.Windows.Forms.Padding(4);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(100, 29);
            this.btn_save.TabIndex = 8;
            this.btn_save.Text = "保存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // ImgRotateFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 236);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.d4);
            this.Controls.Add(this.r3);
            this.Controls.Add(this.t2);
            this.Controls.Add(this.l1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ImgRotateFrm";
            this.Text = "ImgRotateFrm";
            this.Load += new System.EventHandler(this.ImgRotateFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.l1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.t2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.r3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.d4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown l1;
        private System.Windows.Forms.NumericUpDown t2;
        private System.Windows.Forms.NumericUpDown r3;
        private System.Windows.Forms.NumericUpDown d4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_save;
    }
}