
using EFIntro.Consola.Validators;
using EFIntro.Data;
using EFIntro.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EFIntro.Consola
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                Console.WriteLine("BOOKS");
                Console.WriteLine("1 - List of Books");
                Console.WriteLine("2 - Add New Book");
                Console.WriteLine("3 - Delete a Book");
                Console.WriteLine("4 - Edit a Book");
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
                    case "r":
                        return;
                    default:
                        break;
                }

            } while (true);
        }

        private static void EditBooks()
        {
            Console.Clear();
            Console.WriteLine("Editing Books");
            Console.WriteLine("list Of Books to Edit");
            using (var context=new LibraryContext())
            {
                //var books = context.Books.OrderBy(b => b.Id)
                //    .Select(b => new
                //    {
                //        BookId = b.Id,
                //        BookTitle = b.Title
                //    }).ToList();
                //foreach (var item in books)
                //{
                //    Console.WriteLine($"{item.BookId}-{item.BookTitle}");
                //}
                var books = context.Books.OrderBy(b => b.Id)
                    .Select(b => new
                    {
                        b.Id,
                        b.Title
                    }).ToList();
                foreach (var item in books)
                {
                    Console.WriteLine($"{item.Id}-{item.Title}");
                }
                Console.Write("Enter BookID to edit (0 to Escape):");
                int bookId = int.Parse(Console.ReadLine()!);
                if(bookId < 0)
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

                var bookInDb = context.Books.Include(b=>b.Author)
                    .FirstOrDefault(b=>b.Id==bookId);
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
                    if (!int.TryParse(newPages,out int bookPages)||bookPages<=0)
                    {
                        Console.WriteLine("You enter an invalid page count");
                        Console.ReadLine();
                        return;
                    }
                    bookInDb.Pages=bookPages;
                }

                Console.WriteLine($"Current Book Publish Date: {bookInDb.PublishDate}");
                Console.Write("Enter New Book Publish Date (or ENTER to Keep the same):");
                var newDate = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDate))
                {
                    if (!DateOnly.TryParse(newDate,out DateOnly publishDate) ||
                        publishDate>DateOnly.FromDateTime(DateTime.Today))
                    {
                        Console.WriteLine("Invalid Publish Date...");
                        Console.ReadLine();
                        return;
                    }
                    bookInDb.PublishDate=publishDate;
                }
                Console.WriteLine($"Current Book Author:{bookInDb.Author }");
                Console.WriteLine("Available Authors");
                var authors = context.Authors
                    .OrderBy(a=>a.Id)
                    .ToList();
                foreach (var  author in authors)
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
                    if (authorId>0)
                    {
                        var existAuthor = context.Authors.Any(a => a.Id == authorId);
                        if (!existAuthor)
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
                        var existingAuthor = context.Authors.FirstOrDefault(
                                a => a.FirstName.ToLower() == firstName!.ToLower()
                            && a.LastName.ToLower() == lastName!.ToLower());

                        if (existingAuthor is not null)
                        {
                            Console.WriteLine("You have entered an existing author!!!");
                            Console.WriteLine("Assigning his AuthorID");

                            bookInDb.AuthorId= existingAuthor.Id;
                        }
                        else
                        {
                            Author Author = new Author
                            {
                                FirstName = firstName ?? string.Empty,
                                LastName = lastName ?? string.Empty,
                            };

                            var validationContext = new ValidationContext(Author);
                            var errorMessages = new List<ValidationResult>();

                            bool isValid = Validator.TryValidateObject(Author, validationContext, errorMessages, true);

                            if (isValid)
                            {
                                //bookInDb.Author = Author;
                                //Alternativa
                                context.Authors.Add(Author);
                                context.SaveChanges();
                                bookInDb.AuthorId = Author.Id;
                            }
                            else
                            {
                                foreach (var message in errorMessages)
                                {
                                    Console.WriteLine(message);
                                }
                            }

                        }
                    }

                }

                var originalBook = context.Books
                    .AsNoTracking()
                    .FirstOrDefault(a => a.Id == bookInDb.Id);

                Console.Write($"Are you sure to edit \"{originalBook!.Title}\"? (y/n):");
                var confirm = Console.ReadLine();
                try
                {
                    if (confirm?.ToLower() == "y")
                    {
                        context.SaveChanges();
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
                return;


            }
        }

        private static void DeleteBooks()
        {
            Console.Clear();
            Console.WriteLine("Deleting Books");
            Console.WriteLine("List of Books to Delete");
            using (var context=new LibraryContext())
            {
                var books = context.Books
                    .OrderBy(b=>b.Id)
                    .Select(b => new
                    {
                        b.Id,
                        b.Title
                    }).ToList();
                foreach (var bok in books)
                {
                    Console.WriteLine($"{bok.Id} - {bok.Title}");
                }
                Console.Write("Select BookID to Delete (0 to Escape):");
                if (!int.TryParse(Console.ReadLine(),out int bookId)|| bookId < 0)
                {
                    Console.WriteLine("Invalid BookID...");
                    Console.ReadLine();
                    return;
                }
                if (bookId==0)
                {
                    Console.WriteLine("Cancelled by user");
                    Console.ReadLine();
                    return;
                }
                var deleteBook = context.Books.Find(bookId);
                if (deleteBook is null)
                {
                    Console.WriteLine("Book does not exist!!!");
                }
                else
                {
                    context.Books.Remove(deleteBook);
                    context.SaveChanges();
                    Console.WriteLine("Book Successfully Deleted");
                }
                Console.ReadLine();
                return;
            }

        }

        private static void AddBooks()
        {
            Console.Clear();
            Console.WriteLine("Adding New Books");
            Console.Write("Enter book's title:");
            var title=Console.ReadLine();
            Console.Write("Enter Publish Date (dd/mm/yyyy):");
            if(!DateOnly.TryParse(Console.ReadLine(), out var publishDate))
            {
                Console.WriteLine("Wrong Date....");
                Console.ReadLine();
                return;
            }
            Console.Write("Enter Page Count:");
            if(!int.TryParse(Console.ReadLine(),out var pages))
            {
                Console.WriteLine("Wrong Page Count...");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("List of Authors to Select");
            using (var context=new LibraryContext())
            {
                var authorsList = context.Authors
                    .OrderBy(a=>a.Id)
                    .ToList();
                foreach (var author in authorsList)
                {
                    Console.WriteLine($"{author.Id} - {author}");
                }
                Console.Write("Enter AuthorID (0 New Author):");
                if(!int.TryParse(Console.ReadLine(),out var authorId)|| authorId < 0)
                {
                    Console.WriteLine("Invalid AuthorID....");
                    Console.ReadLine();
                    return;
                }
                if (authorId > 0)
                {
                    var selectedAuthor = context.Authors.Find(authorId);
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
                        //bool exist=context.Books.Any(b=>b.Title.ToLower()== title.ToLower() && 
                        //    b.AuthorId==authorId);
                        var existingBook = context.Books.FirstOrDefault(b => b.Title.ToLower() == title!.ToLower() &&
                            b.AuthorId == authorId);

                        if (existingBook is null)
                        {
                            context.Books.Add(newBook);
                            context.SaveChanges();
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
                    var existingAuthor = context.Authors.FirstOrDefault(
                            a => a.FirstName.ToLower() == firstName!.ToLower()
                        && a.LastName.ToLower() == lastName!.ToLower());
                    if (existingAuthor is not null)
                    {
                        Console.WriteLine("You have entered an existing author!!!");
                        Console.WriteLine("Assigning his AuthorID");

                        var newBook = new Book
                        {
                            Title = title ?? string.Empty,
                            PublishDate = publishDate,
                            Pages = pages,
                            AuthorId = existingAuthor.Id
                        };

                        var booksValidator = new BooksValidator();
                        var validationResult = booksValidator.Validate(newBook);

                        if (validationResult.IsValid)
                        {
                            //bool exist=context.Books.Any(b=>b.Title.ToLower()== title.ToLower() && 
                            //    b.AuthorId==authorId);
                            var existingBook = context.Books.FirstOrDefault(b => b.Title.ToLower() == title!.ToLower() &&
                                b.AuthorId == authorId);

                            if (existingBook is null)
                            {
                                context.Books.Add(newBook);
                                context.SaveChanges();
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
                        Author newAuthor = new Author
                        {
                            FirstName = firstName ?? string.Empty,
                            LastName = lastName ?? string.Empty,
                        };

                        var validationContext = new ValidationContext(newAuthor);
                        var errorMessages = new List<ValidationResult>();

                        bool isValid = Validator.TryValidateObject(newAuthor, validationContext, errorMessages, true);

                        if (isValid)
                        {
                            var newBook = new Book
                            {
                                Title = title ?? string.Empty,
                                PublishDate = publishDate,
                                Pages = pages,
                                Author = newAuthor
                            };

                            var booksValidator = new BooksValidator();
                            var validationResult = booksValidator.Validate(newBook);

                            if (validationResult.IsValid)
                            {
                                //bool exist=context.Books.Any(b=>b.Title.ToLower()== title.ToLower() && 
                                //    b.AuthorId==authorId);
                                var existingBook = context.Books.FirstOrDefault(b => b.Title.ToLower() == title!.ToLower() &&
                                    b.AuthorId == authorId);

                                if (existingBook is null)
                                {
                                    context.Add(newBook);
                                    context.SaveChanges();
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
                            foreach (var message in errorMessages)
                            {
                                Console.WriteLine(message);
                            }
                        }

                    }
                }
                Console.ReadLine();
                return;
            }
        }

        private static void BooksList()
        {
            Console.Clear();
            Console.WriteLine("List of Books");
            using (var context=new LibraryContext())
            {
                var books = context.Books
                    .Include(b=>b.Author)
                    .Select(b=>new 
                    {
                        b.Id,
                        b.Title,
                        b.Author
                    })
                    .ToList();
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
                Console.WriteLine("AUTHORS");
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

            using (var context = new LibraryContext())
            {
                var authors = context.Authors
                    .OrderBy(a => a.Id)
                    .Select(a => new
                    {
                        a.Id,
                        FullName = $"{a.FirstName} {a.LastName}",
                        BookCount = context.Books.Count(b => b.AuthorId == a.Id),
                        Books = context.Books
                            .Where(b => b.AuthorId == a.Id)
                            .OrderBy(b => b.Title)
                            .Select(b => new { b.Title, b.PublishDate, b.Pages })
                            .ToList()
                    })
                    .ToList();

                foreach (var a in authors)
                {
                    Console.WriteLine($"{a.Id} - {a.FullName} (Books: {a.BookCount})");

                    if (option == "2") // Opción de detalle
                    {
                        if (a.Books.Any())
                        {
                            Console.WriteLine("   📚 Books:");
                            foreach (var book in a.Books)
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
            using (var context=new LibraryContext())
            {
                var authorGroups = context.Books
                    .GroupBy(a => a.AuthorId).ToList();
                foreach (var group in authorGroups)
                {
                    Console.WriteLine($"AuthorID: {group.Key}");
                    var author = context.Authors.Find(group.Key);
                    Console.WriteLine($"Author: {author}");
                    foreach(var book in group)
                    {
                        Console.WriteLine($"    {book.Title}");
                    }
                    Console.WriteLine($"Books Count: {group.Count()}");
                    Console.WriteLine($"Average Page Count: {group.Average(b=>b.Pages)}");
                }
            }
            Console.ReadLine();
        }

        private static void EditAuthors()
        {
            Console.Clear();
            Console.WriteLine("Edit An Author");
            using (var context = new LibraryContext())
            {
                var authors = context.Authors
                    .OrderBy(a => a.Id)
                    .ToList();
                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.Id} - {author}");
                }
                Console.Write("Enter an AuthorID to edit:");
                int autorId;
                if (!int.TryParse(Console.ReadLine(), out autorId) || autorId <= 0)
                {
                    Console.WriteLine("Invalid AuthorId!!!");
                    Console.ReadLine();
                    return;
                }

                var authorInDb = context.Authors.Find(autorId);
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

                var originalAuthor = context.Authors
                    .AsNoTracking()
                    .FirstOrDefault(a => a.Id == authorInDb.Id);

                Console.Write($"Are you sure to edit \"{originalAuthor!.FirstName} {originalAuthor.LastName}\"? (y/n):");
                var confirm = Console.ReadLine();
                if (confirm?.ToLower() == "y")
                {
                    context.SaveChanges();
                    Console.WriteLine("Author successfully edited");
                }
                else
                {
                    Console.WriteLine("Operation cancelled by user");
                }
                Console.ReadLine();
                return;
            }
        }

        private static void DeleteAuthors()
        {
            Console.Clear();
            Console.WriteLine("Delete An Author");
            using (var context = new LibraryContext())
            {
                var authors = context.Authors
                    .OrderBy(a => a.Id)
                    .ToList();
                foreach (var author in authors)
                {
                    Console.WriteLine($"{author.Id} - {author}");
                }
                Console.Write("Enter an AuthorID to delete:");
                int autorId;
                if (!int.TryParse(Console.ReadLine(), out autorId) || autorId <= 0)
                {
                    Console.WriteLine("Invalid AuthorId!!!");
                    Console.ReadLine();
                    return;
                }

                var authorInDb = context.Authors.Find(autorId);
                if (authorInDb == null)
                {
                    Console.WriteLine("Author does not exist");
                    Console.ReadLine();
                    return;
                }
                var hasBooks=context.Books.Any(b=>b.AuthorId==authorInDb.Id);
                if (!hasBooks)
                {
                    Console.Write($"Are you sure to delete \"{authorInDb.FirstName} {authorInDb.LastName}\"? (y/n):");
                    var confirm = Console.ReadLine();
                    if (confirm?.ToLower() == "y")
                    {
                        context.Authors.Remove(authorInDb);
                        context.SaveChanges();
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
                    context.Entry(authorInDb).Collection(a => a.Books!).Load();
                    foreach(var book in authorInDb.Books!)
                    {
                        Console.WriteLine($"{book.Title}");
                    }
                }

                Console.ReadLine();
                return;
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

            using (var context = new LibraryContext())
            {
                bool exist = context.Authors.Any(a => a.FirstName == firstName &&
                    a.LastName == lastName);

                if (!exist)
                {
                    var author = new Author
                    {
                        FirstName = firstName??string.Empty,
                        LastName = lastName??string.Empty
                    };

                    var validationContext = new ValidationContext(author);
                    var errorMessages = new List<ValidationResult>();

                    bool isValid = Validator.TryValidateObject(author, validationContext, errorMessages, true);

                    if (isValid)
                    {
                        context.Authors.Add(author);
                        context.SaveChanges();
                        Console.WriteLine("Author Succesfully added");

                    }
                    else
                    {
                        foreach (var message in errorMessages)
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
            using (var context = new LibraryContext())
            {
                var authors = context.Authors
                    .OrderBy(a => a.LastName)
                    .ThenBy(a => a.FirstName)
                    .AsNoTracking()
                    .ToList();
                foreach (var author in authors)
                {
                    Console.WriteLine(author);
                }
                Console.WriteLine("ENTER to continue");
                Console.ReadLine();
            }
        }

        private static void CreateDb()
        {
            using (var context = new LibraryContext())
            {
                context.Database.EnsureCreated();
            }
            Console.WriteLine("Database created!!!");
        }
    }
}
