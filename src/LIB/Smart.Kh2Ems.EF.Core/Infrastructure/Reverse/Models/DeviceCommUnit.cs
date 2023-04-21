using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 원격통신기기
/// </summary>
public partial class DeviceCommUnit
{
    /// <summary>
    /// 통신기기ID
    /// </summary>
    public int DeviceId { get; set; }

    /// <summary>
    /// 이름
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 통신 타입 ID
    /// </summary>
    public int CommTypeFk { get; set; }

    /// <summary>
    /// FEP 프로그램 ID
    /// </summary>
    public int? FepId { get; set; }

    /// <summary>
    /// FRTU 주소
    /// </summary>
    public int? FrtuAddr { get; set; }

    /// <summary>
    /// 마스터주소
    /// </summary>
    public int? MasterAddr { get; set; }

    /// <summary>
    /// 원격 연결 TCP 주소
    /// </summary>
    public string? TcpAddress { get; set; }

    /// <summary>
    /// 원격 연결 TCP 포트
    /// </summary>
    public int? TcpPort { get; set; }

    /// <summary>
    /// 시뮬 연결 TCP 주소
    /// </summary>
    public string? SimulAddress { get; set; }

    /// <summary>
    /// 시뮬 연결 TCP 포트
    /// </summary>
    public int? SimulPort { get; set; }

    /// <summary>
    /// 시뮬레이터 사용여부
    /// </summary>
    public byte? SimulUsage { get; set; }

    public virtual CommType CommTypeFkNavigation { get; set; } = null!;

    public virtual ICollection<ConductingEquipment> ConductingEquipments { get; } = new List<ConductingEquipment>();

    public virtual DeviceCommConfig? DeviceCommConfig { get; set; }
}
