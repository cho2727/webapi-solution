using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 스테이션 정보
/// </summary>
public partial class Substation
{
    /// <summary>
    /// 스테이션 ID
    /// </summary>
    public long StationMrfk { get; set; }

    /// <summary>
    /// 스테이션 코드
    /// </summary>
    public string? StationCode { get; set; }

    /// <summary>
    /// 스테이션 타입
    /// </summary>
    public int? StationTypeFk { get; set; }

    /// <summary>
    /// 스테이션 주소
    /// </summary>
    public string? StationAdder { get; set; }

    /// <summary>
    /// 지역 코드 ID
    /// </summary>
    public long? MemberOfficeFk { get; set; }

    public virtual ICollection<ConductingEquipment> ConductingEquipments { get; } = new List<ConductingEquipment>();

    public virtual MemberOffice? MemberOfficeFkNavigation { get; set; }

    public virtual StationType? StationTypeFkNavigation { get; set; }
}
