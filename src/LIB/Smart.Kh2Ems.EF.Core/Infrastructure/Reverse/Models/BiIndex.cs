using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 상태 계측 표준 인덱스
/// </summary>
public partial class BiIndex
{
    /// <summary>
    /// 상태 계측 인덱스 ID
    /// </summary>
    public int IndexId { get; set; }

    /// <summary>
    /// 상태 계측 인덱스명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 상태 계측 영문 인덱스명
    /// </summary>
    public string EName { get; set; } = null!;

    /// <summary>
    /// 알람 우선순위 코드
    /// </summary>
    public int? AlarmPriorityFk { get; set; }

    /// <summary>
    /// 데이터 타입 코드
    /// </summary>
    public int? DataTypeFk { get; set; }

    /// <summary>
    /// 상태 그룹 코드
    /// </summary>
    public int? StateGroupFk { get; set; }

    public virtual AlarmPriority? AlarmPriorityFkNavigation { get; set; }

    public virtual ICollection<BoIndex> BoIndices { get; } = new List<BoIndex>();

    public virtual DataType? DataTypeFkNavigation { get; set; }

    public virtual StateGroup? StateGroupFkNavigation { get; set; }
}
