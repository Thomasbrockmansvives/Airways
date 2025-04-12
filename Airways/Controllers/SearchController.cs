using Airways.Services.Interfaces;
using Airways.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

public class SearchController : Controller
{
    private readonly ICityService _cityService;
    private readonly IConnectionService _connectionService;
    private readonly IFlightService _flightService;

    public SearchController(
        ICityService cityService,
        IConnectionService connectionService,
        IFlightService flightService)
    {
        _cityService = cityService;
        _connectionService = connectionService;
        _flightService = flightService;
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

        // Find the connection between the departure and arrival cities
        var connection = await _connectionService.GetConnectionByCitiesAsync(viewmodel.DepartureCityId, viewmodel.ArrivalCityId);

        if (connection == null)
        {
            ModelState.AddModelError("", "No connection found between these cities.");
            return View(viewmodel);
        }

        // Convert DateTime to DateOnly
        var startDate = DateOnly.FromDateTime(viewmodel.StartDate);
        var endDate = DateOnly.FromDateTime(viewmodel.EndDate);

        // Create the result view model
        var resultViewModel = new FlightSearchResultsVM
        {
            Lines = connection.Lines,
            DepartureCity = connection.Departure.Name,
            ArrivalCity = connection.Arrival.Name,
            StartDate = startDate,
            EndDate = endDate
        };

        // Fetch flights for line 1
        resultViewModel.FlightNumber1 = connection.FlightNumber1;
        resultViewModel.Line1DepartureCity = connection.FlightNumber1Navigation.Departure.Name;
        resultViewModel.Line1ArrivalCity = connection.FlightNumber1Navigation.Arrival.Name;
        resultViewModel.Line1Flights = (await _flightService.GetFlightsByFlightNumberAndDateRangeAsync(
            connection.FlightNumber1, startDate, endDate)).ToList();

        // If there's a second line, fetch flights for it
        if (connection.Lines >= 2 && connection.FlightNumber2.HasValue)
        {
            resultViewModel.FlightNumber2 = connection.FlightNumber2;
            resultViewModel.Line2DepartureCity = connection.FlightNumber2Navigation.Departure.Name;
            resultViewModel.Line2ArrivalCity = connection.FlightNumber2Navigation.Arrival.Name;
            resultViewModel.Line2Flights = (await _flightService.GetFlightsByFlightNumberAndDateRangeAsync(
                connection.FlightNumber2.Value, startDate, endDate)).ToList();
        }

        // If there's a third line, fetch flights for it
        if (connection.Lines >= 3 && connection.FlightNumber3.HasValue)
        {
            resultViewModel.FlightNumber3 = connection.FlightNumber3;
            resultViewModel.Line3DepartureCity = connection.FlightNumber3Navigation.Departure.Name;
            resultViewModel.Line3ArrivalCity = connection.FlightNumber3Navigation.Arrival.Name;
            resultViewModel.Line3Flights = (await _flightService.GetFlightsByFlightNumberAndDateRangeAsync(
                connection.FlightNumber3.Value, startDate, endDate)).ToList();
        }

        // Set search form data back to view model
        viewmodel.SearchResults = resultViewModel;

        return View(viewmodel);
    }

    public async Task<IActionResult> Reset()
    {
        var viewModel = new FlightSearchVM
        {
            StartDate = DateTime.Now.AddDays(3),
            EndDate = DateTime.Now.AddMonths(1),
            AvailableCities = await _cityService.GetAllCitiesAsync()
        };

        return View("Search", viewModel);
    }

    public async Task<IActionResult> GetSearchResults(int departureCityId, int arrivalCityId, DateTime startDate, DateTime endDate)
    {
        
        var connection = await _connectionService.GetConnectionByCitiesAsync(departureCityId, arrivalCityId);

        if (connection == null)
        {
            return PartialView("_NoResultsPartial");
        }

        var startDateOnly = DateOnly.FromDateTime(startDate);
        var endDateOnly = DateOnly.FromDateTime(endDate);

        
        var resultViewModel = new FlightSearchResultsVM
        {
            Lines = connection.Lines,
            DepartureCity = connection.Departure.Name,
            ArrivalCity = connection.Arrival.Name,
            StartDate = startDateOnly,
            EndDate = endDateOnly
        };

        // Get flights for line 1
        resultViewModel.FlightNumber1 = connection.FlightNumber1;
        resultViewModel.Line1DepartureCity = connection.FlightNumber1Navigation.Departure.Name;
        resultViewModel.Line1ArrivalCity = connection.FlightNumber1Navigation.Arrival.Name;
        resultViewModel.Line1Flights = (await _flightService.GetFlightsByFlightNumberAndDateRangeAsync(
            connection.FlightNumber1, startDateOnly, endDateOnly)).ToList();

        // If there's a second line, get flights for it
        if (connection.Lines >= 2 && connection.FlightNumber2.HasValue)
        {
            resultViewModel.FlightNumber2 = connection.FlightNumber2;
            resultViewModel.Line2DepartureCity = connection.FlightNumber2Navigation.Departure.Name;
            resultViewModel.Line2ArrivalCity = connection.FlightNumber2Navigation.Arrival.Name;
            resultViewModel.Line2Flights = (await _flightService.GetFlightsByFlightNumberAndDateRangeAsync(
                connection.FlightNumber2.Value, startDateOnly, endDateOnly)).ToList();
        }

        // If there's a third line, get flights for it
        if (connection.Lines >= 3 && connection.FlightNumber3.HasValue)
        {
            resultViewModel.FlightNumber3 = connection.FlightNumber3;
            resultViewModel.Line3DepartureCity = connection.FlightNumber3Navigation.Departure.Name;
            resultViewModel.Line3ArrivalCity = connection.FlightNumber3Navigation.Arrival.Name;
            resultViewModel.Line3Flights = (await _flightService.GetFlightsByFlightNumberAndDateRangeAsync(
                connection.FlightNumber3.Value, startDateOnly, endDateOnly)).ToList();
        }

        return PartialView("_SearchResultsPartial", resultViewModel);
    }
}