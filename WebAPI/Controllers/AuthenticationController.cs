using LuckyExpenses.Application.Features.Authentication.Command.Login;
using LuckyExpenses.Application.Features.Authentication.Command.Register;
using LuckyExpenses.Application.Features.Authentication.Query.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuckyExpenses.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _sender;
        public AuthenticationController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        {
            return Ok(await _sender.Send(command, cancellationToken));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
        {
            await _sender.Send(command, cancellationToken);
            return Ok();
        }

        [HttpGet]
        [Route("GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            return Ok(await _sender.Send(new GetCurrentUserQuery(), cancellationToken));
        }
    }
}
