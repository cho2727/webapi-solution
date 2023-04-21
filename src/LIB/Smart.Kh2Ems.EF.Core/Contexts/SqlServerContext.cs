using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;

namespace Smart.Kh2Ems.EF.Core.Contexts;

public class SqlServerContext : KH2emsServerContext
{
    public SqlServerContext(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string? connectionString = _configuration.GetConnectionString("Server");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
