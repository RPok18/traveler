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
            // Initial common connections (flight hours, price)
            AddBidirectional("JFK", "LHR", 7.0, 450m);
            AddBidirectional("JFK", "CDG", 7.5, 480m);
            AddBidirectional("LHR", "CDG", 1.5, 80m, "Train");
            AddBidirectional("CDG", "FCO", 2.0, 120m);
            AddBidirectional("FCO", "BER", 2.0, 110m);
            AddBidirectional("BER", "LHR", 1.5, 90m);
            AddBidirectional("LHR", "HND", 12.0, 800m);
            AddBidirectional("HND", "SYD", 9.5, 650m);
            AddBidirectional("SYD", "JFK", 18.0, 1200m); // Fastest
            AddBidirectional("CDG", "HND", 12.5, 820m);
            AddBidirectional("JFK", "LAX", 6.0, 300m);
            AddBidirectional("LAX", "HND", 10.5, 700m);
            AddBidirectional("LAX", "SYD", 14.0, 800m); // Cheapest via LAX (1100)
        }

        private void AddBidirectional(string city1, string city2, double cost, decimal price = 0m, string mode = "Flight")
        {
            _connections.Add(new CityConnection(city1, city2, cost, price, mode));
            _connections.Add(new CityConnection(city2, city1, cost, price, mode));
        }

        public Itinerary Plan(string origin, string destination, string via = "", OptimizationGoal goal = OptimizationGoal.Fastest, decimal maxBudget = 0m)
        {
            Itinerary result;
            if (string.IsNullOrWhiteSpace(via))
            {
                result = FindShortestPath(origin, destination, goal);
            }
            else
            {
                // If via is provided, plan A -> Via and then Via -> Destination
                var firstHalf = FindShortestPath(origin, via, goal);
                var secondHalf = FindShortestPath(via, destination, goal);

                if (!firstHalf.Found || !secondHalf.Found)
                {
                    result = new Itinerary(); // Found nothing or one leg failed.
                }
                else
                {
                    result = new Itinerary();
                    result.Segments.AddRange(firstHalf.Segments);
                    result.Segments.AddRange(secondHalf.Segments);
                }
            }

            // Budget filter
            if (result.Found && maxBudget > 0 && result.TotalPrice > maxBudget)
            {
                return new Itinerary(); // Exceeds budget
            }

            return result;
        }

        private Itinerary FindShortestPath(string startCity, string endCity, OptimizationGoal goal)
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
                    double weight = goal == OptimizationGoal.Fastest ? edge.Cost : (double)edge.Price;
                    double alt = distances[current] + weight;
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
