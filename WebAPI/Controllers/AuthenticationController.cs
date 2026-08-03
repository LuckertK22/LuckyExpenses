using LuckyExpenses.Application.Features.Authentication.Login;
using LuckyExpenses.Application.Features.Authentication.Register;
using LuckyExpenses.Application.Features.Users.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LuckyExpenses.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthenticationController(ISender sender) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var token = await sender.Send(new LoginQuery(request), cancellationToken);
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            await sender.Send(new RegisterCommand(request), cancellationToken);
            return Ok();
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (nameIdentifier is null || !long.TryParse(nameIdentifier, out var userId))
                throw new UnauthorizedAccessException("Token inválido");

            return Ok(await sender.Send(new GetCurrentUserQuery(userId), cancellationToken));
        }
    }
}