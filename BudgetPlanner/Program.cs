using BudgetPlanner.Data;
using BudgetPlanner.Models;
using BudgetPlanner.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure MySQL Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configure Identity Framework
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add MVC controllers & views
builder.Services.AddControllersWithViews();

// Register Services
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<BudgetService>();

var app = builder.Build();

// Seed database if empty
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    try
    {
        await DbInitializer.SeedData(context, userManager);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database seeding: {ex.Message}");
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Configure middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();