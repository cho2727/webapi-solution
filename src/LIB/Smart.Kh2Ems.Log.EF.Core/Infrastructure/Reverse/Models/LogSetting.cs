using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.Log.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 로그 설정 테이블
/// </summary>
public partial class LogSetting
{
    /// <summary>
    /// 기본아이디
    /// </summary>
    public int Idx { get; set; }

    /// <summary>
    /// 설비 종류 ID
    /// </summary>
    public int CeqTypeId { get; set; }

    /// <summary>
    /// 동적 인덱스 ID
    /// </summary>
    public int? DynamicIndex { get; set; }

    /// <summary>
    /// 보고서저장여부
    /// </summary>
    public byte? IsReportSave { get; set; }
}
