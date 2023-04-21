using Kh2Historian.Models;
using Smart.Kh2Ems.EF.Core.Contexts;
using Smart.Kh2Ems.Infrastructure.Models;
using Smart.Kh2Ems.Infrastructure.Const;
using Smart.Kh2Ems.Infrastructure.Enums;
using Smart.Kh2Ems.Infrastructure.Helpers;
using Smart.Kh2Ems.Infrastructure.Shared.Injectables;
using Smart.Kh2Ems.Infrastructure.Shared.Interfaces;
using Smart.Kh2Ems.Log.EF.Core.Contexts;

namespace Kh2Historian.Common;

public class CommonDataManager : ISingletonService
{
    private readonly IApiLogger _logger;
    private readonly SqlServerContext _context;
    private readonly SqlServerLogContext _logContext;
    private readonly IConfiguration _configuration;


    public CommonDataManager(IApiLogger logger, SqlServerContext context, SqlServerLogContext logContext
                        , IConfiguration configuration)
    {
        this._logger = logger;
        this._context = context;
        this._logContext = logContext;
        this._configuration = configuration;
    }
    public List<PointIndexModel>? PointIndexModels { get; set; }
    public List<ConductingEquipmentModel>? ConductingEquipmentModels { get; set; }

    public List<StateValueModel>? stateValueModels { get; set; }

    public List<LogSettingInfo>? LogSettingIndexes { get; set; }

    public List<LogSaveActionModel> LogSaveActions { get; set; } = new List<LogSaveActionModel>();

    public bool IsMiddlewareConnected { get; set; } = false;

    public int DatabaseInit()
    {
        //var logs = _logContext.LogDays.ToList();
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

        LogSettingIndexes = _logContext.LogSettings.Select(x => new LogSettingInfo
        {
            CeqTypeId = x.CeqTypeId,
            DynamicIndex = x.DynamicIndex,
            IsReportSave = x.IsReportSave
        }).ToList();


        LogSettingIndexes = LogSettingIndexes.Join(
            PointIndexModels,
            x => new { x.CeqTypeId, x.DynamicIndex },
            y => new { y.CeqTypeId, y.DynamicIndex },
            (x, y) => new LogSettingInfo
            {
                CeqTypeId = x.CeqTypeId,
                DynamicIndex = x.DynamicIndex,
                DataTypeName = y.EName,
                IsReportSave = x.IsReportSave,
            }).ToList();

        var now = DateTime.Now;
        LogSaveActions.Add(new LogSaveActionModel {  SLogType = SaveLogType.Minute, NextSaveTime = DateTimeHelper.GetNextDateTime(now, SaveLogType.Minute, min:5) });
        LogSaveActions.Add(new LogSaveActionModel {  SLogType = SaveLogType.Hour, NextSaveTime = DateTimeHelper.GetNextDateTime(now, SaveLogType.Hour) });
        LogSaveActions.Add(new LogSaveActionModel {  SLogType = SaveLogType.Day, NextSaveTime = DateTimeHelper.GetNextDateTime(now, SaveLogType.Day) });
        LogSaveActions.Add(new LogSaveActionModel {  SLogType = SaveLogType.Month, NextSaveTime = DateTimeHelper.GetNextDateTime(now, SaveLogType.Month) });

        return ReturnCode.OK;
    }
}
