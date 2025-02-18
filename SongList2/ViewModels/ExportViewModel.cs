using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using SongList2.Commands;
using System.Windows.Forms;
using SL2Lib.Data;
using SL2Lib.Logging;
using SL2Lib.Models;

namespace SongList2.ViewModels
{
    internal class ExportViewModel : ViewModelBase, IProgress<MediaExportProgress>
    {
        private readonly ISongService m_songService;
        private readonly IErrorLogger m_errorLogger;

        private Visibility m_loadingOverlayVisibility;
        private string m_loadingText;
        private double m_progressValue;

        private string? m_outputDirectory;
        private bool m_createArtistFolders;
        private bool m_createAlbumFolders;
        private bool m_openFolderAfterExport;

        public string? OutputDirectory
        {
            get => m_outputDirectory;
            set { SetProperty(ref m_outputDirectory, value); }
        }

        public bool CreateArtistFolders
        {
            get => m_createArtistFolders;
            set { SetProperty(ref m_createArtistFolders, value); }
        }

        public bool CreateAlbumFolders
        {
            get => m_createAlbumFolders;
            set { SetProperty(ref m_createAlbumFolders, value); }
        }

        public bool OpenFolderAfterExport
        {
            get => m_openFolderAfterExport;
            set { SetProperty(ref m_openFolderAfterExport, value); }
        }

        public Visibility LoadingOverlayVisibility
        {
            get => m_loadingOverlayVisibility;
            set => SetProperty(ref m_loadingOverlayVisibility, value);
        }

        public string LoadingText
        {
            get => m_loadingText;
            set => SetProperty(ref m_loadingText, value);
        }

        public double ProgressValue
        {
            get => m_progressValue;
            set => SetProperty(ref m_progressValue, value);
        }

        public ExportViewModel(ISongService songService, IErrorLogger errorLogger)
        {
            m_songService = songService;
            m_errorLogger = errorLogger;
            m_loadingText = string.Empty;

            CreateArtistFolders = true;
            CreateAlbumFolders = true;
            OpenFolderAfterExport = true;
            LoadingOverlayVisibility = Visibility.Collapsed;
        }

        internal async Task<(int ErrorCount, int ExportCount)> Export()
        {
            LoadingOverlayVisibility = Visibility.Visible;
            LoadingText = string.Empty;
            ProgressValue = 0;

            // Filter non-exportable media
            var exportReport = await ExportMediaAsync(m_songService.SongList, this);

            LoadingOverlayVisibility = Visibility.Collapsed;

            return (exportReport.ErrorCount, exportReport.ExportCount);
        }

        private Task<ExportReport> ExportMediaAsync(IEnumerable<Song> exportableMedia, IProgress<MediaExportProgress> progress)
        {
            var task = Task.Run(() =>
            {
                int totalMedia = exportableMedia.Count();
                int errorCount = 0;
                int exportCount = 0;

                foreach (var song in exportableMedia)
                {
                    if (string.IsNullOrEmpty(song.FilePath) || !File.Exists(song.FilePath))
                    {
                        errorCount++;
                        m_errorLogger.LogMessage($"Skipping export: Missing media file \"{song.Name}\".", ErrorLevel.Error);
                        continue;
                    }

                    string artistFolder = GetOptionalFolderName(song.Artist, "Unknown Artist", CreateArtistFolders);
                    string albumFolder = GetOptionalFolderName(song.Album, "Unknown Album", CreateAlbumFolders);

                    string outputPath = Path.Combine(OutputDirectory!,
                        artistFolder,
                        albumFolder,
                        Path.GetFileName(song.FilePath)!);

                    try
                    {
                        var directoryPath = Path.GetDirectoryName(outputPath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath!);
                        }

                        File.Copy(song.FilePath!, outputPath, overwrite: true);
                        exportCount++;
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        m_errorLogger.LogMessage($"Error copying {song.FilePath} to {outputPath}: {ex.Message}", ErrorLevel.Error);
                    }

                    progress.Report(new MediaExportProgress(totalMedia, exportCount + errorCount, song.Name));
                }

                return new ExportReport(errorCount, exportCount);
            }
            );

            return task;
        }

        private static string GetOptionalFolderName(string? folderName, string defaultName, bool include)
        {
            if (!include)
            {
                return string.Empty;
            }

            return string.IsNullOrEmpty(folderName) ? defaultName : folderName;
        }

        void IProgress<MediaExportProgress>.Report(MediaExportProgress value)
        {
            LoadingText = $"Exporting {value.MediaName} ({value.Progress}/{value.TotalMedia})";
            ProgressValue = (double)value.Progress / (double)value.TotalMedia * 100;
        }

        private class ExportReport
        {
            public ExportReport(int errorCount, int exportCount)
            {
                ErrorCount = errorCount;
                ExportCount = exportCount;
            }

            public int ErrorCount { get; }

            public int ExportCount { get; }
        }
    }
}