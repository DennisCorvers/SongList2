using System;

namespace SongList2.Data
{

    internal class AppSettings : IAppSettings
    {
        double IAppSettings.MainWindowWidth
        {
            get => Settings.Default.MainWindowWidth;
            set => Settings.Default.MainWindowWidth = value;
        }

        double IAppSettings.MainWindowHeight
        {
            get => Settings.Default.MainWindowHeight;
            set => Settings.Default.MainWindowHeight = value;
        }

        double IAppSettings.MainWindowLeft
        {
            get => Settings.Default.MainWindowLeft;
            set => Settings.Default.MainWindowLeft = value;
        }

        double IAppSettings.MainWindowTop
        {
            get => Settings.Default.MainWindowTop;
            set => Settings.Default.MainWindowTop = value;
        }

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

        int IAppSettings.StartVersion
        {
            get
            {
                return Settings.Default.StartVersion;
            }
            set
            {
                Settings.Default.StartVersion = value;
                Settings.Default.Save();
            }
        }
    }
}
