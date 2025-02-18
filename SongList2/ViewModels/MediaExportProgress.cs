namespace SongList2.ViewModels
{
    internal class MediaExportProgress
    {
        public MediaExportProgress(int totalMedia, int progress)
        {
            TotalMedia = totalMedia;
            Progress = progress;
        }

        public int TotalMedia { get; }

        public int Progress { get; }
    }
}
