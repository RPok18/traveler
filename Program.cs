using System;
using Traveler.Models;

namespace Traveler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0].ToLower() == "help" || args[0].ToLower() == "--help")
            {
                CommandLineParser.ShowHelp();
                return;
            }

            string command = args[0].ToLower();
            var request = CommandLineParser.Parse(args);

            switch (command)
            {
                case "plan":
                    HandlePlan(request);
                    break;
                case "optimize":
                    Console.WriteLine("Optimization logic coming in Day 5-6!");
                    break;
                default:
                    Console.WriteLine($"Unknown command: {command}");
                    CommandLineParser.ShowHelp();
                    break;
            }
        }

        static void HandlePlan(TravelRequest request)
        {
            var result = request.Validate();
            if (!result.IsValid)
            {
                Console.WriteLine("Invalid travel request:");
                foreach (var error in result.Errors)
                    Console.WriteLine($"  ✗ {error}");
                Console.WriteLine("\nRun 'traveler help' for usage information.");
                return;
            }

            Console.WriteLine("Planning your itinerary...");
            Console.WriteLine(request.ToString());
            Console.WriteLine("\nDijkstra pathfinding coming in Day 3-4!");
        }
    }
}
