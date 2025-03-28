using System;
using System.Collections.Generic;

namespace Airways.Domain.EntitiesDB;

public partial class Meal
{
    public int MealId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool? IsLocalMeal { get; set; }

    public int? CityId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual City? City { get; set; }
}
