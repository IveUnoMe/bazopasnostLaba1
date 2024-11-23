using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pratkicheskaya1
{
    internal class Person
    {
        public static readonly string AllowedDirectory = @"C:\Users\Vecheslav-PC\source\repos\Pratkicheskaya1";

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        public Dictionary<string, string> NestedElements { get; set; } = new Dictionary<string, string>();
    }

    internal static class JsonHandler
    {
        private static Dictionary<string, string> CreateNestedJsonElement()
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

        internal static void CreateJsonFile()
        {
            if (!Directory.Exists(Person.AllowedDirectory))
            {
                Directory.CreateDirectory(Person.AllowedDirectory);
            }

            Console.WriteLine("Введите название JSON файла (без указания пути):");
            string fileName;
            while (true)
            {
                fileName = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(fileName))
                    break;
                Console.WriteLine("Некорректное название файла! Пожалуйста, введите название ещё раз.");
            }

            if (!fileName.EndsWith(".json"))
            {
                fileName += ".json";
            }

            string fullPath = Path.Combine(Person.AllowedDirectory, fileName);
            Console.WriteLine($"Файл будет сохранён по следующему пути: {Path.GetFullPath(fullPath)}");

            List<Person> elements = new List<Person>();

            try
            {
                Console.WriteLine("Для остановки добавления элементов введите '0'.");

                while (true)
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
                        break;

                    Person element = new Person { Name = tag };

                    string valueType;
                    while (true)
                    {
                        Console.Write("Значение элемента является словарем? Да - 1, Нет - 2: ");
                        valueType = Console.ReadLine();
                        if (valueType == "1" || valueType == "2")
                            break;
                        Console.WriteLine("Ошибка! Пожалуйста, введите '1' для словаря или '2' для обычного значения.");
                    }

                    string value;
                    while (true)
                    {
                        Console.Write($"Введите значение для элемента '{tag}': ");
                        value = Console.ReadLine()?.Trim();
                        if (!string.IsNullOrEmpty(value))
                            break;
                        Console.WriteLine($"Некорректное значение! Пожалуйста, введите значение для элемента '{tag}' ещё раз.");
                    }
                    element.NestedElements.Add("Value", value);

                    if (valueType == "1")
                    {
                        Dictionary<string, string> nestedElements = CreateNestedJsonElement();
                        foreach (var nested in nestedElements)
                        {
                            element.NestedElements[nested.Key] = nested.Value;
                        }
                    }

                    elements.Add(element);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка ввода данных: {ex.Message}");
                return;
            }

            string jsonContent;
            try
            {
                jsonContent = JsonSerializer.Serialize(elements, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сериализации JSON: {ex.Message}");
                return;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath, false, Encoding.UTF8))
                {
                    sw.WriteLine(jsonContent);
                }
                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine("\nJSON файл успешно создан.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
                return;
            }

            try
            {
                Console.WriteLine("\nСодержимое JSON файла:");
                PrintFormattedJsonContent(elements);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
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

        private static void PrintFormattedJsonContent(List<Person> elements)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                Console.WriteLine($"Элемент №{i + 1}");
                Console.WriteLine($"Название элемента: {elements[i].Name}");

                if (elements[i].NestedElements.ContainsKey("Value"))
                {
                    Console.WriteLine($"  Значение элемента: {elements[i].NestedElements["Value"]}");
                }

                if (elements[i].NestedElements.Count > 1)
                {
                    Console.WriteLine("  Вложенные элементы:");
                    foreach (var nestedEntry in elements[i].NestedElements)
                    {
                        if (nestedEntry.Key != "Value")
                        {
                            Console.WriteLine($"    {nestedEntry.Key}: {nestedEntry.Value}");
                        }
                    }
                }

                Console.WriteLine();
            }
        }
    }
}


