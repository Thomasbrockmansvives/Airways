using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Airways.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            /* Get next available seat number for this flight and class, 
             * because we don't just give the next seat in the list.
             * sometimes, because another customer cancelled, a lower seatnumber became available
             */
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

        public async Task<bool> UpdateBookingAsync(int bookingId)
        {
            var booking = await _bookingDAO.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                return false;
            }

            // Set seat number to 0 to mark as cancelled
            booking.SeatNumber = 0;

            return await _bookingDAO.UpdateBookingAsync(booking);
        }
    }
}