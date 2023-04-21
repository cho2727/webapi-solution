using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 설비 타입
/// </summary>
public partial class CeqType
{
    /// <summary>
    /// 설비 타입 ID
    /// </summary>
    public int CeqTypeId { get; set; }

    /// <summary>
    /// 설비 타입명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 설비 영문 타입명
    /// </summary>
    public string EName { get; set; } = null!;

    /// <summary>
    /// 프로토콜 ID
    /// </summary>
    public int? ProtocolTypeFk { get; set; }

    public virtual ICollection<CalculationIndex> CalculationIndices { get; } = new List<CalculationIndex>();

    public virtual ICollection<MasterIndex> MasterIndices { get; } = new List<MasterIndex>();

    public virtual ICollection<ModbusSchedule> ModbusSchedules { get; } = new List<ModbusSchedule>();

    public virtual ICollection<ModelInfo> ModelInfos { get; } = new List<ModelInfo>();

    public virtual ProtocolType? ProtocolTypeFkNavigation { get; set; }
}
