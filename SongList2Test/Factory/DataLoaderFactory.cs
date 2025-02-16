using SL2Lib.Data;
using SL2Lib.Models;

namespace SongList2Test.Factory
{
    internal class DataLoaderFactory : IDataLoaderFactory
    {
        private readonly Guid? m_songName;

        public DataLoaderFactory(Guid? songName = default)
        {
            m_songName = songName;
        }

        public IDataLoader CreateDataLoader(string? filePath)
        {
            if (!m_songName.HasValue)
            {
                return new EmptyDataLoader();
            }
            else
            {
                return new DataLoader(m_songName!.Value);
            }
        }

        private class EmptyDataLoader : IDataLoader
        {
            SongList IDataLoader.Load() => new();
        }
    }
}
