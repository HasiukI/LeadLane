using LeadLane.Domain.Identity.Models;
using LeadLane.Domain.Leads.Models;
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

        if (!await db.Users.AnyAsync(cancellationToken))
        {
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

        if (await db.Leads.AnyAsync(cancellationToken))
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;

        db.Leads.AddRange(
            new Lead
            {
                Name = "Olena Koval",
                Phone = "+380501112233",
                Email = "olena@example.com",
                Message = "Please call back about website redesign.",
                Status = LeadStatus.New,
                CreatedAt = now.AddHours(-5)
            },
            new Lead
            {
                Name = "Andriy Shevchenko",
                Phone = "+380671234567",
                Email = null,
                Message = "Interested in monthly support.",
                Status = LeadStatus.InProgress,
                Notes = "Asked for price list.",
                NextContactAt = now.AddDays(1),
                CreatedAt = now.AddDays(-2)
            },
            new Lead
            {
                Name = "Maria Petrenko",
                Phone = "+380931112233",
                Email = "maria@example.com",
                Message = "Callback after 18:00.",
                Status = LeadStatus.Agreed,
                Notes = "Meeting agreed for Friday.",
                NextContactAt = now.AddHours(-2),
                CreatedAt = now.AddDays(-3)
            },
            new Lead
            {
                Name = "Taras Bondar",
                Phone = "+380441112233",
                Message = "Closed deal — landing page.",
                Status = LeadStatus.Won,
                CreatedAt = now.AddDays(-10)
            },
            new Lead
            {
                Name = "Iryna Melnyk",
                Phone = "+380991223344",
                Message = "Too expensive for now.",
                Status = LeadStatus.Lost,
                Notes = "May return next quarter.",
                CreatedAt = now.AddDays(-7)
            });

        await db.SaveChangesAsync(cancellationToken);
    }
}
