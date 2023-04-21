using MediatR;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Models.MidStructs;
using Smart.PowerCUBE.Api;

namespace ApiServer.Features.Server;

public class AgentSendMessage
{
    public class Command : AgentCommandRequestModel, IRequest<Response>
    {
        // public List<AgentCommandModel>? Commands { get; set; }
    }

    public class Response : BaseResponse
    {
        //public AgentCommandRequestModel ModelData { get; set; }
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
                CommonHeadPacket headpacket = new CommonHeadPacket
                {
                    MsgType = request.Header.MsgType,
                    CubeBoxName = request.Header.CubeBoxName,
                    RequestProcessName = request.Header.RequestProcessName,
                    SendTime = request.Header.SendTime
                };

                if (request.Commands != null && request.Commands.Count > 0)
                {
                    var cmds = request.Commands.Select(x => new AgentCommand
                    {
                        ProcFullName = x.ProcFullName,
                        ProcArgs = x.ProcArgs,
                        WindowStyle = x.WindowStyle,
                        IsObservation = x.IsObservation,
                        Description = x.Description
                    }).ToArray();

                    byte[] data = PowerValueConvert.StructArrayToByte(cmds);
                    int ret = PowerCubeApi.Instance.SendControlBox(request.Header.CubeBoxName, headpacket, data);
                    if (ret == CubeReturnCode.CubeOK)
                    {
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
