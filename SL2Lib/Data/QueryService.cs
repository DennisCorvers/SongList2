using ProtoBuf;
using SL2Lib.Models;
using System.Reflection;

namespace SL2Lib.Data
{
    public class QueryService : IQueryService
    {
        private readonly ICollection<PropertyInfo> m_properties;

        private readonly ISongRepo m_repo;

        public QueryService(ISongRepo repo)
        {
            // Find all properties listed for serialization.
            m_properties = typeof(Song)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttribute<ProtoMemberAttribute>() != null)
                .ToArray();

            m_repo = repo;
        }

        public IEnumerable<Song> SongList
            => m_repo.Songs;

        public IEnumerable<Song> FindSongs(string? query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return m_repo.Songs;
            }

            var result = m_repo.Songs.Where(x => m_properties
            .Any(prop =>
            {
                var propValue = prop.GetValue(x) ?? string.Empty;
                var propString = propValue.ToString();

                return !string.IsNullOrEmpty(propString) && propString.Contains(query, StringComparison.OrdinalIgnoreCase);
            }));

            return result;
        }
    }
}
