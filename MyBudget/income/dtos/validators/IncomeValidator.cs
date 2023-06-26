using FluentValidation;

namespace MyBudget.income.dtos.validators
{
    public class IncomeValidator: AbstractValidator<CreateIncomeDto>
    {
        public IncomeValidator()
        {
            RuleFor(dto => dto.Category).IsInEnum().WithMessage("IncomeCategory not exist.");
        }
    }
}
