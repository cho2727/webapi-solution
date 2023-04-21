using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;


public class AgentCommandModel
{
    public string ProcFullName { get; set; } = string.Empty;
    public string ProcArgs { get; set; } = string.Empty;
    public int WindowStyle { get; set; }
    public int IsObservation { get; set; }  // 0:감시 안함 1:감시함(리스트 추가)
    public string Description { get; set; } = string.Empty;
}


public class AgentCommandRequestModel : BaseControlMessage
{
    public List<AgentCommandModel>? Commands { get; set; }
}

public class AgentCommandResponseModel : AgentCommandRequestModel
{
    public bool Result { get; set; }
    public Error? Error { get; set; }// = new Error { Code = "00", Message = "정상처리" };
}
