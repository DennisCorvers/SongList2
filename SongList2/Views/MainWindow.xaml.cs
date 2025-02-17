using Microsoft.Win32;
using SongList2.Data;
using SongList2.ViewModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

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

                ViewModel.OpenFile(openFileDialog.FileName);
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
    }
}