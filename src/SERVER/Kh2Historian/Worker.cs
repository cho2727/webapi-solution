using Kh2Historian.Common;
using Kh2Historian.Tasks;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.Kh2Ems.Log.EF.Core.Contexts;
using Smart.PowerCUBE.Api;

namespace Kh2Historian;

public class Worker : BackgroundService
{
    private readonly IApiLogger _logger;
    private readonly IConfiguration _configuration;
    private readonly Smart.Kh2Ems.EF.Core.Contexts.SqlServerContext _logContext;
    private readonly TaskManager _taskManager;
    private readonly CommonDataManager _dbManager;

    public Worker(IApiLogger logger
                    , IConfiguration configuration
                    , Smart.Kh2Ems.EF.Core.Contexts.SqlServerContext logContext
                    , TaskManager taskManager
                    , CommonDataManager dbManager)
    {
        _logger = logger;
        this._configuration = configuration;
        this._logContext = logContext;
        this._taskManager = taskManager;
        this._dbManager = dbManager;
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        //var context = new SqlServerContext(_configuration);
        //var datas1 = context.AlarmTypes.ToList();
        _dbManager.DatabaseInit(/*_context*/);
        _taskManager.StartTask();

        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(Worker)} is stopping.");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _dbManager.IsMiddlewareConnected = PowerCubeApi.Instance.IsConnect();
            Console.WriteLine($"MIDDLE CONNECT = {_dbManager.IsMiddlewareConnected}");
            _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}