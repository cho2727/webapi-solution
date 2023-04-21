using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Kh2Ems.Infrastructure.Api;

[SupportedOSPlatform("windows")]
public static class ProgramManager
{
    public const int ProcessOK = 0;
    public const int ProcessAlreadyStart = 1;
    public const int ProcessError = -1;
    public const int ProcessException = -2;


    public static Process? ProgramStart(string filefullName, string filePath, ProcessWindowStyle style, string args, Action<object, EventArgs> expried)
    {
        FileInfo fileInfo = new FileInfo(filefullName);
        if(fileInfo.Exists)
        {
            Process p = new Process();

            p.StartInfo.FileName = filefullName;
            p.StartInfo.WorkingDirectory = $@"{filePath}";
            p.StartInfo.Arguments = args;
            p.StartInfo.WindowStyle = style;
            p.EnableRaisingEvents = true;
            p.Exited += new EventHandler(expried!);
            p.Start();
            return p;
        }

        return null;
    }

    public static void ProgramStop(int pid)
    {
        Process p = Process.GetProcessById(pid);
        if (p != null)
        {
            if (p.CloseMainWindow())
            {
                p.WaitForExit(2000);
                p.Kill();
                p.Close();
            }
            else
                p.Kill();
        }
    }

    public static void ProgramStop(Process p)
    {
        if (p != null)
        {
            if (p.CloseMainWindow())
            {
                p.WaitForExit(2000);
                p.Kill();
                p.Close();
            }
            else
                p.Kill();
        }
    }

    public static void ProgramStop(string ProcessName)
    {
        var procs = Process.GetProcessesByName(ProcessName);
        foreach (var p in procs)
        {
            if (p != null)
            {
                if (p.CloseMainWindow())
                {
                    p.WaitForExit(2000);
                    p.Kill();
                    p.Close();
                }
                else
                    p.Kill();
            }
        }
    }

    public static int ServiceStart(string serviceName)
    {
        try
        {
            ServiceController sc = new ServiceController(serviceName);
            if (sc.Status != ServiceControllerStatus.Running)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(3000));
            }
            sc.Close();
        }
        catch
        {
            return ProcessException;
        }

        return ProcessOK;
    }

    public static int ServiceStop(string serviceName)
    {
        try
        {
            ServiceController sc = new ServiceController(serviceName);
            sc.Stop();
            if (sc.Status != ServiceControllerStatus.Stopped)
            {
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(3000));
            }
            sc.Close();
        }
        catch
        {
            return ProcessException;
        }
        return ProcessOK;
    }

    public static int GetServiceStatus(string serviceName)
    {
        try
        {
            ServiceController sc = new ServiceController(serviceName);
            var retrunStatus = sc.Status == ServiceControllerStatus.Running ? 1 : 0;
            sc.Close();
            return retrunStatus;
        }
        catch
        {
            return ProcessException;
        }
    }
}
