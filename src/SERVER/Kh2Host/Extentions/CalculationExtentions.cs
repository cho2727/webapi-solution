using Kh2Host.Models;
using Smart.PowerCUBE.Api;

namespace Kh2Host.Extentions;

public static class CalculationExtentions
{
    public static bool Init(this CalculationDataModel model)
    {
        model.FormularData = new CalculationEngine.Formula(model.RealFormula);
        if (model.FormularData.AnyErrors)
        {
            return false;
        }

        return true;
    }

    public static void Evaluation(this List<CalculationDataModel> models)
    {
        // 1. 데이터 가져오기
        var rpDatas = PowerCubeApi.Instance.GetRealPointData(models.Select(x => x.RealPointName).ToList());
        // 2. FormularData 값 셋팅- 실시간 포인트 스트링 치환
        if(rpDatas != null)
        {
            foreach (var model in models)
            {
                if (model.FormularData != null)
                {
                    foreach (var item in model.FormularData.Variables.Items)
                    {
                        var pointString = item.Value.VariableName.Substring(1, item.Value.VariableName.Length - 2);
                        int lastIndex = pointString.LastIndexOf('/');

                        var realPointName = pointString.Substring(0, lastIndex);
                        var fieldName = pointString.Substring(lastIndex + 1, pointString.Length - (lastIndex + 1));
                        var val = rpDatas.FirstOrDefault(x => x.RealPointName == model.RealPointName)?.PointData.FirstOrDefault(p => p.DataTypeName == fieldName)?.DataValue ?? "0";
                        item.Value.VariableValue = val;
                    }

                    CalculationEngine.Evaluator eval = new CalculationEngine.Evaluator(model.FormularData);
                    bool retValue = eval.Evaluate(out string calValue, out string errMsg);
                    if (retValue == false)
                    {
                        calValue = "0";
                    }

                    model.CalculatedValue = calValue;
                    model.NextProcTime = model.NextProcTime.AddSeconds(model.Period);
                }
            }
        }
        else
        {
            // 로그 출력
        }
    }
}
