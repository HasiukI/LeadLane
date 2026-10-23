using LeadLane.Domain.Identity.Interfaces;
using LeadLane.Domain.Identity.Models;
using LeadLane.Domain.Models;
using LeadLane.Domain.Services;

namespace LeadLane.Domain.Identity.Services;

public class AuthService(IAuthRepository authRepository, IHashService hashService)
{
    public async Task<Result<User>> LoginAsync(
        string login,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await authRepository.GetByLoginAsync(login, cancellationToken);
        if (user is null || !hashService.VerifyPassword(password, user.PasswordHash))
        {
            return Result<User>.Fail("Invalid login or password");
        }

        return Result<User>.Ok(user);
    }
}
