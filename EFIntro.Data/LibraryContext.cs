using EFIntro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFIntro.Data
{
    public class LibraryContext:DbContext
    {
        public LibraryContext(DbContextOptions options) : base(options)
        {
        }

        protected LibraryContext()
        {
        }

        public DbSet<Author>Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=LibraryDb; Trusted_Connection=true; TrustServerCertificate=true;");
            //   //.EnableSensitiveDataLogging() // Permite ver valores en las consultas
            //   //.LogTo(Console.WriteLine, LogLevel.Information)
            //   //.UseLazyLoadingProxies(false);//habilita Lazy Loading
        }
        //FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Book>(entity =>
            //{
            //    entity.ToTable("Books");
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Title).HasMaxLength(300)
            //        .IsRequired();
            //    entity.Property(e => e.Pages).IsRequired();
            //    entity.Property(e => e.PublishDate).HasColumnType("Date")
            //        .IsRequired();
            //    entity.HasIndex(e => new { e.Title, e.AuthorId }, "IX_Books_Title_AuthorId").IsUnique();
            //    entity.HasOne(e => e.Author).WithMany(e => e.Books).HasForeignKey(e => e.AuthorId)
            //        .OnDelete(DeleteBehavior.ClientNoAction);

            //    var bookList=new List<Book>
            //    {
            //        new Book{Id=6,Title="Foundation and Earth", PublishDate=DateOnly.FromDateTime(new DateTime(1986,10,30)), Pages=400, AuthorId=1},
            //        new Book{Id=7,Title="Second Foundation", PublishDate=DateOnly.FromDateTime(new DateTime(1953,10,10)), Pages=400, AuthorId=1}
            //    };
            //    entity.HasData(bookList);
            //});

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryContext).Assembly);

        }
    }
}
