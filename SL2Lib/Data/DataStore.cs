using SL2Lib.Models;

namespace SL2Lib.Data
{
    public class DataStore : IDataLoader, IDataSaver
    {
        public const string Extension = "Song2";

        private readonly string m_filePath;

        public string Path => m_filePath;

        public DataStore(string filePath)
        {
            m_filePath = filePath;
        }

        public SongList Load()
        {
            throw new NotImplementedException();
        }

        public void Persist()
        {
            throw new NotImplementedException();
        }
    }
}
