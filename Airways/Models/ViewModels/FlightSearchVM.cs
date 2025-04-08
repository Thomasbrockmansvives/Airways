using Airways.Domain.EntitiesDB;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airways.ViewModels
{
    public class FlightSearchVM : IValidatableObject
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

        [ValidateNever]
        public IEnumerable<City> AvailableCities { get; set; }

        // complex validation rules
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            
            if (DepartureCityId > 0 && DepartureCityId == ArrivalCityId)
            {
                yield return new ValidationResult(
                    "Departure and destination cities cannot be the same",
                    new[] { nameof(DepartureCityId), nameof(ArrivalCityId) });
            }

            
            if (StartDate > EndDate)
            {
                yield return new ValidationResult(
                    "Start date must be before or equal to end date of your search period",
                    new[] { nameof(StartDate), nameof(EndDate) });
            }

            
            var minDate = DateTime.Now.AddDays(2);
            var maxDate = DateTime.Now.AddMonths(6);

            if (StartDate < minDate || EndDate > maxDate)
            {
                yield return new ValidationResult(
                    "Flights must be booked 3 days to 6 months in advance",
                    new[] { nameof(StartDate), nameof(EndDate) });
            }
        }
    }
}