using BudgetPlanner.Data;
using BudgetPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.Services {
    public class TransactionService {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        // Get all transactions
        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions.Include(t => t.Category).ToListAsync();
        }

        // Get a transaction by ID
        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions.Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Add a transaction (Expense or Income)
        public async Task<bool> AddTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            return await _context.SaveChangesAsync() > 0;
        }

        // Update an existing transaction
        public async Task<bool> UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            return await _context.SaveChangesAsync() > 0;
        }

        // Delete a transaction
        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return false;

            _context.Transactions.Remove(transaction);
            return await _context.SaveChangesAsync() > 0;
        }

        // Get total balance
        public async Task<decimal> GetTotalBalanceAsync()
        {
            var transactions = await _context.Transactions.ToListAsync();
            return transactions.Sum(t => t.Amount); // Income (+) and Expenses (-)
        }
    }
}