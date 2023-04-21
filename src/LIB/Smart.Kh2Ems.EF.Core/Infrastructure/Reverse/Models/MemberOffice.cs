using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 지역(사업소) 정보
/// </summary>
public partial class MemberOffice
{
    /// <summary>
    /// 지역(사업소) 코드
    /// </summary>
    public long MemberOfficeId { get; set; }

    /// <summary>
    /// 그룹사업소 ID
    /// </summary>
    public int GroupOfficeFk { get; set; }

    /// <summary>
    /// 지역(사업소) 명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 지역(사업소) 타입
    /// </summary>
    public byte? OfficeType { get; set; }

    public virtual ICollection<ComputerInfo> ComputerInfos { get; } = new List<ComputerInfo>();

    public virtual GroupOffice GroupOfficeFkNavigation { get; set; } = null!;

    public virtual ICollection<Substation> Substations { get; } = new List<Substation>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
