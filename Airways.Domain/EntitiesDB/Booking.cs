using System;
using System.Collections.Generic;

namespace Airways.Domain.EntitiesDB;

public partial class Booking
{
    public int BookingId { get; set; }

    public int CustomerId { get; set; }

    public int FlightId { get; set; }

    public DateOnly BookingDate { get; set; }

    public decimal TotalPrice { get; set; }

    public int? MealId { get; set; }

    public string Class { get; set; } = null!;

    public int SeatNumber { get; set; }

    public virtual CustomerProfile Customer { get; set; } = null!;

    public virtual Flight Flight { get; set; } = null!;

    public virtual Meal? Meal { get; set; }
}
