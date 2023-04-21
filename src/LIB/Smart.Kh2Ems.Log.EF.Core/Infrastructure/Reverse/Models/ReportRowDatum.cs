using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.Log.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 보고서 기초 데이터
/// </summary>
public partial class ReportRowDatum
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
    /// 동적인덱스이름
    /// </summary>
    public string? IndexName { get; set; }

    /// <summary>
    /// 연산자 타입
    /// </summary>
    public int? OperationTypeId { get; set; }

    /// <summary>
    /// 값
    /// </summary>
    public double? Value { get; set; }

    /// <summary>
    /// 서버 저장 시간
    /// </summary>
    public DateTime? SaveTime { get; set; }
}
