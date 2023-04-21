using Kh2Host.Common;
using Kh2Host.Enums;
using Kh2Host.Extentions;
using Kh2Host.Models;
using MediatR;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;
using Smart.Kh2Ems.Infrastructure.Const;
using Smart.Kh2Ems.Infrastructure.Enums;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.PowerCUBE.Api;
using Smart.PowerCUBE.Api.DataModels;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Claims;
using static Azure.Core.HttpHeader;

namespace Smart.Kh2Ems.Infrastructure.Features.Fep;

public class FepCalculationData
{
    public class HostCalculationData
    {
        public long CeqId { get; set; }
        public int CeqTypeId { get; set; }
        public string RealPointName { get; set; } = string.Empty;
        public RealPointType PointType { get; set; }
        public string MidName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public byte Tlq { get; set; }
        public DateTime CalculatedTM { get; set; }
    }

    public class Command : IRequest<Response>
    {
        public List<HostCalculationData>? Datas { get; set; }
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
            if(request.Datas != null)
            {
                var ceqs = _dbManager.ConductingEquipmentModels!.ToList();
                var rpNames = request.Datas.Select(x => x.RealPointName).Distinct().ToList();
                var rpDatas = PowerCubeApi.Instance.GetRealPointData(rpNames);
                if(rpDatas != null)
                {
                    var alarms = new List<EventBodyData>();
                    var dbupdatas = new List<UpdatePointValue>();

                    var updateRps = rpDatas.Select(x => new RealPointProcModel { Equipment = ceqs.FirstOrDefault(c => c.DpName == x.RealPointName), RealPointName = x.RealPointName, OldRealData = x, NewRealData = new RealPointDataModel { RealPointName = x.RealPointName } }).ToList();
                    var now = DateTime.Now;

                    foreach (var caldat in request.Datas)
                    {
                        var pIndex = _dbManager.PointIndexModels!.FirstOrDefault(x => x.CeqTypeId == caldat.CeqTypeId && x.PointTypeId == (int)caldat.PointType && x.EName == caldat.MidName);
                        if (pIndex == null)
                            continue;
                        var rp = updateRps.FirstOrDefault(x => x.RealPointName == caldat.RealPointName);
                        if (rp != null)
                        {
                            if (caldat.PointType == RealPointType.CALBI)
                            {
                                var stateValues = _dbManager.stateValueModels?.Where(x => x.StateGroupID == pIndex.StateGroupId).ToList();
                                var pValue = new BinaryProcModel { PointIndex = pIndex, NewValue = byte.Parse(caldat.Value), NewTlq = caldat.Tlq, RecvTm = (uint)PowerValueConvert.ConvertToUnixTimestamp(caldat.CalculatedTM), RecvMilli = (ushort)caldat.CalculatedTM.Millisecond };
                                rp.BinaryRealPointProc(pValue, stateValues, alarms, dbupdatas, 0);
                            }
                            else if (caldat.PointType == RealPointType.CALAI)
                            {
                                var pValue = new AnalogProcModel { PointIndex = pIndex, NewValue = float.Parse(caldat.Value), NewTlq = caldat.Tlq, RecvTm = (uint)PowerValueConvert.ConvertToUnixTimestamp(caldat.CalculatedTM), RecvMilli = (ushort)caldat.CalculatedTM.Millisecond };
                                rp.AnalogRealPointProc(pValue, alarms, dbupdatas, 0);
                            }
                        }
                    }

                    var updateRPDatas = updateRps.Select(x => x.NewRealData).ToList();
                    PowerCubeApi.Instance.MultiUpdateRealDataPoint(updateRPDatas);
                    foreach (var alarm in alarms)
                    {
                        RealPointExtention.AlarmDataSend(alarm, string.Empty);
                    }
                    _dbManager.UpdateCeqDatas(dbupdatas, now);
                }
            }

            return Task.FromResult(response);
        }

    }
}
