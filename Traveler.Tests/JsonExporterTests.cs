using NUnit.Framework;
using System.IO;
using Traveler.Logic;
using Traveler.Models;

namespace Traveler.Tests
{
    [TestFixture]
    public class JsonExporterTests
    {
        private string _testFile;

        [SetUp]
        public void Setup()
        {
            _testFile = "test_export.json";
            if (File.Exists(_testFile))
            {
                File.Delete(_testFile);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_testFile))
            {
                File.Delete(_testFile);
            }
        }

        [Test]
        public void ExportToPath_CreatesFileWithContent()
        {
            var itinerary = new Itinerary
            {
                Segments = new System.Collections.Generic.List<CityConnection>
                {
                    new CityConnection("NYC", "LAX", 5, 300)
                }
            };

            JsonExporter.ExportToPath(itinerary, _testFile);

            Assert.That(File.Exists(_testFile), Is.True);
            var content = File.ReadAllText(_testFile);
            Assert.That(content, Does.Contain("NYC"));
            Assert.That(content, Does.Contain("LAX"));
            Assert.That(content, Does.Contain("300"));
        }
    }
}
