using Kh2Host.Enums;
using Kh2Host.Models;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;
using Smart.Kh2Ems.Infrastructure.Models;
using Smart.Kh2Ems.Infrastructure.Const;
using Smart.Kh2Ems.Infrastructure.Enums;
using Smart.PowerCUBE.Api;
using Smart.PowerCUBE.Api.DataModels;
using System.Diagnostics;
using System.Security.Claims;

namespace Kh2Host.Extentions;

public static class AlarmGenerator
{
    private static int alarmCreateNumber = 0;
    private static readonly object syncLock = new object();

    public static long CreateAlarmID()
    {
        long eventID = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss000"));
        lock (syncLock)
        {
            alarmCreateNumber++;

            if (alarmCreateNumber > 255)
                alarmCreateNumber = 1;

            eventID += alarmCreateNumber;
        }

        return eventID;
    }

    public static void PointAlarmGen(this List<EventBodyData> self, AlarmTypeValue alarmType
        , PointIndexModel pointInfo, ConductingEquipmentModel sw
        , float newValue, float oldValue, ushort pointTlq, DateTime deviceTime
        , List<StateValueModel>? stateValues = null, ushort rtuTlq = 0)
    {
        string construction = string.Empty;

        if (alarmType == AlarmTypeValue.None)      // 알람 발생 안함.
            return;

        if (SettingTLQValue.IsTagSet(rtuTlq, SettingTLQValue.TagEventInhibit) || SettingTLQValue.IsTagSet(pointTlq, SettingTLQValue.TagEventInhibit))
            return;

        if (SettingTLQValue.IsTagSet(rtuTlq, SettingTLQValue.TagAlarmInhibit) || SettingTLQValue.IsTagSet(pointTlq, SettingTLQValue.TagAlarmInhibit))
            return;

        string newStatemsg = string.Empty;
        string oldStatemsg = string.Empty;

        if (pointInfo.PointTypeId == (int)RealPointType.BI || pointInfo.PointTypeId == (int)RealPointType.CALBI)
        {
            if (stateValues != null)
            {
                var newStateSet = stateValues.FirstOrDefault(x => x.Value == (byte)newValue);
                var oldStateSet = stateValues.FirstOrDefault(x => x.Value == (byte)oldValue);

                newStatemsg = newStateSet?.Name ?? $"{newValue}:unknown";
                oldStatemsg = oldStateSet?.Name ?? $"{oldValue}:unknown";
            }
            else
            {
                newStatemsg = $"{newValue}:unknown";
                oldStatemsg = $"{oldValue}:unknown";
            }
        }
        else // 아날로그일시
        {
            newStatemsg = string.Format("{0:F2}", newValue);
            oldStatemsg = string.Format("{0:F2}", oldValue);
        }

        string alarmMessage = string.Empty;
        switch (alarmType)
        {
            case AlarmTypeValue.StateChange:
                alarmMessage = $"[{pointInfo.Name}] {oldStatemsg} → {newStatemsg} 상태값 변경";
                break;
            case AlarmTypeValue.MinLimitWrong:
                alarmMessage = $"[{pointInfo.Name}] {oldStatemsg} → {newStatemsg} 최소값 미만";
                break;
            case AlarmTypeValue.MaxLimitWrong:
                alarmMessage = $"[{pointInfo.Name}] {oldStatemsg} → {newStatemsg} 최대값 초과";
                break;
            case AlarmTypeValue.OutofRange:
                alarmMessage = $"[{pointInfo.Name}] {oldStatemsg} → {newStatemsg} 상태값 범위 이상";
                break;
            case AlarmTypeValue.UnsolEvent:
                alarmMessage = $"[{pointInfo.Name}] {oldStatemsg} → {newStatemsg} UNSOL 알람 발생";
                break;
        }

        if (string.IsNullOrEmpty(alarmMessage))
            return;

        self.Add(new EventBodyData
        {
            EventId = CreateAlarmID(),
            EventType = (int)alarmType,
            MemberOffice = sw.OfficeCode ?? 0,
            StationId = sw.StationMrfk ?? 0,
            DeviceId = sw.DeviceFk ?? 0,
            CeqId = sw.CeqId,
            CeqTypeId = sw.CeqTypeFk ?? 0,
            DdataId = pointInfo.DynamicIndex ?? 0,
            PointType  = (byte)pointInfo.PointTypeId,
            PointIndex = pointInfo.PointIndex ?? 0,
            CircuitNo = sw.CircuitNo ?? 0,
            AlarmPriority = pointInfo.AlarmPriority ?? 0,
            TagValue = pointTlq,
            OldValue = oldValue,
            NewValue = newValue,
            DeviceEventTime = deviceTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),//PowerValueConvert.TimeTToDateTime(alarm.soeTimeS).AddMilliseconds(alarm.soeTimeMS).ToString("yyyy-MM-dd HH:mm:ss.fff"),
            EventCreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            EventMsg = alarmMessage
        });
    }

    public static void ComputerAlarmGen(this List<EventBodyData> self
        , AlarmTypeValue alarmType, ComputerInfoDataModel computer
        , float newValue, float oldValue, string deviceTime, List<StateValueModel>? stateValues = null)
    {
        string newStatemsg = $"{newValue}:unknown";
        string oldStatemsg = $"{oldValue}:unknown";
        if (stateValues != null)
        {
            var newStateSet = stateValues.FirstOrDefault(x => x.Value == (byte)newValue);
            var oldStateSet = stateValues.FirstOrDefault(x => x.Value == (byte)oldValue);

            newStatemsg = newStateSet?.Name ?? $"{newValue}:unknown";
            oldStatemsg = oldStateSet?.Name ?? $"{oldValue}:unknown";
        }

        string alarmMessage = $"[{computer.Name}] {oldStatemsg} → {newStatemsg} 프로그램 상태 변경 발생";
        if (string.IsNullOrEmpty(alarmMessage))
            return;

        self.Add(new EventBodyData
        {
            EventId = CreateAlarmID(),
            EventType = (int)alarmType,
            MemberOffice = computer.MemberOfficeFk ?? 0,
            StationId = computer.ComputerId,
            DeviceId = computer.ComputerId,
            CeqId = 0,
            CeqTypeId = 0,
            DdataId = 0,
            PointType = 0,
            PointIndex = 0,
            CircuitNo = 0,
            AlarmPriority = computer.AlarmPriorityFk ?? 0,
            TagValue = 0,
            OldValue = oldValue,
            NewValue = newValue,
            DeviceEventTime = deviceTime,//PowerValueConvert.TimeTToDateTime(alarm.soeTimeS).AddMilliseconds(alarm.soeTimeMS).ToString("yyyy-MM-dd HH:mm:ss.fff"),
            EventCreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            EventMsg = alarmMessage
        });

    }

    public static void ProgramAlarmGen(this List<EventBodyData> self
    , AlarmTypeValue alarmType, ProgramInfoDataModel program, long memberOfficeId
    , float newValue, float oldValue, string deviceTime, List<StateValueModel>? stateValues = null)
    {
        string newStatemsg = $"{newValue}:unknown";
        string oldStatemsg = $"{oldValue}:unknown";
        if (stateValues != null)
        {
            var newStateSet = stateValues.FirstOrDefault(x => x.Value == (byte)newValue);
            var oldStateSet = stateValues.FirstOrDefault(x => x.Value == (byte)oldValue);

            newStatemsg = newStateSet?.Name ?? $"{newValue}:unknown";
            oldStatemsg = oldStateSet?.Name ?? $"{oldValue}:unknown";
        }

        string alarmMessage = $"[{program.Name}] {oldStatemsg} → {newStatemsg} 프로그램 상태 변경 발생";
        if (string.IsNullOrEmpty(alarmMessage))
            return;

        self.Add(new EventBodyData
        {
            EventId = CreateAlarmID(),
            EventType = (int)alarmType,
            MemberOffice = memberOfficeId,
            StationId = program.ComputerFk ?? 0,
            DeviceId = program.ProgramId,
            CeqId = 0,
            CeqTypeId = 0,
            DdataId = 0,
            PointType = 0,
            PointIndex = 0,
            CircuitNo = 0,
            AlarmPriority = program.AlarmPriorityFk,
            TagValue = 0,
            OldValue = oldValue,
            NewValue = newValue,
            DeviceEventTime = deviceTime,//PowerValueConvert.TimeTToDateTime(alarm.soeTimeS).AddMilliseconds(alarm.soeTimeMS).ToString("yyyy-MM-dd HH:mm:ss.fff"),
            EventCreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            EventMsg = alarmMessage
        });

    }

}
