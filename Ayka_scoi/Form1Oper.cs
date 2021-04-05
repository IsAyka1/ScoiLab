using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ayka_scoi
{
    public partial class Form1 : Form
    {
        Bitmap GetVisible(Layer elem)
        {
            double a = (100 - elem.Visible) / 100.0;
            Bitmap picture = (Bitmap)elem.Img.Clone();
            if (elem.Visible == 100)
            {
                return picture;
            }

            for (var i = 0; i < picture.Width; i++)
            {
                for (var j = 0; j < picture.Height; j++)
                {
                    var pix = picture.GetPixel(i, j);
                    pix = Color.FromArgb((int)((255 - pix.R) * a + pix.R), (int)((255 - pix.G) * a + pix.G), (int)((255 - pix.B) * a + pix.B));
                    picture.SetPixel(i, j, pix);
                }
            }
            var w = (PictureBox)elem.plane.GetChildAtPoint(new Point(100, 100));
            w.Image = (Image)picture.Clone();

            return picture;
        }

        int Normalize(int result)
        {
            if (result > 255)
            {
                return 255;
            }
            else if (result < 0)
            {
                return 0;
            }
            else
                return result;
        }

        void No(ECHANEL chanel, byte[] bgraValues, ref byte[] bgraValues1)
        {

            for (var i = 0; i < bgraValues1.Length; i += 4)
            {
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        bgraValues1[i] = bgraValues[i];
                        bgraValues1[i + 1] = bgraValues[i + 1];
                        bgraValues1[i + 2] = bgraValues[i + 2];
                        break;
                    case ECHANEL.RG:
                        bgraValues1[i + 1] = bgraValues[i + 1];
                        bgraValues1[i + 2] = bgraValues[i + 2];
                        break;
                    case ECHANEL.RB:
                        bgraValues1[i] = bgraValues[i];
                        bgraValues1[i + 2] = bgraValues[i + 2];
                        break;
                    case ECHANEL.GB:
                        bgraValues1[i] = bgraValues[i];
                        bgraValues1[i + 1] = bgraValues[i + 1];
                        break;
                    case ECHANEL.R:
                        bgraValues1[i + 2] = bgraValues[i + 2];
                        break;
                    case ECHANEL.G:
                        bgraValues1[i + 1] = bgraValues[i + 1];
                        break;
                    case ECHANEL.B:
                        bgraValues1[i] = bgraValues[i];
                        break;
                }
            }
        }
        void Sum(ECHANEL chanel, byte[] bgraValues, ref byte[] bgraValues1)
        {

            for (var i = 0; i < bgraValues1.Length; i += 4)
            {
                int result = 0;
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        result = bgraValues[i] + bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues[i + 1] + bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = bgraValues[i + 2] + bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RG:
                        result = bgraValues[i + 1] + bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = bgraValues[i + 2] + bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RB:
                        result = bgraValues[i] + bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues[i + 2] + bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.GB:
                        result = bgraValues[i] + bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues[i + 1] + bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.R:
                        result = bgraValues[i + 2] + bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.G:
                        result = bgraValues[i + 1] - bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.B:
                        result = bgraValues[i] + bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        break;
                }
            }
        }

        void Difference(ECHANEL chanel, byte[] bgraValues, ref byte[] bgraValues1)
        {

            for (var i = 0; i < bgraValues1.Length; i += 4)
            {
                int result = 0;
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        result = bgraValues1[i] - bgraValues[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues1[i + 1] - bgraValues[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = bgraValues1[i + 2] - bgraValues[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RG:
                        result = bgraValues1[i + 1] - bgraValues[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = bgraValues1[i + 2] - bgraValues[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RB:
                        result = bgraValues1[i] - bgraValues[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues1[i + 2] - bgraValues[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.GB:
                        result = bgraValues1[i] - bgraValues[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues1[i + 1] - bgraValues[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.R:
                        result = bgraValues1[i + 2] - bgraValues[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.G:
                        result = bgraValues1[i + 1] - bgraValues[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.B:
                        result = bgraValues1[i] - bgraValues[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        break;
                }
            }
        }
        void Multy(ECHANEL chanel, int a, byte[] bgraValues, ref byte[] bgraValues1)
        {

            for (var i = 0; i < bgraValues1.Length; i += 4)
            {
                int result = 0;
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        result = bgraValues1[i] * (1 - a) + bgraValues[i] * bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues1[i + 1] * (1 - a) + bgraValues[i + 1] * bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = bgraValues1[i + 2] * (1 - a) + bgraValues[i + 2] * bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RG:
                        result = bgraValues1[i + 1] * (1 - a) + bgraValues[i + 1] * bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = bgraValues1[i + 2] * (1 - a) + bgraValues[i + 2] * bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RB:
                        result = bgraValues1[i] * (1 - a) + bgraValues[i] * bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues1[i + 2] * (1 - a) + bgraValues[i + 2] * bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.GB:
                        result = bgraValues1[i] * (1 - a) + bgraValues[i] * bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues1[i + 1] * (1 - a) + bgraValues[i + 1] * bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.R:
                        result = bgraValues1[i + 2] * (1 - a) + bgraValues[i + 2] * bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.G:
                        result = bgraValues1[i + 1] * (1 - a) + bgraValues[i + 1] * bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.B:
                        result = bgraValues1[i] * (1 - a) + bgraValues[i] * bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        break;
                }
            }
        }
        void Division(ECHANEL chanel, byte[] bgraValues, ref byte[] bgraValues1)
        {

            for (var i = 0; i < bgraValues1.Length; i += 4)
            {
                int result = 0;
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        result = bgraValues[i] / bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues[i + 1] / bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = bgraValues[i + 2] / bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RG:
                        result = bgraValues[i + 1] / bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = bgraValues[i + 2] / bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RB:
                        result = bgraValues[i] / bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues[i + 2] / bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.GB:
                        result = bgraValues[i] / bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        result = bgraValues[i + 1] / bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.R:
                        result = bgraValues[i + 2] / bgraValues1[i + 2];
                        bgraValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.G:
                        result = bgraValues[i + 1] / bgraValues1[i + 1];
                        bgraValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.B:
                        result = bgraValues[i] / bgraValues1[i];
                        bgraValues1[i] = Convert.ToByte(Normalize(result));
                        break;
                }
            }
        }
        void Min(ECHANEL chanel, byte[] bgraValues, ref byte[] bgraValues1)
        {
            for (var i = 0; i < bgraValues1.Length; i += 4)
            {
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        bgraValues1[i] = bgraValues[i] < bgraValues1[i] ? bgraValues[i] : bgraValues1[i];
                        bgraValues1[i + 1] = bgraValues[i + 1] < bgraValues1[i + 1] ? bgraValues[i + 1] : bgraValues1[i + 1];
                        bgraValues1[i + 2] = bgraValues[i + 2] < bgraValues1[i + 2] ? bgraValues[i + 2] : bgraValues1[i + 2];
                        break;
                    case ECHANEL.RG:
                        bgraValues1[i + 1] = bgraValues[i + 1] < bgraValues1[i + 1] ? bgraValues[i + 1] : bgraValues1[i + 1];
                        bgraValues1[i + 2] = bgraValues[i + 2] < bgraValues1[i + 2] ? bgraValues[i + 2] : bgraValues1[i + 2];
                        break;
                    case ECHANEL.RB:
                        bgraValues1[i] = bgraValues[i] < bgraValues1[i] ? bgraValues[i] : bgraValues1[i];
                        bgraValues1[i + 2] = bgraValues[i + 2] < bgraValues1[i + 2] ? bgraValues[i + 2] : bgraValues1[i + 2];
                        break;
                    case ECHANEL.GB:
                        bgraValues1[i] = bgraValues[i] < bgraValues1[i] ? bgraValues[i] : bgraValues1[i];
                        bgraValues1[i + 1] = bgraValues[i + 1] < bgraValues1[i + 1] ? bgraValues[i + 1] : bgraValues1[i + 1];
                        break;
                    case ECHANEL.R:
                        bgraValues1[i + 2] = bgraValues[i + 2] < bgraValues1[i + 2] ? bgraValues[i + 2] : bgraValues1[i + 2];
                        break;
                    case ECHANEL.G:
                        bgraValues1[i + 1] = bgraValues[i + 1] < bgraValues1[i + 1] ? bgraValues[i + 1] : bgraValues1[i + 1];
                        break;
                    case ECHANEL.B:
                        bgraValues1[i] = bgraValues[i] < bgraValues1[i] ? bgraValues[i] : bgraValues1[i];
                        break;
                }
            }
        }
        void Max(ECHANEL chanel, byte[] bgraValues, ref byte[] bgraValues1)
        {
            for (var i = 0; i < bgraValues1.Length; i += 4)
            {
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        bgraValues1[i] = bgraValues[i] > bgraValues1[i] ? bgraValues[i] : bgraValues1[i];
                        bgraValues1[i + 1] = bgraValues[i + 1] > bgraValues1[i + 1] ? bgraValues[i + 1] : bgraValues1[i + 1];
                        bgraValues1[i + 2] = bgraValues[i + 2] > bgraValues1[i + 2] ? bgraValues[i + 2] : bgraValues1[i + 2];
                        break;
                    case ECHANEL.RG:
                        bgraValues1[i + 1] = bgraValues[i + 1] > bgraValues1[i + 1] ? bgraValues[i + 1] : bgraValues1[i + 1];
                        bgraValues1[i + 2] = bgraValues[i + 2] > bgraValues1[i + 2] ? bgraValues[i + 2] : bgraValues1[i + 2];
                        break;
                    case ECHANEL.RB:
                        bgraValues1[i] = bgraValues[i] > bgraValues1[i] ? bgraValues[i] : bgraValues1[i];
                        bgraValues1[i + 2] = bgraValues[i + 2] > bgraValues1[i + 2] ? bgraValues[i + 2] : bgraValues1[i + 2];
                        break;
                    case ECHANEL.GB:
                        bgraValues1[i] = bgraValues[i] > bgraValues1[i] ? bgraValues[i] : bgraValues1[i];
                        bgraValues1[i + 1] = bgraValues[i + 1] > bgraValues1[i + 1] ? bgraValues[i + 1] : bgraValues1[i + 1];
                        break;
                    case ECHANEL.R:
                        bgraValues1[i + 2] = bgraValues[i + 2] > bgraValues1[i + 2] ? bgraValues[i + 2] : bgraValues1[i + 2];
                        break;
                    case ECHANEL.G:
                        bgraValues1[i + 1] = bgraValues[i + 1] > bgraValues1[i + 1] ? bgraValues[i + 1] : bgraValues1[i + 1];
                        break;
                    case ECHANEL.B:
                        bgraValues1[i] = bgraValues[i] > bgraValues1[i] ? bgraValues[i] : bgraValues1[i];
                        break;
                }
            }
        }
    }
}
