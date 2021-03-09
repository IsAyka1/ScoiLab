using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ayka_scoi
{
    public partial class Form1 : Form
    {
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
                FirstAdd(PicPanel);
            }

            PicPanel.Controls.Add(NewPic());

        }

        void FirstAdd(FlowLayoutPanel p)
        {
            p.Visible = true;
            p.Location = new Point(510, 10);
            p.Size = new Size(270, 420);
            p.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            p.AutoScroll = true;
            p.AutoSize = true;
            p.Dock = DockStyle.Right;
            p.FlowDirection = FlowDirection.TopDown;
            p.WrapContents = false;
            
            this.Controls.Add(p);
            Button BResult = new Button();
            BResult.BackColor = Color.LightGray;
            BResult.ForeColor = Color.Red;
            BResult.Size = new Size(100, 28);
            BResult.Location = new Point(130, 409);
            BResult.Text = "Get Result";
            this.Controls.Add(BResult);
            f = false;
        }

        static uint LayerCount = 0;
        Panel NewPic()
        {
            LayerCount++;
            Panel Layer = new Panel();
            Layer.Size = new Size(240, 400);
            //Layer.Location = new Point(520, ((int)LayerCount-1) * 300);
            Layer.Text = "Layer" + LayerCount;
            PictureBox Pic = new PictureBox();
            Pic.Parent = Layer;
            Pic.SizeMode = PictureBoxSizeMode.Zoom;
            Pic.Size = new Size(225, 250);
            Pic.Dock = DockStyle.Fill;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image Files(*.JPG;*.PNG)|*.JPG;*.PNG|All files (*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Pic.Image = new Bitmap(fileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Layer.Controls.Add(Pic);
            }
            Label Chanel = new Label();
            Chanel.Text = "Канал";
            Chanel.Location = new Point(0, 350);
            
            //Chanel.Dock = DockStyle.Fill;
            Layer.Controls.Add(Chanel);
            
            ComboBox Rgb = new ComboBox();
            Rgb.Items.AddRange(new string[] {"RGB", "RG", "RB", "GB", "R", "G", "B" });
            Rgb.DropDownStyle = ComboBoxStyle.DropDownList;
            //Rgb.Dock = DockStyle.Fill;
            //Rgb.Location = new Point(100,260);
            Layer.Controls.Add(Rgb);
            return Layer;
        }
    }
}
