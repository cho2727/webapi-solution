using MediatR;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Models.MidStructs;
using Smart.PowerCUBE.Api;

namespace ApiServer.Features.Server;

public class ComputerStateMessage
{
    public class Command : ComputerStatusSendRequestModel, IRequest<Response>
    {
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

                if (request.Datas != null && request.Datas.Count > 0)
                {
                    var cmds = request.Datas.Select(x => new ComputerStateInfo
                    {
                        ComputerId = x.ComputerId,
                        CpuRate = x.CpuRate,
                        DiskTotal = x.DiskTotal,
                        DiskUsage = x.DiskUsage,
                        MemTotal = x.MemTotal,
                        MemUsage = x.MemUsage,
                        Status = x.Status,
                        ActiveState = x.ActiveState,
                        UpdateTime = x.UpdateTime,
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
