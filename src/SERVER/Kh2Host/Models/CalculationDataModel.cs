namespace Kh2Host.Models;

public class CalculationDataModel
{
    public long CeqId { get; set; }
    /// <summary>
    /// 인덱스 ID
    /// </summary>
    public int IndexId { get; set; }

    /// <summary>
    /// 설비 종류 ID
    /// </summary>
    public int CeqTypeFk { get; set; }

    /// <summary>
    /// 포인트 타입 ID
    /// </summary>
    public int PointType { get; set; }

    /// <summary>
    /// 인덱스명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 영문 인덱스명
    /// </summary>
    public string EName { get; set; } = null!;

    /// <summary>
    /// 계산주기
    /// </summary>
    public int Period { get; set; }

    /// <summary>
    /// 계산식
    /// </summary>
    public string Formula { get; set; } = string.Empty;

    public string RealFormula { get; set; } = string.Empty;
    public string RealPointName { get; set; } = string.Empty;
    public string CalculatedValue { get; set; } = "0";

    /// <summary>
    /// 포인트 편집 시간
    /// </summary>
    public DateTime NextProcTime { get; set; }
    public CalculationEngine.Formula? FormularData { get; set; }

}
