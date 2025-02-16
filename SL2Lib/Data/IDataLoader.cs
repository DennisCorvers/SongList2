using SL2Lib.Models;

namespace SL2Lib.Data
{
    public interface IDataLoader
    {
        SongList Load();
    }
}
