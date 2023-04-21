using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 원격 제어 설정 정보
/// </summary>
public partial class RemoteControlValue
{
    /// <summary>
    /// 원격 제어 ID
    /// </summary>
    public int RemoteControlId { get; set; }

    /// <summary>
    /// 원격 제어 명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 원격 제어 값
    /// </summary>
    public int? ControlValue { get; set; }

    /// <summary>
    /// HMI 제어 값
    /// </summary>
    public int? Value { get; set; }

    public virtual ICollection<BoIndexToRemote> BoIndexToRemotes { get; } = new List<BoIndexToRemote>();
}
