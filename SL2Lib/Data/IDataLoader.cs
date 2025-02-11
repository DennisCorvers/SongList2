using SL2Lib.Models;

namespace SL2Lib.Data
{
    public interface IDataLoader
    {
        string Path { get; }

        SongList Load();
    }
}
