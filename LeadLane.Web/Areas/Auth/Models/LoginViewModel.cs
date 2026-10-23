using System.ComponentModel.DataAnnotations;

namespace LeadLane.Web.Areas.Auth.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Login")]
    public string Login { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
}
