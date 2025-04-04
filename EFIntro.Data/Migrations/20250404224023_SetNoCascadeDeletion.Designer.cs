﻿// <auto-generated />
using System;
using EFIntro.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFIntro.Data.Migrations
{
    [DbContext(typeof(LibraryContext))]
    [Migration("20250404224023_SetNoCascadeDeletion")]
    partial class SetNoCascadeDeletion
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EFIntro.Entities.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "FirstName", "LastName" }, "IX_Authors_FirstName_LastName")
                        .IsUnique();

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("EFIntro.Entities.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("Pages")
                        .HasColumnType("int");

                    b.Property<DateOnly>("PublishDate")
                        .HasColumnType("Date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex(new[] { "Title", "AuthorId" }, "IX_Books_Title_AuthorId")
                        .IsUnique();

                    b.ToTable("Books", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 6,
                            AuthorId = 1,
                            Pages = 400,
                            PublishDate = new DateOnly(1986, 10, 30),
                            Title = "Foundation and Earth"
                        },
                        new
                        {
                            Id = 7,
                            AuthorId = 1,
                            Pages = 400,
                            PublishDate = new DateOnly(1953, 10, 10),
                            Title = "Second Foundation"
                        });
                });

            modelBuilder.Entity("EFIntro.Entities.Book", b =>
                {
                    b.HasOne("EFIntro.Entities.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("EFIntro.Entities.Author", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
