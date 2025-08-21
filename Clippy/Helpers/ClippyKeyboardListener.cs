using Clippy.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUIEx;

namespace Clippy.Helpers
{
    public class ClippyKeyboardListener
    {
        private static WindowEx Clippy;
        private static bool IsWin = false;

        private static bool IsC = false;

        private const uint VK_WINDOWS = 0x5B;

        private const uint VK_C = 0x43;

        private static KeyboardHelper KeyboardHook;

        private static ISettingsService Settings = App.Current.Services.GetService<ISettingsService>();

        public static void Setup(WindowEx clippy)
        {
            Clippy = clippy;
            KeyboardHook = new KeyboardHelper();
            KeyboardHook.KeyboardPressed += OnKeyPressed;
        }

        private static void OnKeyPressed(object sender, KeyboardHelperEventArgs e)
        {
            if (!Settings.KeyboardEnabled)
                return;
            if (e.KeyboardState == KeyboardHelper.KeyboardState.KeyDown)
            {
                if (e.KeyboardData.VirtualCode == VK_WINDOWS)
                    IsWin = true;
                if (e.KeyboardData.VirtualCode == VK_C)
                    IsC = true;
                if (IsWin && IsC)
                {
                    Clippy.Show();
                    Clippy.SetForegroundWindow();
                    Clippy.BringToFront();
                }
            }
            else if (e.KeyboardState == KeyboardHelper.KeyboardState.KeyUp)
            {
                if (e.KeyboardData.VirtualCode == VK_WINDOWS)
                    IsWin = false;
                if (e.KeyboardData.VirtualCode == VK_C)
                    IsC = false;
            }
        }
    }
}
