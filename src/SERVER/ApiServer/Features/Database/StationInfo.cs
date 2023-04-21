using MediatR;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace ApiServer.Features.Database;


public class StationInfo
{
    public class Command : IRequest<Response>
    {
    }

    public class Response : StationResponseModel
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
                response.Datas = _context.StationViews.Select(x => new StationModel
                {
                    OfficeId = x.MemberOfficeId,
                    OfficeName = x.OfficeName,
                    StationId = x.StationId,
                    StationName = x.StationName,
                    StationTypeId = x.StationTypeId,
                    StationTypeName = x.StationTypeName,
                    ModelId = x.ModelId,
                    ModelName = x.ModelName,
                    StationAdder = x.StationAdder
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