using EFIntro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFIntro.Data.EntityTypeConfigurations
{
    public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> entity)
        {
            entity.ToTable("Authors");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(e => new { e.FirstName, e.LastName }, "IX_Authors_FirstName_LastName")
                .IsUnique();

            entity.HasMany(e => e.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
