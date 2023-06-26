using MyBudget.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MyBudget.expense.dtos;
using System;

namespace MyBudget.expense
{
    public interface IExpenseController
    {
        ActionResult Create([FromBody] CreateExpenseDto expenseDto);
        ActionResult Delete([FromRoute] int id);
        ActionResult<Dictionary<ExpenseCategory, decimal>> GetGroupedByCategory();
        ActionResult<PageResult<GetExpenseDto>> GetAllByDescription([FromQuery] string searchPhase, [FromQuery] int pageNumber, [FromQuery] int pageSize);
        ActionResult Update([FromBody] CreateExpenseDto expenseDto, [FromRoute] int id);
        ActionResult<IEnumerable<GetExpenseDto>> GetByDate([FromQuery] int month, [FromQuery] int year);
    }
    [ApiController]
    [Route("expenses")]
    public class ExpenseController : ControllerBase, IExpenseController
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }
        [HttpGet]
        [Route("byDescription")]
        public ActionResult<PageResult<GetExpenseDto>> GetAllByDescription([FromQuery] string searchPhase, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var expense = _expenseService.GetByDescription(searchPhase, pageNumber, pageSize);
            return Ok(expense);
        }
        [HttpGet]
        [Route("byDate")]
        public ActionResult<IEnumerable<GetExpenseDto>> GetByDate([FromQuery] int month, [FromQuery] int year)
        {
            var expense = _expenseService.GetByDate(month, year);
            return Ok(expense);
        }
        [Route("groupedByCategory")]
        [HttpGet]
        public ActionResult<Dictionary<ExpenseCategory, decimal>> GetGroupedByCategory()
        {
            var expense = _expenseService.GetGroupedByCategory();
            return Ok(expense);
        }
        [HttpGet]
        [Route("expenseCategories")]
        public ActionResult<IEnumerable<ExpenseCategory>> GetExpenseCategories()
        {
            var categories = _expenseService.GetExpenseCategories();
            return Ok(categories);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create([FromBody] CreateExpenseDto expenseDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var expenseId = _expenseService.Create(expenseDto, userId);
            return Created($"/equipment/{expenseId}", null);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete([FromRoute] int id)
        {
            var isDeleted = _expenseService.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Update([FromBody] CreateExpenseDto expenseDto, [FromRoute] int id)
        {
            var isUpdated = _expenseService.Update(id, expenseDto);
            if (!isUpdated)
                return NotFound();
            return Ok();
        }
    }
}
