using NUnit.Framework;
using SL2Lib.Data;
using SL2Lib.Models;
using SongList2Test.Factory;

namespace SongList2Test
{
    [TestFixture]
    internal class QueryServiceTests
    {
        private ISongRepo m_repo;
        private IQueryService m_queryService;

        private Song[] m_songs;

        private static string NewGuid => Guid.NewGuid().ToString();

        [SetUp]
        public void SetUp()
        {
            m_repo = new SongRepo(new DataLoaderFactory());

            m_songs = new Song[]
            {
                new Song(NewGuid, "Some Artist", "First Album", 1),
                new Song(NewGuid, "Some Artist", "Some Album", 2),
                new Song(NewGuid, null, "Second Album", 3)
            };

            foreach (var s in m_songs)
            {
                m_repo.Songs.Add(s);
            }

            m_queryService = new QueryService(m_repo);
        }

        [Test]
        public void SongList()
        {
            var songList = m_queryService.SongList;
            CollectionAssert.AreEqual(m_repo.Songs, songList);
        }

        [Test]
        public void FindAllSongs()
        {
            var result = m_queryService.FindSongs(null);
            Assert.That(result.Count(), Is.EqualTo(3));

        }

        [Test]
        public void FindOneSong()
        {
            var result = m_queryService.FindSongs(m_songs[0].Name);
            var song = result.First();
            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(1));
                Assert.That(m_songs[0], Is.EqualTo(song));
            });
        }

        [Test]
        public void FindMultipleProperties()
        {
            var result = m_queryService
                .FindSongs("Some")
                .ToArray();
            var song = result.First();

            Assert.That(result.Length, Is.EqualTo(2));

            Assert.That(result, Has.Member(m_songs[0]));
            Assert.That(result, Has.Member(m_songs[1]));
        }

        [Test]
        public void FindCaseInsensitive()
        {
            var result = m_queryService.FindSongs("sEcoNd ALBuM");
            var song = result.First();
            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(1));
                Assert.That(m_songs[2], Is.EqualTo(song));
            });
        }
    }
}
