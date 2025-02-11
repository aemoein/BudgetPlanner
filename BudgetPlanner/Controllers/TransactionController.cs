using BudgetPlanner.Models;
using BudgetPlanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetPlanner.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        // GET: /Transaction
        public async Task<IActionResult> Index()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return View(transactions);
        }

        // GET: /Transaction/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Transaction/Create
        [HttpPost]
        public async Task<IActionResult> Create(Transaction transaction)
        {
            if (!ModelState.IsValid) return View(transaction);

            await _transactionService.AddTransactionAsync(transaction);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Transaction/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null) return NotFound();
            return View(transaction);
        }

        // POST: /Transaction/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Transaction transaction)
        {
            if (!ModelState.IsValid) return View(transaction);

            await _transactionService.UpdateTransactionAsync(transaction);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Transaction/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _transactionService.DeleteTransactionAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}