using Sharpsaver.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Sharpsaver.Views
{
    public partial class ScreensaverView : Window
    {
        private bool isPreviewWindow;
        private Point lastMousePosition = default;
        private System.Windows.Threading.DispatcherTimer dispatcherTimer;
        private List<Brick> bricks;

        public ScreensaverView()
        {
            InitializeComponent();
            isPreviewWindow = false;
        }
        public ScreensaverView(IntPtr previewHandle)
        {
            InitializeComponent();
            WindowState = WindowState.Normal;
            isPreviewWindow = true;

            IntPtr windowHandle = new WindowInteropHelper(GetWindow(this)).EnsureHandle();

            // Set the preview window as the parent of this window
            InteropHelper.SetParent(windowHandle, previewHandle);

            // Make this window a tool window while preview.
            // A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB.
            // GWL_EXSTYLE = -20, WS_EX_TOOLWINDOW = 0x00000080L
            InteropHelper.SetWindowLong(windowHandle, -20, 0x00000080L);
            // Make this a child window so it will close when the parent dialog closes
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            InteropHelper.SetWindowLong(windowHandle, -16, 0x40000000L);

            // Place the window inside the parent
            InteropHelper.GetClientRect(previewHandle, out Rect parentRect);

            Width = parentRect.Width;
            Height = parentRect.Height;
        }

        private void Draw()
        {
            Rectangle bkgRect = new Rectangle();
            bkgRect.Fill = new SolidColorBrush(Colors.White);
            var size = this.Field.Width > this.Field.Height ? this.Field.Height : this.Field.Width;
            bkgRect.Width = size;
            bkgRect.Height = size;
            var LEFT = (this.Field.Width - size) / 2;
            var CENTERX = LEFT + size / 2;
            var CENTERY = size / 2;
            var RIGHT = LEFT + size;
            Canvas.SetLeft(bkgRect, LEFT);

            Ellipse ellipse = new Ellipse();
            ellipse.Width = size;
            ellipse.Height = size;
            ellipse.Fill = new SolidColorBrush(Colors.LightGray);
            ellipse.Stroke = new SolidColorBrush(Colors.Gray);
            ellipse.StrokeThickness = size / 100;
            Canvas.SetLeft(ellipse, LEFT);

            this.Field.Children.Add(bkgRect);
            this.Field.Children.Add(ellipse);

            bricks = new List<Brick>();
            for (double y = 0; y < size; y += 45)
                for (double x = LEFT - (y % 2) * 25; x <= RIGHT; x += 50)
                {
                    var normX = x + 25 - CENTERX;
                    var normY = y + 22.5 - CENTERY;
                    var radius = size / 2;
                    var normDistance = ((double)(normX * normX) / (radius * radius)) + ((double)(normY * normY) / (radius * radius));
                    if (normDistance <= 0.90)
                        bricks.Add(new Brick(x, y));
                    else if (normDistance < 1.05)
                        bricks.Add(new Brick(x, y, true));
                }
            foreach (var brick in bricks)
            {
                this.Field.Children.Add(brick.brick);
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPreviewWindow) return;

            Point pos = e.GetPosition(this);

            if (lastMousePosition != default)
            {
                if ((lastMousePosition - pos).Length > 3)
                {
                    Application.Current.Shutdown();
                }
            }
            lastMousePosition = pos;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isPreviewWindow) return;

            Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (isPreviewWindow) return;

            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //  DispatcherTimer setup
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            Draw();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            bricks[Brick.random.Next(bricks.Count)].Switch();
        }
    }
}
