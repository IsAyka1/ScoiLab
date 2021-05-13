
namespace Scoi5
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
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.pictureBoxNewPic = new System.Windows.Forms.PictureBox();
            this.pictureBoxFure = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bCalculate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxMFure = new System.Windows.Forms.TextBox();
            this.textBoxMTrueFilter = new System.Windows.Forms.TextBox();
            this.textBoxMFalseFilter = new System.Windows.Forms.TextBox();
            this.textBoxParams = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bAdd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNewPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFure)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.Location = new System.Drawing.Point(730, 32);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(142, 110);
            this.pictureBoxOriginal.TabIndex = 0;
            this.pictureBoxOriginal.TabStop = false;
            // 
            // pictureBoxNewPic
            // 
            this.pictureBoxNewPic.Location = new System.Drawing.Point(12, 32);
            this.pictureBoxNewPic.Name = "pictureBoxNewPic";
            this.pictureBoxNewPic.Size = new System.Drawing.Size(350, 350);
            this.pictureBoxNewPic.TabIndex = 1;
            this.pictureBoxNewPic.TabStop = false;
            // 
            // pictureBoxFure
            // 
            this.pictureBoxFure.Location = new System.Drawing.Point(374, 30);
            this.pictureBoxFure.Name = "pictureBoxFure";
            this.pictureBoxFure.Size = new System.Drawing.Size(350, 350);
            this.pictureBoxFure.TabIndex = 2;
            this.pictureBoxFure.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Отфильтрованное изображение";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(392, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Фурье";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(740, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Оригинал";
            // 
            // bCalculate
            // 
            this.bCalculate.Location = new System.Drawing.Point(730, 250);
            this.bCalculate.Name = "bCalculate";
            this.bCalculate.Size = new System.Drawing.Size(142, 50);
            this.bCalculate.TabIndex = 6;
            this.bCalculate.Text = "Расчитать";
            this.bCalculate.UseVisualStyleBackColor = true;
            this.bCalculate.Click += new System.EventHandler(this.bCalculate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 404);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Множитель Фурье-образа";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 450);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(201, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Удовлетворяет усл фильтра";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 491);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(222, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "Не удовлетворяет усл фильтра";
            // 
            // textBoxMFure
            // 
            this.textBoxMFure.Location = new System.Drawing.Point(250, 397);
            this.textBoxMFure.Name = "textBoxMFure";
            this.textBoxMFure.Size = new System.Drawing.Size(125, 27);
            this.textBoxMFure.TabIndex = 10;
            // 
            // textBoxMTrueFilter
            // 
            this.textBoxMTrueFilter.Location = new System.Drawing.Point(250, 443);
            this.textBoxMTrueFilter.Name = "textBoxMTrueFilter";
            this.textBoxMTrueFilter.Size = new System.Drawing.Size(125, 27);
            this.textBoxMTrueFilter.TabIndex = 11;
            // 
            // textBoxMFalseFilter
            // 
            this.textBoxMFalseFilter.Location = new System.Drawing.Point(250, 484);
            this.textBoxMFalseFilter.Name = "textBoxMFalseFilter";
            this.textBoxMFalseFilter.Size = new System.Drawing.Size(125, 27);
            this.textBoxMFalseFilter.TabIndex = 12;
            // 
            // textBoxParams
            // 
            this.textBoxParams.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxParams.Location = new System.Drawing.Point(392, 386);
            this.textBoxParams.Multiline = true;
            this.textBoxParams.Name = "textBoxParams";
            this.textBoxParams.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxParams.Size = new System.Drawing.Size(248, 63);
            this.textBoxParams.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(392, 452);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(423, 80);
            this.label7.TabIndex = 14;
            this.label7.Text = "Формат строки: х;у;r1;r2;\r\nх,у - координаты центра окружностей , (у идет сверху в" +
    "низ)\r\nr1 - радиус первой окружности фильтра\r\nr2 - радиус второй окружности фильт" +
    "ра\r\n";
            // 
            // bAdd
            // 
            this.bAdd.Location = new System.Drawing.Point(730, 170);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(142, 50);
            this.bAdd.TabIndex = 15;
            this.bAdd.Text = "Добавить";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(646, 386);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(241, 20);
            this.label8.TabIndex = 16;
            this.label8.Text = "Размерность картинки : 350 х 350";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 538);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxParams);
            this.Controls.Add(this.textBoxMFalseFilter);
            this.Controls.Add(this.textBoxMTrueFilter);
            this.Controls.Add(this.textBoxMFure);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bCalculate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxFure);
            this.Controls.Add(this.pictureBoxNewPic);
            this.Controls.Add(this.pictureBoxOriginal);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNewPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFure)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxOriginal;
        private System.Windows.Forms.PictureBox pictureBoxNewPic;
        private System.Windows.Forms.PictureBox pictureBoxFure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bCalculate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxMFure;
        private System.Windows.Forms.TextBox textBoxMTrueFilter;
        private System.Windows.Forms.TextBox textBoxMFalseFilter;
        private System.Windows.Forms.TextBox textBoxParams;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Label label8;
    }
}

