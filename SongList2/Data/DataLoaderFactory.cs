using SL1LegacySupport;
using SL2Lib.Data;
using System;
using System.IO;

namespace SongList2.Data
{
    public static class DataLoaderFactory
    {
        public static IDataLoader Create(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            // Try to match by extension first.
            var ext = Path.GetExtension(filePath);
            if (ext.StartsWith("."))
            {
                ext = ext[1..];
            }

            if (ext.Equals(LegacyLoader.Extension, StringComparison.CurrentCultureIgnoreCase))
            {
                return new LegacyLoader(filePath);
            }

            if (ext.Equals(DataStore.Extension, StringComparison.CurrentCultureIgnoreCase))
            {
                return new DataStore(filePath);
            }

            throw new Exception($"Unable to load file: {filePath}");
        }
    }
}
