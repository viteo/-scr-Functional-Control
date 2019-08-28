using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sharpsaver.Models
{
    public class Brick
    {
        public static Random random = new Random();

        public Rectangle brick;

        public double Left;
        public double Top;
        private bool transparent;
        private bool isBad;

        public double centerX { get => Left + brick.Width / 2; }

        public double centerY { get => Top + brick.Height / 2; }

        public Brick(double left, double top, bool transparent = false)
        {
            Left = left;
            Top = top;
            brick = new Rectangle();
            brick.Width = 50;
            brick.Height = 45;
            this.transparent = transparent;
            isBad = random.Next(2) == 0;
            if (!transparent)
                brick.Fill = new SolidColorBrush(isBad ? Colors.Red : Colors.Green);
            brick.Stroke = new SolidColorBrush(Colors.Black);
            brick.StrokeThickness = 1;
            Canvas.SetLeft(brick, left);
            Canvas.SetTop(brick, top);
        }

        public void Switch()
        {
            if(!transparent)
            {
                isBad = !isBad;
                brick.Fill = new SolidColorBrush(isBad ? Colors.Red : Colors.Green);
            }
        }
    }
}
