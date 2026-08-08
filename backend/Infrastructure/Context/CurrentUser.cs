using LuckyExpenses.Application.Context;
using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Repositories;
using System.Security.Claims;

namespace LuckyExpenses.Infrastructure.Context
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        private User? _cachedUser;

        public CurrentUser(
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        private ClaimsPrincipal? User =>
            _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;

        private long? _cachedUserId;
        public long? UserId
        {
            get
            {
                if (_cachedUserId.HasValue)
                    return _cachedUserId;

                if (!IsAuthenticated)
                    return null;

                var idClaim =
                    User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User?.FindFirst("sub")?.Value;

                if (string.IsNullOrWhiteSpace(idClaim))
                    return null;

                if (!long.TryParse(idClaim, out var userId))
                    return null;

                _cachedUserId = userId;
                return userId;
            }
        }

        public string? Email =>
            User?.FindFirst(ClaimTypes.Email)?.Value;

        public string? Role =>
            User?.FindFirst(ClaimTypes.Role)?.Value;

        public async Task<User?> GetUserAsync(CancellationToken cancellationToken = default)
        {
            if (!IsAuthenticated || UserId == null)
                return null;

            if (_cachedUser != null)
                return _cachedUser;

            _cachedUser = await _userRepository.GetByIdAsync(UserId.Value, cancellationToken);
            return _cachedUser;
        }
    }
}
