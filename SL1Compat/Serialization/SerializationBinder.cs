using System.Runtime.Serialization;

namespace SL1Compat.Serialization
{
    public class LegacySerializationBinder : SerializationBinder
    {
        private readonly IReadOnlyDictionary<string, Type> m_typeMapping = new Dictionary<string, Type>
        {
            { "SongList.Data", typeof(Data) },
            { "System.Collections.Generic.List`1[[SongList.Song, SongList, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]", typeof(List<Song>) },
            { "SongList.Song", typeof(Song) }
        };

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (m_typeMapping.ContainsKey(typeName))
                return m_typeMapping[typeName];

            // Otherwise use the default behavior
            throw new Exception();
        }
    }
}
