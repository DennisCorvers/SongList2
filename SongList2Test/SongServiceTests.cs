using NUnit.Framework;
using SL2Lib;
using SL2Lib.Data;
using SL2Lib.Models;
using SongList2Test.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongList2Test
{
    [TestFixture]
    internal class SongServiceTests
    {
        private ISongService m_service;
        private Song[] m_songs;

        private static string NewGuid => Guid.NewGuid().ToString();

        [SetUp]
        public void SetUp()
        {
            var repo = new SongRepo(new DataLoaderFactory());
            m_service = new SongService(repo);
            m_songs = new Song[3]
            {
                new Song(NewGuid, null, null),
                new Song(NewGuid, null, null),
                new Song(NewGuid, null, null),
            };
        }

        [Test]
        public void AddSong()
        {
            m_service.AddSong(m_songs[0]);

            Assert.That(m_songs[0], Is.EqualTo(m_service.SongList.First()));
            Assert.That(m_service.SongList.Count(), Is.EqualTo(1));
        }

        [Test]
        public void AddSongs()
        {
            var result = m_service.AddSongs(m_songs);

            Assert.That(result.Count(), Is.EqualTo(m_songs.Length));
            CollectionAssert.AreEqual(result, m_songs);
        }

        [Test]
        public void AddDuplicateSong()
        {
            m_service.AddSong(m_songs[0]);
            Assert.That(m_service.SongList.Count(), Is.EqualTo(1));

            var exception = Assert.Throws<DuplicateSongException>(() =>
            {
                m_service.AddSong(m_songs[0]);
            });

            Assert.NotNull(exception);
            Assert.That(exception.DuplicateSong, Is.EqualTo(m_songs[0]));
            Assert.That(m_service.SongList.Count(), Is.EqualTo(1));
        }

        [Test]
        public void RemoveSong()
        {
            var removedSong = m_songs[1];
            var result = m_service.AddSongs(m_songs);

            Assert.That(result.Count(), Is.EqualTo(m_songs.Length));
            Assert.That(m_service.SongList.Count(), Is.EqualTo(m_songs.Length));

            m_service.RemoveSongs(new Song[1] { removedSong });
            Assert.That(m_service.SongList.Count(), Is.EqualTo(m_songs.Length - 1));

            // Get Enumerable that excludes the removed song.
            var excludedSongs = m_songs.AsEnumerable().Where(x => x != removedSong);
            CollectionAssert.AreEqual(excludedSongs, m_service.SongList);
        }

        [Test]
        public void FindSong()
        {
            const string title = "Test Song";
            m_service.AddSongs(m_songs);
            m_service.AddSongs(new Song[1] { new Song(title, default, default) });

            var result1 = m_service.FindSongs(null);
            Assert.That(result1.Count(), Is.EqualTo(4));

            var result2 = m_service.FindSongs(title);
            Assert.That(result2.Count(), Is.EqualTo(1));

            var song = result2.First();
            Assert.That(song.Name, Is.EqualTo(title));
        }
    }
}
