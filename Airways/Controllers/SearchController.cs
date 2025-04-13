using Airways.Services.Interfaces;
using Airways.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Authorize]
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

    
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    public async Task<IActionResult> Search()
    {
        var viewModel = new FlightSearchVM
        {
            TravelDate = DateTime.Now.AddDays(3),
            
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
        var travelDate = DateOnly.FromDateTime(viewmodel.TravelDate);
       

        // Create the result view model
        var resultViewModel = new FlightSearchResultsVM
        {
            Lines = connection.Lines,
            DepartureCity = connection.Departure.Name,
            ArrivalCity = connection.Arrival.Name,
            TravelDate = travelDate
            
        };

        // Fetch flights for line 1
        resultViewModel.FlightNumber1 = connection.FlightNumber1;
        resultViewModel.Line1DepartureCity = connection.FlightNumber1Navigation.Departure.Name;
        resultViewModel.Line1ArrivalCity = connection.FlightNumber1Navigation.Arrival.Name;
        resultViewModel.Line1Flight = (await _flightService.GetFlightByFlightNumberAndDateAsync(
            connection.FlightNumber1, travelDate));

        resultViewModel.Line1Flight.PriceEconomy = AdjustPriceForSeasonalRates(resultViewModel.Line1ArrivalCity, resultViewModel.Line1Flight.Date, resultViewModel.Line1Flight.PriceEconomy);
        resultViewModel.Line1Flight.PriceBusiness = AdjustPriceForSeasonalRates(resultViewModel.Line1ArrivalCity, resultViewModel.Line1Flight.Date, resultViewModel.Line1Flight.PriceBusiness);


        // If there's a second line, fetch flights for it
        if (connection.Lines >= 2 && connection.FlightNumber2.HasValue)
        {
            resultViewModel.FlightNumber2 = connection.FlightNumber2;
            resultViewModel.Line2DepartureCity = connection.FlightNumber2Navigation.Departure.Name;
            resultViewModel.Line2ArrivalCity = connection.FlightNumber2Navigation.Arrival.Name;
            resultViewModel.Line2Flight = (await _flightService.GetFlightByFlightNumberAndDateAsync(
                connection.FlightNumber2.Value, travelDate));

            resultViewModel.Line2Flight.PriceEconomy = AdjustPriceForSeasonalRates(resultViewModel.Line2ArrivalCity, resultViewModel.Line2Flight.Date, resultViewModel.Line2Flight.PriceEconomy);
            resultViewModel.Line2Flight.PriceBusiness = AdjustPriceForSeasonalRates(resultViewModel.Line2ArrivalCity, resultViewModel.Line2Flight.Date, resultViewModel.Line2Flight.PriceBusiness);
        }

        // If there's a third line, fetch flights for it
        if (connection.Lines >= 3 && connection.FlightNumber3.HasValue)
        {
            resultViewModel.FlightNumber3 = connection.FlightNumber3;
            resultViewModel.Line3DepartureCity = connection.FlightNumber3Navigation.Departure.Name;
            resultViewModel.Line3ArrivalCity = connection.FlightNumber3Navigation.Arrival.Name;
            resultViewModel.Line3Flight = (await _flightService.GetFlightByFlightNumberAndDateAsync(
                connection.FlightNumber3.Value, travelDate));

            resultViewModel.Line3Flight.PriceEconomy = AdjustPriceForSeasonalRates(resultViewModel.Line3ArrivalCity, resultViewModel.Line3Flight.Date, resultViewModel.Line3Flight.PriceEconomy);
            resultViewModel.Line3Flight.PriceBusiness = AdjustPriceForSeasonalRates(resultViewModel.Line3ArrivalCity, resultViewModel.Line3Flight.Date, resultViewModel.Line3Flight.PriceBusiness);
        }

        // Set search form data back to view model
        viewmodel.SearchResults = resultViewModel;

        return View(viewmodel);
    }

    public async Task<IActionResult> Reset()
    {
        var viewModel = new FlightSearchVM
        {
            TravelDate = DateTime.Now.AddDays(3),
            
            AvailableCities = await _cityService.GetAllCitiesAsync()
        };

        return View("Search", viewModel);
    }

    

    private decimal AdjustPriceForSeasonalRates(String arrivalCity, DateOnly travelDate, decimal basePrice)
    {
        bool isChristmasSeason = IsChristmasSeason(travelDate);
        bool isSummerSeason = IsSummerSeason(travelDate);
        bool isChristmasDestination = IsChristmasDestination(arrivalCity);
        bool isExoticDestination = IsExoticDestination(arrivalCity);

        if ((isChristmasSeason && isChristmasDestination) || (isSummerSeason && isExoticDestination))
                {
            return basePrice * 1.3m;
        }

        return basePrice;
    }

    private bool IsChristmasSeason(DateOnly date)
    {
        DateOnly christmas = new DateOnly(date.Year, 12, 25);

        DateOnly startOfSeason = christmas.AddDays(-31);
        DateOnly endOfSeason = christmas;

        return date >= startOfSeason && date <= endOfSeason;

    }

    private bool IsSummerSeason(DateOnly date) {
        return date.Month == 7 || date.Month == 8;
    }

    private bool IsChristmasDestination(String arrivalCity)
    {
        if((arrivalCity == "New York" ) || (arrivalCity == "London"))
        {
            return true;
        }

        return false;
    }

    private bool IsExoticDestination(String arrivalCity)
    {
        if ((arrivalCity == "Singapore") || (arrivalCity == "Dubai") || (arrivalCity == "Tokyo"))
        {
            return true;
        }

        return false;
    }
}