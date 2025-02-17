namespace SongList2.Data
{
    internal interface IAppSettings
    {
        string LastSaveLocation { get; set; }

        string LastImportLocation { get; set; }

        string LastExportLocation { get; set; }
    }
}
