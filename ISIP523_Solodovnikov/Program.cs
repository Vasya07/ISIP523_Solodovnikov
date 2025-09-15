using System;
using System.Collections.Generic;

namespace ExpenseTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Трекер расходов ===");

            int operationsCount = GetOperationsCount();

            List<Expense> expenses = InputExpenses(operationsCount);

            Console.WriteLine("\nВсе операции успешно записаны!");
        }

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

        static List<Expense> InputExpenses(int count)
        {
            List<Expense> expenses = new List<Expense>();

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"\nОперация {i + 1}:");

                Console.Write("Название товара/услуги: ");
                string name = Console.ReadLine();

                decimal amount = GetAmount();

                expenses.Add(new Expense(name, amount));
            }

            return expenses;
        }

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

                switch (choice)
                {
                    case "1":
                        ShowExpenses(expenses);
                        break;
                    case "2":
                        ShowStatistics(expenses);
                        break;
                    case "3":
                        SortExpenses(expenses);
                        break;
                    case "4":
                        ConvertCurrency(expenses);
                        break;
                    case "5":
                        SearchExpenses(expenses);
                        break;
                    case "0":
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор! Попробуйте снова.");
                        break;
                }
            }
        }
        static void ShowExpenses(List<Expense> expenses)
        {
            Console.WriteLine("Функция вывода данных будет реализована в следующем коммите");
        }

        static void ShowStatistics(List<Expense> expenses)
        {
            Console.WriteLine("Функция статистики будет реализована в следующем коммите");
        }

    }

    class Expense
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public Expense(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }
    }
}
