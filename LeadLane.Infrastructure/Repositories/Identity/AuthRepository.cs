using LeadLane.Domain.Identity.Interfaces;
using LeadLane.Domain.Identity.Models;
using LeadLane.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LeadLane.Infrastructure.Repositories.Identity;

public class AuthRepository(LeadLaneDbContext db) : IAuthRepository
{
    public Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == login, cancellationToken);
    }
}
