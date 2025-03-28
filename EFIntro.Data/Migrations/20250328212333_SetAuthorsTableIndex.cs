using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFIntro.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetAuthorsTableIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Authors_FirstName_LastName",
                table: "Authors",
                columns: new[] { "FirstName", "LastName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Authors_FirstName_LastName",
                table: "Authors");
        }
    }
}
