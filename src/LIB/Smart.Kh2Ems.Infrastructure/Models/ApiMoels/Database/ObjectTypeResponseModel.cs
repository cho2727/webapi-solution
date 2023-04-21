using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using System.Runtime.InteropServices;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
public class ObjectTypeModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string EName { get; set; } = null!;
}


public class ObjectTypeResponseModel : BaseResponse
{
    public List<ObjectTypeModel>? Datas { get; set; } = null;
}