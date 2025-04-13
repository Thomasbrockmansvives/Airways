
using System;

namespace Airways.ViewModels
{
    public class FlightCustomizeVM
    {
        public int FlightId { get; set; }
        public DateOnly TravelDate { get; set; }
        public decimal PriceEconomy { get; set; }
        public decimal PriceBusiness { get; set; }
        public int FlightNumber { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }
    }
}