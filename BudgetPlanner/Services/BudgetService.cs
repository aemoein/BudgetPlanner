using BudgetPlanner.Data;
using BudgetPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Services {
    public class BudgetService {
        private readonly AppDbContext _context;

        public BudgetService(AppDbContext context)
        {
            _context = context;
        }

        // Get all budgets
        public async Task<List<Budget>> GetAllBudgetsAsync()
        {
            return await _context.Budgets.Include(b => b.Category).ToListAsync();
        }

        // Get a budget by ID
        public async Task<Budget?> GetBudgetByIdAsync(int id)
        {
            return await _context.Budgets.Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        // Add a new budget
        public async Task<bool> AddBudgetAsync(Budget budget)
        {
            _context.Budgets.Add(budget);
            return await _context.SaveChangesAsync() > 0;
        }

        // Update an existing budget
        public async Task<bool> UpdateBudgetAsync(Budget budget)
        {
            _context.Budgets.Update(budget);
            return await _context.SaveChangesAsync() > 0;
        }

        // Delete a budget
        public async Task<bool> DeleteBudgetAsync(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null) return false;

            _context.Budgets.Remove(budget);
            return await _context.SaveChangesAsync() > 0;
        }

        // Check if spending exceeds budget for a category
        public async Task<bool> IsOverBudgetAsync(int categoryId)
        {
            var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.CategoryId == categoryId);
            if (budget == null) return false; // No budget set for category

            var totalSpent = await _context.Transactions
                .Where(t => t.CategoryId == categoryId && t.Amount < 0) // Only expenses
                .SumAsync(t => t.Amount);

            return Math.Abs(totalSpent) > budget.Amount; // Check if over budget
        }
    }
}