using CubeKit.UI.Helpers;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;
using Windows.Win32;
using WinRT.Interop;
using Clippy.Windows;
using WinUIEx.Messaging;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Microsoft.UI;
using Clippy.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Clippy.Services;
using Clippy.Helpers;
using Clippy.Core.Services;
using Windows.UI.Input.Preview.Injection;
using Windows.UI.Input;
using Windows.Devices.Input;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clippy
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        private SettingsService Settings = (SettingsService)App.Current.Services.GetService<ISettingsService>();
        private ClippyViewModel Clippy = App.Current.Services.GetService<ClippyViewModel>();
        WindowMessageMonitor m;
        HWND Handle;
        WINDOW_EX_STYLE ExStyle
        {
            get => (WINDOW_EX_STYLE)PInvoke.GetWindowLong(Handle, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
            set => _ = PInvoke.SetWindowLong(Handle, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, (int)value);
        }

        public MainWindow()
        {
            this.InitializeComponent();
            m = new(this);
            Handle = new HWND(WindowNative.GetWindowHandle(this));
            ExStyle |= WINDOW_EX_STYLE.WS_EX_LAYERED;
            m.WindowMessageReceived += WindowMessageReceived;

            SystemBackdrop = new TransparentBackdrop();
            Content.Background = new SolidColorBrush(Colors.Red);
            Content.Background = new SolidColorBrush(Colors.Transparent);

            ClippyKeyboardListener.Setup(this);
            ClippyTrayListener.Setup(this);

            Collapse();
        }

        private void WindowMessageReceived(object? sender, WindowMessageEventArgs e)
        {
            if (e.Message.MessageId == PInvoke.WM_ERASEBKGND)
            {
                e.Handled = true;
                e.Result = 1;
            }
        }

        private double GetScale()
        {
            var progmanWindow = NativeHelper.FindWindow("Shell_TrayWnd", null);
            var monitor = NativeHelper.MonitorFromWindow(progmanWindow, NativeHelper.MONITOR_DEFAULTTOPRIMARY);

            NativeHelper.DeviceScaleFactor scale;
            NativeHelper.GetScaleFactorForMonitor(monitor, out scale);

            if (scale == NativeHelper.DeviceScaleFactor.DEVICE_SCALE_FACTOR_INVALID)
                scale = NativeHelper.DeviceScaleFactor.SCALE_100_PERCENT;

            return Convert.ToDouble(scale) / 100;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow s_window = new SettingsWindow();
            s_window.Activate();
        }

        private Visibility BtoV(bool b) => b ? Visibility.Visible : Visibility.Collapsed;

        private void Clippy_Checked(object sender, RoutedEventArgs e) => Expand();

        private void Clippy_Unchecked(object sender, RoutedEventArgs e) => Collapse();

        private void Collapse()
        {
            this.Height = 150;
            this.Width = 150;
            double Scale = GetScale();
            double DisplayHeight = (DisplayArea.Primary.OuterBounds.Height) - 100;
            double DisplayWidth = (DisplayArea.Primary.OuterBounds.Width) - 200;

            double W = this.Width * Scale;
            double H = this.Height * Scale;
            this.MoveAndResize(DisplayWidth - W, DisplayHeight - H, this.Width, this.Height);
        }

        private void Expand()
        {
            this.Height = 1000;
            this.Width = 380;
            double Scale = GetScale();
            double DisplayHeight = (DisplayArea.Primary.OuterBounds.Height) - 100;
            double DisplayWidth = (DisplayArea.Primary.OuterBounds.Width) - 200;

            double W = this.Width * Scale;
            double H = this.Height * Scale;
            this.MoveAndResize(DisplayWidth - W, DisplayHeight - H, this.Width, this.Height);
        }

        private void Background_PointerPressed(object sender, PointerRoutedEventArgs e) => ClippyInputHelper.PointerPress();

        private void Background_PointerMoved(object sender, PointerRoutedEventArgs e) => ClippyInputHelper.PointerHover();
    }
}
