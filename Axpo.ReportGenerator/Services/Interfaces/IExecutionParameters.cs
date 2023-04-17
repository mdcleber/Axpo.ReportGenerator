namespace Axpo.ReportGenerator.Services
{
    public interface IExecutionParameters
    {
        string GetResultPath();
        int GetExecutionInterval();
    }
}
