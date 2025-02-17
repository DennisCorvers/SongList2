using Microsoft.Win32;
using SL2Lib.Logging;
using SL2Lib.Models;
using SongList2.Data;
using SongList2.Utils;
using SongList2.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SongList2.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SongOverviewViewModel ViewModel
        {
            get
            {
                return (SongOverviewViewModel)DataContext;
            }
            init
            {
                DataContext = value;
            }
        }

        private readonly IAppSettings m_settings;

        public MainWindow()
        {
            InitializeComponent();
        }

        internal MainWindow(SongOverviewViewModel viewModel, IAppSettings settings)
            : this()
        {
            ViewModel = viewModel;
            m_settings = settings;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            PendingChangesAskForSave(() => { e.Cancel = false; });
        }

        private void NewFileClick(object sender, RoutedEventArgs e)
        {
            PendingChangesAskForSave(ViewModel.NewFile);
        }

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select a Song File",
                Filter = "Song Files (*.song;*.song2)|*.song;*.song2",
                DefaultExt = ".song",
                InitialDirectory = m_settings.LastSaveLocation,
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                m_settings.LastSaveLocation = Path.GetDirectoryName(filePath) ?? string.Empty;

                string extension = Path.GetExtension(filePath).ToLower();
                string[] allowedExtensions = { ".song", ".song2" };

                if (!allowedExtensions.Contains(extension))
                {
                    MessageBox.Show("Invalid file type selected. Please select a .song or .song2 file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                try
                {
                    ViewModel.OpenFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveFileClick(object sender, RoutedEventArgs e)
        {
            // If this returns false, no prior file path is known.
            if (!ViewModel.SaveFile(null))
            {
                SaveFileAsClick(sender, e);
            }
        }

        private void SaveFileAsClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save Song File",
                Filter = "Song2 Files (*.song2)|*.song2",
                DefaultExt = ".song2",
                AddExtension = true,
                InitialDirectory = m_settings.LastSaveLocation
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                m_settings.LastSaveLocation = Path.GetDirectoryName(filePath) ?? string.Empty;

                try
                {
                    ViewModel.SaveFile(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExitApplicationClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SongsDataGridKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete) return;

            var selectedSongs = SongsDataGrid.SelectedItems.Cast<Song>().ToList();

            if (selectedSongs.Any())
            {
                string songCount = selectedSongs.Count == 1 ? "1 song" : $"{selectedSongs.Count} songs";
                var result = MessageBox.Show($"Are you sure you want to delete {songCount}?",
                                             "Confirm Deletion",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    ViewModel.DeleteSongs(selectedSongs);
                    SongsDataGrid.SelectedItems.Clear();
                }
            }

            e.Handled = true;
        }

        private void OpenLatestLogClick(object sender, RoutedEventArgs e)
        {
            var logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (Directory.Exists(logsDirectory) && Directory.GetFiles(logsDirectory, "*.txt").Any())
            {
                var latestLogFile = Directory.GetFiles(logsDirectory, "*.txt")
                                             .OrderByDescending(f => new FileInfo(f).CreationTime)
                                             .First();

                Process.Start(new ProcessStartInfo(latestLogFile) { UseShellExecute = true });
            }
            else
            {
                MessageBox.Show("No log files found in the Logs directory.", "No Logs", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void PendingChangesAskForSave(Action action)
        {
            if (ViewModel.HasPendingChanges)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Do you want to save your changes before exiting?",
                    "Unsaved Changes",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (!ViewModel.SaveFile(null))
                        {
                            SaveFileAsClick(null!, null!);
                            action();
                        }
                        break;

                    case MessageBoxResult.No:
                        action();
                        break;

                    case MessageBoxResult.Cancel:
                        break;
                }
            }
            else
            {
                action();
            }
        }

        private void SearchTextBox_TextChanged(object sender, string text)
        {
           
        }
    }
}