using Airways.Domain.DataDB;
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

        public async Task<IEnumerable<Flight>> GetFlightsByFlightNumberAndDateRangeAsync(int flightNumber, DateOnly startDate, DateOnly endDate)
        {
            return await _context.Flights
                .Include(f => f.FlightNumberNavigation)
                    .ThenInclude(l => l.Departure)
                .Include(f => f.FlightNumberNavigation)
                    .ThenInclude(l => l.Arrival)
                .Where(f => f.FlightNumber == flightNumber &&
                       f.Date >= startDate &&
                       f.Date <= endDate)
                .OrderBy(f => f.Date)
                .ToListAsync();
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _context.Flights
                .Include(f => f.FlightNumberNavigation)
                .FirstOrDefaultAsync(f => f.FlightId == flightId);
        }
    }
}