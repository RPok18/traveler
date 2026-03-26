using System;
using System.Collections.Generic;
using NUnit.Framework;
using Traveler.Logic;
using Traveler.Models;

namespace Traveler.Tests
{
    [TestFixture]
    public class AIRecommendationEngineTests
    {
        [Test]
        public void GenerateRecommendations_WithLowBudget_AddsBudgetTip()
        {
            var request = new TravelRequest { Budget = 300 };
            var itinerary = new Itinerary { Segments = new List<CityConnection> { new CityConnection("A", "B", 1.0) } };

            AIRecommendationEngine.GenerateRecommendations(request, itinerary);

            Assert.That(itinerary.Recommendations, Has.Some.Contains("budget is tight"));
        }

        [Test]
        public void GenerateRecommendations_WithHighBudget_AddsPremiumTip()
        {
            var request = new TravelRequest { Budget = 6000 };
            var itinerary = new Itinerary { Segments = new List<CityConnection> { new CityConnection("A", "B", 1.0) } };

            AIRecommendationEngine.GenerateRecommendations(request, itinerary);

            Assert.That(itinerary.Recommendations, Has.Some.Contains("generous budget"));
        }

        [Test]
        public void GenerateRecommendations_WithManyPassengers_AddsGroupTip()
        {
            var request = new TravelRequest { Passengers = 4 };
            var itinerary = new Itinerary { Segments = new List<CityConnection> { new CityConnection("A", "B", 1.0) } };

            AIRecommendationEngine.GenerateRecommendations(request, itinerary);

            Assert.That(itinerary.Recommendations, Has.Some.Contains("group of 4"));
        }

        [Test]
        public void GenerateRecommendations_WithLongDuration_AddsWeeklyPassTip()
        {
            var request = new TravelRequest { DurationDays = 8 };
            var itinerary = new Itinerary { Segments = new List<CityConnection> { new CityConnection("A", "B", 1.0) } };

            AIRecommendationEngine.GenerateRecommendations(request, itinerary);

            Assert.That(itinerary.Recommendations, Has.Some.Contains("weekly transit card"));
        }

        [Test]
        public void GenerateRecommendations_WithSummerDate_AddsPeakSeasonTip()
        {
            var request = new TravelRequest { StartDate = new DateTime(2027, 7, 15) };
            var itinerary = new Itinerary { Segments = new List<CityConnection> { new CityConnection("A", "B", 1.0) } };

            AIRecommendationEngine.GenerateRecommendations(request, itinerary);

            Assert.That(itinerary.Recommendations, Has.Some.Contains("peak tourist season"));
        }

        [Test]
        public void GenerateRecommendations_WithWinterDate_AddsWinterTip()
        {
            var request = new TravelRequest { StartDate = new DateTime(2027, 1, 10) };
            var itinerary = new Itinerary { Segments = new List<CityConnection> { new CityConnection("A", "B", 1.0) } };

            AIRecommendationEngine.GenerateRecommendations(request, itinerary);

            Assert.That(itinerary.Recommendations, Has.Some.Contains("Winter travel"));
        }

        [Test]
        public void GenerateRecommendations_WithPreferences_AddsSpecificTips()
        {
            var request = new TravelRequest { Preferences = new List<string> { "beach", "CULTURE" } };
            var itinerary = new Itinerary { Segments = new List<CityConnection> { new CityConnection("A", "B", 1.0) } };

            AIRecommendationEngine.GenerateRecommendations(request, itinerary);

            Assert.That(itinerary.Recommendations, Has.Some.Contains("beach trip"));
            Assert.That(itinerary.Recommendations, Has.Some.Contains("Seeking culture"));
        }
    }
}
