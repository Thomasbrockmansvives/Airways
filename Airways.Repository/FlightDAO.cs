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
    public class FlightDAO : IFlightDAO
    {
        private readonly FlightBookingDBContext _context;

        public FlightDAO(FlightBookingDBContext context)
        {
            _context = context;
        }

        public async Task<Flight> GetFlightByFlightNumberAndDateAsync(int flightNumber, DateOnly travelDate)
        {
            return await _context.Flights
                .Include(f => f.FlightNumberNavigation)
                    .ThenInclude(l => l.Departure) //lookup
                .Include(f => f.FlightNumberNavigation)
                    .ThenInclude(l => l.Arrival) //lookup
                .Where(f => f.FlightNumber == flightNumber &&
                       f.Date == travelDate)
                .OrderBy(f => f.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _context.Flights
        .Include(f => f.FlightNumberNavigation)
            .ThenInclude(l => l.Departure) //lookup
        .Include(f => f.FlightNumberNavigation)
            .ThenInclude(l => l.Arrival) //lookup
        .FirstOrDefaultAsync(f => f.FlightId == flightId);
        }

        public async Task<Boolean> IsEconomyAvailableByFlight(int flightId)
        {
            var flight = await _context.Flights
                .Include(f => f.FlightNumberNavigation)
                .FirstOrDefaultAsync(f => f.FlightId == flightId);

            if (flight == null)
                return false;

            
            return flight.UsedSeatsEconomy < flight.FlightNumberNavigation.TotalSeatsEconomy;
        }

        public async Task<Boolean> IsBusinessAvailableByFlight(int flightId)
        {
            var flight = await _context.Flights
                .Include(f => f.FlightNumberNavigation)
                .FirstOrDefaultAsync(f => f.FlightId == flightId);

            if (flight == null)
                return false;

           
            return flight.UsedSeatsBusiness < flight.FlightNumberNavigation.TotalSeatsBusiness;
        }

    }
}