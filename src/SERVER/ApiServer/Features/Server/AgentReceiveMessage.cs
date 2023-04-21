using MediatR;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Models.MidStructs;
using Smart.PowerCUBE.Api;

namespace ApiServer.Features.Server;

public class AgentReceiveMessage
{
    public class Command : IRequest<Response>
    {
        public string ControlBoxName { get; set; } = null!;
    }

    public class Response : AgentCommandResponseModel
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
                int ret = PowerCubeApi.Instance.RecvControlBox(request.ControlBoxName, out CommonHeadPacket head, out byte[] data);
                if (ret == CubeReturnCode.CubeOK)
                {
                    response.Header = new BaseMessageHeader
                    {
                        MsgType = head.MsgType,
                        CubeBoxName = head.CubeBoxName,
                        RequestProcessName = head.RequestProcessName,
                        SendTime = head.SendTime,
                    };

                    if (data != null)
                    {
                        var cmds = PowerValueConvert.ByteToStructArray<AgentCommand>(data);
                        response.Commands = cmds.Select(x => new AgentCommandModel
                        {
                            ProcFullName = x.ProcFullName,
                            ProcArgs = x.ProcArgs,
                            WindowStyle = x.WindowStyle,
                            IsObservation = x.IsObservation,
                            Description = x.Description
                        }).ToList();
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
