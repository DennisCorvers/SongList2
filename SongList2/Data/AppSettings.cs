using System;

namespace SongList2.Data
{
    internal class AppSettings : IAppSettings
    {
        string IAppSettings.LastSaveLocation
        {
            get
            {
                return Settings.Default.LastSaveLocation;
            }
            set
            {
                Settings.Default.LastSaveLocation = value;
                Settings.Default.Save();
            }
        }

        string IAppSettings.LastImportLocation
        {
            get
            {
                return Settings.Default.LastImportLocation;
            }
            set
            {
                Settings.Default.LastImportLocation = value;
                Settings.Default.Save();
            }
        }

        string IAppSettings.LastExportLocation
        {
            get
            {
                return Settings.Default.LastExportLocation;
            }
            set
            {
                Settings.Default.LastExportLocation = value;
                Settings.Default.Save();
            }
        }
    }
}
