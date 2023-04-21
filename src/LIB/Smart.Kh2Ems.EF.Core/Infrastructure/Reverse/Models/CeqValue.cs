using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 설비 포인트 값 정보
/// </summary>
public partial class CeqValue
{
    /// <summary>
    /// 설비 mrID
    /// </summary>
    public long CeqMrid { get; set; }

    /// <summary>
    /// 동적 인덱스 ID
    /// </summary>
    public int DynamicIndex { get; set; }

    /// <summary>
    /// 포인트 타입 ID
    /// </summary>
    public int? PointTypeFk { get; set; }

    /// <summary>
    /// 포인트 인덱스 ID
    /// </summary>
    public int? PointIndex { get; set; }

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

    /// <summary>
    /// 서버 업데이트 시간
    /// </summary>
    public DateTime? SaveTime { get; set; }

    public virtual PointType? PointTypeFkNavigation { get; set; }
}
