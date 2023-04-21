using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using System.Runtime.InteropServices;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
public class StationModel
{
    public long OfficeId { get; set; }

    public string OfficeName { get; set; } = null!;

    public long StationId { get; set; }

    public string StationName { get; set; } = null!;

    public int? StationTypeId { get; set; }

    public string StationTypeName { get; set; } = null!;
    public int? ModelId { get; set; }

    public string ModelName { get; set; } = null!;

    public string? StationAdder { get; set; }
}


public class StationResponseModel : BaseResponse
{
    public List<StationModel>? Datas { get; set; }
}