using Smart.Kh2Ems.Infrastructure.Enums;

namespace Kh2Historian.Models;

public class LogSaveActionModel
{
    public SaveLogType SLogType { get; set; }
    public DateTime NextSaveTime { get; set; }
}
