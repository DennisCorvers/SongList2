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

        [ProtoMember(4)]
        public int? Year { get; init; }

        [ProtoMember(5)]
        public string? FilePath { get; init; }

        public string YearString
        {
            get => (Year.HasValue && Year.Value > 0) ? Year.Value.ToString() : string.Empty;
        }


        public Song(string name, string? artist, string? album, int? year, string? path = null)
        {
            Name = name;
            Artist = artist;
            Album = album;
            Year = year;
            FilePath = path;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private Song() { }
#pragma warning restore CS8618
    }

}
