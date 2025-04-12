using Airways.Domain.EntitiesDB;
using Airways.Repository.Interfaces;
using Airways.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightDAO _flightDAO;

        public FlightService(IFlightDAO flightDAO)
        {
            _flightDAO = flightDAO;
        }

        public async Task<IEnumerable<Flight>> GetFlightsByFlightNumberAndDateRangeAsync(int flightNumber, DateOnly startDate, DateOnly endDate)
        {
            return await _flightDAO.GetFlightsByFlightNumberAndDateRangeAsync(flightNumber, startDate, endDate);
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _flightDAO.GetFlightByIdAsync(flightId);
        }
    }
}