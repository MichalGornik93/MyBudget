using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudget.income.dtos;
using System.Security.Claims;

namespace MyBudget.income
{
    public interface IIncomeController
    {
        ActionResult Create([FromBody] CreateIncomeDto incomeDto);
        ActionResult Delete([FromRoute] int id);
        ActionResult<IEnumerable<GetIncomeDto>> GetByDate([FromQuery] int month, [FromQuery] int year);
        ActionResult Update([FromBody] CreateIncomeDto incomeDto, [FromRoute] int id);
    }

    [ApiController]
    [Route("incomes")]
    public class IncomeController : ControllerBase, IIncomeController
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService inspectionService)
        {
            _incomeService = inspectionService;
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create([FromBody] CreateIncomeDto incomeDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var income = _incomeService.Create(incomeDto, userId);
            return Created($"{income}", null);
        }
        [HttpGet]
        [Route("byDate")]
        public ActionResult<IEnumerable<GetIncomeDto>> GetByDate([FromQuery] int month, [FromQuery] int year)
        {
            var income = _incomeService.GetByDate(month, year);
            return Ok(income);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete([FromRoute] int id)
        {
            //TODO: Fix
            var isDeleted = _incomeService.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Update([FromBody] CreateIncomeDto incomeDto, [FromRoute] int id)
        {
            //TODO: Fix
            var isUpdated = _incomeService.Update(id, incomeDto);
            if (!isUpdated)
                return NotFound();
            return Ok();
        }
    }
}
