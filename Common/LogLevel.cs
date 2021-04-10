using System;

namespace Common
{
    [Flags]
    public enum LogLevel
    {
        None = 0,
        Error = 1 << 0,
        Warning = 1 << 1,
        Info = 1 << 2,
        Debug = 1 << 3,
        All = ~(-1 << 4)
    }
}
