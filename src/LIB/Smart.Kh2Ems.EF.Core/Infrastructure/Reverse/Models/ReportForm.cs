using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 레포트 폼
/// </summary>
public partial class ReportForm
{
    /// <summary>
    /// 레포트 폼 ID
    /// </summary>
    public int FormId { get; set; }

    /// <summary>
    /// 레포트 폼명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 파일 이름
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// 레포트 폼 타입 ID
    /// </summary>
    public int FormTypeFk { get; set; }

    /// <summary>
    /// 레포트 폼 파일
    /// </summary>
    public byte[] FormFile { get; set; } = null!;

    /// <summary>
    /// 설명
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 생성 시간
    /// </summary>
    public DateTime CreateTime { get; set; }

    public virtual ReportFormType FormTypeFkNavigation { get; set; } = null!;

    public virtual ICollection<Report> Reports { get; } = new List<Report>();
}
