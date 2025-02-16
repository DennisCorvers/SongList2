using SL1LegacySupport;
using SL2Lib.Data;
using SL2Lib.Models;
using System;
using System.IO;

namespace SongList2.Data
{
    public class DataLoaderFactory : IDataLoaderFactory
    {
        public IDataLoader CreateDataLoader(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return new EmptyDataLoader();
            }

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

        private class EmptyDataLoader : IDataLoader
        {
            public SongList Load() => new();
        }
    }
}
