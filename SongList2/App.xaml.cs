using Microsoft.Extensions.DependencyInjection;
using SL2Lib.Data;
using SongList2.ViewModels;
using SongList2.Views;
using System;
using System.Windows;

namespace YourNamespace
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ISongRepo, SongRepo>();
            services.AddSingleton<ISongService, SongService>();
            services.AddTransient<SongOverviewViewModel>();
            services.AddTransient<MainWindow>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetRequiredService<SongOverviewViewModel>();
            mainWindow.Show();
        }
    }
}
