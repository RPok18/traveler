using Traveler.Logic;
using Traveler.Models;
using NUnit.Framework;
using System.Linq;

namespace Traveler.Tests
{
    [TestFixture]
    public class ItineraryPlannerTests
    {
        [Test]
        public void FindDirectPath_ShouldReturnSingleSegment()
        {
            var planner = new ItineraryPlanner();
            var itinerary = planner.Plan("JFK", "LHR");

            Assert.That(itinerary.Found, Is.True);
            Assert.That(itinerary.Segments.Count, Is.EqualTo(1));
            Assert.That(itinerary.Segments[0].From, Is.EqualTo("JFK"));
            Assert.That(itinerary.Segments[0].To, Is.EqualTo("LHR"));
            Assert.That(itinerary.TotalCost, Is.EqualTo(7.0));
        }

        [Test]
        public void FindMultiHopPath_ShouldReturnMultipleSegments()
        {
            var planner = new ItineraryPlanner();
            var itinerary = planner.Plan("JFK", "CDG");

            Assert.That(itinerary.Found, Is.True);
            Assert.That(itinerary.Segments, Is.Not.Empty);
            Assert.That(itinerary.Origin, Is.EqualTo("JFK"));
            Assert.That(itinerary.Destination, Is.EqualTo("CDG"));
        }

        [Test]
        public void PathVia_ShouldEnforceWaypoint()
        {
            var planner = new ItineraryPlanner();
            var itinerary = planner.Plan("JFK", "LHR", via: "SYD");

            Assert.That(itinerary.Found, Is.True);
            Assert.That(itinerary.Segments.Any(s => s.To == "SYD" || s.From == "SYD"), Is.True);
            Assert.That(itinerary.Origin, Is.EqualTo("JFK"));
            Assert.That(itinerary.Destination, Is.EqualTo("LHR"));
        }

        [Test]
        public void UnknownCity_ShouldReturnNotFound()
        {
            var planner = new ItineraryPlanner();
            var itinerary = planner.Plan("Atlantis", "El Dorado");

            Assert.That(itinerary.Found, Is.False);
        }
    }
}
