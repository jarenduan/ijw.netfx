using System;
using System.Diagnostics;
using System.Threading;
using ijw.Client.Win32;
using static System.Console;
using static System.Threading.Thread;

namespace ijw.Client.WinConsole {
    /// <summary>
    /// 控制台帮助类
    /// </summary>
    public static class ConsoleHelper {
        private static Object _syncRoot = new object();

        /// <summary>
        /// 先提供一个提示信息, 再接受用户输入.
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        public static string ReadLine(string message)
        {
            Write(message);
            return Console.ReadLine();
        }

        /// <summary>
        /// 当Debug模式才会暂停，读取一个字符. 通常放在程序结束最后，用于调试时暂停.
        /// </summary>
        [Conditional("DEBUG")]
        public static void PauseInDebugMode() {
            Console.ReadKey();
        }

        /// <summary>
        /// 使用指定颜色输出字符串
        /// </summary>
        /// <param name="message">输出的信息</param>
        /// <param name="color">前景色, 默认使用红色</param>
        public static void WriteInColor(string message, ConsoleColor color = ConsoleColor.Red)
        {
            lock(_syncRoot) {
                var c = ForegroundColor;
                ForegroundColor = color;
                Write(message);
                ForegroundColor = c;
            }
        }

        /// <summary>
        /// 使用指定颜色输出字符串, 并换行.
        /// </summary>
        /// <param name="message">输出的信息</param>
        /// <param name="color">前景色, 默认使用红色</param>
        public static void WriteLineInColor(string message, ConsoleColor color = ConsoleColor.Red) {
            WriteInColor(message + "\n", color);
        }
        
        /// <summary>
        /// 输出一行提示信息, 然后等待接收一个按键.
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <param name="ifIntercept"></param>
        /// <returns></returns>
        public static ConsoleKeyInfo ReadKey(string message, bool ifIntercept = false) {
            WriteLine(message);
            return Console.ReadKey(ifIntercept);
        }

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

        /// <summary>
        /// 指定秒数倒计时读取回车键，如果到时间没有读取到，自动返回指定值。
        /// </summary>
        /// <param name="msgBeforeSeconds">在倒计时秒数之前显示的信息</param>
        /// <param name="seconds">指定的倒计时秒数</param>
        /// <param name="msgAfterSeconds">在倒计时秒数之后显示的信息，默认为空</param>
        /// <param param name="defaultResult">倒计时结束之后返回的默认值，默认是真</param>
        /// <param name="isShowTimeCountDown">是否显示倒计时</param>
        /// <returns>倒计时内读取到回车键，返回真；读取到非回车键，返回假；没有读取任何按键，返回指定的默认值。</returns>
        /// <remarks>
        /// 当控制台有输入法行时，倒计时信息可能会出现重行的信息，这是因为输入法行占据了一行的console缓冲区，将导致计算行数不同于无输入法的状态；
        /// 由于目前无法实时识别控制台是否存在输入法行，此bug目前无解。
        /// </remarks>
        public static bool ReadEnterInSeconds(string msgBeforeSeconds, int seconds, string msgAfterSeconds = "", bool defaultResult = true, bool isShowTimeCountDown = true) {
            //Text before time count down
            Write(msgBeforeSeconds);

            //Remember the position of cursor
            int posx = CursorLeft;
            int posy = CursorTop;

            //Readline in another thread.
            bool stop = false;
            bool hasEnter = false;
            ConsoleKeyInfo key;
            var t = new Thread(() => {
                key = Console.ReadKey(true);
                hasEnter = key.Key == ConsoleKey.Enter;
                stop = true;
            });
            t.IsBackground = true;
            t.Start();

            int countofPostNewLine = NewLineCount(msgAfterSeconds, posx);
            int extraLine = (posy + countofPostNewLine) - (Console.BufferHeight - 1);
            if (extraLine > 0) {
               posy -= extraLine;
            }
            
            //Count down seconds
            while (!stop && seconds > 0) {
                //print the second string
                if (isShowTimeCountDown) {
                    Write(seconds.ToString());
                }
                //print message after seconds.
                Write(msgAfterSeconds);

                //count down 1s within many little loops, so that key pressing could get in
                int i = 0;
                while (!stop && i < 10) {
                    i++;
                    Sleep(100);
                }
                
                //1s passed, digital shrink
                int lastTimeLength = seconds.ToString().Length;
                seconds--;
                int thisTimeLength = seconds.ToString().Length;

                //offset the digital shrink
                if (lastTimeLength > thisTimeLength) {
                    //with a space and a backspace
                    msgAfterSeconds = " \b" + msgAfterSeconds + " \b";
                }

                //restore cursor at in front of second string.
                CursorLeft = posx;
                CursorTop = posy;
            }

            t.Abort();

            //print the second string
            if (isShowTimeCountDown) {
                Write(seconds.ToString());
            }
            //print message after seconds.
            Write(msgAfterSeconds);

            if (stop) {
                WriteLine();
                return hasEnter;
            }

            var oldTitle = Title;
            Title = Guid.NewGuid().ToString();
            Sleep(100);
            IntPtr windowHandle = Win32Window.FindWindow(null, Title);
            Win32Message.PostMessage(windowHandle, Win32Message.WM_KEYDOWN, Win32VirtualKey.VK_RETURN, 0);
            Title = oldTitle;

            return defaultResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="startCursorX"></param>
        /// <returns></returns>
        /// <remarks>
        /// 非线程安全，依赖windowswidth属性。
        /// </remarks>
        public static int NewLineCount(string msg, int startCursorX) {
            string[] paras = msg.Split('\n');
            int newLineChar = paras.Length - 1;
            int line = newLineChar;
            for (int i = 0; i < paras.Length; i++) {
                int paraLength = paras[i].Length;
                if (i == 0) {
                    paraLength += startCursorX;
                }
                line += paraLength / Console.WindowWidth;
            }

            return line;
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