using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

public partial class PointIndexView
{
    public int PointTypeId { get; set; }

    public int? DynamicIndex { get; set; }

    public string? Name { get; set; }

    public string EName { get; set; } = null!;

    public int? AlarmPriorityId { get; set; }

    public int? DataTypeId { get; set; }
}
