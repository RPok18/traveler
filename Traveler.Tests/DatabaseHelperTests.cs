using NUnit.Framework;
using System.IO;
using Traveler.Logic;
using Traveler.Models;

namespace Traveler.Tests
{
    [TestFixture]
    public class DatabaseHelperTests
    {
        private string _testDbFile;

        [SetUp]
        public void Setup()
        {
            _testDbFile = $"test_traveler_{System.Guid.NewGuid()}.db";
            DatabaseHelper.DbFile = _testDbFile;
        }

        [TearDown]
        public void TearDown()
        {
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            if (File.Exists(_testDbFile))
            {
                File.Delete(_testDbFile);
            }
        }

        [Test]
        public void InitializeDatabase_CreatesTable()
        {
            DatabaseHelper.InitializeDatabase();
            Assert.That(File.Exists(_testDbFile), Is.True);
        }

        [Test]
        public void SaveItinerary_InsertsRecord()
        {
            DatabaseHelper.InitializeDatabase();

            var request = new TravelRequest { Origin = "NYC", Destination = "LAX" };
            var itinerary = new Itinerary
            {
                Segments = new System.Collections.Generic.List<CityConnection>
                {
                    new CityConnection("NYC", "LAX", 5, 300)
                }
            };

            Assert.DoesNotThrow(() => DatabaseHelper.SaveItinerary(request, itinerary));
        }
    }
}
