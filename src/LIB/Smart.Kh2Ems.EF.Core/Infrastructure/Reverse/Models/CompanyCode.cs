using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 회사 정보
/// </summary>
public partial class CompanyCode
{
    /// <summary>
    /// 회사 정보 ID
    /// </summary>
    public int CompanyCodeId { get; set; }

    /// <summary>
    /// 회사명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 회사코드
    /// </summary>
    public int? CompanyNo { get; set; }

    /// <summary>
    /// 사업자등록번호
    /// </summary>
    public string? BusinessNo { get; set; }
}
