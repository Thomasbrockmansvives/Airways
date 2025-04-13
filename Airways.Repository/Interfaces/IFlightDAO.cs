using Airways.Domain.EntitiesDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Repository.Interfaces
{
    public interface IFlightDAO
    {
        Task<Flight> GetFlightByFlightNumberAndDateAsync(int flightNumber, DateOnly travelDate);
        Task<Flight> GetFlightByIdAsync(int flightId);
    }
}