using MediatR;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Models.MidStructs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kh2Agent.Commands;

public class AgentCommand
{
    public class Command : AgentCommandResponseModel, IRequest<Response>
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
               // 바로 데이터 처리를 수행함.
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
