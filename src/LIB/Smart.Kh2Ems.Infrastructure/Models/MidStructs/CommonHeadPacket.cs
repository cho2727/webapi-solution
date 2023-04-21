using System.Runtime.InteropServices;

namespace Smart.Kh2Ems.Infrastructure.Models.MidStructs;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CommonHeadPacket
{
    public int MsgType;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string CubeBoxName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string RequestProcessName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string SendTime;

    public int GetSize()
    {
        return Marshal.SizeOf(this);
    }
}