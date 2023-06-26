using FluentValidation;

namespace MyBudget.user.dtos.validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(MyBudgetDbContext dbContext)
        {
            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password).MinimumLength(5);

            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();

            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

            RuleFor(x => x.EmailAddress)
                .Custom((value, context) =>
                {
                    var emailInUser = dbContext.Users.Any(u => u.EmailAddress == value);
                    if (emailInUser)
                    {
                        context.AddFailure("Email", "Email is taken");
                    }
                });
        }
    }
}
