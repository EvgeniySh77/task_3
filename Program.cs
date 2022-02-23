using System;
using System.IO;

namespace task_2
{
    class WorkWithFiles
    {
        static void Main()
        {       
            Console.Write("Введите путь к папке: ");
            string folder = Console.ReadLine();
            string source = "Исходный размер папки";
            string current = "Текущий размер папки";
            double catalogSize = 0;
            double beforeCleaning, afterCleaning, del;
            double catalogSizeTrans = SizeOfFolder(folder, ref catalogSize);

            if (catalogSizeTrans >= 0)
            {
                beforeCleaning = catalogSize;
                ShowSize(source, folder, catalogSize, catalogSizeTrans);

                ClearDirectory(folder);

                catalogSize = 0;
                catalogSizeTrans = SizeOfFolder(folder, ref catalogSize);

                afterCleaning = catalogSize;
                del = beforeCleaning - afterCleaning;
                Console.WriteLine($"Удалено и освобождено (в байтах) - {del} байт.");
                Console.WriteLine($"Удалено и освобождено (в Мегабайтах) - {Math.Round((double)(del / 1024 / 1024), 1):f0}" +
                                  $" МБ.{Environment.NewLine}");
                ShowSize(current, folder, catalogSize, catalogSizeTrans);
            }

            Console.ReadKey();
        }
        static double SizeOfFolder(string folder, ref double catalogSize)
        {            
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folder);
                if (!directory.Exists)
                    Console.WriteLine($"По данному адресу {folder}, папка {directory.Name} не существует .");
                DirectoryInfo[] directoryArr = directory.GetDirectories();
                FileInfo[] filesArr = directory.GetFiles();                

                foreach (FileInfo files in filesArr)
                {
                    catalogSize = catalogSize + files.Length;
                }

                foreach (DirectoryInfo dirIter in directoryArr)
                {
                    SizeOfFolder(dirIter.FullName, ref catalogSize);
                }

                return Math.Round((double)(catalogSize / 1024 / 1024), 1);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"Ошибка: Директория не найдена - {e.Message}{Environment.NewLine}");
                return -1;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Ошибка: Нет доступа - {e.Message}{Environment.NewLine}");
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}{Environment.NewLine}");
                return 0;
            }
        }

        static void ShowSize(string condition, string folder, double catalogSize, double catalogSizeTrans)
        {
            if (catalogSizeTrans >= 0)
            {
                Console.WriteLine($"{condition} {folder} (в байтах) - {catalogSize} байт.");
                Console.WriteLine($"{condition} {folder} (в Гигабайтах) - {catalogSizeTrans:f0} MБ.");
            }
        }

        static void ClearDirectory(string folder)
        {
            var directory = new DirectoryInfo(folder);            
            DirectoryInfo[] dirs = directory.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                TimeSpan timeDifference = DateTime.Now - dir.LastWriteTime;
                if (timeDifference > TimeSpan.FromMinutes(30))
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (Exception e) { Console.WriteLine($"Ошибка: {e.Message}{Environment.NewLine}"); }
                    continue;
                }
            }
            
            Console.WriteLine();
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                TimeSpan timeDifference = DateTime.Now - file.LastWriteTime;
                if (timeDifference > TimeSpan.FromMinutes(30))
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e) { Console.WriteLine($"Ошибка: {e.Message}{Environment.NewLine}"); }
                    continue;
                }
            }            
        }
    }
}
