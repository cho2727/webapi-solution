using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Smart.Kh2Ems.Log.EF.Core.Infrastructure.Reverse.Models;

namespace Smart.Kh2Ems.Log.EF.Core.Infrastructure.Reverse;

public partial class KH2emsLogContext : DbContext
{
    protected readonly IConfiguration _configuration;

    public KH2emsLogContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected KH2emsLogContext(IConfiguration configuration, DbContextOptions options) : base(options)
    {
        _configuration = configuration;
    }
    public KH2emsLogContext(IConfiguration configuration, DbContextOptions<KH2emsLogContext> options)
        : base(options)
    {
        _configuration = configuration;
    }
    public virtual DbSet<LogDay> LogDays { get; set; }

    public virtual DbSet<LogHour> LogHours { get; set; }

    public virtual DbSet<LogMinute> LogMinutes { get; set; }

    public virtual DbSet<LogMonth> LogMonths { get; set; }

    public virtual DbSet<LogSetting> LogSettings { get; set; }

    public virtual DbSet<ReportRowDatum> ReportRowData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=127.0.0.1,4433;Database=k_h2ems_log;User Id=sa;Password=20wellsdb19!@;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogDay>(entity =>
        {
            entity.HasKey(e => e.Idx).IsClustered(false);

            entity.ToTable("log_day", tb => tb.HasComment("일별 이력 저장"));

            entity.Property(e => e.Idx)
                .HasComment("기본아이디")
                .HasColumnName("idx");
            entity.Property(e => e.CeqId)
                .HasComment("설비 ID")
                .HasColumnName("ceq_id");
            entity.Property(e => e.CeqTypeId)
                .HasComment("설비 종류 ID")
                .HasColumnName("ceq_type_id");
            entity.Property(e => e.DeviceUptime)
                .HasComment("기기 업데이트 시간")
                .HasColumnType("datetime")
                .HasColumnName("device_uptime");
            entity.Property(e => e.DynamicIndex)
                .HasComment("동적 인덱스 ID")
                .HasColumnName("dynamic_index");
            entity.Property(e => e.IndexName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("동적인덱스이름")
                .HasColumnName("index_name");
            entity.Property(e => e.MemberOfficeId)
                .HasComment("지역(사업소) 코드")
                .HasColumnName("member_office_id");
            entity.Property(e => e.ModelId)
                .HasComment("모델 ID")
                .HasColumnName("model_id");
            entity.Property(e => e.QualityValue)
                .HasComment("Quality 값")
                .HasColumnName("quality_value");
            entity.Property(e => e.SaveTime)
                .HasComment("서버 저장 시간")
                .HasColumnType("datetime")
                .HasColumnName("save_time");
            entity.Property(e => e.StationId)
                .HasComment("스테이션 ID")
                .HasColumnName("station_id");
            entity.Property(e => e.TagValue)
                .HasComment("TAG 값")
                .HasColumnName("tag_value");
            entity.Property(e => e.Value)
                .HasComment("값")
                .HasColumnName("value");
        });

        modelBuilder.Entity<LogHour>(entity =>
        {
            entity.HasKey(e => e.Idx).IsClustered(false);

            entity.ToTable("log_hour", tb => tb.HasComment("시별 이력 저장"));

            entity.Property(e => e.Idx)
                .HasComment("기본아이디")
                .HasColumnName("idx");
            entity.Property(e => e.CeqId)
                .HasComment("설비 ID")
                .HasColumnName("ceq_id");
            entity.Property(e => e.CeqTypeId)
                .HasComment("설비 종류 ID")
                .HasColumnName("ceq_type_id");
            entity.Property(e => e.DeviceUptime)
                .HasComment("기기 업데이트 시간")
                .HasColumnType("datetime")
                .HasColumnName("device_uptime");
            entity.Property(e => e.DynamicIndex)
                .HasComment("동적 인덱스 ID")
                .HasColumnName("dynamic_index");
            entity.Property(e => e.IndexName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("동적인덱스이름")
                .HasColumnName("index_name");
            entity.Property(e => e.MemberOfficeId)
                .HasComment("지역(사업소) 코드")
                .HasColumnName("member_office_id");
            entity.Property(e => e.ModelId)
                .HasComment("모델 ID")
                .HasColumnName("model_id");
            entity.Property(e => e.QualityValue)
                .HasComment("Quality 값")
                .HasColumnName("quality_value");
            entity.Property(e => e.SaveTime)
                .HasComment("서버 저장 시간")
                .HasColumnType("datetime")
                .HasColumnName("save_time");
            entity.Property(e => e.StationId)
                .HasComment("스테이션 ID")
                .HasColumnName("station_id");
            entity.Property(e => e.TagValue)
                .HasComment("TAG 값")
                .HasColumnName("tag_value");
            entity.Property(e => e.Value)
                .HasComment("값")
                .HasColumnName("value");
        });

        modelBuilder.Entity<LogMinute>(entity =>
        {
            entity.HasKey(e => e.Idx).IsClustered(false);

            entity.ToTable("log_minute", tb => tb.HasComment("분별 이력 저장"));

            entity.Property(e => e.Idx)
                .HasComment("기본아이디")
                .HasColumnName("idx");
            entity.Property(e => e.CeqId)
                .HasComment("설비 ID")
                .HasColumnName("ceq_id");
            entity.Property(e => e.CeqTypeId)
                .HasComment("설비 종류 ID")
                .HasColumnName("ceq_type_id");
            entity.Property(e => e.DeviceUptime)
                .HasComment("기기 업데이트 시간")
                .HasColumnType("datetime")
                .HasColumnName("device_uptime");
            entity.Property(e => e.DynamicIndex)
                .HasComment("동적 인덱스 ID")
                .HasColumnName("dynamic_index");
            entity.Property(e => e.IndexName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("동적인덱스이름")
                .HasColumnName("index_name");
            entity.Property(e => e.MemberOfficeId)
                .HasComment("지역(사업소) 코드")
                .HasColumnName("member_office_id");
            entity.Property(e => e.ModelId)
                .HasComment("모델 ID")
                .HasColumnName("model_id");
            entity.Property(e => e.QualityValue)
                .HasComment("Quality 값")
                .HasColumnName("quality_value");
            entity.Property(e => e.SaveTime)
                .HasComment("서버 저장 시간")
                .HasColumnType("datetime")
                .HasColumnName("save_time");
            entity.Property(e => e.StationId)
                .HasComment("스테이션 ID")
                .HasColumnName("station_id");
            entity.Property(e => e.TagValue)
                .HasComment("TAG 값")
                .HasColumnName("tag_value");
            entity.Property(e => e.Value)
                .HasComment("값")
                .HasColumnName("value");
        });

        modelBuilder.Entity<LogMonth>(entity =>
        {
            entity.HasKey(e => e.Idx).IsClustered(false);

            entity.ToTable("log_month", tb => tb.HasComment("월별 이력 저장"));

            entity.Property(e => e.Idx)
                .HasComment("기본아이디")
                .HasColumnName("idx");
            entity.Property(e => e.CeqId)
                .HasComment("설비 ID")
                .HasColumnName("ceq_id");
            entity.Property(e => e.CeqTypeId)
                .HasComment("설비 종류 ID")
                .HasColumnName("ceq_type_id");
            entity.Property(e => e.DeviceUptime)
                .HasComment("기기 업데이트 시간")
                .HasColumnType("datetime")
                .HasColumnName("device_uptime");
            entity.Property(e => e.DynamicIndex)
                .HasComment("동적 인덱스 ID")
                .HasColumnName("dynamic_index");
            entity.Property(e => e.IndexName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("동적인덱스이름")
                .HasColumnName("index_name");
            entity.Property(e => e.MemberOfficeId)
                .HasComment("지역(사업소) 코드")
                .HasColumnName("member_office_id");
            entity.Property(e => e.ModelId)
                .HasComment("모델 ID")
                .HasColumnName("model_id");
            entity.Property(e => e.QualityValue)
                .HasComment("Quality 값")
                .HasColumnName("quality_value");
            entity.Property(e => e.SaveTime)
                .HasComment("서버 저장 시간")
                .HasColumnType("datetime")
                .HasColumnName("save_time");
            entity.Property(e => e.StationId)
                .HasComment("스테이션 ID")
                .HasColumnName("station_id");
            entity.Property(e => e.TagValue)
                .HasComment("TAG 값")
                .HasColumnName("tag_value");
            entity.Property(e => e.Value)
                .HasComment("값")
                .HasColumnName("value");
        });

        modelBuilder.Entity<LogSetting>(entity =>
        {
            entity.HasKey(e => e.Idx).IsClustered(false);

            entity.ToTable("log_setting", tb => tb.HasComment("로그 설정 테이블"));

            entity.Property(e => e.Idx)
                .HasComment("기본아이디")
                .HasColumnName("idx");
            entity.Property(e => e.CeqTypeId)
                .HasComment("설비 종류 ID")
                .HasColumnName("ceq_type_id");
            entity.Property(e => e.DynamicIndex)
                .HasComment("동적 인덱스 ID")
                .HasColumnName("dynamic_index");
            entity.Property(e => e.IsReportSave)
                .HasComment("보고서저장여부")
                .HasColumnName("is_report_save");
        });

        modelBuilder.Entity<ReportRowDatum>(entity =>
        {
            entity.HasKey(e => e.Idx).IsClustered(false);

            entity.ToTable("report_row_data", tb => tb.HasComment("보고서 기초 데이터"));

            entity.Property(e => e.Idx)
                .HasComment("기본아이디")
                .HasColumnName("idx");
            entity.Property(e => e.CeqId)
                .HasComment("설비 ID")
                .HasColumnName("ceq_id");
            entity.Property(e => e.CeqTypeId)
                .HasComment("설비 종류 ID")
                .HasColumnName("ceq_type_id");
            entity.Property(e => e.DynamicIndex)
                .HasComment("동적 인덱스 ID")
                .HasColumnName("dynamic_index");
            entity.Property(e => e.IndexName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasComment("동적인덱스이름")
                .HasColumnName("index_name");
            entity.Property(e => e.MemberOfficeId)
                .HasComment("지역(사업소) 코드")
                .HasColumnName("member_office_id");
            entity.Property(e => e.ModelId)
                .HasComment("모델 ID")
                .HasColumnName("model_id");
            entity.Property(e => e.OperationTypeId)
                .HasComment("연산자 타입")
                .HasColumnName("operation_type_id");
            entity.Property(e => e.SaveTime)
                .HasComment("서버 저장 시간")
                .HasColumnType("datetime")
                .HasColumnName("save_time");
            entity.Property(e => e.StationId)
                .HasComment("스테이션 ID")
                .HasColumnName("station_id");
            entity.Property(e => e.Value)
                .HasComment("값")
                .HasColumnName("value");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
