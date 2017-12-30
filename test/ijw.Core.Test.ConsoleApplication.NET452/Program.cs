using System;
using static ijw.ConsoleHelper;
using static System.Console;

namespace ijw.Core.Test.ConsoleApplication.NET452 {
    class Program {
        public static void Main(string[] args) {
            int testNum = 0;
            do {
                WriteLine("------------Test Menu-----------");
                WriteLine("   1: Test ReadEnterInSeconds.  ");
                WriteLine("   2: Test ReadKeyInSeconds.    ");
                WriteLine("                                ");
                WriteLine("   0: Exit.                     ");
                WriteLine("--------------------------------");

                testNum = ReadLine("Please input a number and enter:").ToIntAnyway(-1);
                switch (testNum) {
                    case -1:
                        WriteLineInColor("not a valid number, please try again.");
                        break;
                    case 0:
                        break;
                    case 1:
                        readEnterInSecondsTest();
                        break;
                    case 2:
                        readKeyInSecondsTest();
                        break;
                    default:
                        break;
                }

            } while (testNum != 0);

            ReadLine("Press enter to exit...");
        }

        private static void readEnterInSecondsTest() {
            WriteLineInColor("ReadEnterInSeconds() test begin:");

            CursorTop = WindowTop + WindowHeight - 3;
            WriteLineInColor($"Set CursorTop to {CursorTop.ToString()}.", ConsoleColor.Green);

            writeLineInfo();
            writeCurrentLineInfo();
            WriteLineInColor("1st test begin...");
            writeCurrentLineInfo();
            var key = ReadLineInSecondsWithThread("press enter to continue in ", 12, "s, and of course I had to write a lot of string to test it if there's too many characters in one line, and to see if the count down part work still fine.");
            writeLineInfo();
            WriteLineInColor("1st test done.\n\n");

            writeCurrentLineInfo();
            WriteLineInColor("2nd test begin...");
            writeCurrentLineInfo();
            key = ReadLineInSecondsWithThread("press enter to continue in ", 20, "s...");
            writeLineInfo();
            WriteLineInColor("2nd test done.\n\n");

            exitTest();
        }

        private static void readKeyInSecondsTest() {
            WriteLineInColor("ReadKeyInSeconds() test begin:");

            CursorTop = WindowTop + WindowHeight - 3;
            WriteLineInColor($"Set CursorTop to {CursorTop.ToString()}.", ConsoleColor.Green);

            writeLineInfo();
            writeCurrentLineInfo();
            WriteLineInColor("1st test begin...");
            writeCurrentLineInfo();
            try {
                var key = ReadKeyInSeconds("press any key to continue in "
                                            , 12
                                            , "s, and of course I had to write a lot of string to test it if there's too many characters in one line, and to see if the count down part work still fine."
                          );
                WriteLine(key.KeyChar);
            }
            catch (TimeoutException te) {
                WriteLine(te.Message);
            }
            writeLineInfo();
            WriteLineInColor("1st test done.\n\n");

            writeCurrentLineInfo();
            WriteLineInColor("2nd test begin...");
            writeCurrentLineInfo();
            try {
                var key = ReadKeyInSeconds("press any key to continue in ", 20, "s...");
                WriteLine(key.KeyChar);
            }
            catch (TimeoutException te) {
                WriteLine(te.Message);
            }

            writeLineInfo();
            WriteLineInColor("2nd test done.\n\n");

            //WriteLineInColor($"Set BufferHeight back to {bh.ToString()}.", ConsoleColor.Green);
            //BufferHeight = bh;

            exitTest();
        }

        private static void exitTest() {
            ReadLine("Press enter to exit test...\n\n");
        }

        private static void writeCurrentLineInfo() {
            WriteInColor("[CursorTop " + CursorTop.ToString() + ", Line " + (CursorTop + 1).ToString() + "] ", ConsoleColor.Green);
        }

        private static void writeLineInfo() {
            writeCurrentLineInfo();
            WriteLineInColor("[LastLine " + BufferHeight.ToString() + "]", ConsoleColor.Green);
        }
    }
}