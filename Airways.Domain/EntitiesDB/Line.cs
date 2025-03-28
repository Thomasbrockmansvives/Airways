using System;
using System.Collections.Generic;

namespace Airways.Domain.EntitiesDB;

public partial class Line
{
    public int FlightNumber { get; set; }

    public int DepartureId { get; set; }

    public int ArrivalId { get; set; }

    public TimeOnly DepartureTime { get; set; }

    public TimeOnly ArrivalTime { get; set; }

    public int TotalSeatsEconomy { get; set; }

    public int TotalSeatsBusiness { get; set; }

    public virtual City Arrival { get; set; } = null!;

    public virtual ICollection<Connection> ConnectionFlightNumber1Navigations { get; set; } = new List<Connection>();

    public virtual ICollection<Connection> ConnectionFlightNumber2Navigations { get; set; } = new List<Connection>();

    public virtual ICollection<Connection> ConnectionFlightNumber3Navigations { get; set; } = new List<Connection>();

    public virtual City Departure { get; set; } = null!;

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
