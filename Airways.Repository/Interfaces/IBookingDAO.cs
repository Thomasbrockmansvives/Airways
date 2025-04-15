using Airways.Domain.EntitiesDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Repository.Interfaces
{
    public interface IBookingDAO
    {
        Task<Booking> CreateBookingAsync(int customerId, int flightId, DateOnly bookingDate, decimal totalPrice, int? mealId, string travelClass, int seatNumber);
        Task<int> GetNextAvailableSeatNumberAsync(int flightId, string travelClass);
        Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId);
        Task<Booking> GetBookingByIdAsync(int bookingId);

        Task<bool> UpdateBookingAsync(Booking booking);
    }
}