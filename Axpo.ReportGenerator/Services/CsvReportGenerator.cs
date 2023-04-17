using System.Globalization;
using Axpo.ReportGenerator.Model;
using CsvHelper;

namespace Axpo.ReportGenerator.Services
{
    public class CsvReportGenerator : IReportGenerator
    {
        private readonly ILogger<CsvReportGenerator> _logger;
        public CsvReportGenerator(ILogger<CsvReportGenerator> logger)
        {
            _logger = logger;
        }
        public async Task CreateReport(string folderPath, IEnumerable<TradesConsolidated> consolidatedTrades)
        {
            string fileName = $"PowerPosition_{DateTime.Now:yyyyMMdd_HHmm}.csv";
            string filePath = Path.Combine(folderPath, fileName);

            _logger.LogInformation("Generating new report: {}", filePath);

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(consolidatedTrades);
            }
        }
    }
}
