using SL2Lib.Logging;
using SL2Lib.Models;

namespace SL2Lib.Data
{
    public class SongService : ISongService
    {
        private readonly ISongRepo m_songRepo;
        private readonly IEnumerable<IErrorLogger>? m_errorLoggers;

        public IEnumerable<Song> SongList => m_songRepo.Songs;

        public SongService(ISongRepo songRepo, IEnumerable<IErrorLogger>? loggers = null)
        {
            m_songRepo = songRepo;
            m_errorLoggers = loggers;
        }

        public IEnumerable<Song> AddSongs(IEnumerable<Song> songs)
        {
            // Add songs, check for doubles
            throw new NotImplementedException();
        }

        public void RemoveSongs(IEnumerable<Song> songs)
        {
            foreach (var song in songs)
            {
                m_songRepo.Songs.Remove(song);
            }
        }

        public IEnumerable<Song> FindSongs(string? title, string? artist, string? album)
        {
            IEnumerable<Song> query = m_songRepo.Songs;

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(song => song.Name.Contains(title, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(artist))
            {
                query = query.Where(song => song.Artist != null && song.Artist.Contains(artist, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(album))
            {
                query = query.Where(song => song.Album != null && song.Album.Contains(album, StringComparison.OrdinalIgnoreCase));
            }

            return query;
        }

    }
}
