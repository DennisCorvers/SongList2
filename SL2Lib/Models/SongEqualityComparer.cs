using System.Diagnostics.CodeAnalysis;

namespace SL2Lib.Models
{
    public class SongEqualityComparer : IEqualityComparer<Song>
    {
        public static SongEqualityComparer Default = new SongEqualityComparer();

        public bool Equals(Song? x, Song? y)
        {
            if (x == null || y == null)
                return false;

            return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(x.Artist, y.Artist, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(x.Album, y.Album, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Song obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            int hash = StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);

            if (obj.Artist != null)
                hash = HashCode.Combine(hash, StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Artist));

            if (obj.Album != null)
                hash = HashCode.Combine(hash, StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Album));

            return hash;
        }
    }
}
