using Microsoft.Extensions.DependencyInjection;

namespace LeadLane.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // EF, repositories, seed — register here as layers grow
        return services;
    }
}
