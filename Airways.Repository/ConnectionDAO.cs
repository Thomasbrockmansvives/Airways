using Airways.Domain.DataDB;
using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airways.Repository
{
    public class ConnectionDAO : IConnectionDAO
    {
        private readonly FlightBookingDBContext _context;

        public ConnectionDAO(FlightBookingDBContext context)
        {
            _context = context;
        }

        public async Task<Connection> GetConnectionByCitiesAsync(int departureCityId, int arrivalCityId)
        {
            return await _context.Connections
                .Include(c => c.Departure)
                .Include(c => c.Arrival)
                .Include(c => c.FlightNumber1Navigation)
                .Include(c => c.FlightNumber2Navigation)
                .Include(c => c.FlightNumber3Navigation)
                .FirstOrDefaultAsync(c => c.DepartureId == departureCityId && c.ArrivalId == arrivalCityId);
        }

        public async Task<IEnumerable<Connection>> GetAllConnectionsAsync()
        {
            return await _context.Connections
                .Include(c => c.Departure)
                .Include(c => c.Arrival)
                .Include(c => c.FlightNumber1Navigation)
                .Include(c => c.FlightNumber2Navigation)
                .Include(c => c.FlightNumber3Navigation)
                .ToListAsync();
        }
    }
}