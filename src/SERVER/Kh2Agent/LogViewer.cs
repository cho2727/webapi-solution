namespace Kh2Agent;

public static class LogViewer
{
    public static async void LogViewAsync()
    {
        string path = @"D:\001. 개발소스\027. 뉴수소시범도시\개발소스\src\SERVER\Kh2Host\bin\Debug\net7.0\logs\Kh2EmsHost20230412.log";
        using (var reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
        {
            long lastLinePosition = 0;
            reader.BaseStream.Seek(lastLinePosition, SeekOrigin.End);
            while (true)
            {
                string? line = await reader.ReadLineAsync();

                if (line != null)
                {
                    int index = line.LastIndexOf("[INF]");
                    if (index > 0)
                    {
                        Console.WriteLine(line.Substring(index + 5));
                    }
                    //lastLinePosition = reader.BaseStream.Position;
                }
                //else
                //{
                //    reader.BaseStream.Seek(lastLinePosition, SeekOrigin.End);
                //}
                await Task.Delay(10);
            }
        }
    }
}
