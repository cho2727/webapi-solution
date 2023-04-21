using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Kh2Ems.Infrastructure.Models.MidStructs;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ComputerStateInfo
{
    /// <summary>
    /// 컴퓨터 ID
    /// </summary>
    public int ComputerId;

    /// <summary>
    /// CPU 사용률(%)
    /// </summary>
    public double CpuRate;

    /// <summary>
    /// 전체 메모리 크기(MB)
    /// </summary>
    public long MemTotal;

    /// <summary>
    /// 사용 메모리 크기(MB)
    /// </summary>
    public long MemUsage;

    /// <summary>
    /// 전체 디스크 크기(MB)
    /// </summary>
    public long DiskTotal;

    /// <summary>
    /// 사용 디스크 크기(MB)
    /// </summary>
    public long DiskUsage;

    /// <summary>
    /// 상태
    /// </summary>
    public byte Status;

    /// <summary>
    /// 활성화 상태(Active)
    /// </summary>
    public byte ActiveState;

    /// <summary>
    /// 갱신 시간
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string UpdateTime;

    public int GetSize()
    {
        return Marshal.SizeOf(this);
    }
}