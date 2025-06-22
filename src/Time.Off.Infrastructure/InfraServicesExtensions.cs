using Microsoft.Extensions.DependencyInjection;
using Time.Off.Domain.Repositories;
using Time.Off.Infrastructure.Contexts;
using Time.Off.Infrastructure.Repositories;

namespace Time.Off.Infrastructure;

public static class InfraServicesExtensions
{
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
    {
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddSingleton<TimeOffDataBaseContext>();

        return services;
    }
}