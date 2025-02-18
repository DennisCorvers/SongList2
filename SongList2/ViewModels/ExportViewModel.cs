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
    internal class ExportViewModel : ViewModelBase
    {
        private readonly ISongService m_songService;
        private readonly IErrorLogger m_errorLogger;

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

        public ICommand SelectOutputDirectoryCommand { get; }
        public ICommand ExportCommand { get; }

        public Func<string?>? RequestFolderSelection { get; set; }
        public Action<string>? OpenExportFolder { get; set; }
        public Action<UserNotification>? NotifyUser { get; set; }

        public ExportViewModel(ISongService songService, IErrorLogger errorLogger)
        {
            m_songService = songService;
            m_errorLogger = errorLogger;
            SelectOutputDirectoryCommand = new RelayCommand(SelectOutputDirectory);
            ExportCommand = new RelayCommand(Export);
        }

        private void SelectOutputDirectory()
        {
            var selectedDir = RequestFolderSelection?.Invoke();
            if (selectedDir != null)
            {
                OutputDirectory = selectedDir;
            }
        }

        private void Export()
        {
            if (string.IsNullOrWhiteSpace(OutputDirectory))
            {
                NotifyUser?.Invoke(new UserNotification("Please select an output directory.", "Warning", MessageBoxImage.Warning));
                return;
            }

            if (!TryCreateOutputDirectory(OutputDirectory))
            {
                return;
            }

            // Filter non-exportable media and group by artist / album
            var exportableMedia = FilterUncoupledMedia(m_songService.SongList);
            //var groupedMedia = GroupMedia(exportableMedia);
        }

        private bool TryCreateOutputDirectory(string selectedOutput)
        {
            // Creates output root
            if (!Directory.Exists(selectedOutput))
            {
                try
                {
                    Directory.CreateDirectory(selectedOutput);
                }
                catch (Exception ex)
                {
                    NotifyUser?.Invoke(new UserNotification($"Error creating output directory; {ex.Message}", "Error", MessageBoxImage.Error));
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<Song> FilterUncoupledMedia(IEnumerable<Song> songList)
        {
            var missingMedia = songList
                .Where(song => string.IsNullOrEmpty(song.FilePath) || !File.Exists(song.FilePath))
                .ToList();

            foreach (var song in missingMedia)
            {
                m_errorLogger.LogMessage($"Skipping export: Missing media file \"{song.Name}\".", ErrorLevel.Error);
            }

            if (missingMedia.Any())
            {
                NotifyUser?.Invoke(new UserNotification(
                    $"{missingMedia.Count} song(s) were skipped due to missing media files. See the log for details.",
                    "Export Warning",
                    MessageBoxImage.Warning));
            }

            return songList.Except(missingMedia);
        }

    }
}
