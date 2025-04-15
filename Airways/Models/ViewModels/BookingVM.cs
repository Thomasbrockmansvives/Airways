using System;

namespace Airways.ViewModels
{
    public class BookingVM
    {
        public int BookingId { get; set; }
        public DateOnly BookingDate { get; set; }
        public int FlightId { get; set; }
        public DateOnly FlightDate { get; set; }
        public string MealName { get; set; }
        public string Class { get; set; }
        public int SeatNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public bool CanCancel { get; set; }
        public bool IsInFuture { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }
    }
}