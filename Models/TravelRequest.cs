namespace Traveler.Models
{
    public class TravelRequest
    {
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public int Passengers { get; set; } = 1;
        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Origin) && 
                   !string.IsNullOrWhiteSpace(Destination) && 
                   Budget > 0 && 
                   Passengers > 0 && 
                   DurationDays > 0;
        }

        public override string ToString()
        {
            return $"Travel from {Origin} to {Destination} | Budget: ${Budget} | Passengers: {Passengers} | Duration: {DurationDays} days";
        }
    }
}
