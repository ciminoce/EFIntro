using EFIntro.Entities;

namespace EFIntro.Data.Interfaces
{
    public interface IAuthorRepository
    {
        void Add(Author author);
        void Delete(int authorId);
        void Edit(Author author);
        bool Exist(string firstName, string lastName, int? excludeId = null);
        List<Author> GetAll(string sortedBy = "LastName");
        Author? GetById(int authorId, bool tracked = false);
        bool HasBooks(int authorId);
        void LoadBooks(Author author);
        List<Author> GetAllWithBooks();
        List<IGrouping<int, Book>> AuthorsGroupIdBooks();
    }
}