using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 알람 우선순위
/// </summary>
public partial class AlarmPriority
{
    /// <summary>
    /// 알람 우선순위 ID
    /// </summary>
    public int AlarmPriorityId { get; set; }

    /// <summary>
    /// 알람 우선순위명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 소리 ID
    /// </summary>
    public int? SoundFk { get; set; }

    public virtual ICollection<AiIndex> AiIndices { get; } = new List<AiIndex>();

    public virtual ICollection<AoIndex> AoIndices { get; } = new List<AoIndex>();

    public virtual ICollection<BiIndex> BiIndices { get; } = new List<BiIndex>();

    public virtual ICollection<CalculationIndex> CalculationIndices { get; } = new List<CalculationIndex>();

    public virtual ICollection<ComputerInfo> ComputerInfos { get; } = new List<ComputerInfo>();

    public virtual ICollection<CounterIndex> CounterIndices { get; } = new List<CounterIndex>();

    public virtual ICollection<MasterIndex> MasterIndices { get; } = new List<MasterIndex>();

    public virtual ICollection<ProgramInfo> ProgramInfos { get; } = new List<ProgramInfo>();
}
