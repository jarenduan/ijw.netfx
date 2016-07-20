using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;
using static ijw.ConsoleHelper;
using static System.Console;

namespace ijw.TestConsoleApp
{
    public class Program
    {
        public static void Main(string[] args) {
            int testNum = 0;
            do {
                WriteLine("------------Test Menu-----------");
                WriteLine("   1: Test ReadEnterInSeconds.  ");
                WriteLine("   2: Test ReadKeyInSeconds.    ");
                WriteLine("                                ");
                WriteLine("   0: Exit.                     ");
                WriteLine("--------------------------------");

                testNum = ReadLine("Please input a number and enter:").ToInt(-1);
                switch (testNum) {
                    case -1:
                        WriteLineInColor("not a valid number, please try again.");
                        break;
                    case 0:
                        break;
                    case 1:
                        ReadEnterInSecondsTest();
                        break;
                    case 2:
                        ReadKeyInSecondsTest();
                        break;
                    default:
                        break;
                }

            } while (testNum != 0);

            ReadLine("Press enter to exit...");
        }

        private static void ReadKeyInSecondsTest() {
            WriteLineInColor("ReadKeyInSeconds() test begin:");

            var bh = BufferHeight;
            BufferHeight = 25;
            CursorTop = 22;
            WriteLineInColor($"Set BufferHeight to {BufferHeight.ToString()}, CursorTop to {CursorTop.ToString()}.", ConsoleColor.Green);

            WriteLineInfo();
            WriteCurrentLineInfo();
            WriteLineInColor("1st test begin...");
            WriteCurrentLineInfo();
            try {
                var key = ReadKeyInSeconds("press enter to continue in "
                                            , 12
                                            , "s, and of course I had to write a lot of string to test it if there's too many characters in one line, and to see if the count down part work still fine."
                          );
                WriteLine(key.KeyChar);
            }
            catch (TimeoutException te) {
                WriteLine(te.Message);
            }
            WriteLineInfo();
            WriteLineInColor("1st test done.\n\n");

            WriteCurrentLineInfo();
            WriteLineInColor("2nd test begin...");
            WriteCurrentLineInfo();
            try {
                var key = ReadKeyInSeconds("press enter to continue in ", 20, "s...");
                WriteLine(key.KeyChar);
            }
            catch (TimeoutException te) {
                WriteLine(te.Message);
            }

            WriteLineInfo();
            WriteLineInColor("2nd test done.\n\n");

            WriteLineInColor($"Set BufferHeight back to {bh.ToString()}.", ConsoleColor.Green);
            BufferHeight = bh;

            ExitTest();
        }

        private static void ReadEnterInSecondsTest() {
            WriteLineInColor("ReadEnterInSeconds() test begin:");

            var bh = BufferHeight;
            BufferHeight = 25;
            CursorTop = 22;
            WriteLineInColor($"Set BufferHeight to {BufferHeight.ToString()}, CursorTop to {CursorTop.ToString()}.", ConsoleColor.Green);

            WriteLineInfo();
            WriteCurrentLineInfo();
            WriteLineInColor("1st test begin...");
            WriteCurrentLineInfo();
            var key = ReadEnterInSeconds("press enter to continue in ", 12, "s, and of course I had to write a lot of string to test it if there's too many characters in one line, and to see if the count down part work still fine.");
            WriteLineInfo();
            WriteLineInColor("1st test done.\n\n");

            WriteCurrentLineInfo();
            WriteLineInColor("2nd test begin...");
            WriteCurrentLineInfo();
            key = ReadEnterInSeconds("press enter to continue in ", 20, "s...");
            WriteLineInfo();
            WriteLineInColor("2nd test done.\n\n");

            WriteLineInColor($"Set BufferHeight back to {bh.ToString()}.", ConsoleColor.Green);
            BufferHeight = bh;

            ExitTest();
        }

        private static void ExitTest() {
            ReadLine("Press enter to exit test...\n\n");
        }

        private static void WriteCurrentLineInfo() {
            WriteInColor("[CursorTop " + CursorTop.ToString() + ", Line " + (CursorTop + 1).ToString() + "] ",ConsoleColor.Green);
        }

        private static void WriteLineInfo() {
            WriteCurrentLineInfo();
            WriteLineInColor("[LastLine " + BufferHeight.ToString() + "]", ConsoleColor.Green);
        }
    }
}
