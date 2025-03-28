
using EFIntro.Data;
using EFIntro.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;

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
                    case "x":
                        return;
                    default:
                        break;
                }

            } while (true);
            Console.WriteLine("Fin del Programa");
        }

        private static void AuthorsMenu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("AUTHORS");
                Console.WriteLine("1 - List ofAuthors");
                Console.WriteLine("2 - Add New Author");
                Console.WriteLine("3 - Delete an Author");
                Console.WriteLine("4 - Edit an Author");
                Console.WriteLine("r - Return");
                Console.Write("Enter an option:");
                var option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        ListAuthors();
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
                        FirstName = firstName,
                        LastName = lastName
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

        private static void ListAuthors()
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
