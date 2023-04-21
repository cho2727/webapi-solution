using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Database;

public class ProgramModel
{
    /// <summary>
    /// 프로그램 ID
    /// </summary>
    public int ProgramId { get; set; }

    /// <summary>
    /// 프로그램명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 컴퓨터 ID
    /// </summary>
    public int? ComputerId { get; set; }
    public int ComputerGroupId { get; set; }

    /// <summary>
    /// 프로그램 타입 ID
    /// </summary>
    public int? ProgramTypeId { get; set; }

    /// <summary>
    /// 프로그램번호
    /// </summary>
    public int? ProgramNo { get; set; }

    /// <summary>
    /// 실행타입(0:실행안함, 1:콘솔명령, 2:윈도우 서비스)
    /// </summary>
    public byte? ExecuteType { get; set; }

    /// <summary>
    /// 알람 우선순위 ID
    /// </summary>
    public int AlarmPriorityId { get; set; }

    /// <summary>
    /// 상태 그룹 ID
    /// </summary>
    public int? StateGroupId { get; set; }

    /// <summary>
    /// 시작 명령
    /// </summary>
    public string? StartCmd { get; set; }

    /// <summary>
    /// 종료 명령
    /// </summary>
    public string? StopCmd { get; set; }

    /// <summary>
    /// 프로그램 상태 갱신주기
    /// </summary>
    public int? UpdatePeriod { get; set; }

    /// <summary>
    /// 사용 여부
    /// </summary>
    public byte? UseFlag { get; set; }

    /// <summary>
    /// 파일이름(경로포함)
    /// </summary>
    public string? ProcFullName { get; set; }

    /// <summary>
    /// 프로그램 설명(버젼정보)
    /// </summary>
    public string? ProgramDesc { get; set; }
    public string? DpName { get; set; }

    public string DpType { get; set; } = null!;
}

public class ProgramInfoResponseModel : BaseResponse
{
    public List<ProgramModel>? Datas { get; set; }
}