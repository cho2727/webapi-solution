using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Kh2Ems.Infrastructure.Models.MidStructs;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ProgramStateInfo
{
    /// <summary>
    /// 프로그램 ID
    /// </summary>
    public int ProgramId;

    /// <summary>
    /// 상태
    /// </summary>
    public byte Status;

    /// <summary>
    /// 갱신시간
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string UpdateTime;

    /// <summary>
    /// 시작시간
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string StartTime;

    /// <summary>
    /// 종료시간
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string EndTime;

    public int GetSize()
    {
        return Marshal.SizeOf(this);
    }
}