using MediatR;
using Smart.PowerCUBE.Api;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace ApiServer.Features.Middleware;

public class MiddleAlive
{
    public class Command : IRequest<Response>
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
            int retValue = PowerCubeApi.Instance.GetMidLive();
            if (retValue == CubeReturnCode.CubeOK)
            {
                response.Result = true;
            }
            else
            {
                response.Error = new Error { Code = retValue.ToString(), Message = "미들웨어 연결 불가" };
            }
            return await Task.FromResult(response);
        }
    }
}
