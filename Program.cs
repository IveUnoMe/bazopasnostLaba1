using System;

namespace Pratkicheskaya1
{
    internal static class Program
    {
        private static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Информация о дисках");
                Console.WriteLine("2. Работа с файлами");
                Console.WriteLine("3. Работа с JSON");
                Console.WriteLine("4. Работа с XML");
                Console.WriteLine("5. Работа с ZIP архивом");
                Console.WriteLine("6. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        Drive.ShowDriveInfo();
                        break;
                    case "2":
                        FileHandler.WriteAndReadFile();
                        break;
                    case "3":
                        JsonHandler.CreateJsonFile();
                        break;
                    case "4":
                        XmlHandler.CreateXmlFile();
                        break;
                    case "5":
                        ShowZipMenu();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private static void ShowZipMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Меню работы с ZIP архивом:");
                Console.WriteLine("1. Создать архив");
                Console.WriteLine("2. Добавить файл в архив");
                Console.WriteLine("3. Извлечь архив");
                Console.WriteLine("4. Удалить архив");
                Console.WriteLine("5. Назад");

                switch (Console.ReadLine())
                {
                    case "1":
                        ZipHandler.CreateArchive();
                        break;
                    case "2":
                        ZipHandler.AddToArchive();
                        break;
                    case "3":
                        ZipHandler.ExtractArchive();
                        break;
                    case "4":
                        ZipHandler.RemoveArchive();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }
}






