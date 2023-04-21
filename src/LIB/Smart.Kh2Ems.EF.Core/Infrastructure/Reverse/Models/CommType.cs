using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 통신 타입
/// </summary>
public partial class CommType
{
    /// <summary>
    /// 통신 타입 ID
    /// </summary>
    public int CommTypeId { get; set; }

    /// <summary>
    /// 통신 타입명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 통신 성공률 기준값
    /// </summary>
    public int? CommrateStandard { get; set; }

    /// <summary>
    /// 어플리케이션 송신 대기 시간
    /// </summary>
    public int? ApplicationTimeout { get; set; }

    /// <summary>
    /// HMI 명령 타임아웃
    /// </summary>
    public int? HmiCmdTimeout { get; set; }

    /// <summary>
    /// HMI 파형 명령 타임아웃
    /// </summary>
    public int? HmiFileCmdTimeout { get; set; }

    public virtual ICollection<DeviceCommUnit> DeviceCommUnits { get; } = new List<DeviceCommUnit>();
}
