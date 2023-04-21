using Smart.Kh2Ems.Infrastructure.Models;
using Smart.PowerCUBE.Api;

namespace Kh2Host.Models;

public class RealPointProcModel
{
    public ConductingEquipmentModel? Equipment { get; set; } = null!;
    public string RealPointName { get; set; } = string.Empty;
    public RealPointDataModel OldRealData { get; set; } = null!;
    public RealPointDataModel NewRealData { get; set; } = null!;
}

public class BinaryProcModel
{
    public PointIndexModel PointIndex { get; set; } = null!;
    public byte NewValue { get; set; }
    public byte NewTlq { get; set; }
    public uint RecvTm { get; set; }
    public ushort RecvMilli { get; set; }

}

public class AnalogProcModel
{
    public PointIndexModel PointIndex { get; set; } = null!;
    public float NewValue { get; set; }
    public byte NewTlq { get; set; }
    public uint RecvTm { get; set; }
    public ushort RecvMilli { get; set; }
}

public class UpdatePointValue
{
    /// <summary>
    /// 설비 mrID
    /// </summary>
    public long CeqMrid { get; set; }
    /// <summary>
    /// 포인트 타입 ID
    /// </summary>
    public int? PointType { get; set; }

    /// <summary>
    /// 포인트 인덱스 ID
    /// </summary>
    public int? PointIndex { get; set; }

    /// <summary>
    /// 동적 인덱스 ID
    /// </summary>
    public int DynamicIndex { get; set; }

    /// <summary>
    /// 값
    /// </summary>
    public double? Value { get; set; }

    /// <summary>
    /// TAG 값
    /// </summary>
    public short? TagValue { get; set; }

    /// <summary>
    /// Quality 값
    /// </summary>
    public short? QualityValue { get; set; }

    /// <summary>
    /// 기기 업데이트 시간
    /// </summary>
    public DateTime? DeviceUptime { get; set; }
}