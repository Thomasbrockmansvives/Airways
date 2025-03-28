using System;
using System.Collections.Generic;

namespace Airways.Domain.EntitiesDB;

public partial class CustomerProfile
{
    public int ProfileId { get; set; }

    public string UserId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<CustomerPref> CustomerPrefs { get; set; } = new List<CustomerPref>();
}
