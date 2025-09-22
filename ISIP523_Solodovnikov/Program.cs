using System.Media;
using System.Numerics;
using static ShopInventory.Product;


namespace ShopInventory
{
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Food,
        Books,
        Sports
    }

    public class Product
    {
        public string Code { get; private set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InStock => Quantity > 0;
        public ProductCategory Category { get; set; }

        public Product(string name, decimal price, int quantity, ProductCategory category)
        {
            SoundPlayer player = new(@"C:\Users\admin\Music\Звуки Windows\XP\Windows XP Critical Stop.wav");
            SoundPlayer playercorrect = new(@"C:\Windows\Media\Windows Logon.wav");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("ОШИБКА: Название товара не может быть пустым!");
                player.Play();
            if (price < 0)
                throw new ArgumentException("ОШИБКА: Цена не может быть отрицательной!");
                player.Play();
            if (quantity < 0)
                throw new ArgumentException("ОШИБКА: Количество не может быть отрицательным!");
                player.Play();

            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public void SetCode(string code)
        {
            Code = code;
        }

        public override string ToString()
        {
            return $"Код: {Code}, Название: {Name}, Цена: {Price:C}, Количество: {Quantity}, " +
                   $"В наличии: {(InStock ? "Да" : "Нет")}, Категория: {Category}";
        }
        public class ProductManager
        {
            private List<Product> products;
            private int productCounter;

            public ProductManager()
            {
                products = new List<Product>();
                productCounter = 1;
                InitializeTestData();
            }
            private void InitializeTestData()
            {
                AddProduct("Смартфон", 29999.99m, 10, ProductCategory.Electronics);
                AddProduct("Футболка", 1499.50m, 25, ProductCategory.Clothing);
                AddProduct("Хлеб", 50.00m, 30, ProductCategory.Food);
                AddProduct("Программирование на C#", 1200.00m, 15, ProductCategory.Books);
                AddProduct("Футбольный мяч", 2500.00m, 8, ProductCategory.Sports);
            }

            private string GenerateProductCode()
            {
                return $"1{productCounter++:D5}";
            }
            public bool AddProduct(string name, decimal price, int quantity, ProductCategory category)
            {
                SoundPlayer player = new(@"C:\Users\admin\Music\Звуки Windows\XP\Windows XP Critical Stop.wav");
                try
                {
                    var product = new Product(name, price, quantity, category);
                    product.SetCode(GenerateProductCode());
                    products.Add(product);
                    return true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка при добавлении товара: {ex.Message}");
                    player.Play();
                    return false;
                }
            }
            public bool RemoveProduct(string code)
            {
                var product = products.FirstOrDefault(p => p.Code == code);
                if (product != null)
                {
                    products.Remove(product);
                    return true;
                }
                return false;
            }
            public Product FindProductByCode(string code)
            {
                return products.FirstOrDefault(p => p.Code == code);
            }
            public List<Product> FindProductsByName(string name)
            {
                return products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            public List<Product> FindProductsByCategory(ProductCategory category)
            {
                return products.Where(p => p.Category == category).ToList();
            }
            public List<Product> GetAllProducts()
            {
                return new List<Product>(products);
            }
        }
    }
    class Program
    {
        private static ProductManager productManager = new ProductManager();

        static void Main(string[] args)
        {
            Console.WriteLine("СИСТЕМА УЧЁТА ТОВАРА В МАГАЗИНЕ");
            ShowMenu();
            SoundPlayer player = new(@"C:\Users\admin\Music\Звуки Windows\XP\Windows XP Critical Stop.wav");
            while (true)
            {
                Console.Write("\nВведите команду: ");
                var command = Console.ReadLine();

                switch (command?.ToLower())
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        RemoveProduct();
                        break;
                    case "3":
                        OrderSupply();
                        break;
                    case "4":
                        SellProduct();
                        break;
                    case "5":
                        SearchProducts();
                        break;
                    case "6":
                        ShowAllProducts();
                        break;
                    case "7":
                        ShowMenu();
                        break;
                    case "0":
                        return;
                    default:
                        player.Play();
                        Console.WriteLine("Неизвестная команда. Введите '7' для просмотра меню.");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\nМЕНЮ УЧЁТА ТОВАРОВ В МАГАЗИНЕ");
            Console.WriteLine("1 - Добавить товар");
            Console.WriteLine("2 - Удалить товар");
            Console.WriteLine("3 - Заказать поставку товара");
            Console.WriteLine("4 - Продать товар");
            Console.WriteLine("5 - Поиск товаров");
            Console.WriteLine("6 - Показать все товары");
            Console.WriteLine("7 - Показать меню");
            Console.WriteLine("0 - Выход");
        }

        static void AddProduct()
        {
            Console.WriteLine("\nДОБАВЛЕНИЕ ТОВАРА");
            SoundPlayer player = new(@"C:\Users\admin\Music\Звуки Windows\XP\Windows XP Critical Stop.wav");
            try
            {
                Console.Write("Введите название товара: ");
                var name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("ОШИБКА: Название товара не может быть пустым!");
                    player.Play();
                    return;
                }

                Console.Write("Введите цену товара: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
                {
                    Console.WriteLine("ОШИБКА: Некорректная цена!");
                    player.Play();
                    return;
                }

                Console.Write("Введите количество: ");
                if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
                {
                    Console.WriteLine("ОШИБКА: Некорректное количество!");
                    return;
                }

                Console.WriteLine("Доступные категории:");
                var categories = Enum.GetValues(typeof(ProductCategory));
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.WriteLine($"{i + 1} - {categories.GetValue(i)}");
                }

                Console.Write("Выберите категорию (1-5): ");
                if (!int.TryParse(Console.ReadLine(), out int categoryIndex) ||
                    categoryIndex < 1 || categoryIndex > categories.Length)
                {
                    player.Play();
                    Console.WriteLine("ОШИБКА: Некорректный выбор категории!");
                    return;
                }

                var category = (ProductCategory)(categoryIndex - 1);
                SoundPlayer playercorrect = new(@"C:\Windows\Media\Windows Logon.wav");

                if (productManager.AddProduct(name, price, quantity, category))
                {
                    playercorrect.Play();
                    Console.WriteLine("Товар успешно добавлен!");
                }
            }
            catch (Exception ex)
            {
                player.Play();
                Console.WriteLine($"Ошибка при добавлении товара: {ex.Message}");
            }
        }

        static void RemoveProduct()
        {
            Console.WriteLine("\nУДАЛЕНИЕ ТОВАРА");
            Console.Write("Введите код товара для удаления: ");
            var code = Console.ReadLine();
            SoundPlayer player = new(@"C:\Users\admin\Music\Звуки Windows\XP\Windows XP Critical Stop.wav");
            SoundPlayer playercorrect = new(@"C:\Windows\Media\Windows Logon.wav");

            if (productManager.RemoveProduct(code))
            {
                playercorrect.Play();
                Console.WriteLine("Товар успешно удалён!");
            }
            else
            {
                player.Play();
                Console.WriteLine("ОШИБКА: Товар с указанным кодом не найден!");
            }
        }

        static void OrderSupply()
        {
            Console.WriteLine("\nЗАКАЗ ПОСТАВКИ ТОВАРА");
            Console.Write("Введите код товара: ");
            var code = Console.ReadLine();
            SoundPlayer player = new(@"C:\Users\admin\Music\Звуки Windows\XP\Windows XP Critical Stop.wav");
            var product = productManager.FindProductByCode(code);

            if (product == null)
            {
                Console.WriteLine("ОШИБКА: Товар с указанным кодом не найден!");
                player.Play();
                return;
            }

            Console.Write($"Текущее количество: {product.Quantity}. Введите количество для добавления: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("ОШИБКА: Некорректное количество!");
                player.Play();
                return;
            }

            product.Quantity += quantity;
            SoundPlayer playercorrect = new(@"C:\Windows\Media\Windows Logon.wav");

            playercorrect.Play();
            Console.WriteLine($"Поставка успешно зарегистрирована! Новое количество: {product.Quantity}");
        }

        static void SellProduct()
        {
            Console.WriteLine("\nПРОДАЖА ТОВАРА");
            Console.Write("Введите код товара: ");
            var code = Console.ReadLine();
            SoundPlayer player = new(@"C:\Users\admin\Music\Звуки Windows\XP\Windows XP Critical Stop.wav");
            var product = productManager.FindProductByCode(code);

            if (product == null)
            {
                Console.WriteLine("ОШИБКА: Товар с указанным кодом не найден!");
                player.Play();
                return;
            }

            if (!product.InStock)
            {
                Console.WriteLine("ОШИБКА: Товара нет в наличии!");
                player.Play();
                return;
            }

            Console.Write($"Доступное количество: {product.Quantity}. Введите количество для продажи: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("ОШИБКА: Некорректное количество!");
                player.Play();
                return;
            }

            if (quantity > product.Quantity)
            {
                Console.WriteLine("ОШИБКА: Недостаточно товара на складе!");
                return;
            }

            product.Quantity -= quantity;
            SoundPlayer playercorrect = new(@"C:\Windows\Media\Windows Logon.wav");
            playercorrect.Play();
            Console.WriteLine($"Продажа успешно завершена! Остаток: {product.Quantity}, Сумма: {product.Price * quantity:C}");
        }

        static void SearchProducts()
        {
            Console.WriteLine("\nПОИСК ТОВАРА");
            Console.WriteLine("1 - Поиск по коду");
            Console.WriteLine("2 - Поиск по названию");
            Console.WriteLine("3 - Поиск по категории");
            Console.Write("Выберите тип поиска: ");

            SoundPlayer player = new(@"C:\Users\admin\Music\Звуки Windows\XP\Windows XP Critical Stop.wav");
            var searchType = Console.ReadLine();
            switch (searchType)
            {
                case "1":
                    Console.Write("Введите код товара: ");
                    var code = Console.ReadLine();
                    var product = productManager.FindProductByCode(code);
                    if (product != null)
                    {
                        Console.WriteLine("Найденный товар:");
                        Console.WriteLine(product);
                    }
                    else
                    {
                        player.Play();
                        Console.WriteLine("ОШИБКА: Товар не найден!");
                    }
                    break;

                case "2":
                    Console.Write("Введите название товара: ");
                    var name = Console.ReadLine();
                    var productsByName = productManager.FindProductsByName(name);
                    DisplayProducts(productsByName, $"Результаты поиска по названию '{name}':");
                    break;

                case "3":
                    Console.WriteLine("Доступные категории:");
                    var categories = Enum.GetValues(typeof(ProductCategory));
                    for (int i = 0; i < categories.Length; i++)
                    {
                        Console.WriteLine($"{i + 1} - {categories.GetValue(i)}");
                    }
                    Console.Write("Выберите категорию: ");
                    if (int.TryParse(Console.ReadLine(), out int categoryIndex) &&
                        categoryIndex >= 1 && categoryIndex <= categories.Length)
                    {
                        var category = (ProductCategory)(categoryIndex - 1);
                        var productsByCategory = productManager.FindProductsByCategory(category);
                        DisplayProducts(productsByCategory, $"Товары в категории '{category}':");
                    }
                    else
                    {
                        Console.WriteLine("ОШИБКА: Некорректный выбор категории!");
                        player.Play();
                    }
                    break;

                default:
                    Console.WriteLine("ОШИБКА: Некорректный тип поиска!");
                    break;
            }
        }

        static void ShowAllProducts()
        {
            var allProducts = productManager.GetAllProducts();
            DisplayProducts(allProducts, "Все товары в магазине:");
        }

        static void DisplayProducts(System.Collections.Generic.List<Product> products, string title)
        {
            Console.WriteLine($"\n{title} УЧЁТА ТОВАРОВ В МАГАЗИНЕ");
            SoundPlayer player = new(@"C:\Users\admin\Music\Звуки Windows\XP\Windows XP Critical Stop.wav");

            if (products.Count == 0)
            {
                Console.WriteLine("ОШИБКА: Товары не найдены!");
                player.Play();
            }
            else
            {
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }
                Console.WriteLine($"Всего товаров: {products.Count}");
            }
        }
    }
}