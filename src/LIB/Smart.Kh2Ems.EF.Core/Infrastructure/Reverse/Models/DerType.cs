using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 분산전원 타입
/// </summary>
public partial class DerType
{
    /// <summary>
    /// 분산전원 타입 ID
    /// </summary>
    public int DerTypeId { get; set; }

    /// <summary>
    /// 분산전원명
    /// </summary>
    public string Name { get; set; } = null!;
}
