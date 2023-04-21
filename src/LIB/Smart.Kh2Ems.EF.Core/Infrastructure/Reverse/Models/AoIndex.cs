using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 아날로그 설정 표준 인덱스
/// </summary>
public partial class AoIndex
{
    /// <summary>
    /// 아날로그 설정 인덱스 ID
    /// </summary>
    public int IndexId { get; set; }

    /// <summary>
    /// 아날로그 설정 인덱스명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 아날로그 설정 영문 인덱스명
    /// </summary>
    public string EName { get; set; } = null!;

    /// <summary>
    /// 알람 우선순위 ID
    /// </summary>
    public int? AlarmPriorityFk { get; set; }

    /// <summary>
    /// 데이터 타입 ID
    /// </summary>
    public int? DataTypeFk { get; set; }

    /// <summary>
    /// 상태 그룹 ID
    /// </summary>
    public int? StateGroupFk { get; set; }

    /// <summary>
    /// 스케일 펙터터 ID
    /// </summary>
    public int? ScaleFactorFk { get; set; }

    /// <summary>
    /// 단위 ID
    /// </summary>
    public int? UnitFk { get; set; }

    /// <summary>
    /// deadband 값
    /// </summary>
    public double? Deadband { get; set; }

    /// <summary>
    /// AO 포인트 설정 기본 값
    /// </summary>
    public double? DefaultVal { get; set; }

    /// <summary>
    /// AO 포인트 설정 STEP 값
    /// </summary>
    public double? StepVal { get; set; }

    /// <summary>
    /// AO 포인트 설정 최소 값
    /// </summary>
    public double? MinVal { get; set; }

    /// <summary>
    /// AO 포인트 설정 최대 값
    /// </summary>
    public double? MaxVal { get; set; }

    /// <summary>
    /// AO 포인트 설정 OFF 존재 여부
    /// </summary>
    public byte? ExistOff { get; set; }

    /// <summary>
    /// AO 포인트 설정 OFF 값
    /// </summary>
    public double? OffVal { get; set; }

    public virtual AlarmPriority? AlarmPriorityFkNavigation { get; set; }

    public virtual DataType? DataTypeFkNavigation { get; set; }

    public virtual ScaleFactor? ScaleFactorFkNavigation { get; set; }

    public virtual StateGroup? StateGroupFkNavigation { get; set; }

    public virtual Unit? UnitFkNavigation { get; set; }
}
