using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;


public class ComputerStatusModel
{
    /// <summary>
    /// 컴퓨터 ID
    /// </summary>
    public int ComputerId { get; set; }

    /// <summary>
    /// CPU 사용률(%)
    /// </summary>
    public double CpuRate { get; set; }

    /// <summary>
    /// 전체 메모리 크기(MB)
    /// </summary>
    public long MemTotal { get; set; }

    /// <summary>
    /// 사용 메모리 크기(MB)
    /// </summary>
    public long MemUsage { get; set; }

    /// <summary>
    /// 전체 디스크 크기(MB)
    /// </summary>
    public long DiskTotal { get; set; }

    /// <summary>
    /// 사용 디스크 크기(MB)
    /// </summary>
    public long DiskUsage { get; set; }

    /// <summary>
    /// 상태
    /// </summary>
    public byte Status { get; set; }

    /// <summary>
    /// 활성화 상태(Active)
    /// </summary>
    public byte ActiveState { get; set; }

    /// <summary>
    /// 갱신 시간
    /// </summary>
    public string UpdateTime { get; set; } = null!;
}

public class ComputerStatusRequestModel
{
    /// <summary>
    /// 컴퓨터 ID
    /// </summary>
    public int ComputerId { get; set; }

}

public class ComputerStatusSendRequestModel : BaseAsyncMessage
{
    public List<ComputerStatusModel>? Datas { get; set; }
}


public class ComputerStatusResponseModel : ComputerStatusSendRequestModel
{
    public bool Result { get; set; }
    public Error? Error { get; set; }// = new Error { Code = "00", Message = "정상처리" };
}
