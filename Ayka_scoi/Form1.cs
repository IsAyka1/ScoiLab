using Ayka_scoi;
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
                FirstAdd(PicPanel);
            }
            FlowLayoutPanel NewLayer = NewPic(PicPanel);
            PicPanel.Controls.Add(NewLayer);
            LayersList.Add(new Layer((Bitmap)((PictureBox)(NewLayer.GetChildAtPoint(new Point(100, 100)))).Image));
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
            BResult.Text = "Посчитать";
            this.Controls.Add(BResult);
            BResult.Click += new EventHandler(Get_Result);
            f = false;
        }


        FlowLayoutPanel NewPic(FlowLayoutPanel p)
        {
            LayerCount++;
            FlowLayoutPanel Layer = new FlowLayoutPanel();
            Layer.Size = new Size(250, 416);
            Layer.Name = "Layer " + LayerCount;
            Layer.Parent = p;
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
                    MessageBox.Show("Невозможно открыть файл", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Pic.Parent = Layer;
                Layer.Controls.Add(Pic);
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
            Visible.Text = "Прозрачность: 100";
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
            bar.Parent.GetChildAtPoint(new Point(50, 320)).Text = "Прозрачность: " + bar.Value.ToString();

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

            LayersList.RemoveAt(Int32.Parse(id[1]) - 1);
            list.Parent.Parent.Controls.Remove(list.Parent);
            LayerCount--;
        }


        private void Get_Result(object sender, System.EventArgs e)
        {
            
        }

        void Visible()
        {

        }

        void No()
        {

        }
        void Sum()
        {

        }
        void Diff()
        {

        }
        void Multy()
        {

        }
        void Div()
        {

        }
        void Min()
        {

        }
        void Max()
        {

        }
    }
}
