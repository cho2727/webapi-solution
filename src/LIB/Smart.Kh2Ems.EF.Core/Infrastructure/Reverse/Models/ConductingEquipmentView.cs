using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

public partial class ConductingEquipmentView
{
    public long CeqId { get; set; }

    public string CeqName { get; set; } = null!;

    public long? OfficeCode { get; set; }

    public long? StationMrfk { get; set; }

    public string StationName { get; set; } = null!;

    public string? StnTypeCode { get; set; }

    public int ObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;

    public string? ObjectTypeEname { get; set; }

    public string? TypeCode { get; set; }

    public int? ModelId { get; set; }

    public string ModelName { get; set; } = null!;

    public int? CeqTypeFk { get; set; }

    public int? DeviceFk { get; set; }

    public byte? CircuitNo { get; set; }

    public string? CeqAliasName { get; set; }

    public string? DpName { get; set; }

    public string DpType { get; set; } = null!;
}
