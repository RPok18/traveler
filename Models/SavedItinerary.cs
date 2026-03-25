using System;

namespace Traveler.Models
{
    public class SavedItinerary
    {
        public int Id { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string SavedDate { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public double TotalCost { get; set; }
        public string SegmentsJson { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"[{Id}] {SavedDate}: {Origin} to {Destination} | Price: ${TotalPrice:N2} | Duration: {TotalCost:N1} hrs";
        }
    }
}
