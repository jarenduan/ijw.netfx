using System;
using static ijw.ConsoleHelper;
using static System.Console;

namespace ijw.Core.Test.ConsoleApplication.DotNetCore {

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

        private static void readKeyInSecondsTest() {
            WriteLineInColor("ReadKeyInSeconds() test begin:");

            CursorTop = WindowTop + WindowHeight - 3;
            WriteLineInColor($"Set CursorTop to {CursorTop.ToString()}.", ConsoleColor.Green);

            writeLineInfo();

            writeCurrentLineInfo();
            WriteLineInColor("1st test begin...");
            writeCurrentLineInfo();
            try {
                var key = ReadKeyInSeconds("Press any key to continue in ", 12
                                            , "s... And of course I had to write a lot of string to test it when there's too many characters in one line, and to see if the count down part work still fine."
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
                var key = ReadKeyInSeconds("Press any key to continue in ", 20, "s...");
                WriteLine(key.KeyChar);
            }
            catch (TimeoutException te) {
                WriteLine(te.Message);
            }
            writeLineInfo();
            WriteLineInColor("2nd test done.\n\n");

            writeCurrentLineInfo();
            WriteLineInColor("3rd test begin...");
            writeCurrentLineInfo();
            try {
                var key = ReadKeyInSeconds("(Assuming there's a lot of string in one line, this will test if the counting down works correctly.) Press any key to continue in ", 12
                                            , "s... And of course I had to write a lot of string to test it when there's too many characters in one line, and to see if the count down part work still fine."
                          );
                WriteLine(key.KeyChar);
            }
            catch (TimeoutException te) {
                WriteLine(te.Message);
            }
            writeLineInfo();
            WriteLineInColor("3rd test done.\n\n");
        }

        private static void readEnterInSecondsTest() {
            WriteLineInColor("ReadEnterInSeconds() test begin:");

            CursorTop = WindowTop + WindowHeight - 3;
            WriteLineInColor($"Set CursorTop to {CursorTop.ToString()}.", ConsoleColor.Green);

            writeLineInfo();

            writeCurrentLineInfo();
            WriteLineInColor("1st test begin...");
            writeCurrentLineInfo();
            var key = ReadEnterInSeconds("Press Enter to continue in ", 12, "s, and of course I had to write a lot of string to test it if there's too many characters in one line, and to see if the count down part work still fine.");
            writeLineInfo();
            WriteLineInColor("1st test done.\n\n");

            writeCurrentLineInfo();
            WriteLineInColor("2nd test begin...");
            writeCurrentLineInfo();
            key = ReadEnterInSeconds("Press Enter to continue in ", 20, "s...");
            writeLineInfo();
            WriteLineInColor("2nd test done.\n\n");

            writeCurrentLineInfo();
            WriteLineInColor("3rd test begin...");
            writeCurrentLineInfo();
            key = ReadEnterInSeconds("(Assuming there's a lot of string in one line, this will test if the counting down works correctly.) Press Enter to continue in ", 12
                                        , "s... And of course I had to write a lot of string to test it when there's too many characters in one line, and to see if the count down part work still fine."
                        );
            writeLineInfo();
            WriteLineInColor("3rd test done.\n\n");
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