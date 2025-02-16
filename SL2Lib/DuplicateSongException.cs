using SL2Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL2Lib
{
    public class DuplicateSongException : Exception
    {
        public Song DuplicateSong { get; }

        public DuplicateSongException(Song duplicateSong)
            : base()
        {
            DuplicateSong = duplicateSong;
        }
    }
}
