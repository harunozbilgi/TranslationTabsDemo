using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TranslationTabsDemo.Data.Application.Services;
using TranslationTabsDemo.Data.Domain.Entities;
using TranslationTabsDemo.Data.Domain.Repositories;
using TranslationTabsDemo.Data.Infrastructure.Context;
using TranslationTabsDemo.Data.Infrastructure.Interceptors;
using TranslationTabsDemo.Data.Infrastructure.Repositories;
using TranslationTabsDemo.Data.Infrastructure.Services;

namespace TranslationTabsDemo.Data.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection ConfigureDatabaseServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new Exception("Connection string not found");

        services.AddDbContextPool<ApplicationDbContext>((sp, builder) =>
        {
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    options =>
                    {
                        options.EnableStringComparisonTranslations();
                        options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    })
                .AddInterceptors(sp.GetRequiredService<LanguageInterceptor>());
        });

        services.AddSingleton<LanguageInterceptor>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<ILanguageService, LanguageService>();
        return services;
    }

    
    public static async Task MigrateDatabaseAsync(this IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();
        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            await applicationDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration error: {ex.Message}");
            throw;
        }
    }

    public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();
        var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (!await applicationDbContext.Languages.OrderBy(x => x.Id).AnyAsync())
        {
            var languages = new List<Language>()
            {
                new()
                {
                    Name = "İngilizce",
                    Code = "en",
                    IsDefault = false
                },
                new()
                {
                    Name = "Rusça",
                    Code = "ru",
                    IsDefault = false
                },
            };

            await applicationDbContext.Languages.AddRangeAsync(languages);

            await applicationDbContext.SaveChangesAsync();
        }
    }
}