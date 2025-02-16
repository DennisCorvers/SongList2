using SL2Lib.Data;
using SL2Lib.Models;
using System.Windows.Input;

namespace SongList2.ViewModels
{
    internal class SongOverviewViewModel : ViewModelBase
    {
        private Song? m_selectedSong;
        private string m_title;


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
                m_title = value;
                SetProperty(ref m_title, value);
            }
        }


        public ICommand AddSongCommand { get; }

        public ICommand RemoveSongCommand { get; }

        public SongOverviewViewModel(ISongService songService)
        {
            m_title = "Unnamed list";
        }
    }
}
