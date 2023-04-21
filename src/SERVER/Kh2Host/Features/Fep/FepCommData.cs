using Kh2Host.Common;
using MediatR;
using Smart.Kh2Ems.Infrastructure.Const;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.PowerCUBE.Api;

namespace Kh2Host.Features.Fep;

public class FepCommData
{
    public class HostCommStatus
    {
        public uint RtuID { get; set; }
        public byte Status { get; set; }
        public float CommAvg { get; set; }
        public ushort CommSuccess { get; set; }
        public ushort CommFail { get; set; }
        public ushort CommNoResponse { get; set; }
        public uint CommLastSuccess { get; set; }
    }

    public class Command : IRequest<Response>
    {
        public uint SequenceID { get; set; }
        public List<HostCommStatus>? Datas { get; set; }
    }
    public class Response : BaseResponse
    {

    }

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly IApiLogger _logger;
        private readonly IConfiguration _configuration;
        private readonly CommonDataManager _dbManager;

        public CommandHandler(IApiLogger logger, IConfiguration configuration
                            , CommonDataManager dbManager)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._dbManager = dbManager;
        }
        public Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var response = new Response { Result = false };

            var updateRPDatas = new List<RealPointDataModel>();
            foreach (var dev in request.Datas!)
            {
                var ceqs = _dbManager.ConductingEquipmentModels!.Where(x => x.DeviceFk == dev.RtuID);
                var updateRps = ceqs.Select(x => new RealPointDataModel { RealPointName = x.DpName }).ToList();
                int totalCount = dev.CommSuccess + dev.CommFail;
                foreach (var rp in updateRps)
                {
                    rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = ConstDefine.CommStatePointName, DataValue = dev.Status.ToString() });
                    rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = ConstDefine.CommTotalPointName, DataValue = totalCount.ToString() });
                    rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = ConstDefine.CommSuccPointName, DataValue = dev.CommSuccess.ToString() });
                    rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = ConstDefine.CommFailPointName, DataValue = dev.CommFail.ToString() });
                    rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = ConstDefine.CommNoResponsePointName, DataValue = dev.CommNoResponse.ToString() });
                }
                updateRPDatas.AddRange(updateRps);
            }

            PowerCubeApi.Instance.MultiUpdateRealDataPoint(updateRPDatas);

            return Task.FromResult(response);
        }
    }
}
