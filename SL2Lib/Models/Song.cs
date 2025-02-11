using ProtoBuf;

namespace SL2Lib.Models
{
    using ProtoBuf;

    [ProtoContract]
    public record Song
    {
        [ProtoMember(1)]
        public string Name { get; init; }

        [ProtoMember(2)]
        public string? Artist { get; init; }

        [ProtoMember(3)]
        public string? Album { get; init; }

        public Song(string name, string? artist, string? album)
        {
            Name = name;
            Artist = artist;
            Album = album;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Song() { }
#pragma warning restore CS8618
    }

}
