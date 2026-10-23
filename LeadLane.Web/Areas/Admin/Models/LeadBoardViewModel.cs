using LeadLane.Domain.Leads.Models;

namespace LeadLane.Web.Areas.Admin.Models;

public class LeadBoardViewModel
{
    public string? Query { get; set; }
    public IReadOnlyList<Lead> AttentionLeads { get; init; } = [];
    public IReadOnlyDictionary<LeadStatus, IReadOnlyList<Lead>> Columns { get; init; }
        = new Dictionary<LeadStatus, IReadOnlyList<Lead>>();
}
