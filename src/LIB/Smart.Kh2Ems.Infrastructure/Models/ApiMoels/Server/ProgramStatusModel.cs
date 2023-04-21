using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;


public class ProgramStatusModel
{
    /// <summary>
    /// 프로그램 ID
    /// </summary>
    public int ProgramId { get; set; }

    /// <summary>
    /// 상태
    /// </summary>
    public byte Status { get; set; }

    /// <summary>
    /// 갱신시간
    /// </summary>
    public string UpdateTime { get; set; } = null!;

    /// <summary>
    /// 시작시간
    /// </summary>
    public string? StartTime { get; set; }

    /// <summary>
    /// 종료시간
    /// </summary>
    public string? EndTime { get; set; }
}

public class ProgramStatusRequestModel
{
    /// <summary>
    /// 프로그램 ID
    /// </summary>
    public int ProgramId { get; set; }
}


public class ProgramStatusSendRequestModel : BaseAsyncMessage
{
    public List<ProgramStatusModel>? Datas { get; set; }
}

public class ProgramStatusResponseModel : ProgramStatusSendRequestModel
{
    public bool Result { get; set; }
    public Error? Error { get; set; }// = new Error { Code = "00", Message = "정상처리" };
}