using SL2Lib.Models;

namespace SL2Lib.Data
{
    public class SongRepo : ISongRepo
    {
        private SongList m_songList;

        private IDataSaver? m_dataSaver;

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
