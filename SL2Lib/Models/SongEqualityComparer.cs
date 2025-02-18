using System.Diagnostics.CodeAnalysis;

namespace SL2Lib.Models
{
    public class SongEqualityComparer : IEqualityComparer<Song>
    {
        public static SongEqualityComparer Default = new SongEqualityComparer();

        public bool Equals(Song? x, Song? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x == null || y == null) return false;
            return x.Name.Equals(y.Name, StringComparison.OrdinalIgnoreCase) &&
                   (x.Artist?.Equals(y.Artist, StringComparison.OrdinalIgnoreCase) ?? y.Artist == null);
        }

        public int GetHashCode([DisallowNull] Song obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            int hashName = obj.Name?.GetHashCode(StringComparison.OrdinalIgnoreCase) ?? 0;
            int hashArtist = obj.Artist?.GetHashCode(StringComparison.OrdinalIgnoreCase) ?? 0;
            return hashName ^ hashArtist;
        }
    }
}
