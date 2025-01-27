using TranslationTabsDemo.Data.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureDatabaseServices(builder.Configuration)
    .RegisterApplicationServices();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts();
}
await app.MigrateDatabaseAsync();
await app.SeedDatabaseAsync();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();