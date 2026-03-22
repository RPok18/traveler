using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traveler.Models
{
    public class Itinerary
    {
        public List<CityConnection> Segments { get; set; } = new List<CityConnection>();
        public double TotalCost => Segments.Sum(s => s.Cost);
        public bool Found => Segments.Count > 0;

        public string Origin => Segments.FirstOrDefault()?.From ?? string.Empty;
        public string Destination => Segments.LastOrDefault()?.To ?? string.Empty;

        public override string ToString()
        {
            if (!Found) return "No path found.";

            var sb = new StringBuilder();
            sb.AppendLine($"--- Itinerary Found (${TotalCost:N1} total units) ---");
            foreach (var segment in Segments)
            {
                sb.AppendLine($"  {segment.From} --[{segment.TransportMode}]--> {segment.To} ({segment.Cost:N1})");
            }
            return sb.ToString();
        }
    }
}
