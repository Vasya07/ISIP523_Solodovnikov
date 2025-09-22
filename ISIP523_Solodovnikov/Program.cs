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
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название товара не может быть пустым");
            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной");
            if (quantity < 0)
                throw new ArgumentException("Количество не может быть отрицательным");

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
}