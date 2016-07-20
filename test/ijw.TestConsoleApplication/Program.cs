using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ijw.TestConsoleApplication {
    class Program {
        static void Main(string[] args) {
            ConsoleHelper.ReadLine(Win32.InputLanguage.CurrentInputLanguage.Culture.EnglishName);
            ConsoleHelper.ReadLine("Please change to a IME, and press Enter");
            ConsoleHelper.ReadLine(Win32.InputLanguage.CurrentInputLanguage.Culture.EnglishName);
            ConsoleHelper.CloseIME();
            Console.BufferHeight = 26;
            Console.CursorTop = 22;
            Console.WriteLine("Line: " + Console.CursorTop.ToString() + ". LastLine is: " + (Console.BufferHeight - 1).ToString());
            Console.Write("Line: " + Console.CursorTop.ToString() + ". ");
            //var key = ConsoleHelper.ReadEnterInSeconds("press enter to continue in ", 10, "\n");
            var key = ConsoleHelper.ReadEnterInSeconds("press enter to continue in ", 12, " and of course I had to write a lot of string to test it if there's too many characters in one line, and to see if the count down part work still fine.");
            Console.WriteLine("Line: " + Console.CursorTop.ToString() + ". LastLine is: " + (Console.BufferHeight - 1).ToString());


            ConsoleHelper.ReadLine("\n\nPress enter to exit...");
        }
    }
}
