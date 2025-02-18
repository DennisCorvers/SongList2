using SL2Lib.Logging;
using SL2Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SongList2.Data
{
    internal class AudioMetadataAnalyser : IDataAnalyser<Song>
    {
        private readonly HashSet<string> m_audioExtensions;

        private readonly IErrorLogger m_logger;

        public IEnumerable<string> Extensions
            => m_audioExtensions;

        public AudioMetadataAnalyser(IErrorLogger errorLogger)
        {
            m_audioExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".mp3", ".ogg", ".flac", ".mpc", ".spx", ".wav", ".aiff", ".mp4", ".ape", ".dsf", ".dff", ".aac"
            };


            m_logger = errorLogger;
        }

        public IEnumerable<Song> GetFileMetadata(string directory)
        {
            var files = Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
                .Where(x => m_audioExtensions.Contains(Path.GetExtension(x)));

            var songs = new List<Song>();
            foreach (var file in files)
            {
                TagLib.File tagFile;
                try
                {
                    tagFile = TagLib.File.Create(file);
                }
                catch (Exception e)
                {
                    m_logger.LogMessage($"{ e.Message} for file: {file}", ErrorLevel.Error);
                    continue;
                }

                var title = GetTitle(tagFile);
                if (string.IsNullOrEmpty(title))
                {
                    m_logger.LogMessage($"No title available for file: {tagFile.Name}", ErrorLevel.Error);
                    continue;
                }

                var song = new Song(
                    title,
                    GetArtists(tagFile),
                    tagFile.Tag.Album,
                    (int)tagFile.Tag.Year,
                    tagFile.Name);

                songs.Add(song);
            }

            return songs;
        }

        private static string? GetTitle(TagLib.File tagFile)
        {
            if (string.IsNullOrEmpty(tagFile.Tag.Title))
            {
                return Path.GetFileNameWithoutExtension(tagFile.Name);
            }
            else
            {
                return tagFile.Tag.Title;
            }
        }

        private static string GetArtists(TagLib.File tagFile)
            => string.Join(',', tagFile.Tag.Performers);
    }
}
