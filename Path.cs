using System;
using System.IO;

namespace Pratkicheskaya1
{
    internal static class PathHandler
    {
       

        internal static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine("Нажмите любую клавишу для удаления файла...");
                Console.ReadKey();
                try
                {
                    File.Delete(path);
                    Console.WriteLine($"Файл {path} удален.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при удалении файла: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Файл {path} не найден.");
            }
        }
    }
}