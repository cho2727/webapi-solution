using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

public partial class DevicePointIndexView
{
    public int CeqTypeId { get; set; }

    public string CeqTypeName { get; set; } = null!;

    public int PointTypeId { get; set; }

    public int? PointIndex { get; set; }

    public int? DynamicIndex { get; set; }

    public string? Name { get; set; }

    public string EName { get; set; } = null!;

    public int? CircuitNo { get; set; }

    public string? RemoteAddress { get; set; }

    public int? ClassNo { get; set; }

    public int? ModbusAddress { get; set; }

    public int? BitPosition { get; set; }

    public int? AlarmPriority { get; set; }

    public int? DataTypeId { get; set; }

    public int? StateGroupId { get; set; }

    public double? Scale { get; set; }

    public double? Offset { get; set; }

    public double? LimitMinValue { get; set; }

    public double? LimitMaxValue { get; set; }

    public int? UnitId { get; set; }
}
