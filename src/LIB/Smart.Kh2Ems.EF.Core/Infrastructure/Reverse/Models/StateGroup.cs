using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 상태 그룹 정보
/// </summary>
public partial class StateGroup
{
    /// <summary>
    /// 상태 그룹 ID
    /// </summary>
    public int StateGroupId { get; set; }

    /// <summary>
    /// 상태 그룹명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 상태 개수
    /// </summary>
    public int? Count { get; set; }

    public virtual ICollection<AiIndex> AiIndices { get; } = new List<AiIndex>();

    public virtual ICollection<AoIndex> AoIndices { get; } = new List<AoIndex>();

    public virtual ICollection<BiIndex> BiIndices { get; } = new List<BiIndex>();

    public virtual ICollection<CalculationIndex> CalculationIndices { get; } = new List<CalculationIndex>();

    public virtual ICollection<ComputerInfo> ComputerInfos { get; } = new List<ComputerInfo>();

    public virtual ICollection<CounterIndex> CounterIndices { get; } = new List<CounterIndex>();

    public virtual ICollection<MasterIndex> MasterIndices { get; } = new List<MasterIndex>();

    public virtual ICollection<ProgramInfo> ProgramInfos { get; } = new List<ProgramInfo>();

    public virtual ICollection<StateValue> StateValues { get; } = new List<StateValue>();
}
