using SL2Lib.Models;

namespace SL1Compat
{
    [Serializable]
    internal class Data
    {
        public List<Song> SongList;

        public string Name { get; set; }

        public Data(string name)
        {
            SongList = new List<Song>();
            Name = name;
        }
    }

    [Serializable]
    internal class Song
    {
        public string Name { get; private set; }

        public string Artist { get; private set; }

        public Song(string name)
        {
            Name = name;
        }
    }
}
