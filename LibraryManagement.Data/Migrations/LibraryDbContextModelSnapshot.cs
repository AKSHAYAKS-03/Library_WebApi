using System;
using LibraryManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LibraryManagement.Data.Migrations;

[DbContext(typeof(LibraryDbContext))]
public partial class LibraryDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.8")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("LibraryManagement.Models.Book", b =>
        {
            b.Property<int>("BookId")
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            b.Property<int>("AvailableCopies")
                .HasColumnType("int");

            b.Property<string>("Author")
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            b.Property<string>("ISBN")
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            b.Property<int>("PublishedYear")
                .HasColumnType("int");

            b.Property<string>("Title")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            b.HasKey("BookId");

            b.HasIndex("ISBN")
                .IsUnique();

            b.ToTable("Books");
        });

        modelBuilder.Entity("LibraryManagement.Models.Member", b =>
        {
            b.Property<int>("MemberId")
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            b.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("nvarchar(255)");

            b.Property<string>("FullName")
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            b.Property<DateTime>("MembershipDate")
                .HasColumnType("datetime2");

            b.Property<string>("PhoneNumber")
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnType("nvarchar(10)");

            b.HasKey("MemberId");

            b.HasIndex("Email")
                .IsUnique();

            b.HasIndex("PhoneNumber")
                .IsUnique();

            b.ToTable("Members");
        });
    }
}
