using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 프로그램 상태
/// </summary>
public partial class ProgramState
{
    /// <summary>
    /// 프로그램 ID
    /// </summary>
    public int ProgramFk { get; set; }

    /// <summary>
    /// 상태
    /// </summary>
    public byte? Status { get; set; }

    /// <summary>
    /// 갱신시간
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 시작시간
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 종료시간
    /// </summary>
    public DateTime? EndTime { get; set; }

    public virtual ProgramInfo ProgramFkNavigation { get; set; } = null!;
}
