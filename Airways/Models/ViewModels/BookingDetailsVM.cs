using System;

namespace Airways.ViewModels
{
    public class BookingDetailsVM
    {
        public int BookingId { get; set; }
        public DateOnly TravelDate { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public string TravelClass { get; set; }
        public int SeatNumber { get; set; }
        public string MealName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}