using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 스케일 펙터
/// </summary>
public partial class ScaleFactor
{
    /// <summary>
    /// 스케일 펙터 ID
    /// </summary>
    public int ScaleFactorId { get; set; }

    /// <summary>
    /// 스케일 펙터 값
    /// </summary>
    public double Scale { get; set; }

    /// <summary>
    /// 스케일 펙터 OFFSET 값
    /// </summary>
    public double Offset { get; set; }

    public virtual ICollection<AiIndex> AiIndices { get; } = new List<AiIndex>();

    public virtual ICollection<AoIndex> AoIndices { get; } = new List<AoIndex>();

    public virtual ICollection<CounterIndex> CounterIndices { get; } = new List<CounterIndex>();

    public virtual ICollection<MasterIndex> MasterIndices { get; } = new List<MasterIndex>();
}
