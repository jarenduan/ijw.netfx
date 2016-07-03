using System;
using System.Runtime.InteropServices;

namespace ijw.Client.Win32 {
    public class Win32Message {
        public const uint WM_INPUTLANGCHANGEREQUEST = 0x0050;
        public const uint WM_PARENTNOTIFY = 0x210;
        public const uint WM_KEYDOWN = 0x0100;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    }
}
