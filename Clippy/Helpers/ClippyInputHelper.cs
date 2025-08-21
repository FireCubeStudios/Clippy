using CubeKit.UI.Helpers;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using WinUIEx;
using static CubeKit.UI.Helpers.NativeHelper;
using static WindowsInput.Native.SystemMetrics;

namespace Clippy.Helpers
{
    public class ClippyInputHelper
    {
        public static async void PointerPress(IntPtr WindowToIgnore)
        {
            return;
            await Task.Run(async ()  =>    
            {
               // NativeHelper.GetCursorPos(out Point point);

                // Perform hit testing to determine the target
              //  IntPtr hWnd = await GetWindowHandleAtPoint(point, WindowToIgnore);
                
                // Forward the pointer event to the target window
              //  NativeHelper.SendMessage(hWnd, NativeHelper.WM_LBUTTONDOWN, (IntPtr)point.X, (IntPtr)point.Y);
               // NativeHelper.SendMessage(hWnd, NativeHelper.WM_LBUTTONUP, (IntPtr)point.X, (IntPtr)point.Y);
            });
        }

        public static async void PointerHover(IntPtr WindowToIgnore)
        {
            return;
            await Task.Run(async () =>
            {
                NativeHelper.GetCursorPos(out Point point);

                // Perform hit testing to determine the target
                IntPtr hWnd = await GetWindowHandleAtPoint(point, WindowToIgnore);

                // Forward the pointer event to the target window
               NativeHelper.SendMessage(hWnd, NativeHelper.WM_MOUSEMOVE, (IntPtr)point.X, (IntPtr)point.Y);
            });
        }


        public static async Task<IntPtr> GetWindowHandleAtPoint(Point point, IntPtr WindowToIgnore)
        {
            IntPtr hWnd = WindowFromPoint(point);
            await Task.Run(() =>
            {
                

                while (hWnd != IntPtr.Zero && hWnd != WindowToIgnore)
                {
                    RECT rect;
                    GetWindowRect(hWnd, out rect);
                    if (rect.Left <= point.X && rect.Top <= point.Y && rect.Right >= point.X && rect.Bottom >= point.Y)
                    {
                        // Check if there is a child window at the point
                        IntPtr childHwnd = ChildWindowFromPointEx(hWnd, point, GW_CHILD);
                        if (childHwnd != IntPtr.Zero)
                            hWnd = childHwnd;
                        else
                            break;
                    }
                    else
                    {
                        hWnd = GetWindow(hWnd, GW_HWNDNEXT);
                    }
                }

                
            });
            return hWnd;
        }
    }
}
