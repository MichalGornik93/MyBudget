using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyBudget.expense.dtos;
using MyBudget.models;

namespace MyBudget.expense
{
    public interface IExpenseService
    {
        int Create(CreateExpenseDto expenseDto, int logedUser);
        bool Delete(int id);
        Dictionary<ExpenseCategory, decimal> GetGroupedByCategory();
        PageResult<GetExpenseDto> GetByDescription(string searchPhase, int pageNumber, int pageSize);
        IEnumerable<ExpenseCategory> GetExpenseCategories();
        bool Update(int id, CreateExpenseDto expenseDto);
        IEnumerable<GetExpenseDto> GetByDate(int month, int year);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly MyBudgetDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ExpenseService> _logger;

        public ExpenseService(MyBudgetDbContext dbContext, IMapper mapper, ILogger<ExpenseService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public PageResult<GetExpenseDto> GetByDescription(string searchPhase, int pageNumber, int pageSize)
        {
            var baseQuery = _dbContext.Expenses
                .Where(r => searchPhase == null || r.Description.ToLower().Contains(searchPhase.ToLower()));

            var expenses = baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

            var exspensesDto = _mapper.Map<List<GetExpenseDto>>(expenses);

            var result = new PageResult<GetExpenseDto>(exspensesDto, baseQuery.Count(), pageSize, pageNumber);

            return result;
        }
        public IEnumerable<GetExpenseDto> GetByDate(int month, int year)
        {
            var expenses = _dbContext.Expenses.Where(e => e.Date.Month == month && e.Date.Year == year).ToList();

            var expensesDto = _mapper.Map<List<GetExpenseDto>>(expenses);

            return expensesDto;
        }
        public Dictionary<ExpenseCategory, decimal> GetGroupedByCategory()
        {
            Dictionary<ExpenseCategory, decimal> categoryPercentages = new Dictionary<ExpenseCategory, decimal>();

            var expenses = _dbContext.Expenses.ToList();
            decimal totalAmount = expenses.Sum(e => e.Amount);

            foreach (ExpenseCategory category in Enum.GetValues(typeof(ExpenseCategory)))
            {
                decimal categoryAmount = expenses.Where(e => e.Category == category).Sum(e => e.Amount);
                decimal categoryPercentage = 0;

                if (totalAmount > 0)
                {
                    categoryPercentage = (categoryAmount / totalAmount) * 100;
                }
                categoryPercentage = Math.Round(categoryPercentage, 2);
                categoryPercentages.Add(category, categoryPercentage);
            }

            return categoryPercentages;
        }
        public IEnumerable<ExpenseCategory> GetExpenseCategories()
        {
            return Enum.GetValues(typeof(ExpenseCategory)).Cast<ExpenseCategory>();
        }
        public int Create(CreateExpenseDto expenseDto, int logedUser)
        {
            var expense = _mapper.Map<Expense>(expenseDto);
            expense.Date = DateTime.Now;
            expense.UserId = logedUser;
            _dbContext.Expenses.Add(expense);
            _dbContext.SaveChanges();
            return expense.Id;
        }
        public bool Delete(int id)
        {
            _logger.LogTrace($"Expense with id: {id} DELETE action invoked");

            var expense = _dbContext.Expenses.FirstOrDefault(x => x.Id == id);
            if (expense == null) return false;
            _dbContext.Expenses.Remove(expense);
            _dbContext.SaveChanges();
            return true;
        }
        public bool Update(int id, CreateExpenseDto expenseDto)
        {
            var expense = _dbContext.Expenses.FirstOrDefault(x => x.Id == id);
            if (expense == null)
                return false;
            expense.Amount = expenseDto.Amount;
            expense.Date = DateTime.Now;
            expense.Description = expenseDto.Description;
            expense.Category = expenseDto.Category;

            _dbContext.SaveChanges();
            return true;
        }
    }
}
