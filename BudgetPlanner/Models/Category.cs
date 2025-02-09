using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetPlanner.Models {
    public class Category {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public CategoryType Type { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }

    public enum CategoryType {
        Income,
        Expense
    }
}