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
using Windows.System;
using Windows.UI.Core;
using System.Diagnostics.Eventing.Reader;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.WS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Interop.Windows.GWL;
using static TerraFX.Interop.Windows.SWP;
using static TerraFX.Interop.Windows.SW;
using System.Reflection.Metadata;
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

        public MainWindow()
        {
            this.InitializeComponent();
            m = new(this);
            unsafe
            {

				var hwnd = (HWND)this.GetWindowHandle();
				int lExStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
				SetWindowLong(hwnd, GWL_EXSTYLE, lExStyle | WS_EX_LAYERED);
			}
            m.WindowMessageReceived += WindowMessageReceived;

            SystemBackdrop = new TransparentBackdrop();
            Content.Background = new SolidColorBrush(Colors.Red);
            Content.Background = new SolidColorBrush(Colors.Transparent);

            ClippyKeyboardListener.Setup(this);

            Collapse();

            this.BringToFront();
			if (Clippy.IsPinned) Pin();
			else Unpin();
			Clippy.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            {
                if(e.PropertyName == "IsPinned")
                {
                    if (Clippy.IsPinned) Pin();
                    else Unpin();
                }
            };
        }

		private unsafe void Pin()
        {
			var presenter = this.AppWindow.Presenter as OverlappedPresenter;
            var hwnd = (HWND)this.GetWindowHandle();
			if (presenter is not null) presenter.IsAlwaysOnTop = true;

			// Add the extended window styles for always on top
			int lExStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
			SetWindowLong(hwnd, GWL_EXSTYLE, lExStyle | WS_EX_TOPMOST);

			// Move window to top
			SetWindowPos(hwnd, HWND.HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
		}

        private unsafe void Unpin()
        {
			var presenter = this.AppWindow.Presenter as OverlappedPresenter;
			var hwnd = (HWND)this.GetWindowHandle();
			if (presenter is not null) presenter.IsAlwaysOnTop = false;

			// Add the extended window styles for always on top
			int lExStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
			SetWindowLong(hwnd, GWL_EXSTYLE, lExStyle & ~WS_EX_TOPMOST);

			this.BringToFront();
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
            App.Current.OpenSettings();
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

            Content.MaxHeight = this.Height;
        }

		// Bool to Visibility
		public Visibility BoolToVis(bool b) => b ? Visibility.Visible : Visibility.Collapsed;

		// Bool to inverted visibility
		public Visibility InvertBoolToVis(bool b) => b ? Visibility.Collapsed : Visibility.Visible;

		private void Background_PointerPressed(object sender, PointerRoutedEventArgs e) => ClippyInputHelper.PointerPress(this.GetWindowHandle());

        private void Background_PointerMoved(object sender, PointerRoutedEventArgs e) => ClippyInputHelper.PointerHover(this.GetWindowHandle());

		private void TextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
		{
			// NOTE - AcceptsReturn is set to true in XAML.
			/*if (e.Key == VirtualKey.Enter)
			{
				// If SHIFT is pressed, this next IF is skipped over, so the default behavior of "AcceptsReturn" is used.
				var keyState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
				if ((keyState & CoreVirtualKeyStates.Down) != CoreVirtualKeyStates.Down)
					e.Handled = true; // Mark the event as handled
			} */
		}

		private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			/*if (e.Key == VirtualKey.Enter)
			{
				// If SHIFT is pressed, this next IF is skipped over, so the default behavior of "AcceptsReturn" is used.
				var keyState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
				if ((keyState & CoreVirtualKeyStates.Down) != CoreVirtualKeyStates.Down)
				{
					// Force update x:Bind text
					Clippy.CurrentText = (sender as TextBox).Text;

					if (Clippy.SendPromptCommand.CanExecute(this))
						Clippy.SendPromptCommand.Execute(this);
				}
			} */
		}

		private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Exit();

		private void Hide_Click(object sender, RoutedEventArgs e)
		{
            this.Hide();
		}
	}
}
