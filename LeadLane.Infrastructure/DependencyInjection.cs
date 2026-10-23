using LeadLane.Infrastructure.Persistence;
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

        return services;
    }
}
