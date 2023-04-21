using Kh2Historian.Common;
using Kh2Historian.Features.Alarm;
using Kh2Historian.Features.Log;
using Kh2Historian.Models;
using MediatR;
using Smart.Kh2Ems.Infrastructure.Const;
using Smart.Kh2Ems.Infrastructure.Shared.Injectables;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.Kh2Ems.Log.EF.Core.Contexts;
using Smart.Kh2Ems.Log.EF.Core.Infrastructure.Reverse.Models;
using Smart.PowerCUBE.Api;
using Smart.PowerCUBE.Api.DataModels;

namespace Kh2Historian.Tasks;

public class TaskManager : ISingletonService
{
    private readonly IApiLogger _logger;
    private readonly SqlServerLogContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;
    private readonly CommonDataManager _dbManager;
    private readonly CancellationToken _cancellationToken;


    // private readonly IServiceProvider _serviceProvider;
    public TaskManager(IApiLogger logger, SqlServerLogContext context
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
        Task.Run(async () => await AlarmMessageRecvAsync(_cancellationToken, "EVT_SVR_TO_HIST"));
        Task.Run(async () => await LogSaveDataProcAsync(_cancellationToken));
    }
    private async ValueTask LogSaveDataProcAsync(CancellationToken stoppingToken)
    {
        var ceqTypes = _dbManager.LogSettingIndexes!.Select(x => x.CeqTypeId).Distinct().ToList();
        var ceqs = _dbManager.ConductingEquipmentModels!.Where(x => ceqTypes.Any(c => c == x.CeqTypeFk));
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            //bool isExist = _dbManager.LogSaveActions.Any(x => x.NextSaveTime > now);
            var logSaveModel = _dbManager.LogSaveActions.FirstOrDefault(x => x.NextSaveTime <= now);
            if (logSaveModel != null)
            {
                LogSaveData.Command request = new LogSaveData.Command { SaveTime = now };
                var rpNames = ceqs.Select(x => x.DpName);
                if (rpNames != null)
                {
                    // 미들웨어에서 실시간 포인트 목록을 가져옴
                    var rpDatas = PowerCubeApi.Instance.GetRealPointData(rpNames.ToList());
                    if (rpDatas != null)
                    {
                        foreach(var rp in  rpDatas)
                        {
                            var ceq = ceqs.FirstOrDefault(c => c.DpName == rp.RealPointName);
                            if(ceq != null)
                            {
                                var pointIndexes = _dbManager.LogSettingIndexes!.Where(x => x.CeqTypeId == ceq.CeqTypeFk);
                                foreach(var idx in pointIndexes)
                                {
                                    var valp = rp.PointData.FirstOrDefault(pd => pd.DataTypeName == idx.DataTypeName);
                                    double val = double.Parse(valp?.DataValue ?? "0");
                                    var tlqp = rp.PointData.FirstOrDefault(pd => pd.DataTypeName == $"{idx.DataTypeName}_{ConstDefine.RPTagString}");
                                    ushort tlq = ushort.Parse(tlqp?.DataValue ?? "0");
                                    var uptimep = rp.PointData.FirstOrDefault(pd => pd.DataTypeName == $"{idx.DataTypeName}_{ConstDefine.RPUptimeString}");
                                    DateTime uptime = DateTime.Parse(uptimep?.DataValue ?? "1970-01-01");

                                    request.SaveDatas.Add(new LogMinute {
                                        Idx = 0,
                                        MemberOfficeId = ceq.OfficeCode ?? 0,
                                        StationId = ceq.StationMrfk ?? 0,
                                        CeqId = ceq.CeqId,
                                        CeqTypeId = ceq.CeqTypeFk ?? 0,
                                        ModelId = ceq.ModelId ?? 0,
                                        DynamicIndex = idx.DynamicIndex ?? 0,
                                        Value = val,
                                        TagValue = (short)(tlq >> 8),
                                        QualityValue = (short)(tlq & 0x00ff),
                                        DeviceUptime = uptime,
                                        SaveTime = logSaveModel.NextSaveTime
                                    });
                                }
                            }
                        }
                    }
                }
                var response = await _mediator.Send(request);
            }
            await Task.Delay(300);
        }
    }

    private async ValueTask AlarmMessageRecvAsync(CancellationToken stoppingToken, string cubeboxName)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                //{
                //    var request = new LogSaveData.Command { LogType = Smart.Kh2Ems.Infrastructure.Enums.SaveLogType.Minute };
                //    var response = await _mediator.Send(request);
                //}

                if (_dbManager.IsMiddlewareConnected)
                {
                    int retValue = PowerCubeApi.Instance.RecvEventMessageBox(cubeboxName, out EventHeadPacket head, out byte[] data);
                    if (retValue == CubeReturnCode.CubeOK)
                    {
                        var model = new PowerAlarmDataModel
                        {
                            HeadData = new PowerAlarmDataModel.AlarmHeadData
                            {
                                MessageCode = head.MessageCode,
                                SendTime = head.SendTime,
                                RecordCount = head.RecordCount
                            },
                            PacketData = data
                        };

                        var alarmData = PowerValueConvert.ByteToStruct<EventBodyData>(data);
                        var request = new AlarmSaveData.Command
                        {
                            Alarm = new AlarmBodyData
                            {
                                EventId = alarmData.EventId,
                                EventType = alarmData.EventType,
                                MemberOffice = alarmData.MemberOffice,
                                StationId = alarmData.StationId,
                                DeviceId = alarmData.DeviceId,
                                CeqId = alarmData.CeqId,
                                CeqTypeId = alarmData.CeqTypeId,
                                DdataId = alarmData.DdataId,
                                PointType = alarmData.PointType,
                                PointIndex = alarmData.PointIndex,
                                CircuitNo = alarmData.CircuitNo,
                                AlarmPriority = alarmData.AlarmPriority,
                                TagValue = alarmData.TagValue,
                                OldValue = alarmData.OldValue,
                                NewValue = alarmData.NewValue,
                                DeviceEventTime = DateTime.Parse(alarmData.DeviceEventTime),
                                EventCreateTime = DateTime.Parse(alarmData.EventCreateTime),
                                EventMsg = alarmData.EventMsg,
                            }
                        };
                        var response = await _mediator.Send(request);
                        _logger.LogInformation($"[{cubeboxName}->HIST] CODE:0x{model.HeadData.MessageCode.ToString("X")} CNT:{model.HeadData.RecordCount} MSG RCV ");
                    }
                }
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

        }
    }
}

