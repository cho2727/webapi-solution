using Kh2RealMaker.Helpers;
using Kh2RealMaker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Smart.Kh2Ems.EF.Core.Contexts;

namespace Kh2RealMaker;


public class SmartServer : IHostedService
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ILogger<SmartServer> _logger;
    private readonly IConfiguration _configuration;
    private readonly SqlServerContext _context;

    public SmartServer(IHostApplicationLifetime applicationLifetime
        , ILogger<SmartServer> logger
        , IConfiguration configuration
        , SqlServerContext context)
    {
        this._applicationLifetime = applicationLifetime;
        this._logger = logger;
        this._configuration = configuration;
        this._context = context;
        applicationLifetime.ApplicationStarted.Register(OnStarted);
        applicationLifetime.ApplicationStopping.Register(OnStopping);
        applicationLifetime.ApplicationStopped.Register(OnStopped);

    }

    private void OnStopped()
    {
        _logger.LogInformation("5. OnStopped");
    }

    private void OnStopping()
    {
        _logger.LogInformation("3. OnStopping");
    }

    private void OnStarted() 
    {
        _logger.LogInformation("2. OnStarted");
        var realmaps = _context.CeqTypes.Where(x => x.Name != null).Select(x => new RealPointMapDataModel
        {
            RealMapID = x.CeqTypeId,
            RealMapName = x.Name,
            RealTypeName = x.EName,
        }).ToList();
        var realpointindexs = _context.CeqPointIndexViews.Where(pt => !string.IsNullOrEmpty(pt.EName)).Select(x => new RealPointIndexDataModel
        {
            RealMapID = x.CeqTypeId,
            PointType = x.PointTypeId,
            DynamicIndex = x.DynamicIndex ?? 0,
            RemoteAddress = string.Empty,
            CircuitNo = 0,
            MidName = x.EName,
        }).ToList();

        var commonIndexs = _context.CommonIndices.Select(x => new CommonIndexDataModel
        {
            IndexId = x.IndexId,
            IndexGroupFk = x.IndexGroupFk,
            Name = x.Name,
            EName = x.EName,
            DataTypeId = x.DataTypeId,            
            Length = x.Length ?? 1
        }).ToList();

        var indexGroups = _context.CommonIndexGroups.Where(x => x.IsCreate == 1).Select(x => new CommonIndexGroupDataModel { 
            IndexGroupId = x.IndexGroupId,
            Name = x.Name,
            EName = x.EName,
            IsCreate = x.IsCreate,
        }).ToList();

        var datatypes = new List<string>();
        foreach (var realmap in realmaps)
        {
            var rpoints = realpointindexs.Where(x => x.RealMapID == realmap.RealMapID).OrderBy(x => x.PointType).ThenBy(x => x.RemoteAddress).ToList();
            if (rpoints.Count > 0)
                datatypes.Add(CubeMiddleHelper.CreateDataType(rpoints, commonIndexs.Where(x => x.IndexGroupFk == 1).ToList(), realmap.RealTypeName));
        }

        foreach(var grp in indexGroups)
        {
            datatypes.Add(CubeMiddleHelper.CreateDataType(null, commonIndexs.Where(x => x.IndexGroupFk == grp.IndexGroupId).ToList(), grp.EName!));
        }

        datatypes.MiddlewareApply();

        _applicationLifetime.StopApplication();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("1. StartAsync");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("4. StopAsync");
        return Task.CompletedTask;
    }
}