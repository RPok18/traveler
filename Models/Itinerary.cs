using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traveler.Models
{
    public class Itinerary
    {
        public List<CityConnection> Segments { get; set; } = new List<CityConnection>();
        public double TotalCost => Segments.Sum(s => s.Cost);
        public decimal TotalPrice => Segments.Sum(s => s.Price);
        public bool Found => Segments.Count > 0;
        public List<string> Recommendations { get; set; } = new List<string>();

        public string Origin => Segments.FirstOrDefault()?.From ?? string.Empty;
        public string Destination => Segments.LastOrDefault()?.To ?? string.Empty;

        public override string ToString()
        {
            if (!Found) return "No path found.";

            var sb = new StringBuilder();
            sb.AppendLine($"--- Itinerary Found ({TotalCost:N1} hours, ${TotalPrice:N2}) ---");
            foreach (var segment in Segments)
            {
                sb.AppendLine($"  {segment.From} --[{segment.TransportMode}]--> {segment.To} ({segment.Cost:N1} hrs, ${segment.Price:N2})");
            }
            
            if (Recommendations.Any())
            {
                sb.AppendLine();
                sb.AppendLine("--- AI Recommendations ---");
                foreach (var rec in Recommendations)
                {
                    sb.AppendLine($"  {rec}");
                }
            }
            
            return sb.ToString();
        }
    }
}
