namespace ShipmentTracker.App.Infrastructure.Dependencies;
public static class DependencyInjection
{
    public static async Task<IServiceCollection> AddInfrastructure(this IServiceCollection services, SQLLiteSettings settings)
    {
        services.AddDbContext<ShipmentTrackerAppDBContext>(options =>
            options.UseSqlite(settings.ConnectionString));
        return services;
    }
}
