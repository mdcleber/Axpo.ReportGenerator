using Axpo;
using Axpo.ReportGenerator;
using Axpo.ReportGenerator.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<TradeReportsWorker>();
        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<ITradesAggregator, TradesAggregatorService>();
        services.AddSingleton<IReportGenerator, CsvReportGenerator>();
        services.AddSingleton<IExecutionParameters, ExecutionParameters>();
    })
    .Build();

await host.RunAsync();
