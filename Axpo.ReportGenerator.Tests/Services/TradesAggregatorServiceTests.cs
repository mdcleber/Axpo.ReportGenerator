using Axpo.ReportGenerator.Services;

namespace Axpo.ReportGenerator.Tests.Services
{
    public class TradesAggregatorServiceTests
    {
        [Fact]
        public async Task GetTradesAsync_ReturnsConsolidatedTrades()
        {
            var utcTimeZone = new DateTime(2022, 1, 1, 23, 0, 0, DateTimeKind.Utc);

            var mockPowerService = new Mock<IPowerService>();
            mockPowerService
                .Setup(x => x.GetTradesAsync(utcTimeZone))
                .ReturnsAsync(GetPowerTrades(utcTimeZone));

            var mockLogger = new Mock<ILogger<TradesAggregatorService>>();

            var tradesAggregatorService = new TradesAggregatorService(mockPowerService.Object, mockLogger.Object);

            var result = await tradesAggregatorService.GetTradesAsync(utcTimeZone);

            Assert.Equal(24, result.Count());

            double[] expectedVolume = (from period in Enumerable.Range(1, 24)
                                       select 23.0).ToArray();

            var expectedLocalTime = Enumerable.Range(0, 24).Select(hour => utcTimeZone.AddHours(hour).ToString("HH:mm"));

            for (int i = 0; i < 24; i++)
            {
                var consolidatedTrade = result.ElementAt(i);
                Assert.Equal(expectedVolume[i], consolidatedTrade.Volume);
                Assert.Equal(expectedLocalTime.ElementAt(i), consolidatedTrade.LocalTime);
            }
        }

        private static IEnumerable<PowerTrade> GetPowerTrades(DateTime date)
        {
            var numberOfPeriods = 24;

            var powerTrade1 = PowerTrade.Create(date, numberOfPeriods);
            var powerTrade2 = PowerTrade.Create(date, numberOfPeriods);

            for (int x = 0, y = numberOfPeriods - 1; x < numberOfPeriods; x++, y--)
            {
                powerTrade1.Periods[x].SetVolume(x);
                powerTrade2.Periods[x].SetVolume(y);
            }

            return new List<PowerTrade>() { powerTrade1, powerTrade2 };

        }
    }
}
