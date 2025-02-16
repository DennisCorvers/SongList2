using ProtoBuf;

namespace SL2Lib.Models
{
    [ProtoContract]
    public class SongList
    {
        [ProtoMember(1)]
        public HashSet<Song> Songs { get; }

        public SongList()
        {
            Songs = new HashSet<Song>();
        }
    }
}