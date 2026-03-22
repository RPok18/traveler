namespace Traveler.Models
{
    public class CityConnection
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public double Cost { get; set; } // Could be distance, price, or hours.
        public string TransportMode { get; set; } = "Flight";

        public CityConnection(string from, string to, double cost, string mode = "Flight")
        {
            From = from;
            To = to;
            Cost = cost;
            TransportMode = mode;
        }
    }
}
