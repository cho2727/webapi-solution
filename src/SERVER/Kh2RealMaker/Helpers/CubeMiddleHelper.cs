using Kh2RealMaker.Consts;
using Kh2RealMaker.Models;
using System.Text;

namespace Kh2RealMaker.Helpers;

public static class CubeMiddleHelper
{
    public enum RealPointType
    {
        NONE = 0,
        BI = 1,
        BO = 2,
        AI = 3,
        AO = 4,
        COUNTER = 5,
        DEV = 6,
        CAL = 7,
        CALBI = 11,
        CALAI = 13
    }

    public enum UPCDataType
    {
        none,
        upc_ushort,
        upc_uint,
        upc_short,
        upc_float,
        upc_int,
        upc_ulong,
        upc_long,
        upc_time,
        upc_byte,
        upc_char,
    }

    public static string GetMidTypeFromPointType(RealPointType pointType)
    {
        switch (pointType)
        {
            case RealPointType.BI:
                return "upc_byte";
            case RealPointType.BO:
                return "upc_byte";
            case RealPointType.AI:
                return "upc_float";
            case RealPointType.AO:
                return "upc_float";
            case RealPointType.COUNTER:
                return "upc_short";
            case RealPointType.CALBI:
                return "upc_byte";
            case RealPointType.CALAI:
                return "upc_float";

        }

        return "";
    }
    public static string CreateDataType(List<RealPointIndexDataModel>? rpDatas, List<CommonIndexDataModel> commonIndexs, string typeName)
    {
        StringBuilder type_sb = new StringBuilder();
        type_sb.Append($"CREATE STRUCT_TYPE {typeName.ToUpper()}\r\n");
        type_sb.Append("{\r\n");

        if(commonIndexs != null)
        {
            foreach (var idx in commonIndexs)
            {
                type_sb.Append(GetTypeByPoint(idx.EName, ((UPCDataType)idx.DataTypeId).ToString(), idx.Length));
            }
        }


        if(rpDatas != null)
        {
            foreach (var rp in rpDatas)
            {
                if ((RealPointType)rp.PointType != RealPointType.BO)
                {
                    string midTypeName = GetMidTypeFromPointType((RealPointType)rp.PointType);
                    type_sb.Append(GetTypeByPoint(rp.MidName, midTypeName, 1));
                }

                type_sb.Append(GetTypeByPoint(rp.MidName + "_tlq", "upc_ushort", 1));
                type_sb.Append(GetTypeByPoint(rp.MidName + "_uptime", "upc_time", 1));
            }
        }

        type_sb.Append("}\r\n\r\n");

        return type_sb.ToString();
    }

    public static int MiddlewareApply(this List<string> datatypes)
    {
        string dir = @System.AppDomain.CurrentDomain.BaseDirectory + ConstDefine.TempSaveFolderName;
        DirectoryInfo di = new DirectoryInfo(dir);
        if (!di.Exists)
        {
            di.Create();
        }
        string fileHeadStr = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string MiddlePath = System.Environment.GetEnvironmentVariable(ConstDefine.MiddleHomeEnvironmentName, EnvironmentVariableTarget.Machine) ?? string.Empty;
        if (!string.IsNullOrEmpty(MiddlePath))
        {
            string file = MiddlePath + "\\" + ConstDefine.MiddleDataTypeFileName;
            if (File.Exists(file))
            {
                File.Move(file, dir + "\\" + fileHeadStr + ConstDefine.MiddleDataTypeFileName);
                File.Delete(file);
            }

            foreach (var dat in datatypes)
                CubeMiddleHelper.WriteDataType(file, dat);

            //재기동은???
            Console.WriteLine($"실시간 포인트 미들웨어 적용 성공");
        }
        else // 임시 저장소에 저장
        {
            Console.WriteLine($"환경변수:{ConstDefine.MiddleHomeEnvironmentName}가 설정되어 있지 않습니다.");
            string file = dir + "\\" + fileHeadStr + ConstDefine.MiddleDataTypeFileName;
            if (File.Exists(file))
                File.Delete(file);

            foreach (var dat in datatypes)
                CubeMiddleHelper.WriteDataType(file, dat);
        }
        return 0;
    }
    public static void WriteDataType(string file, string strMsg)
    {
        try
        {
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(file));
            if (!di.Exists)
            {
                di.Create();
            }

            using (StreamWriter w = File.AppendText(file))
            {
                w.WriteLine("{0}", strMsg);
                w.Flush();
                w.Close();
                w.Dispose();
            }
        }
        catch (Exception)
        {
        }
    }

    private static string GetQueryByPoint(string midname, string typename, int arraycount)
    {
        StringBuilder sb = new StringBuilder();
        if (typename.Equals("upc_char") || typename.Equals("upc_wchar"))
            sb.Append($"\t[{midname}] {GetDBType(typename, arraycount)} NOT NULL DEFAULT(''), \r\n");
        else
            sb.Append($"\t[{midname}] {GetDBType(typename, arraycount)} NOT NULL DEFAULT('0'), \r\n");

        return sb.ToString();
    }

    private static string GetTypeByPoint(string midname, string typename, int arraycount)
    {
        StringBuilder sb = new StringBuilder();
        if (arraycount > 1)
            sb.Append($"\t{typename}\t\t{midname}[{arraycount}];\r\n");
        else
            sb.Append($"\t{typename}\t\t{midname};\r\n");

        return sb.ToString();
    }

    private static string GetDBType(string upctype, int count)
    {
        string strType = string.Empty;
        switch (upctype)
        {
            case "upc_byte":
                strType = "[tinyint]";
                break;
            case "upc_char":
                {
                    if (count > 1)
                    {
                        if (count > 8000)
                            strType = "[varchar](max)";
                        else
                            strType = $"[varchar]({count})";
                    }
                    else
                        strType = "[tinyint]";
                }
                break;
            case "upc_wchar":
                {
                    if (count > 1)
                    {
                        if (count > 8000)
                            strType = "[varchar](max)";
                        else
                            strType = $"[varchar]({count})";
                    }
                    else
                        strType = "[tinyint]";
                }
                break;
            case "upc_short":
            case "upc_ushort":
                strType = "[smallint]";
                break;
            case "upc_time":
            case "upc_time64":
            case "upc_int":
            case "upc_uint":
                strType = "[int]";
                break;
            case "upc_long":
            case "upc_ulong":
                strType = "[bigint]";
                break;
            case "upc_float":
                strType = "[real]";
                break;
            case "upc_double":
                strType = "[float]";
                break;
        }

        return strType;
    }
}
