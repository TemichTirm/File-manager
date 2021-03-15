using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_manager
{
    class Config
    {
        public static int WindowWidth { get; set; }
        public static int WindowHeigth { get; set; }
        public static int TreeFieldHeigt { get; set; }
        public static int InfoFieldHeigt { get; set; }
        public static int InfoFieldStartPosition { get; private set; }
        public static DeepLevel TreeDeepLevel { get; set; }
        public static string DefaultPath { get; set; }
        private static int numOfTreeRows;
        public static List<string> Errors { get; set; }
        public static int NumOfTreeRows
        {
            get { return numOfTreeRows; }
            set { numOfTreeRows = value >= 5 && value <= TreeFieldHeigt ? value : TreeFieldHeigt; }
        }

        public enum DeepLevel
        {
            Low,
            Medium,
            High
        }
        public static void Initialize()
        {
            Console.Title = "File Manager";
            WindowWidth = 150;
            WindowHeigth = 40;
            InfoFieldHeigt = 5;
            TreeFieldHeigt = WindowHeigth - InfoFieldHeigt - 6;
            InfoFieldStartPosition = TreeFieldHeigt + 3;
            TreeDeepLevel = DeepLevel.High;
            NumOfTreeRows = 30;
            Console.SetWindowSize(WindowWidth, WindowHeigth);
            Console.BufferHeight = WindowHeigth;
            Console.BufferWidth = WindowWidth;
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            DefaultPath = @"C:\Users\Temich\source\repos\File manager\Project\File manager\File manager";
            //DefaultPath = @"C:\Users\Temich\AppData\Local\Application Data";
            Errors = new List<string>();
        }
    }
}
