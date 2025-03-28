using System;
using System.Collections.Generic;

namespace Airways.Domain.EntitiesDB;

public partial class CustomerPref
{
    public int PrefId { get; set; }

    public int ProfileId { get; set; }

    public int CityId { get; set; }

    public int VisitCount { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual CustomerProfile Profile { get; set; } = null!;
}
