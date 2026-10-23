using LeadLane.Domain.Leads.Interfaces;
using LeadLane.Domain.Leads.Models;
using LeadLane.Domain.Models;

namespace LeadLane.Domain.Leads.Services;

public class LeadService(ILeadRepository leadRepository)
{
    public async Task<Result<Lead>> CreateAsync(
        string name,
        string phone,
        string? email,
        string message,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Lead>.Fail("Name is required");
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            return Result<Lead>.Fail("Phone is required");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            return Result<Lead>.Fail("Message is required");
        }

        var lead = new Lead
        {
            Name = name.Trim(),
            Phone = phone.Trim(),
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
            Message = message.Trim(),
            Status = LeadStatus.New,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await leadRepository.AddAsync(lead, cancellationToken);
        await leadRepository.SaveChangesAsync(cancellationToken);

        return Result<Lead>.Ok(lead);
    }

    public async Task<Result<Lead>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var lead = await leadRepository.GetByIdAsync(id, cancellationToken);
        return lead is null
            ? Result<Lead>.Fail("Lead not found")
            : Result<Lead>.Ok(lead);
    }

    public Task<IReadOnlyList<Lead>> GetAllAsync(CancellationToken cancellationToken = default)
        => leadRepository.GetAllAsync(cancellationToken);

    public Task<IReadOnlyList<Lead>> GetNeedingAttentionAsync(
        CancellationToken cancellationToken = default)
        => leadRepository.GetNeedingAttentionAsync(DateTimeOffset.UtcNow, cancellationToken);

    public async Task<IReadOnlyList<Lead>> SearchAsync(
        string? query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return await leadRepository.GetAllAsync(cancellationToken);
        }

        return await leadRepository.SearchAsync(query.Trim(), cancellationToken);
    }

    public async Task<Result<Lead>> UpdateStatusAsync(
        int id,
        LeadStatus status,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(status))
        {
            return Result<Lead>.Fail("Invalid status");
        }

        var lead = await leadRepository.GetByIdAsync(id, cancellationToken);
        if (lead is null)
        {
            return Result<Lead>.Fail("Lead not found");
        }

        lead.Status = status;
        await leadRepository.SaveChangesAsync(cancellationToken);

        return Result<Lead>.Ok(lead);
    }

    public async Task<Result<Lead>> UpdateNotesAsync(
        int id,
        string? notes,
        CancellationToken cancellationToken = default)
    {
        var lead = await leadRepository.GetByIdAsync(id, cancellationToken);
        if (lead is null)
        {
            return Result<Lead>.Fail("Lead not found");
        }

        lead.Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        await leadRepository.SaveChangesAsync(cancellationToken);

        return Result<Lead>.Ok(lead);
    }

    public async Task<Result<Lead>> UpdateNextContactAsync(
        int id,
        DateTimeOffset? nextContactAt,
        CancellationToken cancellationToken = default)
    {
        var lead = await leadRepository.GetByIdAsync(id, cancellationToken);
        if (lead is null)
        {
            return Result<Lead>.Fail("Lead not found");
        }

        lead.NextContactAt = nextContactAt;
        await leadRepository.SaveChangesAsync(cancellationToken);

        return Result<Lead>.Ok(lead);
    }
}
