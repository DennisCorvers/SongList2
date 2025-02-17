using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongList2.Data
{
    internal interface IAppSettings
    {
        string LastSaveLocation { get; }

        string LastLoadLocation { get; }
    }
}
