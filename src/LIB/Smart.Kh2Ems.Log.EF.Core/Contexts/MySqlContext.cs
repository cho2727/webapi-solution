using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smart.Kh2Ems.Log.EF.Core.Infrastructure.Reverse;

namespace Smart.Kh2Ems.Log.EF.Core.Contexts;

public class MySqlLogContext : KH2emsLogContext
{
    public MySqlLogContext(IConfiguration configuration) : base(configuration)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string? connectionString = _configuration.GetConnectionString("Log");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
