namespace SagensVision.VisionTool
{
    partial class ShowCapacityFrm
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
            DevExpress.XtraCharts.SimpleDiagram3D simpleDiagram3D1 = new DevExpress.XtraCharts.SimpleDiagram3D();
            DevExpress.XtraCharts.CustomLegendItem customLegendItem1 = new DevExpress.XtraCharts.CustomLegendItem();
            DevExpress.XtraCharts.CustomLegendItem customLegendItem2 = new DevExpress.XtraCharts.CustomLegendItem();
            DevExpress.XtraCharts.CustomLegendItem customLegendItem3 = new DevExpress.XtraCharts.CustomLegendItem();
            DevExpress.XtraCharts.CustomLegendItem customLegendItem4 = new DevExpress.XtraCharts.CustomLegendItem();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint("ok", new object[] {
            ((object)(1D))}, 0);
            DevExpress.XtraCharts.SeriesPoint seriesPoint2 = new DevExpress.XtraCharts.SeriesPoint("定位异常", new object[] {
            ((object)(1D))}, 1);
            DevExpress.XtraCharts.SeriesPoint seriesPoint3 = new DevExpress.XtraCharts.SeriesPoint("抓边异常", new object[] {
            ((object)(1D))}, 2);
            DevExpress.XtraCharts.SeriesPoint seriesPoint4 = new DevExpress.XtraCharts.SeriesPoint("探高异常", new object[] {
            ((object)(1D))}, 3);
            DevExpress.XtraCharts.Pie3DSeriesView pie3DSeriesView1 = new DevExpress.XtraCharts.Pie3DSeriesView();
            DevExpress.XtraCharts.SeriesTitle seriesTitle1 = new DevExpress.XtraCharts.SeriesTitle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowCapacityFrm));
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.btn_show_clear_data = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(simpleDiagram3D1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pie3DSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            this.chartControl1.DataBindings = null;
            simpleDiagram3D1.RotationMatrixSerializable = "0.477215672887733;0.665010649161712;-0.574478927421923;0;-0.874557448090084;0.423" +
    "44354559115;-0.236315115232545;0;0.086107325707421;0.615188101476881;0.783663912" +
    "822841;0;0;0;0;1";
            this.chartControl1.Diagram = simpleDiagram3D1;
            customLegendItem1.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(229)))), ((int)(((byte)(99)))));
            customLegendItem1.Name = "Custom Legend Item 1";
            customLegendItem1.Text = "OK";
            customLegendItem2.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(105)))), ((int)(((byte)(244)))));
            customLegendItem2.Name = "Custom Legend Item 2";
            customLegendItem2.Text = "定位异常";
            customLegendItem3.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(67)))), ((int)(((byte)(167)))));
            customLegendItem3.Name = "Custom Legend Item 3";
            customLegendItem3.Text = "抓边异常";
            customLegendItem4.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(0)))), ((int)(((byte)(2)))));
            customLegendItem4.Name = "Custom Legend Item 4";
            customLegendItem4.Text = "探高异常";
            this.chartControl1.Legend.CustomItems.AddRange(new DevExpress.XtraCharts.CustomLegendItem[] {
            customLegendItem1,
            customLegendItem2,
            customLegendItem3,
            customLegendItem4});
            this.chartControl1.Legend.Name = "Default Legend";
            this.chartControl1.Location = new System.Drawing.Point(0, 0);
            this.chartControl1.Name = "chartControl1";
            series1.Name = "Series1";
            seriesPoint1.ColorSerializable = "#00E563";
            seriesPoint2.ColorSerializable = "#EC69F4";
            seriesPoint3.ColorSerializable = "#EA43A7";
            seriesPoint4.ColorSerializable = "#C40002";
            series1.Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] {
            seriesPoint1,
            seriesPoint2,
            seriesPoint3,
            seriesPoint4});
            seriesTitle1.Text = "总产能：0";
            pie3DSeriesView1.Titles.AddRange(new DevExpress.XtraCharts.SeriesTitle[] {
            seriesTitle1});
            series1.View = pie3DSeriesView1;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartControl1.Size = new System.Drawing.Size(478, 416);
            this.chartControl1.TabIndex = 0;
            // 
            // btn_show_clear_data
            // 
            this.btn_show_clear_data.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_show_clear_data.Location = new System.Drawing.Point(364, 381);
            this.btn_show_clear_data.Name = "btn_show_clear_data";
            this.btn_show_clear_data.Size = new System.Drawing.Size(101, 23);
            this.btn_show_clear_data.TabIndex = 1;
            this.btn_show_clear_data.Text = "清空产能数据";
            this.btn_show_clear_data.Click += new System.EventHandler(this.btn_show_clear_data_Click);
            // 
            // ShowCapacityFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 416);
            this.Controls.Add(this.btn_show_clear_data);
            this.Controls.Add(this.chartControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShowCapacityFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ShowCapacityFrm";
            this.Load += new System.EventHandler(this.ShowCapacityFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(simpleDiagram3D1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pie3DSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartControl1;
        private DevExpress.XtraEditors.SimpleButton btn_show_clear_data;
    }
}