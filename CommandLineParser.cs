using System;
using System.Collections.Generic;
using Traveler.Models;

namespace Traveler
{
    public static class CommandLineParser
    {
        public static TravelRequest Parse(string[] args)
        {
            var request = new TravelRequest();
            
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--from":
                        if (i + 1 < args.Length) request.Origin = args[++i];
                        break;
                    case "--to":
                        if (i + 1 < args.Length) request.Destination = args[++i];
                        break;
                    case "--budget":
                        if (i + 1 < args.Length && decimal.TryParse(args[++i], out decimal budget)) 
                            request.Budget = budget;
                        break;
                    case "--passengers":
                        if (i + 1 < args.Length && int.TryParse(args[++i], out int passengers)) 
                            request.Passengers = passengers;
                        break;
                    case "--days":
                        if (i + 1 < args.Length && int.TryParse(args[++i], out int days)) 
                            request.DurationDays = days;
                        break;
                }
            }

            return request;
        }

        public static void ShowHelp()
        {
            Console.WriteLine("Traveler AI - Decision Logic & Planning");
            Console.WriteLine("Usage: traveler [command] [options]");
            Console.WriteLine("\nCommands:");
            Console.WriteLine("  plan         Plan an itinerary");
            Console.WriteLine("  optimize     Optimize budget/priority");
            Console.WriteLine("  history      View saved trips");
            Console.WriteLine("\nOptions:");
            Console.WriteLine("  --from <city>       Origin city");
            Console.WriteLine("  --to <city>         Destination city");
            Console.WriteLine("  --budget <amount>   Total budget");
            Console.WriteLine("  --passengers <n>    Number of travelers");
            Console.WriteLine("  --days <n>          Duration of trip");
        }
    }
}
