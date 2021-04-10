using System;

namespace Common
{
    /// <summary>
    /// Flags for different level of logging to enable.
    /// </summary>
    [Flags]
    public enum LogLevel
    {
        /// <summary>
        /// No logging.
        /// </summary>
        None = 0,

        /// <summary>
        /// Error.
        /// </summary>
        Error = 1 << 0,

        /// <summary>
        /// Warning.
        /// </summary>
        Warning = 1 << 1,

        /// <summary>
        /// Info.
        /// </summary>
        Info = 1 << 2,

        /// <summary>
        /// Debug.
        /// </summary>
        Debug = 1 << 3,

        /// <summary>
        /// All options enabled.
        /// </summary>
        All = ~(-1 << 4)
    }
}
