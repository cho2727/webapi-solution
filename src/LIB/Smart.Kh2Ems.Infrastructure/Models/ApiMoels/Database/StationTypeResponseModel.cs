using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using System.Runtime.InteropServices;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
public class StationTypeModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}


public class StationTypeResponseModel : BaseResponse
{
    public List<StationTypeModel>? Datas { get; set; } = null;
}