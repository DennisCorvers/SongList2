using Microsoft.Extensions.DependencyInjection;
using SL2Lib.Data;
using SL2Lib.Models;
using SongList2.Data;
using SongList2.ViewModels;
using System.IO;
using System.Windows;

namespace SongList2.Views
{
    /// <summary>
    /// Interaction logic for ExportWindow.xaml
    /// </summary>
    internal partial class ExportWindow : Window
    {
        private ExportViewModel ViewModel
        {
            get
            {
                return (ExportViewModel)DataContext;
            }
            init
            {
                DataContext = value;
            }
        }

        private readonly IAppSettings m_settings = null!;

        [ActivatorUtilitiesConstructor]
        public ExportWindow(ExportViewModel viewModel, IAppSettings settings)
        {
            InitializeComponent();
            ViewModel = viewModel;
            m_settings = settings;

            ViewModel.NotifyUser = DisplayMessage;
            ViewModel.RequestFolderSelection = GetOutputDirectory;
            ViewModel.OpenExportFolder = OpenFolder;
        }

        private string? GetOutputDirectory()
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.InitialDirectory = m_settings.LastExportLocation;
            dialog.Description = "Select a folder to export";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var parentFolder = Directory.GetParent(dialog.SelectedPath)?.FullName ?? dialog.SelectedPath;
                m_settings.LastExportLocation = parentFolder;
                return dialog.SelectedPath;
            }

            return null;
        }

        private void DisplayMessage(UserNotification notification)
        {
            MessageBox.Show(notification.Message, notification.Title, MessageBoxButton.OK, notification.Icon);
        }

        private void OpenFolder(string targetFolder)
        {
            if (Directory.Exists(targetFolder))
            {
                System.Diagnostics.Process.Start("explorer.exe", targetFolder);
            }
        }
    }
}
