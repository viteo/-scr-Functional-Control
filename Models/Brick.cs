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
        public static double Width;
        public static double Height;
        public static double dotSize { get => Width / 5; }

        public Rectangle brick;
        public Ellipse dot;
        public double Left;
        public double Top;
        
        private bool isTransparent;
        private bool isBad;

        public double centerX { get => Left + brick.Width / 2; }

        public double centerY { get => Top + brick.Height / 2; }

        public Brick(double left, double top, bool transparent = false)
        {
            Left = left;
            Top = top;

            brick = new Rectangle();
            brick.Width = Brick.Width;
            brick.Height = Brick.Height;
            this.isTransparent = transparent;
            isBad = Settings.Random.Next(2) == 0;
            if (!isTransparent)
                brick.Fill = isBad ? Brushes.Red : Brushes.Green;
            brick.Stroke = Brushes.Black;
            brick.StrokeThickness = 1;
            Canvas.SetLeft(brick, left);
            Canvas.SetTop(brick, top);

            dot = new Ellipse();
            dot.Fill = Brushes.Black;
            dot.Width = dot.Height = dotSize;
            if (!isTransparent && !isBad)
                dot.Visibility = System.Windows.Visibility.Hidden;
            Canvas.SetLeft(dot, centerX - dotSize/2);
            Canvas.SetTop(dot, centerY - dotSize/2);
        }

        public void Switch()
        {
            if(!isTransparent)
            {
                isBad = !isBad;
                brick.Fill = new SolidColorBrush(isBad ? Colors.Red : Colors.Green);
                dot.Visibility = isBad ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            }
        }
    }
}
