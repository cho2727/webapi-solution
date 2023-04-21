using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 상태 제어 표준 인덱스
/// </summary>
public partial class BoIndex
{
    /// <summary>
    /// 제어 인덱스 ID
    /// </summary>
    public int IndexId { get; set; }

    /// <summary>
    /// 제어 인덱스명
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 제어 영문 인덱스명
    /// </summary>
    public string EName { get; set; } = null!;

    /// <summary>
    /// 상태 제어 코드 개수
    /// </summary>
    public int? ControlCount { get; set; }

    /// <summary>
    /// 연결된 BI 인덱스 ID
    /// </summary>
    public int? LinkBiIndexFk { get; set; }

    public virtual ICollection<BoIndexToRemote> BoIndexToRemotes { get; } = new List<BoIndexToRemote>();

    public virtual BiIndex? LinkBiIndexFkNavigation { get; set; }
}
