using Newtonsoft.Json;

namespace Smart.Kh2Ems.Infrastructure.Api;

public class JsonSerializer
{
    public JsonSerializer()
    {

    }

    public T? ReadJSONFile<T>(string file) where T : class
    {
        try
        {
            var fi = new FileInfo(file);
            if (fi.Exists)
            {
                using (StreamReader sr = File.OpenText(file))
                {
                    var seriaizer = new Newtonsoft.Json.JsonSerializer();
                    return (T?)seriaizer.Deserialize(sr, typeof(T));
                }
            }
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }

        return null;
    }

    public bool WriteJSONFile(object obj, string file)
    {

        bool retValue = false;
        try
        {
            var dir = Path.GetDirectoryName(file);
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            //DirectoryInfo di = new DirectoryInfo(dir);
            //if (!di.Exists)
            //{
            //    di.Create();
            //}

            using (StreamWriter fi = File.CreateText(file))
            {
                var seriaizer = new Newtonsoft.Json.JsonSerializer();
                seriaizer.Serialize(fi, obj);

                retValue = true;
            }
        }
        catch
        {
            throw;
        }
        return retValue;
    }

    public async Task<bool> JsonFileSave<T>(T data, string file)
    {
        await Task.Delay(1);
        return WriteJSONFile(data, file);
    }

    public string JsonDataToString(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }


    public T? JsonStringToData<T>(T? obj, string jsonString)
    {
        try
        {
            obj = JsonConvert.DeserializeObject<T>(jsonString);
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }

        return obj;
    }


    public T? JsonStringToData<T>(string jsonString)
         where T : class, new()
    {
        T? obj = null;
        try
        {
            obj = JsonConvert.DeserializeObject<T>(jsonString);
        }
        catch (Exception ex)
        {

            throw new Exception(ex.Message);
        }

        return obj;
    }
}
