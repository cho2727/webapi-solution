using Kh2Host.Common;
using Kh2Host.Extentions;
using Kh2Host.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;
using Smart.Kh2Ems.Infrastructure.Const;
using Smart.Kh2Ems.Infrastructure.Enums;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.PowerCUBE.Api;
using Smart.PowerCUBE.Api.DataModels;

namespace Kh2Host.Features.Fep;

public class FepAnalogData
{
    public class HostAnalogData
    {
        public ushort PointIndex { get; set; }
        public float Value { get; set; }
        public byte Tlq { get; set; }
        public uint ReceiveTM { get; set; }
        public ushort ReceiveMilli { get; set; }
    }

    public class Command : IRequest<Response>
    {
        public uint SequenceID { get; set; }
        public RealPointType PointType { get; set; }
        public uint DeviceID { get; set; }
        public bool IsUnsol { get; set; }
        public List<HostAnalogData>? Datas { get; set; }
    }
    public class Response : BaseResponse
    {

    }

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly IApiLogger _logger;
        private readonly IConfiguration _configuration;
        private readonly CommonDataManager _dbManager;

        public CommandHandler(IApiLogger logger
                            , IConfiguration configuration
                            , CommonDataManager dbManager)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._dbManager = dbManager;
        }
        public Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var response = new Response { Result = false };
            var ceqs = _dbManager.ConductingEquipmentModels!.Where(x => x.DeviceFk == request.DeviceID);
            if (ceqs != null)
            {
                int ceqType = ceqs.FirstOrDefault()?.CeqTypeFk?? 0;
                var rpNames = ceqs.Select(x => x.DpName);
                if (rpNames != null)
                {
                    // 미들웨어에서 실시간 포인트 목록을 가져옴
                    var rpDatas = PowerCubeApi.Instance.GetRealPointData(rpNames.ToList());
                    if(rpDatas != null)
                    {
                        // TLQ 처리 스킵
                        var updateRps = rpDatas.Select(x => new RealPointProcModel { Equipment = ceqs.FirstOrDefault(c => c.DpName == x.RealPointName), RealPointName = x.RealPointName, OldRealData = x, NewRealData = new RealPointDataModel { RealPointName = x.RealPointName } }).ToList();
                        // 업데이트 포인트 인덱스 목록
                        var pointIndexes = _dbManager.PointIndexModels!.Where(x => x.PointTypeId == (int)request.PointType);
                        var now = DateTime.Now;

                        var updatePoints = request.Datas!.Join(
                            pointIndexes,
                            x => x.PointIndex,
                            y => y.PointIndex,
                            (x, y) => new AnalogProcModel { PointIndex = y, NewValue = x.Value, NewTlq = x.Tlq, RecvTm = x.ReceiveTM, RecvMilli = x.ReceiveMilli }).ToList();
                        foreach (var rp in updateRps)
                        {
                            rp.NewRealData.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = ConstDefine.GetUpdateTimeString(request.PointType), DataValue = now.ToString("yyyy-MM-dd HH:mm:ss") });
                        }

                        var alarms = new List<EventBodyData>();
                        var dbupdatas = new List<UpdatePointValue>();
                        foreach (var pValue in updatePoints)
                        {
                            ushort newTlqValue = (ushort)(pValue.NewTlq == SettingTLQValue.PointOfflineValue ? SettingTLQValue.QualityOffLineValue : SettingTLQValue.Normal);
                            // 전체 포인트에 업데이트
                            if (pValue.PointIndex.CircuitNo == 0)
                            {
                                foreach (var rp in updateRps)
                                {
                                    rp.AnalogRealPointProc(pValue, alarms, dbupdatas, newTlqValue);
                                }
                            }
                            else
                            {
                                // 업데이트 포인트 인덱스 정보 가져옴
                                var rp = updateRps.FirstOrDefault(x => x.Equipment!.CircuitNo == pValue.PointIndex.CircuitNo);
                                if (rp != null)
                                {
                                    rp.AnalogRealPointProc(pValue, alarms, dbupdatas, newTlqValue);
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
            }

            return Task.FromResult(response);
        }
    }
}
