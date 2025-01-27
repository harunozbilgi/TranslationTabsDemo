namespace TranslationTabsDemo.Data.Infrastructure.Context;

public abstract class DatabaseConfiguration
{
    public static string? Connection
    {
        get
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env}.json", true)
                .AddEnvironmentVariables()
                .Build();

            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}