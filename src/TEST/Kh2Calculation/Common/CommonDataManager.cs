using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.Infrastructure.Models;
using Smart.Kh2Ems.Infrastructure.Const;
using Smart.Kh2Ems.Infrastructure.Shared.Injectables;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;

namespace Kh2Calculation.Common;

public class CommonDataManager : ISingletonService
{
    private readonly IApiLogger _logger;
    private readonly SqlServerContext _context;
    private readonly IConfiguration _configuration;

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

        return ReturnCode.OK;
    }
}
