using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 포인트 TAG 코드
/// </summary>
public partial class PointTagCode
{
    /// <summary>
    /// 포인트 TAG 코드 ID
    /// </summary>
    public int PointTagCodeId { get; set; }

    /// <summary>
    /// 포인트 TAG 코드명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 포인트 TAG 코드 값
    /// </summary>
    public long? Value { get; set; }

    /// <summary>
    /// 별칭
    /// </summary>
    public string? AliseName { get; set; }
}
