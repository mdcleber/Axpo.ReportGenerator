namespace Axpo.ReportGenerator.Services
{
    public class ExecutionParameters : IExecutionParameters
    {
        private readonly ILogger<TradeReportsWorker> _logger;
        private readonly IConfiguration _configuration;
        public ExecutionParameters(ILogger<TradeReportsWorker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public int GetExecutionInterval()
        {
            Console.WriteLine("Please specify the interval in minutes as the first argument (leave empty to use default).");
            string? inputInterval = Console.ReadLine();

            if (!int.TryParse(inputInterval, out int executionInterval))
            {
                _logger.LogInformation("Execution interval setting has been overwritten, new execution interval: {}", executionInterval);
                return executionInterval;
            }


            executionInterval = _configuration.GetValue<int>("ExecutionInterval");
            _logger.LogInformation("Using default execution interval of: {}", executionInterval);


            return executionInterval;
        }

        public string GetResultPath()
        {
            Console.WriteLine("Please specify the folder path to save the CSV file (leave empty to use default).");
            string resultPath = Console.ReadLine()!.Trim();

            if (!string.IsNullOrWhiteSpace(resultPath) && Directory.Exists(resultPath))
            {
                _logger.LogInformation("Result path setting has been overwritten, new Result Path: {}", resultPath);
                return resultPath;
            }


            resultPath = _configuration.GetValue<string>("ResultPath");
            _logger.LogInformation("Using default Result Path of: {}", resultPath);

            return resultPath;
        }
    }
}
