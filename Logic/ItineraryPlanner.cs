using System;
using System.Collections.Generic;
using System.Linq;
using Traveler.Models;

namespace Traveler.Logic
{
    public class ItineraryPlanner
    {
        private readonly List<CityConnection> _connections = new List<CityConnection>();

        public ItineraryPlanner()
        {
            // Initial common connections (flight hours)
            AddBidirectional("JFK", "LHR", 7.0);
            AddBidirectional("JFK", "CDG", 7.5);
            AddBidirectional("LHR", "CDG", 1.5, "Train");
            AddBidirectional("CDG", "FCO", 2.0);
            AddBidirectional("FCO", "BER", 2.0);
            AddBidirectional("BER", "LHR", 1.5);
            AddBidirectional("LHR", "HND", 12.0);
            AddBidirectional("HND", "SYD", 9.5);
            AddBidirectional("SYD", "JFK", 20.0);
            AddBidirectional("CDG", "HND", 12.5);
            AddBidirectional("JFK", "LAX", 6.0);
            AddBidirectional("LAX", "HND", 10.5);
            AddBidirectional("LAX", "SYD", 14.0);
        }

        private void AddBidirectional(string city1, string city2, double cost, string mode = "Flight")
        {
            _connections.Add(new CityConnection(city1, city2, cost, mode));
            _connections.Add(new CityConnection(city2, city1, cost, mode));
        }

        public Itinerary Plan(string origin, string destination, string via = "")
        {
            if (string.IsNullOrWhiteSpace(via))
            {
                return FindShortestPath(origin, destination);
            }

            // If via is provided, plan A -> Via and then Via -> Destination
            var firstHalf = FindShortestPath(origin, via);
            var secondHalf = FindShortestPath(via, destination);

            if (!firstHalf.Found || !secondHalf.Found)
            {
                return new Itinerary(); // Found nothing or one leg failed.
            }

            var fullItinerary = new Itinerary();
            fullItinerary.Segments.AddRange(firstHalf.Segments);
            fullItinerary.Segments.AddRange(secondHalf.Segments);
            return fullItinerary;
        }

        private Itinerary FindShortestPath(string startCity, string endCity)
        {
            var distances = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
            var previous = new Dictionary<string, CityConnection>(StringComparer.OrdinalIgnoreCase);
            var unvisited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Initialize
            var cities = _connections.Select(c => c.From).Concat(_connections.Select(c => c.To)).Distinct(StringComparer.OrdinalIgnoreCase);
            foreach (var city in cities)
            {
                distances[city] = double.MaxValue;
                unvisited.Add(city);
            }

            if (!distances.ContainsKey(startCity)) return new Itinerary(); // Start city not in graph

            distances[startCity] = 0;

            while (unvisited.Count > 0)
            {
                // Find node with minimum distance
                string current = unvisited.OrderBy(u => distances[u]).First();
                if (distances[current] == double.MaxValue) break; // Rest are unreachable

                if (string.Equals(current, endCity, StringComparison.OrdinalIgnoreCase)) break;

                unvisited.Remove(current);

                var neighbors = _connections.Where(c => string.Equals(c.From, current, StringComparison.OrdinalIgnoreCase));
                foreach (var edge in neighbors)
                {
                    double alt = distances[current] + edge.Cost;
                    if (alt < distances[edge.To])
                    {
                        distances[edge.To] = alt;
                        previous[edge.To] = edge;
                    }
                }
            }

            // Reconstruct path
            var itinerary = new Itinerary();
            string step = endCity;
            while (previous.ContainsKey(step))
            {
                var edge = previous[step];
                itinerary.Segments.Insert(0, edge);
                step = edge.From;
            }

            return itinerary;
        }

        public IEnumerable<string> GetKnownCities() => 
            _connections.Select(c => c.From).Concat(_connections.Select(c => c.To)).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(c => c);
    }
}
