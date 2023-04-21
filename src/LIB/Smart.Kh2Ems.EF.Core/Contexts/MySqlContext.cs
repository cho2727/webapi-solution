using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Kh2Ems.EF.Core.Contexts;

public class MySqlContext : KH2emsServerContext
{
    public MySqlContext(IConfiguration configuration) : base(configuration)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string? connectionString = _configuration.GetConnectionString("Server");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
