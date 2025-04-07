using Airways.Domain.EntitiesDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airways.ViewModels
{
    public class FlightSearchVM
    {
        [Required(ErrorMessage = "Please select a departure city")]
        public int DepartureCityId { get; set; }

        [Required(ErrorMessage = "Please select a destination city")]
        public int ArrivalCityId { get; set; }

        [Required(ErrorMessage = "Please select a start date")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please select an end date")]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public IEnumerable<City> AvailableCities { get; set; }

        
        public bool Validate()
        {
            // Check if cities are the same
            if (DepartureCityId > 0 && DepartureCityId == ArrivalCityId)
            {
                return false;
            }

            // Check date range
            if (StartDate > EndDate)
            {
                return false;
            }

            // Check date range constraints (3 days to 6 months in advance)
            var minDate = DateTime.Now.AddDays(3);
            var maxDate = DateTime.Now.AddMonths(6);

            if (StartDate < minDate || EndDate > maxDate)
            {
                return false;
            }

            return true;
        }
    }
}