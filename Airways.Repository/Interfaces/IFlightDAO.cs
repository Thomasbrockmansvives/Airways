using Airways.Domain.EntitiesDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airways.Repository.Interfaces
{
    public interface IFlightDAO
    {
        Task<IEnumerable<Flight>> GetFlightsByFlightNumberAndDateRangeAsync(int flightNumber, DateOnly startDate, DateOnly endDate);
        Task<Flight> GetFlightByIdAsync(int flightId);
    }
}