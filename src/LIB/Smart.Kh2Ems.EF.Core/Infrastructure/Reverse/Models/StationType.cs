using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 스테이션(변전소) 타입
/// </summary>
public partial class StationType
{
    /// <summary>
    /// 스테이션 타입 ID
    /// </summary>
    public int StationTypeId { get; set; }

    /// <summary>
    /// 스테이션명
    /// </summary>
    public string Name { get; set; } = null!;

    public virtual ICollection<Substation> Substations { get; } = new List<Substation>();
}
