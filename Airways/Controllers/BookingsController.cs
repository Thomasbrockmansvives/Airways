﻿using Airways.Domain.EntitiesDB;
using Airways.Services.Interfaces;
using Airways.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Add this using statement
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airways.Controllers
{
    [Authorize] // Ensures only logged-in users can access
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICustomerProfileService _customerProfileService;
        private readonly ICityService _cityService;
        private readonly ILogger<BookingsController> _logger; // Add this field

        public BookingsController(
            IBookingService bookingService,
            UserManager<IdentityUser> userManager,
            ICustomerProfileService customerProfileService,
            ICityService cityService,
            ILogger<BookingsController> logger) // Add logger to constructor
        {
            _bookingService = bookingService;
            _userManager = userManager;
            _customerProfileService = customerProfileService;
            _cityService = cityService;
            _logger = logger; // Initialize the logger
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("User not found when accessing bookings");
                    return RedirectToAction("Login", "Account");
                }

                _logger.LogInformation($"Found user with ID: {user.Id}");

                var profile = await _customerProfileService.GetCustomerProfileByUserIdAsync(user.Id);
                if (profile == null)
                {
                    _logger.LogWarning($"Customer profile not found for user ID: {user.Id}");
                    return RedirectToAction("Index", "Home");
                }

                _logger.LogInformation($"Found profile with ID: {profile.ProfileId}");

                var bookings = await _bookingService.GetBookingsByCustomerIdAsync(profile.ProfileId);
                _logger.LogInformation($"Found {bookings.Count()} bookings for profile {profile.ProfileId}");

                if (!bookings.Any())
                {
                    _logger.LogInformation("No bookings found for user");
                }

                var bookingViewModels = MapBookingsToViewModel(bookings);
                ViewData["ProfileId"] = profile.ProfileId;

                return View(bookingViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bookings");
                // You could add a temporary message for debugging
                ViewBag.ErrorMessage = ex.ToString();
                return View(new List<BookingVM>());
            }
        }

        // The rest of your controller methods remain the same
        // ...

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

        public async Task<IActionResult> LookForHotels(string city, string date)
        {
            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(date))
            {
                return PartialView("_HotelsResults", new HotelsVM
                {
                    City = city ?? "Unknown",
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                    Hotels = new List<HotelVM>()
                });
            }

            DateOnly flightDate;
            if (!DateOnly.TryParse(date, out flightDate))
            {
                flightDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            }

            // Mock response from a hotel API
            var hotels = new List<HotelVM>
            {
                new HotelVM
                {
                    Name = "Grand Hotel " + city,
                    Rating = 9.2m,
                    Price = 120.00m,
                    ImageUrl = "/img/hotel1.jpg"
                },
                new HotelVM
                {
                    Name = "City Plaza " + city,
                    Rating = 8.8m,
                    Price = 95.50m,
                    ImageUrl = "/img/hotel2.jpg"
                },
                new HotelVM
                {
                    Name = "Boutique Stays " + city,
                    Rating = 9.0m,
                    Price = 110.75m,
                    ImageUrl = "/img/hotel3.jpg"
                }
            };

            var hotelsViewModel = new HotelsVM
            {
                City = city,
                Date = flightDate,
                Hotels = hotels
            };

            return PartialView("_HotelsResults", hotelsViewModel);
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
                try
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
                catch (Exception ex)
                {
                    // Log any errors but continue processing other bookings
                    _logger.LogError(ex, $"Error mapping booking {booking.BookingId}");
                }
            }

            // Sort bookings by flight date and then by departure time
            return viewModels
                .OrderBy(b => b.FlightDate)
                .ThenBy(b => b.DepartureTime)
                .ToList();
        }
    }
}