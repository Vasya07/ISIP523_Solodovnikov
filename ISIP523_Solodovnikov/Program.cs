class Product
{
    public int ProductID;
    public string Name;
    public int Price;
    public int Quantity;

    public void PrintInfo()
    {
        Console.WriteLine($"ID товара: {ProductID}, название: {Name}, цена: {Price}, количество: {Quantity}");
    }

    public Product(int ProductID = 0, string Name = "", int Price = 0, int Quantity = 0)
    {
        this.ProductID = ProductID;
        this.Name = Name;
        this.Price = Price;
        this.Quantity = Quantity;
    }
}

class Program
{
    static List<Product> products = new List<Product>();

    static void Main()
    {
        InitializeSampleData();

        bool exit = false;

        while (!exit)
        {
            ShowMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": AddProduct(); break;
                case "2": RemoveProduct(); break;
                case "3": RemoveProductByName(); break;
                case "4": DisplayAllProducts(); break;
                case "5": SearchProduct(); break;
                case "6": UpdateProduct(); break;
                case "7": exit = true; break;
                default: Console.WriteLine("Неверный выбор!"); break;
            }

            if (!exit)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("=== УПРАВЛЕНИЕ ТОВАРАМИ ===");
        Console.WriteLine("1. Добавить товар");
        Console.WriteLine("2. Удалить товар по ID");
        Console.WriteLine("3. Удалить товар по названию");
        Console.WriteLine("4. Показать все товары");
        Console.WriteLine("5. Поиск товара");
        Console.WriteLine("6. Редактировать товар");
        Console.WriteLine("7. Выход");
        Console.Write("Выберите действие: ");
    }

    static void InitializeSampleData()
    {
        products.Add(new Product(1, "Ноутбук", 50000, 10));
        products.Add(new Product(2, "Мышь", 1500, 50));
        products.Add(new Product(3, "Клавиатура", 3000, 25));
    }

    static void AddProduct()
    {
        Console.Write("Введите ID: ");
        int id = Convert.ToInt32(Console.ReadLine());

        if (products.Any(p => p.ProductID == id))
        {
            Console.WriteLine("ID уже существует!");
            return;
        }

        Console.Write("Введите название: ");
        string name = Console.ReadLine();

        Console.Write("Введите цену: ");
        int price = Convert.ToInt32(Console.ReadLine());

        Console.Write("Введите количество: ");
        int quantity = Convert.ToInt32(Console.ReadLine());

        products.Add(new Product(id, name, price, quantity));
        Console.WriteLine("Товар добавлен!");
    }

    static void RemoveProduct()
    {
        Console.Write("Введите ID для удаления: ");
        int id = Convert.ToInt32(Console.ReadLine());

        var product = products.FirstOrDefault(p => p.ProductID == id);
        if (product != null)
        {
            products.Remove(product);
            Console.WriteLine("Товар удален!");
        }
        else
        {
            Console.WriteLine("Товар не найден!");
        }
    }

    static void RemoveProductByName()
    {
        Console.Write("Введите название для удаления: ");
        string name = Console.ReadLine();

        var productsToRemove = products.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();

        if (productsToRemove.Count > 0)
        {
            foreach (var product in productsToRemove)
            {
                products.Remove(product);
            }
            Console.WriteLine($"Удалено {productsToRemove.Count} товаров!");
        }
        else
        {
            Console.WriteLine("Товары не найдены!");
        }
    }

    static void SearchProduct()
    {
        Console.Write("Введите название для поиска: ");
        string searchTerm = Console.ReadLine();

        var foundProducts = products.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToList();

        if (foundProducts.Count > 0)
        {
            Console.WriteLine($"Найдено {foundProducts.Count} товаров:");
            foreach (var product in foundProducts)
            {
                product.PrintInfo();
            }
        }
        else
        {
            Console.WriteLine("Товары не найдены!");
        }
    }

    static void UpdateProduct()
    {
        Console.Write("Введите ID товара для редактирования: ");
        int id = Convert.ToInt32(Console.ReadLine());

        var product = products.FirstOrDefault(p => p.ProductID == id);
        if (product != null)
        {
            Console.Write("Новое название (текущее: " + product.Name + "): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrEmpty(newName)) product.Name = newName;

            Console.Write("Новая цена (текущая: " + product.Price + "): ");
            string priceInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(priceInput)) product.Price = Convert.ToInt32(priceInput);

            Console.Write("Новое количество (текущее: " + product.Quantity + "): ");
            string quantityInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(quantityInput)) product.Quantity = Convert.ToInt32(quantityInput);

            Console.WriteLine("Товар обновлен!");
        }
        else
        {
            Console.WriteLine("Товар не найден!");
        }
    }

    static void DisplayAllProducts()
    {
        if (products.Count == 0)
        {
            Console.WriteLine("Список пуст!");
            return;
        }

        Console.WriteLine($"\nВсего товаров: {products.Count}");
        foreach (var product in products.OrderBy(p => p.ProductID))
        {
            product.PrintInfo();
        }

        Console.WriteLine($"Общая стоимость: {products.Sum(p => p.Price * p.Quantity)} руб.");
    }
}