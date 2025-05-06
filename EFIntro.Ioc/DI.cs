using EFIntro.Data;
using EFIntro.Data.Interfaces;
using EFIntro.Data.Repositories;
using EFIntro.Service.Interfaces;
using EFIntro.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFIntro.Ioc
{
    public static class DI
    {
        public static IServiceProvider ConfigureDI()
        {
            var services = new ServiceCollection();
            var connectionString = @"Data Source=.; Initial Catalog=LibraryDb; Trusted_Connection=true; TrustServerCertificate=true;";

            services.AddDbContext<LibraryContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();

            return services.BuildServiceProvider();


        }
    }
}
