using Axpo.ReportGenerator.Services;
using Microsoft.Extensions.Configuration;

namespace Axpo.ReportGenerator.Tests.Services
{
    public class ExecutionParametersTests
    {
        private readonly Mock<ILogger<TradeReportsWorker>> _loggerMock;
        private readonly IConfiguration _configurationMock;
        private readonly Dictionary<string, string> _inMemorySettings;

        private readonly static string ResultPathKey = "ResultPath";
        private readonly static string ResultPathValue = "C:\\Temp";
        private readonly static string ExecutionIntervalKey = "ExecutionInterval";
        private readonly static int ExecutionIntervalValue = 20;
        private readonly static string MaximumAttemptsKey = "MaximumAttempts";
        private readonly static int MaximumAttemptsValue = 5;


        public ExecutionParametersTests()
        {
            _loggerMock = new Mock<ILogger<TradeReportsWorker>>();

            _inMemorySettings = new Dictionary<string, string> {
                {ResultPathKey, ResultPathValue},
                {ExecutionIntervalKey, ExecutionIntervalValue.ToString()},
                {MaximumAttemptsKey, MaximumAttemptsValue.ToString()}

            };

            _configurationMock = new ConfigurationBuilder()
                .AddInMemoryCollection(_inMemorySettings)
                .Build();
        }

        [Fact]
        public void GetExecutionInterval_WhenInputIsValid_ReturnsInput()
        {
            var args = new[] { "dummy", "30" };
            var executionParameters = new ExecutionParameters(args, _loggerMock.Object, _configurationMock);

            var result = executionParameters.GetExecutionInterval();

            Assert.Equal(30, result);
        }

        [Fact]
        public void GetExecutionInterval_WhenInputIsInvalid_ReturnsDefault()
        {
            var args = new[] { "dummy", "invalid" };
            var executionParameters = new ExecutionParameters(args, _loggerMock.Object, _configurationMock);

            var result = executionParameters.GetExecutionInterval();

            Assert.Equal(ExecutionIntervalValue, result);
        }

        [Fact]
        public void GetExecutionInterval_WhenInputIsEmpty_ReturnsDefault()
        {
            var args = new string[] { };

            _inMemorySettings[ExecutionIntervalKey] = null;

            var configurationMock = new ConfigurationBuilder()
                .AddInMemoryCollection(_inMemorySettings)
                .Build();

            var executionParameters = new ExecutionParameters(args, _loggerMock.Object, configurationMock);

            var result = executionParameters.GetExecutionInterval();

            Assert.Equal(10, result);
            _inMemorySettings[ExecutionIntervalKey] = ExecutionIntervalValue.ToString();
        }

        [Fact]
        public void GetResultPath_WhenPathIsProvidedAsArgument_ReturnsPath()
        {
            var tempPath = "C:\\Temp2";
            var args = new[] { tempPath };
            var executionParameters = new ExecutionParameters(args, _loggerMock.Object, _configurationMock);

            var result = executionParameters.GetResultPath();

            Assert.Equal(tempPath, result);
        }

        [Fact]
        public void GetResultPath_WhenPathIsNotProvidedAsArgument_ReturnsDefault()
        {
            var args = new string[] { };
            var executionParameters = new ExecutionParameters(args, _loggerMock.Object, _configurationMock);

            var result = executionParameters.GetResultPath();

            Assert.Equal(ResultPathValue, result);
        }

        [Fact]
        public void GetResultPath_WhenPathIsNotProvidedAndConfigurationIsNull_ReturnsTempDirectory()
        {
            var args = new string[] { };
            _inMemorySettings[ResultPathKey] = string.Empty;

            var configurationMock = new ConfigurationBuilder()
                .AddInMemoryCollection(_inMemorySettings)
                .Build();

            var executionParameters = new ExecutionParameters(args, _loggerMock.Object, configurationMock);

            var result = executionParameters.GetResultPath();

            Assert.Equal(Path.GetTempPath(), result);
            _inMemorySettings[ResultPathKey] = ResultPathValue;
        }


        [Fact]
        public void GetMaximumAttempts_WhenInputIsValid_ReturnsInput()
        {
            var args = new[] { "dummy", "30", "10" };
            var executionParameters = new ExecutionParameters(args, _loggerMock.Object, _configurationMock);

            var result = executionParameters.GetMaximumAttempts();

            Assert.Equal(10, result);
        }

        [Fact]
        public void GetMaximumAttempts_WhenInputIsInvalid_ReturnsDefault()
        {
            var args = new[] { "dummy", "invalid", "invalid" };
            var executionParameters = new ExecutionParameters(args, _loggerMock.Object, _configurationMock);

            var result = executionParameters.GetMaximumAttempts();

            Assert.Equal(MaximumAttemptsValue, result);
        }

        [Fact]
        public void GetMaximumAttempts_WhenInputIsEmpty_ReturnsDefault()
        {
            var args = new string[] { };

            _inMemorySettings[MaximumAttemptsKey] = null;

            var configurationMock = new ConfigurationBuilder()
                .AddInMemoryCollection(_inMemorySettings)
                .Build();

            var executionParameters = new ExecutionParameters(args, _loggerMock.Object, configurationMock);

            var result = executionParameters.GetMaximumAttempts();

            Assert.Equal(3, result);
            _inMemorySettings[MaximumAttemptsKey] = MaximumAttemptsValue.ToString();
        }

    }
}
