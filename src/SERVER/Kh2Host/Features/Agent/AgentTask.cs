using Azure.Core;
using Kh2Host.Common;
using Kh2Host.Features.Agent;
using MediatR;
using Smart.Kh2Ems.Infrastructure.Enums;
using Smart.Kh2Ems.Infrastructure.Features.Fep;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Models.MidStructs;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.PowerCUBE.Api;
using System.Collections.Concurrent;

namespace Kh2Host.Features.Fep;

public class AgentTask
{
    private readonly IApiLogger _logger;
    private readonly CommonDataManager _dbManager;
    private readonly IMediator _mediator;
    private readonly string _cubeBoxName;

    public AgentTask(IApiLogger logger, CommonDataManager dbManager, IMediator mediator, string cubeBoxName)
    {
        this._cubeBoxName = cubeBoxName;
        this._logger = logger;
        this._dbManager = dbManager;
        this._mediator = mediator;
    }

    public void StartTask(CancellationToken stoppingToken)
    {
        Task.Run(async () => await MessageRecvRunAsync(stoppingToken, _cubeBoxName));
    }

    private async ValueTask MessageRecvRunAsync(CancellationToken stoppingToken, string cubeBoxName)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_dbManager.IsMiddlewareConnected)
                {
                    int retValue = PowerCubeApi.Instance.RecvMessageBox(cubeBoxName, out CommonHeadPacket head, out byte[] data);
                    if (retValue == CubeReturnCode.CubeOK)
                    {
                        if(Enum.IsDefined(typeof(MsgTypeDefine), head.MsgType))
                        {
                            switch ((MsgTypeDefine)head.MsgType)
                            {
                                case MsgTypeDefine.ComputerStateUpdateRequest:
                                    {
                                        var values = PowerValueConvert.ByteToStructArray<ComputerStateInfo>(data);
                                        var request = new AgentComputerState.Command
                                        {
                                            Header = new BaseMessageHeader
                                            {
                                                MsgType = head.MsgType,
                                                RequestProcessName = head.RequestProcessName,
                                                SendTime = head.SendTime,
                                                CubeBoxName = head.CubeBoxName
                                            },
                                            Datas = values.Select(x => new ComputerStatusModel {
                                                ComputerId = x.ComputerId,
                                                CpuRate = x.CpuRate,
                                                MemTotal = x.MemTotal,
                                                MemUsage = x.MemUsage,
                                                DiskTotal = x.DiskTotal,
                                                DiskUsage = x.DiskUsage,
                                                Status = x.Status,
                                                ActiveState = x.ActiveState,
                                                UpdateTime = x.UpdateTime
                                            }).ToList()
                                        };
                                        var response = await _mediator.Send(request);
                                    }
                                    break;

                                case MsgTypeDefine.ProgramStateUpdateRequest:
                                    {
                                        var values = PowerValueConvert.ByteToStructArray<ProgramStateInfo>(data);
                                        var request = new AgentProgramState.Command
                                        {
                                            Header = new BaseMessageHeader
                                            {
                                                MsgType = head.MsgType,
                                                RequestProcessName = head.RequestProcessName,
                                                SendTime = head.SendTime,
                                                CubeBoxName = head.CubeBoxName
                                            },
                                            Datas = values.Select(x => new ProgramStatusModel
                                            {
                                                ProgramId = x.ProgramId,
                                                Status = x.Status,
                                                UpdateTime = x.UpdateTime,
                                                StartTime = x.StartTime,
                                                EndTime = x.EndTime
                                            }).ToList()
                                        };
                                        var response = await _mediator.Send(request);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            _logger.LogError($"정의되지 않은 명령 수신(msgtype:{head.MsgType})");
                        }                        
                    }
                }

                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{cubeBoxName} MessageRecvRunAsync ex{ex.Message}");
            }
        }

        Console.WriteLine($"{cubeBoxName} MessageRecvRunAsync END");
    }
}
