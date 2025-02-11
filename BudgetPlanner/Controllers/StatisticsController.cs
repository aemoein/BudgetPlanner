using BudgetPlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetPlanner.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly TransactionService _transactionService;
        private readonly BudgetService _budgetService;

        public StatisticsController(TransactionService transactionService, BudgetService budgetService)
        {
            _transactionService = transactionService;
            _budgetService = budgetService;
        }

        // GET: /Statistics
        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            var totalBalance = await _transactionService.GetTotalBalanceAsync();

            var income = transactions.Where(t => t.Amount > 0).Sum(t => t.Amount);
            var expenses = transactions.Where(t => t.Amount < 0).ToList();

            var categorizedExpenses = expenses.GroupBy(t => t.Category?.Name)
                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount) })
                .ToList();

            return View(new
            {
                TotalBalance = totalBalance,
                Income = income,
                Expenses = expenses.Sum(t => t.Amount),
                CategorizedExpenses = categorizedExpenses
            });
        }
    }
}