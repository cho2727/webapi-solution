using System.ComponentModel;
using System.Text;

namespace Smart.Kh2Ems.Infrastructure.Api;

public class RestApiClient
{
    private readonly string _hostName;
    private readonly string _apiType;
    private readonly string? _token;

    public string HostName => _hostName; 

    public RestApiClient(string hostName, string apiType = "application/json", string? token = null)
    {
        this._hostName = hostName;
        this._apiType = apiType;
        this._token = token;
    }

    private string GetRestfulString<T>(T model)
    {
        if (typeof(T).Equals(typeof(string)))
        {
            return "?" + model;
        }

        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        List<string> strs = new List<string>();
        foreach (PropertyDescriptor prop in properties)
        {
            strs.Add($"{prop.Name}={prop.GetValue(model)}");
        }

        return "?" + string.Join("&", strs);
    }


    public async Task<HttpResponseMessage> GetAsyncHttp<T>(T requestData, string apiPath)
            where T : class
    {
        var baseAddress = new Uri(_hostName);

        using (var httpClient = new HttpClient { BaseAddress = baseAddress })
        {
            if (!string.IsNullOrEmpty(_token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            }

            string requestUri = apiPath;
            if (requestData != null)
            {
                requestUri += GetRestfulString(requestData);
            }

            return await httpClient.GetAsync(requestUri).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// GET방식
    /// </summary>
    /// <param name="apiPath"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> GetAsyncHttp(string apiPath)
    {
        var baseAddress = new Uri(_hostName);

        using (var httpClient = new HttpClient { BaseAddress = baseAddress })
        {
            if (!string.IsNullOrEmpty(_token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            }

            string requestUri = apiPath;

            return await httpClient.GetAsync(requestUri).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// POST 방식
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="requestData"></param>
    /// <param name="apiPath"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> PostAsyncHttp<T>(T requestData, string apiPath)
    where T : class
    {
        var baseAddress = new Uri(_hostName); //http://121.157.1.130:38870
        using (var httpClient = new HttpClient() { BaseAddress = baseAddress })
        {
            if (!string.IsNullOrEmpty(_token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            }

            string jsonString = new JsonSerializer().JsonDataToString(requestData);
            StringContent queryString = new StringContent(jsonString, UnicodeEncoding.UTF8, _apiType);

            return await httpClient.PostAsync(apiPath, queryString).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// POST Pair 방식
    /// </summary>
    /// <param name="requestDatas"></param>
    /// <param name="apiPath"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> PostPairAsyncHttp(IEnumerable<KeyValuePair<string, string>> requestDatas,string apiPath)
    {
        var baseAddress = new Uri(_hostName); //http://121.157.1.130:38870
        using (var httpClient = new HttpClient() { BaseAddress = baseAddress })
        {
            if (!string.IsNullOrEmpty(_token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            }

            var content = new FormUrlEncodedContent(requestDatas);
            return await httpClient.PostAsync(apiPath, content).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// GET Pair 방식
    /// </summary>
    /// <param name="requestDatas"></param>
    /// <param name="apiPath"></param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> GetPairAsyncHttp(IEnumerable<KeyValuePair<string, string>> requestDatas, string apiPath)
    {
        var baseAddress = new Uri(_hostName);

        using (var httpClient = new HttpClient { BaseAddress = baseAddress })
        {
            if (!string.IsNullOrEmpty(_token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
            }

            string requestUri = apiPath;
            if (requestDatas.Count() > 0)
            {
                var strs = requestDatas.Select(x => string.Format($"{x.Key}={x.Value}"));
                requestUri += $"?{string.Join("&", strs)}";
                //requestUri += System.Web.HttpUtility.UrlEncode($"?{string.Join("&", strs)}", Encoding.UTF8);
            }


            return await httpClient.GetAsync(requestUri).ConfigureAwait(false);
            //return await httpClient.GetAsync(requestUri).Result;
        }
    }


}
