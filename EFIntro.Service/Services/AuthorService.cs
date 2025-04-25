using EFIntro.Data.Interfaces;
using EFIntro.Entities;
using EFIntro.Service.Interfaces;

namespace EFIntro.Service.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository = null!;

        public AuthorService(IAuthorRepository repository)
        {
            _repository = repository;
        }

        public List<IGrouping<int, Book>> AuthorsGroupIdBooks()
        {
            return _repository.AuthorsGroupIdBooks();
        }

        public void Delete(int authorId)
        {
            _repository.Delete(authorId);
        }

        public bool Exist(string firstName, string lastName, int? excludeId = null)
        {
            return _repository.Exist(firstName, lastName, excludeId);
        }

        public List<Author> GetAll(string sortedBy = "LastName")
        {
            return _repository.GetAll(sortedBy);
        }

        public List<Author> GetAllWithBooks()
        {
            return _repository.GetAllWithBooks();
        }

        public Author? GetById(int authorId, bool tracked = false)
        {
            return _repository.GetById(authorId, tracked);
        }

        public bool HasBooks(int authorId)
        {
            return _repository.HasBooks(authorId);
        }

        public void LoadBooks(Author author)
        {
            _repository.LoadBooks(author);
        }

        public void Save(Author author)
        {
            if (author.Id==0)
            {
                _repository.Add(author);
            }
            else
            {
                _repository.Edit(author);
            }
        }
    }
}
