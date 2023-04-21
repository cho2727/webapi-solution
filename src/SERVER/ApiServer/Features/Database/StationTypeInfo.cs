using MediatR;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace ApiServer.Features.Database;


public class StationTypeInfo
{
    public class Command : IRequest<Response>
    {
    }

    public class Response : StationTypeResponseModel
    {
    }


    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly KH2emsServerContext _context;

        public CommandHandler(KH2emsServerContext context)
        {
            this._context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var response = new Response { Result = false };
            try
            {
                response.Datas = _context.StationTypes.Select(x => new StationTypeModel
                {
                    Id = x.StationTypeId, Name = x.Name
                }).ToList();

                response.Result = true;
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