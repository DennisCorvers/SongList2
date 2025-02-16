using SL1Compat.Serialization;
using SL2Lib.Data;
using SL2Lib.Models;

namespace SL1Compat
{
    public class LegacyLoader : IDataLoader
    {
        public const string Extension = "Song";

        private readonly string m_filePath;

        public LegacyLoader(string filePath)
        {
            m_filePath = filePath;
        }

        public SongList Load()
        {
            var service = new DataService();
            Data? legacyData = null;

            try
            {
                legacyData = service.Load(m_filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to load legacy songlist file.", ex);
            }

            if (legacyData == null)
                throw new Exception("Unable to load legacy songlist file.");

            var data = new SongList();

            foreach (var song in legacyData.SongList.Select(MapSong))
            {
                data.Songs.Add(song);
            }

            return data;
        }

        private static SL2Lib.Models.Song MapSong(Song song)
            => new(song.Name, song.Artist, null);
    }
}
