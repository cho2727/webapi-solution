using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 모델 정보
/// </summary>
public partial class ModelInfo
{
    /// <summary>
    /// 모델 ID
    /// </summary>
    public int ModelId { get; set; }

    /// <summary>
    /// 모델명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 설비 타입 ID
    /// </summary>
    public int? CeqTypeFk { get; set; }

    /// <summary>
    /// 개체 타입 ID
    /// </summary>
    public int ObjectTypeFk { get; set; }

    /// <summary>
    /// 설명
    /// </summary>
    public string? Description { get; set; }

    public virtual CeqType? CeqTypeFkNavigation { get; set; }

    public virtual ICollection<IdentityObject> IdentityObjects { get; } = new List<IdentityObject>();

    public virtual ICollection<ModelIndex> ModelIndices { get; } = new List<ModelIndex>();

    public virtual ObjectType ObjectTypeFkNavigation { get; set; } = null!;
}
