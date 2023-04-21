using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Database;


public class CompueterModel
{
    /// <summary>
    /// 컴퓨터 ID
    /// </summary>
    public int ComputerId { get; set; }

    /// <summary>
    /// 컴퓨터명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 컴퓨터 그룹ID
    /// </summary>
    public int ComputerGroupId { get; set; }
    public string GroupName { get; set; } = null!;

    public byte? DupType { get; set; }
    /// <summary>
    /// 알람 우선순위 ID
    /// </summary>
    public int? AlarmPriorityFk { get; set; }

    /// <summary>
    /// 상태 그룹 ID
    /// </summary>
    public int? StateGroupFk { get; set; }

    /// <summary>
    /// 지역 코드
    /// </summary>
    public long? MemberOfficeFk { get; set; }

    /// <summary>
    /// 사용 여부
    /// </summary>
    public byte? UseFlag { get; set; }
    public string? DpName { get; set; }

    public string DpType { get; set; } = null!;
}
public class ComputerInfoResponseModel : BaseResponse
{
    public List<CompueterModel>? Datas { get; set; }
}