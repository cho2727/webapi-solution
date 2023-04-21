using System.Diagnostics;

namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

public class BaseMessageHeader
{
    public int MsgType { get; set; }
    public string RequestProcessName { get; set; } = string.Empty;
    public string SendTime { get; set; } = string.Empty;
    public string CubeBoxName { get; set; } = string.Empty;
}

public class BaseUserMessageHeader : BaseMessageHeader
{
    public string? UserId { get; set; }
}


public class BaseControlMessage
{
    public BaseMessageHeader Header { get; set; } = null!;
}

public class BaseAsyncMessage : BaseControlMessage
{
}

public class BaseEventMessage : BaseControlMessage
{
}

public class BaseUserControlMessage
{
    public BaseUserMessageHeader? Header { get; set; }
}

