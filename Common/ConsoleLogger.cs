using System;

namespace Common
{
    public class ConsoleLogger<T> : ILogger<T>
    {
        private readonly LogLevel logLevel;
        private const string LogMessageFormat = "[{0}] ({1}) {2}";

        private string TypeName { get; } = typeof(T).Name;

        public ConsoleLogger() : this(LogLevel.Error | LogLevel.Warning | LogLevel.Info)
        {

        }

        public ConsoleLogger(LogLevel logLevel)
        {
            this.logLevel = logLevel;
        }

        public void LogDebug(string message)
        {
            TryLog(LogLevel.Debug, message);
        }

        public void LogError(string message)
        {
            TryLog(LogLevel.Error, message);
        }

        public void LogInfo(string message)
        {
            TryLog(LogLevel.Info, message);
        }

        public void LogWarning(string message)
        {
            TryLog(LogLevel.Warning, message);
        }

        private void TryLog(LogLevel level, string message)
        {
            if (!logLevel.HasFlag(level)) return;

            string logLevelName = Enum.GetName(typeof(LogLevel), level);

            Console.WriteLine(string.Format(LogMessageFormat, logLevelName, TypeName, message));
        }
    }
}
