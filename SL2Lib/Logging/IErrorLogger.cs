using SL2Lib.Models;

namespace SL2Lib.Logging
{
    public interface IErrorLogger
    {
        void LogDuplicateSong(Song duplicateSong);
    }
}
