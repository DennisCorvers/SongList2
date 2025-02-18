using SL2Lib.Models;
using System.Collections.Generic;

namespace SongList2.Data
{
    internal interface IDataAnalyser<T>
    {
        IEnumerable<string> Extensions { get; }

        IEnumerable<T> GetFileMetadata(string directory);
    }
}