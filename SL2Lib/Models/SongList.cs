namespace SL2Lib.Models
{
    public class SongList
    {
        public string Name { get; set; }

        public List<Song> Songs { get; }

        public SongList(string name)
        {
            Name = name;
            Songs = new List<Song>();
        }
    }
}