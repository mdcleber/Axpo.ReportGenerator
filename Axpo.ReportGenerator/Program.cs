using Axpo.ReportGenerator.Services;

namespace Axpo.ReportGenerator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<TradeReportsWorker>();
                    services.AddSingleton<IPowerService, PowerService>();
                    services.AddSingleton<ITradesAggregator, TradesAggregatorService>();
                    services.AddSingleton<IReportGenerator, CsvReportGenerator>();
                    services.AddSingleton<IExecutionParameters, ExecutionParameters>(provider => new ExecutionParameters(args,
                        provider.GetService<ILogger<TradeReportsWorker>>(),
                        provider.GetService<IConfiguration>()));
                })
                .Build();

            await host.RunAsync();
        }
    }
}

