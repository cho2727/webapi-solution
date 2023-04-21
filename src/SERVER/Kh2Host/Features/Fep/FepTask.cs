using Kh2Host.Common;
using MediatR;
using Smart.Kh2Ems.Infrastructure.Enums;
using Smart.Kh2Ems.Infrastructure.Features.Fep;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.PowerCUBE.Api;
using System.Collections.Concurrent;

namespace Kh2Host.Features.Fep;

public class FepTask
{
    private readonly IApiLogger _logger;
    private readonly CommonDataManager _dbManager;
    private readonly IMediator _mediator;
    private readonly object _lock = new object();
    private readonly string _cubeBoxName;
    private ConcurrentQueue<FepMessageRequestModel> _fepRecvQueue = new ConcurrentQueue<FepMessageRequestModel>();

    public FepTask(IApiLogger logger, CommonDataManager dbManager, IMediator mediator, string cubeBoxName)
    {
        this._cubeBoxName = cubeBoxName;
        this._logger = logger;
        this._dbManager = dbManager;
        this._mediator = mediator;
    }

    public void Enqueue(FepMessageRequestModel model)
    {
        lock (_lock)
        {
            _fepRecvQueue.Enqueue(model);
            Console.WriteLine($"msg:{model.HeadData.MessageCode} Enqueue ");
        }
    }

    private FepMessageRequestModel? Dequeue()
    {
        lock (_lock)
        {
            if (_fepRecvQueue.TryDequeue(out FepMessageRequestModel? model))
            {
                return model;
            }
        }
        return null;
    }

    public void StartTask(CancellationToken stoppingToken)
    {
        Task.Run(async () => await MessageRecvRunAsync(stoppingToken, _cubeBoxName));
        Task.Run(async () => await MessageProcRunAsync(stoppingToken));
    }

    private async ValueTask MessageRecvRunAsync(CancellationToken stoppingToken, string cubeBoxName)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (_dbManager.IsMiddlewareConnected)
                {
                    int retValue = PowerCubeApi.Instance.RecvMessageBox(cubeBoxName, out FepRequestHeadPacket head, out byte[] data);
                    if (retValue == CubeReturnCode.CubeOK)
                    {
                        var model = new FepMessageRequestModel
                        {
                            HeadData = new FepMessageRequestModel.FepRequestHeadData
                            {
                                MessageCode = head.MessageCode,
                                MessageLengh = head.MessageLengh,
                                SendTime = head.SendTime,
                                SequenceID = head.SequenceID,
                                RtuID = head.RtuID,
                                RecordCount = head.RecordCount
                            },
                            PacketData = data
                        };

                        _logger.LogInformation($"[{cubeBoxName}->HOST] CODE:0x{model.HeadData.MessageCode.ToString("X")} SEQ:{model.HeadData.SequenceID} RTU:{model.HeadData.RtuID} CNT:{model.HeadData.RecordCount} MSG RCV ");
                        Enqueue(model);
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

    private async ValueTask MessageProcRunAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var model = Dequeue();
                if (model != null)
                {
                    await MessageProc(model.HeadData.MessageCode, model.HeadData.MessageLengh, model.HeadData.SendTime, model.HeadData.SequenceID, model.HeadData.RtuID, model.PacketData, model.HeadData.RecordCount);
                }

                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{_cubeBoxName} MessageProcRunAsync ex{ex.Message}");
            }
        }

