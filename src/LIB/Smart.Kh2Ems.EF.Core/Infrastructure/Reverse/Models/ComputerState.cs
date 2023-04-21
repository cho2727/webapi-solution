using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 컴퓨터 상태
/// </summary>
public partial class ComputerState
{
    /// <summary>
    /// 컴퓨터 ID
    /// </summary>
    public int ComputerFk { get; set; }

    /// <summary>
    /// CPU 사용률(%)
    /// </summary>
    public double? CpuRate { get; set; }

    /// <summary>
    /// 전체 메모리 크기(MB)
    /// </summary>
    public long? MemTotal { get; set; }

    /// <summary>
    /// 사용 메모리 크기(MB)
    /// </summary>
    public long? MemUsage { get; set; }

    /// <summary>
    /// 전체 디스크 크기(MB)
    /// </summary>
    public long? DiskTotal { get; set; }

    /// <summary>
    /// 사용 디스크 크기(MB)
    /// </summary>
    public long? DiskUsage { get; set; }

    /// <summary>
    /// 상태
    /// </summary>
    public byte? Status { get; set; }

    /// <summary>
    /// 활성화 상태(Active)
    /// </summary>
    public byte? ActiveState { get; set; }

    /// <summary>
    /// 갱신 시간
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    public virtual ComputerInfo ComputerFkNavigation { get; set; } = null!;
}
