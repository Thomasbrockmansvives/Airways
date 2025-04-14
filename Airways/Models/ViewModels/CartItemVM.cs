public class CartItemVM
{
    public int FlightId { get; set; }
    public DateOnly TravelDate { get; set; }
    public string TravelClass { get; set; }
    public int? MealId { get; set; }
    public decimal TotalPrice { get; set; }
    public string DepartureCity { get; set; }
    public string ArrivalCity { get; set; }
    public TimeOnly DepartureTime { get; set; }
    public TimeOnly ArrivalTime { get; set; }
}