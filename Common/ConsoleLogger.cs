using System;

namespace Common
{
    /// <summary>
    /// Logs messages for a type to the console.
    /// </summary>
    /// <typeparam name="T">The type to log messages for.</typeparam>
    public class ConsoleLogger<T> : ILogger
    {
        private readonly LogLevel logLevel;
        private const string LogMessageFormat = "[{0}] ({1}) {2}";

        private string TypeName { get; } = typeof(T).Name;

        /// <summary>
        /// Creates a new instance of <see cref="ConsoleLogger{T}"/> with the default log level.
        /// </summary>
        public ConsoleLogger() : this(LogLevel.Error | LogLevel.Warning | LogLevel.Info)
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="ConsoleLogger{T}"/>.
        /// </summary>
        /// <param name="logLevel">The level of logs to keep.</param>
        public ConsoleLogger(LogLevel logLevel)
        {
            this.logLevel = logLevel;
        }

        /// <summary>
        /// Logs the message at a debug level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogDebug(string message)
        {
            TryLog(LogLevel.Debug, message);
        }

        /// <summary>
        /// Logs the message at an error level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogError(string message)
        {
            TryLog(LogLevel.Error, message);
        }

        /// <summary>
        /// Logs the message at an info level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInfo(string message)
        {
            TryLog(LogLevel.Info, message);
        }

        /// <summary>
        /// Logs the message at a warning level.
        /// </summary>
        /// <param name="message">The message to log.</param>
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
