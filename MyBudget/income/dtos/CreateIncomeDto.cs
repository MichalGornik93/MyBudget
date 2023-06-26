using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MyBudget.income;

namespace MyBudget.income.dtos
{
    public class CreateIncomeDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public decimal Amount { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public IncomeCategory Category { get; set; }
        public string Description { get; set; }
    }

}
