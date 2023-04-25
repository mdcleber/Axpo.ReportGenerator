using System.Globalization;
using Axpo.ReportGenerator.Model;
using Axpo.ReportGenerator.Services;
using CsvHelper;

namespace Axpo.ReportGenerator.Tests.Services
{
    public class CsvReportGeneratorTests
    {
        [Fact]
        public async Task CreateReport_CreatesCsvFileInSpecifiedDirectory()
        {
            var logger = Mock.Of<ILogger<CsvReportGenerator>>();
            var generator = new CsvReportGenerator(logger);
            var trades = new List<TradesConsolidated>
            {
                new TradesConsolidated { LocalTime = "12:00", Volume = 10 },
                new TradesConsolidated { LocalTime = "13:00", Volume = 20 }
            };
            var tempDir = Path.GetTempPath();


            await generator.CreateReport(tempDir, trades);


            var fileName = Directory.GetFiles(tempDir)
                .Select(Path.GetFileName)
                .FirstOrDefault(name => name.StartsWith("PowerPosition_"));

            Assert.NotNull(fileName);
            var filePath = Path.Combine(tempDir, fileName);

            Assert.True(File.Exists(filePath));

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<TradesConsolidated>().ToList();

                Assert.Equal(2, records.Count);
                Assert.Equal("12:00", records[0].LocalTime);
                Assert.Equal(10, records[0].Volume);
                Assert.Equal("13:00", records[1].LocalTime);
                Assert.Equal(20, records[1].Volume);
            }

            File.Delete(filePath);
        }
    }
}
