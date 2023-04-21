using Kh2Host.Common;
using Kh2Host.Features.Fep;
using MediatR;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.Infrastructure.Shared.Injectables;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;

namespace Kh2Host.Tasks;

public class TaskManager : ISingletonService
{
    private readonly IApiLogger _logger;
    private readonly SqlServerContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;
    private readonly CommonDataManager _dbManager;
    private readonly CancellationToken _cancellationToken;

    public List<FepTask> FepTasks { get; set; } = new List<FepTask>();

    // private readonly IServiceProvider _serviceProvider;
    public TaskManager(IApiLogger logger, SqlServerContext context
                        , IConfiguration configuration
                        , IHostApplicationLifetime applicationLifetime
                        , IMediator mediator
                        , CommonDataManager dbManager)
    {
        _logger = logger;
        _context = context;
        _configuration = configuration;
        _mediator = mediator;
        _dbManager = dbManager;
        _cancellationToken = applicationLifetime.ApplicationStopping;
    }

    public void StartTask()
    {
        var feps = _context.ProgramInfos.Where(x => x.ProgramTypeFk == 3);
        foreach (var fep in feps)
        {
            int cubeNumber = fep.ProgramNo ?? 0;
            if (cubeNumber > 0)
            {
                var task = new FepTask(_logger, _dbManager, _mediator, $"MSG_FEP_P{cubeNumber.ToString("D3")}");
                FepTasks.Add(task);
                task.StartTask(_cancellationToken);
            }
        }

        var baseTask = new FepTask(_logger, _dbManager, _mediator, "MSG_SERVER_FEP");
        FepTasks.Add(baseTask);
        baseTask.StartTask(_cancellationToken);
    }
}

