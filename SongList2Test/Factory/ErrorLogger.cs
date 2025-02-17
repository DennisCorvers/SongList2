using SL2Lib.Logging;
using SL2Lib.Models;

namespace SongList2Test.Factory
{
    internal class ErrorLogger : IErrorLogger
    {
        public List<Song> LoggedSongs { get; }

        public ErrorLogger()
        {
            LoggedSongs = new List<Song>();
        }

        public void LogDuplicateSong(Song duplicateSong)
        {
            LoggedSongs.Add(duplicateSong);
        }
    }
}
