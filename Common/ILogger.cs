namespace Common
{
    public interface ILogger<T>
    {
        void LogInfo(string message);
        void LogDebug(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
