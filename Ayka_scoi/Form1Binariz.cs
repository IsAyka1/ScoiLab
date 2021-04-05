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
            var bmp = (Bitmap)ResultPic.Image;
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
                        LocalBinar(bgraValues, bmp.Width, bmp.Height);
                        break;
                    }
                case EBINAR.Sauvola:
                    {
                        LocalBinar(bgraValues, bmp.Width, bmp.Height);
                        break;
                    }
                case EBINAR.BredliRot:
                    {
                        LocalBinar(bgraValues, bmp.Width, bmp.Height);
                        break;
                    }
                case EBINAR.Wulf:
                    {
                        LocalBinar(bgraValues, bmp.Width, bmp.Height);
                        break;
                    }
            }
            System.Runtime.InteropServices.Marshal.Copy(bgraValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
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

        void LocalBinar(byte[] bgraValues, int w, int h)
        {
            int a = 15;
            double koef = -0.2;
            int[] squard = new int[a*a];
            int[] t = new int[bgraValues.Length / 4];
            Form2.Get_Array((Bitmap)ResultPic.Image.Clone());
            double[] Gisto = new double[Form2.PixArray.Length];
            int min = 257;
            int minIndex = -1;
            for (int i = 0; i < Gisto.Length; ++i)
            {
                Gisto[i] = (double)Form2.PixArray[i];
                if(Gisto[i] < min)
                {
                    min = (int)Gisto[i];
                    minIndex = i;
                }
            }
            for (int y = 0; y < h; y ++)
            {
                for (int x = 0; x < w; x++)
                {
                    Array.Clear(squard, 0, squard.Length);
                    var _x = x - a / 2;
                    var _y = y - a / 2;
                    for(int ky = 0; ky < a; ++ky)
                    {
                        for (int kx = 0; kx < a; ++kx)
                        {
                            squard[ky * a + kx] = GetPic(_x + kx, _y + ky, w, h, bgraValues);
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
                                Niblek(ref t, y * w + x, M, koef, q);
                                break;
                            }
                        case EBINAR.Sauvola:
                            {
                                Sauvola(ref t, y * w + x, M, koef, q);
                                break;
                            }
                        case EBINAR.Wulf:
                            {
                                Wulf(ref t, y * w + x, M, minIndex, q);
                                break;
                            }
                    }
                }
            }
            for(int i = 0; i < bgraValues.Length; i += 4)
            {
                byte BiValue = (byte)GetBiValue(bgraValues[i], t[i / 4]);
                bgraValues[i + 0] = BiValue;
                bgraValues[i + 1] = BiValue;
                bgraValues[i + 2] = BiValue;
            }
        }

        void Niblek(ref int[] t, int i, int M, double k, double q)
        {
            t[i] = (int)Math.Round(M + k * q, 0);
        }

        int GetPic(int x, int y, int w, int h, byte[] bgraValues)
        {
            if(x < 0 || x >= w || y < 0 || y >= h)
            {
                return 0;
            }
            return bgraValues[(y * w + x) * 4 + 1];
        }

        int GetM(int[] arr)
        {
            var Sum = 0;
            for(int i = 0; i < arr.Length; ++i)
            {
                Sum += arr[i];
            }
            return Sum / arr.Length;
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

        void Sauvola(ref int[] t, int i, int M, double k, double q)
        {
            int R = 128;
            t[i] = M * (int)Math.Round(1 + k * (q / R - 1), 0);
        }
        void Wulf(ref int[] t, int i, int min, double M, double q)
        {
            double a = 0.5;
            int R = 128;
            t[i] = (int)(Math.Round((1 - a) * M + a * min + a * (q / R) * (M - min), 0));
        }
        void BredliRot(byte[] bgraValues)
        {

        }
    }
}
