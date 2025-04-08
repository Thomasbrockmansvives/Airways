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

        
        if (!ModelState.IsValid)
        {
            return View(viewmodel);
        }

        // Proceed with search logic if validation passes
        // ...

        return View(viewmodel);
    }

    
    public async Task<IActionResult> Reset()
    {
        
        var viewModel = new FlightSearchVM
        {
            StartDate = DateTime.Now.AddDays(3),
            EndDate = DateTime.Now.AddMonths(1)
        };

        
        return View("Search", viewModel);
    }
}