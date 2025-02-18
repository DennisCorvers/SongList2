using SL2Lib.Models;

namespace SL2Lib.Logging
{
    public interface IErrorLogger
    {
        uint ErrorCount { get; }

        void LogDuplicateSong(Song duplicateSong);

        void LogMessage(string message, ErrorLevel errorLevel);
    }

    public enum ErrorLevel
    {
        Error = 0,
        Warning = 1,
        Info = 2,
        Fatal = 3,
    }
}
