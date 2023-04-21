using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 레포트 연산자 타입
/// </summary>
public partial class ReportOperatorType
{
    /// <summary>
    /// 레포트 연산자 타입 ID
    /// </summary>
    public int OperatorTypeId { get; set; }

    /// <summary>
    /// 레포트 연산자 타입명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 레포트 연산자 타입 코드
    /// </summary>
    public string? OperatorCode { get; set; }
}
