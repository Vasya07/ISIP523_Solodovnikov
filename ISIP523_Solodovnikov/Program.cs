using System;
using System.Collections.Generic;

class Program
{
    // Главный метод программы - точка входа
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("=== Трекер расходов ===");

            // Получаем количество операций от пользователя
            int operationsCount = GetOperationsCount();

            // Вводим данные о тратах
            List<Expense> expenses = InputExpenses(operationsCount);

            Console.WriteLine("\nВсе операции успешно записаны!");

            // Показываем главное меню
            ShowMenu(expenses);
        }
        catch (Exception ex)
        {
            // Обработка непредвиденных ошибок
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }

    // Метод для получения количества операций с проверкой ввода
    static int GetOperationsCount()
    {
        int count;
        while (true)
        {
            Console.Write("Введите количество операций (2-40): ");
            if (int.TryParse(Console.ReadLine(), out count) && count >= 2 && count <= 40)
            {
                return count;
            }
            Console.WriteLine("Ошибка! Введите число от 2 до 40.");
        }
    }

    // Метод для ввода данных о тратах
    static List<Expense> InputExpenses(int count)
    {
        List<Expense> expenses = new List<Expense>();

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"\nОперация {i + 1}:");

            // Ввод названия товара/услуги
            Console.Write("Название товара/услуги: ");
            string name = Console.ReadLine();

            // Ввод суммы с проверкой
            decimal amount = GetAmount();

            // Добавление новой траты в список
            expenses.Add(new Expense(name, amount));
        }

        return expenses;
    }

    // Метод для получения суммы с проверкой корректности
    static decimal GetAmount()
    {
        decimal amount;
        while (true)
        {
            Console.Write("Сумма (рубли): ");
            if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
            {
                return amount;
            }
            Console.WriteLine("Ошибка! Введите корректную сумму.");
        }
    }

    // Метод для отображения главного меню и обработки выбора пользователя
    static void ShowMenu(List<Expense> expenses)
    {
        while (true)
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");

            Console.Write("Выберите пункт меню: ");
            string choice = Console.ReadLine();

            // Обработка выбора пользователя
            switch (choice)
            {
                case "1":
                    ShowExpenses(expenses); // Показать все траты
                    break;
                case "2":
                    ShowStatistics(expenses); // Показать статистику
                    break;
                case "3":
                    SortExpenses(expenses); // Отсортировать по цене
                    break;
                case "4":
                    ConvertCurrency(expenses); // Конвертировать валюту
                    break;
                case "5":
                    SearchExpenses(expenses); // Поиск по названию
                    break;
                case "0":
                    Console.WriteLine("До свидания!"); // Выход из программы
                    return;
                default:
                    Console.WriteLine("Неверный выбор! Попробуйте снова.");
                    break;
            }
        }
    }

    // Метод для отображения всех операций
    static void ShowExpenses(List<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Нет данных для отображения");
            return;
        }

        Console.WriteLine("\n=== ВСЕ ОПЕРАЦИИ ===");
        for (int i = 0; i < expenses.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {expenses[i].Name} - {expenses[i].Amount} руб.");
        }
    }

    // Метод для расчета и отображения статистики
    static void ShowStatistics(List<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Нет данных для статистики");
            return;
        }

        decimal total = 0;
        decimal max = expenses[0].Amount;
        decimal min = expenses[0].Amount;

        // Расчет общей суммы, максимума и минимума
        foreach (var expense in expenses)
        {
            total += expense.Amount;
            if (expense.Amount > max) max = expense.Amount;
            if (expense.Amount < min) min = expense.Amount;
        }

        // Расчет средней траты
        decimal average = total / expenses.Count;

        // Вывод статистики
        Console.WriteLine("\n=== СТАТИСТИКА ===");
        Console.WriteLine($"Общая сумма: {total} руб.");
        Console.WriteLine($"Средняя трата: {average:F2} руб.");
        Console.WriteLine($"Максимальная трата: {max} руб.");
        Console.WriteLine($"Минимальная трата: {min} руб.");
    }

    // Метод для сортировки трат по цене (пузырьковая сортировка)
    static void SortExpenses(List<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Нет данных для сортировки");
            return;
        }

        // Реализация пузырьковой сортировки
        for (int i = 0; i < expenses.Count - 1; i++)
        {
            for (int j = 0; j < expenses.Count - i - 1; j++)
            {
                if (expenses[j].Amount > expenses[j + 1].Amount)
                {
                    // Обмен местами двух элементов
                    var temp = expenses[j];
                    expenses[j] = expenses[j + 1];
                    expenses[j + 1] = temp;
                }
            }
        }

        Console.WriteLine("Данные отсортированы по цене (по возрастанию)");
        ShowExpenses(expenses); // Показываем отсортированный список
    }

    // Метод для конвертации валюты
    static void ConvertCurrency(List<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Нет данных для конвертации");
            return;
        }

        Console.WriteLine("\n=== КОНВЕРТАЦИЯ ВАЛЮТЫ ===");
        Console.WriteLine("Доступные валюты:");
        Console.WriteLine("1. Доллар США (USD)");
        Console.WriteLine("2. Евро (EUR)");
        Console.WriteLine("3. Фунт стерлингов (GBP)");
        Console.WriteLine("4. Другая валюта (ввести курс вручную)");

        Console.Write("Выберите валюту: ");
        string choice = Console.ReadLine();

        decimal rate = 0;

        // Установка курса в зависимости от выбора
        switch (choice)
        {
            case "1":
                rate = 75.0m; // Курс доллара
                break;
            case "2":
                rate = 85.0m; // Курс евро
                break;
            case "3":
                rate = 95.0m; // Курс фунта
                break;
            case "4":
                rate = GetCustomRate(); // Пользовательский курс
                break;
            default:
                Console.WriteLine("Неверный выбор!");
                return;
        }

        Console.WriteLine($"\nКонвертация по курсу: 1 рубль = {1 / rate:F4} выбранной валюты");
        Console.WriteLine("\n=== РЕЗУЛЬТАТ КОНВЕРТАЦИИ ===");

        // Конвертация и вывод каждой траты
        foreach (var expense in expenses)
        {
            decimal convertedAmount = expense.Amount / rate;
            string currencyName = GetCurrencyName(choice);
            Console.WriteLine($"{expense.Name} - {convertedAmount:F2} {currencyName}");
        }
    }

    // Метод для получения пользовательского курса валюты
    static decimal GetCustomRate()
    {
        decimal rate;
        while (true)
        {
            Console.Write("Введите курс (сколько рублей за 1 единицу валюты): ");
            if (decimal.TryParse(Console.ReadLine(), out rate) && rate > 0)
            {
                return rate;
            }
            Console.WriteLine("Ошибка! Введите корректный курс.");
        }
    }

    // Метод для получения названия валюты по выбору
    static string GetCurrencyName(string choice)
    {
        return choice switch
        {
            "1" => "USD",
            "2" => "EUR",
            "3" => "GBP",
            "4" => "ед.",
            _ => "ед."
        };
    }

    // Метод для поиска трат по названию
    static void SearchExpenses(List<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Нет данных для поиска");
            return;
        }

        Console.Write("Введите название для поиска: ");
        string searchTerm = Console.ReadLine().ToLower();

        var results = new List<Expense>();

        // Поиск совпадений в названиях трат
        foreach (var expense in expenses)
        {
            if (expense.Name.ToLower().Contains(searchTerm))
            {
                results.Add(expense);
            }
        }

        // Вывод результатов поиска
        if (results.Count == 0)
        {
            Console.WriteLine("Ничего не найдено");
        }
        else
        {
            Console.WriteLine($"\nНайдено {results.Count} совпадений:");
            foreach (var result in results)
            {
                Console.WriteLine($"{result.Name} - {result.Amount} руб.");
            }
        }
    }
}

// Класс для представления одной траты
class Expense
{
    public string Name { get; set; } // Название товара/услуги
    public decimal Amount { get; set; } // Сумма траты в рублях

    // Конструктор для создания новой траты
    public Expense(string name, decimal amount)
    {
        Name = name;
        Amount = amount;
    }
}