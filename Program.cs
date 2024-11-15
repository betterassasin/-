using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text.Json;


namespace LibraryManagement
{
    class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string SourceLink { get; set; }  // Добавляем новое свойство для ссылки на источник

        public Book(string title, string author, int year, string sourceLink)
        {
            Title = title;
            Author = author;
            Year = year;
            SourceLink = sourceLink;
        }

        public override string ToString()
        {
            return $"'{Title}' by {Author}, {Year} (Source: {SourceLink})";
        }
    }

    class Library
    {
        private List<Book> books;
        private const string FilePath = "library.json";

        public Library()
        {
            books = new List<Book>();
            LoadBooks();
        }

        public void AddBook(Book book)
        {
            books.Add(book);
            Console.WriteLine($"Книга '{book.Title}' добавлена в библиотеку.");
            SaveBooks();
        }

        public void RemoveBook(string title)
        {
            Book bookToRemove = books.Find(book => book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                Console.WriteLine($"Книга '{title}' удалена из библиотеки.");
                SaveBooks();
            }
            else
            {
                Console.WriteLine($"Книга '{title}' не найдена в библиотеке.");
            }
        }

        public void ListBooks()
        {
            Console.WriteLine("Список книг в библиотеке:");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        public void OpenBookSource(string title)
        {
            Book bookToOpen = books.Find(book => book.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (bookToOpen != null && !string.IsNullOrEmpty(bookToOpen.SourceLink))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = bookToOpen.SourceLink,
                    UseShellExecute = true
                });
                Console.WriteLine($"Открытие ссылки на источник книги '{title}': {bookToOpen.SourceLink}");
            }
            else
            {
                Console.WriteLine($"Книга '{title}' не найдена или для нее не указана ссылка на источник.");
            }
        }

        private void SaveBooks()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(FilePath, JsonSerializer.Serialize(books, options));
        }

        private void LoadBooks()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                books = JsonSerializer.Deserialize<List<Book>>(json);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();

            while (true)
            {
                Console.WriteLine("\nДоступные действия: ");
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу");
                Console.WriteLine("3. Просмотреть книги");
                Console.WriteLine("4. Открыть ссылку на источник");
                Console.WriteLine("5. Выход");

                Console.Write("Выберите действие (1-5): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите название книги: ");
                        string title = Console.ReadLine();
                        Console.Write("Введите автора книги: ");
                        string author = Console.ReadLine();
                        Console.Write("Введите год издания: ");
                        int year = int.Parse(Console.ReadLine());
                        Console.Write("Введите ссылку на источник: ");
                        string sourceLink = Console.ReadLine();
                        library.AddBook(new Book(title, author, year, sourceLink));
                        break;
                    case "2":
                        Console.Write("Введите название книги для удаления: ");
                        string removeTitle = Console.ReadLine();
                        library.RemoveBook(removeTitle);
                        break;
                    case "3":
                        library.ListBooks();
                        break;
                    case "4":
                        Console.Write("Введите название книги для открытия ссылки: ");
                        string openTitle = Console.ReadLine();
                        library.OpenBookSource(openTitle);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Недействительный выбор. Пожалуйста, выберите действие от 1 до 5.");
                        break;
                }
            }
        }
    }
}