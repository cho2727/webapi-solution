namespace Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;

public /*abstract*/ class BaseResponse
{
    public bool Result { get; set; }
    public Error? Error { get; set; }// = new Error { Code = "00", Message = "정상처리" };
}

public class Error
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
