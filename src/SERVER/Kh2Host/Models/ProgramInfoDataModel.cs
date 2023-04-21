﻿namespace Kh2Host.Models;

public class ProgramInfoDataModel
{
    public int ProgramId { get; set; }

    public string Name { get; set; } = null!;

    public int? ComputerFk { get; set; }

    public int? ProgramTypeFk { get; set; }

    public int? ProgramNo { get; set; }

    public byte? ExecuteType { get; set; }

    public int AlarmPriorityFk { get; set; }

    public int? StateGroupFk { get; set; }

    public string? IpAddr { get; set; }

    public int? TcpPort { get; set; }

    public string? StartCmd { get; set; }

    public string? StopCmd { get; set; }

    public int? UpdatePeriod { get; set; }

    public byte? UseFlag { get; set; }

    public string? ProcFullName { get; set; }

    public string? ProgramDesc { get; set; }

    public string? DpName { get; set; }

    public string DpType { get; set; } = null!;
}
