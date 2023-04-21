using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 개체 타입
/// </summary>
public partial class ObjectType
{
    /// <summary>
    /// 개체 타입 ID
    /// </summary>
    public int ObjectTypeId { get; set; }

    /// <summary>
    /// 개체 타입명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 타입 코드
    /// </summary>
    public string? TypeCode { get; set; }

    public string? EName { get; set; }

    public virtual ICollection<ModelInfo> ModelInfos { get; } = new List<ModelInfo>();
}
