using System;
using System.Collections.Generic;
using System.Linq;
using static LibraryManagement.Book;

namespace LibraryManagement
{
    public enum Genre
    {
        Fantastika,
        Nauchnaya_Fantastika,
        Detectives,
        Romans,
        Horrors,
        Biography,
        History
    }

    public class Book
    {
        private static int _nextId = 1;

        public int Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public Book(string title, string author, Genre genre, int year, decimal price)
        {
            Id = _nextId++;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Название: \"{Title}\", Автор: {Author}, " +
                   $"Жанр: {Genre}, Год: {Year}, Цена: {Price:C}";
        }
        public class LibraryService
        {
            private List<Book> _books;

            public LibraryService()
            {
                _books = new List<Book>();
                InitializeTestData();
            }
            private void InitializeTestData()
            {
                AddBook("Властелин Колец", "Дж. Р. Р. Толкин", Genre.Fantastika, 1954, 1200);
                AddBook("1984", "Джордж Оруэлл", Genre.Nauchnaya_Fantastika, 1949, 850);
                AddBook("Убийство в Восточном экспрессе", "Агата Кристи", Genre.Detectives, 1934, 750);
                AddBook("Гордость и предубеждение", "Джейн Остин", Genre.Romans, 1813, 680);
                AddBook("Дракула", "Брэм Стокер", Genre.Horrors, 1897, 920);
            }
            public bool AddBook(string title, string author, Genre genre, int year, decimal price)
            {
                if (string.IsNullOrWhiteSpace(title) ||
                    string.IsNullOrWhiteSpace(author) ||
                    year <= 0 ||
                    price < 0)
                {
                    return false;
                }

                var book = new Book(title.Trim(), author.Trim(), genre, year, price);
                _books.Add(book);
                return true;
            }
            public List<Book> GetAllBooks()
            {
                return _books.OrderBy(b => b.Id).ToList();
            }
            public Book FindBookById(int id)
            {
                return _books.FirstOrDefault(b => b.Id == id);
            }
            public bool RemoveBook(int id)
            {
                var book = FindBookById(id);
                if (book != null)
                {
                    _books.Remove(book);
                    return true;
                }
                return false;
            }
            public List<Book> SearchBooks(string searchType, string searchTerm)
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return new List<Book>();

