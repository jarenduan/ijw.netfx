using System;
using System.Threading;
using ijw.Client.Win32;

namespace ijw.Client.WinConsole {
    /// <summary>
    /// 控制台帮助类
    /// </summary>
    public static class ConsoleHelper {
        /// <summary>
        /// 开启控制台窗口
        /// </summary>
        /// <returns></returns>
        public static bool OpenConsole() {
            return Win32Console.AllocConsole();
        }

        /// <summary>
        /// 关闭控制台窗口
        /// </summary>
        /// <returns></returns>
        public static bool CloseConsole() {
            return Win32Console.FreeConsole();
        }

        /// <summary>
        /// 禁用关闭按钮
        /// </summary>
        /// <param name="title">控制台名字</param>
        public static void DisableCloseButton(IntPtr windowHandle) {
            IntPtr closeMenu = Win32Window.GetSystemMenu(windowHandle, IntPtr.Zero);
            Win32Window.RemoveMenu(closeMenu, Win32Window.SC_CLOSE, 0x0);
        }

        /// <summary>
        /// 禁用关闭按钮
        /// </summary>
        public static void DisableCloseButton() {
            IntPtr windowHandle = FindConsoleWindowHandle();
            DisableCloseButton(windowHandle);
        }

        public static IntPtr FindConsoleWindowHandle() {
            string temp = Console.Title;
            Console.Title = Guid.NewGuid().ToString();
            //线程睡眠，确保能够正常FindWindow，否则有时会Find失败。
            Thread.Sleep(100);
            IntPtr windowHandle = Win32Window.FindWindow(null, Console.Title);
            Console.Title = temp;
            return windowHandle;
        }

        public static void CloseIME() {
            Win32Message.PostMessage(FindConsoleWindowHandle(), Win32Message.WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, Win32KeyBoardLayout.LoadKeyboardLayout("0x0409", Win32KeyBoardLayout.KLF_ACTIVATE));
            Thread.Sleep(100);
            //  var handle = Win32API.LoadKeyboardLayout("0x0409", (uint)(KLF.KLF_ACTIVATE | KLF.KLF_SETFORPROCESS));
            //Console.WriteLine("trying...");
            //var a= Win32API.ActivateKeyboardLayout(HKL.HKL_NEXT, (uint)KLF.KLF_SETFORPROCESS);
        }
    }
}