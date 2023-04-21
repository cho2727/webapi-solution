using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using System.Runtime.InteropServices;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
public class EquipmentModel
{
    public long CeqId { get; set; }

    public string CeqName { get; set; } = null!;

    public long? OfficeCode { get; set; }

    public long? StationId { get; set; }

    public string StationName { get; set; } = null!;

    public int ObjectType { get; set; }

    public string ObjectTypeName { get; set; } = null!;
    public string? DataTypeName { get; set; }

    public int? ModelId { get; set; }

    public string ModelName { get; set; } = null!;

    public int? CeqType { get; set; }

    public byte? CircuitNo { get; set; }
}

public class EquipmentResponseModel : BaseResponse
{
    public List<EquipmentModel>? Datas { get; set; } = null;
}