using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ijw.Client.WinConsole.TestConsoleApp {
    class Program {
        static void Main(string[] args) {
            var original = DateTime.Now;
            var newTime = original;

            var waitTime = 10;
            var remainingWaitTime = waitTime;
            var lastWaitTime = waitTime.ToString();
            var keyRead = false;
            Console.Write("Waiting for key press or expiring in " + waitTime);
            do {
                keyRead = Console.KeyAvailable;
                if (!keyRead) {
                    newTime = DateTime.Now;
                    remainingWaitTime = waitTime - (int)(newTime - original).TotalSeconds;
                    var newWaitTime = remainingWaitTime.ToString();
                    if (newWaitTime != lastWaitTime) {
                        var backSpaces = new string('\b', lastWaitTime.Length);
                        var spaces = new string(' ', lastWaitTime.Length);
                        Console.Write(backSpaces + spaces + backSpaces);
                        lastWaitTime = newWaitTime;
                        Console.Write(lastWaitTime);
                        Thread.Sleep(25);
                    }
                }
                else {
                    var keu  = Console.ReadKey();
                    Console.Write(keu.ToString());
                    break;
                }
            } while (remainingWaitTime > 0 && !keyRead);
            Console.Write("\nloop end, press enter to exit");
            Console.ReadLine();

            //ConsoleHelper.ReadLine(Win32.InputLanguage.CurrentInputLanguage.Culture.EnglishName);
            //ConsoleHelper.ReadLine("Please change to a IME, and press Enter");
            //ConsoleHelper.ReadLine(Win32.InputLanguage.CurrentInputLanguage.Culture.EnglishName);
            //ConsoleHelper.CloseIME();
            //Console.BufferHeight = 26;
            //Console.CursorTop = 22;
            //Console.WriteLine("Line: " + Console.CursorTop.ToString() + ". LastLine is: " + (Console.BufferHeight - 1).ToString());
            //Console.Write("Line: " + Console.CursorTop.ToString() + ". ");
            ////var key = ConsoleHelper.ReadEnterInSeconds("press enter to continue in ", 10, "\n");
            //var key = ConsoleHelper.ReadEnterInSeconds("press enter to continue in ", 12, " and of course I had to write a lot of string to test it if there's too many characters in one line, and to see if the count down part work still fine.");
            //Console.WriteLine("Line: " + Console.CursorTop.ToString() + ". LastLine is: " + (Console.BufferHeight - 1).ToString());


            //ConsoleHelper.ReadLine("\n\nPress enter to exit...");
        }
    }
}
