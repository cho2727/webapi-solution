using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 사용자 권한 타입
/// </summary>
public partial class UserAuthorityType
{
    /// <summary>
    /// 사용자 권한 타입 ID
    /// </summary>
    public int UserAuthorityTypeId { get; set; }

    /// <summary>
    /// 사용자 권한 타입명
    /// </summary>
    public string Name { get; set; } = null!;

    public virtual ICollection<UserGroup> UserGroupFks { get; } = new List<UserGroup>();
}
