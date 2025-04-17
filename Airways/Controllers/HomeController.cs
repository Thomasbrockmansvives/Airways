using Airways.Models;
using Airways.ViewModels;
using Airways.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Airways.Domain.DataDB;

namespace Airways.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICustomerProfileService _customerProfileService;
        private readonly ICustomerPrefService _customerPrefService;
        private readonly ICityService _cityService;
        private readonly IFlightService _flightService;
        private readonly FlightBookingDBContext _context;

        public HomeController(
    ILogger<HomeController> logger,
    UserManager<IdentityUser> userManager,
    ICustomerProfileService customerProfileService,
    ICustomerPrefService customerPrefService,
    ICityService cityService,
    IFlightService flightService,
    FlightBookingDBContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _customerProfileService = customerProfileService;
            _customerPrefService = customerPrefService;
            _cityService = cityService;
            _flightService = flightService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Prepare the banner data
            var bannerViewModel = await PrepareBannerViewModel();
            ViewBag.BannerData = bannerViewModel;

            return View();
        }

        private async Task<BannerVM> PrepareBannerViewModel()
        {
            var bannerVM = new BannerVM
            {
                IsLoggedIn = User.Identity.IsAuthenticated,
                HasPreferredCity = false
            };

            if (bannerVM.IsLoggedIn)
            {
                try
                {
                    // Get current user
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        // Step 1: Get profile by userId from Identity
                        var profile = await _customerProfileService.GetCustomerProfileByUserIdAsync(user.Id);
                        if (profile != null)
                        {
                            // Step 2: Get customer preferences using the profile ID
                            var customerPrefs = await _customerPrefService.GetCustomerPrefsByProfileIdAsync(profile.ProfileId);

                            if (customerPrefs != null && customerPrefs.Any())
                            {
                                // Step 3: Find the customer preference with highest visit count
                                var topPref = customerPrefs.OrderByDescending(p => p.VisitCount).FirstOrDefault();
                                if (topPref != null)
                                {
                                    // Step 4: Get city details based on preferred city ID
                                    var preferredCity = await _cityService.GetCityByIdAsync(topPref.CityId);
                                    if (preferredCity != null)
                                    {
                                        // Step 5: Find a line that has this city as arrival
                                        var lines = await _context.Lines
                                            .Where(l => l.ArrivalId == preferredCity.CityId)
                                            .ToListAsync();

                                        if (lines.Any())
                                        {
                                            // Pick a random line
                                            var random = new Random();
                                            var randomLine = lines[random.Next(lines.Count)];

                                            // Step 6: Get a flight for this line
                                            var flights = await _context.Flights
                                                .Where(f => f.FlightNumber == randomLine.FlightNumber)
                                                .ToListAsync();

                                            if (flights.Any())
                                            {
                                                // Pick a random flight
                                                var randomFlight = flights[random.Next(flights.Count)];

                                                // Step 7: Set the banner data
                                                bannerVM.HasPreferredCity = true;
                                                bannerVM.PreferredCityName = preferredCity.Name;
                                                bannerVM.FlightPrice = randomFlight.PriceEconomy;

                                                _logger.LogInformation($"Found personalized banner for user: City={preferredCity.Name}, Price={randomFlight.PriceEconomy}");
                                            }
                                            else
                                            {
                                                _logger.LogInformation($"No flights found for line: {randomLine.FlightNumber}");
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogInformation($"No lines found with arrival city: {preferredCity.Name}");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogInformation($"No customer preferences found for profile: {profile.ProfileId}");
                            }
                        }
                        else
                        {
                            _logger.LogInformation($"No profile found for user: {user.Id}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error preparing banner data");
                }
            }

            return bannerVM;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}