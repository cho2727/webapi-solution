using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 소리
/// </summary>
public partial class Sound
{
    /// <summary>
    /// 소리 ID
    /// </summary>
    public int SoundId { get; set; }

    /// <summary>
    /// 소리 이름
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 소리 파일
    /// </summary>
    public byte[]? SoundFile { get; set; }
}
