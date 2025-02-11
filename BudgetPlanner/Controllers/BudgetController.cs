using BudgetPlanner.Models;
using BudgetPlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetPlanner.Controllers
{
    public class BudgetController : Controller
    {
        private readonly BudgetService _budgetService;

        public BudgetController(BudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        // GET: /Budget
        public async Task<IActionResult> Index()
        {
            var budgets = await _budgetService.GetAllBudgetsAsync();
            return View(budgets);
        }

        // GET: /Budget/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Budget/Create
        [HttpPost]
        public async Task<IActionResult> Create(Budget budget)
        {
            if (!ModelState.IsValid) return View(budget);

            await _budgetService.AddBudgetAsync(budget);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Budget/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id);
            if (budget == null) return NotFound();
            return View(budget);
        }

        // POST: /Budget/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Budget budget)
        {
            if (!ModelState.IsValid) return View(budget);

            await _budgetService.UpdateBudgetAsync(budget);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Budget/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _budgetService.DeleteBudgetAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Budget/CheckOverBudget/{categoryId}
        public async Task<IActionResult> CheckOverBudget(int categoryId)
        {
            var isOverBudget = await _budgetService.IsOverBudgetAsync(categoryId);
            return Json(new { overBudget = isOverBudget });
        }
    }
}