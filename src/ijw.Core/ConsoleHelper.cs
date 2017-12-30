using System;
using System.Diagnostics;
using System.Threading;
using static System.Console;
using static System.Threading.Thread;
#if !NET35
using System.Threading.Tasks;
#endif
namespace ijw {
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
        public static string ReadLine(string message) {
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
        /// 计算字符串将会占据几行
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

        /// <summary>
        /// 使用指定颜色输出字符串
        /// </summary>
        /// <param name="message">输出的信息</param>
        /// <param name="color">前景色, 默认使用红色</param>
        public static void WriteInColor(string message, ConsoleColor color = ConsoleColor.Red) {
            lock (_syncRoot) {
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

#if NET35 || NET40 || NET45
        /// <summary>
        /// 指定秒数倒计时读取回车键，如果到时间没有读取到，自动返回指定值。
        /// </summary>
        /// <param name="frontMessage">在倒计时秒数之前显示的信息</param>
        /// <param name="timeout">指定的倒计时秒数</param>
        /// <param name="behindMessage">在倒计时秒数之后显示的信息，默认为空</param>
        /// <param param name="defaultResult">倒计时结束之后返回的默认值，默认是真</param>
        /// <param name="isShowTimeCountDown">是否显示倒计时</param>
        /// <returns>倒计时内读取到回车键，返回真；读取到非回车键，返回假；没有读取任何按键，返回指定的默认值。</returns>
        /// <remarks>
        /// 当控制台有输入法行时，倒计时信息可能会出现重行的信息，这是因为输入法行占据了一行的console缓冲区，将导致计算行数不同于无输入法的状态；
        /// 由于目前无法实时识别控制台是否存在输入法行，此bug目前无解。
        /// </remarks>
        public static string ReadLineInSecondsWithThread(string frontMessage, int timeout, string behindMessage = "", string defaultResult = "", bool isShowTimeCountDown = true) {
            //Text before time count down
            Write(frontMessage);

            //Remember the position of cursor
            int posx = CursorLeft;
            int posy = CursorTop;

            //Readline in another thread.
            bool enterPressed = false;
            ConsoleKeyInfo key;

            string result = defaultResult;
#pragma warning disable IDE0017 // 简化对象初始化
            var t = new Thread(() => {
                while (!enterPressed) {
                    key = Console.ReadKey(true);
                    enterPressed = key.Key == ConsoleKey.Enter;
                }
                enterPressed = true;
            });
#pragma warning restore IDE0017 // 简化对象初始化

            t.IsBackground = true;
            t.Start();

            //calculateLineNum
            posy = calculateLineNum(behindMessage, posx, posy);

            //loop until timeout
            while (!enterPressed && timeout > 0) {
                //writeTimeoutAndAfterMsg(timeout, behindMessage, isShowTimeCountDown);

                //wait 1s within many little loops, so that key pressing could get in
                int i = 0;
                while (!enterPressed && i < 10) {
                    i++;
                    Sleep(100);
                }

                //do the count down
                timeout--;
            }

            t.Abort();

            //writeTimeoutAndAfterMsg(timeout, behindMessage, isShowTimeCountDown);

            return result;
        }
#endif

        /// <summary>
        /// 指定时间内读取按键，超时没有读取到任何按键将引发异常。
        /// </summary>
        /// <param name="frontMessage">在倒计时秒数之前显示的信息</param>
        /// <param name="timeout">指定的倒计时秒数</param>
        /// <param name="behindMessage">在倒计时秒数之后显示的信息，默认为空</param>
        /// <param name="isShowTimeCountDown">是否显示倒计时</param>
        /// <returns>读取到的键</returns>
        /// <remarks>
        /// 当控制台有输入法行时，倒计时信息可能会出现重行的信息，这是因为输入法行占据了一行的console缓冲区，将导致计算行数不同于无输入法的状态；
        /// 由于目前无法实时识别控制台是否存在输入法行，此bug目前无解。
        /// </remarks>
        public static ConsoleKeyInfo ReadKeyInSeconds(string frontMessage, int timeout, string behindMessage = "", bool isShowTimeCountDown = true) {
            return readKeyInSeconds(frontMessage, timeout, behindMessage, true, ConsoleKey.Enter, isShowTimeCountDown);
        }

        /// <summary>
        /// 指定时间内读取指定按键，超时没有读取到任何按键将引发异常。
        /// </summary>
        /// <param name="frontMessage">在倒计时秒数之前显示的信息</param>
        /// <param name="timeout">指定的倒计时秒数</param>
        /// <param name="behindMessage">在倒计时秒数之后显示的信息，默认为空</param>
        /// <param name="expectedKey">期待的指定按键</param>
        /// <param name="isShowTimeCountDown">是否显示倒计时</param>/// <param name="frontMessage"></param>
        /// <returns></returns>
        public static ConsoleKeyInfo ReadKeyInSeconds(ConsoleKey expectedKey, string frontMessage, int timeout, string behindMessage = "", bool isShowTimeCountDown = true) {
             return readKeyInSeconds(frontMessage, timeout, behindMessage, false, expectedKey, isShowTimeCountDown);
        }

        private static ConsoleKeyInfo readKeyInSeconds(string frontMessage, int timeout, string behindMessage, bool anykey, ConsoleKey expectedKey, bool isShowTimeCountDown) {
            //Text before time count down
            Write(frontMessage);

            //Remember the current cursor position, where the seconds shows.
            int startPosx = CursorLeft;
            int startPosy = calculateLineNum(behindMessage, startPosx, CursorTop);

            //write the timeout and msg for the first time.
            writeTimeoutAndAfterMsg(timeout, behindMessage, isShowTimeCountDown);

            //Remember the end position.
            int endPosX = CursorLeft;
            int endPosY = CursorTop;
           
            //Setting signals
            bool keyPressed = false;
            ConsoleKeyInfo key = new ConsoleKeyInfo();

            //Count down seconds
            while (!keyPressed && timeout > 0) {
                //Count down 1s with 10 little loops, so that key pressing could get in
                int i = 0;
                while (!keyPressed && i < 10) {
                    if (Console.KeyAvailable) {
                        key = Console.ReadKey(true);
                        if (anykey) {
                            keyPressed = true;
                        }
                        else {
                            keyPressed = key.Key == expectedKey;
                        }
                    }
                    else {
                        i++;
                        Sleep(100);
                    }
                }

                timeout--;

                writeTimeoutAndAfterMsg(timeout, behindMessage, isShowTimeCountDown, startPosx, startPosy, ref endPosX, ref endPosY);
            }

            WriteLine();
            if (keyPressed) {
                return key;
            }
            else {
                throw new TimeoutException("No key read in timeout.");
            }
        }

        private static void writeTimeoutAndAfterMsg(int timeout, string behindMessage, bool isShowTimeCountDown) {
            if (isShowTimeCountDown) {
                Write(timeout.ToString());
            }
            Write(behindMessage);
        }

        private static void writeTimeoutAndAfterMsg(int seconds, string behindMessage, bool isShowTimeCountDown, int startPosx, int startPosy, ref int endPosX, ref int endPosY) {
            CursorVisible = false;

            //restore cursor at in front of the second string.
            CursorLeft = startPosx;
            CursorTop = startPosy;

            //print the second string
            if (isShowTimeCountDown) {
                Write(seconds.ToString());
            }

            if (ifShrink(seconds)) {
                //print message after seconds.
                Write($"{behindMessage} \b");
                endPosX = CursorLeft;
                endPosY = CursorTop;
            }
            else {
                CursorLeft = endPosX;
                CursorTop = endPosY;
            }

            CursorVisible = true;
        }

        private static bool ifShrink(int seconds) {
            seconds++;
            int lastTimeLength = seconds.ToString().Length;
            seconds--;
            int thisTimeLength = seconds.ToString().Length;
            return lastTimeLength > thisTimeLength;
        }

        private static int calculateLineNum(string behindMessage, int posx, int posy) {
            //Calculate the line position, in case screen scrolling up
            int countofPostNewLine = NewLineCount(behindMessage, posx);
            int extraLine = (posy + countofPostNewLine) - (Console.BufferHeight - 1);
            if (extraLine > 0) {
                posy -= extraLine;
            }

            return posy;
        }
    }
}