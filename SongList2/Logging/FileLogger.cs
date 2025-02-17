using SL2Lib.Logging;
using SL2Lib.Models;
using System;
using System.IO;

namespace SongList2.Logging
{
    internal class FileLogger : IErrorLogger
    {
        private readonly string m_logfilePath;

        public FileLogger()
        {
            var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
            m_logfilePath = Path.Combine(logsDirectory, $"{timestamp}.txt");

            using (File.Create(m_logfilePath)) { }
        }

        public void LogDuplicateSong(Song duplicateSong)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var logMessage = $"{timestamp} - Duplicate song detected: {duplicateSong.Name} by {duplicateSong.Artist}";
            File.AppendAllText(m_logfilePath, logMessage + Environment.NewLine);
        }
    }
}
