using LuckyExpenses.Application.Features.PaymentMethods.Query.GetPaymentMethods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuckyExpenses.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly ISender _sender;
        public PaymentMethodsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Route("GetPaymentMethods")]
        public async Task<IActionResult> GetPaymentMethods([FromQuery] GetPaymentMethodsQuery query, CancellationToken cancellationToken)
        {
            var paymentMethods = await _sender.Send(query, cancellationToken);
            return Ok(paymentMethods);
        }
    }
}
