// IBookingService.cs
using Airways.Domain.EntitiesDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(int customerId, int flightId, DateOnly travelDate, decimal totalPrice, int? mealId, string travelClass);
        Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId);
        Task<Booking> GetBookingByIdAsync(int bookingId);
    }
}