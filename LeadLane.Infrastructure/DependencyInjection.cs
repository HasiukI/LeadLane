using LeadLane.Domain.Identity.Interfaces;
using LeadLane.Domain.Identity.Services;
using LeadLane.Domain.Leads.Interfaces;
using LeadLane.Domain.Leads.Services;
using LeadLane.Domain.Services;
using LeadLane.Infrastructure.Persistence;
using LeadLane.Infrastructure.Repositories.Identity;
using LeadLane.Infrastructure.Repositories.Leads;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeadLane.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is not configured. " +
                "Copy appsettings.Development.local.json.example to appsettings.Development.local.json.");

        services.AddDbContext<LeadLaneDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddSingleton<IHashService, HashService>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<AuthService>();

        services.AddScoped<ILeadRepository, LeadRepository>();
        services.AddScoped<LeadService>();

        return services;
    }
}
