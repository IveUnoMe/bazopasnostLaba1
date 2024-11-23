using System;
using System.IO;
using System.IO.Compression;

namespace Pratkicheskaya1
{
    public static class ZipHandler
    {
        private const long MaxUnzipSize = 500 * 1024 * 1024; 
        private static readonly string BaseDirectory = @"C:\Users\Vecheslav-PC\source\repos\Pratkicheskaya1";

        private static bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name) && !name.Contains("\\") && !name.Contains("/");
        }

        private static string RequestValidName(string prompt)
        {
            string name;
            while (true)
            {
                Console.Write($"{prompt} (или введите 0 для выхода): ");
                name = Console.ReadLine();

                if (name == "0")
                    return null; 
                if (IsValidName(name))
                    return name; 
                Console.WriteLine("Ошибка: имя не должно содержать путь. Укажите только название.");
            }
        }

        public static void CreateArchive()
        {
            string zipName = RequestValidName("Введите имя архива для создания");
            if (zipName == null) return;

            string zipPath = Path.Combine(BaseDirectory, $"{zipName}.zip");
            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                Console.WriteLine($"Архив {zipName}.zip создан в {BaseDirectory}");
            }
        }

        public static void AddToArchive()
        {
            string zipName = RequestValidName("Введите имя архива для добавления файла");
            if (zipName == null) return;

            string zipPath = Path.Combine(BaseDirectory, $"{zipName}.zip");
            if (!File.Exists(zipPath))
            {
                Console.WriteLine($"Ошибка: архив {zipName}.zip не найден.");
                return;
            }

            string fileName = RequestValidName("Введите имя файла для добавления");
            if (fileName == null) return;

            string filePath = Path.Combine(BaseDirectory, fileName);
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Ошибка: файл {fileName} не найден в {BaseDirectory}.");
                return;
            }

            using (var zip = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                zip.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                Console.WriteLine($"Файл {fileName} добавлен в архив {zipName}.zip и удален из директории.");
            }

            File.Delete(filePath); 
        }

        public static bool IsZipBomb(string zipName)
        {
            string zipPath = Path.Combine(BaseDirectory, $"{zipName}.zip");
            long totalUnzippedSize = 0;

            using (var zip = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in zip.Entries)
                {
                    totalUnzippedSize += entry.Length;
                }

                if (totalUnzippedSize > MaxUnzipSize)
                {
                    Console.WriteLine($"Ошибка: архив {zipName}.zip может быть zip-бомбой!");
                    return true;
                }
            }

            return false;
        }

        public static void ExtractArchive()
        {
            string zipName = RequestValidName("Введите имя архива для извлечения");
            if (zipName == null) return;

            string destFolder = RequestValidName("Введите папку для извлечения");
            if (destFolder == null) return;

            string zipPath = Path.Combine(BaseDirectory, $"{zipName}.zip");
            if (!File.Exists(zipPath))
            {
                Console.WriteLine($"Ошибка: архив {zipName}.zip не найден.");
                return;
            }

            if (IsZipBomb(zipName))
            {
                Console.WriteLine($"Архив {zipName}.zip не будет извлечён из-за риска zip-бомбы.");
                return;
            }

            string destPath = Path.Combine(BaseDirectory, destFolder);
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            using (var zip = ZipFile.OpenRead(zipPath))
            {
                zip.ExtractToDirectory(destPath);
                Console.WriteLine($"Архив {zipName}.zip успешно извлечён в папку {destFolder}");
                Console.WriteLine("Список извлечённых файлов:");
                foreach (var entry in zip.Entries)
                {
                    Console.WriteLine($" - {entry.FullName}");
                }
            }

            Console.WriteLine("Нажмите любую клавишу для удаления архива и извлечённых файлов...");
            Console.ReadKey();

            File.Delete(zipPath);
            Console.WriteLine($"Архив {zipName}.zip удалён.");

            foreach (string file in Directory.GetFiles(destPath))
            {
                File.Delete(file);
            }
            Console.WriteLine("Все извлечённые файлы удалены.");
        }

        public static void RemoveArchive()
        {
            string zipName = RequestValidName("Введите имя архива для удаления");
            if (zipName == null) return;

            string zipPath = Path.Combine(BaseDirectory, $"{zipName}.zip");
            if (!File.Exists(zipPath))
            {
                Console.WriteLine($"Ошибка: архив {zipName}.zip не найден.");
                return;
            }

            File.Delete(zipPath);
            Console.WriteLine($"Архив {zipName}.zip удалён из {BaseDirectory}");
        }
    }
}





