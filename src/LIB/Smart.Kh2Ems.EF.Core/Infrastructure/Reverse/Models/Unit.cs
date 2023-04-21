using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 단위
/// </summary>
public partial class Unit
{
    /// <summary>
    /// 단위 ID
    /// </summary>
    public int UnitId { get; set; }

    /// <summary>
    /// 단위
    /// </summary>
    public string Name { get; set; } = null!;

    public virtual ICollection<AiIndex> AiIndices { get; } = new List<AiIndex>();

    public virtual ICollection<AoIndex> AoIndices { get; } = new List<AoIndex>();

    public virtual ICollection<CalculationIndex> CalculationIndices { get; } = new List<CalculationIndex>();

    public virtual ICollection<CounterIndex> CounterIndices { get; } = new List<CounterIndex>();

    public virtual ICollection<MasterIndex> MasterIndices { get; } = new List<MasterIndex>();

    public virtual ICollection<ModelItemIndex> ModelItemIndices { get; } = new List<ModelItemIndex>();
}
