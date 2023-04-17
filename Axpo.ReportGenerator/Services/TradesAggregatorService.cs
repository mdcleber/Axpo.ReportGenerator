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
        public async Task<IEnumerable<TradesConsolidated>> GetTradesAsync(DateTime localStartTime)
        {
            IEnumerable<PowerTrade> trades = new List<PowerTrade>();

            _logger.LogInformation("Searching trading list");
            trades = await _powerService.GetTradesAsync(localStartTime);


            _logger.LogInformation("Aggregating transactions by Period");
            Dictionary<int, TradesConsolidated> tradesByPeriod = new Dictionary<int, TradesConsolidated>();

            foreach (PowerTrade trade in trades)
            {
                foreach (PowerPeriod period in trade.Periods)
                {
                    int hour = period.Period - 1;
                    if (!tradesByPeriod.ContainsKey(hour))
                    {
                        var tradeModel = new TradesConsolidated();
                        tradeModel.LocalTime = localStartTime.AddHours(hour).ToString("HH:mm", CultureInfo.InvariantCulture);
                        tradesByPeriod.Add(hour, tradeModel);

                    }

                    tradesByPeriod[hour].Volume += period.Volume;
                }
            }

            return tradesByPeriod.Values;
        }
    }
}
