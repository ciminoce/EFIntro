using EFIntro.Consola.Validators;
using EFIntro.Entities;
using EFIntro.Ioc;
using EFIntro.Service.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EFIntro.Consola
{
    internal class Program
    {
        static IServiceProvider _serviceProvider = null!;

        static void Main(string[] args)
        {
            _serviceProvider = DI.ConfigureDI();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //CreateDb();
            do
            {
                Console.Clear();
                Console.WriteLine("Main Menu");
                Console.WriteLine("1 - Authors");
                Console.WriteLine("2 - Books");
                Console.WriteLine("x - Exit");
                Console.Write("Enter an option:");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        AuthorsMenu();
                        break;
                    case "2":
                        BooksMenu();
                        break;
                    case "x":
                        Console.WriteLine("Fin del Programa");
                        return;
                    default:
                        break;
                }

            } while (true);
        }

        private static void BooksMenu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("📕 BOOKS");
                Console.WriteLine("1 - List of Books");
                Console.WriteLine("2 - Add New Book");
                Console.WriteLine("3 - Delete a Book");
                Console.WriteLine("4 - Edit a Book");
                Console.WriteLine("5 - Books Group By Author");
                Console.WriteLine("r - Return");
                Console.Write("Enter an option:");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        BooksList();
                        break;
                    case "2":
                        AddBooks();
                        break;
                    case "3":
                        DeleteBooks();
                        break;
                    case "4":
                        EditBooks();
                        break;
                    case "5":
                        BooksGroupByAuthor();
                        break;
                    case "r":
                        return;
                    default:
                        break;
                }

            } while (true);
        }

        private static void BooksGroupByAuthor()
        {
            Console.Clear();
            Console.WriteLine("List of Books");
            using (var scope = _serviceProvider.CreateScope())
            {
                var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
                var authorService=scope.ServiceProvider.GetRequiredService<IAuthorService>();

                var groups = bookService.BooksGroupByAuthor();
                foreach (var group in groups)
                {
                    var author = authorService.GetById(group.Key);
                    Console.WriteLine($"AuthorId:{group.Key} - Author: {author}");
                    Console.WriteLine("    Books");
                    foreach (var book in group)
                    {
                        Console.WriteLine($"        {book.Title}");

                    }
                }
            }
            Console.WriteLine("ENTER to continue");
            Console.ReadLine();

        }

        private static void EditBooks()
        {
            Console.Clear();
            Console.WriteLine("Editing Books");
            Console.WriteLine("list Of Books to Edit");
            using (var scope=_serviceProvider.CreateScope())
            {
                var bookService=scope.ServiceProvider.GetRequiredService<IBookService>();
                var authorService=scope.ServiceProvider.GetRequiredService<IAuthorService>();

                var books = bookService.GetAll("Id");
                foreach (var item in books)
                {
                    Console.WriteLine($"{item.Id}-{item.Title}");
                }

                Console.Write("Enter BookID to edit (0 to Escape):");
                int bookId = int.Parse(Console.ReadLine()!);
                if (bookId < 0)
                {
                    Console.WriteLine("Invalid BookID... ");
                    Console.ReadLine();
                    return;
                }
                if (bookId == 0)
                {
                    Console.WriteLine("Cancelled by user");
                    Console.ReadLine();
                    return;
                }

                var bookInDb = bookService.GetById(bookId);
                if (bookInDb == null)
                {
                    Console.WriteLine("Book does not exist...");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine($"Current Book Title: {bookInDb.Title}");
                Console.Write("Enter New Title (or ENTER to Keep the same):");
                var newTitle = Console.ReadLine();
                if (!string.IsNullOrEmpty(newTitle))
                {
                    bookInDb.Title = newTitle;
                }
                Console.WriteLine($"Current Book Pages Count: {bookInDb.Pages}");
                Console.Write("Enter Book Pages Count (or ENTER to Keep the same):");
                var newPages = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPages))
                {
                    if (!int.TryParse(newPages, out int bookPages) || bookPages <= 0)
                    {
                        Console.WriteLine("You enter an invalid page count");
                        Console.ReadLine();
                        return;
                    }
                    bookInDb.Pages = bookPages;
                }

                Console.WriteLine($"Current Book Publish Date: {bookInDb.PublishDate}");
                Console.Write("Enter New Book Publish Date (or ENTER to Keep the same):");
                var newDate = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDate))
                {
                    if (!DateOnly.TryParse(newDate, out DateOnly publishDate) ||
                        publishDate > DateOnly.FromDateTime(DateTime.Today))
                    {
                        Console.WriteLine("Invalid Publish Date...");
                        Console.ReadLine();
                        return;
                    }
                    bookInDb.PublishDate = publishDate;
                }
                Console.WriteLine($"Current Book Author:{bookInDb.Author}");
                Console.WriteLine("Available Authors");
                var authors = authorService.GetAll("Id");
                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.Id}-{author}");
                }
                Console.Write("Enter AuthorID (or ENTER to Keep the same or 0 New Author):");
                var newAuthor = Console.ReadLine();
                if (!string.IsNullOrEmpty(newAuthor))
                {
                    if (!int.TryParse(newAuthor, out int authorId) || authorId < 0)
                    {
                        Console.WriteLine("You enter an invalid AuthorID");
                        Console.ReadLine();
                        return;
                    }
                    if (authorId > 0)
                    {
                        var existingAuthor =authorService.GetById(authorId);
                        if (existingAuthor is null)
                        {
                            Console.WriteLine("AuthorID not found");
                            Console.ReadLine();
                            return;
                        }
                        bookInDb.AuthorId = authorId;

                    }
                    else
                    {
                        //Entering new author
                        Console.WriteLine("Adding a New Author");
                        Console.Write("Enter First Name:");
                        var firstName = Console.ReadLine();
                        Console.Write("Enter Last Name:");
                        var lastName = Console.ReadLine();
                        var existingAuthor = authorService.GetByName(firstName ?? string.Empty,
                            lastName ?? string.Empty);

                        if (existingAuthor is not null)
                        {
                            Console.WriteLine("You have entered an existing author!!!");
                            Console.WriteLine("Assigning his AuthorID");

                            bookInDb.AuthorId = existingAuthor.Id;
                        }
                        else
                        {
                            Author author = new Author
                            {
                                FirstName = firstName ?? string.Empty,
                                LastName = lastName ?? string.Empty,
                            };

                            var authorValidator = new AuthorValidator();
                            var authorValidationResult = authorValidator.Validate(author);

                            

                            if (authorValidationResult.IsValid)
                            {
                                //bookInDb.Author = Author;
                                //Alternativa
                                authorService.Save(author);
                                bookInDb.AuthorId = author.Id;
                            }
                            else
                            {
                                foreach (var error in authorValidationResult.Errors)
                                {
                                    Console.WriteLine(error.ErrorMessage);
                                }
                            }

                        }
                    }

                }

                var originalBook = bookService.GetById(bookId);

                Console.Write($"Are you sure to edit \"{originalBook!.Title}\"? (y/n):");
                var confirm = Console.ReadKey().KeyChar;
                try
                {
                    if (confirm.ToString().ToLower() == "y")
                    {
                        bookService.Save(bookInDb);
                        Console.WriteLine("Book successfully edited");
                    }
                    else
                    {
                        Console.WriteLine("Operation cancelled by user");
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();


            }
        }

        private static void DeleteBooks()
        {
            Console.Clear();
            Console.WriteLine("Deleting Books");
            Console.WriteLine("List of Books to Delete");
            using (var scope = _serviceProvider.CreateScope())
            {
                var bookService=scope.ServiceProvider.GetRequiredService<IBookService>();

                var books = bookService.GetAll("Id");
                foreach (var bok in books)
                {
                    Console.WriteLine($"{bok.Id} - {bok.Title}");
                }

                Console.Write("Select BookID to Delete (0 to Escape):");
                if (!int.TryParse(Console.ReadLine(), out int bookId) || bookId < 0)
                {
                    Console.WriteLine("Invalid BookID...");
                    Console.ReadLine();
                    return;
                }
                if (bookId == 0)
                {
                    Console.WriteLine("Cancelled by user");
                    Console.ReadLine();
                    return;
                }
                var deleteBook = bookService.GetById(bookId);
                if (deleteBook is null)
                {
                    Console.WriteLine("Book does not exist!!!");
                }
                else
                {
                    Console.WriteLine($"Are you sure to delete {deleteBook.Title}?");
                    var response=Console.ReadKey().KeyChar;
                    if (response.ToString().ToUpper() == "S")
                    {
                        bookService.Delete(bookId);
                        Console.WriteLine("Book Successfully Deleted");
                    }
                    else
                    {
                        Console.WriteLine("Operation cancelled by user");
                    }
                }
                Console.ReadLine();
            }

        }

        private static void AddBooks()
        {
            Console.Clear();
            Console.WriteLine("Adding New Books");
            Console.Write("Enter book's title:");
            var title = Console.ReadLine();
            Console.Write("Enter Publish Date (dd/mm/yyyy):");
            if (!DateOnly.TryParse(Console.ReadLine(), out var publishDate))
            {
                Console.WriteLine("Wrong Date....");
                Console.ReadLine();
                return;
            }
            Console.Write("Enter Page Count:");
            if (!int.TryParse(Console.ReadLine(), out var pages))
            {
                Console.WriteLine("Wrong Page Count...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("List of Authors to Select");
            using (var scope = _serviceProvider.CreateScope())
            {
                var authorService=scope.ServiceProvider.GetService<IAuthorService>();
                var bookService=scope.ServiceProvider.GetService<IBookService>();

                var authorsList = authorService!.GetAll("Id");
                foreach (var author in authorsList)
                {
                    Console.WriteLine($"{author.Id} - {author}");
                }

                Console.Write("Enter AuthorID (0 New Author):");
                if (!int.TryParse(Console.ReadLine(), out var authorId) || authorId < 0)
                {
                    Console.WriteLine("Invalid AuthorID....");
                    Console.ReadLine();
                    return;
                }
                if (authorId > 0)
                {
                    var selectedAuthor = authorService.GetById(authorId);
                    if (selectedAuthor is null)
                    {
                        Console.WriteLine("Author not found!!!");
                        Console.ReadLine();
                        return;
                    }
                    var newBook = new Book
                    {
                        Title = title ?? string.Empty,
                        PublishDate = publishDate,
                        Pages = pages,
                        AuthorId = authorId
                    };

                    var booksValidator = new BooksValidator();
                    var validationResult = booksValidator.Validate(newBook);

                    if (validationResult.IsValid)
                    {
                        var existBook = bookService!.Exist(title??string.Empty,authorId);

                        if (!existBook)
                        {
                            bookService.Save(newBook);
                            Console.WriteLine("Book Successfully Added!!!");

                        }
                        else
                        {
                            Console.WriteLine("Book duplicated!!!");
                        }

                    }
                    else
                    {
                        foreach (var error in validationResult.Errors)
                        {
                            Console.WriteLine(error);
                        }
                    }

                }
                else
                {
                    //Entering new author
                    Console.WriteLine("Adding a New Author");
                    Console.Write("Enter First Name:");
                    var firstName = Console.ReadLine();
                    Console.Write("Enter Last Name:");
                    var lastName = Console.ReadLine();

                    var existAuthor = authorService.Exist(firstName??string.Empty,
                            lastName??string.Empty);
                    if (existAuthor)
                    {
                        var existingAuthor=authorService.GetByName(firstName??string.Empty,lastName??string.Empty);

                        Console.WriteLine("You have entered an existing author!!!");
                        Console.WriteLine("Assigning his AuthorID");

                        var newBook = new Book
                        {
                            Title = title ?? string.Empty,
                            PublishDate = publishDate,
                            Pages = pages,
                            AuthorId = existingAuthor!.Id
                        };

                        var booksValidator = new BooksValidator();
                        var bookValidationResult = booksValidator.Validate(newBook);

                        if (bookValidationResult.IsValid)
                        {
                            var existBook = bookService!.Exist(newBook.Title, newBook.AuthorId);
                            if (!existBook)
                            {
                                bookService.Save(newBook);
                                Console.WriteLine("Book Successfully Added!!!");

                            }
                            else
                            {
                                Console.WriteLine("Book duplicated!!!");
                            }

                        }
                        else
                        {
                            foreach (var error in bookValidationResult.Errors)
                            {
                                Console.WriteLine(error);
                            }
                        }


                    }
                    else
                    {
                        Author newAuthor = new Author
                        {
                            FirstName = firstName ?? string.Empty,
                            LastName = lastName ?? string.Empty,
                        };

                        var authorValidator = new AuthorValidator();
                        var authorValidationResult = authorValidator.Validate(newAuthor);

                        

                        if (authorValidationResult.IsValid)
                        {
                            authorService.Save(newAuthor);

                            var newBook = new Book
                            {
                                Title = title ?? string.Empty,
                                PublishDate = publishDate,
                                Pages = pages,
                                AuthorId=newAuthor.Id,
                            };

                            var booksValidator = new BooksValidator();
                            var bookValidationResult = booksValidator.Validate(newBook);

                            if (bookValidationResult.IsValid)
                            {
                                var existBook = bookService!.Exist(newBook.Title, newBook.AuthorId);

                                if (!existBook)
                                {
                                    bookService.Save(newBook);
                                    Console.WriteLine("Book Successfully Added!!!");

                                }
                                else
                                {
                                    Console.WriteLine("Book duplicated!!!");
                                }

                            }
                            else
                            {
                                foreach (var error in bookValidationResult.Errors)
                                {
                                    Console.WriteLine(error);
                                }
                            }


                        }
                        else
                        {
                            foreach (var error in authorValidationResult.Errors)
                            {
                                Console.WriteLine(error);
                            }
                        }

                    }
                }
                Console.ReadLine();
            }
        }

        private static void BooksList()
        {
            Console.Clear();
            Console.WriteLine("List of Books");
            using (var scope=_serviceProvider.CreateScope())
            {
                var bookService=scope.ServiceProvider.GetRequiredService<IBookService>();

                var books = bookService.GetAll(include:true);
                foreach (var bo in books)
                {
                    Console.WriteLine($"{bo.Title} - Author:{bo.Author}");
                }
            }
            Console.WriteLine("ENTER to continue");
            Console.ReadLine();
        }

        private static void AuthorsMenu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("✍️ AUTHORS");
                Console.WriteLine("1 - List of Authors");
                Console.WriteLine("2 - Add New Author");
                Console.WriteLine("3 - Delete an Author");
                Console.WriteLine("4 - Edit an Author");
                Console.WriteLine("5 - List of Authors With Books");
                Console.WriteLine("6 - Authors With Books (Summary or Details)");
                Console.WriteLine("r - Return");
                Console.Write("Enter an option:");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        AuthorsList();
                        break;
                    case "2":
                        AddAuthors();
                        break;
                    case "3":
                        DeleteAuthors();
                        break;
                    case "4":
                        EditAuthors();
                        break;
                    case "5":
                        ListOfAuthorsWithBooks();
                        break;
                    case "6":
                        AuthorsWithBooksSummaryOrDetails();
                        break;
                    case "r":
                        return;
                    default:
                        break;
                }

            } while (true);

        }

        private static void AuthorsWithBooksSummaryOrDetails()
        {
            Console.Clear();
            Console.WriteLine("List of Authors");

            Console.Write("Show (1) Summary or (2) Details? ");
            var option = Console.ReadLine();

            using (var scope = _serviceProvider.CreateScope())
            {
                var _service = scope.ServiceProvider.GetRequiredService<IAuthorService>();

                var authorsWithBooks = _service.GetAllWithBooks();
                foreach (var author in authorsWithBooks)
                {
                    Console.WriteLine($"{author.Id} - {author} (Books: {author.Books?.Count??0})");

                    if (option == "2") // Opción de detalle
                    {
                        if (author.Books!.Any())
                        {
                            Console.WriteLine("   📚 Books:");
                            foreach (var book in author.Books!)
                            {
                                Console.WriteLine($"     - {book.Title} ({book.PublishDate}) - {book.Pages} pages");
                            }
                        }
                        else
                        {
                            Console.WriteLine("   🚫 No books available.");
                        }
                    }
                }

            }

            Console.ReadLine();
        }

        private static void ListOfAuthorsWithBooks()
        {
            Console.Clear();
            Console.WriteLine("List of Authors With Books");
            using (var scope = _serviceProvider.CreateScope())
            {
                var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();
                var authorGroups = authorService.AuthorsWithBooksCount();
                foreach (var group in authorGroups)
                {
                    Console.WriteLine($"AuthorID: {group.Key}");
                    var author = authorService.GetById(group.Key);
                    Console.WriteLine($"Author: {author}");
                    foreach (var book in group)
                    {
                        Console.WriteLine($"    {book.Title}");
                    }
                    Console.WriteLine($"Books Count: {group.Count()}");
                    Console.WriteLine($"Average Page Count: {group.Average(b => b.Pages)}");
                }
            }
            Console.ReadLine();
        }

        private static void EditAuthors()
        {
            Console.Clear();
            Console.WriteLine("Edit An Author");
            using (var scope = _serviceProvider.CreateScope())
            {
                var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();
                var authors = authorService.GetAll("Id");
                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.Id} - {author}");
                }
                Console.Write("Enter an AuthorID to edit (0 to Escape):");
                int authorId;
                if (!int.TryParse(Console.ReadLine(), out authorId) || authorId < 0)
                {
                    Console.WriteLine("Invalid AuthorId!!!");
                    Console.ReadLine();
                    return;
                }
                if (authorId == 0) return;
                var authorInDb = authorService.GetById(authorId);
                if (authorInDb == null)
                {
                    Console.WriteLine("Author does not exist");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine($"Current Author First Name: {authorInDb.FirstName}");
                Console.Write("Enter New First Name (or ENTER to Keep the same)");
                var newFirstName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newFirstName))
                {
                    authorInDb.FirstName = newFirstName;
                }
                Console.WriteLine($"Current Author Last Name: {authorInDb.LastName}");
                Console.Write("Enter New Last Name (or ENTER to Keep the same)");
                var newLastName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newLastName))
                {
                    authorInDb.LastName = newLastName;
                }

                var originalAuthor = authorService.GetById(authorId);

                Console.Write($"Are you sure to edit \"{originalAuthor!.FirstName} {originalAuthor.LastName}\"? (y/n):");
                var confirm = Console.ReadLine();
                if (confirm?.ToLower() == "y")
                {
                    bool exist = authorService.Exist(authorInDb.FirstName,
                        authorInDb.LastName,
                        authorInDb.Id);
                    if (!exist)
                    {
                        var authorValidator = new AuthorValidator();

                        var result = authorValidator.Validate(authorInDb);

                        if (result.IsValid)
                        {
                            authorService.Save(authorInDb);
                            Console.WriteLine("Author successfully edited");

                        }
                        else
                        {
                            foreach (var message in result.Errors)
                            {
                                Console.WriteLine(message);
                            }

                        }

                    }
                    else
                    {
                        Console.WriteLine("Author already exist!!!");
                    }
                }
                else
                {
                    Console.WriteLine("Operation cancelled by user");
                }
                Console.ReadLine();
            }
        }

        private static void DeleteAuthors()
        {
            Console.Clear();
            Console.WriteLine("Delete An Author");
            using (var scope = _serviceProvider.CreateScope())
            {
                var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();
                var authors = authorService.GetAll("Id");
                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.Id} - {author}");
                }

                Console.Write("Enter an AuthorID to delete (0 to Escape):");
                int authorId;
                if (!int.TryParse(Console.ReadLine(), out authorId) || authorId < 0)
                {
                    Console.WriteLine("Invalid AuthorId!!!");
                    Console.ReadLine();
                    return;
                }
                if (authorId == 0) return;

                var authorInDb = authorService.GetById(authorId);
                if (authorInDb == null)
                {
                    Console.WriteLine("Author does not exist");
                    Console.ReadLine();
                    return;
                }
                var hasBooks = authorService.HasBooks(authorId);
                if (!hasBooks)
                {
                    Console.Write($"Are you sure to delete \"{authorInDb.FirstName} {authorInDb.LastName}\"? (y/n):");
                    var confirm = Console.ReadLine();
                    if (confirm?.ToLower() == "y")
                    {
                        authorService.Delete(authorId);
                        Console.WriteLine("Author successfully removed");
                    }
                    else
                    {
                        Console.WriteLine("Operation cancelled by user");
                    }

                }
                else
                {
                    Console.WriteLine("Author with books!!! Delete deny");
                    authorService.LoadBooks(authorInDb);
                    foreach (var book in authorInDb.Books!)
                    {
                        Console.WriteLine($"{book.Title}");
                    }
                }

                Console.ReadLine();
            }
        }

        private static void AddAuthors()
        {
            Console.Clear();
            Console.WriteLine("Adding a New Author");
            Console.Write("Enter First Name:");
            var firstName = Console.ReadLine();
            Console.Write("Enter Last Name:");
            var lastName = Console.ReadLine();
            using (var scope = _serviceProvider.CreateScope())
            {
                var authorService = scope.ServiceProvider.GetRequiredService<IAuthorService>();

                bool exist = authorService.Exist(firstName??string.Empty, lastName??string.Empty);

                if (!exist)
                {
                    var author = new Author
                    {
                        FirstName = firstName ?? string.Empty,
                        LastName = lastName ?? string.Empty
                    };

                    var authorValidator = new AuthorValidator();

                    var result = authorValidator.Validate(author);

                    if (result.IsValid)
                    {
                        authorService.Save(author);
                        Console.WriteLine("Author Succesfully added");

                    }
                    else
                    {
                        foreach (var message in result.Errors)
                        {
                            Console.WriteLine(message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Author already exist");
                }
            }
            Console.ReadLine();
        }

        private static void AuthorsList()
        {
            Console.Clear();
            Console.WriteLine("List of Authors");
            using (var scope = _serviceProvider.CreateScope())
            {
                var authorService = scope.ServiceProvider
                    .GetRequiredService<IAuthorService>();
                var authors = authorService.GetAll();
                foreach (var author in authors)
                {
                    Console.WriteLine(author);
                }

            }
            Console.WriteLine("ENTER to continue");
            Console.ReadLine();

        }

    }
}
