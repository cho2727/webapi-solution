using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 컴퓨터 정보
/// </summary>
public partial class ComputerInfo
{
    /// <summary>
    /// 컴퓨터 ID
    /// </summary>
    public int ComputerId { get; set; }

    /// <summary>
    /// 컴퓨터명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 컴퓨터 그룹ID
    /// </summary>
    public int ComputerGroupFk { get; set; }

    /// <summary>
    /// 알람 우선순위 ID
    /// </summary>
    public int? AlarmPriorityFk { get; set; }

    /// <summary>
    /// 상태 그룹 ID
    /// </summary>
    public int? StateGroupFk { get; set; }

    /// <summary>
    /// 지역 코드
    /// </summary>
    public long? MemberOfficeFk { get; set; }

    /// <summary>
    /// OS 이름
    /// </summary>
    public string? OsName { get; set; }

    /// <summary>
    /// OS 버젼
    /// </summary>
    public string? OsVersion { get; set; }

    /// <summary>
    /// 사용 여부
    /// </summary>
    public byte? UseFlag { get; set; }

    public virtual AlarmPriority? AlarmPriorityFkNavigation { get; set; }

    public virtual ComputerGroup ComputerGroupFkNavigation { get; set; } = null!;

    public virtual ComputerState? ComputerState { get; set; }

    public virtual MemberOffice? MemberOfficeFkNavigation { get; set; }

    public virtual ICollection<ProgramInfo> ProgramInfos { get; } = new List<ProgramInfo>();

    public virtual StateGroup? StateGroupFkNavigation { get; set; }
}
