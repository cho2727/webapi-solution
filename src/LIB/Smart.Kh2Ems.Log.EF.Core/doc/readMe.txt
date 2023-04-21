1. 데이터베이스 리버스 엔지니어링
	dotnet ef dbcontext scaffold "Data Source=127.0.0.1,4433;Database=k_h2ems_log;User Id=sa;Password=20wellsdb19!@;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer --context-dir Infrastructure/Reverse --output-dir Infrastructure/Reverse/Models
