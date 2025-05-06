using EFIntro.Data.Interfaces;
using EFIntro.Entities;
using EFIntro.Service.Interfaces;

namespace EFIntro.Service.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository = null!;

        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public void Delete(int bookId)
        {
            _repository.Delete(bookId);
        }

        public bool Exist(string bookTitle, int bookAuthorId, int? excludeId = null)
        {
            return _repository.Exist(bookTitle, bookAuthorId, excludeId);
        }

        public List<Book> GetAll(string sortedBy = "Title")
        {
            return _repository.GetAll(sortedBy);
        }

        public Book? GetById(int bookId, bool include = false, bool tracked = false)
        {
            return _repository.GetById(bookId, include, tracked);
        }

        public void Save(Book book)
        {
            if (book.Id == 0)
            {
                _repository.Add(book);
            }
            else
            {
                _repository.Update(book);
            }
        }
    }
}
