using Kh2Agent;
using MediatR;
using Serilog;
using Smart.Kh2Ems.Infrastructure.Helpers;
using Smart.Kh2Ems.Infrastructure.Shared.Injectables;
using System.Runtime.InteropServices;



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
           // ���������� ����Ǿ ������� �ʰ��ϴ� �ɼ�
           configuration.Sources.Clear();
           // ȯ�溯�� ��������
           IHostEnvironment env = hostingContext.HostingEnvironment;
           configuration
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
       })
       .ConfigureServices(services =>
       {
           services.AddHostedService<Worker>();

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

