using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 모드버스 스케쥴
/// </summary>
public partial class ModbusSchedule
{
    /// <summary>
    /// 스케쥴ID
    /// </summary>
    public int ScheduleId { get; set; }

    /// <summary>
    /// 설비 타입 ID
    /// </summary>
    public int CeqTypeId { get; set; }

    /// <summary>
    /// 계측 순서
    /// </summary>
    public int? OrderNo { get; set; }

    /// <summary>
    /// 함수코드
    /// </summary>
    public int? FunctionCode { get; set; }

    /// <summary>
    /// 시작주소
    /// </summary>
    public int? StartAddress { get; set; }

    /// <summary>
    /// 계측 개수
    /// </summary>
    public int? MeasureCount { get; set; }

    public virtual CeqType CeqType { get; set; } = null!;
}
