using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smart.Kh2Ems.EF.Core.Infrastructure.Reverse.Models;

namespace Smart.Kh2Ems.EF.Core.Infrastructure.Reverse;

public partial class KH2emsServerContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public KH2emsServerContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected KH2emsServerContext(IConfiguration configuration, DbContextOptions options) : base(options)
    {
        _configuration = configuration;
    }
    public KH2emsServerContext(IConfiguration configuration, DbContextOptions<KH2emsServerContext> options)
        : base(options)
    {
        _configuration = configuration;
    }
    public virtual DbSet<AiIndex> AiIndices { get; set; }

    public virtual DbSet<AlarmPriority> AlarmPriorities { get; set; }

    public virtual DbSet<AlarmType> AlarmTypes { get; set; }

    public virtual DbSet<AoIndex> AoIndices { get; set; }

    public virtual DbSet<AreaCode> AreaCodes { get; set; }

    public virtual DbSet<BaseVoltage> BaseVoltages { get; set; }

    public virtual DbSet<BiIndex> BiIndices { get; set; }

    public virtual DbSet<BoIndex> BoIndices { get; set; }

    public virtual DbSet<BoIndexToRemote> BoIndexToRemotes { get; set; }

    public virtual DbSet<CalculationIndex> CalculationIndices { get; set; }

    public virtual DbSet<CeqPointIndexView> CeqPointIndexViews { get; set; }

    public virtual DbSet<CeqType> CeqTypes { get; set; }

    public virtual DbSet<CeqValue> CeqValues { get; set; }

    public virtual DbSet<CommType> CommTypes { get; set; }

    public virtual DbSet<CommonIndex> CommonIndices { get; set; }

    public virtual DbSet<CommonIndexGroup> CommonIndexGroups { get; set; }

    public virtual DbSet<CompanyCode> CompanyCodes { get; set; }

    public virtual DbSet<ComputerGroup> ComputerGroups { get; set; }

    public virtual DbSet<ComputerInfo> ComputerInfos { get; set; }

    public virtual DbSet<ComputerInfoView> ComputerInfoViews { get; set; }

    public virtual DbSet<ComputerState> ComputerStates { get; set; }

    public virtual DbSet<ConductingEquipment> ConductingEquipments { get; set; }

    public virtual DbSet<ConductingEquipmentView> ConductingEquipmentViews { get; set; }

    public virtual DbSet<CounterIndex> CounterIndices { get; set; }

    public virtual DbSet<DataType> DataTypes { get; set; }

    public virtual DbSet<DerType> DerTypes { get; set; }

    public virtual DbSet<DeviceCommConfig> DeviceCommConfigs { get; set; }

    public virtual DbSet<DeviceCommUnit> DeviceCommUnits { get; set; }

    public virtual DbSet<DevicePointIndexView> DevicePointIndexViews { get; set; }

    public virtual DbSet<GroupOffice> GroupOffices { get; set; }

    public virtual DbSet<IdentityObject> IdentityObjects { get; set; }

    public virtual DbSet<LogControl> LogControls { get; set; }

    public virtual DbSet<LogDeviceComm> LogDeviceComms { get; set; }

    public virtual DbSet<LogEvent> LogEvents { get; set; }

    public virtual DbSet<LogSystem> LogSystems { get; set; }

    public virtual DbSet<MasterIndex> MasterIndices { get; set; }

    public virtual DbSet<MemberOffice> MemberOffices { get; set; }

    public virtual DbSet<ModbusSchedule> ModbusSchedules { get; set; }

    public virtual DbSet<ModelIndex> ModelIndices { get; set; }

    public virtual DbSet<ModelInfo> ModelInfos { get; set; }

    public virtual DbSet<ModelItemIndex> ModelItemIndices { get; set; }

    public virtual DbSet<MsgErrorType> MsgErrorTypes { get; set; }

    public virtual DbSet<MsgGroup> MsgGroups { get; set; }

    public virtual DbSet<MsgType> MsgTypes { get; set; }

    public virtual DbSet<ObjectType> ObjectTypes { get; set; }

    public virtual DbSet<PointIndexView> PointIndexViews { get; set; }

    public virtual DbSet<PointQualityCode> PointQualityCodes { get; set; }

    public virtual DbSet<PointTagCode> PointTagCodes { get; set; }

    public virtual DbSet<PointType> PointTypes { get; set; }

    public virtual DbSet<ProgramInfo> ProgramInfos { get; set; }

    public virtual DbSet<ProgramInfoView> ProgramInfoViews { get; set; }

    public virtual DbSet<ProgramState> ProgramStates { get; set; }

    public virtual DbSet<ProgramType> ProgramTypes { get; set; }

    public virtual DbSet<ProtocolType> ProtocolTypes { get; set; }

    public virtual DbSet<RemoteControlValue> RemoteControlValues { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportForm> ReportForms { get; set; }

    public virtual DbSet<ReportFormType> ReportFormTypes { get; set; }

    public virtual DbSet<ReportOperatorType> ReportOperatorTypes { get; set; }

    public virtual DbSet<ScaleFactor> ScaleFactors { get; set; }

    public virtual DbSet<Sound> Sounds { get; set; }

    public virtual DbSet<StateGroup> StateGroups { get; set; }

    public virtual DbSet<StateValue> StateValues { get; set; }

    public virtual DbSet<StationType> StationTypes { get; set; }

    public virtual DbSet<StationView> StationViews { get; set; }

    public virtual DbSet<Substation> Substations { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAuthorityType> UserAuthorityTypes { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=127.0.0.1,4433;Database=k_h2ems_server;User Id=sa;Password=20wellsdb19!@;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AiIndex>(entity =>
        {
            entity.HasKey(e => e.IndexId);

            entity.ToTable("ai_index", tb => tb.HasComment("아날로그 계측 표준 인덱스"));

            entity.Property(e => e.IndexId)
                .ValueGeneratedNever()
                .HasComment("아날로그 계측 인덱스 ID")
                .HasColumnName("index_id");
            entity.Property(e => e.AlarmPriorityFk)
                .HasComment("알람 우선순위 ID")
                .HasColumnName("alarm_priority_fk");
            entity.Property(e => e.DataTypeFk)
                .HasComment("데이터 타입 ID")
                .HasColumnName("data_type_fk");
            entity.Property(e => e.Deadband)
                .HasComment("deadband 값")
                .HasColumnName("deadband");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("아날로그 계측 영문 인덱스명")
                .HasColumnName("e_name");
            entity.Property(e => e.LimitMaxValue)
                .HasComment("최대 LIMIT 값")
                .HasColumnName("limit_max_value");
            entity.Property(e => e.LimitMinValue)
                .HasComment("최소 LIMIT 값")
                .HasColumnName("limit_min_value");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("아날로그 계측 인덱스명")
                .HasColumnName("name");
            entity.Property(e => e.ScaleFactorFk)
                .HasComment("스케일 펙터 ID")
                .HasColumnName("scale_factor_fk");
            entity.Property(e => e.StateGroupFk)
                .HasComment("상태 구룹 ID")
                .HasColumnName("state_group_fk");
            entity.Property(e => e.UnitFk)
                .HasComment("단위 ID")
                .HasColumnName("unit_fk");

            entity.HasOne(d => d.AlarmPriorityFkNavigation).WithMany(p => p.AiIndices)
                .HasForeignKey(d => d.AlarmPriorityFk)
                .HasConstraintName("FK_ai_index_alarm_priority_fk");

            entity.HasOne(d => d.DataTypeFkNavigation).WithMany(p => p.AiIndices)
                .HasForeignKey(d => d.DataTypeFk)
                .HasConstraintName("FK_ai_index_data_type_fk");

            entity.HasOne(d => d.ScaleFactorFkNavigation).WithMany(p => p.AiIndices)
                .HasForeignKey(d => d.ScaleFactorFk)
                .HasConstraintName("FK_ai_index_scale_factor_fk");

            entity.HasOne(d => d.StateGroupFkNavigation).WithMany(p => p.AiIndices)
                .HasForeignKey(d => d.StateGroupFk)
                .HasConstraintName("FK_ai_index_state_group_fk");

            entity.HasOne(d => d.UnitFkNavigation).WithMany(p => p.AiIndices)
                .HasForeignKey(d => d.UnitFk)
                .HasConstraintName("FK_ai_index_unit_fk");
        });

        modelBuilder.Entity<AlarmPriority>(entity =>
        {
            entity.ToTable("alarm_priority", tb => tb.HasComment("알람 우선순위"));

            entity.Property(e => e.AlarmPriorityId)
                .ValueGeneratedNever()
                .HasComment("알람 우선순위 ID")
                .HasColumnName("alarm_priority_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("알람 우선순위명")
                .HasColumnName("name");
            entity.Property(e => e.SoundFk)
                .HasComment("소리 ID")
                .HasColumnName("sound_fk");
        });

        modelBuilder.Entity<AlarmType>(entity =>
        {
            entity.ToTable("alarm_type", tb => tb.HasComment("알람 타입"));

            entity.Property(e => e.AlarmTypeId)
                .ValueGeneratedNever()
                .HasComment("알람 타입 ID")
                .HasColumnName("alarm_type_id");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("알람 영문명")
                .HasColumnName("e_name");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("알람 타입명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<AoIndex>(entity =>
        {
            entity.HasKey(e => e.IndexId);

            entity.ToTable("ao_index", tb => tb.HasComment("아날로그 설정 표준 인덱스"));

            entity.Property(e => e.IndexId)
                .ValueGeneratedNever()
                .HasComment("아날로그 설정 인덱스 ID")
                .HasColumnName("index_id");
            entity.Property(e => e.AlarmPriorityFk)
                .HasComment("알람 우선순위 ID")
                .HasColumnName("alarm_priority_fk");
            entity.Property(e => e.DataTypeFk)
                .HasComment("데이터 타입 ID")
                .HasColumnName("data_type_fk");
            entity.Property(e => e.Deadband)
                .HasComment("deadband 값")
                .HasColumnName("deadband");
            entity.Property(e => e.DefaultVal)
                .HasComment("AO 포인트 설정 기본 값")
                .HasColumnName("default_val");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("아날로그 설정 영문 인덱스명")
                .HasColumnName("e_name");
            entity.Property(e => e.ExistOff)
                .HasComment("AO 포인트 설정 OFF 존재 여부")
                .HasColumnName("exist_off");
            entity.Property(e => e.MaxVal)
                .HasComment("AO 포인트 설정 최대 값")
                .HasColumnName("max_val");
            entity.Property(e => e.MinVal)
                .HasComment("AO 포인트 설정 최소 값")
                .HasColumnName("min_val");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("아날로그 설정 인덱스명")
                .HasColumnName("name");
            entity.Property(e => e.OffVal)
                .HasComment("AO 포인트 설정 OFF 값")
                .HasColumnName("off_val");
            entity.Property(e => e.ScaleFactorFk)
                .HasComment("스케일 펙터터 ID")
                .HasColumnName("scale_factor_fk");
            entity.Property(e => e.StateGroupFk)
                .HasComment("상태 그룹 ID")
                .HasColumnName("state_group_fk");
            entity.Property(e => e.StepVal)
                .HasComment("AO 포인트 설정 STEP 값")
                .HasColumnName("step_val");
            entity.Property(e => e.UnitFk)
                .HasComment("단위 ID")
                .HasColumnName("unit_fk");

            entity.HasOne(d => d.AlarmPriorityFkNavigation).WithMany(p => p.AoIndices)
                .HasForeignKey(d => d.AlarmPriorityFk)
                .HasConstraintName("FK_ao_index_alarm_priority_fk");

            entity.HasOne(d => d.DataTypeFkNavigation).WithMany(p => p.AoIndices)
                .HasForeignKey(d => d.DataTypeFk)
                .HasConstraintName("FK_ao_index_data_type_fk");

            entity.HasOne(d => d.ScaleFactorFkNavigation).WithMany(p => p.AoIndices)
                .HasForeignKey(d => d.ScaleFactorFk)
                .HasConstraintName("FK_ao_index_scale_factor_fk");

            entity.HasOne(d => d.StateGroupFkNavigation).WithMany(p => p.AoIndices)
                .HasForeignKey(d => d.StateGroupFk)
                .HasConstraintName("FK_ao_index_state_group_fk");

            entity.HasOne(d => d.UnitFkNavigation).WithMany(p => p.AoIndices)
                .HasForeignKey(d => d.UnitFk)
                .HasConstraintName("FK_ao_index_unit_fk");
        });

        modelBuilder.Entity<AreaCode>(entity =>
        {
            entity.ToTable("area_code", tb => tb.HasComment("지역(사업소) 코드"));

            entity.Property(e => e.AreaCodeId)
                .ValueGeneratedNever()
                .HasComment("지역 코드")
                .HasColumnName("area_code_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("이름")
                .HasColumnName("name");
        });

        modelBuilder.Entity<BaseVoltage>(entity =>
        {
            entity.ToTable("base_voltage", tb => tb.HasComment("기준 전압"));

            entity.Property(e => e.BaseVoltageId)
                .ValueGeneratedNever()
                .HasComment("기준 전압 ID")
                .HasColumnName("base_voltage_id");
            entity.Property(e => e.MaxVoltage)
                .HasComment("최대 전압")
                .HasColumnName("max_voltage");
            entity.Property(e => e.MaxVoltageUnitFk)
                .HasComment("최대 전압 단위")
                .HasColumnName("max_voltage_unit_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("기준 전압명")
                .HasColumnName("name");
            entity.Property(e => e.NormalVoltage)
                .HasComment("기준 전압")
                .HasColumnName("normal_voltage");
            entity.Property(e => e.NormalVoltageUnitFk)
                .HasComment("기준 전압 단위")
                .HasColumnName("normal_voltage_unit_fk");
        });

        modelBuilder.Entity<BiIndex>(entity =>
        {
            entity.HasKey(e => e.IndexId);

            entity.ToTable("bi_index", tb => tb.HasComment("상태 계측 표준 인덱스"));

            entity.Property(e => e.IndexId)
                .ValueGeneratedNever()
                .HasComment("상태 계측 인덱스 ID")
                .HasColumnName("index_id");
            entity.Property(e => e.AlarmPriorityFk)
                .HasComment("알람 우선순위 코드")
                .HasColumnName("alarm_priority_fk");
            entity.Property(e => e.DataTypeFk)
                .HasComment("데이터 타입 코드")
                .HasColumnName("data_type_fk");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("상태 계측 영문 인덱스명")
                .HasColumnName("e_name");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("상태 계측 인덱스명")
                .HasColumnName("name");
            entity.Property(e => e.StateGroupFk)
                .HasComment("상태 그룹 코드")
                .HasColumnName("state_group_fk");

            entity.HasOne(d => d.AlarmPriorityFkNavigation).WithMany(p => p.BiIndices)
                .HasForeignKey(d => d.AlarmPriorityFk)
                .HasConstraintName("FK_bi_index_alarm_priority_fk");

            entity.HasOne(d => d.DataTypeFkNavigation).WithMany(p => p.BiIndices)
                .HasForeignKey(d => d.DataTypeFk)
                .HasConstraintName("FK_bi_index_data_type_fk");

            entity.HasOne(d => d.StateGroupFkNavigation).WithMany(p => p.BiIndices)
                .HasForeignKey(d => d.StateGroupFk)
                .HasConstraintName("FK_bi_index_state_group_fk");
        });

        modelBuilder.Entity<BoIndex>(entity =>
        {
            entity.HasKey(e => e.IndexId);

            entity.ToTable("bo_index", tb => tb.HasComment("상태 제어 표준 인덱스"));

            entity.Property(e => e.IndexId)
                .ValueGeneratedNever()
                .HasComment("제어 인덱스 ID")
                .HasColumnName("index_id");
            entity.Property(e => e.ControlCount)
                .HasComment("상태 제어 코드 개수")
                .HasColumnName("control_count");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("제어 영문 인덱스명")
                .HasColumnName("e_name");
            entity.Property(e => e.LinkBiIndexFk)
                .HasComment("연결된 BI 인덱스 ID")
                .HasColumnName("link_bi_index_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("제어 인덱스명")
                .HasColumnName("name");

            entity.HasOne(d => d.LinkBiIndexFkNavigation).WithMany(p => p.BoIndices)
                .HasForeignKey(d => d.LinkBiIndexFk)
                .HasConstraintName("FK_bo_index_link_bi_index_fk");
        });

        modelBuilder.Entity<BoIndexToRemote>(entity =>
        {
            entity.HasKey(e => e.BoIndexRemoteId);

            entity.ToTable("bo_index_to_remote", tb => tb.HasComment("상태 제어 코드 설정"));

            entity.Property(e => e.BoIndexRemoteId)
                .ValueGeneratedNever()
                .HasComment("제어 코드 ID")
                .HasColumnName("bo_index_remote_id");
            entity.Property(e => e.BoIndexFk)
                .HasComment("연결된 BO 인덱스")
                .HasColumnName("bo_index_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("제어 코드명")
                .HasColumnName("name");
            entity.Property(e => e.RemoteControlIndexFk)
                .HasComment("연결된 원격 제어 설정 ID")
                .HasColumnName("remote_control_index_fk");
            entity.Property(e => e.ValidBiValue)
                .HasComment("제어 유효성 검사값")
                .HasColumnName("valid_bi_value");

            entity.HasOne(d => d.BoIndexFkNavigation).WithMany(p => p.BoIndexToRemotes)
                .HasForeignKey(d => d.BoIndexFk)
                .HasConstraintName("FK_bo_index_to_remote_bo_index_fk");

            entity.HasOne(d => d.RemoteControlIndexFkNavigation).WithMany(p => p.BoIndexToRemotes)
                .HasForeignKey(d => d.RemoteControlIndexFk)
                .HasConstraintName("FK_bo_index_to_remote_remote_control_index_fk");
        });

        modelBuilder.Entity<CalculationIndex>(entity =>
        {
            entity.HasKey(e => e.IndexId);

            entity.ToTable("calculation_index", tb => tb.HasComment("계산 포인트 인덱스"));

            entity.Property(e => e.IndexId)
                .ValueGeneratedNever()
                .HasComment("인덱스 ID")
                .HasColumnName("index_id");
            entity.Property(e => e.AlarmPriorityFk)
                .HasComment("알람 우선순위 ID")
                .HasColumnName("alarm_priority_fk");
            entity.Property(e => e.CeqTypeFk)
                .HasComment("설비 종류 ID")
                .HasColumnName("ceq_type_fk");
            entity.Property(e => e.DataTypeFk)
                .HasComment("데이터 타입 ID")
                .HasColumnName("data_type_fk");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("설명")
                .HasColumnName("description");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("영문 인덱스명")
                .HasColumnName("e_name");
            entity.Property(e => e.EditTime)
                .HasComment("포인트 편집 시간")
                .HasColumnType("datetime")
                .HasColumnName("edit_time");
            entity.Property(e => e.Formula)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("계산식")
                .HasColumnName("formula");
            entity.Property(e => e.LimitMaxValue)
                .HasComment("최대 LIMIT 값")
                .HasColumnName("limit_max_value");
            entity.Property(e => e.LimitMinValue)
                .HasComment("최소 LIMIT 값")
                .HasColumnName("limit_min_value");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("인덱스명")
                .HasColumnName("name");
            entity.Property(e => e.Period)
                .HasComment("계산주기")
                .HasColumnName("period");
            entity.Property(e => e.PointTypeFk)
                .HasComment("포인트 타입 ID")
                .HasColumnName("point_type_fk");
            entity.Property(e => e.StateGroupFk)
                .HasComment("상태 그룹 ID")
                .HasColumnName("state_group_fk");
            entity.Property(e => e.UnitFk)
                .HasComment("단위 ID")
                .HasColumnName("unit_fk");

            entity.HasOne(d => d.AlarmPriorityFkNavigation).WithMany(p => p.CalculationIndices)
                .HasForeignKey(d => d.AlarmPriorityFk)
                .HasConstraintName("FK_alarm_priority_TO_calculation_index");

            entity.HasOne(d => d.CeqTypeFkNavigation).WithMany(p => p.CalculationIndices)
                .HasForeignKey(d => d.CeqTypeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ceq_type_TO_calculation_index");

            entity.HasOne(d => d.DataTypeFkNavigation).WithMany(p => p.CalculationIndices)
                .HasForeignKey(d => d.DataTypeFk)
                .HasConstraintName("FK_data_type_TO_calculation_index");

            entity.HasOne(d => d.PointTypeFkNavigation).WithMany(p => p.CalculationIndices)
                .HasForeignKey(d => d.PointTypeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_point_type_TO_calculation_index");

            entity.HasOne(d => d.StateGroupFkNavigation).WithMany(p => p.CalculationIndices)
                .HasForeignKey(d => d.StateGroupFk)
                .HasConstraintName("FK_state_group_TO_calculation_index");

            entity.HasOne(d => d.UnitFkNavigation).WithMany(p => p.CalculationIndices)
                .HasForeignKey(d => d.UnitFk)
                .HasConstraintName("FK_calculation_index_unit_fk");
        });

        modelBuilder.Entity<CeqPointIndexView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ceq_point_index_view");

            entity.Property(e => e.AlarmPriority).HasColumnName("alarm_priority");
            entity.Property(e => e.CeqTypeId).HasColumnName("ceq_type_id");
            entity.Property(e => e.CeqTypeName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("ceq_type_name");
            entity.Property(e => e.DataTypeId).HasColumnName("data_type_id");
            entity.Property(e => e.DynamicIndex).HasColumnName("dynamic_index");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("e_name");
            entity.Property(e => e.LimitMaxValue).HasColumnName("limit_max_value");
            entity.Property(e => e.LimitMinValue).HasColumnName("limit_min_value");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PointIndex).HasColumnName("point_index");
            entity.Property(e => e.PointTypeId).HasColumnName("point_type_id");
            entity.Property(e => e.StateGroupId).HasColumnName("state_group_id");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.UnitName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("unit_name");
        });

        modelBuilder.Entity<CeqType>(entity =>
        {
            entity.ToTable("ceq_type", tb => tb.HasComment("설비 타입"));

            entity.Property(e => e.CeqTypeId)
                .ValueGeneratedNever()
                .HasComment("설비 타입 ID")
                .HasColumnName("ceq_type_id");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("설비 영문 타입명")
                .HasColumnName("e_name");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("설비 타입명")
                .HasColumnName("name");
            entity.Property(e => e.ProtocolTypeFk)
                .HasComment("프로토콜 ID")
                .HasColumnName("protocol_type_fk");

            entity.HasOne(d => d.ProtocolTypeFkNavigation).WithMany(p => p.CeqTypes)
                .HasForeignKey(d => d.ProtocolTypeFk)
                .HasConstraintName("FK_ceq_type_protocol_type_fk");
        });

        modelBuilder.Entity<CeqValue>(entity =>
        {
            entity.HasKey(e => new { e.CeqMrid, e.DynamicIndex });

            entity.ToTable("ceq_value", tb => tb.HasComment("설비 포인트 값 정보"));

            entity.Property(e => e.CeqMrid)
                .HasComment("설비 mrID")
                .HasColumnName("ceq_mrid");
            entity.Property(e => e.DynamicIndex)
                .HasComment("동적 인덱스 ID")
                .HasColumnName("dynamic_index");
            entity.Property(e => e.DeviceUptime)
                .HasComment("기기 업데이트 시간")
                .HasColumnType("datetime")
                .HasColumnName("device_uptime");
            entity.Property(e => e.PointIndex)
                .HasComment("포인트 인덱스 ID")
                .HasColumnName("point_index");
            entity.Property(e => e.PointTypeFk)
                .HasComment("포인트 타입 ID")
                .HasColumnName("point_type_fk");
            entity.Property(e => e.QualityValue)
                .HasComment("Quality 값")
                .HasColumnName("quality_value");
            entity.Property(e => e.SaveTime)
                .HasComment("서버 업데이트 시간")
                .HasColumnType("datetime")
                .HasColumnName("save_time");
            entity.Property(e => e.TagValue)
                .HasComment("TAG 값")
                .HasColumnName("tag_value");
            entity.Property(e => e.Value)
                .HasComment("값")
                .HasColumnName("value");

            entity.HasOne(d => d.PointTypeFkNavigation).WithMany(p => p.CeqValues)
                .HasForeignKey(d => d.PointTypeFk)
                .HasConstraintName("FK_ceq_value_point_type_fk");
        });

        modelBuilder.Entity<CommType>(entity =>
        {
            entity.ToTable("comm_type", tb => tb.HasComment("통신 타입"));

            entity.Property(e => e.CommTypeId)
                .ValueGeneratedNever()
                .HasComment("통신 타입 ID")
                .HasColumnName("comm_type_id");
            entity.Property(e => e.ApplicationTimeout)
                .HasComment("어플리케이션 송신 대기 시간")
                .HasColumnName("application_timeout");
            entity.Property(e => e.CommrateStandard)
                .HasComment("통신 성공률 기준값")
                .HasColumnName("commrate_standard");
            entity.Property(e => e.HmiCmdTimeout)
                .HasComment("HMI 명령 타임아웃")
                .HasColumnName("hmi_cmd_timeout");
            entity.Property(e => e.HmiFileCmdTimeout)
                .HasComment("HMI 파형 명령 타임아웃")
                .HasColumnName("hmi_file_cmd_timeout");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("통신 타입명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<CommonIndex>(entity =>
        {
            entity.HasKey(e => e.IndexId).IsClustered(false);

            entity.ToTable("common_index", tb => tb.HasComment("공통 인덱스 테이블"));

            entity.Property(e => e.IndexId)
                .ValueGeneratedNever()
                .HasComment("공통 인덱스ID")
                .HasColumnName("index_id");
            entity.Property(e => e.DataTypeId)
                .HasComment("데이터 타입 ID")
                .HasColumnName("data_type_id");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("상태 계측 영문 인덱스명")
                .HasColumnName("e_name");
            entity.Property(e => e.IndexGroupFk)
                .HasComment("인덱스그룹")
                .HasColumnName("index_group_fk");
            entity.Property(e => e.Length)
                .HasComment("데이터 크기")
                .HasColumnName("length");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("상태 계측 인덱스명")
                .HasColumnName("name");

            entity.HasOne(d => d.DataType).WithMany(p => p.CommonIndices)
                .HasForeignKey(d => d.DataTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_data_type_TO_common_index");

            entity.HasOne(d => d.IndexGroupFkNavigation).WithMany(p => p.CommonIndices)
                .HasForeignKey(d => d.IndexGroupFk)
                .HasConstraintName("FK_common_index_group_TO_common_index");
        });

        modelBuilder.Entity<CommonIndexGroup>(entity =>
        {
            entity.HasKey(e => e.IndexGroupId).IsClustered(false);

            entity.ToTable("common_index_group", tb => tb.HasComment("공통 인덱스 그룹"));

            entity.Property(e => e.IndexGroupId)
                .ValueGeneratedNever()
                .HasComment("그룹ID")
                .HasColumnName("index_group_id");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("영문명")
                .HasColumnName("e_name");
            entity.Property(e => e.IsCreate)
                .HasComment("생성여부")
                .HasColumnName("is_create");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("그룹명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<CompanyCode>(entity =>
        {
            entity.ToTable("company_code", tb => tb.HasComment("회사 정보"));

            entity.Property(e => e.CompanyCodeId)
                .ValueGeneratedNever()
                .HasComment("회사 정보 ID")
                .HasColumnName("company_code_id");
            entity.Property(e => e.BusinessNo)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("사업자등록번호")
                .HasColumnName("business_no");
            entity.Property(e => e.CompanyNo)
                .HasComment("회사코드")
                .HasColumnName("company_no");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("회사명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<ComputerGroup>(entity =>
        {
            entity.HasKey(e => e.ComputerGroupId).IsClustered(false);

            entity.ToTable("computer_group", tb => tb.HasComment("컴퓨터 그룹"));

            entity.Property(e => e.ComputerGroupId)
                .ValueGeneratedNever()
                .HasComment("컴퓨터 그룹ID")
                .HasColumnName("computer_group_id");
            entity.Property(e => e.Description)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("설명")
                .HasColumnName("description");
            entity.Property(e => e.IsDup)
                .HasComment("이중화구성여부")
                .HasColumnName("is_dup");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("컴퓨터 그룹명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<ComputerInfo>(entity =>
        {
            entity.HasKey(e => e.ComputerId);

            entity.ToTable("computer_info", tb => tb.HasComment("컴퓨터 정보"));

            entity.Property(e => e.ComputerId)
                .ValueGeneratedNever()
                .HasComment("컴퓨터 ID")
                .HasColumnName("computer_id");
            entity.Property(e => e.AlarmPriorityFk)
                .HasComment("알람 우선순위 ID")
                .HasColumnName("alarm_priority_fk");
            entity.Property(e => e.ComputerGroupFk)
                .HasComment("컴퓨터 그룹ID")
                .HasColumnName("computer_group_fk");
            entity.Property(e => e.MemberOfficeFk)
                .HasComment("지역 코드")
                .HasColumnName("member_office_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("컴퓨터명")
                .HasColumnName("name");
            entity.Property(e => e.OsName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("OS 이름")
                .HasColumnName("os_name");
            entity.Property(e => e.OsVersion)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("OS 버젼")
                .HasColumnName("os_version");
            entity.Property(e => e.StateGroupFk)
                .HasComment("상태 그룹 ID")
                .HasColumnName("state_group_fk");
            entity.Property(e => e.UseFlag)
                .HasComment("사용 여부")
                .HasColumnName("use_flag");

            entity.HasOne(d => d.AlarmPriorityFkNavigation).WithMany(p => p.ComputerInfos)
                .HasForeignKey(d => d.AlarmPriorityFk)
                .HasConstraintName("FK_alarm_priority_TO_computer_info");

            entity.HasOne(d => d.ComputerGroupFkNavigation).WithMany(p => p.ComputerInfos)
                .HasForeignKey(d => d.ComputerGroupFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_computer_group_TO_computer_info");

            entity.HasOne(d => d.MemberOfficeFkNavigation).WithMany(p => p.ComputerInfos)
                .HasForeignKey(d => d.MemberOfficeFk)
                .HasConstraintName("FK_computer_info_member_office_fk");

            entity.HasOne(d => d.StateGroupFkNavigation).WithMany(p => p.ComputerInfos)
                .HasForeignKey(d => d.StateGroupFk)
                .HasConstraintName("FK_state_group_TO_computer_info");
        });

        modelBuilder.Entity<ComputerInfoView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("computer_info_view");

            entity.Property(e => e.AlarmPriorityFk).HasColumnName("alarm_priority_fk");
            entity.Property(e => e.ComputerGroupFk).HasColumnName("computer_group_fk");
            entity.Property(e => e.ComputerId).HasColumnName("computer_id");
            entity.Property(e => e.DpName)
                .HasMaxLength(42)
                .IsUnicode(false)
                .HasColumnName("dp_name");
            entity.Property(e => e.DpType)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("dp_type");
            entity.Property(e => e.GroupName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("group_name");
            entity.Property(e => e.IsDup).HasColumnName("is_dup");
            entity.Property(e => e.MemberOfficeFk).HasColumnName("member_office_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OsName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("os_name");
            entity.Property(e => e.OsVersion)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("os_version");
            entity.Property(e => e.StateGroupFk).HasColumnName("state_group_fk");
            entity.Property(e => e.UseFlag).HasColumnName("use_flag");
        });

        modelBuilder.Entity<ComputerState>(entity =>
        {
            entity.HasKey(e => e.ComputerFk);

            entity.ToTable("computer_state", tb => tb.HasComment("컴퓨터 상태"));

            entity.Property(e => e.ComputerFk)
                .ValueGeneratedNever()
                .HasComment("컴퓨터 ID")
                .HasColumnName("computer_fk");
            entity.Property(e => e.ActiveState)
                .HasComment("활성화 상태(Active)")
                .HasColumnName("active_state");
            entity.Property(e => e.CpuRate)
                .HasComment("CPU 사용률(%)")
                .HasColumnName("cpu_rate");
            entity.Property(e => e.DiskTotal)
                .HasComment("전체 디스크 크기(MB)")
                .HasColumnName("disk_total");
            entity.Property(e => e.DiskUsage)
                .HasComment("사용 디스크 크기(MB)")
                .HasColumnName("disk_usage");
            entity.Property(e => e.MemTotal)
                .HasComment("전체 메모리 크기(MB)")
                .HasColumnName("mem_total");
            entity.Property(e => e.MemUsage)
                .HasComment("사용 메모리 크기(MB)")
                .HasColumnName("mem_usage");
            entity.Property(e => e.Status)
                .HasComment("상태")
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasComment("갱신 시간")
                .HasColumnType("datetime")
                .HasColumnName("update_time");

            entity.HasOne(d => d.ComputerFkNavigation).WithOne(p => p.ComputerState)
                .HasForeignKey<ComputerState>(d => d.ComputerFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_computer_info_TO_computer_state");
        });

        modelBuilder.Entity<ConductingEquipment>(entity =>
        {
            entity.HasKey(e => e.Mrid);

            entity.ToTable("conducting_equipment", tb => tb.HasComment("설비 정보"));

            entity.Property(e => e.Mrid)
                .ValueGeneratedNever()
                .HasComment("설비 ID")
                .HasColumnName("mrid");
            entity.Property(e => e.CircuitNo)
                .HasComment("회로번호")
                .HasColumnName("circuit_no");
            entity.Property(e => e.DeviceFk)
                .HasComment("통신기기ID")
                .HasColumnName("device_fk");
            entity.Property(e => e.StationMrfk)
                .HasComment("소속 스테이션 ID")
                .HasColumnName("station_mrfk");

            entity.HasOne(d => d.DeviceFkNavigation).WithMany(p => p.ConductingEquipments)
                .HasForeignKey(d => d.DeviceFk)
                .HasConstraintName("FK_device_comm_unit_TO_conducting_equipment");

            entity.HasOne(d => d.StationMrfkNavigation).WithMany(p => p.ConductingEquipments)
                .HasForeignKey(d => d.StationMrfk)
                .HasConstraintName("FK_conducting_equipment_station_mrfk");
        });

        modelBuilder.Entity<ConductingEquipmentView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("conducting_equipment_view");

            entity.Property(e => e.CeqAliasName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("ceq_alias_name");
            entity.Property(e => e.CeqId).HasColumnName("ceq_id");
            entity.Property(e => e.CeqName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("ceq_name");
            entity.Property(e => e.CeqTypeFk).HasColumnName("ceq_type_fk");
            entity.Property(e => e.CircuitNo).HasColumnName("circuit_no");
            entity.Property(e => e.DeviceFk).HasColumnName("device_fk");
            entity.Property(e => e.DpName)
                .HasMaxLength(189)
                .IsUnicode(false)
                .HasColumnName("dp_name");
            entity.Property(e => e.DpType)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("dp_type");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.ModelName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("model_name");
            entity.Property(e => e.ObjectType).HasColumnName("object_type");
            entity.Property(e => e.ObjectTypeEname)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("object_type_ename");
            entity.Property(e => e.ObjectTypeName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("object_type_name");
            entity.Property(e => e.OfficeCode).HasColumnName("office_code");
            entity.Property(e => e.StationMrfk).HasColumnName("station_mrfk");
            entity.Property(e => e.StationName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("station_name");
            entity.Property(e => e.StnTypeCode)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("stn_type_code");
            entity.Property(e => e.TypeCode)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("type_code");
        });

        modelBuilder.Entity<CounterIndex>(entity =>
        {
            entity.HasKey(e => e.IndexId);

            entity.ToTable("counter_index", tb => tb.HasComment("카운터 표준 인텍스"));

            entity.Property(e => e.IndexId)
                .ValueGeneratedNever()
                .HasComment("카운터 표준 인텍스 ID")
                .HasColumnName("index_id");
            entity.Property(e => e.AlarmPriorityFk)
                .HasComment("알람 우선순위 ID")
                .HasColumnName("alarm_priority_fk");
            entity.Property(e => e.DataTypeFk)
                .HasComment("데이터 타입 ID")
                .HasColumnName("data_type_fk");
            entity.Property(e => e.Deadband)
                .HasComment("deadband 값")
                .HasColumnName("deadband");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("카운터 영문 인덱스명")
                .HasColumnName("e_name");
            entity.Property(e => e.LimitMaxValue)
                .HasComment("최대 LIMIT 값")
                .HasColumnName("limit_max_value");
            entity.Property(e => e.LimitMinValue)
                .HasComment("최소 LIMIT 값")
                .HasColumnName("limit_min_value");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("카운터 표준 인텍스명")
                .HasColumnName("name");
            entity.Property(e => e.ScaleFactorFk)
                .HasComment("스케일 펙터 ID")
                .HasColumnName("scale_factor_fk");
            entity.Property(e => e.StateGroupFk)
                .HasComment("상태 그룹 ID")
                .HasColumnName("state_group_fk");
            entity.Property(e => e.UnitFk)
                .HasComment("단위 ID")
                .HasColumnName("unit_fk");

            entity.HasOne(d => d.AlarmPriorityFkNavigation).WithMany(p => p.CounterIndices)
                .HasForeignKey(d => d.AlarmPriorityFk)
                .HasConstraintName("FK_counter_index_alarm_priority_fk");

            entity.HasOne(d => d.DataTypeFkNavigation).WithMany(p => p.CounterIndices)
                .HasForeignKey(d => d.DataTypeFk)
                .HasConstraintName("FK_counter_index_data_type_fk");

            entity.HasOne(d => d.ScaleFactorFkNavigation).WithMany(p => p.CounterIndices)
                .HasForeignKey(d => d.ScaleFactorFk)
                .HasConstraintName("FK_counter_index_scale_factor_fk");

            entity.HasOne(d => d.StateGroupFkNavigation).WithMany(p => p.CounterIndices)
                .HasForeignKey(d => d.StateGroupFk)
                .HasConstraintName("FK_counter_index_state_group_fk");

            entity.HasOne(d => d.UnitFkNavigation).WithMany(p => p.CounterIndices)
                .HasForeignKey(d => d.UnitFk)
                .HasConstraintName("FK_counter_index_unit_fk");
        });

        modelBuilder.Entity<DataType>(entity =>
        {
            entity.ToTable("data_type", tb => tb.HasComment("데이터 타입"));

            entity.Property(e => e.DataTypeId)
                .ValueGeneratedNever()
                .HasComment("데이터 타입 ID")
                .HasColumnName("data_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("데이터 타입명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<DerType>(entity =>
        {
            entity.ToTable("der_type", tb => tb.HasComment("분산전원 타입"));

            entity.Property(e => e.DerTypeId)
                .ValueGeneratedNever()
                .HasComment("분산전원 타입 ID")
                .HasColumnName("der_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("분산전원명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<DeviceCommConfig>(entity =>
        {
            entity.HasKey(e => e.DeviceFk).IsClustered(false);

            entity.ToTable("device_comm_config", tb => tb.HasComment("원격통신기기설정"));

            entity.Property(e => e.DeviceFk)
                .ValueGeneratedNever()
                .HasComment("통신기기ID")
                .HasColumnName("device_fk");
            entity.Property(e => e.AppRetryCount)
                .HasComment("재시도 수행횟수")
                .HasColumnName("app_retry_count");
            entity.Property(e => e.AppRetryTimeout)
                .HasComment("재시도 타임아웃")
                .HasColumnName("app_retry_timeout");
            entity.Property(e => e.AppSendTimeout)
                .HasComment("전송 타임아웃")
                .HasColumnName("app_send_timeout");
            entity.Property(e => e.CommStatusInterval)
                .HasComment("통신상태전송주기")
                .HasColumnName("comm_status_interval");
            entity.Property(e => e.EventInterval)
                .HasComment("이벤트 계측 주기")
                .HasColumnName("event_interval");
            entity.Property(e => e.ParameterProperty)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("파라메터 속성값")
                .HasColumnName("parameter_property");
            entity.Property(e => e.TotalInterval)
                .HasComment("전체계측 주기")
                .HasColumnName("total_interval");

            entity.HasOne(d => d.DeviceFkNavigation).WithOne(p => p.DeviceCommConfig)
                .HasForeignKey<DeviceCommConfig>(d => d.DeviceFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_device_comm_unit_TO_device_comm_config");
        });

        modelBuilder.Entity<DeviceCommUnit>(entity =>
        {
            entity.HasKey(e => e.DeviceId).IsClustered(false);

            entity.ToTable("device_comm_unit", tb => tb.HasComment("원격통신기기"));

            entity.Property(e => e.DeviceId)
                .ValueGeneratedNever()
                .HasComment("통신기기ID")
                .HasColumnName("device_id");
            entity.Property(e => e.CommTypeFk)
                .HasComment("통신 타입 ID")
                .HasColumnName("comm_type_fk");
            entity.Property(e => e.FepId)
                .HasComment("FEP 프로그램 ID")
                .HasColumnName("fep_id");
            entity.Property(e => e.FrtuAddr)
                .HasComment("FRTU 주소")
                .HasColumnName("frtu_addr");
            entity.Property(e => e.MasterAddr)
                .HasComment("마스터주소")
                .HasColumnName("master_addr");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("이름")
                .HasColumnName("name");
            entity.Property(e => e.SimulAddress)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("시뮬 연결 TCP 주소")
                .HasColumnName("simul_address");
            entity.Property(e => e.SimulPort)
                .HasComment("시뮬 연결 TCP 포트")
                .HasColumnName("simul_port");
            entity.Property(e => e.SimulUsage)
                .HasComment("시뮬레이터 사용여부")
                .HasColumnName("simul_usage");
            entity.Property(e => e.TcpAddress)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("원격 연결 TCP 주소")
                .HasColumnName("tcp_address");
            entity.Property(e => e.TcpPort)
                .HasComment("원격 연결 TCP 포트")
                .HasColumnName("tcp_port");

            entity.HasOne(d => d.CommTypeFkNavigation).WithMany(p => p.DeviceCommUnits)
                .HasForeignKey(d => d.CommTypeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comm_type_TO_device_comm_unit");
        });

        modelBuilder.Entity<DevicePointIndexView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("device_point_index_view");

            entity.Property(e => e.AlarmPriority).HasColumnName("alarm_priority");
            entity.Property(e => e.BitPosition).HasColumnName("bit_position");
            entity.Property(e => e.CeqTypeId).HasColumnName("ceq_type_id");
            entity.Property(e => e.CeqTypeName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("ceq_type_name");
            entity.Property(e => e.CircuitNo).HasColumnName("circuit_no");
            entity.Property(e => e.ClassNo).HasColumnName("class_no");
            entity.Property(e => e.DataTypeId).HasColumnName("data_type_id");
            entity.Property(e => e.DynamicIndex).HasColumnName("dynamic_index");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("e_name");
            entity.Property(e => e.LimitMaxValue).HasColumnName("limit_max_value");
            entity.Property(e => e.LimitMinValue).HasColumnName("limit_min_value");
            entity.Property(e => e.ModbusAddress).HasColumnName("modbus_address");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Offset).HasColumnName("offset");
            entity.Property(e => e.PointIndex).HasColumnName("point_index");
            entity.Property(e => e.PointTypeId).HasColumnName("point_type_id");
            entity.Property(e => e.RemoteAddress)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("remote_address");
            entity.Property(e => e.Scale).HasColumnName("scale");
            entity.Property(e => e.StateGroupId).HasColumnName("state_group_id");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");
        });

        modelBuilder.Entity<GroupOffice>(entity =>
        {
            entity.HasKey(e => e.GroupOfficeId).IsClustered(false);

            entity.ToTable("group_office", tb => tb.HasComment("그룹 사업소"));

            entity.Property(e => e.GroupOfficeId)
                .ValueGeneratedNever()
                .HasComment("그룹사업소 ID")
                .HasColumnName("group_office_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("이름")
                .HasColumnName("name");
        });

        modelBuilder.Entity<IdentityObject>(entity =>
        {
            entity.HasKey(e => e.Mrid);

            entity.ToTable("identity_object", tb => tb.HasComment("개체 식별 정보"));

            entity.Property(e => e.Mrid)
                .ValueGeneratedNever()
                .HasComment("개체 ID")
                .HasColumnName("mrid");
            entity.Property(e => e.AliasName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("별칭")
                .HasColumnName("alias_name");
            entity.Property(e => e.ModelFk)
                .HasComment("연결 모델 ID")
                .HasColumnName("model_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("개체명")
                .HasColumnName("name");

            entity.HasOne(d => d.ModelFkNavigation).WithMany(p => p.IdentityObjects)
                .HasForeignKey(d => d.ModelFk)
                .HasConstraintName("FK_identity_object_model_fk");
        });

        modelBuilder.Entity<LogControl>(entity =>
        {
            entity.HasKey(e => e.LogId).IsClustered(false);

            entity.ToTable("log_control", tb => tb.HasComment("제어 수행로그"));

            entity.Property(e => e.LogId)
                .HasComment("로그ID")
                .HasColumnName("log_id");
            entity.Property(e => e.CeqFk)
                .HasComment("설비 ID")
                .HasColumnName("ceq_fk");
            entity.Property(e => e.CeqTypeFk)
                .HasComment("설비 타입 ID")
                .HasColumnName("ceq_type_fk");
            entity.Property(e => e.ControlResult)
                .HasComment("제어 결과(0:실패, 1:성공)")
                .HasColumnName("control_result");
            entity.Property(e => e.ControlResultMessage)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("제어 결과 메시지")
                .HasColumnName("control_result_message");
            entity.Property(e => e.ControlResultTime)
                .HasComment("제어 결과 시간")
                .HasColumnType("datetime")
                .HasColumnName("control_result_time");
            entity.Property(e => e.ControlTime)
                .HasComment("제어 수행 시간")
                .HasColumnType("datetime")
                .HasColumnName("control_time");
            entity.Property(e => e.ControlUserFk)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("사용자 ID")
                .HasColumnName("control_user_fk");
            entity.Property(e => e.DdataIndexFk)
                .HasComment("동적인덱스 ID")
                .HasColumnName("ddata_index_fk");
            entity.Property(e => e.DeviceCeqFk)
                .HasComment("연결 기기 ID")
                .HasColumnName("device_ceq_fk");
            entity.Property(e => e.MemberOfficeFk)
                .HasComment("지역 코드")
                .HasColumnName("member_office_fk");
            entity.Property(e => e.NewValue)
                .HasComment("현재값")
                .HasColumnName("new_value");
            entity.Property(e => e.PointIndexFk)
                .HasComment("포인트 인덱스 ID")
                .HasColumnName("point_index_fk");
            entity.Property(e => e.PointTypeFk)
                .HasComment("포인트 타입 ID")
                .HasColumnName("point_type_fk");
            entity.Property(e => e.StationFk)
                .HasComment("변전소 ID")
                .HasColumnName("station_fk");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("업데이트 시간(저장시간)")
                .HasColumnType("datetime")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<LogDeviceComm>(entity =>
        {
            entity.HasKey(e => e.LogId).IsClustered(false);

            entity.ToTable("log_device_comm", tb => tb.HasComment("원격 설비 통신 로그"));

            entity.Property(e => e.LogId)
                .ValueGeneratedNever()
                .HasComment("로그ID")
                .HasColumnName("log_id");
            entity.Property(e => e.CommFailCnt)
                .HasComment("통신 실패 개수")
                .HasColumnName("comm_fail_cnt");
            entity.Property(e => e.CommNoResponseCnt)
                .HasComment("무응답 개수")
                .HasColumnName("comm_no_response_cnt");
            entity.Property(e => e.CommState)
                .HasComment("통신상태")
                .HasColumnName("comm_state");
            entity.Property(e => e.CommSucessCnt)
                .HasComment("통신 성공 개수")
                .HasColumnName("comm_sucess_cnt");
            entity.Property(e => e.CommTotalCnt)
                .HasComment("전체 통신 수행 횟수")
                .HasColumnName("comm_total_cnt");
            entity.Property(e => e.DeviceCeqFk)
                .HasComment("연결 기기 ID")
                .HasColumnName("device_ceq_fk");
            entity.Property(e => e.MemberOfficeFk)
                .HasComment("지역 코드")
                .HasColumnName("member_office_fk");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("업데이트 시간(저장시간)")
                .HasColumnType("datetime")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<LogEvent>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("log_event", tb => tb.HasComment("이벤트 로그"));

            entity.Property(e => e.LogId)
                .HasComment("로그 ID")
                .HasColumnName("log_id");
            entity.Property(e => e.Ack)
                .HasComment("ACK")
                .HasColumnName("ack");
            entity.Property(e => e.AckTime)
                .HasComment("ACK 타임")
                .HasColumnType("datetime")
                .HasColumnName("ack_time");
            entity.Property(e => e.AckUserFk)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("ACK 사용자 ID")
                .HasColumnName("ack_user_fk");
            entity.Property(e => e.AlarmPriority)
                .HasComment("알람 우선순위")
                .HasColumnName("alarm_priority");
            entity.Property(e => e.CeqFk)
                .HasComment("설비 ID")
                .HasColumnName("ceq_fk");
            entity.Property(e => e.CeqTypeFk)
                .HasComment("설비 타입 ID")
                .HasColumnName("ceq_type_fk");
            entity.Property(e => e.CircuitNo)
                .HasComment("회로번호")
                .HasColumnName("circuit_no");
            entity.Property(e => e.DdataIndexFk)
                .HasComment("동적인덱스 ID")
                .HasColumnName("ddata_index_fk");
            entity.Property(e => e.DeviceCeqFk)
                .HasComment("연결 기기 ID")
                .HasColumnName("device_ceq_fk");
            entity.Property(e => e.DeviceEventTime)
                .HasComment("연결 기기 발생 시간")
                .HasColumnType("datetime")
                .HasColumnName("device_event_time");
            entity.Property(e => e.EventCreateTime)
                .HasComment("서버 발생 시간")
                .HasColumnType("datetime")
                .HasColumnName("event_create_time");
            entity.Property(e => e.EventId)
                .HasComment("이벤트 ID")
                .HasColumnName("event_id");
            entity.Property(e => e.EventMsg)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("메시지")
                .HasColumnName("event_msg");
            entity.Property(e => e.EventTypeFk)
                .HasComment("이벤트 타입")
                .HasColumnName("event_type_fk");
            entity.Property(e => e.MemberOfficeFk)
                .HasComment("지역 코드")
                .HasColumnName("member_office_fk");
            entity.Property(e => e.NewValue)
                .HasComment("현재값")
                .HasColumnName("new_value");
            entity.Property(e => e.OldValue)
                .HasComment("이전값")
                .HasColumnName("old_value");
            entity.Property(e => e.PointIndexFk)
                .HasComment("포인트 인덱스 ID")
                .HasColumnName("point_index_fk");
            entity.Property(e => e.PointTypeFk)
                .HasComment("포인트 타입 ID")
                .HasColumnName("point_type_fk");
            entity.Property(e => e.QualityValue)
                .HasComment("Quality 값")
                .HasColumnName("quality_value");
            entity.Property(e => e.StationFk)
                .HasComment("변전소 ID")
                .HasColumnName("station_fk");
            entity.Property(e => e.TagValue)
                .HasComment("TAG 값")
                .HasColumnName("tag_value");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("업데이트 시간(저장시간)")
                .HasColumnType("datetime")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<LogSystem>(entity =>
        {
            entity.HasKey(e => e.LogId).IsClustered(false);

            entity.ToTable("log_system", tb => tb.HasComment("시스템 로그"));

            entity.Property(e => e.LogId)
                .HasComment("로그ID")
                .HasColumnName("log_id");
            entity.Property(e => e.ComputerFk)
                .HasComment("컴퓨터 ID")
                .HasColumnName("computer_fk");
            entity.Property(e => e.ControlUserFk)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("사용자 ID")
                .HasColumnName("control_user_fk");
            entity.Property(e => e.MemberOfficeFk)
                .HasComment("지역 코드")
                .HasColumnName("member_office_fk");
            entity.Property(e => e.MsgResult)
                .HasComment("메시지 수행 결과 코드(0:실패, 1:성공)")
                .HasColumnName("msg_result");
            entity.Property(e => e.MsgResultMessage)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("결과 메시지")
                .HasColumnName("msg_result_message");
            entity.Property(e => e.MsgTypeId)
                .HasComment("메시지 타입 ID")
                .HasColumnName("msg_type_id");
            entity.Property(e => e.ProgramId)
                .HasComment("프로그램 ID")
                .HasColumnName("program_id");
            entity.Property(e => e.UpdateTime)
                .HasDefaultValueSql("(getdate())")
                .HasComment("업데이트 시간(저장시간)")
                .HasColumnType("datetime")
                .HasColumnName("update_time");
        });

        modelBuilder.Entity<MasterIndex>(entity =>
        {
            entity.ToTable("master_index", tb => tb.HasComment("마스터 인덱스"));

            entity.Property(e => e.MasterIndexId)
                .ValueGeneratedNever()
                .HasComment("마스터 인덱스 ID")
                .HasColumnName("master_index_id");
            entity.Property(e => e.AlarmPriorityFk)
                .HasComment("알람 우선순위 ID")
                .HasColumnName("alarm_priority_fk");
            entity.Property(e => e.BitPosition)
                .HasComment("비트 포지션")
                .HasColumnName("bit_position");
            entity.Property(e => e.CeqTypeFk)
                .HasComment("설비 종류 ID")
                .HasColumnName("ceq_type_fk");
            entity.Property(e => e.CircuitNo)
                .HasComment("회로 번호")
                .HasColumnName("circuit_no");
            entity.Property(e => e.ClassNo)
                .HasComment("클래스번호_REG_TYPE")
                .HasColumnName("class_no");
            entity.Property(e => e.DataTypeFk)
                .HasComment("데이터 타입 ID")
                .HasColumnName("data_type_fk");
            entity.Property(e => e.Deadband)
                .HasComment("deadband 값")
                .HasColumnName("deadband");
            entity.Property(e => e.DefaultVal)
                .HasComment("AO 포인트 설정 기본값")
                .HasColumnName("default_val");
            entity.Property(e => e.ExistOff)
                .HasComment("AO 포인트 설정 OFF 존재 여부")
                .HasColumnName("exist_off");
            entity.Property(e => e.IndexFk)
                .HasComment("포인트 타입별 인덱스 ID")
                .HasColumnName("index_fk");
            entity.Property(e => e.LimitMaxValue)
                .HasComment("최대 LIMIT 값")
                .HasColumnName("limit_max_value");
            entity.Property(e => e.LimitMinValue)
                .HasComment("최소 LIMIT 값")
                .HasColumnName("limit_min_value");
            entity.Property(e => e.MaxVal)
                .HasComment("AO 포인트 설정 최대 값")
                .HasColumnName("max_val");
            entity.Property(e => e.MinVal)
                .HasComment("AO 포인트 설정 최소 값")
                .HasColumnName("min_val");
            entity.Property(e => e.ModbusAddress)
                .HasComment("모드버스ADDR")
                .HasColumnName("modbus_address");
            entity.Property(e => e.ObjVar)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("object_variation")
                .HasColumnName("obj_var");
            entity.Property(e => e.OffVal)
                .HasComment("AO 포인트 설정 OFF 값")
                .HasColumnName("off_val");
            entity.Property(e => e.PointTypeFk)
                .HasComment("포인트 타입")
                .HasColumnName("point_type_fk");
            entity.Property(e => e.RemoteAddress)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("프로토콜 의존적인 Remote Address 정보")
                .HasColumnName("remote_address");
            entity.Property(e => e.ScaleFactorFk)
                .HasComment("스케일 펙터 ID")
                .HasColumnName("scale_factor_fk");
            entity.Property(e => e.StateGroupFk)
                .HasComment("상태 그룹 ID")
                .HasColumnName("state_group_fk");
            entity.Property(e => e.StepVal)
                .HasComment("AO 포인트 설정 STEP 값")
                .HasColumnName("step_val");
            entity.Property(e => e.UnitFk)
                .HasComment("단위 ID")
                .HasColumnName("unit_fk");

            entity.HasOne(d => d.AlarmPriorityFkNavigation).WithMany(p => p.MasterIndices)
                .HasForeignKey(d => d.AlarmPriorityFk)
                .HasConstraintName("FK_master_index_alarm_priority_fk");

            entity.HasOne(d => d.CeqTypeFkNavigation).WithMany(p => p.MasterIndices)
                .HasForeignKey(d => d.CeqTypeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_master_index_ceq_type_fk");

            entity.HasOne(d => d.DataTypeFkNavigation).WithMany(p => p.MasterIndices)
                .HasForeignKey(d => d.DataTypeFk)
                .HasConstraintName("FK_master_index_data_type_fk");

            entity.HasOne(d => d.PointTypeFkNavigation).WithMany(p => p.MasterIndices)
                .HasForeignKey(d => d.PointTypeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_master_index_point_type_fk");

            entity.HasOne(d => d.ScaleFactorFkNavigation).WithMany(p => p.MasterIndices)
                .HasForeignKey(d => d.ScaleFactorFk)
                .HasConstraintName("FK_master_index_scale_factor_fk");

            entity.HasOne(d => d.StateGroupFkNavigation).WithMany(p => p.MasterIndices)
                .HasForeignKey(d => d.StateGroupFk)
                .HasConstraintName("FK_master_index_state_group_fk");

            entity.HasOne(d => d.UnitFkNavigation).WithMany(p => p.MasterIndices)
                .HasForeignKey(d => d.UnitFk)
                .HasConstraintName("FK_master_index_unit_fk");
        });

        modelBuilder.Entity<MemberOffice>(entity =>
        {
            entity.ToTable("member_office", tb => tb.HasComment("지역(사업소) 정보"));

            entity.Property(e => e.MemberOfficeId)
                .ValueGeneratedNever()
                .HasComment("지역(사업소) 코드")
                .HasColumnName("member_office_id");
            entity.Property(e => e.GroupOfficeFk)
                .HasComment("그룹사업소 ID")
                .HasColumnName("group_office_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("지역(사업소) 명")
                .HasColumnName("name");
            entity.Property(e => e.OfficeType)
                .HasComment("지역(사업소) 타입")
                .HasColumnName("office_type");

            entity.HasOne(d => d.GroupOfficeFkNavigation).WithMany(p => p.MemberOffices)
                .HasForeignKey(d => d.GroupOfficeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_group_office_TO_member_office");
        });

        modelBuilder.Entity<ModbusSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).IsClustered(false);

            entity.ToTable("modbus_schedule", tb => tb.HasComment("모드버스 스케쥴"));

            entity.Property(e => e.ScheduleId)
                .HasComment("스케쥴ID")
                .HasColumnName("schedule_id");
            entity.Property(e => e.CeqTypeId)
                .HasComment("설비 타입 ID")
                .HasColumnName("ceq_type_id");
            entity.Property(e => e.FunctionCode)
                .HasComment("함수코드")
                .HasColumnName("function_code");
            entity.Property(e => e.MeasureCount)
                .HasComment("계측 개수")
                .HasColumnName("measure_count");
            entity.Property(e => e.OrderNo)
                .HasComment("계측 순서")
                .HasColumnName("order_no");
            entity.Property(e => e.StartAddress)
                .HasComment("시작주소")
                .HasColumnName("start_address");

            entity.HasOne(d => d.CeqType).WithMany(p => p.ModbusSchedules)
                .HasForeignKey(d => d.CeqTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ceq_type_TO_modbus_schedule");
        });

        modelBuilder.Entity<ModelIndex>(entity =>
        {
            entity.HasKey(e => new { e.ModelFk, e.ItemFk });

            entity.ToTable("model_index", tb => tb.HasComment("모델 인덱스 정보"));

            entity.Property(e => e.ModelFk)
                .HasComment("모델 ID")
                .HasColumnName("model_fk");
            entity.Property(e => e.ItemFk)
                .HasComment("모델 아이템 ID")
                .HasColumnName("item_fk");
            entity.Property(e => e.Seq)
                .HasComment("표시 순서")
                .HasColumnName("seq");
            entity.Property(e => e.Value)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("값")
                .HasColumnName("value");

            entity.HasOne(d => d.ItemFkNavigation).WithMany(p => p.ModelIndices)
                .HasForeignKey(d => d.ItemFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_model_index_item_fk");

            entity.HasOne(d => d.ModelFkNavigation).WithMany(p => p.ModelIndices)
                .HasForeignKey(d => d.ModelFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_model_index_model_fk");
        });

        modelBuilder.Entity<ModelInfo>(entity =>
        {
            entity.HasKey(e => e.ModelId);

            entity.ToTable("model_info", tb => tb.HasComment("모델 정보"));

            entity.Property(e => e.ModelId)
                .ValueGeneratedNever()
                .HasComment("모델 ID")
                .HasColumnName("model_id");
            entity.Property(e => e.CeqTypeFk)
                .HasComment("설비 타입 ID")
                .HasColumnName("ceq_type_fk");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("설명")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("모델명")
                .HasColumnName("name");
            entity.Property(e => e.ObjectTypeFk)
                .HasComment("개체 타입 ID")
                .HasColumnName("object_type_fk");

            entity.HasOne(d => d.CeqTypeFkNavigation).WithMany(p => p.ModelInfos)
                .HasForeignKey(d => d.CeqTypeFk)
                .HasConstraintName("FK_ceq_type_TO_model_info");

            entity.HasOne(d => d.ObjectTypeFkNavigation).WithMany(p => p.ModelInfos)
                .HasForeignKey(d => d.ObjectTypeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_model_info_object_type_fk");
        });

        modelBuilder.Entity<ModelItemIndex>(entity =>
        {
            entity.HasKey(e => e.IndexId);

            entity.ToTable("model_item_index", tb => tb.HasComment("모델 아이템 정보"));

            entity.Property(e => e.IndexId)
                .ValueGeneratedNever()
                .HasComment("모델 아이템 ID")
                .HasColumnName("index_id");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("모델 아이템 영문명")
                .HasColumnName("e_name");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("모델 아이템명")
                .HasColumnName("name");
            entity.Property(e => e.UnitFk)
                .HasComment("단위 ID")
                .HasColumnName("unit_fk");

            entity.HasOne(d => d.UnitFkNavigation).WithMany(p => p.ModelItemIndices)
                .HasForeignKey(d => d.UnitFk)
                .HasConstraintName("FK_model_item_index_unit_fk");
        });

        modelBuilder.Entity<MsgErrorType>(entity =>
        {
            entity.HasKey(e => e.MsgErrorTypeId).HasName("PK_error_msg_type");

            entity.ToTable("msg_error_type", tb => tb.HasComment("메시지 결과 에러 코드"));

            entity.Property(e => e.MsgErrorTypeId)
                .ValueGeneratedNever()
                .HasComment("에러 메시지 코드 ID")
                .HasColumnName("msg_error_type_id");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("설명")
                .HasColumnName("description");
            entity.Property(e => e.MsgErrorCode)
                .HasComment("메시지 코드")
                .HasColumnName("msg_error_code");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("에러 코드명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<MsgGroup>(entity =>
        {
            entity.ToTable("msg_group", tb => tb.HasComment("메시지 그룹"));

            entity.Property(e => e.MsgGroupId)
                .ValueGeneratedNever()
                .HasComment("메시지 그룹 ID")
                .HasColumnName("msg_group_id");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("설명")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("메시지 그룹명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<MsgType>(entity =>
        {
            entity.ToTable("msg_type", tb => tb.HasComment("메시지 타입"));

            entity.Property(e => e.MsgTypeId)
                .ValueGeneratedNever()
                .HasComment("메시지 타입 ID")
                .HasColumnName("msg_type_id");
            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("설명")
                .HasColumnName("description");
            entity.Property(e => e.MsgCode)
                .HasComment("메시지 타입 코드")
                .HasColumnName("msg_code");
            entity.Property(e => e.MsgGroupFk)
                .HasComment("메시지 그룹")
                .HasColumnName("msg_group_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("메시지 타입명")
                .HasColumnName("name");

            entity.HasOne(d => d.MsgGroupFkNavigation).WithMany(p => p.MsgTypes)
                .HasForeignKey(d => d.MsgGroupFk)
                .HasConstraintName("FK_msg_group_TO_msg_type");
        });

        modelBuilder.Entity<ObjectType>(entity =>
        {
            entity.ToTable("object_type", tb => tb.HasComment("개체 타입"));

            entity.Property(e => e.ObjectTypeId)
                .ValueGeneratedNever()
                .HasComment("개체 타입 ID")
                .HasColumnName("object_type_id");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("e_name");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("개체 타입명")
                .HasColumnName("name");
            entity.Property(e => e.TypeCode)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("타입 코드")
                .HasColumnName("type_code");
        });

        modelBuilder.Entity<PointIndexView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("point_index_view");

            entity.Property(e => e.AlarmPriorityId).HasColumnName("alarm_priority_id");
            entity.Property(e => e.DataTypeId).HasColumnName("data_type_id");
            entity.Property(e => e.DynamicIndex).HasColumnName("dynamic_index");
            entity.Property(e => e.EName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("e_name");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PointTypeId).HasColumnName("point_type_id");
        });

        modelBuilder.Entity<PointQualityCode>(entity =>
        {
            entity.ToTable("point_quality_code", tb => tb.HasComment("포인트 Quality 코드"));

            entity.Property(e => e.PointQualityCodeId)
                .ValueGeneratedNever()
                .HasComment("포인트 Quality 코드 ID")
                .HasColumnName("point_quality_code_id");
            entity.Property(e => e.AliseName)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("별칭")
                .HasColumnName("alise_name");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("포인트 Quality 코드명")
                .HasColumnName("name");
            entity.Property(e => e.Value)
                .HasComment("포인트 Quality 코드 값")
                .HasColumnName("value");
        });

        modelBuilder.Entity<PointTagCode>(entity =>
        {
            entity.ToTable("point_tag_code", tb => tb.HasComment("포인트 TAG 코드"));

            entity.Property(e => e.PointTagCodeId)
                .ValueGeneratedNever()
                .HasComment("포인트 TAG 코드 ID")
                .HasColumnName("point_tag_code_id");
            entity.Property(e => e.AliseName)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("별칭")
                .HasColumnName("alise_name");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("포인트 TAG 코드명")
                .HasColumnName("name");
            entity.Property(e => e.Value)
                .HasComment("포인트 TAG 코드 값")
                .HasColumnName("value");
        });

        modelBuilder.Entity<PointType>(entity =>
        {
            entity.ToTable("point_type", tb => tb.HasComment("포인트 타입 정보"));

            entity.Property(e => e.PointTypeId)
                .ValueGeneratedNever()
                .HasComment("포인트 타입 ID")
                .HasColumnName("point_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("포인트 타입명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<ProgramInfo>(entity =>
        {
            entity.HasKey(e => e.ProgramId);

            entity.ToTable("program_info", tb => tb.HasComment("프로그램 정보"));

            entity.Property(e => e.ProgramId)
                .ValueGeneratedNever()
                .HasComment("프로그램 ID")
                .HasColumnName("program_id");
            entity.Property(e => e.AlarmPriorityFk)
                .HasComment("알람 우선순위 ID")
                .HasColumnName("alarm_priority_fk");
            entity.Property(e => e.ComputerFk)
                .HasComment("컴퓨터 ID")
                .HasColumnName("computer_fk");
            entity.Property(e => e.ExecuteType)
                .HasComment("실행타입(0:실행안함, 1:콘솔명령, 2:윈도우 서비스)")
                .HasColumnName("execute_type");
            entity.Property(e => e.IpAddr)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("IP ADDR")
                .HasColumnName("ip_addr");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("프로그램명")
                .HasColumnName("name");
            entity.Property(e => e.ProcFullName)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("파일이름(경로포함)")
                .HasColumnName("proc_full_name");
            entity.Property(e => e.ProgramDesc)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("프로그램 설명(버젼정보)")
                .HasColumnName("program_desc");
            entity.Property(e => e.ProgramNo)
                .HasComment("프로그램번호")
                .HasColumnName("program_no");
            entity.Property(e => e.ProgramTypeFk)
                .HasComment("프로그램 타입 ID")
                .HasColumnName("program_type_fk");
            entity.Property(e => e.StartCmd)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("시작 명령")
                .HasColumnName("start_cmd");
            entity.Property(e => e.StateGroupFk)
                .HasComment("상태 그룹 ID")
                .HasColumnName("state_group_fk");
            entity.Property(e => e.StopCmd)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("종료 명령")
                .HasColumnName("stop_cmd");
            entity.Property(e => e.TcpPort)
                .HasComment("TCP PORT")
                .HasColumnName("tcp_port");
            entity.Property(e => e.UpdatePeriod)
                .HasComment("프로그램 상태 갱신주기")
                .HasColumnName("update_period");
            entity.Property(e => e.UseFlag)
                .HasComment("사용 여부")
                .HasColumnName("use_flag");

            entity.HasOne(d => d.AlarmPriorityFkNavigation).WithMany(p => p.ProgramInfos)
                .HasForeignKey(d => d.AlarmPriorityFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_alarm_priority_TO_program_info");

            entity.HasOne(d => d.ComputerFkNavigation).WithMany(p => p.ProgramInfos)
                .HasForeignKey(d => d.ComputerFk)
                .HasConstraintName("FK_program_info_computer_fk");

            entity.HasOne(d => d.ProgramTypeFkNavigation).WithMany(p => p.ProgramInfos)
                .HasForeignKey(d => d.ProgramTypeFk)
                .HasConstraintName("FK_program_info_program_type_fk");

            entity.HasOne(d => d.StateGroupFkNavigation).WithMany(p => p.ProgramInfos)
                .HasForeignKey(d => d.StateGroupFk)
                .HasConstraintName("FK_state_group_TO_program_info");
        });

        modelBuilder.Entity<ProgramInfoView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("program_info_view");

            entity.Property(e => e.AlarmPriorityFk).HasColumnName("alarm_priority_fk");
            entity.Property(e => e.ComputerFk).HasColumnName("computer_fk");
            entity.Property(e => e.ComputerGroupFk).HasColumnName("computer_group_fk");
            entity.Property(e => e.ComputerName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("computer_name");
            entity.Property(e => e.DpName)
                .HasMaxLength(41)
                .IsUnicode(false)
                .HasColumnName("dp_name");
            entity.Property(e => e.DpType)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("dp_type");
            entity.Property(e => e.ExecuteType).HasColumnName("execute_type");
            entity.Property(e => e.IpAddr)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasColumnName("ip_addr");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ProcFullName)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("proc_full_name");
            entity.Property(e => e.ProgramDesc)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("program_desc");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.ProgramNo).HasColumnName("program_no");
            entity.Property(e => e.ProgramTypeFk).HasColumnName("program_type_fk");
            entity.Property(e => e.StartCmd)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("start_cmd");
            entity.Property(e => e.StateGroupFk).HasColumnName("state_group_fk");
            entity.Property(e => e.StopCmd)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("stop_cmd");
            entity.Property(e => e.TcpPort).HasColumnName("tcp_port");
            entity.Property(e => e.UpdatePeriod).HasColumnName("update_period");
            entity.Property(e => e.UseFlag).HasColumnName("use_flag");
        });

        modelBuilder.Entity<ProgramState>(entity =>
        {
            entity.HasKey(e => e.ProgramFk);

            entity.ToTable("program_state", tb => tb.HasComment("프로그램 상태"));

            entity.Property(e => e.ProgramFk)
                .ValueGeneratedNever()
                .HasComment("프로그램 ID")
                .HasColumnName("program_fk");
            entity.Property(e => e.EndTime)
                .HasComment("종료시간")
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.StartTime)
                .HasComment("시작시간")
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasComment("상태")
                .HasColumnName("status");
            entity.Property(e => e.UpdateTime)
                .HasComment("갱신시간")
                .HasColumnType("datetime")
                .HasColumnName("update_time");

            entity.HasOne(d => d.ProgramFkNavigation).WithOne(p => p.ProgramState)
                .HasForeignKey<ProgramState>(d => d.ProgramFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_program_state_program_fk");
        });

        modelBuilder.Entity<ProgramType>(entity =>
        {
            entity.ToTable("program_type", tb => tb.HasComment("프로그램 타입"));

            entity.Property(e => e.ProgramTypeId)
                .ValueGeneratedNever()
                .HasComment("프로그램 타입 ID")
                .HasColumnName("program_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("타입명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<ProtocolType>(entity =>
        {
            entity.ToTable("protocol_type", tb => tb.HasComment("프로토콜 타입"));

            entity.Property(e => e.ProtocolTypeId)
                .ValueGeneratedNever()
                .HasComment("프로토콜 타입 ID")
                .HasColumnName("protocol_type_id");
            entity.Property(e => e.DefaultDeviceCmd)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasComment("Default 설비 명령")
                .HasColumnName("default_device_cmd");
            entity.Property(e => e.DefaultParameter)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasComment("Default 파라메타")
                .HasColumnName("default_parameter");
            entity.Property(e => e.DllDescription)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("DLL 설명")
                .HasColumnName("dll_description");
            entity.Property(e => e.DllFile)
                .HasComment("DLL 파일")
                .HasColumnName("dll_file");
            entity.Property(e => e.DllName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("DLL 이름")
                .HasColumnName("dll_name");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("프로토콜 타입명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<RemoteControlValue>(entity =>
        {
            entity.HasKey(e => e.RemoteControlId).HasName("PK_bo_remote_control_value");

            entity.ToTable("remote_control_value", tb => tb.HasComment("원격 제어 설정 정보"));

            entity.Property(e => e.RemoteControlId)
                .ValueGeneratedNever()
                .HasComment("원격 제어 ID")
                .HasColumnName("remote_control_id");
            entity.Property(e => e.ControlValue)
                .HasComment("원격 제어 값")
                .HasColumnName("control_value");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("원격 제어 명")
                .HasColumnName("name");
            entity.Property(e => e.Value)
                .HasComment("HMI 제어 값")
                .HasColumnName("value");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("report", tb => tb.HasComment("레포트"));

            entity.Property(e => e.ReportId)
                .ValueGeneratedNever()
                .HasComment("레포트 ID")
                .HasColumnName("report_id");
            entity.Property(e => e.CreateTime)
                .HasComment("생성 시간")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.FileName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("파일 이름")
                .HasColumnName("file_name");
            entity.Property(e => e.FormFk)
                .HasComment("레포트 폼 ID")
                .HasColumnName("form_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("레포트명")
                .HasColumnName("name");
            entity.Property(e => e.ReportExcelFile)
                .HasComment("레포트 엑셀 파일")
                .HasColumnName("report_excel_file");
            entity.Property(e => e.ReportPdfFile)
                .HasComment("레포트 PDF 파일")
                .HasColumnName("report_pdf_file");

            entity.HasOne(d => d.FormFkNavigation).WithMany(p => p.Reports)
                .HasForeignKey(d => d.FormFk)
                .HasConstraintName("FK_report_form_fk");
        });

        modelBuilder.Entity<ReportForm>(entity =>
        {
            entity.HasKey(e => e.FormId);

            entity.ToTable("report_form", tb => tb.HasComment("레포트 폼"));

            entity.Property(e => e.FormId)
                .ValueGeneratedNever()
                .HasComment("레포트 폼 ID")
                .HasColumnName("form_id");
            entity.Property(e => e.CreateTime)
                .HasComment("생성 시간")
                .HasColumnType("datetime")
                .HasColumnName("create_time");
            entity.Property(e => e.Description)
                .HasMaxLength(512)
                .IsUnicode(false)
                .HasComment("설명")
                .HasColumnName("description");
            entity.Property(e => e.FileName)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("파일 이름")
                .HasColumnName("file_name");
            entity.Property(e => e.FormFile)
                .HasComment("레포트 폼 파일")
                .HasColumnName("form_file");
            entity.Property(e => e.FormTypeFk)
                .HasComment("레포트 폼 타입 ID")
                .HasColumnName("form_type_fk");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("레포트 폼명")
                .HasColumnName("name");

            entity.HasOne(d => d.FormTypeFkNavigation).WithMany(p => p.ReportForms)
                .HasForeignKey(d => d.FormTypeFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_report_form_form_type_fk");
        });

        modelBuilder.Entity<ReportFormType>(entity =>
        {
            entity.HasKey(e => e.FormTypeId).HasName("PK_form_type");

            entity.ToTable("report_form_type", tb => tb.HasComment("레포트 폼 타입"));

            entity.Property(e => e.FormTypeId)
                .ValueGeneratedNever()
                .HasComment("레포트 폼 타입 ID")
                .HasColumnName("form_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("레포트 폼 타입명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<ReportOperatorType>(entity =>
        {
            entity.HasKey(e => e.OperatorTypeId);

            entity.ToTable("report_operator_type", tb => tb.HasComment("레포트 연산자 타입"));

            entity.Property(e => e.OperatorTypeId)
                .ValueGeneratedNever()
                .HasComment("레포트 연산자 타입 ID")
                .HasColumnName("operator_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("레포트 연산자 타입명")
                .HasColumnName("name");
            entity.Property(e => e.OperatorCode)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("레포트 연산자 타입 코드")
                .HasColumnName("operator_code");
        });

        modelBuilder.Entity<ScaleFactor>(entity =>
        {
            entity.ToTable("scale_factor", tb => tb.HasComment("스케일 펙터"));

            entity.Property(e => e.ScaleFactorId)
                .ValueGeneratedNever()
                .HasComment("스케일 펙터 ID")
                .HasColumnName("scale_factor_id");
            entity.Property(e => e.Offset)
                .HasComment("스케일 펙터 OFFSET 값")
                .HasColumnName("offset");
            entity.Property(e => e.Scale)
                .HasDefaultValueSql("((1.0))")
                .HasComment("스케일 펙터 값")
                .HasColumnName("scale");
        });

        modelBuilder.Entity<Sound>(entity =>
        {
            entity.ToTable("sound", tb => tb.HasComment("소리"));

            entity.Property(e => e.SoundId)
                .ValueGeneratedNever()
                .HasComment("소리 ID")
                .HasColumnName("sound_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("소리 이름")
                .HasColumnName("name");
            entity.Property(e => e.SoundFile)
                .HasComment("소리 파일")
                .HasColumnName("sound_file");
        });

        modelBuilder.Entity<StateGroup>(entity =>
        {
            entity.ToTable("state_group", tb => tb.HasComment("상태 그룹 정보"));

            entity.Property(e => e.StateGroupId)
                .ValueGeneratedNever()
                .HasComment("상태 그룹 ID")
                .HasColumnName("state_group_id");
            entity.Property(e => e.Count)
                .HasComment("상태 개수")
                .HasColumnName("count");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("상태 그룹명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<StateValue>(entity =>
        {
            entity.ToTable("state_value", tb => tb.HasComment("상태 값 변경이름정보"));

            entity.Property(e => e.StateValueId)
                .ValueGeneratedNever()
                .HasComment("상태값 변경 ID")
                .HasColumnName("state_value_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("상태값 변경명")
                .HasColumnName("name");
            entity.Property(e => e.StateGroupFk)
                .HasComment("상태 그룹 ID")
                .HasColumnName("state_group_fk");
            entity.Property(e => e.Value)
                .HasComment("상태값")
                .HasColumnName("value");

            entity.HasOne(d => d.StateGroupFkNavigation).WithMany(p => p.StateValues)
                .HasForeignKey(d => d.StateGroupFk)
                .HasConstraintName("FK_state_value_state_group_fk");
        });

        modelBuilder.Entity<StationType>(entity =>
        {
            entity.ToTable("station_type", tb => tb.HasComment("스테이션(변전소) 타입"));

            entity.Property(e => e.StationTypeId)
                .ValueGeneratedNever()
                .HasComment("스테이션 타입 ID")
                .HasColumnName("station_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("스테이션명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<StationView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("station_view");

            entity.Property(e => e.MemberOfficeId).HasColumnName("member_office_id");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.ModelName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("model_name");
            entity.Property(e => e.OfficeName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("office_name");
            entity.Property(e => e.StationAdder)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("station_adder");
            entity.Property(e => e.StationId).HasColumnName("station_id");
            entity.Property(e => e.StationName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("station_name");
            entity.Property(e => e.StationTypeId).HasColumnName("station_type_id");
            entity.Property(e => e.StationTypeName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("station_type_name");
        });

        modelBuilder.Entity<Substation>(entity =>
        {
            entity.HasKey(e => e.StationMrfk);

            entity.ToTable("substation", tb => tb.HasComment("스테이션 정보"));

            entity.Property(e => e.StationMrfk)
                .ValueGeneratedNever()
                .HasComment("스테이션 ID")
                .HasColumnName("station_mrfk");
            entity.Property(e => e.MemberOfficeFk)
                .HasComment("지역 코드 ID")
                .HasColumnName("member_office_fk");
            entity.Property(e => e.StationAdder)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("스테이션 주소")
                .HasColumnName("station_adder");
            entity.Property(e => e.StationCode)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComment("스테이션 코드")
                .HasColumnName("station_code");
            entity.Property(e => e.StationTypeFk)
                .HasComment("스테이션 타입")
                .HasColumnName("station_type_fk");

            entity.HasOne(d => d.MemberOfficeFkNavigation).WithMany(p => p.Substations)
                .HasForeignKey(d => d.MemberOfficeFk)
                .HasConstraintName("FK_substation_member_office_fk");

            entity.HasOne(d => d.StationTypeFkNavigation).WithMany(p => p.Substations)
                .HasForeignKey(d => d.StationTypeFk)
                .HasConstraintName("FK_substation_station_type_fk");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.ToTable("unit", tb => tb.HasComment("단위"));

            entity.Property(e => e.UnitId)
                .ValueGeneratedNever()
                .HasComment("단위 ID")
                .HasColumnName("unit_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("단위")
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user", tb => tb.HasComment("사용자 정보"));

            entity.Property(e => e.UserId)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("사용자 ID")
                .HasColumnName("user_id");
            entity.Property(e => e.DeleteFlag)
                .HasComment("삭제 여부")
                .HasColumnName("delete_flag");
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("이메일")
                .HasColumnName("email");
            entity.Property(e => e.IsNotify)
                .HasDefaultValueSql("((0))")
                .HasComment("알림여부:0,1")
                .HasColumnName("is_notify");
            entity.Property(e => e.MemberOfficeFk)
                .HasComment("지역 코드")
                .HasColumnName("member_office_fk");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("전화번호")
                .HasColumnName("mobile_no");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("사용자명")
                .HasColumnName("name");
            entity.Property(e => e.Organization)
                .HasMaxLength(32)
                .IsUnicode(false)
                .HasComment("조직정보")
                .HasColumnName("organization");
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasComment("패스워드")
                .HasColumnName("password");
            entity.Property(e => e.UserGroupFk)
                .HasComment("사용자 그룹 ID")
                .HasColumnName("user_group_fk");

            entity.HasOne(d => d.MemberOfficeFkNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.MemberOfficeFk)
                .HasConstraintName("FK_user_member_office_fk");

            entity.HasOne(d => d.UserGroupFkNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserGroupFk)
                .HasConstraintName("FK_user_user_group_fk");
        });

        modelBuilder.Entity<UserAuthorityType>(entity =>
        {
            entity.ToTable("user_authority_type", tb => tb.HasComment("사용자 권한 타입"));

            entity.Property(e => e.UserAuthorityTypeId)
                .ValueGeneratedNever()
                .HasComment("사용자 권한 타입 ID")
                .HasColumnName("user_authority_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("사용자 권한 타입명")
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.ToTable("user_group", tb => tb.HasComment("사용자 그룹 정보"));

            entity.Property(e => e.UserGroupId)
                .ValueGeneratedNever()
                .HasComment("사용자 그룹 ID")
                .HasColumnName("user_group_id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("사용자 그룹명")
                .HasColumnName("name");

            entity.HasMany(d => d.UserAuthorityTypeFks).WithMany(p => p.UserGroupFks)
                .UsingEntity<Dictionary<string, object>>(
                    "UserGroupAuthority",
                    r => r.HasOne<UserAuthorityType>().WithMany()
                        .HasForeignKey("UserAuthorityTypeFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_user_group_authority_user_authority_type_fk"),
                    l => l.HasOne<UserGroup>().WithMany()
                        .HasForeignKey("UserGroupFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_user_group_authority_user_group_fk"),
                    j =>
                    {
                        j.HasKey("UserGroupFk", "UserAuthorityTypeFk");
                        j.ToTable("user_group_authority", tb => tb.HasComment("사용자 그룹별 권한 정보"));
                        j.IndexerProperty<int>("UserGroupFk")
                            .HasComment("사용자 그룹 ID")
                            .HasColumnName("user_group_fk");
                        j.IndexerProperty<int>("UserAuthorityTypeFk")
                            .HasComment("사용자 그룹별 권한 ID")
                            .HasColumnName("user_authority_type_fk");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
