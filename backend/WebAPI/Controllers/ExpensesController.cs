using LuckyExpenses.Application.Features.Expenses.Command.CreateExpense;
using LuckyExpenses.Application.Features.Expenses.Command.DeleteExpense;
using LuckyExpenses.Application.Features.Expenses.Command.UpdateExpense;
using LuckyExpenses.Application.Features.Expenses.Query.GetExpenseById;
using LuckyExpenses.Application.Features.Expenses.Query.GetExpenses;
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

        [HttpGet]
        [Route("GetExpenses")]
        public async Task<IActionResult> GetExpenses([FromQuery] GetExpensesQuery query, CancellationToken cancellationToken)
        {
            var expenses = await _sender.Send(query, cancellationToken);
            return Ok(expenses);
        }

        [HttpGet]
        [Route("GetExpenseById")]
        public async Task<IActionResult> GetExpenseById([FromQuery] GetExpenseByIdQuery query, CancellationToken cancellationToken)
        {
            var expense = await _sender.Send(query, cancellationToken);
            return Ok(expense);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateExpense([FromBody] UpdateExpenseCommand command, CancellationToken cancellationToken)
        {
            var expense = await _sender.Send(command, cancellationToken);
            return Ok(expense);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteExpense([FromBody] DeleteExpenseCommand command, CancellationToken cancellationToken)
        {
            await _sender.Send(command, cancellationToken);
            return Ok();
        }
    }
}
