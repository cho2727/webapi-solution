using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using System.Runtime.InteropServices;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
public class ModelIndexModel
{
    public int ModelId { get; set; }
    public int DdataId { get; set; }
    public string? MidName { get; set; }
    public string? PointName { get; set; }
    public double? LowValue { get; set; }
    public double? HighValue { get; set; }
    public string? Unit { get; set; }
}


public class ModelIndexResponseModel : BaseResponse
{
    public List<ModelIndexModel>? Datas { get; set; } = null;
}