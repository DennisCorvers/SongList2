using SL2Lib.Logging;
using SL2Lib.Models;
using System;
using System.IO;

namespace SongList2.Logging
{
    internal class FileLogger : IErrorLogger
    {
        private readonly string m_logfilePath;
        private uint m_errorCount = 0;
        public uint ErrorCount
        {
            get { return m_errorCount; }
        }

        public FileLogger()
        {
            var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
            m_logfilePath = Path.Combine(logsDirectory, $"{timestamp}.txt");
        }

        public void LogDuplicateSong(Song duplicateSong)
            => WriteMessage($"Duplicate song detected: {duplicateSong.Name} by {duplicateSong.Artist}", ErrorLevel.Info);

        public void LogMessage(string message, ErrorLevel errorLevel)
            => WriteMessage(message, ErrorLevel.Error);

        private void WriteMessage(string message, ErrorLevel errorLevel)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var logMessage = $"{timestamp} [{errorLevel.ToString().ToUpper()}] - {message}";
            File.AppendAllText(m_logfilePath, logMessage + Environment.NewLine);
            m_errorCount++;
        }
    }
}
