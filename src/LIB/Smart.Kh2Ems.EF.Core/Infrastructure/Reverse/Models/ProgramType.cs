using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 프로그램 타입
/// </summary>
public partial class ProgramType
{
    /// <summary>
    /// 프로그램 타입 ID
    /// </summary>
    public int ProgramTypeId { get; set; }

    /// <summary>
    /// 타입명
    /// </summary>
    public string Name { get; set; } = null!;

    public virtual ICollection<ProgramInfo> ProgramInfos { get; } = new List<ProgramInfo>();
}
