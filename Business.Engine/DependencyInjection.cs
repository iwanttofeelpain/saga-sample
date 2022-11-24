using Business.Engine.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Engine;

public static class DependencyInjection
{
    public static void AddEngine(this IServiceCollection services, IConfiguration configuration, string sectionName)
    {
        services.AddDbContext<BusinessContext>(options => options.UseNpgsql(configuration.GetConnectionString("postgresConnection"), b =>
        {
            b.MigrationsAssembly("Business.Engine");
            b.MigrationsHistoryTable("__EFMigrationsHistory", "core");
        }));
    }
}