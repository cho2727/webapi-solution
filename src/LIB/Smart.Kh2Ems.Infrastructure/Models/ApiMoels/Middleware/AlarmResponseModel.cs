using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using System.Runtime.InteropServices;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;

public class AlarmModel
{
    public long EventId { get; set; }

    public int EventType { get; set; }

    public long MemberOffice { get; set; }

    public long StationId { get; set; }

    public int DeviceId { get; set; }

    public long CeqId { get; set; }

    public int CeqTypeId { get; set; }

    public int DdataId { get; set; }

    public byte PointType { get; set; }

    public int PointIndex { get; set; }

    public byte CircuitNo { get; set; }

    public int AlarmPriority { get; set; }

    public int TagValue { get; set; }

    public double OldValue { get; set; }

    public double NewValue { get; set; }

    public string DeviceEventTime { get; set; } = null!;

    public string EventCreateTime { get; set; } = null!;

    public string EventMsg { get; set; } = null!;
}

public class AlarmResponseModel : BaseResponse
{
    public AlarmModel? Data { get; set; } = null;
}