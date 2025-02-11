using BudgetPlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly TransactionService _transactionService;

        public HomeController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: /
        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            var totalBalance = await _transactionService.GetTotalBalanceAsync();

            var income = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            var expenses = transactions.Where(t => t.Amount < 0).ToList();

            var categorizedExpenses = expenses.GroupBy(t => t.Category?.Name)
                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) })
                .ToList();

            // Process the categorized expenses
            var categories = categorizedExpenses.Select(c => c.Category).ToList();
            var categoryValues = categorizedExpenses.Select(c => c.Total).ToList();

            // Log the processed data for debugging
            Console.WriteLine("Categories: " + string.Join(", ", categories));
            Console.WriteLine("Values: " + string.Join(", ", categoryValues));

            // Pass the processed data to the view
            return View(new
            {
                TotalBalance = totalBalance,
                Income = income,
                Expenses = expenses.Sum(t => t.Amount),
                Categories = categories,
                CategoryValues = categoryValues
            });
        }
    }
}