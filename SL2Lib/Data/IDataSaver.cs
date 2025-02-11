using SL2Lib.Models;

namespace SL2Lib.Data
{
    public interface IDataSaver
    {
        string FilePath { get; }

        void Persist(SongList data);
    }
}
