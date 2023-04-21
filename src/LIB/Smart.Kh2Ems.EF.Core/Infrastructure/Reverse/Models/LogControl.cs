using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 제어 수행로그
/// </summary>
public partial class LogControl
{
    /// <summary>
    /// 로그ID
    /// </summary>
    public long LogId { get; set; }

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
    /// 현재값
    /// </summary>
    public double? NewValue { get; set; }

    /// <summary>
    /// 사용자 ID
    /// </summary>
    public string? ControlUserFk { get; set; }

    /// <summary>
    /// 제어 수행 시간
    /// </summary>
    public DateTime? ControlTime { get; set; }

    /// <summary>
    /// 제어 결과(0:실패, 1:성공)
    /// </summary>
    public int? ControlResult { get; set; }

    /// <summary>
    /// 제어 결과 메시지
    /// </summary>
    public string? ControlResultMessage { get; set; }

    /// <summary>
    /// 제어 결과 시간
    /// </summary>
    public DateTime? ControlResultTime { get; set; }

    /// <summary>
    /// 업데이트 시간(저장시간)
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
