using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_manager
{
    class ProgramWindow
    {
        public static void Print()
        {
            string horisontalLine = new string('\u2550', Config.WindowWidth - 3);
            for (int i = 1; i < Config.WindowHeigth; i++)
            {
                Console.SetCursorPosition(0, i);
                if (i == 1)
                {
                    Console.Write('\u2554' + horisontalLine + '\u2557');
                }
                else if (i == Config.TreeFieldHeigt + 2 || i == Config.WindowHeigth - 3)
                {
                    Console.Write('\u2560' + horisontalLine + '\u2563');
                }
                else if (i == Config.WindowHeigth - 1)
                {
                    Console.Write('\u255A' + horisontalLine + '\u255D');
                }
                else
                {
                    Console.Write('\u2551');
                    Console.SetCursorPosition(Config.WindowWidth - 2, i);
                    Console.Write('\u2551');
                }
            }
            Console.SetCursorPosition(1, Config.WindowHeigth - 2);
            Console.SetWindowSize(Config.WindowWidth, Config.WindowHeigth);            
        }
        public static bool Confirmation(string message)
        {
            bool confirm = false;
            PrintConfirmationWindow(confirm, message);
            Console.CursorVisible = false;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.RightArrow :
                            confirm = false;
                            PrintConfirmationWindow(confirm, message);
                            break;
                        case ConsoleKey.LeftArrow:
                            confirm = true;
                            PrintConfirmationWindow(confirm, message);
                            break;
                        case ConsoleKey.Enter:
                            return confirm;
                        case ConsoleKey.Escape:
                            return false;
                        case ConsoleKey.Y:
                            return true;
                        case ConsoleKey.N:
                            return false;
                    }
                }
            }
        }
        private static void PrintConfirmationWindow(bool confirm, string message)
        {
            int confirmWindowWidth = message.Length + 6;
            int confirmWindowHeight = 8;
            int leftSide = (Config.WindowWidth - confirmWindowWidth) / 2;
            int topSide = (Config.WindowHeigth - confirmWindowHeight) / 2;
            Console.SetCursorPosition(leftSide, topSide);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            string line = new string('\u2550', confirmWindowWidth - 2);
            for (int i = 0; i < confirmWindowHeight; i++)
            {
                Console.SetCursorPosition(leftSide, Console.CursorTop + 1);
                switch (i)
                {
                    case 0:
                        Console.Write('\u2554' + line + '\u2557');
                        break;
                    case 1:
                    case 3:
                    case 4:
                    case 6:
                        Console.Write('\u2551' + new string(' ', confirmWindowWidth - 2) + '\u2551');
                        break;
                    case 2:
                        Console.Write("\u2551  " + message + "  \u2551");
                        break;
                    case 5:
                        Console.Write('\u2551' + new string(' ', confirmWindowWidth - 2) + '\u2551');
                        Console.SetCursorPosition((Config.WindowWidth - confirmWindowWidth / 2) / 2 - 2, Console.CursorTop);
                        if (confirm)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" Yes ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.SetCursorPosition((Config.WindowWidth + confirmWindowWidth / 2) / 2, Console.CursorTop);
                            Console.Write(" No ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" Yes ");
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.SetCursorPosition((Config.WindowWidth + confirmWindowWidth / 2) / 2, Console.CursorTop);
                            Console.Write(" No ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                        }
                        break;
                    case 7:
                        Console.Write('\u255A' + line + '\u255D');
                        break;
                }
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
