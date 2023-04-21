using KafkaNet;
using KafkaNet.Model;
using Smart.Kh2Ems.Infrastructure.Api;
using Smart.Kh2Ems.Infrastructure.Models.ApiMoels.Database;
using Smart.Kh2Ems.Infrastructure.WebServerApi;
using System.Text;

namespace Kh2Calculation
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly WebApiContext _webContext;

        public Worker(ILogger<Worker> logger, WebApiContext webContext)
        {
            _logger = logger;
            this._webContext = webContext;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time0} {time1} {time2}", DateTimeOffset.Now, "111111111", "222233");

                try
                {
                    GetFromKafka("test");

                    //var computerInfos = await _webContext.GetComputerInfos();
                    //var programInfos = await _webContext.GetProgramInfos();

                    //if (computerInfos != null)
                    //{
                    //    if (computerInfos.Result)
                    //    {
                    //        foreach (var item in computerInfos.Datas!)
                    //        {
                    //            _logger.LogInformation($"com:{item.ComputerId} nm:{item.Name} tm:{DateTimeOffset.Now}");
                    //        }
                    //    }
                    //}

                    //if (programInfos != null)
                    //{
                    //    if (programInfos.Result)
                    //    {
                    //        foreach (var item in programInfos.Datas!)
                    //        {
                    //            _logger.LogInformation($"program:{item.ProgramId} nm:{item.Name} com:{item.ComputerId}  tm:{DateTimeOffset.Now}");
                    //        }
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }
                
                await Task.Delay(1000, stoppingToken);
            }
        }

        public void GetFromKafka(string topic)
        {
            var options = new KafkaOptions(new Uri(@"http://localhost:9092"));
            BrokerRouter brokerRouter = new BrokerRouter(options);
            Consumer kafkaConsumer = new Consumer(new ConsumerOptions(topic, brokerRouter));

            string message = string.Empty;
            foreach (var msg in kafkaConsumer.Consume())
            {
                message += Encoding.UTF8.GetString(msg.Value);
            }

            Console.WriteLine(message);
        }

        public void SetFromKafka(string topic)
        {
            var options = new KafkaOptions(new Uri(@"http://localhost:9092"));
            BrokerRouter brokerRouter = new BrokerRouter(options);
            Consumer kafkaConsumer = new Consumer(new ConsumerOptions(topic, brokerRouter));
            Producer kafkaProducer = new Producer(brokerRouter);

            string message = string.Empty;
            foreach (var msg in kafkaConsumer.Consume())
            {
                message += Encoding.UTF8.GetString(msg.Value);
            }

            Console.WriteLine(message);
        }
    }
}