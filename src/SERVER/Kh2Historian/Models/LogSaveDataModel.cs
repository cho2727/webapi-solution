namespace Kh2Historian.Models;

public class LogSaveDataModel
{
    public long MemberOfficeId { get; set; }

    /// <summary>
    /// 스테이션 ID
    /// </summary>
    public long StationId { get; set; }

    /// <summary>
    /// 설비 ID
    /// </summary>
    public long CeqMrid { get; set; }

    /// <summary>
    /// 설비 종류 ID
    /// </summary>
    public int CeqTypeId { get; set; }

    /// <summary>
    /// 모델 ID
    /// </summary>
    public int ModelId { get; set; }

    /// <summary>
    /// 동적 인덱스 ID
    /// </summary>
    public int DynamicIndex { get; set; }

    /// <summary>
    /// 값
    /// </summary>
    public double? Value { get; set; } = 0.0f;

    /// <summary>
    /// TAG 값
    /// </summary>
    public short? TagValue { get; set; } = 0;

    /// <summary>
    /// Quality 값
    /// </summary>
    public short? QualityValue { get; set; } = 0;

    /// <summary>
    /// 기기 업데이트 시간
    /// </summary>
    public DateTime? DeviceUptime { get; set; }

    /// <summary>
    /// 서버 저장 시간
    /// </summary>
    public DateTime? SaveTime { get; set; }
}

