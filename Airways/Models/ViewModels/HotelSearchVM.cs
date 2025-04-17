namespace Airways.ViewModels
{
    public class HotelSearchVM
    {
        public string ArrivalCity { get; set; }
        public decimal? DestId { get; set; }
        public string DestIdType { get; set; }
        public string CheckinDate { get; set; }
        public string CheckoutDate { get; set; }

        // Fixed values
        public string Currency { get; set; } = "EUR";
        public int AdultsNumber { get; set; } = 1;
        public string Units { get; set; } = "metric";
        public string Locale { get; set; } = "en-gb";
        public string DestType { get; set; } = "city";
        public string OrderBy { get; set; } = "popularity";
        public int RoomNumber { get; set; } = 1;
    }
}