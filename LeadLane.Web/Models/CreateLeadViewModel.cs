using System.ComponentModel.DataAnnotations;

namespace LeadLane.Web.Models;

public class CreateLeadViewModel
{
    [Required]
    [Display(Name = "Name")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Phone")]
    [StringLength(50)]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress]
    [Display(Name = "Email")]
    [StringLength(256)]
    public string? Email { get; set; }

    [Required]
    [Display(Name = "Message")]
    [StringLength(4000)]
    public string Message { get; set; } = string.Empty;
}
