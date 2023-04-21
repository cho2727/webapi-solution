using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 상태 제어 코드 설정
/// </summary>
public partial class BoIndexToRemote
{
    /// <summary>
    /// 제어 코드 ID
    /// </summary>
    public int BoIndexRemoteId { get; set; }

    /// <summary>
    /// 제어 코드명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 연결된 BO 인덱스
    /// </summary>
    public int? BoIndexFk { get; set; }

    /// <summary>
    /// 연결된 원격 제어 설정 ID
    /// </summary>
    public int? RemoteControlIndexFk { get; set; }

    /// <summary>
    /// 제어 유효성 검사값
    /// </summary>
    public int? ValidBiValue { get; set; }

    public virtual BoIndex? BoIndexFkNavigation { get; set; }

    public virtual RemoteControlValue? RemoteControlIndexFkNavigation { get; set; }
}
