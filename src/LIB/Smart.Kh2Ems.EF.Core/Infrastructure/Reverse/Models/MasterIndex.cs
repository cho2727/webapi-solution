using System;
using System.Collections.Generic;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

/// <summary>
/// 마스터 인덱스
/// </summary>
public partial class MasterIndex
{
    /// <summary>
    /// 마스터 인덱스 ID
    /// </summary>
    public int MasterIndexId { get; set; }

    /// <summary>
    /// 설비 종류 ID
    /// </summary>
    public int CeqTypeFk { get; set; }

    /// <summary>
    /// 포인트 타입
    /// </summary>
    public int PointTypeFk { get; set; }

    /// <summary>
    /// 포인트 타입별 인덱스 ID
    /// </summary>
    public int? IndexFk { get; set; }

    /// <summary>
    /// 회로 번호
    /// </summary>
    public int? CircuitNo { get; set; }

    /// <summary>
    /// 프로토콜 의존적인 Remote Address 정보
    /// </summary>
    public string? RemoteAddress { get; set; }

    /// <summary>
    /// 모드버스ADDR
    /// </summary>
    public int? ModbusAddress { get; set; }

    /// <summary>
    /// 비트 포지션
    /// </summary>
    public byte? BitPosition { get; set; }

    /// <summary>
    /// 알람 우선순위 ID
    /// </summary>
    public int? AlarmPriorityFk { get; set; }

    /// <summary>
    /// 데이터 타입 ID
    /// </summary>
    public int? DataTypeFk { get; set; }

    /// <summary>
    /// 상태 그룹 ID
    /// </summary>
    public int? StateGroupFk { get; set; }

    /// <summary>
    /// 스케일 펙터 ID
    /// </summary>
    public int? ScaleFactorFk { get; set; }

    /// <summary>
    /// 단위 ID
    /// </summary>
    public int? UnitFk { get; set; }

    /// <summary>
    /// deadband 값
    /// </summary>
    public double? Deadband { get; set; }

    /// <summary>
    /// 클래스번호_REG_TYPE
    /// </summary>
    public int? ClassNo { get; set; }

    /// <summary>
    /// object_variation
    /// </summary>
    public string? ObjVar { get; set; }

    /// <summary>
    /// 최소 LIMIT 값
    /// </summary>
    public double? LimitMinValue { get; set; }

    /// <summary>
    /// 최대 LIMIT 값
    /// </summary>
    public double? LimitMaxValue { get; set; }

    /// <summary>
    /// AO 포인트 설정 기본값
    /// </summary>
    public double? DefaultVal { get; set; }

    /// <summary>
    /// AO 포인트 설정 STEP 값
    /// </summary>
    public double? StepVal { get; set; }

    /// <summary>
    /// AO 포인트 설정 최소 값
    /// </summary>
    public double? MinVal { get; set; }

    /// <summary>
    /// AO 포인트 설정 최대 값
    /// </summary>
    public double? MaxVal { get; set; }

    /// <summary>
    /// AO 포인트 설정 OFF 존재 여부
    /// </summary>
    public byte? ExistOff { get; set; }

    /// <summary>
    /// AO 포인트 설정 OFF 값
    /// </summary>
    public double? OffVal { get; set; }

    public virtual AlarmPriority? AlarmPriorityFkNavigation { get; set; }

    public virtual CeqType CeqTypeFkNavigation { get; set; } = null!;

    public virtual DataType? DataTypeFkNavigation { get; set; }

    public virtual PointType PointTypeFkNavigation { get; set; } = null!;

    public virtual ScaleFactor? ScaleFactorFkNavigation { get; set; }

    public virtual StateGroup? StateGroupFkNavigation { get; set; }

    public virtual Unit? UnitFkNavigation { get; set; }
}
