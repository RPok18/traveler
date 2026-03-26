using System;
using System.Collections.Generic;
using System.Linq;

namespace Traveler.Models
{
    public enum OptimizationGoal { Fastest, Cheapest }

    public class TravelRequest
    {
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public int Passengers { get; set; } = 1;
        public DateTime? StartDate { get; set; }
        public int DurationDays { get; set; }
        public List<string> Preferences { get; set; } = new List<string>();
        public string Via { get; set; } = string.Empty;
        
        public decimal MaxBudget { get; set; } // Upper limit for trip budget
        public OptimizationGoal OptimizationGoal { get; set; } = OptimizationGoal.Fastest;

        public string ExportPath { get; set; } = string.Empty;
        public bool SaveToDatabase { get; set; } = false;
        public bool EnableRecommendations { get; set; } = false;

        
        public ValidationResult Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Origin))
                errors.Add("Origin (--from) is required.");

            if (string.IsNullOrWhiteSpace(Destination))
                errors.Add("Destination (--to) is required.");

            if (!string.IsNullOrWhiteSpace(Origin) &&
                !string.IsNullOrWhiteSpace(Destination) &&
                string.Equals(Origin.Trim(), Destination.Trim(), StringComparison.OrdinalIgnoreCase))
                errors.Add("Origin and Destination must differ.");

            if (Budget <= 0)
                errors.Add("Budget (--budget) must be greater than 0.");
            else if (Budget > 1_000_000)
                errors.Add("Budget (--budget) must be 1,000,000 or less.");

            if (MaxBudget < 0)
                errors.Add("Max Budget must be 0 or greater.");
            else if (MaxBudget > 1_000_000)
                errors.Add("Max Budget must be 1,000,000 or less.");

            if (Passengers < 1)
                errors.Add("Passengers (--passengers) must be at least 1.");
            else if (Passengers > 20)
                errors.Add("Passengers (--passengers) must be 20 or fewer.");

            if (DurationDays < 1)
                errors.Add("Duration (--days) must be at least 1 day.");
            else if (DurationDays > 365)
                errors.Add("Duration (--days) must be 365 days or fewer.");

            if (StartDate.HasValue && StartDate.Value.Date < DateTime.Today)
                errors.Add($"Start date must not be in the past (got {StartDate.Value:yyyy-MM-dd}).");

            return errors.Count == 0
                ? ValidationResult.Ok()
                : ValidationResult.Fail(errors);
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append($"Travel from {Origin} to {Destination}");
            if (!string.IsNullOrWhiteSpace(Via)) sb.Append($" via {Via}");
            sb.Append($" | Budget: ${Budget:N2}");
            if (MaxBudget > 0) sb.Append($" (Max: ${MaxBudget:N2})");
            sb.Append($" | Passengers: {Passengers} | Duration: {DurationDays} day(s) | Opt Goal: {OptimizationGoal}");
            if (StartDate.HasValue) sb.Append($" | Departs: {StartDate.Value:yyyy-MM-dd}");
            if (Preferences.Count > 0) sb.Append($" | Preferences: {string.Join(", ", Preferences)}");
            return sb.ToString();
        }
    }
}
