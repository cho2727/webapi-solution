using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 데이터 타입
/// </summary>
public partial class DataType
{
    /// <summary>
    /// 데이터 타입 ID
    /// </summary>
    public int DataTypeId { get; set; }

    /// <summary>
    /// 데이터 타입명
    /// </summary>
    public string Name { get; set; } = null!;

    public virtual ICollection<AiIndex> AiIndices { get; } = new List<AiIndex>();

    public virtual ICollection<AoIndex> AoIndices { get; } = new List<AoIndex>();

    public virtual ICollection<BiIndex> BiIndices { get; } = new List<BiIndex>();

    public virtual ICollection<CalculationIndex> CalculationIndices { get; } = new List<CalculationIndex>();

    public virtual ICollection<CommonIndex> CommonIndices { get; } = new List<CommonIndex>();

    public virtual ICollection<CounterIndex> CounterIndices { get; } = new List<CounterIndex>();

    public virtual ICollection<MasterIndex> MasterIndices { get; } = new List<MasterIndex>();
}
