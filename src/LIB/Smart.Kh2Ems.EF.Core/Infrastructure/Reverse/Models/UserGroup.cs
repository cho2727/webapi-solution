using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 사용자 그룹 정보
/// </summary>
public partial class UserGroup
{
    /// <summary>
    /// 사용자 그룹 ID
    /// </summary>
    public int UserGroupId { get; set; }

    /// <summary>
    /// 사용자 그룹명
    /// </summary>
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();

    public virtual ICollection<UserAuthorityType> UserAuthorityTypeFks { get; } = new List<UserAuthorityType>();
}
