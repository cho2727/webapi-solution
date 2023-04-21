using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

public partial class StationView
{
    public long MemberOfficeId { get; set; }

    public string OfficeName { get; set; } = null!;

    public long StationId { get; set; }

    public string StationName { get; set; } = null!;

    public int? StationTypeId { get; set; }

    public int? ModelId { get; set; }

    public string ModelName { get; set; } = null!;

    public string StationTypeName { get; set; } = null!;

    public string? StationAdder { get; set; }
}
