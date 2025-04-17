﻿// ViewModels/HotelsVM.cs
using System.Collections.Generic;
using Airways.Models.API;

namespace Airways.ViewModels
{
    public class HotelsVM
    {
        public string City { get; set; }
        public string CheckinDate { get; set; }
        public string CheckoutDate { get; set; }
        public int TotalHotels { get; set; }
        public List<HotelResult> Hotels { get; set; }
    }
}