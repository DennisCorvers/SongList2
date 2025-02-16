using SL2Lib.Models;

namespace SL2Lib.Data
{
    public interface ISongRepo
    {
        HashSet<Song> Songs { get; }

        void Load(string? filePath);

        void Persist();

        void Persist(string filePath);
    }
}
