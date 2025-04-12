// FlightSearchResultsVM.cs (in Airways.ViewModels folder)
using Airways.Domain.EntitiesDB;
using System;
using System.Collections.Generic;

namespace Airways.ViewModels
{
    public class FlightSearchResultsVM
    {
        public int Lines { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        // For connection with 1 line
        public int? FlightNumber1 { get; set; }
        public string Line1DepartureCity { get; set; }
        public string Line1ArrivalCity { get; set; }
        public List<Flight> Line1Flights { get; set; } = new List<Flight>();

        // For connection with 2 lines
        public int? FlightNumber2 { get; set; }
        public string Line2DepartureCity { get; set; }
        public string Line2ArrivalCity { get; set; }
        public List<Flight> Line2Flights { get; set; } = new List<Flight>();

        // For connection with 3 lines
        public int? FlightNumber3 { get; set; }
        public string Line3DepartureCity { get; set; }
        public string Line3ArrivalCity { get; set; }
        public List<Flight> Line3Flights { get; set; } = new List<Flight>();
    }
}