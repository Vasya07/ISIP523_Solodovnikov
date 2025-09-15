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