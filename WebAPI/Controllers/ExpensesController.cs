using LuckyExpenses.Application.Features.Expenses.Command.CreateExpense;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuckyExpenses.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly ISender _sender;
        public ExpensesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseCommand command, CancellationToken cancellationToken)
        {
            var expense = await _sender.Send(command, cancellationToken);
            return Created($"/api/v1/expenses/{expense.Id}", expense);
        }
    }
}
