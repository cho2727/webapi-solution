using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 지역(사업소) 코드
/// </summary>
public partial class AreaCode
{
    /// <summary>
    /// 지역 코드
    /// </summary>
    public long AreaCodeId { get; set; }

    /// <summary>
    /// 이름
    /// </summary>
    public string Name { get; set; } = null!;
}
