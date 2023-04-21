using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

namespace Kh2RealMaker.Models;


public class RealPointMapDataModel
{
    public int RealMapID { get; set; }
    public string RealMapName { get; set; }
    public string RealTypeName { get; set; }
}

public class RealPointIndexDataModel
{
    public int RealMapID { get; set; }
    public int PointType { get; set; }
    public int DynamicIndex { get; set; }
    public string RemoteAddress { get; set; }
    public int CircuitNo { get; set; }
    public string MidName { get; set; }
}

public class CommonIndexDataModel
{
    /// <summary>
    /// 공통 인덱스ID
    /// </summary>
    public int IndexId { get; set; }

    /// <summary>
    /// 인덱스그룹
    /// </summary>
    public int? IndexGroupFk { get; set; }

    /// <summary>
    /// 상태 계측 인덱스명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 상태 계측 영문 인덱스명
    /// </summary>
    public string EName { get; set; } = null!;

    /// <summary>
    /// 데이터 타입 ID
    /// </summary>
    public int DataTypeId { get; set; }

    /// <summary>
    /// 데이터 크기
    /// </summary>
    public int Length { get; set; }
}

public partial class CommonIndexGroupDataModel
{
    /// <summary>
    /// 그룹ID
    /// </summary>
    public int IndexGroupId { get; set; }

    /// <summary>
    /// 그룹명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 영문명
    /// </summary>
    public string? EName { get; set; }

    /// <summary>
    /// 생성여부
    /// </summary>
    public byte? IsCreate { get; set; }
}
