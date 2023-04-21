using Kh2Historian.Common;
using Kh2Historian.Features.Shard;
using MediatR;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.Infrastructure.Enums;
using Smart.Kh2Ems.Infrastructure.Helpers;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.Kh2Ems.Log.EF.Core.Contexts;
using Smart.Kh2Ems.Log.EF.Core.Infrastructure.Reverse.Models;
using System.Runtime.ConstrainedExecution;

namespace Kh2Historian.Features.Log;

public class LogSaveData
{
    public class Command : IRequest<Response>
    {
        public DateTime SaveTime { get; set; }
        public List<LogMinute> SaveDatas { get; set; } = new List<LogMinute>();
    }
    public class Response : BaseResponse
    {

    }

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly IApiLogger _logger;
        private readonly SqlServerLogContext _context;
        private readonly IConfiguration _configuration;
        private readonly CommonDataManager _dbManager;

        public CommandHandler(IApiLogger logger
                            , SqlServerLogContext context
                            , IConfiguration configuration
                            , CommonDataManager dbManager)
        {
            this._logger = logger;
            this._context = context;
            this._configuration = configuration;
            this._dbManager = dbManager;
        }
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var response = new Response { Result = false };
            var logSaveModels = _dbManager.LogSaveActions.Where(x => x.NextSaveTime <= request.SaveTime);
            var now = DateTime.Now;
            foreach(var model in logSaveModels)
            {
                model.NextSaveTime = DateTimeHelper.GetNextDateTime(now, model.SLogType, min: 5);
                switch (model.SLogType)
                {
                    case SaveLogType.Minute:
                        {
                            _context.LogMinutes.AddRange(request.SaveDatas);
                        }
                        break;
                    case SaveLogType.Hour:
                        {
                            _context.LogHours.AddRange(request.SaveDatas.Select(x => new LogHour {
                                MemberOfficeId = x.MemberOfficeId,
                                StationId = x.StationId,
                                CeqId = x.CeqId,
                                CeqTypeId = x.CeqTypeId,
                                ModelId = x.ModelId,
                                DynamicIndex = x.DynamicIndex,
                                Value = x.Value,
                                TagValue = x.TagValue,
                                QualityValue = x.QualityValue,
                                DeviceUptime = x.DeviceUptime,
                                SaveTime = x.SaveTime
                            }));
                        }
                        break;
                    case SaveLogType.Day:
                        {
                            _context.LogDays.AddRange(request.SaveDatas.Select(x => new LogDay
                            {
                                MemberOfficeId = x.MemberOfficeId,
                                StationId = x.StationId,
                                CeqId = x.CeqId,
                                CeqTypeId = x.CeqTypeId,
                                ModelId = x.ModelId,
                                DynamicIndex = x.DynamicIndex,
                                Value = x.Value,
                                TagValue = x.TagValue,
                                QualityValue = x.QualityValue,
                                DeviceUptime = x.DeviceUptime,
                                SaveTime = x.SaveTime
                            }));
                        }
                        break;
                    case SaveLogType.Month:
                        {
                            _context.LogMonths.AddRange(request.SaveDatas.Select(x => new LogMonth
                            {
                                MemberOfficeId = x.MemberOfficeId,
                                StationId = x.StationId,
                                CeqId = x.CeqId,
                                CeqTypeId = x.CeqTypeId,
                                ModelId = x.ModelId,
                                DynamicIndex = x.DynamicIndex,
                                Value = x.Value,
                                TagValue = x.TagValue,
                                QualityValue = x.QualityValue,
                                DeviceUptime = x.DeviceUptime,
                                SaveTime = x.SaveTime
                            }));
                        }
                        break;
                }

            }
             await _context.SaveChangesAsync();
            return await Task.FromResult(response);
        }
    }
}
