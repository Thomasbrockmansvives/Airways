﻿using Airways.Domain.DataDB;
using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airways.Repository
{
    public class BookingDAO : IBookingDAO
    {
        private readonly FlightBookingDBContext _context;

        public BookingDAO(FlightBookingDBContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBookingAsync(int customerId, int flightId, DateOnly bookingDate, decimal totalPrice, int? mealId, string travelClass, int seatNumber)
        {
                        int newBookingId = await GenerateUniqueBookingIdAsync();

            var booking = new Booking
            {
                BookingId = newBookingId,
                CustomerId = customerId,
                FlightId = flightId,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                TotalPrice = totalPrice,
                MealId = mealId,
                Class = travelClass,
                SeatNumber = seatNumber
            };

            _context.Bookings.Add(booking);

            
            var flight = await _context.Flights.FindAsync(flightId);
            if (flight != null)
            {
                if (travelClass == "Economy")
                {
                    flight.UsedSeatsEconomy += 1;
                }
                else if (travelClass == "Business")
                {
                    flight.UsedSeatsBusiness += 1;
                }
            }

            await _context.SaveChangesAsync();
            return booking;
        }

        private async Task<int> GenerateUniqueBookingIdAsync()
        {
            int maxId = await _context.Bookings.MaxAsync(b => (int?)b.BookingId) ?? 0;
            return maxId + 1;
        }

        public async Task<int> GetNextAvailableSeatNumberAsync(int flightId, string travelClass)
        {
            
            var highestSeatNumber = await _context.Bookings
                .Where(b => b.FlightId == flightId && b.Class == travelClass)
                .OrderByDescending(b => b.SeatNumber)
                .Select(b => b.SeatNumber)
                .FirstOrDefaultAsync();

            
            return highestSeatNumber + 1;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId)
        {
            return await _context.Bookings
                .Include(b => b.Flight)
                    .ThenInclude(f => f.FlightNumberNavigation)
                        .ThenInclude(l => l.Departure)
                .Include(b => b.Flight)
                    .ThenInclude(f => f.FlightNumberNavigation)
                        .ThenInclude(l => l.Arrival)
                .Include(b => b.Meal)
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Flight)
                    .ThenInclude(f => f.FlightNumberNavigation)
                        .ThenInclude(l => l.Departure)
                .Include(b => b.Flight)
                    .ThenInclude(f => f.FlightNumberNavigation)
                        .ThenInclude(l => l.Arrival)
                .Include(b => b.Meal)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            try
            {
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating booking: {ex.Message}");
                return false;
            }
        }
    }
}