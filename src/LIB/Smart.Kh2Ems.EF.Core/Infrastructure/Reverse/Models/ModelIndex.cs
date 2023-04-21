using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 모델 인덱스 정보
/// </summary>
public partial class ModelIndex
{
    /// <summary>
    /// 모델 ID
    /// </summary>
    public int ModelFk { get; set; }

    /// <summary>
    /// 모델 아이템 ID
    /// </summary>
    public int ItemFk { get; set; }

    /// <summary>
    /// 표시 순서
    /// </summary>
    public int Seq { get; set; }

    /// <summary>
    /// 값
    /// </summary>
    public string? Value { get; set; }

    public virtual ModelItemIndex ItemFkNavigation { get; set; } = null!;

    public virtual ModelInfo ModelFkNavigation { get; set; } = null!;
}
