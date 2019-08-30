using Sharpsaver.Views;
using System;
using System.Windows;
using System.Windows.Interop;

namespace Sharpsaver
{
    public partial class App : Application
    {
        private ScreensaverView previewWindow;
        private HwndSource winWPFContent;

        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);

            if (args.Args.Length == 0 || args.Args[0].ToLower().StartsWith("/s"))
            {
                //Start the screensaver in full-screen mode.
                var screensaverWindow = new ScreensaverView();
                screensaverWindow.Show();
            }
            else if (args.Args[0].ToLower().StartsWith("/p"))
            {
                //Display a preview of the screensaver using the specified window handle.
                //IntPtr previewHwnd = new IntPtr(Convert.ToInt32(args.Args[1]));
                //var previewWindow = new ScreensaverView(previewHwnd);
                //previewWindow.Show();

                previewWindow = new ScreensaverView(/**/);

                IntPtr previewHwnd = new IntPtr(Convert.ToInt32(args.Args[1]));

                Rect lpRect = new Rect();
                bool bGetRect = InteropHelper.GetClientRect(previewHwnd, ref lpRect);

                HwndSourceParameters sourceParams = new HwndSourceParameters("sourceParams");

                sourceParams.PositionX = 0;
                sourceParams.PositionY = 0;
                sourceParams.Height = lpRect.Bottom - lpRect.Top;
                sourceParams.Width = lpRect.Right - lpRect.Left;
                previewWindow.Field.Height = sourceParams.Height;
                previewWindow.Field.Width = sourceParams.Width;
                sourceParams.ParentWindow = previewHwnd;
                //WS_VISIBLE = 0x10000000; WS_CHILD = 0x40000000; WS_CLIPCHILDREN = 0x02000000;
                sourceParams.WindowStyle = (int)(0x10000000L | 0x40000000L | 0x02000000L);
                //sourceParams.ExtendedWindowStyle = (int)0x00000080L;

                winWPFContent = new HwndSource(sourceParams);
                winWPFContent.Disposed += new EventHandler(winWPFContent_Disposed);
                winWPFContent.ContentRendered += new EventHandler(previewWindow.Window_Loaded);
                winWPFContent.RootVisual = previewWindow.Viewbox;
            }
            else if (args.Args[0].ToLower().StartsWith("/c"))
            {
                //Show the configuration settings dialog box.
                var settingsWindow = new SettingsView();
                settingsWindow.Show();
            }

            void winWPFContent_Disposed(object sender, EventArgs e)
            {
                previewWindow.Close();
                //            Application.Current.Shutdown();
            }
        }
    }
}
