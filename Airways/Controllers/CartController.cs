using Airways.Infrastructure;
using Airways.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Airways.Controllers
{
    public class CartController : Controller
    {
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
    }
}