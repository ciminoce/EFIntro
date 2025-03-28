using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFIntro.Data.Migrations
{
    /// <inheritdoc />
    public partial class PopulateAuthorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values:new object[,]
                    {
                        {1, "Isaac", "Asimov" },
                        {2, "Arthur","Clarke" },
                        {3, "Ray", "Bradbury" },
                        {4, "Philip","Dick" },
                        {5, "Ursula","Le Guin" }
                    }
                );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5 });

        }
    }
}
