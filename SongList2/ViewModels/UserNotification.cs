using System.Windows;

namespace SongList2.ViewModels
{
    internal class UserNotification
    {
        public string Message { get; set; }

        public string Title { get; set; }

        public MessageBoxImage Icon { get; set; }

        public UserNotification(string message, string title, MessageBoxImage icon = MessageBoxImage.Information)
        {
            Message = message;
            Title = title;
            Icon = icon;
        }
    }
}
