using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Smart.Kh2Ems.Infrastructure.resources;

[SupportedOSPlatform("windows")]
public class ResourceCheck
{
    private const int NETWROK_LAN = 0x01;
    private const int NETWROK_WAN = 0x02;
    private const int NETWROK_AOL = 0x04;

    [DllImport("sensapi.dll")] private static extern bool IsNetworkAlive(out int flags);
    //Declare external functions
    //[DllImport("winInet.dll ")] private static extern bool InternetGetConnectedState(out int flag, int dwReserved);
    [DllImport("winInet.dll ")] private static extern bool InternetGetConnectedState(ref int flag, int dwReserved);

    public class UsageInfo
    {
        /// <summary>
        /// 전체 용량
        /// </summary>
        public ulong TotalSize { get; set; }

        /// <summary>
        /// 남은 용량
        /// </summary>
        public ulong FreeSize { get; set; }

        /// <summary>
        /// (readonly) 사용량
        /// </summary>
        public ulong UsedSize => TotalSize - FreeSize;

        /// <summary>
        /// (readonly) 사용률 
        /// </summary>
        public double Usage => ((double)(UsedSize) / (double)TotalSize) * 100;
    }

    public static TimeSpan GetComputerStartTime()
    {
        using (var uptime = new PerformanceCounter("System", "System Up Time"))
        {
            uptime.NextValue();       //Call this an extra time before reading its value
            return TimeSpan.FromSeconds(uptime.NextValue());
        }
    }

    public static double GetTotalCpuUsage()
    {
        try
        {
            var wmi = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor where Name != '_Total'");
            var cpuUsages = wmi.Get().Cast<ManagementObject>().Select(mo => (long)(ulong)mo["PercentProcessorTime"]);
            var totalUsage = cpuUsages.Average();

            return (double)totalUsage;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public static UsageInfo? GetMemoryUsage()
    {
        try
        {
            var wmi = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            var info = wmi.Get().Cast<ManagementObject>().Select(mo => new UsageInfo()
            {
                TotalSize = ulong.Parse(mo["TotalVisibleMemorySize"].ToString()),
                FreeSize = ulong.Parse(mo["FreePhysicalMemory"].ToString()),
            }).FirstOrDefault();

            return info;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static UsageInfo? GetHddUsage()
    {
        try
        {
            var driveName = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory);
            var d = new DriveInfo(driveName!);
            var info = new UsageInfo()
            {
                TotalSize = (ulong)d.TotalSize,
                FreeSize = (ulong)(d.TotalFreeSpace)
            };

            return info;
        }
        catch (Exception)
        {
            return null;
        }
    }


    /// <summary>
    /// PING 체크
    /// </summary>
    /// <param name="addr"></param>
    /// <returns></returns>
    public static bool IsCheckNetwork(string addr)
    {
        bool networkState = NetworkInterface.GetIsNetworkAvailable();
        bool pingResult = true;

        //네트워크가 연결이 되어있다면
        if (networkState)
        {
            Ping pingSender = new Ping();

            //Ping 체크 (IP, TimeOut 지정)
            PingReply reply = pingSender.Send(addr, 300);

            //상태가 Success이면 true반환
            pingResult = reply.Status == IPStatus.Success;
        }

        return networkState & pingResult;
    }

    public static bool IsNetworkConnectedState()
    {
        int flags = NETWROK_LAN;
        return InternetGetConnectedState(ref flags, 0);
    }

}
