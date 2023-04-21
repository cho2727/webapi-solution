using Kh2Host.Extentions;
using Kh2Host.Models;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;
using Smart.Kh2Ems.Infrastructure.Models;
using Smart.Kh2Ems.Infrastructure.Shared.Injectables;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.PowerCUBE.Api;
using System.Text.RegularExpressions;

namespace Kh2Host.Common;

public class CommonDataManager : ISingletonService
{
    private readonly IApiLogger _logger;
    private readonly SqlServerContext _context;
    private readonly IConfiguration _configuration;
    private readonly object _lock = new object();

    public readonly string RPUptimeString = "uptime";
    public readonly string RPTagString = "tlq";

    //public static CommonDataManager Instance => _instance.Value;
    //private static readonly Lazy<CommonDataManager> _instance
    //                  = new Lazy<CommonDataManager>(() => new CommonDataManager());

    public CommonDataManager(IApiLogger logger, SqlServerContext context
                        , IConfiguration configuration)
    {
        this._logger = logger;
        this._context = context;
        this._configuration = configuration;
    }
    public List<PointIndexModel>? PointIndexModels { get; set; }
    public List<ConductingEquipmentModel>? ConductingEquipmentModels { get; set; }

    public List<StateValueModel>? stateValueModels { get; set; }

    public bool IsMiddlewareConnected { get; set; } = false;

    public List<CalculationDataModel> CalculationDatas { get; set; } = new List<CalculationDataModel>();

    public List<ComputerInfoDataModel>? ComputerInfoDatas { get; set; }

    public List<ProgramInfoDataModel>? ProgramInfoDatas { get; set; }

