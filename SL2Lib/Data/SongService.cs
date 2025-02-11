using SL2Lib.Models;

namespace SL2Lib.Data
{
    internal class SongService
    {
        private readonly ISongRepo m_songRepo;

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
