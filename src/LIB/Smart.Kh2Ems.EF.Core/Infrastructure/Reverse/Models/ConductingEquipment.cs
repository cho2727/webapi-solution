using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 설비 정보
/// </summary>
public partial class ConductingEquipment
{
    /// <summary>
    /// 설비 ID
    /// </summary>
    public long Mrid { get; set; }

    /// <summary>
    /// 통신기기ID
    /// </summary>
    public int? DeviceFk { get; set; }

    /// <summary>
    /// 소속 스테이션 ID
    /// </summary>
    public long? StationMrfk { get; set; }

    /// <summary>
    /// 회로번호
    /// </summary>
    public byte? CircuitNo { get; set; }

    public virtual DeviceCommUnit? DeviceFkNavigation { get; set; }

    public virtual Substation? StationMrfkNavigation { get; set; }
}
