using LeadLane.Domain.Identity.Models;
using LeadLane.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LeadLane.Infrastructure.Persistence;

public static class DbSeeder
{
    public const string DemoLogin = "admin";
    public const string DemoPassword = "admin123";

    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LeadLaneDbContext>();
        var hashService = scope.ServiceProvider.GetRequiredService<IHashService>();

        await db.Database.MigrateAsync(cancellationToken);

        if (await db.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var hash = hashService.HashPassword(DemoPassword);
        if (!hash.IsSuccess || hash.Data is null)
        {
            throw new InvalidOperationException("Failed to hash demo user password.");
        }

        db.Users.Add(new User
        {
            Login = DemoLogin,
            PasswordHash = hash.Data
        });

        await db.SaveChangesAsync(cancellationToken);
    }
}
