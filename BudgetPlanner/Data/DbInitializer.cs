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

            // Check if there is existing data in any table
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
                new() { Name = "Entertainment", Type = CategoryType.Expense }
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            // Seed Default User
            var defaultUser = new User { UserName = "admin@example.com", Email = "admin@example.com", FullName = "Admin User" };
            var password = "Admin@123";

            if (await userManager.FindByEmailAsync(defaultUser.Email) == null)
            {
                await userManager.CreateAsync(defaultUser, password);
            }

            await context.SaveChangesAsync();
        }
    }
}