                return searchType.ToLower() switch
                {
                    "title" => _books.Where(b => b.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                                     .OrderBy(b => b.Title)
                                     .ToList(),
                    "author" => _books.Where(b => b.Author.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                                      .OrderBy(b => b.Author)
                                      .ToList(),
                    "genre" => Enum.TryParse<Genre>(searchTerm, true, out var genre)
                               ? _books.Where(b => b.Genre == genre).OrderBy(b => b.Title).ToList()
                               : new List<Book>(),
                    _ => new List<Book>()
                };
            }
            public List<Book> SortBooks(string sortBy, bool ascending = true)
            {
                return sortBy.ToLower() switch
                {
                    "title" => ascending
                        ? _books.OrderBy(b => b.Title).ToList()
                        : _books.OrderByDescending(b => b.Title).ToList(),
                    "year" => ascending
                        ? _books.OrderBy(b => b.Year).ToList()
                        : _books.OrderByDescending(b => b.Year).ToList(),
                    "price" => ascending
                        ? _books.OrderBy(b => b.Price).ToList()
                        : _books.OrderByDescending(b => b.Price).ToList(),
                    _ => _books.OrderBy(b => b.Id).ToList()
                };
            }
            public (Book MostExpensive, Book Cheapest) GetPriceExtremes()
            {
                if (!_books.Any())
                    return (null, null);

                var mostExpensive = _books.OrderByDescending(b => b.Price).First();
                var cheapest = _books.OrderBy(b => b.Price).First();

                return (mostExpensive, cheapest);
            }
            public Dictionary<string, int> GetAuthorStatistics()
            {
                return _books.GroupBy(b => b.Author)
                             .ToDictionary(g => g.Key, g => g.Count())
                             .OrderByDescending(kv => kv.Value)
                             .ToDictionary(kv => kv.Key, kv => kv.Value);
            }
        }
        class Program
        {
            static void Main(string[] args)
            {
                var libraryService = new LibraryService();
                while (true)
                {
                    ShowMainMenu();
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Clear();
                            ShowAllBooks(libraryService);
                            break;
                        case "2":
                            Console.Clear();
                            AddBookMenu(libraryService);
                            break;
                        case "3":
                            Console.Clear();
                            RemoveBookMenu(libraryService);
                            break;
                        case "4":
                            Console.Clear();
                            SearchBooksMenu(libraryService);
                            break;
                        case "5":
                            Console.Clear();
                            SortBooksMenu(libraryService);
                            break;
                        case "6":
                            Console.Clear();
                            ShowPriceExtremes(libraryService);
                            break;
                        case "7":
                            Console.Clear();
                            ShowAuthorStatistics(libraryService);
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Данный функционал не предусмотрен!");
                            break;
                    }

                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            static void ShowMainMenu()
            {
                Console.WriteLine("1. Показать все книги");
                Console.WriteLine("2. Добавить книгу");
                Console.WriteLine("3. Удалить книгу по ID");
                Console.WriteLine("4. Поиск книг");
                Console.WriteLine("5. Сортировать книги");
                Console.WriteLine("6. Анализировать цены на книги");
                Console.WriteLine("7. Показать количество книг каждого автора");
                Console.WriteLine("0. Выход из программы");
                Console.Write("Выберите действие: ");
            }
            static void ShowAllBooks(LibraryService libraryService)
            {
                var books = libraryService.GetAllBooks();

                if (!books.Any())
                {
                    Console.WriteLine("Книги не найдены!");
                    return;
                }

                Console.WriteLine($"Всего книг: {books.Count}");
                Console.WriteLine();

                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
            }
            static void AddBookMenu(LibraryService libraryService)
            {
                try
                {
                    Console.Write("Введите название книги: ");
                    var title = Console.ReadLine();

                    Console.Write("Введите автора: ");
                    var author = Console.ReadLine();

                    Console.WriteLine("Доступные жанры:");
                    var genres = Enum.GetValues(typeof(Genre));
                    for (int i = 0; i < genres.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {genres.GetValue(i)}");
                    }

                    Console.Write("Выберите номер жанра: ");
                    if (!int.TryParse(Console.ReadLine(), out int genreIndex) ||
                        genreIndex < 1 || genreIndex > genres.Length)
                    {
                        Console.WriteLine("ОШИБКА: Неверный выбор жанра! Книга не добавлена.");
                        return;
                    }
                    var genre = (Genre)(genreIndex - 1);

                    Console.Write("Введите год издания: ");
                    if (!int.TryParse(Console.ReadLine(), out int year) || year <= 0)
                    {
                        Console.WriteLine("ОШИБКА: Год должен быть положительным числом! Книга не добавлена.");
                        return;
                    }

                    Console.Write("Введите цену: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
                    {
                        Console.WriteLine("ОШИБКА: Цена не может быть отрицательной! Книга не добавлена.");
                        return;
                    }

                    if (libraryService.AddBook(title, author, genre, year, price))
                    {
                        Console.WriteLine("Книга успешно добавлена!");
                    }
                    else
                    {
                        Console.WriteLine("Книга не добавлена.");
                        Console.WriteLine("ОШИБКА: Неверные данные для книги!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }

            static void RemoveBookMenu(LibraryService libraryService)
            {
                try
                {
                    Console.Write("Введите ID книги для удаления: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("ОШИБКА: Неверно указанный ID книги! Книга не удалена.");
                        return;
                    }

                    if (libraryService.RemoveBook(id))
                    {
                        Console.WriteLine("Книга успешно удалена!");
                    }
                    else
                    {
                        Console.WriteLine("Книга не удалена.");
                        Console.WriteLine("ОШИБКА: Книга с указанным ID не найдена!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
            static void SearchBooksMenu(LibraryService libraryService)
            {
                Console.Write("Как будем искать книгу?\n");
                Console.WriteLine("1. По названию");
                Console.WriteLine("2. По автору");
                Console.WriteLine("3. По жанру");
                
                var searchTypeChoice = Console.ReadLine();
                string searchType = searchTypeChoice switch
                {
                    "1" => "title",
                    "2" => "author",
                    "3" => "genre",
                    _ => null
                };

                if (searchType == null)
                {
                    Console.WriteLine("Поиск завершён неудачно. ОШИБКА: Неверный выбор типа поиска!");
                    return;
                }

                if (searchType == "genre")
                {
                    Console.WriteLine("Доступные жанры: " + string.Join(", ", Enum.GetNames(typeof(Genre))));
                }

                Console.Write("Введите поисковый запрос: ");
                var searchTerm = Console.ReadLine();

                var results = libraryService.SearchBooks(searchType, searchTerm);

                if (!results.Any())
                {
                    Console.WriteLine($"По запросу {searchTerm} ничего не найдено!");
                    return;
                }

                Console.WriteLine($"\nРезультаты поиска ({results.Count} книг):");
                foreach (var book in results)
                {
                    Console.WriteLine(book);
                }
            }   
            static void SortBooksMenu(LibraryService libraryService)
            {
                Console.WriteLine("1. По названию (А-Я)");
                Console.WriteLine("2. По названию (Я-А)");
                Console.WriteLine("3. По году (сначала старые)");
                Console.WriteLine("4. По году (сначала новые)");
                Console.Write("Выберите тип сортировки: ");

                var choice = Console.ReadLine();
                var (sortBy, ascending) = choice switch
                {
                    "1" => ("title", true),
                    "2" => ("title", false),
                    "3" => ("year", true),
                    "4" => ("year", false),
                    _ => (null, true)
                };

                if (sortBy == null)
                {
                    Console.WriteLine("ОШИБКА: Неверный выбор сортировки!");
                    return;
                }

                var sortedBooks = libraryService.SortBooks(sortBy, ascending);

                foreach (var book in sortedBooks)
                {
                    Console.WriteLine(book);
                }
            }
            static void ShowPriceExtremes(LibraryService libraryService)
            {

                var (mostExpensive, cheapest) = libraryService.GetPriceExtremes();

                if (mostExpensive == null || cheapest == null)
                {
                    Console.WriteLine("ОШИБКА: В библиотеке отсутсвуют книги!");
                    return;
                }

                Console.WriteLine($"Самая дорогая книга: {mostExpensive}");
                Console.WriteLine($"Самая дешевая книга: {cheapest}");

                var averagePrice = libraryService.GetAllBooks().Average(b => b.Price);
                Console.WriteLine($"Средняя цена: {averagePrice}");
            }

            static void ShowAuthorStatistics(LibraryService libraryService)
            {
                var statistics = libraryService.GetAuthorStatistics();

                if (!statistics.Any())
                {
                    Console.WriteLine("ОШИБКА: В библиотеке отсутсвуют книги!");
                    return;
                }

                foreach (var (author, count) in statistics)
                {
                    Console.WriteLine($"{author}: {count} книг(и)");
                }
            }
        }
    }
}