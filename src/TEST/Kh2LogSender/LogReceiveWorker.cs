using Confluent.Kafka;
using Kafka.Public;
using Kafka.Public.Loggers;
using System.Text;
using System.Threading;

namespace Kh2LogSender
{
    public class LogReceiveWorker : BackgroundService
    {
        private readonly ILogger<LogSendWorker> _logger;
        private readonly ClusterClient _cluster;

        public LogReceiveWorker(ILogger<LogSendWorker> logger)
        {
            _logger = logger;
            _cluster = new ClusterClient(new Configuration { 
                Seeds = "localhost:9092"
            }, new ConsoleLogger());

        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _cluster?.Dispose();
            //return Task.CompletedTask;
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cluster.ConsumeFromLatest(topic: "test");
            _cluster.MessageReceived += record =>
            {
                _logger.LogInformation($"Received:{Encoding.UTF8.GetString(record.Value as byte[])}");
            };

            _logger.LogInformation("¿Ï·á");
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker2 running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }
    }
}