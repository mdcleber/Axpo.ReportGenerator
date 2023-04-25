using System.Globalization;
using Axpo.ReportGenerator.Model;

namespace Axpo.ReportGenerator.Services
{
    public class TradesAggregatorService : ITradesAggregator
    {
        private readonly IPowerService _powerService;
        private readonly ILogger<TradesAggregatorService> _logger;

        public TradesAggregatorService(IPowerService powerService, ILogger<TradesAggregatorService> logger)
        {
            _powerService = powerService;
            _logger = logger;
        }
        public async Task<IEnumerable<TradesConsolidated>> GetTradesAsync(DateTime utcTimeZone)
        {
            IEnumerable<PowerTrade> trades = trades = await _powerService.GetTradesAsync(utcTimeZone);

            return trades.SelectMany(trade => trade.Periods)
                .GroupBy(period => period.Period)
                .Select(aggregated =>
                {
                    int hour = aggregated.Key - 1;
                    DateTime localDateTime = utcTimeZone.AddHours(hour);
                    return new TradesConsolidated
                    {
                        LocalTime = localDateTime.ToString("HH:mm", CultureInfo.InvariantCulture),
                        Volume = aggregated.Sum(x => x.Volume)
                    };
                });
        }
    }
}
