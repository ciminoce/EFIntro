using EFIntro.Data.Interfaces;
using EFIntro.Entities;
using EFIntro.Service.Interfaces;

namespace EFIntro.Service.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository = null!;

        public AuthorService(IAuthorRepository repository)
        {
            _authorRepository = repository;
        }

        public List<IGrouping<int, Book>> AuthorsWithBooksCount()
        {
            return _authorRepository.AuthorsWithBooksCount();
        }

        public void Delete(int authorId)
        {
            _authorRepository.Delete(authorId);
        }

        public bool Exist(string firstName, string lastName, int? excludeId = null)
        {
            return _authorRepository.Exist(firstName, lastName, excludeId);
        }

        public List<Author> GetAll(string sortedBy = "LastName")
        {
            return _authorRepository.GetAll(sortedBy);
        }

        public List<Author> GetAllWithBooks()
        {
            return _authorRepository.GetAllWithBooks();
        }

        public Author? GetById(int authorId, bool tracked = false)
        {
            return _authorRepository.GetById(authorId, tracked);
        }

        public Author? GetByName(string firstName, string lastName)
        {
            return _authorRepository.GetByName(firstName, lastName);
        }

        public bool HasBooks(int authorId)
        {
            return _authorRepository.HasBooks(authorId);
        }

        public void LoadBooks(Author author)
        {
            _authorRepository.LoadBooks(author);
        }

        public void Save(Author author)
        {
            if (author.Id==0)
            {
                _authorRepository.Add(author);
            }
            else
            {
                _authorRepository.Edit(author);
            }
        }
    }
}
