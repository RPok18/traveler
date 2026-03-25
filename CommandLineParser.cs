using System;
using System.Collections.Generic;
using System.Globalization;
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
                        if (i + 1 < args.Length) request.Origin = args[++i].Trim();
                        break;

                    case "--to":
                        if (i + 1 < args.Length) request.Destination = args[++i].Trim();
                        break;

                    case "--via":
                        if (i + 1 < args.Length) request.Via = args[++i].Trim();
                        break;

                    case "--budget":
                        if (i + 1 < args.Length &&
                            decimal.TryParse(args[++i], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal budget))
                            request.Budget = budget;
                        break;
                    
                    case "--max-budget":
                        if (i + 1 < args.Length &&
                            decimal.TryParse(args[++i], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal maxBudget))
                            request.MaxBudget = maxBudget;
                        break;
                    
                    case "--goal":
                        if (i + 1 < args.Length)
                        {
                            var g = args[++i];
                            if (Enum.TryParse<OptimizationGoal>(g, true, out var goal))
                            {
                                request.OptimizationGoal = goal;
                            }
                            else
                            {
                                Console.WriteLine($"Warning: could not parse --goal '{g}'. Using 'Fastest'.");
                            }
                        }
                        break;

                    case "--passengers":
                        if (i + 1 < args.Length && int.TryParse(args[++i], out int passengers))
                            request.Passengers = passengers;
                        break;

                    case "--days":
                        if (i + 1 < args.Length && int.TryParse(args[++i], out int days))
                            request.DurationDays = days;
                        break;

                    case "--date":
                        if (i + 1 < args.Length &&
                            DateTime.TryParseExact(args[++i], "yyyy-MM-dd",
                                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                            request.StartDate = date;
                        else if (i < args.Length)
                            Console.WriteLine($"Warning: could not parse --date '{args[i]}'. Expected format: yyyy-MM-dd");
                        break;

                    case "--preferences":
                        if (i + 1 < args.Length)
                        {
                            var raw = args[++i];
                            foreach (var pref in raw.Split(',', StringSplitOptions.RemoveEmptyEntries))
                            {
                                var p = pref.Trim();
                                if (!string.IsNullOrEmpty(p)) request.Preferences.Add(p);
                            }
                        }
                        break;
                        
                    case "--export":
                        if (i + 1 < args.Length) request.ExportPath = args[++i].Trim();
                        break;

                    case "--save":
                        request.SaveToDatabase = true;
                        break;
                }
            }

            return request;
        }

        public static void ShowHelp()
        {
            Console.WriteLine("Traveler AI - Decision Logic & Planning");
            Console.WriteLine("Usage: traveler [command] [options]");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  plan         Plan an itinerary");
            Console.WriteLine("  optimize     Optimize budget/priority  (coming Day 5-6)");
            Console.WriteLine("  history      View saved trips           (coming Day 7-8)");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --from <city>              Origin city                 (required)");
            Console.WriteLine("  --to <city>                Destination city            (required)");
            Console.WriteLine("  --budget <amount>          Total budget in USD         (required)");
            Console.WriteLine("  --max-budget <amount>      Maximum acceptable budget   (optional, Day 5-6)");
            Console.WriteLine("  --goal <Fastest|Cheapest>  Optimization priority       (optional, default: Fastest)");
            Console.WriteLine("  --days <n>                 Duration of trip (1-365)    (required)");
            Console.WriteLine("  --passengers <n>           Number of travelers (1-20)  (default: 1)");
            Console.WriteLine("  --date <yyyy-MM-dd>        Departure date              (optional)");
            Console.WriteLine("  --via <city>               Waypoint city               (optional, used in Day 3-4)");
            Console.WriteLine("  --preferences <tag,...>    Travel preferences          (optional)");
            Console.WriteLine("                             e.g. beach,culture,food");
            Console.WriteLine("  --export <path>            Export itinerary to JSON    (optional)");
            Console.WriteLine("  --save                     Save itinerary to history   (optional)");
        }
    }
}
