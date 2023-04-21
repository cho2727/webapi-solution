using Kh2Historian.Common;
using Kh2Historian.Features.Shard;
using MediatR;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.PowerCUBE.Api.DataModels;

namespace Kh2Historian.Features.Alarm;

public class AlarmSaveData
{
    public class Command : IRequest<Response>
    {
        public AlarmBodyData Alarm { get; set; } = null!;
    }
    public class Response : BaseResponse
    {

    }

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly IApiLogger _logger;
        private readonly SqlServerContext _context;
        private readonly IConfiguration _configuration;
        private readonly CommonDataManager _dbManager;

        public CommandHandler(IApiLogger logger
                            , SqlServerContext context
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
            _context.LogEvents.Add(new Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models.LogEvent
            {
                EventId = request.Alarm.EventId,
                EventTypeFk = request.Alarm.EventType,
                MemberOfficeFk = request.Alarm.MemberOffice,
                StationFk = request.Alarm.StationId,
                DeviceCeqFk = request.Alarm.DeviceId,
                CeqFk = request.Alarm.CeqId,
                CeqTypeFk = request.Alarm.CeqTypeId,
                DdataIndexFk = request.Alarm.DdataId,
                PointTypeFk = request.Alarm.PointType,
                PointIndexFk = request.Alarm.PointIndex,
                CircuitNo = request.Alarm.CircuitNo,
                AlarmPriority = request.Alarm.AlarmPriority,
                TagValue = request.Alarm.TagValue >> 8,
                QualityValue = request.Alarm.TagValue & 0x00ff,
                OldValue = request.Alarm.OldValue,
                NewValue = request.Alarm.NewValue,
                DeviceEventTime = request.Alarm.DeviceEventTime,
                EventCreateTime = request.Alarm.EventCreateTime,
                EventMsg = request.Alarm.EventMsg,

            });
            await _context.SaveChangesAsync();
            return await Task.FromResult(response);
        }
    }
}
