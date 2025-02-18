using Microsoft.Extensions.DependencyInjection;
using SL2Lib.Data;
using SL2Lib.Models;
using SongList2.Data;
using SongList2.ViewModels;
using System;
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
        }

        private void SelectOutputDirClick(object sender, RoutedEventArgs e)
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.InitialDirectory = m_settings.LastExportLocation;
            dialog.Description = "Select a folder to export";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var parentFolder = Directory.GetParent(dialog.SelectedPath)?.FullName ?? dialog.SelectedPath;
                m_settings.LastExportLocation = parentFolder;
                ViewModel.OutputDirectory = dialog.SelectedPath;
            }
        }

        private async void ExportClick(object sender, RoutedEventArgs e)
        {
            var outputDirectory = ViewModel.OutputDirectory;
            if (string.IsNullOrWhiteSpace(outputDirectory))
            {
                MessageBox.Show("Please select an output directory.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TryCreateOutputDirectory(outputDirectory))
            {
                return;
            }

            var (errorCount, exportCount) = await ViewModel.Export();

            if (errorCount > 0)
            {
                MessageBox.Show($"{errorCount} error(s) occured when trying to export media. See the log file for more information.", "Error exporting", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (exportCount > 0)
            {
                MessageBox.Show($"Exported {exportCount} files to {outputDirectory}", "Finished exporting", MessageBoxButton.OK, MessageBoxImage.Information);
                OpenFolder(outputDirectory);
            }

            this.Close();
        }

        private static void OpenFolder(string targetFolder)
        {
            if (Directory.Exists(targetFolder))
            {
                System.Diagnostics.Process.Start("explorer.exe", targetFolder);
            }
        }

        private static bool TryCreateOutputDirectory(string selectedOutput)
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
                    MessageBox.Show($"Error creating output directory; {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }
    }
}
