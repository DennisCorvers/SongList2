using ProtoBuf;

namespace SL2Lib.Models
{
    [ProtoContract]
    public class SongList
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public List<Song> Songs { get; }

        private SongList() { }

        public SongList(string name)
        {
            Name = name;
            Songs = new List<Song>();
        }
    }
}