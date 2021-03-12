using Ayka_scoi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace Ayka_scoi
{
    public partial class Form1 : Form
    {
        List<Layer> LayersList = new List<Layer>();
        FlowLayoutPanel PicPanel = new FlowLayoutPanel();
        public Form1()
        {
            InitializeComponent();
            PicPanel.Parent = this;
            PicPanel.Visible = true;
            PicPanel.Location = new Point(510, 10);
            PicPanel.Size = new Size(270, 420);
            PicPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            PicPanel.AutoScroll = true;
            PicPanel.AutoSize = true;
            PicPanel.Dock = DockStyle.Right;
            PicPanel.FlowDirection = FlowDirection.TopDown;
            PicPanel.WrapContents = false;

            this.Controls.Add(PicPanel);

            Button BResult = new Button();
            BResult.BackColor = Color.LightGray;
            BResult.ForeColor = Color.Red;
            BResult.Size = new Size(110, 30);
            BResult.Location = new Point(130, 414);
            BResult.Text = "Посчитать";
            this.Controls.Add(BResult);
            BResult.Click += new EventHandler(Get_Result);

            ResultPic.Image = Image.FromFile("Background.png");
            ResultPic.SizeMode = PictureBoxSizeMode.Zoom;
            ResultPic.Size = new Size(500, 380);
            ResultPic.Location = new Point(10, 10);
            ResultPic.BackColor = Color.White;
            this.Controls.Add(ResultPic);
        }


        private void BAdd_Click(object sender, EventArgs e)
        {
            Bitmap Img = null;
            FlowLayoutPanel NewLayer = NewPic(ref Img);
            if (NewLayer != null)
            {
                //panels.Add(NewLayer, new Layer(Img, null));
                LayersList.Add(new Layer(ref Img, NewLayer));
                PicPanel.Controls.Add(NewLayer);
            }
        }

        PictureBox ResultPic = new PictureBox();


        FlowLayoutPanel NewPic(ref Bitmap ImgC)
        {
            FlowLayoutPanel Layer = new FlowLayoutPanel();
            Layer.Size = new Size(250, 416);
            Layer.Parent = PicPanel;
            Layer.FlowDirection = FlowDirection.TopDown;
            Layer.WrapContents = false;
            Layer.BorderStyle = BorderStyle.Fixed3D;
            PictureBox Pic = new PictureBox();
            Pic.SizeMode = PictureBoxSizeMode.Zoom;
            Pic.Size = new Size(250, 200);
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.JPG;*.PNG)|*.JPG;*.PNG|All files (*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    
                    Pic.Image = new Bitmap(Image.FromFile(fileDialog.FileName), new Size(ResultPic.Image.Width, ResultPic.Image.Height));
                    ImgC = (Bitmap)Pic.Image;
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Pic.Parent = Layer;
                Layer.Controls.Add(Pic);
            } else
            {
                PicPanel.Controls.Remove(Layer);
                MessageBox.Show("Невозможно добавить файл", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            Label Operation = new Label();
            Operation.Text = "Операция:";
            Layer.Controls.Add(Operation);
            ComboBox Oper = new ComboBox();
            Oper.Items.AddRange(new string[]
            { EOPER.No.ToString(), EOPER.Sum.ToString(), EOPER.Difference.ToString(), EOPER.Multy.ToString(), EOPER.Min.ToString(), EOPER.Max.ToString() });
            Oper.DropDownStyle = ComboBoxStyle.DropDownList;
            Oper.SelectedIndex = 0;
            Layer.Controls.Add(Oper);
            Oper.SelectedIndexChanged += new EventHandler(Oper_Select);

            Label Chanel = new Label();
            Chanel.Text = "Канал:";
            Layer.Controls.Add(Chanel);
            ComboBox Rgb = new ComboBox();
            Rgb.Items.AddRange(new string[]
            { ECHANEL.RGB.ToString(), ECHANEL.RG.ToString(), ECHANEL.RB.ToString(), ECHANEL.GB.ToString(), ECHANEL.R.ToString(), ECHANEL.G.ToString(), ECHANEL.B.ToString() });
            Rgb.DropDownStyle = ComboBoxStyle.DropDownList;
            Rgb.SelectedIndex = 0;
            Layer.Controls.Add(Rgb);
            Rgb.SelectedIndexChanged += new EventHandler(Rgb_Select);

            Label Visible = new Label();
            Visible.Text = "Непрозрачность: 100";
            Visible.Size = new Size(110, 35);
            Layer.Controls.Add(Visible);
            TrackBar VisBar = new TrackBar();
            VisBar.Maximum = 100;
            VisBar.Minimum = 0;
            VisBar.Value = 100;
            Layer.Controls.Add(VisBar);
            VisBar.Scroll += new EventHandler(VisBar_Scroll);

            Button BDelete = new Button();
            BDelete.Text = "Удалить";
            Layer.Controls.Add(BDelete);
            BDelete.Click += new EventHandler(Delete_Click);

            return Layer;
        }


        private void VisBar_Scroll(object sender, System.EventArgs e)
        {
            var bar = (TrackBar)sender;
            int index = GetIndex((FlowLayoutPanel)bar.Parent);
            bar.Parent.GetChildAtPoint(new Point(50, 320)).Text = "Непрозрачность: " + bar.Value.ToString();

            LayersList[index].Visible = (uint)bar.Value;

        }
        private void Rgb_Select(object sender, System.EventArgs e)
        {
            var list = (ComboBox)sender;
            int index = GetIndex((FlowLayoutPanel)list.Parent);
            LayersList[index].EChanel = (ECHANEL)list.SelectedIndex;
        }
        private void Oper_Select(object sender, System.EventArgs e)
        {
            var list = (ComboBox)sender;
            int index = GetIndex((FlowLayoutPanel)list.Parent);
            LayersList[index].EOper = (EOPER)list.SelectedIndex;
        }
        private void Delete_Click(object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            int index = GetIndex((FlowLayoutPanel)button.Parent);
            LayersList[index].plane.Dispose();
            LayersList.RemoveAt(index);
        }

        public int GetIndex(FlowLayoutPanel parent)
        {
            int i = 0;
            for (; i < LayersList.Count; ++i)
            {
                if (LayersList[i].plane == parent)
                {
                    return i;
                }
            }
            return -1;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileFialog = new SaveFileDialog();
            saveFileFialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileFialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            saveFileFialog.RestoreDirectory = true;

            if (saveFileFialog.ShowDialog() == DialogResult.OK)
            {
                if (ResultPic.Image != null)
                {
                    ResultPic.Image.Save(saveFileFialog.FileName);
                }
            }
            saveFileFialog.Dispose();
        }

        private void Get_Result(object sender, System.EventArgs e)
        {
            ResultPic.Image = Image.FromFile("Background0.png");
            for (int i = LayersList.Count - 1; i >= 0; i--)
            {
                switch (LayersList[i].EOper)
                {
                    case EOPER.No:
                        No(LayersList[i].EChanel, GetVisible(LayersList[i]));
                        break;
                    case EOPER.Sum:
                        Sum(LayersList[i].EChanel, GetVisible(LayersList[i]));
                        break;
                    case EOPER.Difference:
                        ResultPic.Image = Image.FromFile("Background.png");
                        Difference(LayersList[i].EChanel, GetVisible(LayersList[i]));
                        break;
                    case EOPER.Multy:
                        Multy(LayersList[i].EChanel, GetVisible(LayersList[i]));
                        break;
                   // case EOPER.Division:
                   //     ResultPic.Image = Image.FromFile("Background.png");
                   //     Division(LayersList[i].EChanel, GetVisible(LayersList[i]));
                   //     break;
                    case EOPER.Min:
                        Min(LayersList[i].EChanel, GetVisible(LayersList[i]));
                        break;
                    case EOPER.Max:
                        Max(LayersList[i].EChanel, GetVisible(LayersList[i]));
                        break;
                    default:
                        break;
                }
            }
        }

        Bitmap GetVisible(Layer elem)
        {
            double a = (100 - elem.Visible) / 100.0;
            Bitmap picture = (Bitmap)elem.Img.Clone();
            if (elem.Visible == 100)
            {
                return picture;
            }
            
            for(var i = 0; i < picture.Width; i++)
            {
                for(var j = 0; j < picture.Height; j++)
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

        void No(ECHANEL chanel, Bitmap pic)
        {
            var bmpData = pic.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * pic.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            var bmp = (Bitmap)ResultPic.Image;
            BitmapData bmpData1 = bmp.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr1 = bmpData1.Scan0;
            byte[] rgbValues1 = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes);

            for (var i = 0; i < rgbValues1.Length; i += 4)
            {
                int result = 0;
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        rgbValues1[i] = rgbValues[i];
                        rgbValues1[i + 1] = rgbValues[i + 1];
                        rgbValues1[i + 2] = rgbValues[i + 2];
                        break;
                    case ECHANEL.RG:
                        rgbValues1[i + 1] = rgbValues[i + 1];
                        rgbValues1[i + 2] = rgbValues[i + 2];
                        break;
                    case ECHANEL.RB:
                        rgbValues1[i] = rgbValues[i];
                        rgbValues1[i + 2] = rgbValues[i + 2];
                        break;
                    case ECHANEL.GB:
                        rgbValues1[i] = rgbValues[i];
                        rgbValues1[i + 1] = rgbValues[i + 1];
                        break;
                    case ECHANEL.R:
                        rgbValues1[i + 2] = rgbValues[i + 2];
                        break;
                    case ECHANEL.G:
                        rgbValues1[i + 1] = rgbValues[i + 1];
                        break;
                    case ECHANEL.B:
                        rgbValues1[i] = rgbValues[i];
                        break;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues1, 0, ptr1, bytes);
            pic.UnlockBits(bmpData);
            bmp.UnlockBits(bmpData1);
        }
        void Sum(ECHANEL chanel, Bitmap pic)
        {
            var bmpData = pic.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * pic.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            var bmp = (Bitmap)ResultPic.Image;
            BitmapData bmpData1 = bmp.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr1 = bmpData1.Scan0;
            byte[] rgbValues1 = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes);

            for (var i = 0; i < rgbValues1.Length; i += 4)
            {
                int result = 0;
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        result = rgbValues[i] + rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i+1] + rgbValues1[i+1];
                        rgbValues1[i+1] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i+2] + rgbValues1[i+2];
                        rgbValues1[i+2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RG:
                        result = rgbValues[i + 1] + rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 2] + rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RB:
                        result = rgbValues[i] + rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 2] + rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.GB:
                        result = rgbValues[i] + rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 1] + rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.R:
                        result = rgbValues[i + 2] + rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.G:
                        result = rgbValues[i + 1] - rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.B:
                        result = rgbValues[i] + rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        break;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues1, 0, ptr1, bytes);
            pic.UnlockBits(bmpData);
            bmp.UnlockBits(bmpData1);
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

        void Difference(ECHANEL chanel, Bitmap pic)
        {
            var bmpData = pic.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * pic.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            var bmp = (Bitmap)ResultPic.Image;
            BitmapData bmpData1 = bmp.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr1 = bmpData1.Scan0;
            byte[] rgbValues1 = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes);

            for (var i = 0; i < rgbValues1.Length; i += 4)
            {
                int result = 0;
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        result = rgbValues1[i] - rgbValues[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues1[i + 1] - rgbValues[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = rgbValues1[i + 2] - rgbValues[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RG:
                        result = rgbValues1[i + 1] - rgbValues[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = rgbValues1[i + 2] - rgbValues[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RB:
                        result = rgbValues1[i] - rgbValues[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues1[i + 2] - rgbValues[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.GB:
                        result = rgbValues1[i] - rgbValues[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues1[i + 1] - rgbValues[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.R:
                        result = rgbValues1[i + 2] - rgbValues[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.G:
                        result = rgbValues1[i + 1] - rgbValues[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.B:
                        result = rgbValues1[i] - rgbValues[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        break;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues1, 0, ptr1, bytes);
            pic.UnlockBits(bmpData);
            bmp.UnlockBits(bmpData1);
        }
        void Multy(ECHANEL chanel, Bitmap pic)
        {
            var bmpData = pic.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * pic.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            var bmp = (Bitmap)ResultPic.Image;
            BitmapData bmpData1 = bmp.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr1 = bmpData1.Scan0;
            byte[] rgbValues1 = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes);

            for (var i = 0; i < rgbValues1.Length; i += 4)
            {
                int result = 0;
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        result = rgbValues[i] * rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 1] * rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 2] * rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RG:
                        result = rgbValues[i + 1] * rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 2] * rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RB:
                        result = rgbValues[i] * rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 2] * rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.GB:
                        result = rgbValues[i] * rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 1] * rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.R:
                        result = rgbValues[i + 2] * rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.G:
                        result = rgbValues[i + 1] * rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.B:
                        result = rgbValues[i] * rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        break;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues1, 0, ptr1, bytes);
            pic.UnlockBits(bmpData);
            bmp.UnlockBits(bmpData1);
        }
        void Division(ECHANEL chanel, Bitmap pic)
        {
            var bmpData = pic.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * pic.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            var bmp = (Bitmap)ResultPic.Image;
            BitmapData bmpData1 = bmp.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr1 = bmpData1.Scan0;
            byte[] rgbValues1 = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes);

            for (var i = 0; i < rgbValues1.Length; i += 4)
            {
                int result = 0;
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        result = rgbValues[i] / rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 1] / rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 2] / rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RG:
                        result = rgbValues[i + 1] / rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 2] / rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RB:
                        result = rgbValues[i] / rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 2] / rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.GB:
                        result = rgbValues[i] / rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i + 1] / rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.R:
                        result = rgbValues[i + 2] / rgbValues1[i + 2];
                        rgbValues1[i + 2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.G:
                        result = rgbValues[i + 1] / rgbValues1[i + 1];
                        rgbValues1[i + 1] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.B:
                        result = rgbValues[i] / rgbValues1[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        break;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues1, 0, ptr1, bytes);
            pic.UnlockBits(bmpData);
            bmp.UnlockBits(bmpData1);
        }
        void Min(ECHANEL chanel, Bitmap pic)
        {
            var bmpData = pic.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * pic.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            var bmp = (Bitmap)ResultPic.Image;
            BitmapData bmpData1 = bmp.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr1 = bmpData1.Scan0;
            byte[] rgbValues1 = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes);
            for (var i = 0; i < rgbValues1.Length; i += 4)
            {
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        rgbValues1[i] = rgbValues[i] < rgbValues1[i]? rgbValues[i]: rgbValues1[i];
                        rgbValues1[i + 1] = rgbValues[i + 1] < rgbValues1[i + 1] ? rgbValues[i + 1] : rgbValues1[i + 1];
                        rgbValues1[i + 2] = rgbValues[i + 2] < rgbValues1[i + 2] ? rgbValues[i + 2] : rgbValues1[i + 2];
                        break;
                    case ECHANEL.RG:
                        rgbValues1[i + 1] = rgbValues[i + 1] < rgbValues1[i + 1] ? rgbValues[i + 1] : rgbValues1[i + 1];
                        rgbValues1[i + 2] = rgbValues[i + 2] < rgbValues1[i + 2] ? rgbValues[i + 2] : rgbValues1[i + 2];
                        break;
                    case ECHANEL.RB:
                        rgbValues1[i] = rgbValues[i] < rgbValues1[i] ? rgbValues[i] : rgbValues1[i];
                        rgbValues1[i + 2] = rgbValues[i + 2] < rgbValues1[i + 2] ? rgbValues[i + 2] : rgbValues1[i + 2];
                        break;
                    case ECHANEL.GB:
                        rgbValues1[i] = rgbValues[i] < rgbValues1[i] ? rgbValues[i] : rgbValues1[i];
                        rgbValues1[i + 1] = rgbValues[i + 1] < rgbValues1[i + 1] ? rgbValues[i + 1] : rgbValues1[i + 1];
                        break;
                    case ECHANEL.R:
                        rgbValues1[i + 2] = rgbValues[i + 2] < rgbValues1[i + 2] ? rgbValues[i + 2] : rgbValues1[i + 2];
                        break;
                    case ECHANEL.G:
                        rgbValues1[i + 1] = rgbValues[i + 1] < rgbValues1[i + 1] ? rgbValues[i + 1] : rgbValues1[i + 1];
                        break;
                    case ECHANEL.B:
                        rgbValues1[i] = rgbValues[i] < rgbValues1[i] ? rgbValues[i] : rgbValues1[i];
                        break;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues1, 0, ptr1, bytes);
            pic.UnlockBits(bmpData);
            bmp.UnlockBits(bmpData1);
        }
        void Max(ECHANEL chanel, Bitmap pic)
        {
            var bmpData = pic.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * pic.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            var bmp = (Bitmap)ResultPic.Image;
            BitmapData bmpData1 = bmp.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, pic.PixelFormat);
            IntPtr ptr1 = bmpData1.Scan0;
            byte[] rgbValues1 = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes);
            for (var i = 0; i < rgbValues1.Length; i += 4)
            {
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        rgbValues1[i] = rgbValues[i] > rgbValues1[i] ? rgbValues[i] : rgbValues1[i];
                        rgbValues1[i + 1] = rgbValues[i + 1] > rgbValues1[i + 1] ? rgbValues[i + 1] : rgbValues1[i + 1];
                        rgbValues1[i + 2] = rgbValues[i + 2] > rgbValues1[i + 2] ? rgbValues[i + 2] : rgbValues1[i + 2];
                        break;
                    case ECHANEL.RG:
                        rgbValues1[i + 1] = rgbValues[i + 1] > rgbValues1[i + 1] ? rgbValues[i + 1] : rgbValues1[i + 1];
                        rgbValues1[i + 2] = rgbValues[i + 2] > rgbValues1[i + 2] ? rgbValues[i + 2] : rgbValues1[i + 2];
                        break;
                    case ECHANEL.RB:
                        rgbValues1[i] = rgbValues[i] > rgbValues1[i] ? rgbValues[i] : rgbValues1[i];
                        rgbValues1[i + 2] = rgbValues[i + 2] > rgbValues1[i + 2] ? rgbValues[i + 2] : rgbValues1[i + 2];
                        break;
                    case ECHANEL.GB:
                        rgbValues1[i] = rgbValues[i] > rgbValues1[i] ? rgbValues[i] : rgbValues1[i];
                        rgbValues1[i + 1] = rgbValues[i + 1] > rgbValues1[i + 1] ? rgbValues[i + 1] : rgbValues1[i + 1];
                        break;
                    case ECHANEL.R:
                        rgbValues1[i + 2] = rgbValues[i + 2] > rgbValues1[i + 2] ? rgbValues[i + 2] : rgbValues1[i + 2];
                        break;
                    case ECHANEL.G:
                        rgbValues1[i + 1] = rgbValues[i + 1] > rgbValues1[i + 1] ? rgbValues[i + 1] : rgbValues1[i + 1];
                        break;
                    case ECHANEL.B:
                        rgbValues1[i] = rgbValues[i] > rgbValues1[i] ? rgbValues[i] : rgbValues1[i];
                        break;
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues1, 0, ptr1, bytes);
            pic.UnlockBits(bmpData);
            bmp.UnlockBits(bmpData1);
        }
    }
}
