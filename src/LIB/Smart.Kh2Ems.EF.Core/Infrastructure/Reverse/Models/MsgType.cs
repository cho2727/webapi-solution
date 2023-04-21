using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 메시지 타입
/// </summary>
public partial class MsgType
{
    /// <summary>
    /// 메시지 타입 ID
    /// </summary>
    public int MsgTypeId { get; set; }

    /// <summary>
    /// 메시지 타입명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 메시지 타입 코드
    /// </summary>
    public int? MsgCode { get; set; }

    /// <summary>
    /// 메시지 그룹
    /// </summary>
    public int? MsgGroupFk { get; set; }

    /// <summary>
    /// 설명
    /// </summary>
    public string? Description { get; set; }

    public virtual MsgGroup? MsgGroupFkNavigation { get; set; }
}
