using BudgetPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Data
{
    public static class DbInitializer
    {
        public static async Task SeedData(AppDbContext context, UserManager<User> userManager)
        {
            // Apply any pending migrations
            await context.Database.MigrateAsync();

            // Check if data already exists
            if (context.Users.Any() || context.Categories.Any() || context.Transactions.Any() || context.Budgets.Any())
            {
                return; // Database already seeded
            }

            // Seed Categories
            var categories = new List<Category>
            {
                new() { Name = "Salary", Type = CategoryType.Income },
                new() { Name = "Groceries", Type = CategoryType.Expense },
                new() { Name = "Rent", Type = CategoryType.Expense },
                new() { Name = "Entertainment", Type = CategoryType.Expense },
                new() { Name = "Transport", Type = CategoryType.Expense }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Seed Default User
            var defaultUser = new User { UserName = "admin@example.com", Email = "admin@example.com", FullName = "Admin User" };
            var password = "Admin@123";

            if (await userManager.FindByEmailAsync(defaultUser.Email) == null)
            {
                await userManager.CreateAsync(defaultUser, password);
                defaultUser = await userManager.FindByEmailAsync(defaultUser.Email); // Get user after creation
            }

            if (defaultUser == null)
            {
                return; // Fail-safe in case user creation failed
            }

            // Seed Transactions
            var transactions = new List<Transaction>
            {
                new() { UserId = defaultUser.Id, Description = "Monthly Salary", Amount = 2500.00m, Date = DateTime.UtcNow.AddDays(-10), CategoryId = categories.First(c => c.Name == "Salary").Id },
                new() { UserId = defaultUser.Id, Description = "Grocery Shopping", Amount = -200.00m, Date = DateTime.UtcNow.AddDays(-8), CategoryId = categories.First(c => c.Name == "Groceries").Id },
                new() { UserId = defaultUser.Id, Description = "Rent Payment", Amount = -900.00m, Date = DateTime.UtcNow.AddDays(-5), CategoryId = categories.First(c => c.Name == "Rent").Id },
                new() { UserId = defaultUser.Id, Description = "Movie Night", Amount = -50.00m, Date = DateTime.UtcNow.AddDays(-3), CategoryId = categories.First(c => c.Name == "Entertainment").Id },
                new() { UserId = defaultUser.Id, Description = "Taxi Ride", Amount = -30.00m, Date = DateTime.UtcNow.AddDays(-1), CategoryId = categories.First(c => c.Name == "Transport").Id }
            };

            await context.Transactions.AddRangeAsync(transactions);
            await context.SaveChangesAsync();

            // Seed Budgets (Optional: Assign per category)
            var budgets = new List<Budget>
            {
                new() { Amount = 1000.00m, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30), CategoryId = categories.First(c => c.Name == "Groceries").Id },
                new() { Amount = 1200.00m, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30), CategoryId = categories.First(c => c.Name == "Rent").Id },
                new() { Amount = 200.00m, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30), CategoryId = categories.First(c => c.Name == "Entertainment").Id },
                new() { Amount = 150.00m, StartDate = DateTime.UtcNow.AddDays(-30), EndDate = DateTime.UtcNow.AddDays(30), CategoryId = categories.First(c => c.Name == "Transport").Id }
            };

            await context.Budgets.AddRangeAsync(budgets);
            await context.SaveChangesAsync();
        }
    }
}