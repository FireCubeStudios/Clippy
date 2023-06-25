using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CubeKit.UI.Helpers
{
    public static class NativeHelper
    {
        public const int MONITOR_DEFAULTTOPRIMARY = 1;
        public const int MONITOR_DEFAULTTONEAREST = 2;

        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_MOUSEMOVE = 0x0200;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("shcore.dll")]
        public static extern IntPtr GetScaleFactorForMonitor(IntPtr hwnd, out DeviceScaleFactor dwFlags);

        public enum DeviceScaleFactor
        {
            DEVICE_SCALE_FACTOR_INVALID = 0,
            SCALE_100_PERCENT = 100,
            SCALE_120_PERCENT = 120,
            SCALE_125_PERCENT = 125,
            SCALE_140_PERCENT = 140,
            SCALE_150_PERCENT = 150,
            SCALE_160_PERCENT = 160,
            SCALE_175_PERCENT = 175,
            SCALE_180_PERCENT = 180,
            SCALE_200_PERCENT = 200,
            SCALE_225_PERCENT = 225,
            SCALE_250_PERCENT = 250,
            SCALE_300_PERCENT = 300,
            SCALE_350_PERCENT = 350,
            SCALE_400_PERCENT = 400,
            SCALE_450_PERCENT = 450,
            SCALE_500_PERCENT = 500,
        }

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point point);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out Point lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;
        }
    }
}
