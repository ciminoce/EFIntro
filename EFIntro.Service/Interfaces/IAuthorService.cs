using EFIntro.Entities;

namespace EFIntro.Service.Interfaces
{
    public interface IAuthorService
    {
        void Save(Author author);
        void Delete(int authorId);
        bool Exist(string firstName, string lastName, int? excludeId = null);
        List<Author> GetAll(string sortedBy = "LastName");
        Author? GetById(int authorId, bool tracked = false);
        Author? GetByName(string firstName, string lastName);
        bool HasBooks(int authorId);
        void LoadBooks(Author author);
        List<Author> GetAllWithBooks();
        List<IGrouping<int, Book>> AuthorsWithBooksCount();
    }
}
