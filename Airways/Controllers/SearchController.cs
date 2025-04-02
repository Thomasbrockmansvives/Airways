using Airways.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Airways.Controllers
{
    public class SearchController : Controller
    {
        private readonly ICityService _cityService;

        public SearchController(ICityService cityService)
        {
            _cityService = cityService;
        }

        public async Task<IActionResult> Search()
        {
            var cities = await _cityService.GetAllCitiesAsync();
            return View(cities);
        }
    }
}