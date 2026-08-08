using LuckyExpenses.Application.Features.Dashboard.Query.GetDashboardSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuckyExpenses.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ISender _sender;
        public DashboardController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Route("Summary")]
        public async Task<IActionResult> GetDashboardSummary([FromQuery] GetDashboardSummaryQuery query, CancellationToken cancellationToken)
        {
            var summary = await _sender.Send(query, cancellationToken);
            return Ok(summary);
        }
    }
}