        Console.WriteLine($"{_cubeBoxName} MessageProcRunAsync END");
    }

    private async ValueTask MessageProc(ushort messageCode, ushort length, uint sendtm, uint sequence, uint rtuid, byte[] rcvdata, ushort count)
    {
        _logger.LogInformation($"[{_cubeBoxName}->HOST] CODE:0x{messageCode.ToString("X")} SEQ:{sequence} RTU:{rtuid} CNT:{count} MSG PROC ");
        switch (messageCode)
        {
            case CubeFunctionCode.FepUnsolBinaryInput:
                {
                    var data = PowerValueConvert.ByteToStructArray<FepBinaryInputEvent>(rcvdata);
                    var request = new FepBinaryData.Command
                    {
                        SequenceID = sequence,
                        DeviceID = rtuid,
                        PointType = RealPointType.BI,
                        IsUnsol = true,
                        Datas = data.Select(x => new FepBinaryData.HostBinaryData
                        {
                            PointIndex = x.PointIndex,
                            Value = x.Value,
                            Tlq = x.Tlq,
                            ReceiveTM = x.TimeSec,
                            ReceiveMilli = x.TimeMilliSec
                        }).ToList()
                    };
                    var response = await _mediator.Send(request);
                }
                break;
            case CubeFunctionCode.FepMeasureBinaryInput:
                {
                    var data = PowerValueConvert.ByteToStructArray<FepBinaryInput>(rcvdata);
                    var request = new FepBinaryData.Command
                    {
                        SequenceID = sequence,
                        DeviceID = rtuid,
                        PointType = RealPointType.BI,
                        IsUnsol = false,
                        Datas = data.Select(x => new FepBinaryData.HostBinaryData
                        {
                            PointIndex = x.PointIndex,
                            Value = x.Value,
                            Tlq = x.Tlq,
                            ReceiveTM = sendtm,
                            ReceiveMilli = 0
                        }).ToList()
                    };
                    var response = await _mediator.Send(request);
                }
                break;
            case CubeFunctionCode.FepUnsolAnalogInput:
                {
                    var data = PowerValueConvert.ByteToStructArray<FepAnalogInputEvent>(rcvdata);
                    var request = new FepAnalogData.Command
                    {
                        SequenceID = sequence,
                        DeviceID = rtuid,
                        PointType = RealPointType.AI,
                        IsUnsol = true,
                        Datas = data.Select(x => new FepAnalogData.HostAnalogData
                        {
                            PointIndex = x.PointIndex,
                            Value = x.Value,
                            Tlq = x.Tlq,
                            ReceiveTM = x.TimeSec,
                            ReceiveMilli = x.TimeMilliSec
                        }).ToList()
                    };
                    var response = await _mediator.Send(request);
                }
                break;
            case CubeFunctionCode.FepMeasureAnalogOutput:
            case CubeFunctionCode.FepMeasureAnalogInput:
                {
                    var data = PowerValueConvert.ByteToStructArray<FepAnalogInput>(rcvdata);
                    var request = new FepAnalogData.Command
                    {
                        SequenceID = sequence,
                        DeviceID = rtuid,
                        PointType = RealPointType.AI,
                        IsUnsol = false,
                        Datas = data.Select(x => new FepAnalogData.HostAnalogData
                        {
                            PointIndex = x.PointIndex,
                            Value = x.Value,
                            Tlq = x.Tlq,
                            ReceiveTM = sendtm,
                            ReceiveMilli = 0
                        }).ToList()
                    };
                    var response = await _mediator.Send(request);
                }
                break;
            case CubeFunctionCode.FepMeasureCounter:
                break;
            case CubeFunctionCode.FepCommStatus:
                {
                    var data = PowerValueConvert.ByteToStructArray<FepRtuStatus>(rcvdata);
                    var request = new FepCommData.Command
                    {
                        SequenceID = sequence,
                        Datas = data.Select(x => new FepCommData.HostCommStatus
                        {
                            RtuID = x.RtuID,
                            Status = x.Status,
                            CommAvg = x.CommAvg,
                            CommSuccess = x.CommSuccess,
                            CommFail = x.CommFail,
                            CommNoResponse = x.CommNoResponse,
                            CommLastSuccess = x.CommLastSuccess
                        }).ToList()
                    };
                    var response = await _mediator.Send(request);
                }
                break;
        }
    }
}
