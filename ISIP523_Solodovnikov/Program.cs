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
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
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
        }
    }
}