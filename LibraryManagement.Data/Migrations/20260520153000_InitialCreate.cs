using System;
using LibraryManagement.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Data.Migrations;

[DbContext(typeof(LibraryDbContext))]
[Migration("20260520153000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Books",
            columns: table => new
            {
                BookId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Author = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                ISBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                PublishedYear = table.Column<int>(type: "int", nullable: false),
                AvailableCopies = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Books", x => x.BookId);
            });

        migrationBuilder.CreateTable(
            name: "Members",
            columns: table => new
            {
                MemberId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                MembershipDate = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Members", x => x.MemberId);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Books_ISBN",
            table: "Books",
            column: "ISBN",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Members_Email",
            table: "Members",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Members_PhoneNumber",
            table: "Members",
            column: "PhoneNumber",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Books");

        migrationBuilder.DropTable(
            name: "Members");
    }
}
