using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 공통 인덱스 테이블
/// </summary>
public partial class CommonIndex
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
    public int? Length { get; set; }

    public virtual DataType DataType { get; set; } = null!;

    public virtual CommonIndexGroup? IndexGroupFkNavigation { get; set; }
}
