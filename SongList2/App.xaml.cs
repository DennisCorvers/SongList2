using Microsoft.Extensions.DependencyInjection;
using SL2Lib.Data;
using SongList2;
using SongList2.Data;
using SongList2.ViewModels;
using SongList2.Views;
using System;
using System.Windows;

namespace SongList2
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ISongRepo, SongRepo>();
            services.AddSingleton<ISongService, SongService>();
            services.AddSingleton<IDataLoaderFactory, DataLoaderFactory>();
            services.AddTransient<SongOverviewViewModel>();

            services.AddTransient(x => new MainWindow(x.GetRequiredService<SongOverviewViewModel>()));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
