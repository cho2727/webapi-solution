namespace Kh2Historian.Models;

public class LogSettingInfo
{
    public int CeqTypeId { get; set; }

    /// <summary>
    /// 동적 인덱스 ID
    /// </summary>
    public int? DynamicIndex { get; set; }

    public string DataTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 보고서저장여부
    /// </summary>
    public byte? IsReportSave { get; set; }
}
