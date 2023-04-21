using MediatR;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace ApiServer.Features.Database;


public class EquipmentDetailInfo
{
    public class Command : IRequest<Response>
    {
        public long StationId { get; set; }
    }

    public class Response : Dictionary<string, object>
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
            var response = new Response();
            try
            {

                var equips = request.StationId > 0 ? _context.ConductingEquipmentViews.Where(x => x.StationMrfk == request.StationId).ToList()
                    : _context.ConductingEquipmentViews.ToList();

                var modelIndexes = _context.ModelIndices.Join(
                    _context.ModelItemIndices,
                    x => x.ItemFk,
                    y => y.IndexId,
                    (x, y) => new { x.ModelFk, x.ItemFk, x.Seq, x.Value, y.EName });

                if(equips.Count > 0)
                {
                    response["Result"] = true;
                    response["Error"] = null!; 
                    var stationModels = new List<Dictionary<string, object>>();
                    response["Datas"] = stationModels;
                    foreach (var stn in equips)
                    {
                        var datas = new Dictionary<string, object>();
                        datas["CeqId"] = stn.CeqId;
                        datas["CeqName"] = stn.CeqName;
                        datas["OfficeCode"] = stn.OfficeCode ?? 0;
                        datas["StationId"] = stn.StationMrfk ?? 0;
                        datas["StationName"] = stn.StationName;
                        datas["ObjectType"] = stn.ObjectType;
                        datas["ObjectTypeName"] = stn.ObjectTypeName;
                        datas["DataTypeName"] = stn.DpType;
                        datas["ModelId"] = stn.ModelId ?? 0;
                        datas["ModelName"] = stn.ModelName;
                        datas["CeqType"] = stn.CeqTypeFk ?? 0;
                        datas["CircuitNo"] = stn.CircuitNo ?? 0;

                        var items = modelIndexes.Where(x => x.ModelFk == stn.ModelId).ToList();
                        foreach (var item in items)
                        {
                            datas[item.EName!] = item.Value!;
                        }

                        stationModels.Add(datas);
                    }
                }
                else
                {
                    response["Result"] = false;
                    response["Error"] = new Error { Code = "02", Message = $"스테이션 정보가 존재하지 않습니다." };
                }

            }
            catch (Exception ex)
            {
                response["Result"] = false;
                response["Error"] = new Error { Code = "02", Message = ex.Message };
            }

            return await Task.FromResult(response);
        }
    }
}