using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

public partial class CeqPointIndexView
{
    public int CeqTypeId { get; set; }

    public string CeqTypeName { get; set; } = null!;

    public int ModelId { get; set; }

    public int PointTypeId { get; set; }

    public int? PointIndex { get; set; }

    public int? DynamicIndex { get; set; }

    public string? Name { get; set; }

    public string EName { get; set; } = null!;

    public int? AlarmPriority { get; set; }

    public int? DataTypeId { get; set; }

    public int? StateGroupId { get; set; }

    public double? LimitMinValue { get; set; }

    public double? LimitMaxValue { get; set; }

    public int? UnitId { get; set; }

    public string? UnitName { get; set; }
}
