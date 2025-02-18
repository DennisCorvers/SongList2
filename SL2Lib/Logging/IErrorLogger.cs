using SL2Lib.Models;

namespace SL2Lib.Logging
{
    public interface IErrorLogger
    {
        uint ErrorCount { get; }

        void LogDuplicateSong(Song duplicateSong);

        void LogError(string message);
    }
}
