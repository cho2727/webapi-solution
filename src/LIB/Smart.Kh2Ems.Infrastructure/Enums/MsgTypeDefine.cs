namespace Smart.Kh2Ems.Infrastructure.Enums;

public enum MsgTypeDefine
{
    None = 0,
    ProcessStartCommand = 1,
    ProcessEndCommand = 2,
    TotalProcessEndCommand = 3,

    EventAlarmData = 101,

    ComputerStateUpdateRequest = 201,
    ProgramStateUpdateRequest = 202
}
