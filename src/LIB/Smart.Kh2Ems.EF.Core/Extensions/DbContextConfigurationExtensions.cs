
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;

namespace Smart.Kh2Ems.EF.Core.Extensions;

public static class DbContextConfigurationExtensions
{
    public static void AddServerAccessServices(this IServiceCollection self, string connectionString, string dbProvider = "SqlServer", int maxRetryCount = 3)
    {
        if (dbProvider.Equals("MySql", StringComparison.OrdinalIgnoreCase))
        {
            self.AddDbContext<KH2emsServerContext, MySqlContext>
                (options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                            x => x.EnableRetryOnFailure(maxRetryCount: maxRetryCount)));
        }
        else
        {
            self.AddDbContext<KH2emsServerContext, SqlServerContext>
                (options => options.UseSqlServer(connectionString,
                            x => x.EnableRetryOnFailure(maxRetryCount: maxRetryCount)));
        }
    }
}