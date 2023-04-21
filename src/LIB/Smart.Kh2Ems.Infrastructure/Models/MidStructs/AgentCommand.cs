using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Kh2Ems.Infrastructure.Models.MidStructs;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AgentCommand
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string ProcFullName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string ProcArgs;
    public int WindowStyle;
    public int IsObservation;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string Description;

    public int GetSize()
    {
        return Marshal.SizeOf(this);
    }
}