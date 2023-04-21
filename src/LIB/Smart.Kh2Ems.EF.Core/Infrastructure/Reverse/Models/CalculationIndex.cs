using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 계산 포인트 인덱스
/// </summary>
public partial class CalculationIndex
{
    /// <summary>
    /// 인덱스 ID
    /// </summary>
    public int IndexId { get; set; }

    /// <summary>
    /// 설비 종류 ID
    /// </summary>
    public int CeqTypeFk { get; set; }

    /// <summary>
    /// 인덱스명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 영문 인덱스명
    /// </summary>
    public string EName { get; set; } = null!;

    /// <summary>
    /// 계산주기
    /// </summary>
    public int? Period { get; set; }

    /// <summary>
    /// 계산식
    /// </summary>
    public string? Formula { get; set; }

    /// <summary>
    /// 포인트 타입 ID
    /// </summary>
    public int PointTypeFk { get; set; }

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
    /// 단위 ID
    /// </summary>
    public int? UnitFk { get; set; }

    /// <summary>
    /// 최소 LIMIT 값
    /// </summary>
    public double? LimitMinValue { get; set; }

    /// <summary>
    /// 최대 LIMIT 값
    /// </summary>
    public double? LimitMaxValue { get; set; }

    /// <summary>
    /// 설명
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 포인트 편집 시간
    /// </summary>
    public DateTime? EditTime { get; set; }

    public virtual AlarmPriority? AlarmPriorityFkNavigation { get; set; }

    public virtual CeqType CeqTypeFkNavigation { get; set; } = null!;

    public virtual DataType? DataTypeFkNavigation { get; set; }

    public virtual PointType PointTypeFkNavigation { get; set; } = null!;

    public virtual StateGroup? StateGroupFkNavigation { get; set; }

    public virtual Unit? UnitFkNavigation { get; set; }
}
