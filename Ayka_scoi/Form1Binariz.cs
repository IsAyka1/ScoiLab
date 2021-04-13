using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayka_scoi
{
    public partial class Form1
    {
        void GetBinariz()
        {
            var bmp = new Bitmap(ResultPic.Image);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] bgraValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, bgraValues, 0, bytes);

            GetMonochrom(bgraValues);
            switch (EBinar)
            {
                case EBINAR.Gavrilov:
                    {
                        Gavrilov(bgraValues);
                        break;
                    }
                case EBINAR.Otsu:
                    {
                        Otsu(bgraValues);
                        break;
                    }
                case EBINAR.Niblek:
                    {
                        //LocalBinar(bgraValues, bmp.Width, bmp.Height);
                        LocalBinarOptimyz(bgraValues, bmp.Height, bmp.Width);
                        break;
                    }
                case EBINAR.Sauvola:
                    {
                        LocalBinarOptimyz(bgraValues, bmp.Height, bmp.Width);
                        break;
                    }
                case EBINAR.BredliRot:
                    {
                        LocalBinarOptimyz(bgraValues, bmp.Height, bmp.Width);
                        break;
                    }
                case EBINAR.Wulf:
                    {
                        Form2.Get_Array((Bitmap)ResultPic.Image.Clone());
                        int[] Gisto = new int[Form2.PixArray.Length];
                        int minIndex = 0;
                        #region poisk
                        //for (int i = 0; i < Gisto.Length; ++i)
                        //{
                        //    Gisto[i] = Form2.PixArray[i];
                        //    if (Gisto[i] <= min)
                        //    {
                        //        min = Gisto[i];
                        //        minIndex = i;
                        //    }
                        //}
                        //for(int i = Gisto.Length - 1; i >= 0; i--)
                        //{
                        //    Gisto[i] = Form2.PixArray[i];
                        //    if (Gisto[i] != 0)
                        //    {
                        //        minIndex = i;
                        //        //min = Gisto[i];
                        //        break;
                        //    }
                        //}
                        #endregion
                        for (int i = 0; i < Gisto.Length; i++)
                        {
                            Gisto[i] = Form2.PixArray[i];
                            if (Gisto[i] != 0)
                            {
                                minIndex = i;
                                break;
                            }
                        }
                        LocalBinarOptimyz(bgraValues, bmp.Height, bmp.Width, minIndex);
                        break;
                    }
            }
            System.Runtime.InteropServices.Marshal.Copy(bgraValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            ResultPic.Image.Dispose();
            ResultPic.Image = bmp;
        }

        void GetMonochrom(byte[] bgraValues)
        {
            for(int i = 0; i < bgraValues.Length; i += 4)
            {
                int result = (bgraValues[i] + bgraValues[i + 1] + bgraValues[i + 2]) / 3;
                bgraValues[i] = (byte)result;
                bgraValues[i + 1] = (byte)result;
                bgraValues[i + 2] = (byte)result;
            }
        }

        int GetBiValue(int a, int t)
        {
            a = a <= t ? 0 : 255;
            return a;
        }

        void LetsBinariz(byte[] bgraValues, int t)
        {
            for (int i = 0; i < bgraValues.Length; i += 4)
            {
                bgraValues[i] = (byte)GetBiValue(bgraValues[i], t);
                bgraValues[i + 1] = (byte)GetBiValue(bgraValues[i + 1], t);
                bgraValues[i + 2] = (byte)GetBiValue(bgraValues[i + 2], t);
            }
        }

        void Gavrilov(byte[] bgraValues)
        {
            long Sum = 0;
            for (int i = 0; i < bgraValues.Length; i += 4)
            {
                Sum += bgraValues[i];
            }
            int t = (int)(Sum / (bgraValues.Length / 4));

            LetsBinariz(bgraValues, t);
        }
        void Otsu(byte[] bgraValues)
        {
            Form2.Get_Array((Bitmap)ResultPic.Image.Clone());
            double[] Gisto = new double[Form2.PixArray.Length];
            for(int i = 0; i < Gisto.Length; ++i)
            {
                Gisto[i] = (double)Form2.PixArray[i];
            }
            for(int i =0; i < Gisto.Length; ++i)
            {
                Gisto[i] = Gisto[i] / (bgraValues.Length / 4); //Normalyz
            }

            double w1 = 0;
            double w2 = 0;
            double m1 = 0;
            double m2 = 0;
            double mT = 0;
            double qB = 0;
            double MaxQB = 0;
            int t = 0;
            for (int i = 1; i < 256; ++i)
            {
                w1 = Sum(i - 1, Gisto);
                w2 = 1 - w1;
                m1 = Sum(Gisto, i - 1) / w1;
                mT = Sum(Gisto, 255);
                m2 = (mT - m1 * w1) / w2;
                int m = (int)(m1 - m2);
                qB = w1 * w2 * m * m;
                if(qB > MaxQB)
                {
                    MaxQB = qB;
                    t = i;
                }
            }
            LetsBinariz(bgraValues, t);
        }

        double Sum(int max, double[] arr)
        {
            double Sum = 0;
            for(int i = 0; i <= max; ++i)
            {
                Sum += arr[i];
            }
            return Sum;
        }

        double Sum(double[] arr, int max)
        {
            double Sum = 0;
            for (int i = 0; i <= max; ++i)
            {
                Sum += i * arr[i];
            }
            return Sum;
        }

        void LocalBinar(byte[] bgraValues, int w, int h, double Maxq = 0, int min = -1)
        {
            int a = 15;
            int[] squard = new int[a * a];
            int[] t = new int[bgraValues.Length / 4];
            //Form2.Get_Array((Bitmap)ResultPic.Image.Clone());
            //double[] Gisto = new double[Form2.PixArray.Length];
            //int min = 257;
            //int minIndex = -1;
            //double Maxq = await Task.Run(() => GetMaxq((byte[])bgraValues.Clone(), w, h, a));
            //for (int i = 0; i < Gisto.Length; ++i)
            //{
            //    Gisto[i] = (double)Form2.PixArray[i];
            //    if (Gisto[i] < min)
            //    {
            //        min = (int)Gisto[i];
            //        minIndex = i;
            //    }
            //}
            //TODO M&D in arr

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Array.Clear(squard, 0, squard.Length);
                    var _x = x - a / 2;
                    var _y = y - a / 2;
                    for (int ky = 0; ky < a; ++ky)
                    {
                        for (int kx = 0; kx < a; ++kx)
                        {
                            squard[ky * a + kx] = GetPix(_x + kx, _y + ky, w, h, bgraValues);
                        }
                    }
                    var M = GetM(squard);
                    var M2 = GetM2(squard);
                    var D = M2 - (M * M);
                    var q = Math.Sqrt(D);
                    switch (EBinar)
                    {
                        case EBINAR.Niblek:
                            {
                               t[y * w + x] =  Niblek(M, q, a);
                                break;
                            }
                        case EBINAR.Sauvola:
                            {
                                //Sauvola(ref t, y * w + x, M, q);
                                break;
                            }
                        case EBINAR.Wulf:
                            {
                                //Wulf(ref t, y * w + x, M, min, q, Maxq);
                                break;
                            }
                    }//TODO менять сразу t/ не масив ??
                }
            }
            for (int i = 0; i < bgraValues.Length; i += 4)
            {
                byte BiValue = (byte)GetBiValue(bgraValues[i], t[i / 4]);
                bgraValues[i + 0] = BiValue;
                bgraValues[i + 1] = BiValue;
                bgraValues[i + 2] = BiValue;
            }
        }

        void LocalBinarOptimyz(byte[] bgraValues, int h, int w, int min = -1)
        {
            int a = 15;
            double k = 0.15; //rote
            int[] t = new int[bgraValues.Length / 4];
            long[,] M = new long[h, w];
            long[,] M2 = new long[h, w];
            
            GetIntegralM(ref M, ref M2, h, w, bgraValues);
            var Maxq = min != -1 ? GetMaxq(M, M2, h, w, a) : 0;
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    var m = Rote(M, x, y, h, w, a);
                    var m2 = Rote(M2, x, y, h, w, a);
                    var d = m2 - m * m;
                    var q = Math.Sqrt(d);
                    switch (EBinar)
                    {
                        case EBINAR.Niblek:
                            {
                                t[y * w + x] = Niblek(m, q);
                                break;
                            }
                        case EBINAR.Sauvola:
                            {
                                t[y * w + x] = Sauvola(m, q);
                                break;
                            }
                        case EBINAR.Wulf:
                            {
                                t[y * w + x] = Wulf(m, min, q, Maxq);
                                break;
                            }
                        case EBINAR.BredliRot:
                            {
                                t[y * w + x] = (int)(Rote(M, x, y, h, w, a) * (1 - k));
                                break;
                            }
                    }
                    byte BiValue = (byte)GetBiValue(bgraValues[(y * w + x) * 4], t[y * w + x]);
                    bgraValues[(y * w + x) * 4 + 0] = BiValue;
                    bgraValues[(y * w + x) * 4 + 1] = BiValue;
                    bgraValues[(y * w + x) * 4 + 2] = BiValue;
                }
            }
        }

        double GetMaxq(Bitmap bmp, int w, int h, int a)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] bgraValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, bgraValues, 0, bytes);

            int[] squard = new int[a * a];
            double Maxq = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Array.Clear(squard, 0, squard.Length);
                    var _x = x - a / 2;
                    var _y = y - a / 2;
                    for (int ky = 0; ky < a; ++ky)
                    {
                        for (int kx = 0; kx < a; ++kx)
                        {
                            squard[ky * a + kx] = GetPix(_x + kx, _y + ky, w, h, bgraValues);
                        }
                    }
                    var M = GetM(squard);
                    var M2 = GetM2(squard);
                    var D = M2 - (M * M);
                    var q = Math.Sqrt(D);
                    Maxq = q > Maxq ? q : Maxq;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(bgraValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            return Maxq;
        }

        double GetMaxq(long[,] M, long[,] M2, int h, int w, int a)
        {
            double Maxq = -1;
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    var m = Rote(M, x, y, h, w, a);
                    var m2 = Rote(M2, x, y, h, w, a);
                    var d = m2 - m * m;
                    var q = Math.Sqrt(d);
                    Maxq = q > Maxq ? q : Maxq;
                }
            }
            return Maxq;
        }

        int Niblek(double M, double q,double k = -0.2)
        {
            return (int)Math.Round(M + k * q, 0);
        }

        int GetPix(int x, int y, int w, int h, byte[] bgraValues)
        {
            if(x < 0 || x >= w || y < 0 || y >= h)
            {
                return 0;
            }
            return bgraValues[(y * w + x) * 4];
        }

        void GetIntegralM(ref long[,] m, ref long[,] m2, int h, int w, byte[] bgraValue)
        {
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    var i = GetPix(x, y, w, h, bgraValue);
                    var s1 = x - 1 < 0 || y < 0 || x - 1 >= w || y >= h ? 0 : m[y, x - 1];
                    var s2 = x < 0 || y - 1 < 0 || x >= w || y - 1 >= h ? 0 : m[y - 1, x];
                    var s3 = x - 1 < 0 || y - 1 < 0 || x - 1 >= w || y - 1 >= h ? 0 : m[y - 1, x - 1];
                    m[y, x] = i + s1 + s2 - s3;
                    s1 = x - 1 < 0 || y < 0 || x - 1 >= w || y >= h ? 0 : m2[y, x - 1];
                    s2 = x < 0 || y - 1 < 0 || x >= w || y - 1 >= h ? 0 : m2[y - 1, x];
                    s3 = x - 1 < 0 || y - 1 < 0 || x - 1 >= w || y - 1 >= h ? 0 : m2[y - 1, x - 1];
                    m2[y, x] = i * i + s1 + s2 - s3;
                }
            }

        }
        int Rote(long[,] s, int x, int y, int hei, int wid, int a)
        {
            int h = a / 2;
            var y1 = (y - h - 1 < 0) || (x - h - 1 < 0) || (y - h - 1 >= hei) || (x - h - 1 >= wid) ? 0 : s[y - h - 1, x - h - 1]; //done
            var y2 = (y - h - 1 < 0) || (x + h < 0) || (y - h - 1 >= hei) || (x + h >= wid) ? 0 : s[y - h - 1, x + h]; //done
            var x1 = (y + h < 0) || (x - h - 1 < 0) || (y + h >= hei) || (x - h - 1 >= wid) ? 0 : s[y + h, x - h - 1]; //done
            var x2 = (y + h < 0) || (x + h < 0) || (y + h >= hei) || (x + h >= wid) ? 0 : s[y + h, x + h]; //done
            return (int)(x2 + y1 - x1 - y2) / (a * a);
        }

        int GetM2(int[] arr)
        {
            var Sum = 0;
            for (int i = 0; i < arr.Length; ++i)
            {
                Sum += arr[i] * arr[i];
            }
            return Sum / arr.Length;
        }

        int GetM(int[] arr)
        {
            var Sum = 0;
            for (int i = 0; i < arr.Length; ++i)
            {
                Sum += arr[i];
            }
            return Sum / arr.Length;
        }

        int Sauvola(double M, double q, double k = 0.2)
        {
            int R = 128;
            //return (int)(M * Math.Round(1 + k * ((q / R) - 1), 0));
            //return (int)Math.Round(M * 1 + k * ((q / R) - 1), 0);
            //return (int)(M * (int)(1 + k * ((q / R) - 1)));
            return (int)(M * (1 + k * ((q / R) - 1)));
        }
        int Wulf(double M, double min, double q, double R)
        {
            double a = 0.5;
            return (int)(Math.Round((1 - a) * M  + a * min + Math.Abs(a * q * (M - min) / R) , 0));
        }
    }
}
