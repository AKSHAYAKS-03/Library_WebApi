using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Member> Members => Set<Member>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books");

            entity.HasKey(x => x.BookId);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Author)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.ISBN)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.PublishedYear)
                .IsRequired();

            entity.Property(x => x.AvailableCopies)
                .IsRequired();

            entity.HasIndex(x => x.ISBN).IsUnique();
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Members");

            entity.HasKey(x => x.MemberId);

            entity.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(x => x.MembershipDate)
                .IsRequired();

            entity.HasIndex(x => x.Email).IsUnique();
            entity.HasIndex(x => x.PhoneNumber).IsUnique();
        });
    }
}
