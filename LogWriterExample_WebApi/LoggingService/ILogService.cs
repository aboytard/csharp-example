namespace LoggingService
{
    public interface ILogService<T>
    {
        void LogError(string message);
        void LogWarning(string message);
        void LogInformation(string message);
    }
}
