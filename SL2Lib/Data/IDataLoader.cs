using SL2Lib.Models;

namespace SL2Lib.Data
{
    public interface IDataLoader
    {
        string FilePath { get; }

        SongList Load();
    }
}
