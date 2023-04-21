using Kh2Host.Common;
using Kh2Host.Enums;
using Kh2Host.Extentions;
using MediatR;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.PowerCUBE.Api.DataModels;
using Smart.PowerCUBE.Api;

namespace Kh2Host.Features.Agent;

public class AgentComputerState
{
    public class Command : ComputerStatusSendRequestModel, IRequest<Response>
    {

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
            var alarms = new List<EventBodyData>();
            var updateRps = new List<RealPointDataModel>();
            foreach (var data in request.Datas!)
            {
                var computerInfo = _dbManager.ComputerInfoDatas?.FirstOrDefault(x => x.ComputerId == data.ComputerId);
                if (computerInfo != null)
                {
                    var statusValues = _dbManager.stateValueModels?.Where(x => x.StateGroupID == computerInfo.StateGroupFk).ToList();
                    var rp = PowerCubeApi.Instance.GetRealPointData(computerInfo.DpName);
                    if (rp != null)
                    {
                        var dp = new RealPointDataModel() { RealPointName = computerInfo.DpName };

                        var status_point = rp.PointData.FirstOrDefault(x => x.DataTypeName == "status");
                        byte status = byte.Parse(status_point?.DataValue ?? "0");
                        if (status != data.Status)
                        {
                            // 알람 발생 및 실시간 포인트 업데이트
                            alarms.ComputerAlarmGen(AlarmTypeValue.ProgramStateChange, computerInfo, data.Status, status, data.UpdateTime, statusValues);
                            dp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "status", DataValue = data.Status.ToString() });
                        }

                        dp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "update_tm", DataValue = data.UpdateTime });
                        dp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "active_state", DataValue = data.ActiveState.ToString() });
                        updateRps.Add(dp);
                    }
                }
            }

            if (updateRps.Count > 0)
            {
                response.Result = true;
                PowerCubeApi.Instance.MultiUpdateRealDataPoint(updateRps);
                foreach (var alarm in alarms)
                {
                    RealPointExtention.AlarmDataSend(alarm, string.Empty);
                }
            }
            return Task.FromResult(response);
        }

    }
}
