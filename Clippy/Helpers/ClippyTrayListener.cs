using H.NotifyIcon.Core;
using System;
using System.Drawing;
using Windows.ApplicationModel;
using Clippy.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using WinUIEx;

namespace Clippy.Helpers
{
    public class ClippyTrayListener
    {
        private static WindowEx Clippy;
        private static ISettingsService Settings = App.Current.Services.GetService<ISettingsService>();
        private static TrayIcon trayIcon = new()
        {
            Icon =
           new Bitmap(
               @$"{Package.Current.InstalledPath}\Assets\Clippy\Clippy.png"
           ).GetHicon(),
            Visibility = IconVisibility.Visible,
        };

        public static void Setup(WindowEx clippy)
        {
           if (!Settings.TrayClippy)
               return;
           Clippy = clippy;

           trayIcon.Create();
           trayIcon.MessageWindow.MouseEventReceived += MessageWindow_MouseEventReceived;
        }

        public static void Recreate()
        {
            trayIcon.Show();
            trayIcon.MessageWindow.MouseEventReceived += MessageWindow_MouseEventReceived;
        }

        public static void Remove()
        {
            trayIcon.MessageWindow.MouseEventReceived -= MessageWindow_MouseEventReceived;
            trayIcon.Hide();
        }

        private static void MessageWindow_MouseEventReceived(object sender, MessageWindow.MouseEventReceivedEventArgs e)
        {
            if (!Settings.TrayClippy)
            {
                trayIcon.MessageWindow.MouseEventReceived -= MessageWindow_MouseEventReceived;
                trayIcon.Hide();
                return;
            }
            if(e.MouseEvent == MouseEvent.IconLeftMouseDown)
            {
                Clippy.Show();
                Clippy.SetForegroundWindow();
                Clippy.BringToFront();
            }
            else if (e.MouseEvent == MouseEvent.IconRightMouseDown)
            {
                SettingsWindow s_window = new SettingsWindow();
                s_window.Activate();
            }
        }
    }
}
