using Microsoft.Extensions.Configuration;
using Smart.Kh2Ems.Infrastructure.Api;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Database;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Middleware;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Server;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Shard;
using Smart.Kh2Ems.Infrastructure.Shared.Injectables;

namespace Smart.Kh2Ems.Infrastructure.WebServerApi;

public class WebApiContext : ISingletonService
{
    private readonly IConfiguration _configuration;
    public RestApiClient ApiClient { get; set; }
    public string ErrorString { get; set; } = string.Empty;
    public WebApiContext(IConfiguration configuration)
    {
        _configuration = configuration;
        var url = _configuration.GetSection("ApiSetting")?.GetValue<string>("ServerUrl") ?? "https://localhost:7199";
        ApiClient = new RestApiClient(url);
    }

    //public async Task<T?> WebApiCall<T>(string apiPath)
    //    where T : class
    //{
    //    try
    //    {
    //        var response = await ApiClient.GetAsyncHttp(apiPath);
    //        if (response != null)
    //        {
    //            var responseModel = new JsonSerializer()
    //                .JsonStringToData<T>(await response.Content.ReadAsStringAsync());

    //            return await Task.FromResult(responseModel);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
    //    }

    //    return default(T);
    //}

    public async Task<ComputerInfoResponseModel?> GetComputerInfos(int? computerId = null, string apiPath = $"api/Database/ComputerInfo")
    {
        try
        {
            if(computerId != null)
            {
                apiPath = apiPath += $"/{computerId}";
            }

            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<ComputerInfoResponseModel>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<string?> GetComputerInfoToString(int? computerId = null, string apiPath = $"api/Database/ComputerInfo")
    {
        try
        {
            if(computerId != null)
            {
                apiPath = apiPath += $"/{computerId}";
            }

            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = await response.Content.ReadAsStringAsync();

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<ProgramInfoResponseModel?> GetProgramInfos(int? programId = null, string apiPath = $"api/Database/ProgramInfo")
    {
        try
        {
            if (programId != null)
            {
                apiPath = apiPath += $"/{programId}";
            }

            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<ProgramInfoResponseModel>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<StationResponseModel?> GetStationInfos(string apiPath = $"api/Database/Stations")
    {
        try
        {
            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<StationResponseModel>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<EquipmentResponseModel?> GetEquipmentInfos(long? stationId = null, string apiPath = $"api/Database/Equipments")
    {
        try
        {
            if (stationId != null)
            {
                apiPath = apiPath += $"/{stationId}";
            }

            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<EquipmentResponseModel>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<StationTypeResponseModel?> GetStationTypes(string apiPath = $"api/Database/StationTypes")
    {
        try
        {
            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<StationTypeResponseModel>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<ObjectTypeResponseModel?> GetObjectTypes(string apiPath = $"api/Database/ObjectTypes")
    {
        try
        {
            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<ObjectTypeResponseModel>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<ModelInfoResponseModel?> GetModelInfos(string apiPath = $"api/Database/ModelInfos")
    {
        try
        {
            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<ModelInfoResponseModel>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<ModelIndexResponseModel?> GetModelIndexInfos(string apiPath = $"api/Database/ModelIndexes")
    {
        try
        {
            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<ModelIndexResponseModel>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<BaseResponse?> SendAgentCommand(AgentCommandRequestModel request, string apiPath = $"api/Server/SendAgentCommand")
    {
        try
        {
            var response = await ApiClient.PostAsyncHttp(request, apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<BaseResponse>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<AgentCommandResponseModel?> RecvAgentCommand(string controlBoxName, string apiPath = $"api/Server/RecvAgentCommand")
    {
        try
        {
            apiPath = $"{apiPath}/{controlBoxName}";

            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<AgentCommandResponseModel>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<BaseResponse?> ProgramStateUpdateCommand(ProgramStatusSendRequestModel request, string apiPath = $"api/Server/ProgramStateUpdate")
    {
        try
        {
            var response = await ApiClient.PostAsyncHttp(request, apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<BaseResponse>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<BaseResponse?> ComputerStateUpdateCommand(ComputerStatusSendRequestModel request, string apiPath = $"api/Server/ComputerStateUpdate")
    {
        try
        {
            var response = await ApiClient.PostAsyncHttp(request, apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<BaseResponse>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<AlarmResponseModel?> RecvAlarmData(string eventBoxName, string apiPath = $"api/Middleware/AlarmData")
    {
        try
        {
            apiPath = $"{apiPath}/{eventBoxName}";

            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<AlarmResponseModel>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }
    public async Task<BaseResponse?>MiddlewareAliveCheck(string apiPath = $"api/Middleware/MidAlive")
    {
        try
        {
            //apiPath = $"{apiPath}/{eventBoxName}";

            var response = await ApiClient.GetAsyncHttp(apiPath);
            if (response != null)
            {
                var responseModel = new JsonSerializer()
                    .JsonStringToData<BaseResponse>(await response.Content.ReadAsStringAsync());

                return await Task.FromResult(responseModel);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"URL:{ApiClient.HostName} PATH:{apiPath} {ex.Message}");
        }

        return null;
    }


}
