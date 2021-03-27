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
        private int[] PixArray;
        public Form2()
        {
            InitializeComponent();
            var Panel = new Pan();
            Panel.Size = new Size(420, 420);
            Panel.Location = new Point(15, 15);
            Panel.BackColor = Color.Black;
            PixArray = new int[256];
            for(int i = 0; i < PixArray.Length; ++i) {
                PixArray[i] = 0;
            }
        }
        
        public void Get_Grafisc(Bitmap ResultPic)
        {
            int max = Get_Array((Bitmap)ResultPic.Clone());
            Set_ChartOrig(max);
        }

        public bool ThumbnailCallback()
        {
            return false;
        }
        int Get_Array(Bitmap ResultPic)
        {
            int max = 0;
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            var bmp = (Bitmap)ResultPic.GetThumbnailImage(ResultPic.Width, ResultPic.Height, myCallback, IntPtr.Zero);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, ResultPic.Width, ResultPic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, ResultPic.PixelFormat);
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
            bmp.UnlockBits(bmpData);
            ResultPic.Dispose();
            return max;
        }

        void Set_ChartOrig(int max)
        {
            for (int i = 0; i < PixArray.Length; ++i)
            {
                this.chartOrig.Series[0].Points.AddXY(i, PixArray[i]);
            }
            this.chartOrig.ChartAreas[0].AxisY.Maximum = max;
        }
    }
}
