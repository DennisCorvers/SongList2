using SL2Lib.Data;
using SL2Lib.Models;

namespace SongList2Test.Factory
{
    internal class DataLoader : IDataLoader
    {
        private readonly Guid m_guid;

        public DataLoader(Guid songName)
        {
            m_guid = songName;
        }

        public SongList Load()
        {
            var sl = new SongList();
            sl.Songs.Add(new Song(m_guid.ToString(), null, null, null));
            return sl;
        }
    }
}
