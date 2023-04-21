namespace Smart.Kh2Ems.Infrastructure.Models;

public class StateValueModel
{
    /// <summary>
    /// 상태 그룹 ID
    /// </summary>
    public int? StateGroupID { get; set; }

    /// <summary>
    /// 상태값 변경명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 상태값
    /// </summary>
    public double? Value { get; set; }

}
