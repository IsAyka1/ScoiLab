
namespace Scoi4
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bCalculate = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxMatrix = new System.Windows.Forms.TextBox();
            this.textBoxSigma = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonMedian = new System.Windows.Forms.RadioButton();
            this.radioButtonLine = new System.Windows.Forms.RadioButton();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.bAdd = new System.Windows.Forms.Button();
            this.radioButtonGauss = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxR = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // bCalculate
            // 
            this.bCalculate.Location = new System.Drawing.Point(17, 416);
            this.bCalculate.Name = "bCalculate";
            this.bCalculate.Size = new System.Drawing.Size(140, 38);
            this.bCalculate.TabIndex = 0;
            this.bCalculate.Text = "Расчитать";
            this.bCalculate.UseVisualStyleBackColor = true;
            this.bCalculate.Click += new System.EventHandler(this.bCalculate_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(17, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(601, 385);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // textBoxMatrix
            // 
            this.textBoxMatrix.Location = new System.Drawing.Point(629, 163);
            this.textBoxMatrix.Multiline = true;
            this.textBoxMatrix.Name = "textBoxMatrix";
            this.textBoxMatrix.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMatrix.Size = new System.Drawing.Size(177, 136);
            this.textBoxMatrix.TabIndex = 2;
            // 
            // textBoxSigma
            // 
            this.textBoxSigma.Location = new System.Drawing.Point(629, 321);
            this.textBoxSigma.Name = "textBoxSigma";
            this.textBoxSigma.Size = new System.Drawing.Size(175, 27);
            this.textBoxSigma.TabIndex = 3;
            this.textBoxSigma.TextChanged += new System.EventHandler(this.textBoxSigma_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(631, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Матрица";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(631, 299);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Сигма";
            // 
            // radioButtonMedian
            // 
            this.radioButtonMedian.AutoSize = true;
            this.radioButtonMedian.Location = new System.Drawing.Point(624, 448);
            this.radioButtonMedian.Name = "radioButtonMedian";
            this.radioButtonMedian.Size = new System.Drawing.Size(198, 24);
            this.radioButtonMedian.TabIndex = 6;
            this.radioButtonMedian.TabStop = true;
            this.radioButtonMedian.Text = "Медианная фильтрация";
            this.radioButtonMedian.UseVisualStyleBackColor = true;
            this.radioButtonMedian.CheckedChanged += new System.EventHandler(this.radioButtonMedian_CheckedChanged);
            // 
            // radioButtonLine
            // 
            this.radioButtonLine.AutoSize = true;
            this.radioButtonLine.Location = new System.Drawing.Point(624, 399);
            this.radioButtonLine.Name = "radioButtonLine";
            this.radioButtonLine.Size = new System.Drawing.Size(188, 24);
            this.radioButtonLine.TabIndex = 7;
            this.radioButtonLine.TabStop = true;
            this.radioButtonLine.Text = "Линейная фильтрация";
            this.radioButtonLine.UseVisualStyleBackColor = true;
            this.radioButtonLine.CheckedChanged += new System.EventHandler(this.radioButtonLine_CheckedChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(633, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(172, 121);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(190, 416);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(140, 38);
            this.bAdd.TabIndex = 9;
            this.bAdd.Text = "Добавить";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // radioButtonGauss
            // 
            this.radioButtonGauss.AutoSize = true;
            this.radioButtonGauss.Location = new System.Drawing.Point(624, 423);
            this.radioButtonGauss.Name = "radioButtonGauss";
            this.radioButtonGauss.Size = new System.Drawing.Size(184, 24);
            this.radioButtonGauss.TabIndex = 10;
            this.radioButtonGauss.TabStop = true;
            this.radioButtonGauss.Text = "Фильтрация по Гауссу";
            this.radioButtonGauss.UseVisualStyleBackColor = true;
            this.radioButtonGauss.CheckedChanged += new System.EventHandler(this.radioButtonGauss_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(634, 348);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "r";
            // 
            // textBoxR
            // 
            this.textBoxR.Location = new System.Drawing.Point(629, 370);
            this.textBoxR.Name = "textBoxR";
            this.textBoxR.Size = new System.Drawing.Size(175, 27);
            this.textBoxR.TabIndex = 11;
            this.textBoxR.TextChanged += new System.EventHandler(this.textBoxR_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 476);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxR);
            this.Controls.Add(this.radioButtonGauss);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.radioButtonLine);
            this.Controls.Add(this.radioButtonMedian);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSigma);
            this.Controls.Add(this.textBoxMatrix);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.bCalculate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCalculate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBoxMatrix;
        private System.Windows.Forms.TextBox textBoxSigma;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButtonMedian;
        private System.Windows.Forms.RadioButton radioButtonLine;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.RadioButton radioButtonGauss;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxR;
    }
}

