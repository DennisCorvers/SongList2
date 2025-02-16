using SL2Lib.Models;
using System.Windows.Input;

namespace SongList2.ViewModels
{
    internal class SongOverviewViewModel : ViewModelBase
    {
        private Song? m_selectedSong;

        public Song? SelectedSong
        {
            get
            {
                return m_selectedSong;
            }
            set
            {
                m_selectedSong = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddSongCommand { get; }

        public ICommand RemoveSongCommand { get; }
    }
}
