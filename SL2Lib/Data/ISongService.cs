using SL2Lib.Models;

namespace SL2Lib.Data
{
    public interface ISongService
    {
        IEnumerable<Song> SongList { get; }

        IEnumerable<Song> AddSongs(IEnumerable<Song> songs);

        IEnumerable<Song> FindSongs(string? title, string? artist = default, string? album = default);

        void RemoveSongs(IEnumerable<Song> songs);
    }
}
