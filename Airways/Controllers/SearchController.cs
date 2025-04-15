using Airways.Services.Interfaces;
using Airways.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;


/*
 * this controller is (together with the cartController) my most difficult piece.
 * because i have been struggling a lot with it and used the help of chatgpt/copilot,
 * i use some commenting for myself to understand the code better
 * especially for the use of tempdata and jsonserialiser and invariant culture
 */

[Authorize] // user must be logged in
public class SearchController : Controller
{
    private readonly ICityService _cityService;
    private readonly IConnectionService _connectionService;
    private readonly IFlightService _flightService;
    private readonly IMealService _mealService;

    public SearchController(
        ICityService cityService,
        IConnectionService connectionService,
        IFlightService flightService,
        IMealService mealService)
    {
        _cityService = cityService;
        _connectionService = connectionService;
        _flightService = flightService;
        _mealService = mealService;
    }

    
    [AllowAnonymous] // can of course be accessed when not logged in
    public IActionResult AccessDenied()
    {
        return View();
    }

    public async Task<IActionResult> Search()
    {
        // viewmodel for search form

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
        // Get cities to reset dropdown

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

        // Get flights for line 1
        resultViewModel.FlightNumber1 = connection.FlightNumber1;
        resultViewModel.Line1DepartureCity = connection.FlightNumber1Navigation.Departure.Name;
        resultViewModel.Line1ArrivalCity = connection.FlightNumber1Navigation.Arrival.Name;
        resultViewModel.Line1Flight = (await _flightService.GetFlightByFlightNumberAndDateAsync(
            connection.FlightNumber1, travelDate));

        resultViewModel.Line1Flight.PriceEconomy = AdjustPriceForSeasonalRates(resultViewModel.Line1ArrivalCity, resultViewModel.Line1Flight.Date, resultViewModel.Line1Flight.PriceEconomy);
        resultViewModel.Line1Flight.PriceBusiness = AdjustPriceForSeasonalRates(resultViewModel.Line1ArrivalCity, resultViewModel.Line1Flight.Date, resultViewModel.Line1Flight.PriceBusiness);


        // If there's a second line, get flights for it
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

        // If there's a third line, get flights for it
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

    [HttpGet]
    public async Task<IActionResult> CustomizeFlight(int flightId, string travelDate, string priceEconomy, string priceBusiness, int flightNumber)
    {
        // date from string format
        DateOnly parsedDate;
        if (!DateOnly.TryParse(travelDate, out parsedDate))
        {
            return BadRequest("Invalid date format");
        }

        /* decimal values using invariant culture
         * to handle decimal values when formatting currency
         */
        decimal parsedPriceEconomy;
        decimal parsedPriceBusiness;

        if (!decimal.TryParse(priceEconomy, System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out parsedPriceEconomy) ||
            !decimal.TryParse(priceBusiness, System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out parsedPriceBusiness))
        {
            // If parsing fails, fetch the flight and recalculate prices
            var flight = await _flightService.GetFlightByIdAsync(flightId);
            if (flight == null)
            {
                return NotFound();
            }

            parsedPriceEconomy = AdjustPriceForSeasonalRates(flight.FlightNumberNavigation.Arrival.Name, parsedDate, flight.PriceEconomy);
            parsedPriceBusiness = AdjustPriceForSeasonalRates(flight.FlightNumberNavigation.Arrival.Name, parsedDate, flight.PriceBusiness);
        }

        // Get the flight details
        var flightDetails = await _flightService.GetFlightByIdAsync(flightId);
        if (flightDetails == null)
        {
            return NotFound();
        }

        // seats availability

        var availableEconomySeats = await _flightService.IsEconomyAvailableByFlightAsync(flightId);
        var availableBusinessSeats = await _flightService.IsBusinessAvailableByFlightAsync(flightId);

        var arrivalCity = flightDetails.FlightNumberNavigation.Arrival.CityId;
        var meals = await _mealService.GetMealsByCityIdAsync(arrivalCity);




        // Create the view model
        var viewModel = new FlightCustomizeVM
        {
            FlightId = flightId,
            TravelDate = parsedDate,
            PriceEconomy = parsedPriceEconomy,
            PriceBusiness = parsedPriceBusiness,
            FlightNumber = flightNumber,
            DepartureCity = flightDetails.FlightNumberNavigation.Departure.Name,
            ArrivalCity = flightDetails.FlightNumberNavigation.Arrival.Name,
            DepartureTime = flightDetails.FlightNumberNavigation.DepartureTime,
            ArrivalTime = flightDetails.FlightNumberNavigation.ArrivalTime,
            HasAvailableEconomySeats = availableEconomySeats,
            HasAvailableBusinessSeats = availableBusinessSeats,
            Meals = meals
        };

        return PartialView("_FlightCustomize", viewModel);
    }


    [HttpPost]
    public IActionResult AddToCartSimple(int flightId, string travelDate, string departureCity, string arrivalCity,
    string departureTime, string arrivalTime, string travelClass, int? mealId, decimal economyPrice, decimal businessPrice)
    {
        // price calculation
        decimal classPrice = travelClass == "Economy" ? economyPrice : businessPrice;
        decimal mealPrice = mealId.HasValue ? 35.00m : 0;
        decimal totalPrice = classPrice + mealPrice;

        // Get meal if applicable
        string mealName = "";
        if (mealId.HasValue)
        {
            var meal = _mealService.GetMealByIdAsync(mealId.Value).Result;
            mealName = meal?.Name ?? "";
        }

        // Save in viewbag to be used in confirmation view

        ViewBag.FlightId = flightId;
        ViewBag.TravelDate = travelDate;
        ViewBag.DepartureCity = departureCity;
        ViewBag.ArrivalCity = arrivalCity;
        ViewBag.DepartureTime = departureTime;
        ViewBag.ArrivalTime = arrivalTime;
        ViewBag.TravelClass = travelClass;
        ViewBag.MealId = mealId;
        ViewBag.MealName = mealName;
        ViewBag.TotalPrice = totalPrice;
        ViewBag.ClassPrice = classPrice;

        List<dynamic> cartItems;

        // create a new shopping cart or use existing one

        if (TempData["SimpleCart"] != null) 
        {
            cartItems = JsonSerializer.Deserialize<List<dynamic>>(TempData["SimpleCart"].ToString());
        }
        else
        {
            cartItems = new List<dynamic>();
        }

        // add to cart
        cartItems.Add(new
        {
            FlightId = flightId,
            TravelDate = travelDate,
            DepartureCity = departureCity,
            ArrivalCity = arrivalCity,
            DepartureTime = departureTime,
            ArrivalTime = arrivalTime,
            TravelClass = travelClass,
            MealId = mealId,
            MealName = mealName,
            TotalPrice = totalPrice,
            ClassPrice = classPrice
        });

        // Update TempData
        TempData["SimpleCart"] = JsonSerializer.Serialize(cartItems);
        TempData["CartItemCount"] = cartItems.Count;

        // show confirmation
        return PartialView("_AddToCartConfirmation");
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