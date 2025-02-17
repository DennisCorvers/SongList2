using SL2Lib.Data;
using SL2Lib.Models;
using SongList2.Commands;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SongList2.ViewModels
{
    internal class SongOverviewViewModel : ViewModelBase
    {
        private const string DefaultTitle = "Unnamed song list";
        private Song? m_selectedSong;
        private string m_title;
        private ISongService m_service;

        public Song? SelectedSong
        {
            get
            {
                return m_selectedSong;
            }
            set
            {
                m_selectedSong = value;
                SetProperty(ref m_selectedSong, value);
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

        public SongOverviewViewModel(ISongService songService)
        {
            m_title = GetTitle(null);
            m_service = songService;
        }

        internal void NewFile()
        {
            m_service.LoadSongs(null);
            m_title = GetTitle(null);
        }

        internal void OpenFile(string filePath)
        {
            m_service.LoadSongs(filePath);
            m_title = GetTitle(filePath);
        }

        internal bool SaveFile(string? filePath)
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

        private static string GetTitle(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return DefaultTitle;
            }
            else
            {
                var fileName = Path.GetFileName(filePath);
                return char.ToUpper(fileName[0]) + fileName.Substring(1);
            }
        }
    }
}
