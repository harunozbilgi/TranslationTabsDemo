using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TranslationTabsDemo.Data.Infrastructure.Context;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = DatabaseConfiguration.Connection;
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        if (connectionString != null)
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}