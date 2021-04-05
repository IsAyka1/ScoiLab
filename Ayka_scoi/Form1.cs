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
        PictureBox ResultPic = new PictureBox();
        enum EBINAR
        {
            No, Gavrilov, Otsu, Niblek, Sauvola, BredliRot, Wulf
        }
        EBINAR EBinar = 0;
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

            Button BAdd = new Button();
            BAdd.Size = new Size(100, 30);
            BAdd.Location = new Point(10, 414);
            BAdd.Text = "Добавить";
            this.Controls.Add(BAdd);
            BAdd.Click += new EventHandler(BAdd_Click);

            Button BResult = new Button();
            BResult.BackColor = Color.LightGray;
            BResult.ForeColor = Color.Red;
            BResult.Size = new Size(100, 30);
            BResult.Location = new Point(110, 414);
            BResult.Text = "Посчитать";
            this.Controls.Add(BResult);
            BResult.Click += new EventHandler(Get_Result);

            ResultPic.Image = Image.FromFile("Background.png");
            ResultPic.SizeMode = PictureBoxSizeMode.Zoom;
            ResultPic.Size = new Size(500, 380);
            ResultPic.Location = new Point(10, 10);
            ResultPic.BackColor = Color.White;
            this.Controls.Add(ResultPic);

            ComboBox Binariz = new ComboBox();
            Binariz.Items.AddRange(new object[] { "-", "Критерий Гаврилова", "Критерий Отсу", "Критерий Ниблека", "Критерий Сауволы", "Критерий Брэдли-Рота", "Критерий Вульфа" });
            Binariz.Location = new Point(415, 415);
            Binariz.Size = new Size(100, 30);
            Binariz.DropDownStyle = ComboBoxStyle.DropDownList;
            Binariz.SelectedIndex = 0;
            Binariz.SelectedIndexChanged += Binariz_Select;
            this.Controls.Add(Binariz);
        }


        private void BAdd_Click(object sender, EventArgs e)
        {
            Bitmap Img = null;
            FlowLayoutPanel NewLayer = NewPic(ref Img);
            if (NewLayer != null)
            {
                LayersList.Add(new Layer(ref Img, NewLayer));
                PicPanel.Controls.Add(NewLayer);
            }
        }

        FlowLayoutPanel NewPic(ref Bitmap ImgC)
        {
            FlowLayoutPanel Layer = new FlowLayoutPanel();
            Layer.Size = new Size(250, 420);
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

        private void Binariz_Select(object sender, System.EventArgs e)
        {
            var list = (ComboBox)sender;
            EBinar = (EBINAR)list.SelectedIndex;
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

        private void BSave_Click(object sender, EventArgs e)
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
                Bitmap Visible = GetVisible(LayersList[i]);

                var bmpData = Visible.LockBits(new Rectangle(0, 0, Visible.Width, Visible.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Visible.PixelFormat);
                IntPtr ptr = bmpData.Scan0;
                int bytes = Math.Abs(bmpData.Stride) * Visible.Height;
                byte[] bgraValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr, bgraValues, 0, bytes);

                var bmp = (Bitmap)ResultPic.Image;
                BitmapData bmpData1 = bmp.LockBits(new Rectangle(0, 0, Visible.Width, Visible.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Visible.PixelFormat);
                IntPtr ptr1 = bmpData1.Scan0;
                byte[] bgraValues1 = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(ptr1, bgraValues1, 0, bytes);
                switch (LayersList[i].EOper)
                {
                    case EOPER.No:
                        No(LayersList[i].EChanel, bgraValues, ref bgraValues1);
                        break;
                    case EOPER.Sum:
                        Sum(LayersList[i].EChanel, bgraValues, ref bgraValues1);
                        break;
                    case EOPER.Difference:
                        Difference(LayersList[i].EChanel, bgraValues, ref bgraValues1);
                        break;
                    case EOPER.Multy:
                        Multy(LayersList[i].EChanel, (int)LayersList[i].Visible, bgraValues, ref bgraValues1);
                        break;
                    case EOPER.Min:
                        Min(LayersList[i].EChanel, bgraValues, ref bgraValues1);
                        break;
                    case EOPER.Max:
                        Max(LayersList[i].EChanel, bgraValues, ref bgraValues1);
                        break;
                    default:
                        break;
                }
                System.Runtime.InteropServices.Marshal.Copy(bgraValues1, 0, ptr1, bytes);
                Visible.UnlockBits(bmpData);
                bmp.UnlockBits(bmpData1);
                Visible.Dispose();
            }

            if(EBinar != 0)
            {
                GetBinariz();
            }

            Button BGistogram = new Button();
            BGistogram.Size = new Size(100, 30);
            BGistogram.Location = new Point(210, 414);
            BGistogram.Text = "Гистограмма";
            this.Controls.Add(BGistogram);
            BGistogram.Click += new EventHandler(Get_Gistogram);

            Button BSave = new Button();
            BSave.Size = new Size(100, 30);
            BSave.Location = new Point(310, 414);
            BSave.Text = "Сохранить";
            this.Controls.Add(BSave);
            BSave.Click += new EventHandler(BSave_Click);


        }        

        void Get_Gistogram(object sender, System.EventArgs e)
        {
            Form2 F = new Form2((Bitmap)ResultPic.Image);
            F.Focus();
            F.Show();
            F.Get_Grafisc((Bitmap)ResultPic.Image.Clone());
            F.FormClosed += Gistogram_Close;
        }

        private void Gistogram_Close(object sender, FormClosedEventArgs e)
        {
            Form2 F = (Form2)sender;
            ResultPic.Image = F.Changer;
            F.Dispose();
        }
    }
}
