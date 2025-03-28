using System;
using System.Collections.Generic;

namespace Airways.Domain.EntitiesDB;

public partial class City
{
    public int CityId { get; set; }

    public string Name { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual ICollection<Connection> ConnectionArrivals { get; set; } = new List<Connection>();

    public virtual ICollection<Connection> ConnectionDepartures { get; set; } = new List<Connection>();

    public virtual ICollection<CustomerPref> CustomerPrefs { get; set; } = new List<CustomerPref>();

    public virtual ICollection<Line> LineArrivals { get; set; } = new List<Line>();

    public virtual ICollection<Line> LineDepartures { get; set; } = new List<Line>();

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
