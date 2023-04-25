namespace Axpo.ReportGenerator.Services
{
    public class ExecutionParameters : IExecutionParameters
    {
        private readonly string[] _args;
        private readonly ILogger<TradeReportsWorker> _logger;
        private readonly IConfiguration _configuration;
        private static readonly int defaultExecutionInterval = 10;
        private static readonly int defaultMaximumAttempts = 3;
        public ExecutionParameters(string[] args, ILogger<TradeReportsWorker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _args = args;
        }

        public int GetExecutionInterval()
        {
            string? inputInterval = _args.Length >= 2 ? _args[1] : null;

            if (int.TryParse(inputInterval, out int executionInterval))
            {
                _logger.LogInformation("Execution interval setting has been overwritten, new execution interval: {}", executionInterval);
                return ValidateExecutionInterval(executionInterval);
            }


            executionInterval = _configuration.GetValue<int>("ExecutionInterval");
            _logger.LogInformation("Using default execution interval of: {}", executionInterval);


            return ValidateExecutionInterval(executionInterval);
        }

        private int ValidateExecutionInterval(int executionInterval)
        {
            if (executionInterval <= 0)
            {
                _logger.LogInformation("Invalid execution interval provided, using default value of {} minutes.", defaultExecutionInterval);
                return defaultExecutionInterval;
            }

            return executionInterval;
        }

        public string GetResultPath()
        {
            string resultPath = _args.Length >= 1 ? _args[0] : String.Empty;

            if (string.IsNullOrWhiteSpace(resultPath))
            {
                return GetResultPahtFromConfiguration();
            }

            _logger.LogInformation("Result path setting has been overwritten, new Result Path: {}", resultPath);
            CreateDirectoryIfNotExists(resultPath);
            return resultPath;

        }

        private string GetResultPahtFromConfiguration()
        {
            string? resultPath = _configuration.GetValue<string>("ResultPath");
            if (string.IsNullOrEmpty(resultPath))
            {
                resultPath = Path.GetTempPath();
                _logger.LogInformation("Invalid result path provided, using temp directory {} {}", resultPath);
                return resultPath;
            }

            CreateDirectoryIfNotExists(resultPath);

            _logger.LogInformation("Using default Result Path of: {}", resultPath);

            return resultPath;
        }

        private void CreateDirectoryIfNotExists(string resultPath)
        {
            if (!Directory.Exists(resultPath))
            {
                Directory.CreateDirectory(resultPath);
            }
        }

        public int GetMaximumAttempts()
        {
            string? inputAttempts = _args.Length >= 3 ? _args[2] : null;

            if (int.TryParse(inputAttempts, out int maximumAttempts))
            {
                _logger.LogInformation("Maximum Attempts setting has been overwritten, new Maximum Attempts: {}", maximumAttempts);
                return ValidateMaximumAttempts(maximumAttempts);
            }


            maximumAttempts = _configuration.GetValue<int>("MaximumAttempts");
            _logger.LogInformation("Using default Maximum Attempts of: {}", maximumAttempts);


            return ValidateMaximumAttempts(maximumAttempts);
        }

        private int ValidateMaximumAttempts(int maximumAttempts)
        {
            if (maximumAttempts <= 0)
            {
                _logger.LogInformation("Invalid Maximum Attempts provided, using default value of {} attempts.", defaultMaximumAttempts);
                return defaultMaximumAttempts;
            }

            return maximumAttempts;
        }
    }
}
