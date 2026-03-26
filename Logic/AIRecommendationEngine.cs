using System;
using Traveler.Models;

namespace Traveler.Logic
{
    public static class AIRecommendationEngine
    {
        public static void GenerateRecommendations(TravelRequest request, Itinerary itinerary)
        {
            if (itinerary == null || !itinerary.Found) return;

            // 1. Budget-based tips
            if (request.Budget < 500)
            {
                itinerary.Recommendations.Add("💡 AI Tip: Your budget is tight. Consider staying in hostels or taking overnight buses to save on accommodation.");
            }
            else if (request.Budget > 5000)
            {
                itinerary.Recommendations.Add("💡 AI Tip: With a generous budget, consider upgrading to premium economy for longer segments or booking boutique hotels.");
            }

            // 2. Passenger-based tips
            if (request.Passengers >= 3)
            {
                itinerary.Recommendations.Add("💡 AI Tip: Traveling in a group of " + request.Passengers + "? Look for group discounts on rail passes or family-sized vacation rentals.");
            }

            // 3. Duration-based tips
            if (request.DurationDays >= 7)
            {
                itinerary.Recommendations.Add("💡 AI Tip: For a trip of a week or more, a multi-day city pass or weekly transit card usually offers the best value.");
            }
            else if (request.DurationDays <= 3)
            {
                itinerary.Recommendations.Add("💡 AI Tip: A short weekend getaway! Stick to a central hotel to minimize transit time and maximize sightseeing.");
            }

            // 4. Seasonality tips
            if (request.StartDate.HasValue)
            {
                int month = request.StartDate.Value.Month;
                if (month >= 6 && month <= 8)
                {
                    itinerary.Recommendations.Add("💡 AI Tip: Summer is peak tourist season. Book major attractions well in advance to skip the lines.");
                }
                else if (month == 12 || month <= 2)
                {
                    itinerary.Recommendations.Add("💡 AI Tip: Winter travel! Pack layers and check if your destinations have festive winter markets or seasonal closures.");
                }
            }

            // 5. Destination/Preference tips
            if (request.Preferences.Contains("beach", StringComparer.OrdinalIgnoreCase))
            {
                itinerary.Recommendations.Add("💡 AI Tip: You requested a beach trip. Remember sunscreen, and consider waterproof bags for your electronics.");
            }
            if (request.Preferences.Contains("culture", StringComparer.OrdinalIgnoreCase))
            {
                itinerary.Recommendations.Add("💡 AI Tip: Seeking culture? Check museum free-entry days or local walking tours to get the most out of your visit.");
            }
        }
    }
}
