namespace Common
{
    /// <summary>
    /// Logs information 
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs the message at an info level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogInfo(string message);

        /// <summary>
        /// Logs the message at a debug level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogDebug(string message);

        /// <summary>
        /// Logs the message at a warning level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs the message at an error level.
        /// </summary>
        /// <param name="message">The message to log.</param>
        void LogError(string message);
    }
}
