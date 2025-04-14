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

        public async Task<Flight> GetFlightByFlightNumberAndDateAsync(int flightNumber, DateOnly travelDate)
        {
            return await _flightDAO.GetFlightByFlightNumberAndDateAsync(flightNumber, travelDate);
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _flightDAO.GetFlightByIdAsync(flightId);
        }

        public async Task<bool> IsEconomyAvailableByFlightAsync(int flightId)
        {
            return await _flightDAO.IsEconomyAvailableByFlight(flightId);
        }

        public async Task<bool> IsBusinessAvailableByFlightAsync(int flightId)
        {
            return await _flightDAO.IsBusinessAvailableByFlight(flightId);
        }
    }
}