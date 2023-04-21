using Kh2Host.Common;
using Kh2Host.Extentions;
using Kh2Host.Features.Fep;
using Kh2Host.Models;
using Kh2Host.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;
using Smart.Kh2Ems.Infrastructure.Enums;
using Smart.Kh2Ems.Infrastructure.Features.Fep;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.PowerCUBE.Api;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kh2Host
{
    public class Worker : BackgroundService
    {
        private readonly IApiLogger _logger;
        private readonly SqlServerContext _context;
        private readonly IConfiguration _configuration;
        private readonly TaskManager _taskManager;
        private readonly CommonDataManager _dbManager;
        private readonly IMediator _mediator;

        public Worker(IApiLogger logger
                        , SqlServerContext context
                        , IConfiguration configuration
                        , TaskManager taskManager
                        , CommonDataManager dbManager
                        , IMediator mediator)
        {
            _logger = logger;
            this._context = context;
            this._configuration = configuration;
            this._taskManager = taskManager;
            this._dbManager = dbManager;
            this._mediator = mediator;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            //var context = new SqlServerContext(_configuration);
            //var datas1 = context.AlarmTypes.ToList();
            _dbManager.DatabaseInit(/*_context*/);
            _dbManager.CalculationInit();
            _dbManager.MiddlewareInit();
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
                if( _dbManager.IsMiddlewareConnected )
                {
                    var calDatas = _dbManager.CalculationDatas.Where(x => x.NextProcTime <= DateTime.Now)?/*.OrderBy(p => p.Period)*/.ToList();
                    if(calDatas != null)
                    {
                        calDatas?.Evaluation();
                        var now = DateTime.Now;
                        var request = new FepCalculationData.Command
                        {
                            Datas = calDatas.Select(x => new FepCalculationData.HostCalculationData
                            {
                                CeqId = x.CeqId,
                                CeqTypeId = x.CeqTypeFk,
                                RealPointName = x.RealPointName,
                                PointType = (RealPointType)x.PointType,
                                MidName = x.EName,
                                Value = x.CalculatedValue,
                                Tlq = 1,
                                CalculatedTM = now
                            }).ToList()
                        };
                        var response = await _mediator.Send(request);
                    }
                }
                await Task.Delay(500);
            }
            //await Task.Delay(1000, stoppingToken);
        }
    }
}