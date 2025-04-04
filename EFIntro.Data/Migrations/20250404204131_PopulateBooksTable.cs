using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFIntro.Data.Migrations
{
    /// <inheritdoc />
    public partial class PopulateBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Books (Title, PublishDate, Pages, AuthorId) VALUES ('Foundation', '1951-01-01', 255, 1)");
            migrationBuilder.Sql("INSERT INTO Books (Title, PublishDate, Pages, AuthorId) VALUES ('2001: A Space Odyssey', '1968-01-01', 320, 2)");
            migrationBuilder.Sql("INSERT INTO Books (Title, PublishDate, Pages, AuthorId) VALUES ('Fahrenheit 451', '1953-01-01', 272, 3)");
            migrationBuilder.Sql("INSERT INTO Books (Title, PublishDate, Pages, AuthorId) VALUES ('Do Androids Dream of Electric Sheep?', '1968-01-01', 240, 4)");
            migrationBuilder.Sql("INSERT INTO Books (Title, PublishDate, Pages, AuthorId) VALUES ('The Left Hand of Darkness', '1969-01-01', 300, 5)");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Books WHERE Id IN (1,2,3,4,5)");
        }
    }
}
