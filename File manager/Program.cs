using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_manager
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Get configuration from file
            
            Config.Initialize();
            #endregion
            string currentPath = @"C:\Users\Temich\AppData";
            TreeWindow treeWindow = new TreeWindow(currentPath);
            //treeWindow.PrintHeader();
            //ProgramWindow.Print();
            treeWindow.PrintTree();
            while (true)
            {
                //if (Console.KeyAvailable)
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                { 
                    case ConsoleKey.PageDown :
                        treeWindow.ChangePage(true);
                        treeWindow.PrintTree();
                        break;
                    case ConsoleKey.PageUp :                
                        treeWindow.ChangePage(false);
                        treeWindow.PrintTree();
                        break;
                    case ConsoleKey.Escape :                
                        if (ProgramWindow.Confirmation("Are you sure to exit program?"))
                        return;
                        Console.CursorVisible = true;
                        treeWindow.PrintTree();
                        break;
                    default:
                        Console.Read();
                        break;
                }
            
            }
        }
        
    }
}
