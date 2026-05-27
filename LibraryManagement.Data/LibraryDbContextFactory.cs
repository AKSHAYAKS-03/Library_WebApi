using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibraryManagement.Data;

public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
    public LibraryDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("LIBRARY_DB_CONNECTION")
            ?? "Server=localhost,1433;Database=LibraryDb;User Id=sa;Password=YourStrong@Pass123;TrustServerCertificate=True;Encrypt=False";

        var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new LibraryDbContext(optionsBuilder.Options);
    }
}
