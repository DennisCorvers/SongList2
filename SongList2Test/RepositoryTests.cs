using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SL2Lib.Data;
using SL2Lib.Models;

namespace SongList2Test
{
    [TestFixture]
    internal class RepositoryTests
    {
        private ISongRepo m_repo;
        private static readonly string m_filePath = Directory.GetCurrentDirectory() + "\\TestFile.Song2";


        [SetUp]
        public void SetUp()
        {
            m_repo = new SongRepo();
            m_repo.Songs.Add(new Song("Test Song", null, null));
        }

        [TearDown]
        public void TearDown()
        {
            try { File.Delete(m_filePath); }
            catch { }
        }

        [Test]
        public void FixWrongFileExtension()
        {
            var dir = GetDirectory("TestFile.abc");
            m_repo.Persist(dir);

            Assert.That(File.Exists(m_filePath));
        }

        [Test]
        public void FixMissingFileExtension()
        {
            var dir = GetDirectory("TestFile");
            m_repo.Persist(dir);

            Assert.That(File.Exists(m_filePath));
        }

        [Test]
        public void PersistWithoutPath()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                m_repo.Persist();
            });
        }

        [Test]
        public void PersistWithPath()
        {
            m_repo.Persist(m_filePath);
            Assert.DoesNotThrow(() =>
            {
                m_repo.Persist();
            });
        }

        [Test]
        public void TestLoad()
        {
            var songName = Guid.NewGuid();
            IDataLoader dataLoader = new DataLoader(songName);

            var repo = SongRepo.Load(dataLoader);

            Assert.That(repo.Songs.Count, Is.EqualTo(1));
            Assert.That(repo.Songs.First().Name, Is.EqualTo(songName.ToString()));
        }

        private static string GetDirectory(string fileName)
            => $"{Directory.GetCurrentDirectory()}\\{fileName}";

        private class DataLoader : IDataLoader
        {
            private readonly Guid m_guid;

            public DataLoader(Guid songName)
            {
                m_guid = songName;
            }

            public SongList Load()
            {
                var sl = new SongList();
                sl.Songs.Add(new Song(m_guid.ToString(), null, null));
                return sl;
            }
        }
    }
}
