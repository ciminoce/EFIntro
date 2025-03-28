using EFIntro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFIntro.Data
{
    public class LibraryContext:DbContext
    {
        public DbSet<Author>Authors { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=LibraryDb; Trusted_Connection=true; TrustServerCertificate=true;")
               .EnableSensitiveDataLogging() // Permite ver valores en las consultas
               .LogTo(Console.WriteLine, LogLevel.Information);// Muestra los SQL en la consola;
            ;
        }
    }
}
