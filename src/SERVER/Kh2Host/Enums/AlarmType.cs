namespace Kh2Host.Enums;

public enum AlarmTypeValue
{
    None = 0,
    StateChange = 1,
    MinLimitWrong,
    MaxLimitWrong,
    OutofRange,
    UnsolEvent,
    ProgramStateChange = 11,
    ComputerStateChange = 12,
}
