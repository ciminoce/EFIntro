using EFIntro.Data.Interfaces;
using EFIntro.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFIntro.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context = null!;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public List<Book> GetAll(string sortedBy = "Title", bool include = false)
        {
            IQueryable<Book> query = _context.Books.AsNoTracking();
            query=include?query.Include(b=>b.Author):query;
            return sortedBy switch
            {
                "Title" => query.OrderBy(b => b.Title)
                        .ThenBy(b => b.AuthorId).ToList(),
                _ => query.OrderBy(b => b.Id).ToList()

            };
        }
        public Book? GetById(int bookId, bool include = false, bool tracked = false)
        {
            IQueryable<Book> query = _context.Books;
            query = include ? query.Include(b => b.Author) : query;
            return tracked ? query.FirstOrDefault(b => b.Id == bookId)
                : query.AsNoTracking().FirstOrDefault(b => b.Id == bookId);
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }
        public void Update(Book book)
        {
            var bookInDb = GetById(book.Id);
            if (bookInDb != null)
            {
                bookInDb.Title = book.Title;
                bookInDb.AuthorId = book.AuthorId;
                bookInDb.PublishDate = book.PublishDate;
                bookInDb.Pages = book.Pages;

                _context.SaveChanges();
            }
        }
        public void Delete(int bookId)
        {
            var bookInDb = GetById(bookId);
            if (bookInDb != null)
            {
                _context.Books.Remove(bookInDb);
                _context.SaveChanges();
            }

        }

        public bool Exist(string bookTitle, int bookAuthorId, int? excludeId = null)
        {
            return excludeId.HasValue ? _context.Books.Any(b => b.Title == bookTitle
                    && b.AuthorId == bookAuthorId && b.Id != excludeId)
                : _context.Books.Any(b => b.Title == bookTitle
                    && b.AuthorId == bookAuthorId);
        }

        public List<IGrouping<int, Book>> BooksGroupByAuthor()
        {
            return _context.Books.GroupBy(b=>b.AuthorId).ToList();
        }
    }
}
