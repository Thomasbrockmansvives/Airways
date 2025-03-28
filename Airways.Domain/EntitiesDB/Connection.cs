using System;
using System.Collections.Generic;

namespace Airways.Domain.EntitiesDB;

public partial class Connection
{
    public int ConnectionId { get; set; }

    public int DepartureId { get; set; }

    public int ArrivalId { get; set; }

    public int Lines { get; set; }

    public int FlightNumber1 { get; set; }

    public int? FlightNumber2 { get; set; }

    public int? FlightNumber3 { get; set; }

    public virtual City Arrival { get; set; } = null!;

    public virtual City Departure { get; set; } = null!;

    public virtual Line FlightNumber1Navigation { get; set; } = null!;

    public virtual Line? FlightNumber2Navigation { get; set; }

    public virtual Line? FlightNumber3Navigation { get; set; }
}
