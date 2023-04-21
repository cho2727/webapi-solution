using Kh2LogSender;
using Smart.Kh2Ems.Infrastructure.Helpers;
using Smart.Kh2Ems.Infrastructure.Shared.Injectables;

IHost host = Host.CreateDefaultBuilder(args)
    
    .ConfigureServices(services =>
    {
        services.AddHostedService<LogSendWorker>();
        // services.AddHostedService<LogReceiveWorker>();
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
    })
    .Build();

host.Run();
