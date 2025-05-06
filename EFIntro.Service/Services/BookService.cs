using EFIntro.Data.Interfaces;
using EFIntro.Entities;
using EFIntro.Service.Interfaces;

namespace EFIntro.Service.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository = null!;

        public BookService(IBookRepository repository)
        {
            _bookRepository = repository;
        }

        public List<IGrouping<int, Book>> BooksGroupByAuthor()
        {
            return _bookRepository.BooksGroupByAuthor();
        }

        public void Delete(int bookId)
        {
            _bookRepository.Delete(bookId);
        }

        public bool Exist(string bookTitle, int bookAuthorId, int? excludeId = null)
        {
            return _bookRepository.Exist(bookTitle, bookAuthorId, excludeId);
        }

        public List<Book> GetAll(string sortedBy = "Title", bool include=false)
        {
            return _bookRepository.GetAll(sortedBy,true);
        }

        public Book? GetById(int bookId, bool include = false, bool tracked = false)
        {
            return _bookRepository.GetById(bookId, include, tracked);
        }

        public void Save(Book book)
        {
            if (book.Id == 0)
            {
                _bookRepository.Add(book);
            }
            else
            {
                _bookRepository.Update(book);
            }
        }
    }
}
