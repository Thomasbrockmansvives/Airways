// BookingService.cs
using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Airways.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingDAO _bookingDAO;

        public BookingService(IBookingDAO bookingDAO)
        {
            _bookingDAO = bookingDAO;
        }

        public async Task<Booking> CreateBookingAsync(int customerId, int flightId, DateOnly travelDate, decimal totalPrice, int? mealId, string travelClass)
        {
            // Get the next available seat number for this flight and class
            int seatNumber = await _bookingDAO.GetNextAvailableSeatNumberAsync(flightId, travelClass);

            // Create the booking
            return await _bookingDAO.CreateBookingAsync(
                customerId,
                flightId,
                travelDate,
                totalPrice,
                mealId,
                travelClass,
                seatNumber);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId)
        {
            return await _bookingDAO.GetBookingsByCustomerIdAsync(customerId);
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingDAO.GetBookingByIdAsync(bookingId);
        }
    }
}