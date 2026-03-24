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

        [Test]
        public void Path_ExceedingBudget_ShouldReturnNotFound()
        {
            var planner = new ItineraryPlanner();
            // JFK -> LHR base price is $450
            var itinerary = planner.Plan("JFK", "LHR", maxBudget: 400m);
            Assert.That(itinerary.Found, Is.False);
        }

        [Test]
        public void Path_Cheapest_ShouldPrioritizePriceOverTime()
        {
            var planner = new ItineraryPlanner();
            // JFK -> SYD:
            // Fastest: Direct 18h, $1200
            // Cheapest: via LAX 20h, $1100 (JFK->LAX $300 + LAX->SYD $800)
            
            var fastest = planner.Plan("JFK", "SYD", goal: OptimizationGoal.Fastest);
            Assert.That(fastest.TotalCost, Is.EqualTo(18.0));
            Assert.That(fastest.TotalPrice, Is.EqualTo(1200m));
            Assert.That(fastest.Segments.Count, Is.EqualTo(1));

            var cheapest = planner.Plan("JFK", "SYD", goal: OptimizationGoal.Cheapest);
            Assert.That(cheapest.TotalPrice, Is.EqualTo(1100m));
            Assert.That(cheapest.TotalCost, Is.EqualTo(20.0));
            Assert.That(cheapest.Segments.Count, Is.EqualTo(2));
            Assert.That(cheapest.Segments.Any(s => s.To == "LAX" || s.From == "LAX"), Is.True);
        }
    }
}
