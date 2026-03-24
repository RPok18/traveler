namespace Traveler.Models
{
    public class CityConnection
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public double Cost { get; set; } // Could be distance, or hours.
        public decimal Price { get; set; } // Monetary cost
        public string TransportMode { get; set; } = "Flight";

        public CityConnection(string from, string to, double cost, decimal price = 0m, string mode = "Flight")
        {
            From = from;
            To = to;
            Cost = cost;
            Price = price;
            TransportMode = mode;
        }
    }
}
