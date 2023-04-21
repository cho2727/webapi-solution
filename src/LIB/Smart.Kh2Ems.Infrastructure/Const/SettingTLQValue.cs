namespace Smart.Kh2Ems.Infrastructure.Const;

public class SettingTLQValue
{
    public const ushort Normal = 0x0000;
    public const ushort TagControlInhibit = 0x0001;
    public const ushort TagScanInhibit = 0x0002;
    public const ushort TagAlarmInhibit = 0x0004;
    public const ushort TagEventInhibit = 0x0008;
    public const ushort TagConstruction = 0x0010;

    public const ushort TagLimitMax = 0x0020;
    public const ushort TagLimitMin = 0x0040;

    public const ushort QualityControlProgress = 0x0100;
    public const ushort QualityInitialValue = 0x0200;
    public const ushort QualityOffLineValue = 0x0400;
    public const ushort QualityOutRangeValue = 0x0800;      // BI 상태값이 없을때
    public const ushort QualityLimitMaxState = 0x1000;      // 
    public const ushort QualityLimitMinState = 0x2000;
    public const ushort QualityManualEntered = 0x4000;
    public const ushort QualityAlarmState = 0x8000;

    public const ushort TlqTagMask = TagControlInhibit | TagScanInhibit | TagAlarmInhibit | TagEventInhibit | TagConstruction;
    public const ushort TlqLimitMask = QualityLimitMaxState | QualityLimitMinState ;
    public const ushort TlqQualityMask = QualityControlProgress | QualityInitialValue | QualityOffLineValue | QualityOutRangeValue | QualityManualEntered | QualityAlarmState;

    public const ushort TlqBinaryMask = QualityInitialValue | QualityManualEntered | QualityOffLineValue | QualityAlarmState | QualityOutRangeValue;
    public const ushort TlqAnalogMask = QualityInitialValue | QualityManualEntered | QualityOffLineValue | QualityAlarmState | QualityOutRangeValue | TlqLimitMask;

    public const ushort PointOfflineValue = 0x00;
    public const ushort PointOnlineValue = 0x01;

    public static bool IsTagSet(ushort value, ushort tagValue)
    {
        return ((value) & tagValue) != 0;
    }

    public static ushort TlgMaskGeneration(ushort oldValue, ushort maskValue, ushort newValue)
    {
        return (ushort)(((maskValue) & (newValue)) | (~(maskValue) & (oldValue)));
    }
}
