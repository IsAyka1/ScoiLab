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
using System.Linq;
using System.Numerics;

namespace Scoi5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap Picture;
        uint W = 0;
        uint H = 0;
        int MFure = 0;
        int MTrueFilter = 0;
        int MFalseFilter = 0;
        List<Params> ListXYRR = new List<Params>();
        

        private async void bCalculate_Click(object sender, EventArgs e)
        {
            if(GetMFure() && GetMTrueFilter() && GetMFalseFilter() && GetParams() && Picture != null)
            {
                var bmp = new Bitmap((Image)Picture.Clone());
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
                IntPtr ptr = bmpData.Scan0;
                int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
                byte[] bgraValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr, bgraValues, 0, bytes);

                Bitmap bmpFure;
                if (pictureBoxFure.Image == null)
                {
                    bmpFure = new Bitmap(pictureBoxFure.Width, pictureBoxFure.Height);
                }
                else
                {
                    bmpFure = (Bitmap)pictureBoxFure.Image.Clone();
                }
                BitmapData bmpDataFure = bmpFure.LockBits(new Rectangle(0, 0, bmpFure.Width, bmpFure.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmpFure.PixelFormat);
                IntPtr ptrFure = bmpDataFure.Scan0;
                int bytesFure = Math.Abs(bmpDataFure.Stride) * bmpFure.Height;
                byte[] bgraValuesFure = new byte[bytesFure];
                System.Runtime.InteropServices.Marshal.Copy(ptrFure, bgraValuesFure, 0, bytesFure);

                Task t1 = Task.Run(() => UnFure(Fure(bgraValues, 0, bgraValuesFure), ref bgraValues, 0));
                Task t2 = Task.Run(() => UnFure(Fure(bgraValues, 1, bgraValuesFure), ref bgraValues, 1));
                Task t3 = Task.Run(() => UnFure(Fure(bgraValues, 2, bgraValuesFure), ref bgraValues, 2));

                await Task.WhenAll(new[] { t1, t2, t3 });

                System.Runtime.InteropServices.Marshal.Copy(bgraValuesFure, 0, ptrFure, bytesFure);
                bmpFure.UnlockBits(bmpDataFure);
                if (pictureBoxFure.Image != null)
                    pictureBoxFure.Image.Dispose();
                pictureBoxFure.Image = bmpFure;
                pictureBoxFure.Paint += p_event;

                System.Runtime.InteropServices.Marshal.Copy(bgraValues, 0, ptr, bytes);
                bmp.UnlockBits(bmpData);
                if (pictureBoxNewPic.Image != null)
                    pictureBoxNewPic.Image.Dispose();
                pictureBoxNewPic.Image = bmp;
            }
        }

        void UnFure(Complex[] Arr, ref byte[] bgraValues, int channel)
        {
            Complex[] complexArr = new Complex[W];

            for (int y = 0; y < H; ++y)
            {
                for (int x = 0; x < W; ++x)
                {
                    complexArr[x] = new Complex(Arr[GetIndexArr(x, y, (int)W)].Real, -Arr[GetIndexArr(x, y, (int)W)].Imaginary);// * Math.Pow(-1, x + y);
                }
                complexArr = DTF(complexArr);
                for (int i = 0; i < W; ++i)
                {
                    Arr[y * W + i] = complexArr[i];// * Math.Pow(-1, y + i);
                }
            }
            Array.Clear(complexArr, 0, complexArr.Length);

            complexArr = new Complex[H];
            for (int x = 0; x < W; ++x)
            {
                for (int y = 0; y < H; ++y)
                {
                    complexArr[y] = new Complex(Arr[GetIndexArr(x, y, (int)W)].Real, -Arr[GetIndexArr(x, y, (int)W)].Imaginary);
                }
                complexArr = DTF(complexArr);
                for (int i = 0; i < H; ++i)
                {
                    Arr[GetIndexArr(x, i, (int)W)] = complexArr[i];// * Math.Pow(-1, x + i);
                }
            }
            GetNewPixels(Arr, ref bgraValues, channel); 
        }

        void GetNewPixels(Complex[] Arr, ref byte[] bgraValues, int channel)
        {
            for(int i = 0; i < bgraValues.Length; i += 4)
            {
                bgraValues[i + channel] = (byte)Arr[i / 4].Magnitude;
            }
        }

        Complex[] Fure(byte[] bgraValues, int channel, byte[] bgraValuesFure)
        {
            //byte[] -> Complex[] -> new Complex[] -> old byte[]
            Complex[] complexArr = new Complex[W];
            Complex[] complexFureArr = new Complex[W * H];
            for (int y = 0; y < H; ++y)
            {
                for (int x = 0; x < W; ++x)
                {
                    complexArr[x] = (new Complex(bgraValues[GetIndexByte(x, y, channel)] * Math.Pow(-1, x + y), 0));
                }
                complexArr = DTF(complexArr, 1.0 / W);
                for(int i = 0; i < W; ++i)
                {
                    complexFureArr[y * W + i] = complexArr[i];
                }
            }
            Array.Clear(complexArr, 0, complexArr.Length);

            complexArr = new Complex[H];
            double m = 0;
            for (int x = 0; x < W; ++x)
            {
                for (int y = 0; y < H; ++y)
                {
                    complexArr[y] = complexFureArr[GetIndexArr(x, y, (int)W)];
                }
                complexArr = DTF(complexArr, 1.0 / H);
                for (int i = 0; i < H; ++i)
                {
                    complexFureArr[GetIndexArr(x, i, (int)W)] = complexArr[i];
                    m = Math.Log(complexArr[i].Magnitude + 1) > m ? Math.Log(complexArr[i].Magnitude + 1) : m;
                }
            }
            GetFurePicture(ref complexFureArr, m, channel, bgraValuesFure);
            return complexFureArr;
        }

        void GetFurePicture(ref Complex[] arr, double m, int channel, byte[] bgraValuesFure)
        {
            for(int i = 0; i < bgraValuesFure.Length; i += 4)
            {
                double res = Math.Log(arr[i / 4].Magnitude + 1) * m / 255;
                res *= MFure * 1000;
                res = res > 255 ? 255 : Math.Round(res, 0);
                bgraValuesFure[i + channel] = (byte)res;
                bgraValuesFure[i + 3] = 255;
                
                var x = (i / 4) % W;
                var y = (i / 4) / W;
                foreach (var curcle in ListXYRR) {
                    if (curcle.r1 == 0) continue;
                    if (Math.Pow(x - curcle.x, 2) + Math.Pow(y - curcle.y, 2) < Math.Pow(curcle.r1, 2) && Math.Pow(x - curcle.x, 2) + Math.Pow(y - curcle.y, 2) > Math.Pow(curcle.r2, 2))
                    {
                        arr[i / 4] *= MTrueFilter * 1;
                    } else
                    {
                        arr[i / 4] *= MFalseFilter;
                    }
                }
            }
        }

        public void p_event(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            foreach (var curcle in ListXYRR)
            {
                var x = curcle.x - curcle.r1;
                var y = curcle.y - curcle.r1;
                e.Graphics.DrawEllipse(new Pen(Color.Red), new RectangleF(x, y, curcle.r1 * 2, curcle.r1 * 2));
                x = curcle.x - curcle.r2;
                y = curcle.y - curcle.r2;
                e.Graphics.DrawEllipse(new Pen(Color.Red), new RectangleF(x, y, curcle.r2 * 2, curcle.r2 * 2));
            }
        }

        Complex[] DTF(Complex[] inputList, double n = 1)
        {
            int N = inputList.Length;
            Complex[] G = new Complex[N];
            for (int u = 0; u < N; ++u)
            {
                for (int k = 0; k < N; ++k)
                {
                    double fi = -2.0 * Math.PI * u * k / N;
                    G[u] += new Complex(Math.Cos(fi), Math.Sin(fi)) * inputList[k];
                }
                G[u] *= n;
            }
            return G;
        }

        int GetIndexArr(int x, int y, int WH)
        {
            return (int)(y * WH + x);
        }

        int GetIndexByte(int x, int y, int channel)
        {
            return (int)((y * W + x) * 4 + channel);
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            Bitmap Img = null;

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.JPG;*.PNG)|*.JPG;*.PNG|All files (*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Img = new Bitmap(Image.FromFile(fileDialog.FileName), new Size(pictureBoxOriginal.Width, pictureBoxOriginal.Height));
                    pictureBoxOriginal.Image = (Bitmap)Img.Clone();
                    Img.Dispose();
                    Picture = new Bitmap(Image.FromFile(fileDialog.FileName), new Size(pictureBoxNewPic.Width, pictureBoxNewPic.Height));
                    W = (uint)Picture.Width;
                    H = (uint)Picture.Height;
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

        private bool GetMFure()
        {
            try
            {
                MFure = UInt16.Parse(textBoxMFure.Text);
                return true;
            }
            catch
            {
                MessageBox.Show("Неправильный множитель Фурье", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool GetMTrueFilter()
        {
            try
            {
                MTrueFilter = UInt16.Parse(textBoxMTrueFilter.Text);
                return true;
            }
            catch
            {
                MessageBox.Show("Неправильное значение коэффициента для удовлетворяющих фильтру", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool GetMFalseFilter()
        {
            try
            {
                MFalseFilter = UInt16.Parse(textBoxMFalseFilter.Text);
                return true;
            }
            catch
            {
                MessageBox.Show("Неправильное значение коэффициента для неудовлетворяющих фильтру", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool GetParams()
        {
            try
            {
                ListXYRR.Clear();
                string[] rows = textBoxParams.Text.Split("\r\n");
                int rowCount = rows.Length;
                if (rows[rows.Length - 1] == "")
                    rowCount--;
                for (int i = 0; i < rowCount; ++i)
                {
                    string[] ParamString = rows[i].Split(";");
                    if (ParamString.Length != 5 && ParamString.Length != 4)
                    {
                        throw new Exception();
                    }
                    
                    int x = UInt16.Parse(ParamString[0]);
                    int y = UInt16.Parse(ParamString[1]);
                    int r1 = UInt16.Parse(ParamString[2]);
                    int r2 = UInt16.Parse(ParamString[3]);
                    if (r1 < r2)
                    {
                        throw new Exception();
                    }
                    ListXYRR.Add(new Params(x, y, r1, r2));
                }
                return true;
            }
            catch
            {
                MessageBox.Show("Неправильные значения параметров, проверте формат записи", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
