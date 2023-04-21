namespace Kh2Host.Models;

public class ComputerInfoDataModel
{
    public int ComputerId { get; set; }

    public string Name { get; set; } = null!;

    public int? AlarmPriorityFk { get; set; }

    public int? StateGroupFk { get; set; }

    public long? MemberOfficeFk { get; set; }

    public string? OsName { get; set; }

    public string? OsVersion { get; set; }

    public byte? UseFlag { get; set; }

    public string? DpName { get; set; }

    public string DpType { get; set; } = null!;
}
