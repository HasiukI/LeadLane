using LeadLane.Domain.Leads.Models;

namespace LeadLane.Domain.Leads.Interfaces;

public interface ILeadRepository
{
    Task AddAsync(Lead lead, CancellationToken cancellationToken = default);
    Task<Lead?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Lead>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Lead>> GetNeedingAttentionAsync(
        DateTimeOffset asOf,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Lead>> SearchAsync(string query, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
