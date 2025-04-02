﻿using Airways.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Airways.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public BookingsController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Bookings()
        {
            return View();
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
