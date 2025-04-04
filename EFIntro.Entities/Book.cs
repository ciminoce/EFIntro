namespace EFIntro.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateOnly PublishDate { get; set; }
        public int Pages { get; set; }

        public int AuthorId { get; set; }
        public Author? Author { get; set; }//Navigation Property
        public override string ToString()
        {
            return $"Title:{Title} - Pages:{Pages}";
        }
    }
}
