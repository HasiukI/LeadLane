using LeadLane.Domain.Identity.Models;

namespace LeadLane.Domain.Identity.Interfaces;

public interface IAuthRepository
{
    Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
}
