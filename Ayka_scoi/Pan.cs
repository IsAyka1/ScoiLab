using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ayka_scoi
{
    class Pan : System.Windows.Forms.Panel
    {
        public Pan()
        {
            //настраиваем стель для плавного рисования
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);

            //прикрепляем методы к событиям
            //событие отрисовки
            Paint += p_event;

            //события мыши
            //перехватываем клики, смотрим координаты, 
            //создаем массивы с точками, рисуем, 
            //интерполируем, итд.
            MouseDown += Pan_MouseDown;
            MouseUp += Pan_MouseUp;
            MouseMove += Pan_MouseMove;

            //включаем постоянную перерисовку по таймеру
            //не совсем оптимальный вариант, все время рисовать на виджите
            //но для сделанного на коленке пойдет

            Timer y = new Timer();
            y.Interval = 30;
            y.Tick += (s, a) => { this.Refresh(); };

            VisibleChanged += (s, a) => { y.Start(); };

        }

        private void Pan_MouseMove(object sender, MouseEventArgs e)
        {
            //e->Location - координаты мыши в рамках окна
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
        }

        private void Pan_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void Pan_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left) { }
            else if (e.Button == MouseButtons.Right)
            { }

        }

        public void p_event(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //событие отрисовки вызывается, когда ОС дает окну команду на перересовку.

            //Тут уже знакомый нам Graphics
            //все что на нем рисуется - отобразится на форме в процессе перерисовки
            e.Graphics.FillRectangle(Brushes.Red, 0, 0, Size.Width, Size.Height);
        }
    }
}
