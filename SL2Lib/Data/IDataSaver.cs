using SL2Lib.Models;

namespace SL2Lib.Data
{
    public interface IDataSaver
    {
        void Persist(SongList data);
    }
}
