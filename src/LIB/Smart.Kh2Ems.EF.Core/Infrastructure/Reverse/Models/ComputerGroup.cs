using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 컴퓨터 그룹
/// </summary>
public partial class ComputerGroup
{
    /// <summary>
    /// 컴퓨터 그룹ID
    /// </summary>
    public int ComputerGroupId { get; set; }

    /// <summary>
    /// 컴퓨터 그룹명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 이중화구성여부
    /// </summary>
    public byte? IsDup { get; set; }

    /// <summary>
    /// 설명
    /// </summary>
    public string? Description { get; set; }

    public virtual ICollection<ComputerInfo> ComputerInfos { get; } = new List<ComputerInfo>();
}
