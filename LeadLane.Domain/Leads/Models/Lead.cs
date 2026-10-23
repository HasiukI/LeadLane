namespace LeadLane.Domain.Leads.Models;

public class Lead
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Message { get; set; } = string.Empty;
    public LeadStatus Status { get; set; } = LeadStatus.New;
    public string? Notes { get; set; }
    public DateTimeOffset? NextContactAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
