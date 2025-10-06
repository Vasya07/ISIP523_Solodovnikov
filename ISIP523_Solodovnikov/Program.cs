using System;
using System.Collections.Generic;
using System.Linq;
using static LibraryManagement.Book;

namespace LibraryManagement
{
    public enum Genre
    {
        Fantasy,
        ScienceFiction,
        Mystery,
        Romance,
        Horror,
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
                AddBook("Властелин Колец", "Дж. Р. Р. Толкин", Genre.Fantasy, 1954, 1200m);
                AddBook("1984", "Джордж Оруэлл", Genre.ScienceFiction, 1949, 850m);
                AddBook("Убийство в Восточном экспрессе", "Агата Кристи", Genre.Mystery, 1934, 750m);
                AddBook("Гордость и предубеждение", "Джейн Остин", Genre.Romance, 1813, 680m);
                AddBook("Дракула", "Брэм Стокер", Genre.Horror, 1897, 920m);
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
                            ShowAllBooks(libraryService);
                            break;
                        case "2":
                            AddBookMenu(libraryService);
                            break;
                        case "3":
                            RemoveBookMenu(libraryService);
                            break;
                        case "4":
                            SearchBooksMenu(libraryService);
                            break;
                        case "5":
                            SortBooksMenu(libraryService);
                            break;
                        case "6":
                            ShowPriceExtremes(libraryService);
                            break;
                        case "7":
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
                Console.WriteLine("\nГлавное меню:");
                Console.WriteLine("1. Показать все книги");
                Console.WriteLine("2. Добавить книгу");
                Console.WriteLine("3. Удалить книгу по ID");
                Console.WriteLine("4. Найти книги");
                Console.WriteLine("5. Сортировать книги");
                Console.WriteLine("6. Самая дорогая/дешевая книга");
                Console.WriteLine("7. Статистика по авторам");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");
            }
            static void ShowAllBooks(LibraryService libraryService)
            {
                var books = libraryService.GetAllBooks();
                Console.WriteLine("\nПеред вами список всех книг:");

                if (!books.Any())
                {
                    Console.WriteLine("ОШИБКА: Книги не найдены!");
                    return;
                }

                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
            }
            static void AddBookMenu(LibraryService libraryService)
            {
                Console.WriteLine("\nДобавление новой книги:");
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
                        Console.WriteLine("ОШИБКА: неверный выбор жанра.");
                        return;
                    }
                    var genre = (Genre)(genreIndex - 1);

                    Console.Write("Введите год издания: ");
                    if (!int.TryParse(Console.ReadLine(), out int year) || year <= 0)
                    {
                        Console.WriteLine("ОШИБКА: год должен быть положительным числом.");
                        return;
                    }

                    Console.Write("Введите цену: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
                    {
                        Console.WriteLine("ОШИБКА: цена не может быть отрицательной.");
                        return;
                    }

                    if (libraryService.AddBook(title, author, genre, year, price))
                    {
                        Console.WriteLine("Книга успешно добавлена!");
                    }
                    else
                    {
                        Console.WriteLine("ОШИБКА: неверные данные книги.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }

            static void RemoveBookMenu(LibraryService libraryService)
            {
                Console.WriteLine("\nУдаление книги:");
                try
                {
                    Console.Write("Введите ID книги для удаления: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.WriteLine("ОШИБКА: неверный формат ID.");
                        return;
                    }

                    if (libraryService.RemoveBook(id))
                    {
                        Console.WriteLine("Книга успешно удалена!");
                    }
                    else
                    {
                        Console.WriteLine("ОШИБКА: книга с указанным ID не найдена.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
            static void SearchBooksMenu(LibraryService libraryService)
            {
                Console.WriteLine("\nПоиск книг:");
                Console.Write("Как будем искать книгу?");
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
                    Console.WriteLine("ОШИБКА: Неверный выбор типа поиска!");
                    return;
                }

                if (searchType == "genre")
                {
                    Console.WriteLine("Доступные жанры: " + string.Join(", ", Enum.GetNames(typeof(Genre))));
                }

                Console.Write("Введите поисковый запрос: ");
                var searchTerm = Console.ReadLine();

                var results = libraryService.SearchBooks(searchType, searchTerm);

                Console.WriteLine($"\nРезультаты поиска ({results.Count} книг):");

                if (!results.Any())
                {
                    Console.WriteLine("Книги не найдены!");
                    return;
                }

                foreach (var book in results)
                {
                    Console.WriteLine(book);
                }
            }   
            static void SortBooksMenu(LibraryService libraryService)
            {
                Console.WriteLine("\nСортировка книг:");
                Console.WriteLine("================");

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
                    Console.WriteLine("Неверный выбор сортировки.");
                    return;
                }

                var sortedBooks = libraryService.SortBooks(sortBy, ascending);

                Console.WriteLine($"\nОтсортированные книги:");
                Console.WriteLine("=====================");

                foreach (var book in sortedBooks)
                {
                    Console.WriteLine(book);
                }
            }
            static void ShowPriceExtremes(LibraryService libraryService)
            {
                Console.WriteLine("\nАнализ цен:");
                Console.WriteLine("===========");

                var (mostExpensive, cheapest) = libraryService.GetPriceExtremes();

                if (mostExpensive == null || cheapest == null)
                {
                    Console.WriteLine("В библиотеке нет книг.");
                    return;
                }

                Console.WriteLine($"Самая дорогая книга: {mostExpensive}");
                Console.WriteLine($"Самая дешевая книга: {cheapest}");

                var averagePrice = libraryService.GetAllBooks().Average(b => b.Price);
                Console.WriteLine($"Средняя цена: {averagePrice:C}");
            }

            static void ShowAuthorStatistics(LibraryService libraryService)
            {
                Console.WriteLine("\nСтатистика по авторам:");
                Console.WriteLine("=====================");

                var statistics = libraryService.GetAuthorStatistics();

                if (!statistics.Any())
                {
                    Console.WriteLine("В библиотеке нет книг.");
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