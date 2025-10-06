using System;

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
    }
}