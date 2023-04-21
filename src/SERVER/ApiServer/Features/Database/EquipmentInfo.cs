using MediatR;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace ApiServer.Features.Database;


public class EquipmentInfo
{
    public class Command : IRequest<Response>
    {
        public long StationId { get; set; }
    }

    public class Response : EquipmentResponseModel
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
                if(request.StationId == 0)
                {
                    response.Datas = _context.ConductingEquipmentViews.Select(x => new EquipmentModel
                    {
                        CeqId = x.CeqId,
                        CeqName = x.CeqName,
                        OfficeCode = x.OfficeCode,
                        StationId = x.StationMrfk,
                        StationName = x.StationName,
                        ObjectType = x.ObjectType,
                        ObjectTypeName = x.ObjectTypeName,
                        DataTypeName = x.DpType,
                        ModelId = x.ModelId,
                        ModelName = x.ModelName,
                        CeqType = x.CeqTypeFk,
                        CircuitNo = x.CircuitNo
                    }).ToList();
                    response.Result = true;
                }
                else
                {
                    var equips = _context.ConductingEquipmentViews
                        .Where(x => x.StationMrfk == request.StationId);
                    if(equips?.Count() > 0)
                    {
                        response.Datas = equips
                        .Select(x => new EquipmentModel
                        {
                            CeqId = x.CeqId,
                            CeqName = x.CeqName,
                            OfficeCode = x.OfficeCode,
                            StationId = x.StationMrfk,
                            StationName = x.StationName,
                            ObjectType = x.ObjectType,
                            ObjectTypeName = x.ObjectTypeName,
                            DataTypeName = x.DpType,
                            ModelId = x.ModelId,
                            ModelName = x.ModelName,
                            CeqType = x.CeqTypeFk,
                            CircuitNo = x.CircuitNo
                        }).ToList();

                        response.Result = true;
                    }
                    else
                    {
                        response.Error = new Error
                        {
                            Code = "01",
                            Message = $"Station ID({request.StationId})에 대한 설비가 존재하지 않습니다."
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