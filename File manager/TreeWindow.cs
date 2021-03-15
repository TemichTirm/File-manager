using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_manager
{
    class TreeWindow
    {
        public string CurrentPath { get; set; }
        private int CurrentPage { get; set; }
        private int Pages { get; set; }
        private List<(string value, byte layer, bool isFinal)> Tree { get; set; }
        private string[] Info { get; set; }
        private bool IsLayer1Finished { get; set; }
        private bool IsLayer2Finished { get; set; }
        private DriveInfo CurrentDrive { get; set; }
        public TreeWindow(string path)
        {
            CurrentPath = path;
            Tree = new List<(string, byte, bool)>();
            Info = new string[5];
            CreateTree();
        }
        public void ChangePage(bool direction)
        {
            if (direction)
            {
                CurrentPage = CurrentPage < Pages - 1 ? CurrentPage + 1 : CurrentPage;
            }
            else
            {
                CurrentPage = CurrentPage > 0 ? CurrentPage - 1 : CurrentPage;
            }
        }
        private void CreateTree()
        {

            DirectoryInfo directory = new DirectoryInfo(CurrentPath);
            DirectoryInfo[] subDirsL1 = GetSubDirectoryInfos(directory);

            if (subDirsL1 == null)
            {
                CurrentPath = Config.DefaultPath;
                directory = new DirectoryInfo(CurrentPath);
                subDirsL1 = GetSubDirectoryInfos(directory);
            }
            CurrentDrive = GetDrive(directory);
            FileInfo[] filesL1 = directory.GetFiles();
            DirInfo(directory);
            bool isFinal = false;
            if (directory.Parent != null)
                Tree.Add(("..", 0, isFinal));
            else
                Tree.Add((CurrentPath, 0, isFinal));
            int countL1 = 0;
            foreach (var dir1 in subDirsL1)
            {
                isFinal = countL1 == subDirsL1.Length - 1 && filesL1.Length == 0;
                Tree.Add((dir1.ToString(), 1, isFinal));
                countL1++;
                if (Config.TreeDeepLevel > Config.DeepLevel.Low)
                {
                    DirectoryInfo[] subDirsL2 = GetSubDirectoryInfos(dir1);
                    if (subDirsL2 == null)
                    {
                        Tree.RemoveAt(Tree.Count - 1);
                        Tree.Add((dir1.ToString() + $" ({Config.Errors.Last()})", 1, isFinal));
                        continue;
                    }
                    FileInfo[] filesL2 = dir1.GetFiles();
                    int countL2 = 0;
                    foreach (var dir2 in subDirsL2)
                    {
                        isFinal = countL2 == subDirsL2.Length - 1 && filesL2.Length == 0;
                        Tree.Add((dir2.ToString(), 2, isFinal));
                        countL2++;
                        if (Config.TreeDeepLevel == Config.DeepLevel.High)
                        {
                            DirectoryInfo[] subDirsL3 = GetSubDirectoryInfos(dir2);
                            if (subDirsL3 == null)
                            {
                                Tree.RemoveAt(Tree.Count - 1);
                                Tree.Add((dir2.ToString() + $" ({Config.Errors.Last()})", 2, isFinal));
                                continue;
                            }
                            FileInfo[] filesL3 = dir2.GetFiles();
                            int countL3 = 0;
                            foreach (var dir3 in subDirsL3)
                            {
                                isFinal = countL3 == subDirsL3.Length - 1 && filesL3.Length == 0;
                                Tree.Add((dir3.ToString(), 3, isFinal));
                                countL3++;
                            }
                            countL3 = 0;
                            foreach (var file3 in filesL3)
                            {
                                isFinal = countL3 == filesL3.Length - 1;
                                Tree.Add((file3.ToString(), 3, isFinal));
                                countL3++;
                            }
                        }
                    }
                    countL2 = 0;
                    foreach (var file2 in filesL2)
                    {
                        isFinal = countL2 == filesL2.Length - 1;
                        Tree.Add((file2.ToString(), 2, isFinal));
                        countL2++;
                    }
                }
            }
            countL1 = 0;
            foreach (var file1 in filesL1)
            {
                isFinal = countL1 == filesL1.Length - 1;
                Tree.Add((file1.ToString(), 1, isFinal));
                countL1++;
            }
            Pages = Tree.Count / Config.NumOfTreeRows;
            Pages = Tree.Count % Config.NumOfTreeRows != 0 ? Pages + 1 : Pages;

        }
        private DirectoryInfo[] GetSubDirectoryInfos(DirectoryInfo dir)
        {
            try
            {
                return dir.GetDirectories();
            }
            catch(Exception e)           // Обработка исключения отсутствия авторизации
            {
                Config.Errors.Add(e.Message);
                return null;
            }
           
        }
        public void PrintTree()
        {
            Console.OutputEncoding = Encoding.Unicode;            
            Console.Clear();
            PrintHeader();
            ProgramWindow.Print();
            string pageMarker = $" page {CurrentPage + 1} of {Pages} (\u25B2 PgUp/PgDn \u25BC) ";
            Console.SetCursorPosition((Config.WindowWidth - pageMarker.Length) / 2, 1);
            Console.Write(pageMarker);
            Console.SetCursorPosition(1, 2);
            //bool IsLayer1Finished = true;
            //bool IsLayer2Finished = false;
            int start = CurrentPage * Config.NumOfTreeRows;
            int end = CurrentPage == Pages - 1 ? Tree.Count : start + Config.NumOfTreeRows;
            for (int i = start; i < end; i++)
            {                
                if (start != 0)
                    for (int j = 0; j < i; j++)
                    {
                        switch (Tree[j].layer)
                        {
                            case 1:
                                IsLayer1Finished = Tree[j].isFinal;
                                break;
                            case 2:
                                IsLayer2Finished = Tree[j].isFinal;
                                break;
                        }
                    }
                if (i != 0)
                    Console.SetCursorPosition(3, Console.CursorTop);
                string line = null;
                switch (Tree[i].layer)
                {
                    case 1:
                        IsLayer1Finished = Tree[i].isFinal;
                        line = Tree[i].isFinal ? "\u2514 " : "\u251C ";
                        break;
                    case 2:
                        IsLayer2Finished = Tree[i].isFinal;
                        if (!IsLayer1Finished)
                        {
                            Console.Write("\u2502  ");
                        }
                        else
                            Console.Write("   ");
                        line = Tree[i].isFinal ? "\u2514 " : "\u251C ";
                        break;
                    case 3:
                        if (!IsLayer1Finished)
                        {
                            if (!IsLayer2Finished)
                                Console.Write("\u2502  \u2502  ");
                            else
                                Console.Write("\u2502     ");
                        }
                        else if (!IsLayer2Finished)
                            Console.Write("   \u2502  ");
                        else
                            Console.Write("      ");
                        line = Tree[i].isFinal ? "\u2514 " : "\u251C ";
                        break;
                }
                Console.Write(line);
                Console.WriteLine(Tree[i].value);
            }
            PrintDirInfo(Info);
            Console.SetCursorPosition(1, Config.WindowHeigth - 2);
        }
        public void PrintHeader()
        {
            (int x, int y) currentCursorPosition = (Console.CursorLeft, Console.CursorTop);
            Console.SetCursorPosition(1, 0);
            Console.WriteLine(CurrentPath);
            Console.SetCursorPosition(currentCursorPosition.x, currentCursorPosition.y);
        }
        public void PrintDirInfo(string[] info)
        {
            (int x, int y) currentCursorPosition = (Console.CursorLeft, Console.CursorTop);
            Console.SetCursorPosition(Config.WindowWidth/2 - 3, Config.InfoFieldStartPosition - 1);
            Console.WriteLine(" Info ");
            for (int i = 0; i < info.Length; i++)
            {
                Console.SetCursorPosition(2, Config.InfoFieldStartPosition + i);
                Console.Write(info[i]);
            }
            Console.SetCursorPosition(currentCursorPosition.x, currentCursorPosition.y);
        }
        private void DirInfo(DirectoryInfo dir)
        {
            Info[0] =   $"Drive: {CurrentDrive.Name}\t " +
                        $"Total space: {(CurrentDrive.TotalSize / 1024).ToString("#,##0")} KBytes\t " +
                        $"Available space: {(CurrentDrive.AvailableFreeSpace / 1024).ToString("#,##0")} KBytes";
            Info[1] = $"Directory: {dir.Name}";
            Info[2] = $"Created: {dir.CreationTime}";
        }
        private DriveInfo GetDrive (DirectoryInfo dir)
        {
            if (dir.Parent != null)
                return (GetDrive(dir.Parent));
            else
                return new DriveInfo(dir.Name);
        }
    }
}
