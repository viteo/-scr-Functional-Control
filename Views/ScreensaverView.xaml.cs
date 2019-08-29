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
            var size = this.Field.Width > this.Field.Height ? this.Field.Height : this.Field.Width;
            var LEFT = (this.Field.Width - size) / 2;
            var CENTERX = LEFT + size / 2;
            var CENTERY = size / 2;
            var RIGHT = LEFT + size;

            Rectangle bkgRect = new Rectangle();
            bkgRect.Fill = new SolidColorBrush(Colors.White);
            if (Settings.Instance.IsFullscreen)
            {
                bkgRect.Width = this.Field.Width;
                bkgRect.Height = this.Field.Height;
                Canvas.SetLeft(bkgRect, 0);
            }
            else
            {
                bkgRect.Width = size;
                bkgRect.Height = size;
                Canvas.SetLeft(bkgRect, LEFT);
            }
            

            Ellipse bkgCircle = new Ellipse();
            bkgCircle.Width = size;
            bkgCircle.Height = size;
            bkgCircle.Fill = new SolidColorBrush(Colors.LightGray);
            bkgCircle.Stroke = new SolidColorBrush(Colors.Gray);
            bkgCircle.StrokeThickness = size / 100;
            Canvas.SetLeft(bkgCircle, LEFT);

            this.Field.Children.Add(bkgRect);
            this.Field.Children.Add(bkgCircle);

            Brick.Width = size * Settings.Instance.BrickSize / 100;
            Brick.Height = size * Settings.Instance.BrickSize / 110;
            bricks = new List<Brick>();
            bool layoutShift = false;
            for (double y = 0; y < size; y += Brick.Height)
            {
                if (Settings.Instance.Layout == Layout.Brickwall)
                    layoutShift = !layoutShift;
                for (double x = LEFT - (layoutShift ? 0 : Brick.Width / 2); x <= RIGHT; x += Brick.Width)
                {
                    var normX = x + Brick.Width / 2 - CENTERX;
                    var normY = y + Brick.Height / 2 - CENTERY;
                    var radius = size / 2;
                    var normDistance = ((double)(normX * normX) / (radius * radius)) + ((double)(normY * normY) / (radius * radius));
                    if (normDistance <= 0.90)
                        bricks.Add(new Brick(x, y));
                    else if (normDistance < 1.05)
                        bricks.Add(new Brick(x, y, true));
                }
            }
            foreach (var brick in bricks)
            {
                this.Field.Children.Add(brick.brick);
                this.Field.Children.Add(brick.dot);
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
            // Get settins from file or default
            Settings.Instance = new Settings();
            Settings.Instance.LoadSettings();
            //  DispatcherTimer setup
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(Settings.Instance.SwitchPeriod);
            dispatcherTimer.Start();

            Draw();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Change color of random brick
            bricks[Settings.Random.Next(bricks.Count)].Switch();
        }
    }
}
