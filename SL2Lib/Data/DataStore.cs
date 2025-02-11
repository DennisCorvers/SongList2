using SL2Lib.Models;

namespace SL2Lib.Data
{
    public class DataStore : IDataLoader, IDataSaver
    {
        public const string Extension = "Song2";

        private readonly string m_filePath;

        public string FilePath => m_filePath;

        public DataStore(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            var directory = Path.GetDirectoryName(filePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

            if (directory == null || fileNameWithoutExtension == null)
            {
                throw new InvalidOperationException($"Invalid file path: {filePath}");
            }

            var fileName = $"{fileNameWithoutExtension}.{Extension}";
            m_filePath = Path.Combine(directory, fileName);
        }

        public SongList Load()
        {
            using (var file = File.OpenRead(m_filePath))
            {
                return ProtoBuf.Serializer.Deserialize<SongList>(file);
            }
        }

        public void Persist(SongList data)
        {
            using (var file = File.Create(m_filePath))
            {
                ProtoBuf.Serializer.Serialize(file, data);
            }
        }
    }
}
