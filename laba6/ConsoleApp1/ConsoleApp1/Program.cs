using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

class Apartment
{
    public string Region { get; set; }
    public string Address { get; set; }
    public int Views { get; set; }

    public Apartment(string region, string address)
    {
        Region = region;
        Address = address;
        Views = 0;
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Apartment> apartments = new List<Apartment>();

        XDocument xmlDoc = XDocument.Load("apartments.xml");
        foreach (XElement apartmentElem in xmlDoc.Descendants("apartments"))
        {
            string region = apartmentElem.Attribute("region").Value;
            string address = apartmentElem.Attribute("address").Value;
            Apartment apartment = new Apartment(region, address);
            apartments.Add(apartment);
        }

         while (true)
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Добавить объявление о квартире");
            Console.WriteLine("2. Просмотреть квартиры в определенном регионе");
            Console.WriteLine("3. Просмотреть детальную информацию о квартире");
            Console.WriteLine("4. Сохранить квартиру в файл");
            Console.WriteLine("5. Выход");

            int menuChoice = ReadIntInput("Выберите пункт меню: ");

            if (menuChoice == 1)
            {
                Console.WriteLine("Введите информацию о квартире:");

                string region = ReadStringInput("Регион: ");
                string address = ReadStringInput("Адрес: ");
                Apartment apartment = new Apartment(region, address);
                apartments.Add(apartment);

                Console.WriteLine("Операция выполнена успешно!");
                Console.WriteLine();
            }
            else if (menuChoice == 2)
            {
                Console.WriteLine("Введите регион для поиска квартир:");
                string region = ReadStringInput("Регион: ");
                IEnumerable<Apartment> filteredApartments = apartments.Where(a => a.Region == region);

                if (filteredApartments.Count() == 0)
                {
                    Console.WriteLine("Квартиры в выбранном регионе не найдены");
                }
                else
                {
                    Console.WriteLine("Найденные квартиры:");
                    foreach (Apartment apartment in filteredApartments)
                    {
                        Console.WriteLine(apartment.Address);
                    }
                }
                Console.WriteLine();
            }
            else if (menuChoice == 3)
            {
                Console.WriteLine("Введите адрес квартиры для отображения подробной информации:");
                string address = ReadStringInput("Адрес: ");

                Apartment selectedApartment = apartments.FirstOrDefault(a => a.Address == address);

                if (selectedApartment == null)
                {
                    Console.WriteLine("Квартира с указанным адресом не найдена");
                }
                else
                {
                    Console.WriteLine($"Регион: {selectedApartment.Region}");
                    Console.WriteLine($"Адрес: {selectedApartment.Address}");
                    Console.WriteLine($"Просмотры: {selectedApartment.Views}");
                    selectedApartment.Views++;
                }
                Console.WriteLine();
            }
            else if (menuChoice == 4)
            {
                Console.WriteLine("Выберите квартиру для сохранения в файл:");

                for (int i = 0; i < apartments.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {apartments[i].Region} - {apartments[i].Address}");
                }

                int apartmentChoice = ReadIntInput("Выберите номер квартиры: ");

                if (apartmentChoice >= 1 && apartmentChoice <= apartments.Count)
                {
                    string fileName = ReadStringInput("Введите название файла: ");
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine($"Регион: {apartments[apartmentChoice - 1].Region}");
                        writer.WriteLine($"Адрес: {apartments[apartmentChoice - 1].Address}");
                    }

                    Console.WriteLine("Квартира успешно сохранена в файл");
                }
                else
                {
                    Console.WriteLine("Некорректный номер квартиры");
                }
                Console.WriteLine();
            }
            else if (menuChoice == 5)
            {
                break;
            }
            else
            {
                Console.WriteLine("Некорректный выбор");
                Console.WriteLine();
            }
        }
    }

    static int ReadIntInput(string prompt)
    {
        int result;

        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (int.TryParse(input, out result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("Некорректный ввод, пожалуйста, введите целое число");
            }
        }
    }

    static string ReadStringInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}