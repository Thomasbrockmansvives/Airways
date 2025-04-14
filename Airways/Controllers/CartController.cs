using Airways.Infrastructure;
using Airways.Services.Interfaces;
using Airways.Util.Mail.Interfaces;
using Airways.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Airways.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly ICustomerPrefService _customerPrefService;
        private readonly ICustomerProfileService _customerProfileService;
        private readonly ICityService _cityService;
        private readonly IMealService _mealService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSend _emailSend;

        public CartController(
            IBookingService bookingService,
            ICustomerPrefService customerPrefService,
            ICustomerProfileService customerProfileService,
            ICityService cityService,
            IMealService mealService,
            UserManager<IdentityUser> userManager,
            IEmailSend emailSend)
        {
            _bookingService = bookingService;
            _customerPrefService = customerPrefService;
            _customerProfileService = customerProfileService;
            _cityService = cityService;
            _mealService = mealService;
            _userManager = userManager;
            _emailSend = emailSend;
        }

        public IActionResult Index()
        {
            var cartViewModel = new CartVM();

            if (TempData["SimpleCart"] != null)
            {
                try
                {
                    // For now, we'll continue using the dynamic approach in TempData
                    // but convert to typed objects in our controller
                    var itemsJson = TempData["SimpleCart"].ToString();
                    var dynamicItems = JsonSerializer.Deserialize<List<dynamic>>(itemsJson);

                    if (dynamicItems != null)
                    {
                        foreach (var item in dynamicItems)
                        {
                            // Convert string dates/times to DateOnly/TimeOnly
                            DateOnly travelDate = DateOnly.Parse(item.GetProperty("TravelDate").GetString());
                            TimeOnly departureTime = TimeOnly.Parse(item.GetProperty("DepartureTime").GetString());
                            TimeOnly arrivalTime = TimeOnly.Parse(item.GetProperty("ArrivalTime").GetString());

                            cartViewModel.Items.Add(new CartItemVM
                            {
                                FlightId = item.GetProperty("FlightId").GetInt32(),
                                TravelDate = travelDate,
                                DepartureCity = item.GetProperty("DepartureCity").GetString(),
                                ArrivalCity = item.GetProperty("ArrivalCity").GetString(),
                                DepartureTime = departureTime,
                                ArrivalTime = arrivalTime,
                                TravelClass = item.GetProperty("TravelClass").GetString(),
                                MealId = item.GetProperty("MealId").ValueKind == JsonValueKind.Null ? null : item.GetProperty("MealId").GetInt32(),
                                TotalPrice = item.GetProperty("TotalPrice").GetDecimal()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception and continue with an empty cart
                    Console.WriteLine($"Error deserializing cart: {ex.Message}");
                }

                // Preserve TempData for the view
                TempData.Keep("SimpleCart");
            }

            return View(cartViewModel);
        }

        public IActionResult RemoveItem(int index)
        {
            if (TempData["SimpleCart"] != null)
            {
                try
                {
                    var itemsJson = TempData["SimpleCart"].ToString();
                    var dynamicItems = JsonSerializer.Deserialize<List<dynamic>>(itemsJson);

                    if (dynamicItems != null && index >= 0 && index < dynamicItems.Count)
                    {
                        dynamicItems.RemoveAt(index);

                        // Update TempData
                        TempData["SimpleCart"] = JsonSerializer.Serialize(dynamicItems);
                        TempData["CartItemCount"] = dynamicItems.Count;
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error removing item from cart: {ex.Message}");
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveAll()
        {
            // Clear cart
            TempData.Remove("SimpleCart");
            TempData["CartItemCount"] = 0;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking()
        {
            if (TempData["SimpleCart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Get the current user
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Get the user's profile
                var profile = await _customerProfileService.GetCustomerProfileByUserIdAsync(user.Id);
                if (profile == null)
                {
                    TempData["ErrorMessage"] = "User profile not found.";
                    return RedirectToAction("Index", "Home");
                }

                // Deserialize cart items
                var itemsJson = TempData["SimpleCart"].ToString();
                var dynamicItems = JsonSerializer.Deserialize<List<dynamic>>(itemsJson);

                if (dynamicItems == null || dynamicItems.Count == 0)
                {
                    return RedirectToAction("Index");
                }

                // Lists to store booking details for email
                var bookings = new List<BookingDetailsVM>();
                decimal totalAmount = 0;

                // Email content builder
                var emailContent = new StringBuilder();
                emailContent.AppendLine("<h2>Your Booking Confirmation</h2>");
                emailContent.AppendLine("<p>Dear " + profile.FirstName + ",</p>");
                emailContent.AppendLine("<p>Thank you for booking with Michael Scott Airways. Here are your booking details:</p>");
                emailContent.AppendLine("<table border='1' cellpadding='5' style='border-collapse: collapse;'>");
                emailContent.AppendLine("<tr><th>Flight</th><th>Date</th><th>From</th><th>To</th><th>Class</th><th>Seat</th><th>Meal</th><th>Price</th></tr>");

                // Process each cart item
                foreach (var item in dynamicItems)
                {
                    int flightId = item.GetProperty("FlightId").GetInt32();
                    DateOnly travelDate = DateOnly.Parse(item.GetProperty("TravelDate").GetString());
                    string travelClass = item.GetProperty("TravelClass").GetString();
                    int? mealId = item.GetProperty("MealId").ValueKind == JsonValueKind.Null ? null : item.GetProperty("MealId").GetInt32();
                    decimal totalPrice = item.GetProperty("TotalPrice").GetDecimal();
                    string departureCity = item.GetProperty("DepartureCity").GetString();
                    string arrivalCity = item.GetProperty("ArrivalCity").GetString();

                    // Create booking in the database
                    var booking = await _bookingService.CreateBookingAsync(
                        profile.ProfileId,
                        flightId,
                        travelDate,
                        totalPrice,
                        mealId,
                        travelClass
                    );

                    totalAmount += totalPrice;

                    // Get the arrival city ID to track in customer preferences
                    var arrivalCityObj = await _cityService.GetCityByNameAsync(arrivalCity);
                    if (arrivalCityObj != null)
                    {
                        // Track the visit in customer preferences
                        await _customerPrefService.TrackCityVisitAsync(profile.ProfileId, arrivalCityObj.CityId);
                    }

                    // Get meal name for email
                    string mealName = "No meal selected";
                    if (mealId.HasValue)
                    {
                        var meal = await _mealService.GetMealByIdAsync(mealId.Value);
                        if (meal != null)
                        {
                            mealName = meal.Name;
                        }
                    }

                    // Create booking details for the view model
                    var bookingDetail = new BookingDetailsVM
                    {
                        BookingId = booking.BookingId,
                        TravelDate = travelDate,
                        DepartureCity = departureCity,
                        ArrivalCity = arrivalCity,
                        DepartureTime = TimeOnly.Parse(item.GetProperty("DepartureTime").GetString()),
                        ArrivalTime = TimeOnly.Parse(item.GetProperty("ArrivalTime").GetString()),
                        TravelClass = travelClass,
                        SeatNumber = booking.SeatNumber,
                        MealName = mealName,
                        TotalPrice = totalPrice
                    };

                    bookings.Add(bookingDetail);

                    // Add to email content
                    emailContent.AppendLine($"<tr><td>{flightId}</td><td>{travelDate.ToString("yyyy-MM-dd")}</td><td>{departureCity}</td><td>{arrivalCity}</td><td>{travelClass}</td><td>{booking.SeatNumber}</td><td>{mealName}</td><td>${totalPrice.ToString("0.00")}</td></tr>");
                }

                emailContent.AppendLine("</table>");
                emailContent.AppendLine("<p>Total amount: $" + totalAmount.ToString("0.00") + "</p>");
                emailContent.AppendLine("<p>We look forward to seeing you on board!</p>");
                emailContent.AppendLine("<p>Best regards,<br>Michael Scott Airways</p>");

                // Send email to the user
                await _emailSend.SendEmailAsync(
                    profile.Email,
                    "Your Booking Confirmation - Michael Scott Airways",
                    emailContent.ToString()
                );

                // Clear the cart
                TempData.Remove("SimpleCart");
                TempData["CartItemCount"] = 0;

                // Create confirmation view model
                var confirmationViewModel = new BookingConfirmationVM
                {
                    BookingCount = bookings.Count,
                    CustomerName = $"{profile.FirstName} {profile.LastName}",
                    CustomerEmail = profile.Email,
                    TotalAmount = totalAmount
                };

                // Store it in TempData to be accessed by the confirmation page
                TempData["BookingConfirmation"] = JsonSerializer.Serialize(confirmationViewModel);

                return RedirectToAction("BookingConfirmation");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your booking: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult BookingConfirmation()
        {
            if (TempData["BookingConfirmation"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var confirmationJson = TempData["BookingConfirmation"].ToString();
                var confirmationVM = JsonSerializer.Deserialize<BookingConfirmationVM>(confirmationJson);

                return View(confirmationVM);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}