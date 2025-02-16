using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SL2Lib.Data;
using SL2Lib.Models;
using SongList2Test.Factory;

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
            var loaderFactory = new DataLoaderFactory();

            m_repo = new SongRepo(loaderFactory);
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
            // Setup the factory to mock a songlist with 1 song
            var songName = Guid.NewGuid();
            var dataLoaderFactory = new DataLoaderFactory(songName);

            // Create a new repo and load the mocked data.
            var repo = new SongRepo(dataLoaderFactory);
            repo.Load(null);

            Assert.That(repo.Songs.Count, Is.EqualTo(1));
            Assert.That(repo.Songs.First().Name, Is.EqualTo(songName.ToString()));
        }

        private static string GetDirectory(string fileName)
            => $"{Directory.GetCurrentDirectory()}\\{fileName}";
    }
}
