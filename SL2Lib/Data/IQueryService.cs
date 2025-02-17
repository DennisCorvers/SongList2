﻿using SL2Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL2Lib.Data
{
    public interface IQueryService
    {
        IEnumerable<Song> SongList { get; }

        IEnumerable<Song> FindSongs(string query);
    }
}
