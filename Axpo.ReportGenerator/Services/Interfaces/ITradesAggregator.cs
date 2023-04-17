using Axpo.ReportGenerator.Model;

namespace Axpo.ReportGenerator.Services
{
    public interface ITradesAggregator
    {
        Task<IEnumerable<TradesConsolidated>> GetTradesAsync(DateTime localStartTime);
    }
}
