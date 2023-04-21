using Kh2Host;
using MediatR;
using Serilog;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.Infrastructure.Helpers;
using Smart.Kh2Ems.Infrastructure.Shared.Injectables;

var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();

Environment.SetEnvironmentVariable("BASEDIR", AppDomain.CurrentDomain.BaseDirectory);
Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configurationBuilder)
    .CreateBootstrapLogger();

try
{
    IHost host = Host.CreateDefaultBuilder(args)
       .UseWindowsService()
       .ConfigureAppConfiguration((hostingContext, configuration) =>
       {
           // 설정파일이 변경되어도 적용되지 않게하는 옵션
           configuration.Sources.Clear();
           // 환경변수 가져오기
           IHostEnvironment env = hostingContext.HostingEnvironment;
           configuration
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
       })
       .ConfigureServices(services =>
       {
           services.AddHostedService<Worker>();
           services.AddSingleton(typeof(SqlServerContext));
           services.Scan(scan => scan
               .FromAssemblies(AssemblyHelper.GetAllAssemblies(SearchOption.AllDirectories))
               .AddClasses(classes => classes.AssignableTo<ITransientService>())
               .AsSelfWithInterfaces()
               .WithTransientLifetime()
               .AddClasses(classes => classes.AssignableTo<IScopedService>())
               .AsSelfWithInterfaces()
               .WithScopedLifetime()
               .AddClasses(classes => classes.AssignableTo<ISingletonService>())
               .AsSelfWithInterfaces()
               .WithSingletonLifetime());

           services.AddMediatR(AssemblyHelper.GetAllAssemblies().ToArray());
           //var connectionString = configurationBuilder.GetConnectionString("Server") ?? string.Empty;
           //services.AddDataAccessServices(connectionString);

           //services.AddDbContext<ServerDBContext, SqlServerContext>
           //    (options => options.UseSqlServer(connectionString,
           //                x => x.EnableRetryOnFailure(maxRetryCount: maxRetryCount)));
           //services.AddDataAccessServices(connectionString);
       })
       .UseSerilog((context, configuration) =>
               configuration.ReadFrom.Configuration(context.Configuration))
       .Build();

    host.Run();

}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
        throw;
    if (type.Equals("HostAbortedException", StringComparison.Ordinal))
        throw;
    Log.Fatal(ex, $"{type} Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

