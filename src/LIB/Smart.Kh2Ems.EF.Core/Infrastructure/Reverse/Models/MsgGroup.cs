using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 메시지 그룹
/// </summary>
public partial class MsgGroup
{
    /// <summary>
    /// 메시지 그룹 ID
    /// </summary>
    public int MsgGroupId { get; set; }

    /// <summary>
    /// 메시지 그룹명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 설명
    /// </summary>
    public string? Description { get; set; }

    public virtual ICollection<MsgType> MsgTypes { get; } = new List<MsgType>();
}
