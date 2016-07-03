using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ijw.Client.Win32{
    public class Win32Window    {
        public const uint SC_CLOSE = 0xF060;

        [DllImport("user32")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hwnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        public static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);
    }
}
      
