using AutoMapper;
using MyBudget.expense;
using MyBudget.expense.dtos;
using MyBudget.income;
using MyBudget.income.dtos;
using MyBudget.user;
using MyBudget.user.dtos;

namespace MyBudget
{
    public class MyBudgetMappingProfile: Profile
    {
        public MyBudgetMappingProfile()
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<CreateIncomeDto, Income>();
            CreateMap<Income, GetIncomeDto>();
            CreateMap<CreateExpenseDto, Expense>();
            CreateMap<Expense, GetExpenseDto>();
        }
    }
}
