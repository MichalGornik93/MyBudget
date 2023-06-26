
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBudget.expense.dtos
{
    public class GetExpenseDto
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public ExpenseCategory Category { get; set; }
        public string Description { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
