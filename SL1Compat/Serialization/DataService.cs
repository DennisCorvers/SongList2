using System.Runtime.Serialization.Formatters.Binary;

namespace SL1Compat.Serialization
{
    internal class DataService
    {
        public Data? Load(string path)
        {
            var formatter = new BinaryFormatter()
            {
                Binder = new LegacySerializationBinder()
            };
            using Stream serializationStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
#pragma warning disable SYSLIB0011 // Required for legacy compatibility
            return formatter.Deserialize(serializationStream) as Data;
#pragma warning restore SYSLIB0011
        }
    }
}
