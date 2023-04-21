using ApiServer.Features.Middleware;
using Smart.Kh2Ems.Infrastructure.Enums;
using Smart.PowerCUBE.Api;

namespace ApiServer.Extentions;

public static class MiddlewareExtention
{
    public static void SetMeasureData(this Dictionary<string, object> model, List<ModelPointIndex> pointIndexs, RealPointDataModel rp)
    {
        if (rp == null)
            return;

        foreach (var data in pointIndexs)
        {
            var pd = rp.PointData.FirstOrDefault(x => x.DataTypeName == data.EName);
            if (pd != null)
            {
                if (data.PointTypeId == (int)RealPointType.BI)
                {
                    //model[data.DynamicIndex.ToString()!] = int.Parse(pd.DataValue);
                    model[data.EName] = int.Parse(pd.DataValue);
                }
                else
                {
                    //model[data.DynamicIndex.ToString()!] = float.Parse(pd.DataValue);
                    model[data.EName] = float.Parse(pd.DataValue);
                }
            }
        }
    }

    public static void SetMeasureData(this Dictionary<string, object> model, RealPointWithTypeNameDataModel rp)
    {
        if (rp == null)
            return;

        foreach (var pd in rp.PointData)
        {
            if (pd != null)
            {
                switch(pd.TypeName)
                {
                    case MiddleTypeValue.TP_BYTE_TYPE:
                        model[pd.DataTypeName] = byte.Parse(pd.DataValue);
                        break;
                    case MiddleTypeValue.TP_SHORT_TYPE:
                        model[pd.DataTypeName] = short.Parse(pd.DataValue);
                        break;
                    case MiddleTypeValue.TP_USHORT_TYPE:
                        model[pd.DataTypeName] = ushort.Parse(pd.DataValue);
                        break;
                    case MiddleTypeValue.TP_FLOAT_TYPE:
                        model[pd.DataTypeName] = float.Parse(pd.DataValue);
                        break;
                    case MiddleTypeValue.TP_DOUBLE_TYPE:
                        model[pd.DataTypeName] = double.Parse(pd.DataValue);
                        break;
                    case MiddleTypeValue.TP_INT_TYPE:
                        model[pd.DataTypeName] = int.Parse(pd.DataValue);
                        break;
                    case MiddleTypeValue.TP_UINT_TYPE:
                        model[pd.DataTypeName] = uint.Parse(pd.DataValue);
                        break;
                    case MiddleTypeValue.TP_LONG_TYPE:
                        model[pd.DataTypeName] = long.Parse(pd.DataValue);
                        break;
                    case MiddleTypeValue.TP_ULONG_TYPE:
                        model[pd.DataTypeName] = ulong.Parse(pd.DataValue);
                        break;
                    default:
                        model[pd.DataTypeName] = pd.DataValue;
                        break;
                }
            }
        }
    }


}
