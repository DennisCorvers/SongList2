using System;
using System.Windows;

namespace SongList2
{
    internal interface IWindowService
    {
        void ShowWindow<T>(Window? owner) where T : Window;

        void ShowDialog<T>(Window? owner) where T : Window;
    }

    internal class WindowService : IWindowService
    {
        private readonly IServiceProvider m_serviceProvider;

        public WindowService(IServiceProvider serviceProvider)
        {
            m_serviceProvider = serviceProvider;
        }

        public void ShowDialog<T>(Window? owner) where T : Window
        {
            if (m_serviceProvider.GetService(typeof(T)) is Window window)
            {
                window.Owner = owner;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.ShowDialog();
            }
        }

        public void ShowWindow<T>(Window? owner) where T : Window
        {
            if (m_serviceProvider.GetService(typeof(T)) is Window window)
            {
                window.Owner = owner;
                window.ShowDialog();
            }
        }
    }
}
