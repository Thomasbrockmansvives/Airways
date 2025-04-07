using Airways.Services.Interfaces;
using Airways.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class SearchController : Controller
{
    private readonly ICityService _cityService;

    public SearchController(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<IActionResult> Search()
    {
        var viewModel = new FlightSearchVM
        {
            StartDate = DateTime.Now.AddDays(3),
            EndDate = DateTime.Now.AddMonths(1),
            AvailableCities = await _cityService.GetAllCitiesAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Search(FlightSearchVM viewmodel)
    {
        // Get cities again to reset dropdown
        viewmodel.AvailableCities = await _cityService.GetAllCitiesAsync();

        // Check model state first
        if (!ModelState.IsValid)
        {
            return View(viewmodel);
        }

        // Custom validation
        if (!viewmodel.Validate())
        {
            
            if (viewmodel.DepartureCityId == viewmodel.ArrivalCityId)
            {
                ModelState.AddModelError("", "Departure and destination cities cannot be the same");
            }

            if (viewmodel.StartDate > viewmodel.EndDate)
            {
                ModelState.AddModelError("", "Start date must be before or equal to end date of your search period");
            }

            if (viewmodel.StartDate < DateTime.Now.AddDays(3) || viewmodel.EndDate > DateTime.Now.AddMonths(6))
            {
                ModelState.AddModelError("", "Flights must be booked 3 days to 6 months in advance");
            }

            return View(viewmodel);
        }

        

        return View(viewmodel);
    }
}