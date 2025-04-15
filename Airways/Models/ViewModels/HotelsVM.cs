using System;
using System.Collections.Generic;

namespace Airways.ViewModels
{
    public class HotelsVM
    {
        public string City { get; set; }
        public DateOnly Date { get; set; }
        public List<HotelVM> Hotels { get; set; }
    }
}