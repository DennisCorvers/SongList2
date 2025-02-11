using SL2Lib.Models;

namespace SL2Lib.Data
{
    internal class SongService
    {
        private ISongRepo m_songRepo;

        public IEnumerable<Song> SongList => m_songRepo.Songs;

        public SongService(ISongRepo songRepo)
        {
            m_songRepo = songRepo;
        }

        public IEnumerable<Song> AddSongs(IEnumerable<Song> songs)
        {
            // Add songs, check for doubles
            throw new NotImplementedException();
        }

        public void RemoveSongs(IEnumerable<Song> songs)
        {
            // Remove songs
        }

        public IEnumerable<Song> FindSongs(string? title, string? artist, string? album)
        {
            throw new NotImplementedException();
        }
    }
}
