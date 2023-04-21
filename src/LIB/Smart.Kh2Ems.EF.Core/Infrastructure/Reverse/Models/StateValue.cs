using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 상태 값 변경이름정보
/// </summary>
public partial class StateValue
{
    /// <summary>
    /// 상태값 변경 ID
    /// </summary>
    public int StateValueId { get; set; }

    /// <summary>
    /// 상태값 변경명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 상태값
    /// </summary>
    public double? Value { get; set; }

    /// <summary>
    /// 상태 그룹 ID
    /// </summary>
    public int? StateGroupFk { get; set; }

    public virtual StateGroup? StateGroupFkNavigation { get; set; }
}
