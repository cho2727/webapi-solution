using MediatR;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace ApiServer.Features.Database;


public class ModelDataInfo
{
    public class Command : IRequest<Response>
    {
    }

    public class Response : ModelIndexResponseModel
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
                response.Datas = _context.CeqPointIndexViews.Select(x => new ModelIndexModel
                {
                    ModelId = x.ModelId, DdataId = x.DynamicIndex ?? 0, MidName = x.EName, HighValue = x.LimitMaxValue, LowValue = x.LimitMinValue, PointName = x.Name, Unit = x.UnitName
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