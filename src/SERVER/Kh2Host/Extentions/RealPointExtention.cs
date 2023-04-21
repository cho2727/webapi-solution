using Kh2Host.Enums;
using Kh2Host.Models;
using Smart.Kh2Ems.Infrastructure.Models;
using Smart.Kh2Ems.Infrastructure.Const;
using Smart.PowerCUBE.Api;
using Smart.PowerCUBE.Api.DataModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Smart.Kh2Ems.Infrastructure.Enums;

namespace Kh2Host.Extentions;

public static class RealPointExtention
{
    public static void BinaryRealPointProc(this RealPointProcModel self, BinaryProcModel model, List<StateValueModel>? stateValues, List<EventBodyData> alarms, List<UpdatePointValue> dbupdates, ushort newTlqValue)
    {
        var oldPointValue = self.OldRealData.PointData.FirstOrDefault(pd => pd.DataTypeName == model.PointIndex.EName);
        if (oldPointValue == null)
        {
            // 에러 로그 출력
            return;
        }
        var oldPointTLQ = self.OldRealData.PointData.FirstOrDefault(pd => pd.DataTypeName == $"{model.PointIndex.EName}_{ConstDefine.RPTagString}");
        ushort oldTlqValue = ushort.Parse(oldPointTLQ?.DataValue ?? "0");
        byte oldValue = byte.Parse(oldPointValue?.DataValue ?? "0");
        if (!SettingTLQValue.IsTagSet(oldTlqValue, SettingTLQValue.TagScanInhibit))  // 스켄 금지가 아니면
        {
            //var stateValues = _dbManager.stateValueModels?.Where(x => x.StateGroupID == model.PointIndex.StateGroupId);
            DateTime deviceUptime = PowerValueConvert.TimeTToDateTime(model.RecvTm).AddMilliseconds(model.RecvMilli);
            var stateValue = stateValues?.FirstOrDefault(x => x.Value == model.NewValue);
            if (stateValue == null)  // OUT RANGE TLQ 설정
            {
                newTlqValue |= SettingTLQValue.QualityOutRangeValue;
                if (!SettingTLQValue.IsTagSet(oldTlqValue, SettingTLQValue.QualityOutRangeValue))
                {
                    // 이벤트 발생
                    alarms.PointAlarmGen(AlarmTypeValue.OutofRange, model.PointIndex, self.Equipment!, model.NewValue, oldValue, oldTlqValue, deviceUptime, stateValues);
                }
            }

            if (oldValue != model.NewValue)
            {
                newTlqValue |= SettingTLQValue.QualityAlarmState;
                // 이벤트 발생
                alarms.PointAlarmGen(AlarmTypeValue.StateChange, model.PointIndex, self.Equipment!, model.NewValue, oldValue, oldTlqValue, deviceUptime, stateValues);
                self.NewRealData.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = model.PointIndex.EName, DataValue = model.NewValue.ToString() });
            }
            self.NewRealData.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = $"{model.PointIndex.EName}_{ConstDefine.RPUptimeString}", DataValue = deviceUptime.ToString("yyyy-MM-dd HH:mm:ss") });

            ushort genTlq = SettingTLQValue.TlgMaskGeneration(oldTlqValue, SettingTLQValue.TlqBinaryMask, newTlqValue);
            self.NewRealData.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = $"{model.PointIndex.EName}_{ConstDefine.RPTagString}", DataValue = genTlq.ToString() });
            dbupdates.Add(new UpdatePointValue
            {
                CeqMrid = self.Equipment!.CeqId,
                PointType = model.PointIndex.PointTypeId,
                PointIndex = model.PointIndex.PointIndex,
                DynamicIndex = model.PointIndex.DynamicIndex ?? 0,
                Value = model.NewValue,
                TagValue = (short)(genTlq >> 8),
                QualityValue = (short)(genTlq & 0x00ff),
                DeviceUptime = deviceUptime
            });
        }
    }
    public static void AnalogRealPointProc(this RealPointProcModel self, AnalogProcModel model, List<EventBodyData> alarms, List<UpdatePointValue> dbupdates, ushort newTlqValue)
    {
        var oldPointValue = self.OldRealData.PointData.FirstOrDefault(pd => pd.DataTypeName == model.PointIndex.EName);
        if (oldPointValue == null)
        {
            // 에러 로그 출력
            return;
        }
        var oldPointTLQ = self.OldRealData.PointData.FirstOrDefault(pd => pd.DataTypeName == $"{model.PointIndex.EName}_{ConstDefine.RPTagString}");
        ushort oldTlqValue = ushort.Parse(oldPointTLQ?.DataValue ?? "0");
        float oldValue = float.Parse(oldPointValue?.DataValue ?? "0");
        if (!SettingTLQValue.IsTagSet(oldTlqValue, SettingTLQValue.TagScanInhibit))  // 스켄 금지가 아니면
        {
            DateTime deviceUptime = PowerValueConvert.TimeTToDateTime(model.RecvTm).AddMilliseconds(model.RecvMilli);
            if (model.NewValue < model.PointIndex.LimitMinValue)
            {
                // 이벤트 추가
                newTlqValue |= SettingTLQValue.QualityLimitMinState;
                alarms.PointAlarmGen(AlarmTypeValue.OutofRange, model.PointIndex, self.Equipment!, model.NewValue, oldValue, oldTlqValue, deviceUptime);
            }

            if (model.NewValue > model.PointIndex.LimitMaxValue)
            {
                // 이벤트 추가
                newTlqValue |= SettingTLQValue.QualityLimitMaxState;
                alarms.PointAlarmGen(AlarmTypeValue.OutofRange, model.PointIndex, self.Equipment!, model.NewValue, oldValue, oldTlqValue, deviceUptime);
            }
            self.NewRealData.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = model.PointIndex.EName, DataValue = model.NewValue.ToString() });
            self.NewRealData.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = $"{model.PointIndex.EName}_{ConstDefine.RPUptimeString}", DataValue = deviceUptime.ToString("yyyy-MM-dd HH:mm:ss") });

            ushort genTlq = SettingTLQValue.TlgMaskGeneration(oldTlqValue, SettingTLQValue.TlqBinaryMask, newTlqValue);
            self.NewRealData.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = $"{model.PointIndex.EName}_{ConstDefine.RPTagString}", DataValue = genTlq.ToString() });
            dbupdates.Add(new UpdatePointValue { 
                CeqMrid = self.Equipment!.CeqId,
                PointType = model.PointIndex.PointTypeId,
                PointIndex = model.PointIndex.PointIndex,
                DynamicIndex = model.PointIndex.DynamicIndex ?? 0,
                Value = model.NewValue,
                TagValue = (short)(genTlq >> 8),
                QualityValue = (short)(genTlq & 0x00ff),
                DeviceUptime = deviceUptime
            });
        }
    }

    public static int AlarmDataSend(EventBodyData alarm, string cubeBoxName)
    {
        EventHeadPacket head = new EventHeadPacket
        {
            MessageCode = (ushort)MsgTypeDefine.EventAlarmData,//CubeFunctionCode.AlarmMessageCreate,
            SendTime = (uint)PowerValueConvert.ConvertToUnixTimestamp(DateTime.Now),
            RecordCount = 1,
        };
        var data = PowerValueConvert.StructToByte(alarm);
        return PowerCubeApi.Instance.SendEventMessageBox(cubeBoxName, head, data);
    }
}
