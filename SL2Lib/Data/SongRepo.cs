using SL2Lib.Models;

namespace SL2Lib.Data
{
    public class SongRepo : ISongRepo
    {
        private SongList m_songList;

        private IDataSaver? m_dataSaver;

        public HashSet<Song> Songs => m_songList.Songs;

        public SongRepo()
        {
            m_songList = new SongList();
        }

        public static SongRepo Load(IDataLoader loader)
        {
            var songRepo = new SongRepo()
            {
                m_songList = loader.Load(),
            };

            if (loader is IDataSaver saver)
            {
                songRepo.m_dataSaver = saver;
            }

            return songRepo;
        }

        public void Persist(string filePath)
        {
            m_dataSaver = new DataStore(filePath);
            m_dataSaver.Persist(m_songList);
        }

        public void Persist()
        {
            if (m_dataSaver == null)
            {
                throw new InvalidOperationException("No filepath specified.");
            }

            m_dataSaver.Persist(m_songList);
        }
    }
}
