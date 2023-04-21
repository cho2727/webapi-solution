using MediatR;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Models.MidStructs;
using Smart.PowerCUBE.Api;

namespace ApiServer.Features.Middleware;

public class ProgramStateMessage
{ 
    public class Command : BaseAsyncMessage, IRequest<Response>
    {
        public List<ProgramStatusModel>? Commands { get; set; }
    }

    public class Response : BaseResponse
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
                CommonHeadPacket headpacket = new CommonHeadPacket
                {
                    MsgType = request.Header.MsgType,
                    CubeBoxName = request.Header.CubeBoxName,
                    RequestProcessName = request.Header.RequestProcessName,
                    SendTime = request.Header.SendTime
                };

                if (request.Commands != null && request.Commands.Count > 0)
                {
                    var cmds = request.Commands.Select(x => new ProgramStateInfo
                    {
                        ProgramId = x.ProgramId,
                        Status = x.Status,
                        StartTime = x.StartTime ?? "",
                        EndTime = x.EndTime ?? "",
                        UpdateTime = x.UpdateTime
                    }).ToArray();

                    byte[] data = PowerValueConvert.StructArrayToByte(cmds);
                    int ret = PowerCubeApi.Instance.SendMessageBox(request.Header.CubeBoxName, headpacket, data);
                    if (ret == CubeReturnCode.CubeOK)
                    {
                        response.Result = true;
                    }
                    else
                    {
                        response.Error = new Error
                        {
                            Code = ret.ToString(),
                            Message = $"cubebox:{request.Header.CubeBoxName} 메시지 전송 실패"
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
