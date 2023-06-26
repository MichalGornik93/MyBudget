using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBudget.expense.dtos
{
    public class CreateExpenseDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public ExpenseCategory Category { get; set; }
        public string Description { get; set; }
    }
}
