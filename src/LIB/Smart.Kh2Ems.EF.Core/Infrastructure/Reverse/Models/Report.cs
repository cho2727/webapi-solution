using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 레포트
/// </summary>
public partial class Report
{
    /// <summary>
    /// 레포트 ID
    /// </summary>
    public int ReportId { get; set; }

    /// <summary>
    /// 레포트명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 파일 이름
    /// </summary>
    public string? FileName { get; set; }

    /// <summary>
    /// 레포트 엑셀 파일
    /// </summary>
    public byte[]? ReportExcelFile { get; set; }

    /// <summary>
    /// 레포트 PDF 파일
    /// </summary>
    public byte[]? ReportPdfFile { get; set; }

    /// <summary>
    /// 레포트 폼 ID
    /// </summary>
    public int? FormFk { get; set; }

    /// <summary>
    /// 생성 시간
    /// </summary>
    public DateTime? CreateTime { get; set; }

    public virtual ReportForm? FormFkNavigation { get; set; }
}
