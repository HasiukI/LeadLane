using LeadLane.Domain.Models;

namespace LeadLane.Domain.Services;

public interface IHashService
{
    Result<string> HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
