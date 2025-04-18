using Airways.Services.Interfaces;
using Airways.Util.Mail.Interfaces;
using Airways.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

/*
 * this controller is (together with the searchController and bookingController) my most difficult piece.
 * because i have been struggling a lot with it and used the help of chatgpt/copilot,
 * i use some commenting for myself to understand the code better
 * especially for the use of tempdata and jsonserialiser
 * TempData: temporary storage mechanism which keeps data for one subsequent request
 * versus ViewBag: only lasts for current request, does not survive redirects
 * Json Serialiser, converting between json string and objects
 */

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
            var cartVM = new CartVM(); 

            if (TempData["SimpleCart"] != null) // is there data in TempData?
            {
                try
                {
                    // get the existing data (json string)                    
                    var itemsJson = TempData["SimpleCart"].ToString();
                    // convert to list of dynamic objects
                    var dynamicItems = JsonSerializer.Deserialize<List<dynamic>>(itemsJson);

                    if (dynamicItems != null)
                    {
                        // convert each item into a cartItemVM and add to cartVM
                        foreach (var item in dynamicItems)
                        {
                            
                            DateOnly travelDate = DateOnly.Parse(item.GetProperty("TravelDate").GetString());
                            TimeOnly departureTime = TimeOnly.Parse(item.GetProperty("DepartureTime").GetString());
                            TimeOnly arrivalTime = TimeOnly.Parse(item.GetProperty("ArrivalTime").GetString());

                            cartVM.Items.Add(new CartItemVM
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
                    
                    Console.WriteLine($"Error deserializing cart: {ex.Message}");
                }

                
                TempData.Keep("SimpleCart");
            }

            return View(cartVM);
        }

        public IActionResult RemoveItem(int index)
        {
            if (TempData["SimpleCart"] != null) // is there data in TempData
            {
                try
                {
                    var itemsJson = TempData["SimpleCart"].ToString(); // get json string
                    var dynamicItems = JsonSerializer.Deserialize<List<dynamic>>(itemsJson); // convert into objects-list

                    if (dynamicItems != null && index >= 0 && index < dynamicItems.Count) // check whether the index to be removed exists
                    {
                        dynamicItems.RemoveAt(index); // remove

                        
                        TempData["SimpleCart"] = JsonSerializer.Serialize(dynamicItems); // turn back into json and put in tempdata
                        TempData["CartItemCount"] = dynamicItems.Count;
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Error removing item from cart: {ex.Message}");
                }
            }

            return RedirectToAction("Index"); // back to this page but with data updated (removed)
        }

        public IActionResult RemoveAll()
        {
            // simple remove without converting json into list of objects
            TempData.Remove("SimpleCart");
            TempData["CartItemCount"] = 0;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBooking()
        {
            if (TempData["SimpleCart"] == null) // if the basket is empty, go back to home screen
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                
                var user = await _userManager.GetUserAsync(User); // check for login
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                
                var profile = await _customerProfileService.GetCustomerProfileByUserIdAsync(user.Id); // check for customer profile
                if (profile == null)
                {
                    TempData["ErrorMessage"] = "User profile not found.";
                    return RedirectToAction("Index", "Home");
                }

                
                var itemsJson = TempData["SimpleCart"].ToString(); // get json and convert to list of objects
                var dynamicItems = JsonSerializer.Deserialize<List<dynamic>>(itemsJson);

                if (dynamicItems == null || dynamicItems.Count == 0) // if list is empty
                {
                    return RedirectToAction("Index");
                }

                // create a list of booking ViewModels

                var bookings = new List<BookingDetailsVM>();
                decimal totalAmount = 0;

                // create email setup

                var emailContent = new StringBuilder();
                emailContent.AppendLine("<h2>Your Booking Confirmation</h2>");
                emailContent.AppendLine("<p>Dear " + profile.FirstName + ",</p>");
                emailContent.AppendLine("<p>Thank you for booking with Michael Scott Airways. Here are your booking details:</p>");
                emailContent.AppendLine("<table border='1' cellpadding='5' style='border-collapse: collapse;'>");
                emailContent.AppendLine("<tr><th>Flight</th><th>Date</th><th>From</th><th>To</th><th>Class</th><th>Seat</th><th>Meal</th><th>Price</th></tr>");

                // prepare to convert to viewmodels

                foreach (var item in dynamicItems)
                {
                    int flightId = item.GetProperty("FlightId").GetInt32();
                    DateOnly travelDate = DateOnly.Parse(item.GetProperty("TravelDate").GetString());
                    string travelClass = item.GetProperty("TravelClass").GetString();
                    int? mealId = item.GetProperty("MealId").ValueKind == JsonValueKind.Null ? null : item.GetProperty("MealId").GetInt32();
                    decimal totalPrice = item.GetProperty("TotalPrice").GetDecimal();
                    string departureCity = item.GetProperty("DepartureCity").GetString();
                    string arrivalCity = item.GetProperty("ArrivalCity").GetString();

                    // create a booking item
                    
                    var booking = await _bookingService.CreateBookingAsync(
                        profile.ProfileId,
                        flightId,
                        travelDate,
                        totalPrice,
                        mealId,
                        travelClass
                    );

                    totalAmount += totalPrice;

                    // register in customerpref

                    var arrivalCityObj = await _cityService.GetCityByNameAsync(arrivalCity);
                    if (arrivalCityObj != null)
                    {
                        
                        await _customerPrefService.TrackCityVisitAsync(profile.ProfileId, arrivalCityObj.CityId);
                    }

                    // get meal

                    string mealName = "No meal selected";
                    if (mealId.HasValue)
                    {
                        var meal = await _mealService.GetMealByIdAsync(mealId.Value);
                        if (meal != null)
                        {
                            mealName = meal.Name;
                        }
                    }

                    // convert booking item into viewmodel

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

                    // Add to the list of viewmodels

                    bookings.Add(bookingDetail);

                    // add to mail

                    emailContent.AppendLine($"<tr><td>{flightId}</td><td>{travelDate.ToString("yyyy-MM-dd")}</td><td>{departureCity}</td><td>{arrivalCity}</td><td>{travelClass}</td><td>{booking.SeatNumber}</td><td>{mealName}</td><td>${totalPrice.ToString("0.00")}</td></tr>");
                }

                emailContent.AppendLine("</table>");
                emailContent.AppendLine("<p>Total amount: $" + totalAmount.ToString("0.00") + "</p>");
                emailContent.AppendLine("<p>We look forward to seeing you on board!</p>");
                emailContent.AppendLine("<p>Best regards,<br>Michael Scott Airways</p>");

                // send mail

                await _emailSend.SendEmailAsync(
                    profile.Email,
                    "Your Booking Confirmation - Michael Scott Airways",
                    emailContent.ToString()
                );

                // clear basket

                TempData.Remove("SimpleCart");
                TempData["CartItemCount"] = 0;

                // create confirmation viewmodel

                var confirmationViewModel = new BookingConfirmationVM
                {
                    BookingCount = bookings.Count,
                    CustomerName = $"{profile.FirstName} {profile.LastName}",
                    CustomerEmail = profile.Email,
                    TotalAmount = totalAmount
                };

                // serialise the confirmation viewmodel

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
                var confirmationJson = TempData["BookingConfirmation"].ToString(); // convert into the confirmation viewmodel
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