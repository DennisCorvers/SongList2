using SL2Lib.Data;
using SongList2.Data;
using System.Windows;

namespace SongList2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var loader = DataLoaderFactory.Create("D:\\Downloads\\Song List\\Song List\\bh.Song");
            var dat = loader.Load();

            var dataStore = new DataStore("D:\\Downloads\\Song List\\Song List\\bh.Song2");
            var dat2 = dataStore.Load();
        }
    }
}
