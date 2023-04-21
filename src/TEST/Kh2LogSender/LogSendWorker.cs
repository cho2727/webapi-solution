using Confluent.Kafka;
using Smart.Kh2Ems.Infrastructure.WebServerApi;
using System.Threading;


namespace Kh2LogSender;

public class LogSendWorker : BackgroundService
{
    private readonly ILogger<LogSendWorker> _logger;
    private readonly WebApiContext _webContext;
    private IProducer<Null, string> _producer;

    public LogSendWorker(ILogger<LogSendWorker> logger, WebApiContext webContext)
    {
        _logger = logger;
        this._webContext = webContext;
        var config = new ProducerConfig()
        {
            BootstrapServers = "localhost:9092"
        };
        _producer = new ProducerBuilder<Null, string>(config).Build();

    }


    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _producer?.Dispose();
        //return Task.CompletedTask;
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int count = 0;
        //for (var i = 0; i < 100; ++i)
        while (!stoppingToken.IsCancellationRequested)
        {
            var value = await _webContext.GetComputerInfoToString();

            // var value = $"Send Hello world {count++}";
            _logger.LogInformation(value);
            await _producer.ProduceAsync(topic: "test", new Message<Null, string>()
            {
                Value = value
            }, stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }

        _producer.Flush(timeout: TimeSpan.FromSeconds(10));

        //while (!stoppingToken.IsCancellationRequested)
        //{
        //    _logger.LogInformation("Worker1 running at: {time}", DateTimeOffset.Now);
        //    await Task.Delay(1000, stoppingToken);
        //}
    }
}