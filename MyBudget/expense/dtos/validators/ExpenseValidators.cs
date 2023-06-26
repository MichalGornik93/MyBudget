using FluentValidation;

namespace MyBudget.expense.dtos.validators
{
    public class ExpenseValidator : AbstractValidator<CreateExpenseDto>
    {
        public ExpenseValidator()
        {
            RuleFor(dto => dto.Category).IsInEnum().WithMessage("ExpenseCategory not exist.");
        }
    }
}
