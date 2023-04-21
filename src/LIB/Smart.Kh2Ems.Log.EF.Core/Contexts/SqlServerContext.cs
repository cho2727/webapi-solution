using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smart.Kh2Ems.Log.EF.Core.Infrastructure.Reverse;

namespace Smart.Kh2Ems.Log.EF.Core.Contexts;

public class SqlServerLogContext : KH2emsLogContext
{
    public SqlServerLogContext(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string? connectionString = _configuration.GetConnectionString("Log");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
