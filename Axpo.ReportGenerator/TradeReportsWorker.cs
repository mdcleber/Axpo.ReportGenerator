using Axpo.ReportGenerator.Model;
using Axpo.ReportGenerator.Services;

namespace Axpo.ReportGenerator
{
    public class TradeReportsWorker : BackgroundService
    {
        private readonly ILogger<TradeReportsWorker> _logger;
        private readonly IExecutionParameters _configuration;
        private readonly ITradesAggregator _tradesAggregator;
        private readonly IReportGenerator _reportGenerator;

        public TradeReportsWorker(ILogger<TradeReportsWorker> logger,
            IExecutionParameters configuration,
            ITradesAggregator tradesAggregator,
            IReportGenerator reportGenerator)
        {
            _logger = logger;
            _configuration = configuration;
            _tradesAggregator = tradesAggregator;
            _reportGenerator = reportGenerator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int executionInterval = _configuration.GetExecutionInterval();
            string resultPath = _configuration.GetResultPath();

            while (!stoppingToken.IsCancellationRequested)
            {
                await RunExtract(resultPath, executionInterval, stoppingToken);
            }
        }

        private async Task RunExtract(string folderPath, int intervalMinutes, CancellationToken stoppingToken)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                DateTime localStartTime = new DateTime(now.Year, now.Month, now.Day, 23, 0, 0, DateTimeKind.Utc);

                IEnumerable<TradesConsolidated> consolidatedTrades = await _tradesAggregator.GetTradesAsync(localStartTime);
                await _reportGenerator.CreateReport(folderPath, consolidatedTrades);
            }
            catch (PowerServiceException ex)
            {
                _logger.LogInformation("Error fetching trading list. Message: {}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Unexpected error. Message: {}", ex.Message);
            }

            TimeSpan timeToNextExtract = TimeSpan.FromMinutes(intervalMinutes);
            _logger.LogInformation("Next execution scheduled to {}", DateTime.Now.Add(timeToNextExtract).ToString("yyyy-MM-dd HH:mm"));

            await Task.Delay(timeToNextExtract > TimeSpan.Zero ? timeToNextExtract : TimeSpan.Zero, stoppingToken);
        }
    }
}
