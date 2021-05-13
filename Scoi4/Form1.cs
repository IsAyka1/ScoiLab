using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scoi4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap Picture;
        uint w = 0;
        uint h = 0;
        uint Sigma = 0;
        uint R = 0;
        double[,] Matrix;

        enum EFILTR
        {
            No, Line, Gauss, Median
        }
        EFILTR EFiltr = 0;

        private void Form1_Load(object sender, EventArgs e)
        {        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            Bitmap Img = null;

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.JPG;*.PNG)|*.JPG;*.PNG|All files (*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Img = new Bitmap(Image.FromFile(fileDialog.FileName), new Size(pictureBox2.Width, pictureBox2.Height));
                    pictureBox2.Image = (Bitmap)Img.Clone();
                    Img.Dispose();
                    Picture = new Bitmap(Image.FromFile(fileDialog.FileName), new Size(pictureBox1.Width, pictureBox1.Height));
                    w = (uint)Picture.Width;
                    h = (uint)Picture.Height;
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Файл не добавлен", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bCalculate_Click(object sender, EventArgs e)
        {
            if(pictureBox2.Image == null )
            {
                return;
            }
            var bmp = new Bitmap((Image)Picture.Clone());
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] bgraValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, bgraValues, 0, bytes);
            byte[] bgraValuesResult = (byte[])bgraValues.Clone();
            
            switch (EFiltr)
            {
                case EFILTR.Line:
                    {
                        if (!GetMatrix())
                        {
                            MessageBox.Show("Ошибочная матрица", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            System.Runtime.InteropServices.Marshal.Copy(bgraValues, 0, ptr, bytes);
                            bmp.UnlockBits(bmpData);
                            return;
                        }
                        CalculateLine(bgraValues, ref bgraValuesResult);
                        Array.Clear(Matrix, 0, Matrix.Length);
                    } break;
                case EFILTR.Gauss:
                    {
                        if (R != 0 && Sigma != 0)
                        {
                            GetGaussMatrix();
                            CalculateLine(bgraValues, ref bgraValuesResult);
                            Array.Clear(Matrix, 0, Matrix.Length);
                        }
                    } break;
                case EFILTR.Median:
                    {
                        if (R != 0)
                        {
                            CalculateMedian(bgraValues, ref bgraValuesResult);
                        }
                    } break;
            }
            
            System.Runtime.InteropServices.Marshal.Copy(bgraValuesResult, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            if(pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
            pictureBox1.Image = bmp;
        }

        void CalculateMedian(byte[] bgraValues, ref byte[] bgraValuesResult, int channel = 0)
        {
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    bgraValuesResult[GetIndex(x, y, 0)] = (byte)GetMedian(x, y, bgraValues, 0);
                    bgraValuesResult[GetIndex(x, y, 1)] = (byte)GetMedian(x, y, bgraValues, 1);
                    bgraValuesResult[GetIndex(x, y, 2)] = (byte)GetMedian(x, y, bgraValues, 2);
                }
            }
        }

        int GetMedian(int x, int y, byte[] bgraValues, int chanel = 0)
        {
            
            List<int> list = new List<int>();
            for (int i = (int)(x - R), a = 0; i < x + R; ++i, ++a)
            {
                for (int j = (int)(y - R), b = 0; j < y + R; ++j, ++b)
                {
                    list.Add(GetPixel(i, j, bgraValues, chanel));
                }
            }
            list.Sort();
            return list[(int)R];
        }

        void GetGaussMatrix()
        {
            double s = 0;
            double g = 0;

            var sig_sqr = 2.0 * Sigma * Sigma;
            var pi_siq_sqr = sig_sqr * Math.PI;

            Matrix = new double[2 * R + 1, 2 * R + 1];

            for (int i = (int)-R, a = 0; i <= R; ++i, ++a)
            {
                for (int j = (int)-R, b = 0; j <= R; ++j, ++b)
                {
                    g = 1.0 / pi_siq_sqr * Math.Exp(-1.0 * (i * i + j * j) / (sig_sqr));
                    s += g;

                    //std::replace(g_str.begin(), g_str.end(), '.', ',');
                    Matrix[a, b] = g;
                }
            }
        }

        bool GetMatrix()
        {
            string[] rows = textBoxMatrix.Text.Split("\r\n");
            int rowCount = rows.Length;
            if (rows[rows.Length - 1] == "")
                rowCount--;
            int colCount = rows[0].Split(" ").Length;
            if (colCount < 2 || rowCount < 2)
                return false;
            Matrix = new double[rowCount, colCount];
            
            for(int i = 0; i < rowCount; ++i)
            {
                string[] rowValues = rows[i].Split(" ");
                if (rowValues.Length != colCount)
                    return false;
                if(rowCount != colCount || rowCount % 2 != 1 || colCount % 2 != 1)
                {
                    return false;
                }
                for(int j = 0; j < colCount; ++j)
                {
                    try
                    {
                        Matrix[i, j] = GetNum(rowValues[j]);
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        double GetNum(string val)
        {
            try
            {
                return Double.Parse(val);
            }
            catch
            {
                string[] res = val.Split("/");
                if (res.Length != 2)
                    throw new ArgumentException("Неправильная матрица");
                double a = Double.Parse(res[0]);
                double b = Double.Parse(res[1]);
                return a / b;
            }
        }

        void CalculateLine(byte[] bgraValues, ref byte[] bgraValuesResult, int channel = 0)
        {
            for(int y = 0; y < h; ++y)
            {
                for(int x = 0; x < w; ++x)
                {
                    bgraValuesResult[GetIndex(x, y, 0)] = (byte)GetNewPix(x, y, bgraValues, 0);
                    bgraValuesResult[GetIndex(x, y, 1)] = (byte)GetNewPix(x, y, bgraValues, 1);
                    bgraValuesResult[GetIndex(x, y, 2)] = (byte)GetNewPix(x, y, bgraValues, 2);
                }
            }
        }

        int GetNewPix(int x, int y, byte[] bgraValues, int chanel = 0)
        {
            int r = (int)(Math.Sqrt(Matrix.Length) / 2);
            int sum = 0;
            for(int i = x - r, a = 0; i <= x + r; ++i, ++a)
            {
                for(int j = y - r, b = 0; j <= y + r; ++j, ++b)
                {
                    sum += (int)(GetPixel(i, j, bgraValues, chanel) * Matrix[a,b]);
                }
            }
            return sum > 0 ? sum : 0;
        }

        byte GetPixel(int x, int y, byte[] bgraValues, int channel = 0)
        {
            if (x < 0 || x >= w || y < 0 || y >= h)
            {
                return 0;
            }
            return bgraValues[(y * w + x) * 4 + channel];
        }

        int GetIndex(int x, int y, int channel)
        {
            return (int)((y * w + x) * 4 + channel);
        }

        private void radioButtonLine_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButtonLine.Checked)
            {
                EFiltr = EFILTR.Line;
            } 
        }

        private void radioButtonMedian_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMedian.Checked)
            {
                EFiltr = EFILTR.Median;
            }
        }

        private void radioButtonGauss_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonGauss.Checked)
            {
                EFiltr = EFILTR.Gauss;
            }
        }

        private void textBoxR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                R = UInt16.Parse(textBoxR.Text);
            }
            catch
            {
                MessageBox.Show("Неправильное r", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void textBoxSigma_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Sigma = UInt16.Parse(textBoxSigma.Text);
            }
            catch
            {
                MessageBox.Show("Неправильная Сигма", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

    }
}
