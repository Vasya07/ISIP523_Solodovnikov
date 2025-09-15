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
            Console.WriteLine("\n=== ВСЕ ОПЕРАЦИИ ===");
            for (int i = 0; i < expenses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {expenses[i].Name} - {expenses[i].Amount} руб.");
            }
        }

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

            foreach (var expense in expenses)
            {
                total += expense.Amount;
                if (expense.Amount > max) max = expense.Amount;
                if (expense.Amount < min) min = expense.Amount;
            }

            decimal average = total / expenses.Count;

            Console.WriteLine("\n=== СТАТИСТИКА ===");
            Console.WriteLine($"Общая сумма: {total} руб.");
            Console.WriteLine($"Средняя трата: {average:F2} руб.");
            Console.WriteLine($"Максимальная трата: {max} руб.");
            Console.WriteLine($"Минимальная трата: {min} руб.");
        }
        static void SortExpenses(List<Expense> expenses)
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("Нет данных для сортировки");
                return;
            }

            for (int i = 0; i < expenses.Count - 1; i++)
            {
                for (int j = 0; j < expenses.Count - i - 1; j++)
                {
                    if (expenses[j].Amount > expenses[j + 1].Amount)
                    {
                        var temp = expenses[j];
                        expenses[j] = expenses[j + 1];
                        expenses[j + 1] = temp;
                    }
                }
            }

            Console.WriteLine("Данные отсортированы по цене (по возрастанию)");
            ShowExpenses(expenses);
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
