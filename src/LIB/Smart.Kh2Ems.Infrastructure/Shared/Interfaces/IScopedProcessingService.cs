using Smart.Kh2Ems.Infrastructure.Shared.Injectables;

namespace Smart.Kh2Ems.Infrastructure.Shared.Interfaces;

public interface IScopedProcessingService : IScopedService
{
    Task DoWork(CancellationToken stoppingToken);
}
