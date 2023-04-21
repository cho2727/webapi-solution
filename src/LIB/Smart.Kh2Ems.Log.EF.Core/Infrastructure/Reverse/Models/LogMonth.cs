using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.Log.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 월별 이력 저장
/// </summary>
public partial class LogMonth
{
    /// <summary>
    /// 기본아이디
    /// </summary>
    public long Idx { get; set; }

    /// <summary>
    /// 지역(사업소) 코드
    /// </summary>
    public long MemberOfficeId { get; set; }

    /// <summary>
    /// 스테이션 ID
    /// </summary>
    public long StationId { get; set; }

    /// <summary>
    /// 설비 ID
    /// </summary>
    public long CeqId { get; set; }

    /// <summary>
    /// 설비 종류 ID
    /// </summary>
    public int CeqTypeId { get; set; }

    /// <summary>
    /// 모델 ID
    /// </summary>
    public int ModelId { get; set; }

    /// <summary>
    /// 동적 인덱스 ID
    /// </summary>
    public int DynamicIndex { get; set; }

    /// <summary>
    /// 값
    /// </summary>
    public double? Value { get; set; }

    /// <summary>
    /// 동적인덱스이름
    /// </summary>
    public string? IndexName { get; set; }

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
    /// 서버 저장 시간
    /// </summary>
    public DateTime? SaveTime { get; set; }
}
