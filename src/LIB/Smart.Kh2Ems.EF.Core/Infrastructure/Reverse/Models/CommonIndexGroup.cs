using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 공통 인덱스 그룹
/// </summary>
public partial class CommonIndexGroup
{
    /// <summary>
    /// 그룹ID
    /// </summary>
    public int IndexGroupId { get; set; }

    /// <summary>
    /// 그룹명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 영문명
    /// </summary>
    public string? EName { get; set; }

    /// <summary>
    /// 생성여부
    /// </summary>
    public byte? IsCreate { get; set; }

    public virtual ICollection<CommonIndex> CommonIndices { get; } = new List<CommonIndex>();
}
