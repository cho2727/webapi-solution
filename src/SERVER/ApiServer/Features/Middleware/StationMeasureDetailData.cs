using ApiServer.Extentions;
using MediatR;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.PowerCUBE.Api;

namespace ApiServer.Features.Middleware;

public class StationMeasureDetailData
{
    public class Command : IRequest<Response>
    {
        public long StationId { get; set; }
    }

    public class Response : Dictionary<string, object>
    {
        // public Dictionary<string, object>? Datas { get; set; }
    }



    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly IConfiguration _configuration;
        private readonly KH2emsServerContext _context;

        public CommandHandler(IConfiguration configuration, KH2emsServerContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var response = new Response();
            try
            {
                //ConductingEquipmentView
                var equips = request.StationId == 0 ? _context.ConductingEquipmentViews.ToList() : _context.ConductingEquipmentViews.Where(x => x.StationMrfk == request.StationId).ToList();
                var rps = PowerCubeApi.Instance.GetRealPointDataWithTypeName(equips.Select(x => x.DpName).ToList());
                if (rps != null)
                {
                    response["Result"] = true;
                    response["Error"] = null!;
                    List<Dictionary<string, object>>? equipModels = null;
                    foreach (var equip in equips)
                    {
                        if (response.ContainsKey(equip.DpType))
                        {
                            equipModels = response[equip.DpType] as List<Dictionary<string, object>>;
                        }
                        else
                        {
                            equipModels = new List<Dictionary<string, object>>();
                        }

                        var rp = rps.FirstOrDefault(x => x.RealPointName == equip.DpName);
                        if (rp != null)
                        {
                            var datas = new Dictionary<string, object>();
                            //datas["CeqId"] = equip.CeqId;
                            //datas["ObjectTypeId"] = equip.ObjectType;
                            //datas["ModelId"] = equip.ModelId!;
                            //datas["ModelName"] = equip.ModelName;

                            datas.SetMeasureData(rp);
                            equipModels!.Add(datas);
                        }

                        if (equipModels!.Count > 0)
                            response[equip.DpType] = equipModels;
                    }
                }
                else
                {
                    response["Result"] = false;
                    response["Error"] = new Error { Code = "02", Message = $"Station ID:{request.StationId} is not exist" };
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
