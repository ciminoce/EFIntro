using EFIntro.Entities;

namespace EFIntro.Service.Interfaces
{
    public interface IBookService
    {
        void Save(Book book);
        void Delete(int bookId);
        bool Exist(string bookTitle, int bookAuthorId, int? excludeId = null);
        List<Book> GetAll(string sortedBy = "Title", bool include = false);
        Book? GetById(int bookId, bool include = false, bool tracked = false);
        List<IGrouping<int, Book>> BooksGroupByAuthor();
    }
}
