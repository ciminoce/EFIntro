using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFIntro.Data.Migrations
{
    /// <inheritdoc />
    public partial class AnotherPopulationToBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "Pages", "PublishDate", "Title" },
                values: new object[,]
                {
                    { 6, 1, 400, new DateOnly(1986, 10, 30), "Foundation and Earth" },
                    { 7, 1, 400, new DateOnly(1953, 10, 10), "Second Foundation" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
