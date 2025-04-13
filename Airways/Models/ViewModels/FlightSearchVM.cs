// FlightSearchVM.cs (update in Airways.ViewModels folder)
using Airways.Domain.EntitiesDB;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airways.ViewModels
{
    public class FlightSearchVM
    {
        [Required(ErrorMessage = "Please select a departure city")]
        [Display(Name = "Departure City")]
        public int DepartureCityId { get; set; }

        [Required(ErrorMessage = "Please select an arrival city")]
        [Display(Name = "Arrival City")]
        public int ArrivalCityId { get; set; }

        [Required(ErrorMessage = "Please select a start date")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime TravelDate { get; set; }

        

        [ValidateNever]
        public IEnumerable<City> AvailableCities { get; set; }

        [ValidateNever]
        public FlightSearchResultsVM SearchResults { get; set; }
    }
}