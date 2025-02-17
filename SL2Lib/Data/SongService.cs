using SL2Lib.Logging;
using SL2Lib.Models;

namespace SL2Lib.Data
{
    public class SongService : ISongService
    {
        private readonly ISongRepo m_songRepo;
        private readonly IEnumerable<IErrorLogger>? m_errorLoggers;

        public IEnumerable<Song> SongList => m_songRepo.Songs;

        public bool HasPendingChanges => false;

        public SongService(ISongRepo songRepo, IEnumerable<IErrorLogger>? loggers = null)
        {
            m_songRepo = songRepo;
            m_errorLoggers = loggers;
        }

        public void AddSong(Song song)
        {
            if (!m_songRepo.Songs.Add(song))
            {
                throw new DuplicateSongException(song);
            }
        }

        public IEnumerable<Song> AddSongs(IEnumerable<Song> songs)
        {
            var newSongs = new List<Song>();

            foreach (var song in songs)
            {
                try
                {
                    AddSong(song);
                    newSongs.Add(song);
                }
                catch (DuplicateSongException ex)
                {
                    if (m_errorLoggers != null)
                    {
                        foreach (var logger in m_errorLoggers)
                        {
                            logger.LogDuplicateSong(ex.DuplicateSong);
                        }
                    }
                }
            }

            return newSongs;
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

        public void LoadSongs(string? filePath)
        {
            m_songRepo.Load(filePath);
        }

        public string SaveSongs(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return m_songRepo.Persist();
            }
            else
            {
                return m_songRepo.Persist(filePath);
            }
        }
    }
}
