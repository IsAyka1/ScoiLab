
namespace Ayka_scoi
{
    partial class Form2
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chartOrig = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartChange = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartOrig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartChange)).BeginInit();
            this.SuspendLayout();
            // 
            // chartOrig
            // 
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.Maximum = 255D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.Maximum = 30000D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartArea1";
            this.chartOrig.ChartAreas.Add(chartArea1);
            this.chartOrig.Location = new System.Drawing.Point(451, 15);
            this.chartOrig.Name = "chartOrig";
            this.chartOrig.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            series1.ChartArea = "ChartArea1";
            series1.IsVisibleInLegend = false;
            series1.LabelBorderWidth = 0;
            series1.LabelForeColor = System.Drawing.Color.Transparent;
            series1.MarkerBorderColor = System.Drawing.Color.Red;
            series1.Name = "Series1";
            this.chartOrig.Series.Add(series1);
            this.chartOrig.Size = new System.Drawing.Size(344, 189);
            this.chartOrig.TabIndex = 1;
            this.chartOrig.Text = "chart1";
            title1.Name = "Title1";
            title1.Text = "Оригинал";
            this.chartOrig.Titles.Add(title1);
            // 
            // chartChange
            // 
            chartArea2.AxisX.Maximum = 255D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisY.Maximum = 10000D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.Name = "ChartArea1";
            this.chartChange.ChartAreas.Add(chartArea2);
            this.chartChange.Location = new System.Drawing.Point(448, 226);
            this.chartChange.Name = "chartChange";
            this.chartChange.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            series2.ChartArea = "ChartArea1";
            series2.MarkerBorderColor = System.Drawing.Color.White;
            series2.Name = "Series1";
            this.chartChange.Series.Add(series2);
            this.chartChange.Size = new System.Drawing.Size(346, 207);
            this.chartChange.TabIndex = 2;
            this.chartChange.Text = "chart2";
            title2.Name = "Title1";
            title2.Text = "Измененная";
            this.chartChange.Titles.Add(title2);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chartChange);
            this.Controls.Add(this.chartOrig);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.chartOrig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartChange)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chartOrig;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartChange;
    }
}