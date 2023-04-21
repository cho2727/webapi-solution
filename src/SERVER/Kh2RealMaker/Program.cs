using Kh2RealMaker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Smart.Kh2Ems.EF.Core.Contexts;

IHost host = Host.CreateDefaultBuilder(args)
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
    .ConfigureServices((hostcontext, services) =>
    {
        services.AddHostedService<SmartServer>();
        services.AddSingleton(typeof(SqlServerContext));

    })
    //.UseSerilog((context, configuration) =>
    //                configuration.ReadFrom.Configuration(context.Configuration))
    .Build();

host.Run();