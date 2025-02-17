using SL2Lib.Models;

namespace SL2Lib.Data
{
    public class SongRepo : ISongRepo
    {
        private SongList m_songList;

        private IDataSaver? m_dataSaver;

        private readonly IDataLoaderFactory m_dataLoaderFactory;

        public HashSet<Song> Songs => m_songList.Songs;

        public SongRepo(IDataLoaderFactory dataLoaderFactory)
        {
            m_dataLoaderFactory = dataLoaderFactory;
            m_songList = new SongList();
        }

        public void Load(string? filePath)
        {
            var loader = m_dataLoaderFactory.CreateDataLoader(filePath);

            m_songList = loader.Load();
            if (loader is IDataSaver saver)
            {
                m_dataSaver = saver;
            }
        }

        public string Persist(string filePath)
        {
            m_dataSaver = new DataStore(filePath);
            return m_dataSaver.Persist(m_songList);
        }

        public string Persist()
        {
            if (m_dataSaver == null)
            {
                throw new InvalidOperationException("No filepath specified.");
            }

            return m_dataSaver.Persist(m_songList);
        }
    }
}
