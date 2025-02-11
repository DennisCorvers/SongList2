using SL2Lib.Models;

namespace SL2Lib.Data
{
    public class SongRepo : ISongRepo
    {
        private SongList m_songList;

        private string? m_filePath;

        public ICollection<Song> Songs => m_songList.Songs;

        public SongRepo(string name)
        {
            m_songList = new SongList(name);
        }

        public static SongRepo Load(IDataLoader loader)
        {
            // string.Empty gets overwritten by the load.
            var songRepo = new SongRepo(string.Empty)
            {
                m_songList = loader.Load(),
                m_filePath = loader.Path
            };

            return songRepo;
        }

        public void Persist(string filePath)
        {
            m_filePath = filePath;

            IDataSaver dataStore = new DataStore(filePath);
            dataStore.Persist();
        }

        public void Persist()
        {
            if (string.IsNullOrWhiteSpace(m_filePath))
            {
                throw new InvalidOperationException("No filepath specified.");
            }

            IDataSaver dataStore = new DataStore(m_filePath);
            dataStore.Persist();
        }
    }
}
