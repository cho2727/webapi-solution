using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 시스템 로그
/// </summary>
public partial class LogSystem
{
    /// <summary>
    /// 로그ID
    /// </summary>
    public long LogId { get; set; }

    /// <summary>
    /// 메시지 타입 ID
    /// </summary>
    public int MsgTypeId { get; set; }

    /// <summary>
    /// 지역 코드
    /// </summary>
    public long? MemberOfficeFk { get; set; }

    /// <summary>
    /// 컴퓨터 ID
    /// </summary>
    public int? ComputerFk { get; set; }

    /// <summary>
    /// 프로그램 ID
    /// </summary>
    public int? ProgramId { get; set; }

    /// <summary>
    /// 사용자 ID
    /// </summary>
    public string? ControlUserFk { get; set; }

    /// <summary>
    /// 메시지 수행 결과 코드(0:실패, 1:성공)
    /// </summary>
    public int? MsgResult { get; set; }

    /// <summary>
    /// 결과 메시지
    /// </summary>
    public string? MsgResultMessage { get; set; }

    /// <summary>
    /// 업데이트 시간(저장시간)
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
