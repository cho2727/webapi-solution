using Smart.Kh2Ems.Infrastructure.Enums;

namespace Smart.Kh2Ems.Infrastructure.Helpers;

public class DateTimeHelper
{
    public static DateTime GetNextDateTime(DateTime now, SaveLogType logType, int day = 1, int hour = 0, int min = 0, int sec = 0   )
    {
        DateTime date = DateTime.Now;
        //int addMin = (60 - now.Minute + min) % 60;
        //int addHour = (24 - now.Hour + hour) % 24;
        //if (logType == SaveLogType.None)
        //{
        //    int addSec = Math.Abs((60 - now.Second + sec) % 60);
        //    date = now.AddSeconds(addSec);
        //}

        switch (logType)
        {
            case SaveLogType.None:
                
                break;
            case SaveLogType.Minute:
                {
                    int addMin = min - (now.Minute % min);
                    DateTime mindate = now.AddMinutes(addMin);
                    date = new DateTime(mindate.Year, mindate.Month, mindate.Day, mindate.Hour, mindate.Minute, sec);
                }
                break;
            case SaveLogType.Hour:
                {
                    date = new DateTime(now.Year, now.Month, now.Day, now.Hour, min, sec);
                    if (now > date)
                        date = date.AddHours(1);
                }
                break;
            case SaveLogType.Day:
                {
                    date = new DateTime(now.Year, now.Month, now.Day, hour, min, sec);
                    if (now > date)
                        date = date.AddDays(1);
                }
                break;
            case SaveLogType.Month:
                {
                    date = new DateTime(now.Year, now.Month, day, hour, min, sec);
                    if (now > date)
                        date = date.AddMonths(1);
                }
                break;
            default:
                break;
        }

        return date;
    }

}
