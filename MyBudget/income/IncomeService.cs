using AutoMapper;
using MyBudget.expense.dtos;
using MyBudget.income.dtos;

namespace MyBudget.income
{
    public interface IIncomeService
    {
        int Create(CreateIncomeDto incomeDto, int logedUser);
        bool Delete(int id);
        IEnumerable<GetIncomeDto> GetByDate(int month, int year);
        bool Update(int id, CreateIncomeDto incomeDto);
    }

    public class IncomeService : IIncomeService
    {
        private readonly MyBudgetDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<IncomeService> _logger;
        public IncomeService(MyBudgetDbContext dbContext, IMapper mapper, ILogger<IncomeService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public IEnumerable<GetIncomeDto> GetByDate(int month, int year)
        {
            var incomes = _dbContext.Incomes.Where(e => e.Date.Month == month && e.Date.Year == year).ToList();

            var incomesDto = _mapper.Map<List<GetIncomeDto>>(incomes);

            return incomesDto;
        }
        public int Create(CreateIncomeDto incomeDto, int logedUser)
        {
            var income = _mapper.Map<Income>(incomeDto);
            income.Date = DateTime.Now;
            income.UserId = logedUser;
            _dbContext.Incomes.Add(income);
            _dbContext.SaveChanges();
            return income.Id;
        }
        public bool Delete(int id)
        {
            _logger.LogTrace($"Income with id: {id} DELETE action invoked");

            var income = _dbContext.Incomes.FirstOrDefault(x => x.Id == id);
            if (income == null) return false;
            _dbContext.Incomes.Remove(income);
            _dbContext.SaveChanges();
            return true;
        }
        public bool Update(int id, CreateIncomeDto incomeDto)
        {
            var income = _dbContext.Incomes.FirstOrDefault(x => x.Id == id);
            if (income == null)
                return false;
            income.Amount = incomeDto.Amount;
            income.Date = DateTime.Now;
            income.Description = incomeDto.Description;
            income.Category = incomeDto.Category;

            _dbContext.SaveChanges();
            return true;
        }
    }
}
