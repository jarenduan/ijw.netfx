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
        public static bool ReadEnterInSecondsWithThread(string msgBeforeSeconds, int seconds, string msgAfterSeconds = "", bool defaultResult = true, bool isShowTimeCountDown = true) {
            //Text before time count down
            Write(msgBeforeSeconds);

            //Remember the position of cursor
            int posx = CursorLeft;
            int posy = CursorTop;

            //Readline in another thread.
            bool stop = false;
            bool hasEnter = false;
            ConsoleKeyInfo key;

#pragma warning disable IDE0017 // 简化对象初始化
            var t = new Thread(() => {
                key = Console.ReadKey(true);
                hasEnter = key.Key == ConsoleKey.Enter;
                stop = true;
            });
#pragma warning restore IDE0017 // 简化对象初始化

            t.IsBackground = true;
            t.Start();

            int countofPostNewLine = NewLineCount(msgAfterSeconds, posx);
            int extraLine = (posy + countofPostNewLine) - (Console.BufferHeight - 1);
            if (extraLine > 0) {
                posy -= extraLine;
            }

            //Count down seconds
            while (!stop && seconds > 0) {
                writeSecondAndAfterMsg(seconds, msgAfterSeconds, isShowTimeCountDown);


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

            writeSecondAndAfterMsg(seconds, msgAfterSeconds, isShowTimeCountDown);

            if (stop) {
                WriteLine();
                return hasEnter;
            }

            return defaultResult;
        }
#endif

        /// <summary>
        /// 在指定的超时时间内读取回车键
        /// </summary>
        /// <param name="msgBeforeSeconds">显示在超时时间之前的一段文本信息</param>
        /// <param name="timeout">超时时间, 单位是秒</param>
        /// <param name="msgAfterSeconds">显示在超时时间之后的一段文本信息，默认为空</param>
        /// <param param name="defaultResult">超时后返回的默认值，默认是真</param>
        /// <param name="isShowTimeCountDown">是否以倒计时方式显示时间</param>
        /// <returns>倒计时内读取到回车键，返回真；读取到非回车键，返回假；没有读取任何按键，返回指定的默认值。</returns>
        /// <remarks>
        /// 当控制台有输入法行时，倒计时信息可能会出现重行的信息，这是因为输入法行占据了一行的console缓冲区，将导致计算行数不同于无输入法的状态；
        /// 由于目前无法实时识别控制台是否存在输入法行，此bug目前无解。
        /// </remarks>
        public static bool ReadEnterInSeconds(string msgBeforeSeconds, int timeout, string msgAfterSeconds = "", bool defaultResult = true, bool isShowTimeCountDown = true) {
            //Setting signals
            bool shouldStop = false;
            bool hasEnter = false;

            Write(msgBeforeSeconds);

            //Remember the position of cursor, where the seconds string shows.
            int posx = CursorLeft;
            int posy = CursorTop;

            //Calculate the line position, in case screen scrolling up
            int countofPostNewLine = NewLineCount(msgAfterSeconds, posx);
            int extraLine = (posy + countofPostNewLine) - (Console.BufferHeight - 1);
            if (extraLine > 0) {
                posy -= extraLine;
            }

            writeSecondAndAfterMsg(timeout, msgAfterSeconds, isShowTimeCountDown);

            //Time count down
            while (!shouldStop && timeout > 0) {
                //1s with 10 little loops, so that key pressing could get in
                int i = 0;
                while (!shouldStop && i < 10) {
                    if (Console.KeyAvailable) {
                        var key = Console.ReadKey(true);
                        hasEnter = key.Key == ConsoleKey.Enter;
                        shouldStop = true;
                    }
                    else {
                        i++;
                        Sleep(100);
                    }
                }

                //1s passed, time digital shrink
                int lastTimeLength = timeout.ToString().Length;
                timeout--;
                int thisTimeLength = timeout.ToString().Length;

                //offset the digital shrink
                if (lastTimeLength > thisTimeLength) {
                    //with a space and a backspace
                    msgAfterSeconds = " \b" + msgAfterSeconds + " \b";
                }

                //restore cursor at in front of the second string.
                CursorLeft = posx;
                CursorTop = posy;

                writeSecondAndAfterMsg(timeout, msgAfterSeconds, isShowTimeCountDown);
            }

            WriteLine();

            if (shouldStop) {
                return hasEnter;
            }
            else {
                return defaultResult;
            }
        }
        private static void writeSecondAndAfterMsg(int seconds, string msgAfterSeconds, bool isShowTimeCountDown) {
            //print the second string
            if (isShowTimeCountDown) {
                Write(seconds.ToString());
            }
            //print message after seconds.
            Write(msgAfterSeconds);
        }

        /// <summary>
        /// 指定时间内读取按键，超时没有读取到任何按键将引发异常。
        /// </summary>
        /// <param name="msgBeforeSeconds">在倒计时秒数之前显示的信息</param>
        /// <param name="timeout">指定的倒计时秒数</param>
        /// <param name="msgAfterSeconds">在倒计时秒数之后显示的信息，默认为空</param>
        /// <param name="isShowTimeCountDown">是否显示倒计时</param>
        /// <returns>读取到的键</returns>
        /// <remarks>
        /// 当控制台有输入法行时，倒计时信息可能会出现重行的信息，这是因为输入法行占据了一行的console缓冲区，将导致计算行数不同于无输入法的状态；
        /// 由于目前无法实时识别控制台是否存在输入法行，此bug目前无解。
        /// </remarks>
        public static ConsoleKeyInfo ReadKeyInSeconds(string msgBeforeSeconds, int timeout, string msgAfterSeconds = "", bool isShowTimeCountDown = true) {
            //Setting signals
            bool shouldStop = false;
            ConsoleKeyInfo key = new ConsoleKeyInfo();

            //Text before time count down
            Write(msgBeforeSeconds);

            //Remember the current cursor position, where the seconds shows.
            int posx = CursorLeft;
            int posy = CursorTop;

            //Calculate the line position, in case screen scrolling up
            int countofPostNewLine = NewLineCount(msgAfterSeconds, posx);
            int extraLine = (posy + countofPostNewLine) - (Console.BufferHeight - 1);
            if (extraLine > 0) {
                posy -= extraLine;
            }

            writeSecondAndAfterMsg(timeout, msgAfterSeconds, isShowTimeCountDown);

            //Count down seconds
            while (!shouldStop && timeout > 0) {
                //Count down 1s with 10 little loops, so that key pressing could get in
                int i = 0;
                while (!shouldStop && i < 10) {
                    if (Console.KeyAvailable) {
                        key = Console.ReadKey(true);
                        shouldStop = true;
                    }
                    else {
                        i++;
                        Sleep(100);
                    }
                }

                //1s passed, digital shrink
                int lastTimeLength = timeout.ToString().Length;
                timeout--;
                int thisTimeLength = timeout.ToString().Length;

                //offset the digital shrink
                if (lastTimeLength > thisTimeLength) {
                    //with a space and a backspace
                    msgAfterSeconds = " \b" + msgAfterSeconds + " \b";
                }

                //restore cursor at in front of second string.
                CursorLeft = posx;
                CursorTop = posy;

                writeSecondAndAfterMsg(timeout, msgAfterSeconds, isShowTimeCountDown);
            }

            WriteLine();
            if (shouldStop) {
                return key;
            }
            else {
                throw new TimeoutException("No key read in timeout.");
            }
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
    }
}