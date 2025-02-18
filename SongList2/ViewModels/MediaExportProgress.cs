namespace SongList2.ViewModels
{
    internal class MediaExportProgress
    {
        public MediaExportProgress(int totalMedia, int progress, string mediaName)
        {
            TotalMedia = totalMedia;
            Progress = progress;
            MediaName = mediaName;
        }

        public int TotalMedia { get; }

        public int Progress { get; }

        public string MediaName { get; }
    }
}
