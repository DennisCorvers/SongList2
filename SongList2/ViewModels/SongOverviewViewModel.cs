using SL2Lib.Data;
using SL2Lib.Models;
using SongList2.Data;
using SongList2.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace SongList2.ViewModels
{
    internal class SongOverviewViewModel : ViewModelBase
    {
        private const string DefaultTitle = "Unnamed song list";

        private ObservableCollection<Song> m_selectedSongs;

        private string m_title;
        private readonly ISongService m_service;
        private readonly IQueryService m_queryService;
        private readonly IAppSettings m_settings;

        private double m_mainWindowWidth;
        private double m_mainWindowHeight;
        private double m_mainWindowLeft;
        private double m_mainWindowTop;

        public ObservableBulkCollection<Song> Songs { get; set; }

        public ObservableCollection<Song> SelectedSongs
        {
            get
            {
                return m_selectedSongs;
            }
            set
            {
                m_selectedSongs = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get
            {
                return m_title;
            }
            set
            {
                SetProperty(ref m_title, value);
            }
        }

        public bool HasPendingChanges
            => m_service.HasPendingChanges;

        public double WindowHeight
        {
            get => m_mainWindowHeight;
            set
            {
                SetProperty(ref m_mainWindowHeight, value);
                m_settings.MainWindowHeight = value;
            }
        }

        public double WindowWidth
        {
            get => m_mainWindowWidth;
            set
            {
                SetProperty(ref m_mainWindowWidth, value);
                m_settings.MainWindowWidth = value;
            }
        }

        public double WindowLeft
        {
            get => m_mainWindowLeft;
            set
            {
                SetProperty(ref m_mainWindowLeft, value);
                m_settings.MainWindowLeft = value;
            }
        }

        public double WindowTop
        {
            get => m_mainWindowTop;
            set
            {
                SetProperty(ref m_mainWindowTop, value);
                m_settings.MainWindowTop = value;
            }
        }


        public SongOverviewViewModel(IAppSettings settings, ISongService songService, IQueryService queryService)
        {
            m_title = GetTitle(null);
            m_service = songService;
            m_queryService = queryService;
            m_settings = settings;
            m_selectedSongs = new ObservableCollection<Song>();
            Songs = new ObservableBulkCollection<Song>();

            SetWindowLayout();
        }

        private void SetWindowLayout()
        {
            WindowHeight = m_settings.MainWindowHeight;
            WindowWidth = m_settings.MainWindowWidth;

            // Only set the left / top if the application has been started at least once.
            if (m_settings.StartVersion > 0)
            {
                WindowTop = m_settings.MainWindowTop;
                WindowLeft = m_settings.MainWindowLeft;
            }
            else
            {
                WindowLeft = (SystemParameters.PrimaryScreenWidth - WindowWidth) / 2;
                WindowTop = (SystemParameters.PrimaryScreenHeight - WindowHeight) / 2;
            }
        }

        public void NewFile()
        {
            m_service.LoadSongs(null);
            Title = GetTitle(null);
            RefreshSongList();
            SelectedSongs.Clear();
        }

        public void OpenFile(string filePath)
        {
            m_service.LoadSongs(filePath);
            Title = GetTitle(filePath);
            RefreshSongList();
            SelectedSongs.Clear();
        }

        public bool SaveFile(string? filePath)
        {
            string? fileName;
            try
            {
                fileName = m_service.SaveSongs(filePath);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            Title = GetTitle(fileName);
            return true;
        }

        public void AddSongs(IEnumerable<Song> songs)
        {
            var addedSongs = m_service.AddSongs(songs);
            Songs.AddRange(addedSongs);
        }

        public void DeleteSongs(IEnumerable<Song> songs)
        {
            m_service.RemoveSongs(songs);
            Songs.RemoveRange(songs);
        }

        public void FindSongs(string query)
        {
            var foundSongs = m_queryService.FindSongs(query);
            Songs = new ObservableBulkCollection<Song>(foundSongs);
            OnPropertyChanged(nameof(Songs));
        }

        private void RefreshSongList()
        {
            Songs = new ObservableBulkCollection<Song>(m_service.SongList);
            OnPropertyChanged(nameof(Songs));
        }

        private static string GetTitle(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return DefaultTitle;
            }
            else
            {
                var fileName = Path.GetFileName(filePath);
                return char.ToUpper(fileName[0]) + fileName[1..];
            }
        }
    }
}
