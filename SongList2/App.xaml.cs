using Microsoft.Extensions.DependencyInjection;
using SL2Lib.Data;
using SL2Lib.Logging;
using SL2Lib.Models;
using SongList2;
using SongList2.Data;
using SongList2.Logging;
using SongList2.ViewModels;
using SongList2.Views;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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
            services.AddSingleton<IQueryService, QueryService>();
            services.AddSingleton<IDataLoaderFactory, DataLoaderFactory>();
            services.AddSingleton<IErrorLogger, FileLogger>();
            services.AddSingleton<IDataAnalyser<Song>, AudioMetadataAnalyser>();

            SetupMVVM(services);

            _serviceProvider = services.BuildServiceProvider();

            // Set up global exception handling
            SetupGlobalExceptionHandling();
        }

        private static IServiceCollection SetupMVVM(IServiceCollection services)
        {
            services.AddSingleton<IAppSettings, AppSettings>();
            services.AddSingleton<IWindowService, WindowService>();

            services.AddTransient<SongOverviewViewModel>();
            services.AddTransient<ExportViewModel>();

            services.AddTransient<MainWindow>();
            services.AddTransient<ExportWindow>();

            return services;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void SetupGlobalExceptionHandling()
        {
            DispatcherUnhandledException += (sender, args) =>
            {
                LogAndHandleException(args.Exception, "DispatcherUnhandledException");
                args.Handled = true;
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                LogAndHandleException(args.ExceptionObject as Exception, "UnhandledException");
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                LogAndHandleException(args.Exception, "UnobservedTaskException");
                args.SetObserved();
            };
        }

        private void LogAndHandleException(Exception? ex, string source)
        {
            if (ex == null) return;

            var logger = _serviceProvider.GetRequiredService<IErrorLogger>();
            logger.LogMessage($"[{source}] {ex.Message}\n{ex.StackTrace}", ErrorLevel.Fatal);

            MessageBox.Show("An unexpected error occurred. See the log file for details.",
                            "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
