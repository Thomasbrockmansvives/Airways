using Airways.Domain.EntitiesDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Services.Interfaces
{
    public interface IFlightService
    {
        Task<IEnumerable<Flight>> GetFlightsByFlightNumberAndDateRangeAsync(int flightNumber, DateOnly startDate, DateOnly endDate);
        Task<Flight> GetFlightByIdAsync(int flightId);
    }
}