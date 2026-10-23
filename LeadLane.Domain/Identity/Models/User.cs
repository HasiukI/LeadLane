namespace LeadLane.Domain.Identity.Models;

public class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string PasswordHash { get; set; }
}