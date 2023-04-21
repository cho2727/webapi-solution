using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 프로토콜 타입
/// </summary>
public partial class ProtocolType
{
    /// <summary>
    /// 프로토콜 타입 ID
    /// </summary>
    public int ProtocolTypeId { get; set; }

    /// <summary>
    /// 프로토콜 타입명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Default 파라메타
    /// </summary>
    public string? DefaultParameter { get; set; }

    /// <summary>
    /// Default 설비 명령
    /// </summary>
    public string? DefaultDeviceCmd { get; set; }

    /// <summary>
    /// DLL 이름
    /// </summary>
    public string? DllName { get; set; }

    /// <summary>
    /// DLL 파일
    /// </summary>
    public byte[]? DllFile { get; set; }

    /// <summary>
    /// DLL 설명
    /// </summary>
    public string? DllDescription { get; set; }

    public virtual ICollection<CeqType> CeqTypes { get; } = new List<CeqType>();
}
