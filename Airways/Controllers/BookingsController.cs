using Airways.Domain.EntitiesDB;
using Airways.Models.API;
using Airways.Util.Api;
using Airways.Services.Interfaces;
using Airways.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

/*
 * this controller is (together with the searchController and cartcontroller) my most difficult piece.
 * because i have been struggling a lot with it and used the help of chatgpt/copilot,
 * i use some commenting for myself to understand the code better
 * especially for the use of tempdata and jsonserialiser
 * Api knowledge
 */

namespace Airways.Controllers
{
    [Authorize] // Ensures only logged-in users can access
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICustomerProfileService _customerProfileService;
        private readonly ICityService _cityService;
        private readonly BookingApiSettings _apiSettings;


        public BookingsController(
            IBookingService bookingService,
            UserManager<IdentityUser> userManager,
            ICustomerProfileService customerProfileService,
            ICityService cityService,
            BookingApiSettings apiSettings)
        {
            _bookingService = bookingService;
            _userManager = userManager;
            _customerProfileService = customerProfileService;
            _cityService = cityService;
            _apiSettings = apiSettings;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                
                var profile = await _customerProfileService.GetCustomerProfileByUserIdAsync(user.Id);
                if (profile == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                
                var bookings = await _bookingService.GetBookingsByCustomerIdAsync(profile.ProfileId);
                

                var bookingViewModels = MapBookingsToViewModel(bookings);
                ViewData["ProfileId"] = profile.ProfileId;

                return View(bookingViewModels);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                return View(new List<BookingVM>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var profile = await _customerProfileService.GetCustomerProfileByUserIdAsync(user.Id);
            if (profile == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var booking = await _bookingService.GetBookingByIdAsync(bookingId);

            // Verify this booking belongs to the current user
            if (booking == null || booking.CustomerId != profile.ProfileId)
            {
                TempData["ErrorMessage"] = "Booking not found or you don't have permission to cancel it.";
                return RedirectToAction("Index");
            }

            // Check if the flight date is at least 7 days in the future
            var flightDate = booking.Flight.Date;
            var today = DateOnly.FromDateTime(DateTime.Now);
            var daysDifference = (flightDate.ToDateTime(new TimeOnly(0, 0)) - DateTime.Now).TotalDays;

            if (daysDifference < 7)
            {
                TempData["ErrorMessage"] = "Bookings can only be cancelled at least 7 days before the flight.";
                return RedirectToAction("Index");
            }

            // Cancel the booking by setting seatNumber to 0
            booking.SeatNumber = 0;
            var result = await _bookingService.UpdateBookingAsync(booking.BookingId);

            if (result)
            {
                TempData["SuccessMessage"] = "Your booking has been successfully cancelled.";
            }
            else
            {
                TempData["ErrorMessage"] = "There was an error cancelling your booking. Please try again.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> LookForHotels(string city, string date)
        {
            try
            {
                // Parse the date string
                DateOnly flightDate;
                if (!DateOnly.TryParse(date, out flightDate))
                {
                    return PartialView("_Error", "Invalid date format");
                }

                // Get the destination ID for the city
                var destId = await _cityService.GetDestIdByCityNameAsync(city);
                if (!destId.HasValue)
                {
                    return PartialView("_Error", $"Unable to find destination ID for {city}");
                }

                // Format destId
                string formattedDestId = ((int)destId.Value).ToString();

                // Calculate checkout date (3 days after check-in)
                var checkinDate = flightDate.ToString("yyyy-MM-dd");
                var checkoutDate = flightDate.AddDays(3).ToString("yyyy-MM-dd"); // Changed to 3 days

                // Call booking.com API
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://booking-com.p.rapidapi.com/v1/hotels/search?filter_by_currency=EUR&checkout_date={checkoutDate}&checkin_date={checkinDate}&adults_number=1&units=metric&dest_id={formattedDestId}&locale=en-gb&dest_type=city&order_by=popularity&room_number=1"),
                    Headers =
            {
                { "x-rapidapi-key", _apiSettings.ApiKey },
                { "x-rapidapi-host", _apiSettings.ApiHost },
            },
                };

                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    // Manual extraction approach to bypass the deserialization issues
                    try
                    {
                        // Use JsonDocument for low-level access to the JSON
                        using (JsonDocument doc = JsonDocument.Parse(body))
                        {
                            var root = doc.RootElement;

                            // Get the total hotels count
                            int totalHotels = 0;
                            if (root.TryGetProperty("primary_count", out var countElement))
                            {
                                totalHotels = countElement.GetInt32();
                            }

                            // Get the results array
                            if (!root.TryGetProperty("result", out var resultsElement) || resultsElement.ValueKind != JsonValueKind.Array)
                            {
                                return PartialView("_Error", "No hotels found in response");
                            }

                            var hotelsList = new List<HotelResult>();

                            // Get the first 3 hotels
                            int count = 0;
                            foreach (var hotelElement in resultsElement.EnumerateArray())
                            {
                                if (count >= 3) break;

                                // Try hotel_name_trans first, fall back to hotel_name if needed
                                string hotelName = GetStringProperty(hotelElement, "hotel_name_trans");
                                if (string.IsNullOrEmpty(hotelName))
                                {
                                    hotelName = GetStringProperty(hotelElement, "hotel_name");
                                }

                                var hotel = new HotelResult
                                {
                                    HotelName = hotelName,
                                    ReviewScoreWord = GetStringProperty(hotelElement, "review_score_word"),
                                    MainPhotoUrl = GetStringProperty(hotelElement, "main_photo_url"),
                                    // Try to get max_photo_url for higher quality images
                                    MaxPhotoUrl = GetStringProperty(hotelElement, "max_photo_url"),
                                    Url = GetStringProperty(hotelElement, "url"),
                                    PriceBreakdown = new PriceBreakdown
                                    {
                                        PriceString = GetPriceString(hotelElement),
                                        Currency = "EUR"
                                    }
                                };

                                hotelsList.Add(hotel);
                                count++;
                            }

                            string currencySymbol = GetCurrencySymbolForCity(city);

                            // Create view model
                            var viewModel = new HotelsVM
                            {
                                City = city,
                                CheckinDate = checkinDate,
                                CheckoutDate = checkoutDate,
                                TotalHotels = totalHotels,
                                Hotels = hotelsList,
                                CurrencySymbol = currencySymbol
                            };

                            return PartialView("_HotelResults", viewModel);
                        }
                    }
                    catch (Exception jsonEx)
                    {
                        
                        // Return the debug view with more context
                        return PartialView("~/Views/Shared/_HotelDebug", new Dictionary<string, string>
                {
                    { "City", city },
                    { "CheckinDate", checkinDate },
                    { "CheckoutDate", checkoutDate },
                    { "ApiResponsePreview", body },
                    { "ErrorMessage", jsonEx.Message }
                });
                    }
                }
            }
            catch (Exception ex)
            {
                return PartialView("_Error", $"Unable to find hotels: {ex.Message}");
            }
        }

        // Helper method to safely get string properties from JsonElement
        private string GetStringProperty(JsonElement element, string propertyPath, string defaultValue = "")
        {
            string[] parts = propertyPath.Split('.');
            JsonElement current = element;

            foreach (var part in parts)
            {
                if (!current.TryGetProperty(part, out current))
                    return defaultValue;
            }

            return current.ValueKind == JsonValueKind.String ? current.GetString() : defaultValue;
        }

        // Helper method to get price as a string regardless of format
        private string GetPriceString(JsonElement hotelElement)
        {
            if (hotelElement.TryGetProperty("price_breakdown", out var priceBreakdown))
            {
                if (priceBreakdown.TryGetProperty("gross_price", out var grossPrice))
                {
                    switch (grossPrice.ValueKind)
                    {
                        case JsonValueKind.String:
                            return grossPrice.GetString();
                        case JsonValueKind.Number:
                            return grossPrice.GetDecimal().ToString("0.00");
                        default:
                            break;
                    }
                }

                // Alternative price properties
                if (priceBreakdown.TryGetProperty("all_inclusive_price", out var allInclusivePrice))
                {
                    return allInclusivePrice.GetDecimal().ToString("0.00");
                }
            }

            return "0.00";
        }

        private List<BookingVM> MapBookingsToViewModel(IEnumerable<Booking> bookings)
        {
            var viewModels = new List<BookingVM>();

            if (bookings == null || !bookings.Any())
            {
                return viewModels;
            }

            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);

            foreach (var booking in bookings)
            {
                
                    // Determine booking status based on seat number and date
                    string status;
                    bool canCancel = false;
                    bool isInFuture = booking.Flight.Date > today;

                    if (booking.SeatNumber == 0)
                    {
                        status = "Cancelled";
                    }
                    else if (booking.Flight.Date < today)
                    {
                        status = "Done";
                    }
                    else if (booking.Flight.Date == today)
                    {
                        // Further refine status based on time of day if needed
                        TimeOnly currentTime = TimeOnly.FromDateTime(now);
                        TimeOnly departureTime = booking.Flight.FlightNumberNavigation.DepartureTime;

                        if (currentTime > departureTime.AddHours(1))
                        {
                            status = "Flying";
                        }
                        else if (currentTime > departureTime.AddMinutes(-30))
                        {
                            status = "Boarding";
                        }
                        else if (currentTime > departureTime.AddHours(-2))
                        {
                            status = "Checking In";
                        }
                        else
                        {
                            status = "Booked";
                            // Can cancel if more than 7 days until flight
                            canCancel = (booking.Flight.Date.ToDateTime(new TimeOnly(0, 0)) - now).TotalDays >= 7;
                        }
                    }
                    else
                    {
                        status = "Booked";
                        // Can cancel if more than 7 days until flight
                        canCancel = (booking.Flight.Date.ToDateTime(new TimeOnly(0, 0)) - now).TotalDays >= 7;
                    }

                    // Get meal name
                    string mealName = booking.Meal?.Name ?? "No meal";

                    viewModels.Add(new BookingVM
                    {
                        BookingId = booking.BookingId,
                        BookingDate = booking.BookingDate,
                        FlightId = booking.FlightId,
                        FlightDate = booking.Flight.Date,
                        MealName = mealName,
                        Class = booking.Class,
                        SeatNumber = booking.SeatNumber,
                        TotalPrice = booking.TotalPrice,
                        Status = status,
                        CanCancel = canCancel,
                        IsInFuture = isInFuture,
                        DepartureCity = booking.Flight.FlightNumberNavigation.Departure.Name,
                        ArrivalCity = booking.Flight.FlightNumberNavigation.Arrival.Name,
                        DepartureTime = booking.Flight.FlightNumberNavigation.DepartureTime,
                        ArrivalTime = booking.Flight.FlightNumberNavigation.ArrivalTime
                    });
                
            }

            // Sort bookings by flight date and then by departure time
            return viewModels
                .OrderBy(b => b.FlightDate)
                .ThenBy(b => b.DepartureTime)
                .ToList();
        }

        // Add this method to the BookingsController class
        private string GetCurrencySymbolForCity(string city)
        {
            // Default to Euro
            string currencySymbol = "€";

            // Map cities to their currency symbols
            switch (city.ToLower())
            {
                case "new york":
                    currencySymbol = "USD";
                    break;
                case "london":
                    currencySymbol = "GBP";
                    break;
                case "tokyo":
                    currencySymbol = "JPY";
                    break;
                case "dubai":
                    currencySymbol = "AED";
                    break;
                case "singapore":
                    currencySymbol = "SGD";
                    break;
                case "sydney":
                    currencySymbol = "AUD";
                    break;
                case "cape town":
                    currencySymbol = "ZAR";
                    break;
                default:
                    currencySymbol = "EUR";
                    break;
            }

            return currencySymbol;
        }
    }
}