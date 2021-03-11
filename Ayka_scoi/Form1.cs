using Ayka_scoi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Ayka_scoi
{
    public partial class Form1 : Form
    {
        uint LayerCount = 0;
        List<Layer> LayersList = new List<Layer>();
        public Form1()
        {
            InitializeComponent();

        }

        static bool f = true;
        FlowLayoutPanel PicPanel = new FlowLayoutPanel();

        private void BAdd_Click(object sender, EventArgs e)
        {
            PicPanel.Parent = this;
            if (f)
            {
                FirstAdd();
            }
            FlowLayoutPanel NewLayer = NewPic();
            if (NewLayer != null)
            {
                var id = NewLayer.Name.Split(new char[] { ' ' });
                LayersList.Add(new Layer((Bitmap)((PictureBox)(NewLayer.GetChildAtPoint(new Point(100, 100)))).Image, UInt32.Parse(id[1]) - 1));
                PicPanel.Controls.Add(NewLayer);
            }
        }

        PictureBox ResultPic = new PictureBox();

        void FirstAdd()
        {
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

            ResultPic.Image = Image.FromFile(@"C:\Users\mtara\Desktop\ScoiLab\Background.png");
            ResultPic.SizeMode = PictureBoxSizeMode.Zoom;
            ResultPic.Size = new Size(500,400);
            ResultPic.Location = new Point(10,10);
            ResultPic.BackColor = Color.White;
            this.Controls.Add(ResultPic);
            f = false;
        }


        FlowLayoutPanel NewPic()
        {
            LayerCount++;
            FlowLayoutPanel Layer = new FlowLayoutPanel();
            Layer.Size = new Size(250, 416);
            Layer.Name = "Layer " + LayerCount;
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
                    Pic.Image = new Bitmap(fileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); //TODO
                }
                Pic.Parent = Layer;
                Layer.Controls.Add(Pic);
            } else
            {
                LayerCount--;
                PicPanel.Controls.Remove(Layer);
                MessageBox.Show("Невозможно добавить файл", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); //TODO
                return null;
            }
            Label Operation = new Label();
            Operation.Text = "Операция:";
            Layer.Controls.Add(Operation);
            ComboBox Oper = new ComboBox();
            //Oper.Items.AddRange(new string[] { "Нет", "Сумма", "Разность", "Умножение", "Деление", "Минимум", "Максимум" });
            Oper.Items.AddRange(new string[]
            { EOPER.No.ToString(), EOPER.Sum.ToString(), EOPER.Difference.ToString(), EOPER.Multy.ToString(), EOPER.Division.ToString(), EOPER.Min.ToString(), EOPER.Max.ToString() });
            Oper.DropDownStyle = ComboBoxStyle.DropDownList;
            Oper.SelectedIndex = 0;
            Layer.Controls.Add(Oper);
            Oper.SelectedIndexChanged += new EventHandler(Oper_Select);

            Label Chanel = new Label();
            Chanel.Text = "Канал:";
            Layer.Controls.Add(Chanel);
            ComboBox Rgb = new ComboBox();
            //Rgb.Items.AddRange(new string[] {"RGB", "RG", "RB", "GB", "R", "G", "B" });
            Rgb.Items.AddRange(new string[]
            { ECHANEL.RGB.ToString(), ECHANEL.RG.ToString(), ECHANEL.RB.ToString(), ECHANEL.GB.ToString(), ECHANEL.R.ToString(), ECHANEL.G.ToString(), ECHANEL.B.ToString() });
            Rgb.DropDownStyle = ComboBoxStyle.DropDownList;
            Rgb.SelectedIndex = 0;
            Layer.Controls.Add(Rgb);
            Rgb.SelectedIndexChanged += new EventHandler(Rgb_Select);

            Label Visible = new Label();
            Visible.Text = "Непрозрачность: 100";
            Visible.Size = new Size(110, 28);
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
            var id = bar.Parent.Name.Split(new char[] { ' ' });
            bar.Parent.GetChildAtPoint(new Point(50, 320)).Text = "Непрозрачность: " + bar.Value.ToString();

            LayersList[Int32.Parse(id[1])-1].Visible = (uint)bar.Value;

        }
        private void Rgb_Select(object sender, System.EventArgs e)
        {
            var list = (ComboBox)sender;
            var id = list.Parent.Name.Split(new char[] { ' ' });


            LayersList[Int32.Parse(id[1])-1].EChanel = (ECHANEL)list.SelectedIndex;
        }
        private void Oper_Select(object sender, System.EventArgs e)
        {
            var list = (ComboBox)sender;
            var id = list.Parent.Name.Split(new char[] { ' ' });


            LayersList[Int32.Parse(id[1])-1].EOper = (EOPER)list.SelectedIndex;
        }
        private void Delete_Click(object sender, System.EventArgs e)
        {
            var list = (Button)sender;
            var id = list.Parent.Name.Split(new char[] { ' ' });

            for (int i = Int32.Parse(id[1]); i < LayerCount; i++)
            {
                LayersList[i].id--;
                list.Parent.Name = "Layer " + Convert.ToString(LayersList[i].id + 1);
            }
            LayersList.RemoveAt(Int32.Parse(id[1]) - 1);
            //this.Controls.Remove(list.Parent);
            list.Parent.Dispose();
            LayerCount--;
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
            ResultPic.Image = Image.FromFile(@"C:\Users\mtara\Desktop\ScoiLab\Background0.png");
            //foreach(var elem in LayersList)
            for (int i = LayersList.Count - 1; i >= 0; i--)
            {
                switch (LayersList[i].EOper)
                {
                    case EOPER.No:
                        No(GetVisible(LayersList[i]));
                        break;
                    case EOPER.Sum:
                        Sum(LayersList[i].EChanel, GetVisible(LayersList[i]));
                        break;
                    case EOPER.Difference:
                        Difference(LayersList[i].EChanel, GetVisible(LayersList[i]));
                        break;
                    case EOPER.Multy:
                        Multy(LayersList[i].EChanel, GetVisible(LayersList[i]));
                        break;
                    case EOPER.Division:
                        Division(LayersList[i].EChanel, GetVisible(LayersList[i]));
                        break;
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

            if (elem.Visible == 100)
            {
                return elem.Img;
            }
            double a = (100 - elem.Visible) / 100.0;
            Bitmap picture = new Bitmap(elem.Img);
            for(var i = 0; i < elem.Img.Width; i++)
            {
                for(var j = 0; j < elem.Img.Height; j++)
                {
                    var pix = elem.Img.GetPixel(i, j);
                    pix = Color.FromArgb((int)((255 - pix.R) * a + pix.R), (int)((255 - pix.G) * a + pix.G), (int)((255 - pix.B) * a + pix.B));
                    picture.SetPixel(i, j, pix);
                }
            }
            //var q = (FlowLayoutPanel)PicPanel.GetChildAtPoint(new Point(10, (int)(417 * elem.id) + 10));
            //var w = (PictureBox)q.GetChildAtPoint(new Point(100, 100));
            //w.Image = picture;

           //((PictureBox)((FlowLayoutPanel)PicPanel.GetChildAtPoint(new Point(520, (int)(426 * elem.id) + 10))).GetChildAtPoint(new Point(100, 100))).Image = picture;
            //picture.Dispose();
            return picture;
        }

        void No(Bitmap pic)
        {
            ResultPic.Image = pic;
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
            int bytes1 = Math.Abs(bmpData.Stride) * pic.Height;
            byte[] rgbValues1 = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, rgbValues1, 0, bytes1);

            for (var i = 0; i < rgbValues1.Length; i += 3)
            {
                switch (chanel)
                {
                    case ECHANEL.RGB:
                        int result = rgbValues[i] + rgbValues[i];
                        rgbValues1[i] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i+1] + rgbValues[i+1];
                        rgbValues1[i+1] = Convert.ToByte(Normalize(result));
                        result = rgbValues[i+2] + rgbValues[i+2];
                        rgbValues1[i+2] = Convert.ToByte(Normalize(result));
                        break;
                    case ECHANEL.RG:

                        break;
                    case ECHANEL.RB:
                        break;
                    case ECHANEL.GB:
                        break;
                    case ECHANEL.R:
                        break;
                    case ECHANEL.G:
                        break;
                    case ECHANEL.B:
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

        }
        void Multy(ECHANEL chanel, Bitmap pic)
        {

        }
        void Division(ECHANEL chanel, Bitmap pic)
        {

        }
        void Min(ECHANEL chanel, Bitmap pic)
        {

        }
        void Max(ECHANEL chanel, Bitmap pic)
        {

        }
    }
}
