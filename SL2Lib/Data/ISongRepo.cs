using SL2Lib.Models;

namespace SL2Lib.Data
{
    public interface ISongRepo
    {
        HashSet<Song> Songs { get; }

        void Persist();

        void Persist(string filePath);
    }
}
