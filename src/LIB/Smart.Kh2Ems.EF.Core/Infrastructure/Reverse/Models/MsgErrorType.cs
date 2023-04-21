using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 메시지 결과 에러 코드
/// </summary>
public partial class MsgErrorType
{
    /// <summary>
    /// 에러 메시지 코드 ID
    /// </summary>
    public int MsgErrorTypeId { get; set; }

    /// <summary>
    /// 에러 코드명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 메시지 코드
    /// </summary>
    public int? MsgErrorCode { get; set; }

    /// <summary>
    /// 설명
    /// </summary>
    public string? Description { get; set; }
}
