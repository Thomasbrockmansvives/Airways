using Airways.Domain.EntitiesDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services.Interfaces
{
    public interface IFlightService
    {
        Task<Flight> GetFlightByFlightNumberAndDateAsync(int flightNumber, DateOnly travelDate);
        Task<Flight> GetFlightByIdAsync(int flightId);
        Task<bool> IsEconomyAvailableByFlightAsync(int flightId);
        Task<bool> IsBusinessAvailableByFlightAsync(int flightId);
    }
}