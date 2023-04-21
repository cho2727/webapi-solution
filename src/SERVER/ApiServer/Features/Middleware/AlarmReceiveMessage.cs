using MediatR;
using Smart.Kh2Ems.Infrastructure.Enums;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Models.MidStructs;
using Smart.PowerCUBE.Api;
using Smart.PowerCUBE.Api.DataModels;
using System.Diagnostics;

namespace ApiServer.Features.Middleware;

public class AlarmReceiveMessage
{
    public class Command : IRequest<Response>
    {
        public string EventBoxName { get; set; } = null!;
    }

    public class Response : AlarmResponseModel
    {
    }


    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly IConfiguration _configuration;

        public CommandHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var response = new Response { Result = false };
            try
            {
                int ret = PowerCubeApi.Instance.RecvEventMessageBox(request.EventBoxName, out EventHeadPacket head, out byte[] data);
                if (ret == CubeReturnCode.CubeOK)
                {
                    if (data != null)
                    {
                        if (Enum.IsDefined(typeof(MsgTypeDefine), head.MessageCode))
                        {
                            switch ((MsgTypeDefine)head.MessageCode)
                            {
                                case MsgTypeDefine.EventAlarmData:
                                    {
                                        var alarmData = PowerValueConvert.ByteToStruct<EventBodyData>(data);
                                        response.Data = new AlarmModel
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
                                            DeviceEventTime = alarmData.DeviceEventTime,
                                            EventCreateTime = alarmData.EventCreateTime,
                                            EventMsg = alarmData.EventMsg,
                                        };
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            // 문제있음(MessageCode)
                        }
                    }
                     
                    response.Result = true;
                }
                else
                {
                    response.Error = new Error
                    {
                        Code = ret.ToString(),
                        Message = "Agent 제어 요청 명령 전송 실패"
                    };
                }
            }
            catch (Exception ex)
            {
                response.Error = new Error
                {
                    Code = "02",
                    Message = ex.Message
                };
            }

            return await Task.FromResult(response);
        }
    }
}
