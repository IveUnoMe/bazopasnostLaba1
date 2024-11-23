using System;
using System.IO;

namespace Pratkicheskaya1
{
    internal static class FileHandler
    {
        private static readonly string AllowedDirectory = @"C:\Users\Vecheslav-PC\source\repos\Pratkicheskaya1";

        internal static void WriteAndReadFile()
        {
            if (!Directory.Exists(AllowedDirectory))
            {
                Directory.CreateDirectory(AllowedDirectory);
            }

            Console.WriteLine("Введите название файла (без указания пути):");
            string fileName = Console.ReadLine();

            string fullPath = Path.Combine(AllowedDirectory, fileName);

            Console.WriteLine($"Файл будет сохранён по следующему пути: {Path.GetFullPath(fullPath)}");

            Console.WriteLine("Введите строку для записи в файл:");
            string text = Console.ReadLine();

            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath, false))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        sw.WriteLine(text);
                    }
                }

                Console.WriteLine("\nФайл успешно создан.");

                Console.WriteLine("\nСодержимое файла:");
                using (StreamReader sr = new StreamReader(fullPath))
                {
                    string fileContent = sr.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(fileContent))
                    {
                        Console.WriteLine("Файл пуст.");
                    }
                    else
                    {
                        Console.WriteLine(fileContent);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return;
            }

            try
            {
                PathHandler.DeleteFile(fullPath);
                Console.WriteLine("\nФайл был успешно удалён.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении файла: {ex.Message}");
            }
        }
    }
}
