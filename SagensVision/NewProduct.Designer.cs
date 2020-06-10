namespace SagensVision
{
    partial class NewProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProduct));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit_type = new DevExpress.XtraEditors.TextEdit();
            this.listBox_AllType = new System.Windows.Forms.ListBox();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Copy = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn_ReName = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_type.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(142, 30);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "产品型号：";
            // 
            // textEdit_type
            // 
            this.textEdit_type.Location = new System.Drawing.Point(208, 27);
            this.textEdit_type.Name = "textEdit_type";
            this.textEdit_type.Size = new System.Drawing.Size(132, 20);
            this.textEdit_type.TabIndex = 1;
            // 
            // listBox_AllType
            // 
            this.listBox_AllType.FormattingEnabled = true;
            this.listBox_AllType.ItemHeight = 14;
            this.listBox_AllType.Location = new System.Drawing.Point(209, 71);
            this.listBox_AllType.Name = "listBox_AllType";
            this.listBox_AllType.Size = new System.Drawing.Size(132, 102);
            this.listBox_AllType.TabIndex = 2;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(58, 201);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 3;
            this.simpleButton1.Text = "新建";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btn_Copy
            // 
            this.btn_Copy.Location = new System.Drawing.Point(167, 201);
            this.btn_Copy.Name = "btn_Copy";
            this.btn_Copy.Size = new System.Drawing.Size(75, 23);
            this.btn_Copy.TabIndex = 4;
            this.btn_Copy.Text = "复制";
            this.btn_Copy.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(276, 201);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(75, 23);
            this.btn_Delete.TabIndex = 5;
            this.btn_Delete.Text = "删除";
            this.btn_Delete.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btn_ReName
            // 
            this.btn_ReName.Location = new System.Drawing.Point(385, 201);
            this.btn_ReName.Name = "btn_ReName";
            this.btn_ReName.Size = new System.Drawing.Size(75, 23);
            this.btn_ReName.TabIndex = 6;
            this.btn_ReName.Text = "重命名";
            this.btn_ReName.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // NewProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 249);
            this.Controls.Add(this.btn_ReName);
            this.Controls.Add(this.btn_Delete);
            this.Controls.Add(this.btn_Copy);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.listBox_AllType);
            this.Controls.Add(this.textEdit_type);
            this.Controls.Add(this.labelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewProduct";
            this.Text = "NewProduct";
            this.Load += new System.EventHandler(this.NewProduct_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_type.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textEdit_type;
        private System.Windows.Forms.ListBox listBox_AllType;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton btn_Copy;
        private DevExpress.XtraEditors.SimpleButton btn_Delete;
        private DevExpress.XtraEditors.SimpleButton btn_ReName;
    }
}