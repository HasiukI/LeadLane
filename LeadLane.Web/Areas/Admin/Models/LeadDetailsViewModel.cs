using System.ComponentModel.DataAnnotations;
using LeadLane.Domain.Leads.Models;

namespace LeadLane.Web.Areas.Admin.Models;

public class LeadDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }

    [Display(Name = "Status")]
    public LeadStatus Status { get; set; }

    [Display(Name = "Notes")]
    [StringLength(8000)]
    public string? Notes { get; set; }

    [Display(Name = "Next contact")]
    public DateTimeOffset? NextContactAt { get; set; }

    public static LeadDetailsViewModel FromLead(Lead lead) => new()
    {
        Id = lead.Id,
        Name = lead.Name,
        Phone = lead.Phone,
        Email = lead.Email,
        Message = lead.Message,
        CreatedAt = lead.CreatedAt,
        Status = lead.Status,
        Notes = lead.Notes,
        NextContactAt = lead.NextContactAt
    };
}
