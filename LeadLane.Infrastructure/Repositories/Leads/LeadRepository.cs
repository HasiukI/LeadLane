using LeadLane.Domain.Leads.Interfaces;
using LeadLane.Domain.Leads.Models;
using LeadLane.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LeadLane.Infrastructure.Repositories.Leads;

public class LeadRepository(LeadLaneDbContext db) : ILeadRepository
{
    public async Task AddAsync(Lead lead, CancellationToken cancellationToken = default)
    {
        await db.Leads.AddAsync(lead, cancellationToken);
    }

    public Task<Lead?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return db.Leads.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Lead>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await db.Leads
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Lead>> GetNeedingAttentionAsync(
        DateTimeOffset asOf,
        CancellationToken cancellationToken = default)
    {
        return await db.Leads
            .AsNoTracking()
            .Where(x =>
                x.NextContactAt != null
                && x.NextContactAt <= asOf
                && x.Status != LeadStatus.Won
                && x.Status != LeadStatus.Lost)
            .OrderBy(x => x.NextContactAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Lead>> SearchAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        var pattern = $"%{query}%";

        return await db.Leads
            .AsNoTracking()
            .Where(x =>
                EF.Functions.ILike(x.Name, pattern)
                || EF.Functions.ILike(x.Phone, pattern)
                || (x.Email != null && EF.Functions.ILike(x.Email, pattern)))
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => db.SaveChangesAsync(cancellationToken);
}
