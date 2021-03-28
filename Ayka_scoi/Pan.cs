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
        private bool paint_mode = false;
        List<Point> PointsSpline = new List<Point>();
        List<Point> Points = new List<Point>();
        public Pan(ref List<Point> Points)
        {
            Points = this.Points;
            Points.Add(new Point(0, 420));
            Points.Add(new Point(420, 0));

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

        public void DrawSpline(List<Point> Spline)
        {
            PointsSpline = Spline;
        }

        private void Pan_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint_mode == true)
            {
                int X = e.Location.X;
                int Y = e.Location.Y;
                for (int i = 1; i < Points.Count - 1; ++i)
                {
                    if ((X >= Points[i].X - 5 && X <= Points[i].X + 5) && (Y >= Points[i].Y - 5 && Y <= Points[i].Y + 5))
                    {
                        Points[i] = new Point(X, Y);
                    }
                }
            }
        }

        private void Pan_MouseUp(object sender, MouseEventArgs e)
        {
            paint_mode = false;
            var a = (Form2)this.Parent;
            a.LetsSpline();
        }

        private void Pan_MouseDown(object sender, MouseEventArgs e)
        {
            int X = e.Location.X;
            int Y = e.Location.Y;
            for (int i = 0; i < Points.Count; ++i)
            {
                if ((X >= Points[i].X - 5 && X <= Points[i].X + 5) && (Y >= Points[i].Y - 5 && Y <= Points[i].Y + 5))
                {
                    paint_mode = true;
                    return;
                }
            }
            NormalyzePoints(new Point(X, Y));

        }

        void NormalyzePoints(Point tmp)
        {
            for(int i = 0; i < Points.Count; ++i)
            {
                if (Points[i].X > tmp.X)
                {
                    Points.Add(Points[Points.Count - 1]);
                    for (int j = Points.Count - 2; j >= i; --j)
                    {
                        Points[j + 1] = Points[j]; 
                    }
                    Points[i] = tmp;
                    break;
                }
                else continue;
            }

        }

        public void p_event(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (Points.Count == 2)
            {
                for (int i = 0; i < Points.Count - 1; ++i)
                {
                    e.Graphics.DrawLine(Pens.Black, Points[i], Points[i + 1]);
                }
            }
            for (int i = 1; i < Points.Count - 1; ++i)
            {
                e.Graphics.FillRectangle(Brushes.Red, new RectangleF(Points[i].X - 5, Points[i].Y - 5, 10, 10));
                
            }
            if(PointsSpline.Count != 0)
            {
                for (int i = 0; i < PointsSpline.Count - 1; ++i)
                {
                    e.Graphics.DrawLine(Pens.Black, PointsSpline[i], PointsSpline[i + 1]);
                }
            }
            //событие отрисовки вызывается, когда ОС дает окну команду на перересовку.

            //Тут уже знакомый нам Graphics
            //все что на нем рисуется - отобразится на форме в процессе перерисовки
            //e.Graphics.FillRectangle(Brushes.Red, 0, 0, Size.Width, Size.Height);
        }
    }
}
