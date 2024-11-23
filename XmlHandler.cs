using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Pratkicheskaya1
{
    internal static class XmlHandler
    {
        private static readonly string AllowedDirectory = @"C:\Users\Vecheslav-PC\source\repos\Pratkicheskaya1";

        private static Dictionary<string, string> CreateNestedXmlElement()
        {
            var nestedElements = new Dictionary<string, string>();
            Console.WriteLine("Для остановки записи вложенных элементов введите '0'.");

            while (true)
            {
                string tag;
                while (true)
                {
                    Console.Write("Введите название вложенного элемента: ");
                    tag = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(tag))
                        break;
                    Console.WriteLine("Некорректное название элемента! Пожалуйста, введите название ещё раз.");
                }

                if (tag == "0")
                    break;

                string value;
                while (true)
                {
                    Console.Write($"Введите значение для элемента '{tag}': ");
                    value = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(value))
                        break;
                    Console.WriteLine($"Некорректное значение! Пожалуйста, введите значение для элемента '{tag}' ещё раз.");
                }

                nestedElements[tag] = value;
            }

            return nestedElements;
        }

        public static bool AddElementToXml(XElement root)
        {
            string tag;
            while (true)
            {
                Console.Write("Введите название элемента: ");
                tag = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(tag))
                    break;
                Console.WriteLine("Некорректное название элемента! Пожалуйста, введите название ещё раз.");
            }

            if (tag == "0")
                return false;

            string valueType;
            while (true)
            {
                Console.Write("Значение элемента является словарем? Да - 1, Нет - 2: ");
                valueType = Console.ReadLine();
                if (valueType == "1" || valueType == "2")
                    break;
                else
                    Console.WriteLine("Ошибка! Пожалуйста, введите '1' для словаря или '2' для обычного значения.");
            }

            XElement element = new XElement(tag);

            if (valueType == "1")
            {
                string generalValue;
                while (true)
                {
                    Console.Write($"Введите значение для элемента-словаря '{tag}': ");
                    generalValue = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(generalValue))
                        break;
                    Console.WriteLine($"Некорректное значение! Пожалуйста, введите значение для элемента-словаря '{tag}' ещё раз.");
                }

                element.Add(new XText(generalValue));  

                Dictionary<string, string> nestedElements = CreateNestedXmlElement();

                foreach (var nestedEntry in nestedElements)
                {
                    XElement nestedElement = new XElement(nestedEntry.Key, nestedEntry.Value);
                    element.Add(nestedElement);
                }
            }
            else
            {
                string value;
                while (true)
                {
                    Console.Write($"Введите значение для элемента '{tag}': ");
                    value = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(value))
                        break;
                    Console.WriteLine($"Некорректное значение! Пожалуйста, введите значение для элемента '{tag}' ещё раз.");
                }

                element.Value = value;
            }

            root.Add(element);
            return true;
        }

        internal static void CreateXmlFile()
        {
            if (!Directory.Exists(AllowedDirectory))
            {
                Directory.CreateDirectory(AllowedDirectory);
            }

            Console.WriteLine("Введите название XML файла (без указания пути):");
            string fileName;
            while (true)
            {
                fileName = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(fileName))
                    break;
                Console.WriteLine("Некорректное название файла! Пожалуйста, введите название ещё раз.");
            }

            if (!fileName.EndsWith(".xml"))
            {
                fileName += ".xml";
            }

            string fullPath = Path.Combine(AllowedDirectory, fileName);
            Console.WriteLine($"\nФайл будет сохранён по следующему пути: {Path.GetFullPath(fullPath)}");

            XElement rootElement = new XElement("Root");

            try
            {
                Console.WriteLine("Для остановки добавления элементов введите '0'.");
                while (AddElementToXml(rootElement)) { }

                XDocument xdoc = new XDocument(rootElement);
                xdoc.Save(fullPath);
                Console.WriteLine("\nФайл успешно сохранен!");

                Console.WriteLine("\nСодержимое XML файла:");
                PrintFormattedXml(rootElement);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                return;
            }

            try
            {
                PathHandler.DeleteFile(fullPath);
                Console.WriteLine("\nВременный XML файл был успешно удалён.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении файла: {ex.Message}");
            }
        }

        private static void PrintFormattedXml(XElement rootElement)
        {
            foreach (XElement element in rootElement.Elements())
            {
                Console.WriteLine($"\nЭлемент: {element.Name}");
                if (element.HasElements)
                {
                    Console.WriteLine($"  Значение элемента: {element.Nodes().OfType<XText>().FirstOrDefault()?.Value}");
                    foreach (XElement nestedElement in element.Elements())
                    {
                        Console.WriteLine($"  Вложенный элемент: {nestedElement.Name}, Значение: {nestedElement.Value}");
                    }
                }
                else if (!string.IsNullOrEmpty(element.Value))
                {
                    Console.WriteLine($"  Значение элемента: {element.Value}");
                }
            }
        }
    }
}



