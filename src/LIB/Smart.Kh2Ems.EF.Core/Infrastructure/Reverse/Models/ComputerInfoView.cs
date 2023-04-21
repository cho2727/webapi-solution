using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

public partial class ComputerInfoView
{
    public int ComputerId { get; set; }

    public string Name { get; set; } = null!;

    public int ComputerGroupFk { get; set; }

    public string GroupName { get; set; } = null!;

    public byte? IsDup { get; set; }

    public int? AlarmPriorityFk { get; set; }

    public int? StateGroupFk { get; set; }

    public long? MemberOfficeFk { get; set; }

    public string? OsName { get; set; }

    public string? OsVersion { get; set; }

    public byte? UseFlag { get; set; }

    public string? DpName { get; set; }

    public string DpType { get; set; } = null!;
}
