using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using System.Runtime.InteropServices;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
public class ModelInfoModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? CeqType { get; set; }
    public int? ObjectType { get; set; }
}


public class ModelInfoResponseModel : BaseResponse
{
    public List<ModelInfoModel>? Datas { get; set; } = null;
}