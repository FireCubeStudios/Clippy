using CubeKit.UI.Helpers;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using WinUIEx;
using static CubeKit.UI.Helpers.NativeHelper;

namespace Clippy.Helpers
{
    public class ClippyInputHelper
    {
        public static void PointerPress()
        {
            NativeHelper.GetCursorPos(out Point point);

            // Perform hit testing to determine the target
            IntPtr hWnd = NativeHelper.WindowFromPoint(point);

            // Forward the pointer event to the target window
            NativeHelper.SendMessage(hWnd, NativeHelper.WM_LBUTTONDOWN, IntPtr.Zero, IntPtr.Zero);
            NativeHelper.SendMessage(hWnd, NativeHelper.WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
        }

        public static void PointerHover()
        {
            NativeHelper.GetCursorPos(out Point point);

            // Perform hit testing to determine the target
            IntPtr hWnd = NativeHelper.WindowFromPoint(point);

            // Forward the pointer event to the target window
            NativeHelper.SendMessage(hWnd, NativeHelper.WM_MOUSEMOVE, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
