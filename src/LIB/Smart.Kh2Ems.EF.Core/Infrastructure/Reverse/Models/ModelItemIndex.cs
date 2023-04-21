using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 모델 아이템 정보
/// </summary>
public partial class ModelItemIndex
{
    /// <summary>
    /// 모델 아이템 ID
    /// </summary>
    public int IndexId { get; set; }

    /// <summary>
    /// 모델 아이템명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 모델 아이템 영문명
    /// </summary>
    public string? EName { get; set; }

    /// <summary>
    /// 단위 ID
    /// </summary>
    public int? UnitFk { get; set; }

    public virtual ICollection<ModelIndex> ModelIndices { get; } = new List<ModelIndex>();

    public virtual Unit? UnitFkNavigation { get; set; }
}
