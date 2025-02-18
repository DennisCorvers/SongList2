namespace SongList2.Data
{
    internal interface IAppSettings
    {
        string LastSaveLocation { get; set; }

        string LastImportLocation { get; set; }

        string LastExportLocation { get; set; }

        double MainWindowWidth { get; set; }

        double MainWindowHeight { get; set; }

        double MainWindowLeft { get; set; }

        double MainWindowTop { get; set; }

        int StartVersion { get; set; }
    }
}
