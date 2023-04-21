using Smart.Kh2Ems.Infrastructure.Enums;

namespace Smart.Kh2Ems.Infrastructure.Const;

public class ConstDefine
{
    public const string RPUptimeString = "uptime";
    public const string RPTagString = "tlq";
    public const string BIUpdateTimePointName = "bi_tm";
    public const string AIUpdateTimePointName = "ai_tm";
    public const string AOUpdateTimePointName = "ao_tm";
    public const string CounterUpdateTimePointName = "cnt_tm";

    public const string CommStatePointName = "comm_state";
    public const string CommTotalPointName = "comm_total_cnt";
    public const string CommSuccPointName = "comm_sucess_cnt";
    public const string CommFailPointName = "comm_fail_cnt";
    public const string CommNoResponsePointName = "comm_no_response_cnt";


    public static DateTime TimeTToDateTime(uint time_t)
    {
        long win32FileTime = 10000000 * (long)time_t + 116444736000000000;
        //return DateTime.FromFileTimeUtc(win32FileTime);
        return DateTime.FromFileTime(win32FileTime);
    }

    public static string GetUpdateTimeString(RealPointType type)
    {
        switch(type)
        {
            case RealPointType.BI:
                return BIUpdateTimePointName;
            case RealPointType.AI:
                return AIUpdateTimePointName;
            case RealPointType.AO:
                return AOUpdateTimePointName;
            case RealPointType.COUNTER:
                return CounterUpdateTimePointName;
        }
        return string.Empty;
    }
}
