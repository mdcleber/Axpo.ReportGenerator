using Axpo.ReportGenerator.Model;

namespace Axpo.ReportGenerator.Services
{
    public interface IReportGenerator
    {
        Task CreateReport(string folderPath, IEnumerable<TradesConsolidated> consolidatedTrades);
    }
}
