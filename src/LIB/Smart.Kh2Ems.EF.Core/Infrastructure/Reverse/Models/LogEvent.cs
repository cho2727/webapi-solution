using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 이벤트 로그
/// </summary>
public partial class LogEvent
{
    /// <summary>
    /// 로그 ID
    /// </summary>
    public long LogId { get; set; }

    /// <summary>
    /// 이벤트 ID
    /// </summary>
    public long? EventId { get; set; }

    /// <summary>
    /// 이벤트 타입
    /// </summary>
    public int? EventTypeFk { get; set; }

    /// <summary>
    /// 지역 코드
    /// </summary>
    public long? MemberOfficeFk { get; set; }

    /// <summary>
    /// 변전소 ID
    /// </summary>
    public long? StationFk { get; set; }

    /// <summary>
    /// 연결 기기 ID
    /// </summary>
    public long? DeviceCeqFk { get; set; }

    /// <summary>
    /// 설비 ID
    /// </summary>
    public long? CeqFk { get; set; }

    /// <summary>
    /// 설비 타입 ID
    /// </summary>
    public int? CeqTypeFk { get; set; }

    /// <summary>
    /// 동적인덱스 ID
    /// </summary>
    public int? DdataIndexFk { get; set; }

    /// <summary>
    /// 포인트 타입 ID
    /// </summary>
    public byte? PointTypeFk { get; set; }

    /// <summary>
    /// 포인트 인덱스 ID
    /// </summary>
    public int? PointIndexFk { get; set; }

    /// <summary>
    /// 회로번호
    /// </summary>
    public byte? CircuitNo { get; set; }

    /// <summary>
    /// 알람 우선순위
    /// </summary>
    public int? AlarmPriority { get; set; }

    /// <summary>
    /// TAG 값
    /// </summary>
    public int? TagValue { get; set; }

    /// <summary>
    /// Quality 값
    /// </summary>
    public int? QualityValue { get; set; }

    /// <summary>
    /// 이전값
    /// </summary>
    public double? OldValue { get; set; }

    /// <summary>
    /// 현재값
    /// </summary>
    public double? NewValue { get; set; }

    /// <summary>
    /// 연결 기기 발생 시간
    /// </summary>
    public DateTime? DeviceEventTime { get; set; }

    /// <summary>
    /// 서버 발생 시간
    /// </summary>
    public DateTime? EventCreateTime { get; set; }

    /// <summary>
    /// 메시지
    /// </summary>
    public string? EventMsg { get; set; }

    /// <summary>
    /// ACK
    /// </summary>
    public byte? Ack { get; set; }

    /// <summary>
    /// ACK 사용자 ID
    /// </summary>
    public string? AckUserFk { get; set; }

    /// <summary>
    /// ACK 타임
    /// </summary>
    public DateTime? AckTime { get; set; }

    /// <summary>
    /// 업데이트 시간(저장시간)
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
