using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 원격통신기기설정
/// </summary>
public partial class DeviceCommConfig
{
    /// <summary>
    /// 통신기기ID
    /// </summary>
    public int DeviceFk { get; set; }

    /// <summary>
    /// 전송 타임아웃
    /// </summary>
    public int? AppSendTimeout { get; set; }

    /// <summary>
    /// 재시도 수행횟수
    /// </summary>
    public int? AppRetryCount { get; set; }

    /// <summary>
    /// 재시도 타임아웃
    /// </summary>
    public int? AppRetryTimeout { get; set; }

    /// <summary>
    /// 파라메터 속성값
    /// </summary>
    public string? ParameterProperty { get; set; }

    /// <summary>
    /// 이벤트 계측 주기
    /// </summary>
    public int? EventInterval { get; set; }

    /// <summary>
    /// 전체계측 주기
    /// </summary>
    public int? TotalInterval { get; set; }

    /// <summary>
    /// 통신상태전송주기
    /// </summary>
    public int? CommStatusInterval { get; set; }

    public virtual DeviceCommUnit DeviceFkNavigation { get; set; } = null!;
}
