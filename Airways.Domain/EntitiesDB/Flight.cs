using System;
using System.Collections.Generic;

namespace Airways.Domain.EntitiesDB;

public partial class Flight
{
    public int FlightNumber { get; set; }

    public DateOnly Date { get; set; }

    public int UsedSeatsEconomy { get; set; }

    public int UsedSeatsBusiness { get; set; }

    public decimal PriceEconomy { get; set; }

    public decimal PriceBusiness { get; set; }

    public int FlightId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Line FlightNumberNavigation { get; set; } = null!;
}
