using CsvHelper.Configuration.Attributes;

namespace Axpo.ReportGenerator.Model
{
    public class TradesConsolidated
    {
        [Name("Local Time")]
        public string LocalTime { get; set; }
        public double Volume { get; set; }
    }
}
