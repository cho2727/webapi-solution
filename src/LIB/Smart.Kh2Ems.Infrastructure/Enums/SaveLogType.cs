namespace Smart.Kh2Ems.Infrastructure.Enums;

public enum SaveLogType
{
    None = 0,
    Minute,     // 분별
    Hour,       // 분별 + 시별
    Day,        // 분별 + 시별 + 일별
    Month,      // 분별 + 시별 + 일별 + 월별
}
