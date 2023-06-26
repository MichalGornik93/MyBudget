using Microsoft.EntityFrameworkCore;
using MyBudget.user;
using MyBudget.expense;
using MyBudget.income;

namespace MyBudget
{
    public class MyBudgetDbContext : DbContext
    {
        public MyBudgetDbContext(DbContextOptions<MyBudgetDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Role> Roles { get; set; }

    }

}