    public int DatabaseInit()
    {
        var devicePoints = _context.DevicePointIndexViews;
        
        PointIndexModels = devicePoints.Select(x => new PointIndexModel
        {
            CeqTypeId = x.CeqTypeId,
            PointTypeId = x.PointTypeId,
            PointIndex = x.PointIndex,
            DynamicIndex = x.DynamicIndex,
            Name = x.Name,
            EName = x.EName,
            CircuitNo = x.CircuitNo,
            RemoteAddress = x.RemoteAddress,
            ClassNo = x.ClassNo,
            ModbusAddress = x.ModbusAddress,
            BitPosition = x.BitPosition ?? 0,
            AlarmPriority = x.AlarmPriority,
            DataTypeId = x.DataTypeId,
            StateGroupId = x.StateGroupId,
            Scale = x.Scale,
            Offset = x.Offset,
            LimitMaxValue = x.LimitMaxValue,
            LimitMinValue = x.LimitMinValue
        }).ToList();

        ConductingEquipmentModels = _context.ConductingEquipmentViews.Select(x => new ConductingEquipmentModel
        {
            CeqId = x.CeqId,
            CeqName = x.CeqName,
            OfficeCode = x.OfficeCode,
            StationMrfk = x.StationMrfk,
            StationName = x.StationName,
            ObjectType = x.ObjectType,
            ObjectTypeName = x.ObjectTypeName,
            TypeCode = x.TypeCode,
            ModelId = x.ModelId,
            ModelName = x.ModelName,
            CeqTypeFk = x.CeqTypeFk,
            DeviceFk = x.DeviceFk,
            CircuitNo = x.CircuitNo,
            CeqAliasName = x.CeqAliasName,
            DpName = x.DpName,
            DpType = x.DpType,
        }).ToList();

        stateValueModels = _context.StateValues.Select(x => new StateValueModel
        {
            StateGroupID = x.StateGroupFk,
            Name = x.Name,
            Value = x.Value
        }).ToList();

        ProgramInfoDatas = _context.ProgramInfoViews.Select(x => new ProgramInfoDataModel {
            ProgramId = x.ProgramId,
            Name = x.Name,
            ComputerFk = x.ComputerFk,
            ProgramTypeFk = x.ProgramTypeFk,
            ProgramNo = x.ProgramNo,
            ExecuteType = x.ExecuteType,
            AlarmPriorityFk = x.AlarmPriorityFk,
            StateGroupFk = x.StateGroupFk,
            IpAddr = x.IpAddr,
            TcpPort = x.TcpPort,
            StartCmd = x.StartCmd,
            StopCmd = x.StopCmd,
            UpdatePeriod = x.UpdatePeriod,
            UseFlag = x.UseFlag,
            ProcFullName = x.ProcFullName,
            ProgramDesc = x.ProgramDesc,
            DpName = x.DpName, DpType = x.DpType
        }).ToList();

        ComputerInfoDatas = _context.ComputerInfoViews.Select(x => new ComputerInfoDataModel {
            ComputerId = x.ComputerId,
            Name = x.Name,
            AlarmPriorityFk = x.AlarmPriorityFk,
            StateGroupFk = x.StateGroupFk,
            MemberOfficeFk = x.MemberOfficeFk,
            OsName = x.OsName,
            OsVersion = x.OsVersion,
            UseFlag = x.UseFlag,
            DpName = x.DpName,
            DpType = x.DpType
        }).ToList();

        return CubeReturnCode.CubeOK;
    }
    public int MiddlewareInit()
    {
        var totalCeqValues = _context.CeqValues.Join(_context.PointIndexViews,
            x => x.DynamicIndex,
            y => y.DynamicIndex, (x, y) => new { x.CeqMrid, x.DynamicIndex, y.EName, x.PointTypeFk, x.Value, x.QualityValue, x.TagValue, x.DeviceUptime }).ToList();

        var updateRPs = new List<RealPointDataModel>();
        if (ConductingEquipmentModels != null)
        {
            foreach (var ceq in ConductingEquipmentModels)
            {
                var rp = new RealPointDataModel
                {
                    RealPointName = ceq.DpName
                };
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "ceq_id", DataValue = ceq.CeqId.ToString() });
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "name", DataValue = ceq.CeqName.ToString() });
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "ceq_type", DataValue = ceq.CeqTypeFk.ToString() });
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "object_type", DataValue = ceq.ObjectType.ToString() });
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "object_name", DataValue = ceq.ObjectTypeName.ToString() });
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "model_id", DataValue = ceq.ModelId.ToString() });
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "model_name", DataValue = ceq.ModelName.ToString() });
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "device_comm_id", DataValue = ceq.DeviceFk.ToString() });
                var ceqValues = totalCeqValues.Where(x => x.CeqMrid == ceq.CeqId);
                foreach (var val in ceqValues)
                {
                    rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = val.EName, DataValue = val.Value.ToString() });
                    rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = $"{val.EName}_{RPTagString}", DataValue = ((val.TagValue << 8) + (val.QualityValue & 0x00ff)).ToString() });
                    rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = $"{val.EName}_{RPUptimeString}", DataValue = val.DeviceUptime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss") });
                }

                updateRPs.Add(rp);
            }
        }

        if(ProgramInfoDatas != null)
        {
            foreach (var ceq in ProgramInfoDatas)
            {
                var rp = new RealPointDataModel
                {
                    RealPointName = ceq.DpName
                };
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "program_id", DataValue = ceq.ProgramId.ToString() });
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "name", DataValue = ceq.Name.ToString() });

                updateRPs.Add(rp);
            }
        }

        if (ComputerInfoDatas != null)
        {
            foreach (var ceq in ComputerInfoDatas)
            {
                var rp = new RealPointDataModel
                {
                    RealPointName = ceq.DpName
                };
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "computer_id", DataValue = ceq.ComputerId.ToString() });
                rp.PointData.Add(new RealPointDataModel.RealPointData { DataTypeName = "name", DataValue = ceq.Name.ToString() });

                updateRPs.Add(rp);
            }
        }

        if(updateRPs.Count > 0)
            PowerCubeApi.Instance.MultiUpdateRealDataPoint(updateRPs);

        return CubeReturnCode.CubeOK;
    }
    public int CalculationInit()
    {
        var calDatas = _context.CalculationIndices.Select(x => new
        {
            x.IndexId, x.CeqTypeFk, x.PointTypeFk, x.Name, x.EName, x.Period, x.Formula
        });

        foreach (var calData in calDatas)
        {
            var points = PointIndexModels!.Where(x => x.CeqTypeId == calData.CeqTypeFk).ToList();
            if(points.Count > 0 && calData.Formula != null)
            {
                MatchCollection matches = Regex.Matches(calData.Formula, "\"([^\"]*)\"");
                string realFormula = calData.Formula;
                foreach (Match match in matches)
                {
                    string searchValue = match.Value.Replace("\"", "");
                    var mp = points.FirstOrDefault(x => x.DynamicIndex.ToString() == searchValue);
                    if (mp != null)
                        realFormula = realFormula.Replace(searchValue, $"_DP_/{mp.EName}");
                }

                var ceqs = ConductingEquipmentModels!.Where(x => x.CeqTypeFk == calData.CeqTypeFk).ToList();
                foreach ( var ceq in ceqs)
                {
                    var cal = new CalculationDataModel
                    {
                        CeqId = ceq.CeqId,
                        IndexId = calData.IndexId,
                        CeqTypeFk = calData.CeqTypeFk,
                        PointType = calData.PointTypeFk,
                        Name = calData.Name,
                        EName = calData.EName,
                        Period = calData.Period ?? 10,
                        Formula = calData.Formula,
                        RealFormula = realFormula.Replace("_DP_", ceq.DpName),
                        RealPointName = ceq.DpName!,
                        NextProcTime = DateTime.Now.AddSeconds(calData.Period ?? 10)
                    };
                    cal.Init();
                    CalculationDatas.Add(cal);
                }
            }
        }

        return 0;
    }
    public int UpdateCeqDatas(List<UpdatePointValue> dbupdatas, DateTime now)
    {
        int ret = 0;
        lock(_lock)
        {
            if (dbupdatas.Count > 0)
            {
                foreach (var upval in dbupdatas)
                {
                    var ceqValue = _context.CeqValues.FirstOrDefault(x => x.CeqMrid == upval.CeqMrid && x.DynamicIndex == upval.DynamicIndex);
                    if (ceqValue != null)
                    {
                        ceqValue.Value = upval.Value;
                        ceqValue.TagValue = upval.TagValue;
                        ceqValue.QualityValue = upval.QualityValue;
                        ceqValue.DeviceUptime = upval.DeviceUptime;
                        ceqValue.SaveTime = now;
                    }
                    else
                    {
                        _context.CeqValues.Add(new CeqValue
                        {
                            CeqMrid = upval.CeqMrid,
                            PointTypeFk = upval.PointType,
                            PointIndex = upval.PointIndex,
                            DynamicIndex = upval.DynamicIndex,
                            Value = upval.Value,
                            TagValue = upval.TagValue,
                            QualityValue = upval.QualityValue,
                            DeviceUptime = upval.DeviceUptime,
                            SaveTime = now
                        });
                    }
                }
                ret = _context.SaveChanges();
            }
        }
        return ret;
    }
}
