using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 알람 타입
/// </summary>
public partial class AlarmType
{
    /// <summary>
    /// 알람 타입 ID
    /// </summary>
    public int AlarmTypeId { get; set; }

    /// <summary>
    /// 알람 타입명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 알람 영문명
    /// </summary>
    public string? EName { get; set; }
}
