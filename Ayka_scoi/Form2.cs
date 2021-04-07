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

namespace Ayka_scoi
{
    public partial class Form2 : Form
    {
        static public int[] PixArray = new int[256];
        public Image Img { get; set; }
        List<Point> Points = new List<Point>();
        Dictionary<int, int> PointsSpline = new Dictionary<int, int>();
        public Bitmap Changer;
        public Bitmap Static;
        public Form2(Bitmap Pb)
        {
            InitializeComponent();
            Img = Pb;
            Static = (Bitmap)Pb.Clone();
            var Panel = new Pan(ref Points);
            Panel.Size = new Size(420, 420);
            Panel.Location = new Point(15, 15);
            Panel.Padding = new Padding(0);
            Panel.BackColor = Color.LightGray;
            this.Controls.Add(Panel);
            Array.Clear(PixArray, 0, PixArray.Length);
        }

        public async void Get_Grafisc(Bitmap Result)
        {
            Changer = Result;
            int maxOrig = Get_Array((Bitmap)Result.Clone());
            Set_ChartOrig(maxOrig);
            if (Points.Count != 2)
            {
                    LetsSpline();
            } 
        }

        
        public static int Get_Array(Bitmap ResultPic)
        {
            Array.Clear(PixArray, 0, PixArray.Length);
            int max = 0;
            BitmapData bmpData = ResultPic.LockBits(new Rectangle(0, 0, ResultPic.Width, ResultPic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, ResultPic.PixelFormat);
            IntPtr ptr1 = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * ResultPic.Height;
            byte[] bgraValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, bgraValues, 0, bytes);
            for (var i = 0; i < bgraValues.Length; i += 4)
            {
                int result = (bgraValues[i] + bgraValues[i + 1] + bgraValues[i + 2]) / 3;
                PixArray[result]++;
                if (PixArray[result] > max) max = PixArray[result];
            }
            System.Runtime.InteropServices.Marshal.Copy(bgraValues, 0, ptr1, bytes);
            ResultPic.UnlockBits(bmpData);
            ResultPic.Dispose();
            return max;
        }

        public async void LetsSpline()
        {
            PointsSpline.Clear();
            double B = 0;

            double h = Points[1].X - Points[0].X;
            double f = (Points[1].Y - Points[0].Y) / h;
            double a = (1 / (h * h)) * (Points[1].Y - Points[0].Y) - (1 / h) * f;
            double b = f - 2 * a * Points[0].X;
            double c = Points[0].Y - f * Points[0].X + a * Points[0].X * Points[0].X;

            CalculateSpline(h, a, b, c, Points[0]);

            for (int i = 1; i < Points.Count - 1; ++i)
            {
                h = Points[i + 1].X - Points[i].X;
                B = 2 * a * Points[i].X + b;
                a = (1 / (h * h)) * (Points[i + 1].Y - Points[i].Y) - (1 / h) * B;
                b = B - 2 * a * Points[i].X;
                c = Points[i].Y - B * Points[i].X + a * Points[i].X * Points[i].X;

                CalculateSpline(h, a, b, c, Points[i]);
            }

            var pan = (Pan)GetChildAtPoint(new Point(15, 15));
            pan.DrawSpline(PointsSpline);

            Changer.Dispose();
            Changer = (Bitmap)Static.Clone();
            Bitmap tmp = await Task.Run(() => ChangePicAsync((Bitmap)Changer.Clone()));
            Changer.Dispose();
            Changer = tmp;

            int maxChange = Get_Array((Bitmap)Changer.Clone());
            Set_ChartChange(maxChange);
        }

        Bitmap ChangePicAsync(Bitmap tmp)
        {
            double k = 419.0 / 255;
            BitmapData bmpData = tmp.LockBits(new Rectangle(0, 0, Changer.Width, Changer.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Changer.PixelFormat);
            IntPtr ptr1 = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * Changer.Height;
            
            byte[] bgraValues = new byte[bytes];
            
            System.Runtime.InteropServices.Marshal.Copy(ptr1, bgraValues, 0, bytes);
            byte[] value2 = (byte[])bgraValues.Clone();
            for (var i = 0; i < bgraValues.Length; i += 4)
            {
                double index = bgraValues[i] * k;
                bgraValues[i] = (byte)((419 - PointsSpline[(int)Math.Round(index, 0)]) / k); 
                
                index = bgraValues[i + 1] * k;
                bgraValues[i + 1] = (byte)((419 - PointsSpline[(int)Math.Round(index, 0)]) / k);

                index = bgraValues[i + 2] * k;
                bgraValues[i + 2] = (byte)((419 - PointsSpline[(int)Math.Round(index, 0)]) / k);
            }

            System.Runtime.InteropServices.Marshal.Copy(bgraValues, 0, ptr1, bytes);
            tmp.UnlockBits(bmpData);
            Img = tmp;
            return tmp;
        }

        void CalculateSpline(double h, double a, double b, double c, Point tmp)
        {
            
            for (int j = 0; j < h; j ++)
            {
                int X = tmp.X + j;
                int Y = (int)(a * X * X + b * X + c);
                X = X > 420 ? 420 : X;
                X = X < 0 ? 0 : X;
                Y = Y > 420 ? 420 : Y;
                Y = Y < 0 ? 0 : Y;
                PointsSpline.Add(X, Y);
            }
        }

        void Set_ChartOrig(int max)
        {
            try
            {
                for (int i = 0; i < PixArray.Length; ++i)
                {
                    this.chartOrig.Series[0].Points.AddXY(i, PixArray[i]);
                }
                this.chartOrig.ChartAreas[0].AxisY.Maximum = max;
            }
            catch (NullReferenceException)
            {

            }
        }
        void Set_ChartChange(int max)
        {
            try {
                for (int i = 0; i < PixArray.Length; ++i)
                {
                    this.chartChange.Series[0].Points.AddXY(i, PixArray[i]);
                }
                this.chartChange.ChartAreas[0].AxisY.Maximum = max;
            }
            catch(NullReferenceException e)
            {
                return;
            }
        }
    }
}
