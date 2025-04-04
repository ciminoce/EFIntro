
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
                        //EditAuthors();
                        break;
                    case "r":
                        return;
                    default:
                        break;
                }

            } while (true);
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
                var selectedAuthor = context.Authors.Find(authorId);
                if(selectedAuthor is null)
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
                    case "r":
                        return;
                    default:
                        break;
                }

            } while (true);

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
