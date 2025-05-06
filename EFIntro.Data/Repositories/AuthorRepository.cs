using EFIntro.Data.Interfaces;
using EFIntro.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace EFIntro.Data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context = null!;

        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }
        public List<Author> GetAll(string sortedBy = "LastName")
        {
            IQueryable<Author> query = _context.Authors.AsNoTracking();


            //MODERM FORM--> MAS BANANA
            return sortedBy switch
            {
                "LastName" => query.OrderBy(a => a.LastName)
                                        .ThenBy(a => a.FirstName)
                                        .ToList(),
                "FirstName" => query.OrderBy(a => a.FirstName)
                                    .ThenBy(a => a.LastName)
                                    .ToList(),
                _ => query.OrderBy(a => a.Id).ToList(),
            };
        }

        public Author? GetById(int authorId, bool tracked = false)
        {
            return tracked
                ? _context.Authors
                    .FirstOrDefault(a => a.Id == authorId)
                : _context.Authors
                    .AsNoTracking()
                    .FirstOrDefault(a => a.Id == authorId);
        }
        public bool Exist(string firstName, string lastName, int? excludeId = null)
        {
            return excludeId.HasValue
                ? _context.Authors.Any(a => a.FirstName == firstName &&
                    a.LastName == lastName && a.Id != excludeId)
                : _context.Authors.Any(a => a.FirstName == firstName &&
                    a.LastName == lastName);


        }

        public void Add(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();

        }

        public void Delete(int authorId)
        {
            var authorInDb = GetById(authorId,true);
            if (authorInDb != null)
            {
                _context.Authors.Remove(authorInDb);
                _context.SaveChanges();

            }

        }
        public void Edit(Author author)
        {
            var authorInDb = GetById(author.Id,true);
            if (authorInDb != null)
            {
                authorInDb.FirstName = author.FirstName;
                authorInDb.LastName = author.LastName;

                _context.SaveChanges();
            }
        }

        public bool HasBooks(int authorId)
        {
            return _context.Books.Any(b => b.AuthorId == authorId);

        }

        public void LoadBooks(Author author)
        {
            _context.Entry(author).Collection(a => a.Books!).Load();

        }

        public List<Author> GetAllWithBooks()
        {
            return _context.Authors.Include(a => a.Books).ToList();
        }

        public List<IGrouping<int,Book>> AuthorsWithBooksCount()
        {
            var groupedBooksByAuthor =
                from author in _context.Authors
                join book in _context.Books on author.Id equals book.AuthorId into authorBooks
                from book in authorBooks.DefaultIfEmpty() // Left Join
                group book by author.Id into grouped // Agrupar por AuthorId
                select grouped; // Resultado: IGrouping<int, Book>

            // Convertir a lista
            var result = groupedBooksByAuthor.ToList();
            return result;
        }

        public Author? GetByName(string firstName, string lastName)
        {
            return _context.Authors.FirstOrDefault(a=>a.LastName == lastName 
                    && a.FirstName==firstName);
        }
    }
}
